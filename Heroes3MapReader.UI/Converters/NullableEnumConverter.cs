using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Heroes3MapReader.UI.Converters;

public sealed class NullableEnumConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value == null ? "All" : value.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}