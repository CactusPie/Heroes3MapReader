namespace Heroes3MapReader.Logic.Interfaces;

/// <summary>
/// Interface for stream decompression services.
/// </summary>
public interface IStreamDecompressor
{
    /// <summary>
    /// Checks if the stream is compressed and decompresses it if needed.
    /// </summary>
    /// <param name="stream">The input stream to check and potentially decompress</param>
    /// <returns>
    /// The original stream if not compressed, or a new decompressed stream.
    /// The caller is responsible for disposing the returned stream.
    /// </returns>
    Stream DecompressIfNeeded(Stream stream);
}