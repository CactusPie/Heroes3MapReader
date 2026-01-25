using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.Logic.Models;

/// <summary>
/// Represents a single tile on the map
/// </summary>
public sealed class TerrainTile
{
    /// <summary>
    /// Terrain type of this tile
    /// </summary>
    public TerrainType TerrainType { get; set; }

    /// <summary>
    /// Terrain sprite index
    /// </summary>
    public byte TerrainSprite { get; set; }

    /// <summary>
    /// River type on this tile
    /// </summary>
    public RiverType RiverType { get; set; }

    /// <summary>
    /// River sprite index
    /// </summary>
    public byte RiverSprite { get; set; }

    /// <summary>
    /// Road type on this tile
    /// </summary>
    public RoadType RoadType { get; set; }

    /// <summary>
    /// Road sprite index
    /// </summary>
    public byte RoadSprite { get; set; }

    /// <summary>
    /// Tile flags (mirroring, etc.)
    /// </summary>
    public byte Flags { get; set; }
}
