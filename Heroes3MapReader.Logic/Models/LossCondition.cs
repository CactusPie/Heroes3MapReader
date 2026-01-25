using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.Logic.Models;

/// <summary>
/// Loss condition configuration
/// </summary>
public sealed class LossCondition
{
    /// <summary>
    /// Type of loss condition
    /// </summary>
    public LossConditionType Type { get; set; }

    /// <summary>
    /// Town coordinates X (for LoseSpecificTown)
    /// </summary>
    public int TownX { get; set; }

    /// <summary>
    /// Town coordinates Y (for LoseSpecificTown)
    /// </summary>
    public int TownY { get; set; }

    /// <summary>
    /// Town coordinates Z (for LoseSpecificTown)
    /// </summary>
    public int TownZ { get; set; }

    /// <summary>
    /// Days until loss (for TimeExpires)
    /// </summary>
    public int Days { get; set; }
}