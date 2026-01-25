using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.Logic.Models;

/// <summary>
/// Represents the complete information about a Heroes 3 map
/// </summary>
public sealed class MapInfo
{
    /// <summary>
    /// Map format version (ROE, AB, SOD, etc.)
    /// </summary>
    public MapFormat Format { get; set; }

    /// <summary>
    /// Name of the map
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the map
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Map difficulty level
    /// </summary>
    public MapDifficulty Difficulty { get; set; }

    /// <summary>
    /// Maximum player level (0 = unlimited)
    /// </summary>
    public int MaxPlayerLevel { get; set; }

    /// <summary>
    /// Map dimensions
    /// </summary>
    public MapSize Size { get; set; }

    /// <summary>
    /// Width of the map in tiles
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Height of the map in tiles
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Whether the map has two levels (underground)
    /// </summary>
    public bool HasUnderground { get; set; }

    /// <summary>
    /// Number of players configured for this map
    /// </summary>
    public int PlayerCount { get; set; }

    /// <summary>
    /// Information about each player
    /// </summary>
    public List<PlayerInfo> Players { get; set; } = new();

    /// <summary>
    /// Victory conditions
    /// </summary>
    public VictoryCondition? VictoryCondition { get; set; }

    /// <summary>
    /// Loss conditions
    /// </summary>
    public LossCondition? LossCondition { get; set; }

    /// <summary>
    /// Number of teams
    /// </summary>
    public int TeamCount { get; set; }

    /// <summary>
    /// Available heroes on the map
    /// </summary>
    public List<string> AvailableHeroes { get; set; } = new();

    /// <summary>
    /// Custom hero data if present
    /// </summary>
    public List<CustomHero> CustomHeroes { get; set; } = new();

    /// <summary>
    /// Available artifacts
    /// </summary>
    public byte[]? AvailableArtifacts { get; set; }

    /// <summary>
    /// Available spells
    /// </summary>
    public byte[]? AvailableSpells { get; set; }

    /// <summary>
    /// Available secondary skills
    /// </summary>
    public byte[]? AvailableSecondarySkills { get; set; }

    /// <summary>
    /// Rumors/gossip on the map
    /// </summary>
    public List<string> Rumors { get; set; } = new();

    /// <summary>
    /// Terrain data for the surface level (Width x Height)
    /// </summary>
    public TerrainTile[,]? SurfaceTerrain { get; set; }

    /// <summary>
    /// Terrain data for the underground level (Width x Height), if HasUnderground is true
    /// </summary>
    public TerrainTile[,]? UndergroundTerrain { get; set; }
}
