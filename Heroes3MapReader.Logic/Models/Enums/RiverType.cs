namespace Heroes3MapReader.Logic.Models.Enums;

/// <summary>
/// River types
/// </summary>
public enum RiverType : byte
{
    /// <summary>No river on this tile</summary>
    None = 0,
    /// <summary>Clear river type</summary>
    Clear = 1,
    /// <summary>Icy river type</summary>
    Icy = 2,
    /// <summary>Muddy river type</summary>
    Muddy = 3,
    /// <summary>Lava river type</summary>
    Lava = 4,
}