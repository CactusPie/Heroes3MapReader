using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Heroes3MapReader.Logic.Models.Enums;

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
            return format switch
            {
                MapFormat.RoE => "Restoration of Erathia",
                MapFormat.AB => "Armageddon's Blade",
                MapFormat.SoD => "Shadow of Death",
                MapFormat.WoG => "Wake of Gods",
                MapFormat.HotA => "Horn of the Abyss",
                _ => "Unknown",
            };
        }

        return value.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}