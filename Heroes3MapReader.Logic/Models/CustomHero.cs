namespace Heroes3MapReader.Logic.Models;

/// <summary>
/// Custom hero definition
/// </summary>
public sealed class CustomHero
{
    /// <summary>
    /// Hero ID
    /// </summary>
    public int HeroID { get; set; }

    /// <summary>
    /// Portrait ID
    /// </summary>
    public int PortraitID { get; set; }

    /// <summary>
    /// Custom hero name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Which players can use this hero (bit mask)
    /// </summary>
    public byte AllowedPlayers { get; set; }
}
