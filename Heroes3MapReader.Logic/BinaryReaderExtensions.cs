using System.Text;

namespace Heroes3MapReader.Logic;

/// <summary>
/// Static utility class for reading data from BinaryReader with H3M map format-specific logic.
/// </summary>
internal static class BinaryReaderExtensions
{
    /// <summary>
    /// Reads a string from the binary reader using the H3M format.
    /// Format: 4-byte length prefix followed by the string bytes.
    /// </summary>
    /// <param name="reader">The binary reader to read from.</param>
    /// <param name="encoding">The encoding to use for decoding the string.</param>
    /// <returns>The decoded string.</returns>
    /// <exception cref="InvalidDataException">Thrown when the string length is too large (over 100,000 bytes).</exception>
    public static string ReadString(BinaryReader reader, Encoding encoding)
    {
        uint length = reader.ReadUInt32();
        if (length == 0)
        {
            return string.Empty;
        }

        if (length > 100000)
        {
            throw new InvalidDataException($"String length too large: {length}");
        }

        byte[] bytes = reader.ReadBytes((int)length);
        try
        {
            return encoding.GetString(bytes);
        }
        catch
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }

    /// <summary>
    /// Reads a variable number of bytes as a uint32 value.
    /// </summary>
    /// <param name="reader">The binary reader to read from.</param>
    /// <param name="bytes">Number of bytes to read (1, 2, or 4).</param>
    /// <returns>The value read as a uint32.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when bytes is not 1, 2, or 4.</exception>
    public static uint ReadBytesAsUint32(BinaryReader reader, uint bytes)
    {
        if (bytes == 1)
        {
            return reader.ReadByte();
        }

        if (bytes == 2)
        {
            return reader.ReadUInt16();
        }

        if (bytes == 4)
        {
            return reader.ReadUInt32();
        }

        throw new ArgumentOutOfRangeException($"Unsupported byte read count: {bytes}");
    }

    /// <summary>
    /// Clamps a value between a minimum and maximum value.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The clamped value.</returns>
    public static uint Clamp(uint value, uint min, uint max)
    {
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }
}
