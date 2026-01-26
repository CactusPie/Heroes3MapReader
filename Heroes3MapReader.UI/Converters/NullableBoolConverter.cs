using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Heroes3MapReader.UI.Converters;

public sealed class NullableBoolConverter : IValueConverter
{
    public static readonly NullableBoolConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
        {
            return b ? "Yes" : "No";
        }
        return "All";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
