using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.Logic.Models;

/// <summary>
/// Information about a player slot
/// </summary>
public sealed class PlayerInfo
{
    /// <summary>
    /// Player color/ID
    /// </summary>
    public PlayerColor Color { get; set; }

    /// <summary>
    /// Whether this player can be human-controlled
    /// </summary>
    public bool CanBeHuman { get; set; }

    /// <summary>
    /// Whether this player can be AI-controlled
    /// </summary>
    public bool CanBeComputer { get; set; }

    /// <summary>
    /// AI tactic for this player
    /// </summary>
    public AITactic AITactic { get; set; }

    /// <summary>
    /// Whether this player has a random town
    /// </summary>
    public bool HasRandomTown { get; set; }

    /// <summary>
    /// Town type ID (if not random)
    /// </summary>
    public int TownType { get; set; }

    /// <summary>
    /// Whether the main town is generated
    /// </summary>
    public bool HasMainTown { get; set; }

    /// <summary>
    /// Whether the player generates a random hero
    /// </summary>
    public bool GenerateHeroAtTheMainTown { get; set; }

    /// <summary>
    /// Whether the player has a random hero assigned
    /// </summary>
    public bool HasRandomHero { get; set; }

    /// <summary>
    /// Custom hero type ID (if specified)
    /// </summary>
    public int CustomHeroType { get; set; }

    /// <summary>
    /// Custom hero portrait ID
    /// </summary>
    public int CustomHeroPortrait { get; set; }

    /// <summary>
    /// Custom hero name
    /// </summary>
    public string CustomHeroName { get; set; } = string.Empty;

    /// <summary>
    /// Team number (0 = no team)
    /// </summary>
    public int Team { get; set; }
}
