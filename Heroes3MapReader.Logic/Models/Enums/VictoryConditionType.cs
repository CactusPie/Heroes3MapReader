namespace Heroes3MapReader.Logic.Models;

/// <summary>
/// Victory condition types
/// </summary>
public enum VictoryConditionType
{
    /// <summary>Victory by acquiring a specific artifact</summary>
    AcquireArtifact = 0,
    /// <summary>Victory by accumulating a specific number of creatures</summary>
    AccumulateCreatures = 1,
    /// <summary>Victory by accumulating a specific amount of resources</summary>
    AccumulateResources = 2,
    /// <summary>Victory by upgrading a specific town to a certain level</summary>
    UpgradeSpecificTown = 3,
    /// <summary>Victory by building a specific grail structure</summary>
    BuildGrailStructure = 4,
    /// <summary>Victory by defeating a specific hero</summary>
    DefeatSpecificHero = 5,
    /// <summary>Victory by capturing a specific town</summary>
    CaptureTown = 6,
    /// <summary>Victory by defeating a specific monster</summary>
    DefeatSpecificMonster = 7,
    /// <summary>Victory by flagging all creature dwellings</summary>
    FlagAllCreatureDwellings = 8,
    /// <summary>Victory by flagging all mines</summary>
    FlagAllMines = 9,
    /// <summary>Victory by transporting a specific artifact to a destination</summary>
    TransportArtifact = 10,
    /// <summary>Victory by defeating all opponents</summary>
    DefeatAll = 11,
    /// <summary>Victory by surviving for a specified amount of time</summary>
    SurviveTime = 12,
    /// <summary>Standard victory condition</summary>
    Standard = 255,
}