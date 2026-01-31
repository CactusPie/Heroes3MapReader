using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.UI.Converters;

public sealed class FactionTypeConverter : IValueConverter
{
    public static readonly FactionTypeConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            FactionType.Castle => "Castle",
            FactionType.Rampart => "Rampart",
            FactionType.Tower => "Tower",
            FactionType.Inferno => "Inferno",
            FactionType.Necropolis => "Necropolis",
            FactionType.Dungeon => "Dungeon",
            FactionType.Stronghold => "Stronghold",
            FactionType.Fortress => "Fortress",
            FactionType.Conflux => "Conflux",
            FactionType.Cove => "Cove",
            FactionType.Factory => "Factory",
            FactionType.Bulwark => "Bulwark",
            _ => value?.ToString() ?? "Unknown",
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
