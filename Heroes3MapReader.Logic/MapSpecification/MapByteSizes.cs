namespace Heroes3MapReader.Logic.MapSpecification;

/// <summary>
/// Contains the byte sizes for different map format elements
/// </summary>
/// <param name="FactionsBytes">The number of bytes used to store faction data in the map file</param>
/// <param name="HeroesBytes">The number of bytes used to store hero data in the map file</param>
/// <param name="ArtifactsBytes">The number of bytes used to store artifact data in the map file</param>
/// <param name="SkillsBytes">The number of bytes used to store skill data in the map file</param>
/// <param name="ResourcesBytes">The number of bytes used to store resource data in the map file</param>
/// <param name="SpellsBytes">The number of bytes used to store spell data in the map file</param>
/// <param name="BuildingsBytes">The number of bytes used to store building data in the map file</param>
public sealed record MapByteSizes(
    int FactionsBytes,
    int HeroesBytes,
    int ArtifactsBytes,
    int SkillsBytes,
    int ResourcesBytes,
    int SpellsBytes,
    int BuildingsBytes
);
