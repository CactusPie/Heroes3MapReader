using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.Logic.MapSpecificationLogic;

/// <summary>
/// Repository for Heroes 3 map format features with lazy loading
/// Based on VCMI's MapFeaturesH3M.cpp implementation
/// </summary>
public sealed class MapSpecificationRepository : IMapSpecificationRepository
{
    // Lazy-loaded feature instances
    private readonly Lazy<MapSpecification> _roeFeatures;
    private readonly Lazy<MapSpecification> _abFeatures;
    private readonly Lazy<MapSpecification> _sodFeatures;
    private readonly Lazy<MapSpecification> _chrFeatures;
    private readonly Lazy<MapSpecification> _wogFeatures;

    // HotA versions are cached dynamically since they have a parameter
    private readonly Dictionary<uint, MapSpecification> _hotaFeaturesCache = new();
    private readonly object _hotaCacheLock = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MapSpecificationRepository"/> class,
    /// setting up lazy loading for all supported map format specifications.
    /// </summary>
    public MapSpecificationRepository()
    {
        _roeFeatures = new Lazy<MapSpecification>(CreateSpecificationRoe);
        _abFeatures = new Lazy<MapSpecification>(CreateSpecificationAb);
        _sodFeatures = new Lazy<MapSpecification>(CreateSpecificationSod);
        _chrFeatures = new Lazy<MapSpecification>(CreateSpecificationHeroesChronicles);
        _wogFeatures = new Lazy<MapSpecification>(CreateFeaturesWog);
    }

    /// <summary>
    /// Gets the appropriate map format features based on the format and optional HotA version
    /// </summary>
    public MapSpecification Get(MapFormat format, uint hotaVersionMajor = 0)
    {
        return format switch
        {
            MapFormat.RoE => _roeFeatures.Value,
            MapFormat.AB => _abFeatures.Value,
            MapFormat.SoD => _sodFeatures.Value,
            MapFormat.CHR => _chrFeatures.Value,
            MapFormat.WoG => _wogFeatures.Value,
            MapFormat.HotA => GetOrCreateHotAFeatures(hotaVersionMajor),
            _ => throw new ArgumentException("Invalid map format!"),
        };
    }

    /// <summary>
    /// Gets or creates HotA features with thread-safe caching
    /// </summary>
    private MapSpecification GetOrCreateHotAFeatures(uint hotaVersionMajor)
    {
        if (_hotaFeaturesCache.TryGetValue(hotaVersionMajor, out MapSpecification? cached))
        {
            return cached;
        }

        lock (_hotaCacheLock)
        {
            if (_hotaFeaturesCache.TryGetValue(hotaVersionMajor, out cached))
            {
                return cached;
            }

            MapSpecification features = CreateSpecificationHota(hotaVersionMajor);
            _hotaFeaturesCache[hotaVersionMajor] = features;
            return features;
        }
    }

    /// <summary>
    /// Creates the features for the original Heroes 3: The Restoration of Erathia (ROE) format
    /// </summary>
    private MapSpecification CreateSpecificationRoe()
    {
        return new MapSpecification
        {
            VersionFlags = new MapVersionFlags(
                IsRoeOrHigher: true,
                IsAbOrHigher: false,
                IsSoDOrHigher: false,
                IsChroniclesOrHigher: false,
                IsWoGOrHigher: false,
                IsHotA0OrHigher: false,
                IsHotA1OrHigher: false,
                IsHotA2OrHigher: false,
                IsHotA3OrHigher: false,
                IsHotA5OrHigher: false,
                IsHotA6OrHigher: false,
                IsHotA7OrHigher: false,
                IsHotA8OrHigher: false,
                IsHotA9OrHigher: false
            ),

            ByteSizes = new MapByteSizes(
                FactionsBytes: 1,
                HeroesBytes: 16,
                ArtifactsBytes: 16,
                SkillsBytes: 4,
                ResourcesBytes: 4,
                SpellsBytes: 9,
                BuildingsBytes: 6
            ),

            Counts = new MapCounts(
                FactionsCount: 8,
                HeroesCount: 128,
                HeroesPortraitsCount: 130, // +General Kendal, +Catherine (portrait-only in RoE)
                ArtifactsCount: 127,
                ResourcesCount: 7,
                CreaturesCount: 118,
                SpellsCount: 70,
                SkillsCount: 28,
                TerrainsCount: 10,
                ArtifactSlotsCount: 18,
                BuildingsCount: 41,
                RoadsCount: 3,
                RiversCount: 4
            ),

            InvalidIdentifiers = new MapInvalidIdentifiers(
                HeroIdentifierInvalid: 0xff,
                ArtifactIdentifierInvalid: 0xff,
                CreatureIdentifierInvalid: 0xff,
                SpellIdentifierInvalid: 0xff
            ),
        };
    }

    /// <summary>
    /// Creates the features for the Armageddon's Blade (AB) format
    /// </summary>
    private MapSpecification CreateSpecificationAb()
    {
        MapSpecification baseFeatures = _roeFeatures.Value;
        return new MapSpecification
        {
            VersionFlags = baseFeatures.VersionFlags with
            {
                IsAbOrHigher = true,
            },

            ByteSizes = baseFeatures.ByteSizes with
            {
                FactionsBytes = 2, // + Conflux
                HeroesBytes = 20,
                ArtifactsBytes = 17,
            },

            Counts = baseFeatures.Counts with
            {
                FactionsCount = 9,
                CreaturesCount = 145, // + Conflux and new neutrals
                HeroesCount = 156, // + Conflux and campaign heroes
                HeroesPortraitsCount = 159, // +Kendal, +young Cristian, +Ordwald
                ArtifactsCount = 129, // + Armaggedon Blade and Vial of Dragon Blood
            },

            InvalidIdentifiers = baseFeatures.InvalidIdentifiers with
            {
                ArtifactIdentifierInvalid = 0xffff, // Now uses 2 bytes / object
                CreatureIdentifierInvalid = 0xffff, // Now uses 2 bytes / object
            },
        };
    }

    /// <summary>
    /// Creates the features for the Shadow of Death (SOD) format
    /// </summary>
    private MapSpecification CreateSpecificationSod()
    {
        MapSpecification baseFeatures = _abFeatures.Value;
        return new MapSpecification
        {
            VersionFlags = baseFeatures.VersionFlags with
            {
                IsSoDOrHigher = true,
            },

            ByteSizes = baseFeatures.ByteSizes with
            {
                ArtifactsBytes = 18,
            },

            Counts = baseFeatures.Counts with
            {
                HeroesPortraitsCount = 163, // +Finneas +young Gem +young Sandro +young Yog
                ArtifactsCount = 144, // + Combined artifacts + 3 unfinished artifacts (required for some maps)
                ArtifactSlotsCount = 19, // + MISC_5 slot
            },

            InvalidIdentifiers = baseFeatures.InvalidIdentifiers,
        };
    }

    /// <summary>
    /// Creates the features for the Chronicles (CHR) format
    /// </summary>
    private MapSpecification CreateSpecificationHeroesChronicles()
    {
        MapSpecification baseFeatures = _sodFeatures.Value;
        return new MapSpecification
        {
            VersionFlags = baseFeatures.VersionFlags with
            {
                IsChroniclesOrHigher = true,
            },

            ByteSizes = baseFeatures.ByteSizes,

            Counts = baseFeatures.Counts with
            {
                HeroesPortraitsCount = 169, // +6x tarnum
            },

            InvalidIdentifiers = baseFeatures.InvalidIdentifiers,
        };
    }

    /// <summary>
    /// Creates the features for the Wake of Gods (WOG) format
    /// </summary>
    private MapSpecification CreateFeaturesWog()
    {
        MapSpecification baseFeatures = _sodFeatures.Value;

        return new MapSpecification
        {
            VersionFlags = baseFeatures.VersionFlags with
            {
                IsChroniclesOrHigher = false,
                IsWoGOrHigher = true,
            },

            ByteSizes = baseFeatures.ByteSizes,

            Counts = baseFeatures.Counts,

            InvalidIdentifiers = baseFeatures.InvalidIdentifiers,
        };
    }

    /// <summary>
    /// Creates the features for the Horn of the Abyss (HotA) format
    /// </summary>
    /// <param name="hotaVersion">The HotA version number (supports versions 0-8)</param>
    /// <returns>The features for the specified HotA version</returns>
    private MapSpecification CreateSpecificationHota(uint hotaVersion)
    {
        MapSpecification baseFeatures = _sodFeatures.Value;

        int artifactsBytes = 21;
        int heroesBytes = 23;
        int artifactsCount = 163;
        int heroesCount = 178;
        int heroesPortraitsCount = 186;
        int factionsCount = 10;
        int creaturesCount = 171;

        if (hotaVersion >= 3)
        {
            artifactsCount = 165; // + HotA artifacts
            heroesCount = 179; // + Giselle
            heroesPortraitsCount = 188; // + campaign portrait + Giselle
        }

        if (hotaVersion >= 5)
        {
            factionsCount = 11; // + Factory
            creaturesCount = 186; // + 16 Factory
            artifactsCount = 166; // +pendant of reflection, +sleepkeeper
            heroesCount = 198; // + 16 Factory, +3 campaign
            heroesPortraitsCount = 228; // + 16 Factory, +A LOT campaign
            heroesBytes = 25;
        }

        var result = new MapSpecification
        {
            VersionFlags = baseFeatures.VersionFlags with
            {
                IsHotA0OrHigher = true,
                IsHotA1OrHigher = hotaVersion > 0,
                IsHotA2OrHigher = hotaVersion > 1,
                IsHotA3OrHigher = hotaVersion > 2,
                IsHotA5OrHigher = hotaVersion > 4,
                IsHotA6OrHigher = hotaVersion > 5,
                IsHotA7OrHigher = hotaVersion > 6,
                IsHotA8OrHigher = hotaVersion > 7,
                IsHotA9OrHigher = hotaVersion > 8,
            },

            ByteSizes = baseFeatures.ByteSizes with
            {
                HeroesBytes = heroesBytes,
                ArtifactsBytes = artifactsBytes,
            },

            Counts = baseFeatures.Counts with
            {
                TerrainsCount = 12, // +Highlands +Wasteland
                SkillsCount = 29, // + Interference
                FactionsCount = factionsCount,
                CreaturesCount = creaturesCount,
                ArtifactsCount = artifactsCount,
                HeroesCount = heroesCount,
                HeroesPortraitsCount = heroesPortraitsCount,
            },

            InvalidIdentifiers = baseFeatures.InvalidIdentifiers,
        };

        return result;
    }
}