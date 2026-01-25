namespace Heroes3MapReader.Logic.Models.Enums;

/// <summary>
/// Loss condition types
/// </summary>
public enum LossConditionType
{
    /// <summary>Loss by losing a specific town</summary>
    LoseSpecificTown = 0,
    /// <summary>Loss by losing a specific hero</summary>
    LoseSpecificHero = 1,
    /// <summary>Loss by time expiring</summary>
    TimeExpires = 2,
    /// <summary>Standard loss condition</summary>
    Standard = 255,
}