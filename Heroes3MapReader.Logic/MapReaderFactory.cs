using Heroes3MapReader.Logic.MapSpecificationLogic;

namespace Heroes3MapReader.Logic;

/// <summary>
/// Implementation of IMapReaderFactory.
/// </summary>
public class MapReaderFactory : IMapReaderFactory
{
    private readonly IStreamDecompressor _decompressor;
    private readonly IMapSpecificationRepository _mapSpecificationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapReaderFactory"/> class.
    /// </summary>
    /// <param name="decompressor">The stream decompressor.</param>
    /// <param name="mapSpecificationRepository">The map specification repository.</param>
    public MapReaderFactory(IStreamDecompressor decompressor, IMapSpecificationRepository mapSpecificationRepository)
    {
        _decompressor = decompressor;
        _mapSpecificationRepository = mapSpecificationRepository;
    }

    /// <inheritdoc />
    public IMapReader Create()
    {
        return new MapReader(_decompressor, _mapSpecificationRepository);
    }
}
