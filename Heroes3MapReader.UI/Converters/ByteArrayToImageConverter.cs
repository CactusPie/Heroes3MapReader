using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace Heroes3MapReader.UI.Converters;

public sealed class ByteArrayToImageConverter : IValueConverter
{
    public static readonly ByteArrayToImageConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not byte[] imageData || imageData.Length <= 0)
        {
            return null;
        }

        using var stream = new MemoryStream(imageData);
        return new Bitmap(stream);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}