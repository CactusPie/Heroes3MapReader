using System.Text;
using Heroes3MapReader.Logic.Features;
using Heroes3MapReader.Logic.Models;
using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.Logic;

/// <summary>
/// Reads Heroes 3 map files (.h3m) and returns MapInfo
/// </summary>
public sealed class MapReader : IMapReader
{
    private readonly Encoding _encoding;
    private readonly IStreamDecompressor _decompressor;
    private readonly IMapSpecificationRepository _mapSpecificationRepository;

    static MapReader()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    /// <summary>
    /// Initializes a new instance of the H3MReader class with a custom stream decompressor.
    /// </summary>
    /// <param name="decompressor">The stream decompressor to use for handling compressed map files</param>
    /// <param name="mapSpecificationRepository">The repository for map format specifications.</param>
    public MapReader(IStreamDecompressor decompressor, IMapSpecificationRepository mapSpecificationRepository)
    {
        _encoding = Encoding.GetEncoding("windows-1251");
        _decompressor = decompressor;
        _mapSpecificationRepository = mapSpecificationRepository;
    }

    /// <summary>
    /// Read a map from a file path
    /// </summary>
    public MapInfo ReadMap(string filePath)
    {
        using FileStream fileStream = File.OpenRead(filePath);
        return ReadMap(fileStream);
    }

    /// <summary>
    /// Read a map from a stream
    /// </summary>
    public MapInfo ReadMap(Stream stream)
    {
        // Check if the stream is gzip compressed
        Stream decompressedStream = _decompressor.DecompressIfNeeded(stream);

        using var reader = new BinaryReader(decompressedStream, _encoding, leaveOpen: true);

        try
        {
            return ParseMap(reader);
        }
        finally
        {
            if (decompressedStream != stream)
            {
                decompressedStream.Dispose();
            }
        }
    }

    private MapInfo ParseMap(BinaryReader reader)
    {
        var mapInfo = new MapInfo();

        // Read format
        uint formatValue = reader.ReadUInt32();
        MapFormat format = ParseMapFormat(formatValue);
        mapInfo.Format = format;

        // HotA extended format info
        MapSpecification mapSpecification;
        if (format == MapFormat.HotA)
        {
            mapSpecification = ReadHotaHeaderAndFeatures(reader, format);
        }
        else
        {
            mapSpecification = _mapSpecificationRepository.Get(format);
        }

        // Basic info
        reader.ReadByte(); // skip isPlayable - 0 if false, 1 if true
        uint size = reader.ReadUInt32();
        mapInfo.Width = (int)size;
        mapInfo.Height = (int)size;
        mapInfo.Size = ParseMapSize((int)size);
        mapInfo.HasUnderground = reader.ReadBoolean();
        mapInfo.Name = ReadString(reader);
        mapInfo.Description = ReadString(reader);
        mapInfo.Difficulty = (MapDifficulty)reader.ReadByte();

        // Max hero level (AB+)
        ReadMaxHeroLevel(reader, mapInfo, format);

        mapInfo.Players = ReadPlayers(reader, mapSpecification);
        mapInfo.PlayerCount = mapInfo.Players.Count(p => p.CanBeHuman || p.CanBeComputer);
        mapInfo.VictoryCondition = ReadVictoryCondition(reader, format, mapSpecification.VersionFlags.IsHotA8OrHigher);
        mapInfo.LossCondition = ReadLossCondition(reader);

        ReadTeams(reader, mapInfo);
        ReadHeroAvailability(reader, mapInfo, mapSpecification);

        // Placeholder heroes (AB+)
        ReadPlaceholderHeroes(reader, mapSpecification);

        // Custom heroes (SoD+)
        ReadCustomHeroes(reader, mapInfo, mapSpecification);

        // Reserved 31 bytes
        reader.ReadBytes(31);

        ReadHotaExtraHeader(reader, mapSpecification);

        // Unavailable artifacts (AB+)
        GetUnavailableArtifacts(reader, mapInfo, mapSpecification);

        // Unavailable spells and skills (SoD+)
        GetUnavailableSpellsAndSkills(reader, mapInfo, mapSpecification);

        ReadRumors(reader, mapInfo);

        // Hero settings (SoD+)
        ReadHeroSettings(reader, mapSpecification);

        ReadTerrainTiles(reader, mapInfo);

        return mapInfo;
    }

    private static void ReadMaxHeroLevel(BinaryReader reader, MapInfo mapInfo, MapFormat format)
    {
        if (format > MapFormat.RoE)
        {
            byte maxLevel = reader.ReadByte();
            mapInfo.MaxPlayerLevel = maxLevel == 0 ? 0 : maxLevel;
        }
    }

    private void ReadHeroSettings(BinaryReader reader, MapSpecification features)
    {
        if (!features.VersionFlags.IsSoDOrHigher)
        {
            return;
        }

        uint heroCount;

        if (features.VersionFlags.IsHotA0OrHigher)
        {
            heroCount = reader.ReadUInt32();
        }
        else
        {
            heroCount = features.VersionFlags.IsAbOrHigher ? 156U : 128U;
        }

        for (int i = 0; i < heroCount; i++)
        {
            bool isCustomHero = reader.ReadBoolean();
            if (isCustomHero)
            {
                SkipHeroSettings(reader, features);
            }
        }

        if (features.VersionFlags.IsHotA5OrHigher)
        {
            for (int i = 0; i < heroCount; i++)
            {
                reader.ReadBoolean(); // alwaysAddSkills - prevent heroes from receiving additional random secondary skills at the start of the map if they are not of the first level
                reader.ReadBoolean(); // cannotGainXP
                reader.ReadInt32(); // level - VCMI comment: needs investigation how this interacts with usual setting of level via experience
            }
        }
    }

    private static void GetUnavailableSpellsAndSkills(BinaryReader reader, MapInfo mapInfo, MapSpecification features)
    {
        if (features.VersionFlags.IsSoDOrHigher)
        {
            mapInfo.AvailableSpells = reader.ReadBytes(features.ByteSizes.SpellsBytes);
            mapInfo.AvailableSecondarySkills = reader.ReadBytes(features.ByteSizes.SkillsBytes);
        }
    }

    private static void GetUnavailableArtifacts(BinaryReader reader, MapInfo mapInfo, MapSpecification features)
    {
        if (!features.VersionFlags.IsAbOrHigher)
        {
            return;
        }

        int artifactBitfieldLength;
        if (features.VersionFlags.IsHotA0OrHigher)
        {
            int artifactsCount = reader.ReadInt32();
            artifactBitfieldLength = (artifactsCount + 7) / 8;
        }
        else
        {
            artifactBitfieldLength = features.ByteSizes.ArtifactsBytes;
        }

        mapInfo.AvailableArtifacts = reader.ReadBytes(artifactBitfieldLength);
    }

    private static void ReadHotaExtraHeader(BinaryReader reader, MapSpecification features)
    {
        if(features.VersionFlags.IsHotA0OrHigher)
        {
            reader.ReadBoolean(); // allowSpecialMonths, skipped
            reader.ReadBytes(3);
        }

        if(features.VersionFlags.IsHotA1OrHigher)
        {
            int combinedArtifactsCount = reader.ReadInt32();
            int combinedArtifactsBytes = (combinedArtifactsCount + 7) / 8;

            for (int i = 0; i < combinedArtifactsBytes; ++i)
            {
                reader.ReadByte(); // bitmask containing combined artifact data, skipped
            }
        }

        if(features.VersionFlags.IsHotA3OrHigher)
        {
            reader.ReadInt32(); // roundLimit, skipped
        }

        if(features.VersionFlags.IsHotA5OrHigher)
        {
            const int playerLimit = 8;
            for (int i = 0; i < playerLimit; ++i)
            {
                // VCMI docs: unconfirmed, but only remaining option according to changelog
                reader.ReadBoolean(); // heroRecruitmentBlocked, skipped
            }
        }
    }

    private void ReadRumors(BinaryReader reader, MapInfo mapInfo)
    {
        uint rumorCount = reader.ReadUInt32();
        mapInfo.Rumors = new List<string>();
        for (int i = 0; i < rumorCount; i++)
        {
            string rumorName = ReadString(reader);
            string rumorDescription = ReadString(reader);
            mapInfo.Rumors.Add($"{rumorName}: {rumorDescription}");
        }
    }

    private void ReadCustomHeroes(BinaryReader reader, MapInfo mapInfo, MapSpecification features)
    {
        if (features.VersionFlags.IsSoDOrHigher)
        {
            byte customHeroCount = reader.ReadByte();
            mapInfo.CustomHeroes = new List<CustomHero>();
            for (int i = 0; i < customHeroCount; i++)
            {
                mapInfo.CustomHeroes.Add(ReadCustomHero(reader));
            }
        }
    }

    private static void ReadPlaceholderHeroes(BinaryReader reader, MapSpecification features)
    {
        if (features.VersionFlags.IsAbOrHigher)
        {
            uint placeholderCount = reader.ReadUInt32();
            for (int i = 0; i < placeholderCount; i++)
            {
                reader.ReadByte(); // placeholder hero type id
            }
        }
    }

    private static void ReadTeams(BinaryReader reader, MapInfo mapInfo)
    {
        byte maxTeam = reader.ReadByte();
        mapInfo.TeamCount = maxTeam;
        if (maxTeam > 0)
        {
            for (int i = 0; i < 8; i++)
            {
                byte team = reader.ReadByte();
                if (i < mapInfo.Players.Count)
                {
                    mapInfo.Players[i].Team = team;
                }
            }
        }
    }

    private MapSpecification ReadHotaHeaderAndFeatures(BinaryReader reader, MapFormat mapFormat)
    {
        uint hotaVersion = reader.ReadUInt32();
        MapSpecification features = _mapSpecificationRepository.Get(mapFormat, hotaVersion);

  if(features.VersionFlags.IsHotA8OrHigher)
  {
            reader.ReadUInt32(); // major
            reader.ReadUInt32(); // minor
            reader.ReadUInt32(); // patch
        }

  if(features.VersionFlags.IsHotA1OrHigher)
  {
  	reader.ReadByte(); //isMirrorMap
  	reader.ReadByte(); //isArenaMap
  }

  if(features.VersionFlags.IsHotA2OrHigher)
  {
  	reader.ReadUInt32(); // terrainTypesCount
  }

  if(features.VersionFlags.IsHotA5OrHigher)
  {
            reader.ReadUInt32(); // townTypesCount
            reader.ReadSByte(); // allowedDifficultiesMask
  }

  if(features.VersionFlags.IsHotA7OrHigher)
  {
  	reader.ReadByte(); // canHireDefeatedHeroes
  }

        if(features.VersionFlags.IsHotA8OrHigher)
        {
            reader.ReadBoolean(); // forceMatchingVersion
        }

        return features;
    }

    private static MapFormat ParseMapFormat(uint value)
    {
        var mapFormat = (MapFormat)value;
        if (!Enum.IsDefined(typeof(MapFormat), mapFormat))
        {
            throw new FormatException($"Map format {value} is not supported");
        }

        return mapFormat;
    }

    private static MapSize ParseMapSize(int size)
    {
        return size switch
        {
            36 => MapSize.Small,
            72 => MapSize.Medium,
            108 => MapSize.Large,
            144 => MapSize.XLarge,
            180 => MapSize.Huge,
            216 => MapSize.XHuge,
            252 => MapSize.Giant,
            _ => MapSize.Medium, // Default for non-standard sizes
        };
    }

    private string ReadString(BinaryReader reader)
    {
        uint length = reader.ReadUInt32();
        if (length == 0)
        {
            return string.Empty;
        }

        if (length > 100000)
        {
            throw new InvalidDataException($"String length too large: {length}");
        }

        byte[] bytes = reader.ReadBytes((int)length);
        try
        {
            return _encoding.GetString(bytes);
        }
        catch
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }

    private List<PlayerInfo> ReadPlayers(BinaryReader reader, MapSpecification features)
    {
        var players = new List<PlayerInfo>();

        for (int i = 0; i < 8; i++)
        {
            var player = new PlayerInfo
            {
                Color = (PlayerColor)i,
                CanBeHuman = reader.ReadBoolean(),
                CanBeComputer = reader.ReadBoolean(),
            };

            bool canPlay = player.CanBeHuman || player.CanBeComputer;

            if(!canPlay)
            {
                if (features.VersionFlags.IsRoeOrHigher)
                {
                    reader.ReadBytes(6);
                }

                if (features.VersionFlags.IsAbOrHigher)
                {
                    reader.ReadBytes(6);
                }

                if (features.VersionFlags.IsSoDOrHigher)
                {
                    reader.ReadBytes(1);
                }

                continue;
            }

            player.AITactic = (AITactic)reader.ReadByte();

            // Customized towns (SoD+)
            if (features.VersionFlags.IsSoDOrHigher)
            {
                reader.ReadByte(); // faction is selectable
            }

            switch (features.ByteSizes.FactionsBytes)
            {
                // Town types bitfield (1 byte RoE, 2 bytes AB+)
                case 2:
                {
                    byte townTypes1 = reader.ReadByte();
                    byte townTypes2 = reader.ReadByte();
                    player.TownType = townTypes1 | (townTypes2 << 8);
                    break;
                }
                case 1:
                    player.TownType = reader.ReadByte();
                    break;
                default:
                    throw new InvalidOperationException("Unsupported faction byte size");
            }

            // Random town (only meaningful for playable players)
            player.HasRandomTown = reader.ReadBoolean();

            // Main town
            player.HasMainTown = reader.ReadBoolean();
            if (player.HasMainTown)
            {
                if (features.VersionFlags.IsAbOrHigher)
                {
                    player.GenerateHeroAtTheMainTown = reader.ReadBoolean();
                    reader.ReadByte(); // starting town type, unused
                }
                else
                {
                    player.GenerateHeroAtTheMainTown = true;
                }

                // Position of the main town
                reader.ReadByte(); // x
                reader.ReadByte(); // y
                reader.ReadByte(); // z
            }

            // Starting hero info
            player.HasRandomHero = reader.ReadBoolean();
            byte heroId = reader.ReadByte();

            if (heroId != 0xFF)
            {
                player.CustomHeroType = heroId;
                byte portraitId = reader.ReadByte();
                if (portraitId != 0xFF)
                {
                    player.CustomHeroPortrait = portraitId;
                }
                player.CustomHeroName = ReadString(reader);
            }

            // Additional player info (AB+) - read for ALL players
            if (features.VersionFlags.IsAbOrHigher)
            {
                reader.ReadByte(); // skip placeholder heroes count (max 8)
                uint heroCount = reader.ReadUInt32();
                for (int h = 0; h < heroCount; h++)
                {
                    reader.ReadByte(); // hero type
                    ReadString(reader); // hero name
                }
            }

            players.Add(player);
        }

        return players;
    }

    private VictoryCondition ReadVictoryCondition(BinaryReader reader, MapFormat format, bool isHotaFactory)
    {
        var type = (VictoryConditionType)reader.ReadByte();
        if (type == VictoryConditionType.Standard)
        {
            return new VictoryCondition
            {
                Type = VictoryConditionType.Standard,
            };
        }

        var condition = new VictoryCondition
        {
            Type = type,
            AlsoDefeatAllEnemies = reader.ReadBoolean(),
            AppliesToAI = reader.ReadBoolean(),
        };

        uint artifactSize;
        if (format == MapFormat.RoE)
        {
            artifactSize = 1;
        }
        else if (isHotaFactory)
        {
            artifactSize = 4;
        }
        else
        {
            artifactSize = 2;
        }

        switch (type)
        {
            case VictoryConditionType.AcquireArtifact:
                condition.ArtifactID = ReadBytesAsUint32(reader, Clamp(artifactSize, 1, 2));
                break;

            case VictoryConditionType.AccumulateCreatures:
                condition.CreatureID = ReadBytesAsUint32(reader, Clamp(artifactSize, 1, 2));
                condition.CreatureCount = reader.ReadUInt32();
                break;

            case VictoryConditionType.AccumulateResources:
                condition.ResourceType = reader.ReadByte();
                condition.ResourceAmount = reader.ReadUInt32();
                break;

            case VictoryConditionType.UpgradeSpecificTown:
                condition.TownX = reader.ReadByte();
                condition.TownY = reader.ReadByte();
                condition.TownZ = reader.ReadByte();
                condition.HallLevel = reader.ReadByte();
                condition.CastleLevel = reader.ReadByte();
                break;

            case VictoryConditionType.BuildGrailStructure:
            case VictoryConditionType.DefeatSpecificHero:
            case VictoryConditionType.CaptureTown:
            case VictoryConditionType.DefeatSpecificMonster:
                condition.TownX = reader.ReadByte();
                condition.TownY = reader.ReadByte();
                condition.TownZ = reader.ReadByte();
                break;

            case VictoryConditionType.FlagAllCreatureDwellings:
            case VictoryConditionType.FlagAllMines:
            case VictoryConditionType.DefeatAll:
                // No additional data
                break;

            case VictoryConditionType.SurviveTime:
                condition.Days = reader.ReadInt32();
                break;

            case VictoryConditionType.TransportArtifact:
                condition.ArtifactID = reader.ReadByte();
                condition.TownX = reader.ReadByte();
                condition.TownY = reader.ReadByte();
                condition.TownZ = reader.ReadByte();
                break;
        }

        return condition;
    }

    private static LossCondition ReadLossCondition(BinaryReader reader)
    {
        var type = (LossConditionType)reader.ReadByte();
        if (type == LossConditionType.Standard)
        {
            return new LossCondition
            {
                Type = LossConditionType.Standard,
            };
        }

        var condition = new LossCondition
        {
            Type = type,
        };

        switch (type)
        {
            case LossConditionType.LoseSpecificTown:
            case LossConditionType.LoseSpecificHero:
                condition.TownX = reader.ReadByte();
                condition.TownY = reader.ReadByte();
                condition.TownZ = reader.ReadByte();
                break;

            case LossConditionType.TimeExpires:
                condition.Days = reader.ReadUInt16();
                break;
        }

        return condition;
    }

    private static void ReadHeroAvailability(BinaryReader reader, MapInfo mapInfo, MapSpecification features)
    {
        int heroCount;
        int bitfieldLength;
        if (features.VersionFlags.IsHotA0OrHigher)
        {
            heroCount = (int)reader.ReadUInt32();
            bitfieldLength = (heroCount + 7) / 8;
        }
        else
        {
            heroCount = features.Counts.HeroesCount;
            bitfieldLength = features.ByteSizes.HeroesBytes;
        }

        byte[] heroAvailability = reader.ReadBytes(bitfieldLength);

        mapInfo.AvailableHeroes = new List<string>();
        for (int i = 0; i < heroCount; i++)
        {
            int byteIndex = i / 8;
            int bitIndex = i % 8;
            if ((heroAvailability[byteIndex] & (1 << bitIndex)) != 0)
            {
                mapInfo.AvailableHeroes.Add($"Hero_{i}");
            }
        }
    }

    private CustomHero ReadCustomHero(BinaryReader reader)
    {
        var hero = new CustomHero
        {
            HeroID = reader.ReadByte(),
            PortraitID = reader.ReadByte(),
        };

        if (hero.PortraitID == 0xFF)
        {
            hero.PortraitID = -1; // Default
        }

        hero.Name = ReadString(reader);
        hero.AllowedPlayers = reader.ReadByte(); // contains a bitmask for allowed players

        return hero;
    }

    private void SkipHeroSettings(BinaryReader reader, MapSpecification features)
    {
        // Experience
        bool hasExperience = reader.ReadBoolean();
        if (hasExperience)
        {
            reader.ReadUInt32();
        }

        // Secondary skills
        bool hasSkills = reader.ReadBoolean();
        if (hasSkills)
        {
            uint skillCount = reader.ReadUInt32();
            for (int i = 0; i < skillCount; i++)
            {
                reader.ReadByte(); // type
                reader.ReadByte(); // level
            }
        }

        // Artifacts
        bool hasArtifacts = reader.ReadBoolean();
        if (hasArtifacts)
        {
            // Equipment slots
            for (int i = 0; i < features.Counts.ArtifactSlotsCount; i++)
            {
                ReadArtifacts();
            }

            // Backpack
            ushort backpackCount = reader.ReadUInt16();
            for (int i = 0; i < backpackCount; i++)
            {
                ReadArtifacts();
            }
        }

        // Biography
        bool hasBiography = reader.ReadBoolean();
        if (hasBiography)
        {
            ReadString(reader);
        }

        // Gender
        reader.ReadByte();

        // Spells
        bool hasSpells = reader.ReadBoolean();
        if (hasSpells)
        {
            reader.ReadBytes(features.ByteSizes.SpellsBytes);
        }

        // Primary skills
        bool hasPrimarySkills = reader.ReadBoolean();
        if (hasPrimarySkills)
        {
            reader.ReadBytes(4);
        }

        void ReadArtifacts()
        {
            // Artifact id
            if(features.VersionFlags.IsAbOrHigher)
            {
                reader.ReadUInt16();
            }
            else
            {
                reader.ReadByte();
            }

            if (features.VersionFlags.IsHotA5OrHigher)
            {
                reader.ReadInt16(); // scroll spell
            }
        }
    }

    private void ReadTerrainTiles(BinaryReader reader, MapInfo mapInfo)
    {
        int width = mapInfo.Width;
        int height = mapInfo.Height;

        mapInfo.SurfaceTerrain = new TerrainTile[width, height];
        if (mapInfo.HasUnderground)
        {
            mapInfo.UndergroundTerrain = new TerrainTile[width, height];
        }

        // Surface level
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                mapInfo.SurfaceTerrain[x, y] = ReadTerrainTile(reader);
            }
        }

        // Underground level
        if (mapInfo.HasUnderground)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mapInfo.UndergroundTerrain![x, y] = ReadTerrainTile(reader);
                }
            }
        }
    }

    private static uint Clamp(uint value, uint min, uint max)
    {
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }

    private static uint ReadBytesAsUint32(BinaryReader reader, uint bytes)
    {
        if (bytes == 1)
        {
            return reader.ReadByte();
        }

        if (bytes == 2)
        {
            return reader.ReadUInt16();
        }

        if (bytes == 4)
        {
            return reader.ReadUInt32();
        }

        throw new ArgumentOutOfRangeException($"Unsupported byte read count: {bytes}");
    }

    private static TerrainTile ReadTerrainTile(BinaryReader reader)
    {
        return new TerrainTile
        {
            TerrainType = (TerrainType)reader.ReadByte(),
            TerrainSprite = reader.ReadByte(),
            RiverType = (RiverType)reader.ReadByte(),
            RiverSprite = reader.ReadByte(),
            RoadType = (RoadType)reader.ReadByte(),
            RoadSprite = reader.ReadByte(),
            Flags = reader.ReadByte(),
        };
    }
}
