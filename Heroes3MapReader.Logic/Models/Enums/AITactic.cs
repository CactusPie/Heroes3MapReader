namespace Heroes3MapReader.Logic.Models.Enums;

/// <summary>
/// AI tactic settings
/// </summary>
public enum AITactic
{
    /// <summary>Random AI tactic</summary>
    Random = 0,
    /// <summary>Warrior AI tactic - focuses on combat</summary>
    Warrior = 1,
    /// <summary>Builder AI tactic - focuses on town development</summary>
    Builder = 2,
    /// <summary>Explorer AI tactic - focuses on map exploration</summary>
    Explorer = 3,
}