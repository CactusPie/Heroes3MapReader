namespace Heroes3MapReader.Logic.Models;

/// <summary>
/// Terrain types in Heroes 3
/// </summary>
public enum TerrainType : byte
{
    /// <summary>Dirt terrain type</summary>
    Dirt = 0,
    /// <summary>Sand terrain type</summary>
    Sand = 1,
    /// <summary>Grass terrain type</summary>
    Grass = 2,
    /// <summary>Snow terrain type</summary>
    Snow = 3,
    /// <summary>Swamp terrain type</summary>
    Swamp = 4,
    /// <summary>Rough terrain type</summary>
    Rough = 5,
    /// <summary>Subterranean terrain type</summary>
    Subterranean = 6,
    /// <summary>Lava terrain type</summary>
    Lava = 7,
    /// <summary>Water terrain type</summary>
    Water = 8,
    /// <summary>Rock terrain type</summary>
    Rock = 9,
}