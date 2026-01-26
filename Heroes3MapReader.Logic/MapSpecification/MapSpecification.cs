namespace Heroes3MapReader.Logic.MapSpecification;

/// <summary>
/// Represents the specification for a given map format, detailing version flags, byte sizes, counts, and invalid identifiers.
/// </summary>
public class MapSpecification
{
    /// <summary>
    /// Gets the version flags that indicate which optional sections are present in the map file.
    /// </summary>
    public required MapVersionFlags VersionFlags { get; init; }
    /// <summary>
    /// Gets the byte sizes for various structures within the map file.
    /// </summary>
    public required MapByteSizes ByteSizes { get; init; }
    /// <summary>
    /// Gets the expected counts for different collections in the map file.
    /// </summary>
    public required MapCounts Counts { get; init; }
    /// <summary>
    /// Gets the values used to represent invalid or unset identifiers.
    /// </summary>
    public required MapInvalidIdentifiers InvalidIdentifiers { get; init; }
}