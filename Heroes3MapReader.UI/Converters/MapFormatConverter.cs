using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Heroes3MapReader.Logic.Models.Enums;
using Heroes3MapReader.UI.Resources;

namespace Heroes3MapReader.UI.Converters;

public sealed class MapFormatConverter : IValueConverter
{
    public static readonly MapFormatConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return "All";
        }

        if (value is MapFormat format)
        {
            return MapFormatNames.ResourceManager.GetString(format.ToString()) ?? format.ToString();
        }

        return value.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}