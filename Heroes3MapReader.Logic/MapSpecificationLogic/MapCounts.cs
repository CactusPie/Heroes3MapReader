namespace Heroes3MapReader.Logic.MapSpecificationLogic;

/// <summary>
/// Contains the counts for various map format elements
/// </summary>
/// <param name="FactionsCount">The total number of factions available in the map format</param>
/// <param name="HeroesCount">The total number of heroes available in the map format</param>
/// <param name="HeroesPortraitsCount">The total number of hero portraits available in the map format</param>
/// <param name="ArtifactsCount">The total number of artifacts available in the map format</param>
/// <param name="ResourcesCount">The total number of resource types available in the map format</param>
/// <param name="CreaturesCount">The total number of creature types available in the map format</param>
/// <param name="SpellsCount">The total number of spells available in the map format</param>
/// <param name="SkillsCount">The total number of secondary skills available in the map format</param>
/// <param name="TerrainsCount">The total number of terrain types available in the map format</param>
/// <param name="ArtifactSlotsCount">The total number of artifact slots per hero in the map format</param>
/// <param name="BuildingsCount">The total number of buildings available in the map format</param>
/// <param name="RoadsCount">The total number of road types available in the map format</param>
/// <param name="RiversCount">The total number of river types available in the map format</param>
public sealed record MapCounts(
    int FactionsCount,
    int HeroesCount,
    int HeroesPortraitsCount,
    int ArtifactsCount,
    int ResourcesCount,
    int CreaturesCount,
    int SpellsCount,
    int SkillsCount,
    int TerrainsCount,
    int ArtifactSlotsCount,
    int BuildingsCount,
    int RoadsCount,
    int RiversCount
);
