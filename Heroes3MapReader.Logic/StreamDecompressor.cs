using System.IO.Compression;

namespace Heroes3MapReader.Logic;

/// <summary>
/// Implementation of stream decompression using gzip.
/// </summary>
public sealed class StreamDecompressor : IStreamDecompressor
{
    /// <summary>
    /// Checks if the stream is compressed and decompresses it if needed.
    /// </summary>
    /// <param name="stream">The input stream to check and potentially decompress</param>
    /// <returns>
    /// The original stream if not compressed, or a new decompressed stream.
    /// The caller is responsible for disposing the returned stream.
    /// </returns>
    public Stream DecompressIfNeeded(Stream stream)
    {
        // Read first two bytes to check for gzip magic
        int firstByte = stream.ReadByte();
        int secondByte = stream.ReadByte();
        stream.Position = 0;

        // Gzip magic: 0x1F 0x8B
        if (firstByte != 0x1F || secondByte != 0x8B)
        {
            return stream;
        }

        // Decompress the entire stream into memory
        using var gzipStream = new GZipStream(stream, CompressionMode.Decompress, leaveOpen: true);
        var memoryStream = new MemoryStream();
        gzipStream.CopyTo(memoryStream);
        memoryStream.Position = 0;

        return memoryStream;
    }
}