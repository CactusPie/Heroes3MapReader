namespace Heroes3MapReader.Logic.Interfaces;

/// <summary>
/// Factory for creating instances of IMapReader.
/// </summary>
public interface IMapReaderFactory
{
    /// <summary>
    /// Creates a new instance of IMapReader.
    /// </summary>
    /// <returns>A new IMapReader instance.</returns>
    IMapReader Create();
}
