namespace Heroes3MapReader.Logic.Features;

/// <summary>
/// Contains the version flags for different Heroes 3 map formats
/// </summary>
/// <param name="IsRoeOrHigher">A value indicating whether the map uses the original RoE (Restoration of Erathia) format or higher</param>
/// <param name="IsAbOrHigher">A value indicating whether the map uses the AB (Armageddon's Blade) expansion format or higher</param>
/// <param name="IsSoDOrHigher">A value indicating whether the map uses the SoD (Shadow of Death) expansion format or higher</param>
/// <param name="IsChroniclesOrHigher">A value indicating whether the map uses the Heroes Chronicles format or higher</param>
/// <param name="IsWoGOrHigher">A value indicating whether the map uses the WoG (In the Wake of Gods) format or higher</param>
/// <param name="IsHotA0OrHigher">A value indicating whether the map uses HotA (Horn of the Abyss) version 0.x format or higher</param>
/// <param name="IsHotA1OrHigher">A value indicating whether the map uses HotA (Horn of the Abyss) version 1.x format or higher</param>
/// <param name="IsHotA2OrHigher">A value indicating whether the map uses HotA (Horn of the Abyss) version 2.x format or higher</param>
/// <param name="IsHotA3OrHigher">A value indicating whether the map uses HotA (Horn of the Abyss) version 3.x format or higher</param>
/// <param name="IsHotA5OrHigher">A value indicating whether the map uses HotA (Horn of the Abyss) version 5.x format or higher</param>
/// <param name="IsHotA6OrHigher">A value indicating whether the map uses HotA (Horn of the Abyss) version 6.x format or higher</param>
/// <param name="IsHotA7OrHigher">A value indicating whether the map uses HotA (Horn of the Abyss) version 7.x format or higher</param>
/// <param name="IsHotA8OrHigher">A value indicating whether the map uses HotA (Horn of the Abyss) version 8.x format or higher</param>
public sealed record MapVersionFlags(
    bool IsRoeOrHigher,
    bool IsAbOrHigher,
    bool IsSoDOrHigher,
    bool IsChroniclesOrHigher,
    bool IsWoGOrHigher,
    bool IsHotA0OrHigher,
    bool IsHotA1OrHigher,
    bool IsHotA2OrHigher,
    bool IsHotA3OrHigher,
    bool IsHotA5OrHigher,
    bool IsHotA6OrHigher,
    bool IsHotA7OrHigher,
    bool IsHotA8OrHigher
);
