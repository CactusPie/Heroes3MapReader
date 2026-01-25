using Heroes3MapReader.Logic.Models;

namespace Heroes3MapReader.Logic;

/// <summary>
/// Defines a reader for Heroes 3 map files.
/// </summary>
public interface IMapReader
{
    /// <summary>
    /// Reads a map from the specified file path.
    /// </summary>
    /// <param name="filePath">The path to the map file.</param>
    /// <returns>A <see cref="MapInfo"/> object containing the map's data.</returns>
    MapInfo ReadMap(string filePath);
    /// <summary>
    /// Reads a map from the provided stream.
    /// </summary>
    /// <param name="stream">The stream containing the map data.</param>
    /// <returns>A <see cref="MapInfo"/> object containing the map's data.</returns>
    MapInfo ReadMap(Stream stream);
}
