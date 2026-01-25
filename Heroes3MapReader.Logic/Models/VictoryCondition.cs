namespace Heroes3MapReader.Logic.Models;

/// <summary>
/// Victory condition configuration
/// </summary>
public sealed class VictoryCondition
{
    /// <summary>
    /// Type of victory condition
    /// </summary>
    public VictoryConditionType Type { get; set; }

    /// <summary>
    /// Whether defeating all enemies is also required
    /// </summary>
    public bool AlsoDefeatAllEnemies { get; set; }

    /// <summary>
    /// Whether AI can achieve this victory
    /// </summary>
    public bool AppliesToAI { get; set; }

    /// <summary>
    /// Artifact ID (for AcquireArtifact)
    /// </summary>
    public uint ArtifactID { get; set; }

    /// <summary>
    /// Creature ID (for AccumulateCreatures)
    /// </summary>
    public uint CreatureID { get; set; }

    /// <summary>
    /// Creature count (for AccumulateCreatures)
    /// </summary>
    public uint CreatureCount { get; set; }

    /// <summary>
    /// Resource type (for AccumulateResources)
    /// </summary>
    public int ResourceType { get; set; }

    /// <summary>
    /// Resource amount (for AccumulateResources)
    /// </summary>
    public uint ResourceAmount { get; set; }

    /// <summary>
    /// Town coordinates X (for UpgradeSpecificTown, CaptureTown)
    /// </summary>
    public int TownX { get; set; }

    /// <summary>
    /// Town coordinates Y (for UpgradeSpecificTown, CaptureTown)
    /// </summary>
    public int TownY { get; set; }

    /// <summary>
    /// Town coordinates Z (for UpgradeSpecificTown, CaptureTown)
    /// </summary>
    public int TownZ { get; set; }

    /// <summary>
    /// Hall level (for UpgradeSpecificTown)
    /// </summary>
    public int HallLevel { get; set; }

    /// <summary>
    /// Castle level (for UpgradeSpecificTown)
    /// </summary>
    public int CastleLevel { get; set; }

    /// <summary>
    /// Number of days the player must survive (used for SurviveTime victory condition).
    /// </summary>
    public int Days { get; set; }
}