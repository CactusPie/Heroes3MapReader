namespace Heroes3MapReader.Logic.MapSpecification;

/// <summary>
/// Contains the invalid identifier values for different map format elements
/// </summary>
/// <param name="HeroIdentifierInvalid">The invalid identifier value used to denote an invalid or missing hero</param>
/// <param name="ArtifactIdentifierInvalid">The invalid identifier value used to denote an invalid or missing artifact</param>
/// <param name="CreatureIdentifierInvalid">The invalid identifier value used to denote an invalid or missing creature</param>
/// <param name="SpellIdentifierInvalid">The invalid identifier value used to denote an invalid or missing spell</param>
public sealed record MapInvalidIdentifiers(
    int HeroIdentifierInvalid,
    int ArtifactIdentifierInvalid,
    int CreatureIdentifierInvalid,
    int SpellIdentifierInvalid
);
