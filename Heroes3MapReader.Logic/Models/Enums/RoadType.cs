namespace Heroes3MapReader.Logic.Models.Enums;

/// <summary>
/// Road types
/// </summary>
public enum RoadType : byte
{
    /// <summary>No road on this tile</summary>
    None = 0,
    /// <summary>Dirt road type</summary>
    Dirt = 1,
    /// <summary>Gravel road type</summary>
    Gravel = 2,
    /// <summary>Cobblestone road type</summary>
    Cobblestone = 3,
}
