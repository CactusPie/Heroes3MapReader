using Heroes3MapReader.Logic.Models;

namespace Heroes3MapReader.Logic.Interfaces;

/// <summary>
/// Defines a reader for Heroes 3 map files.
/// </summary>
public interface IMapReader
{
    /// <summary>
    /// Reads a map from the specified file path.
    /// </summary>
    /// <param name="filePath">The path to the map file.</param>
    /// <param name="readTerrain">If true, reads terrain data from the map; otherwise, skips terrain data. Useful for reducing RAM consumption</param>
    /// <returns>A <see cref="MapInfo"/> object containing the map's data.</returns>
    MapInfo ReadMap(string filePath, bool readTerrain);
}
