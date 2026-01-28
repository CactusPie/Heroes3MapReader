using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.Logic.MapSpecificationLogic;

/// <summary>
/// Defines a repository for retrieving map specifications based on format and version.
/// </summary>
public interface IMapSpecificationRepository
{
    /// <summary>
    /// Gets the map specification for a given format and HotA version.
    /// </summary>
    /// <param name="format">The map format.</param>
    /// <param name="hotaVersionMajor">The major version for HotA maps.</param>
    /// <returns>The corresponding map specification.</returns>
    MapSpecification Get(MapFormat format, uint hotaVersionMajor = 0);
}