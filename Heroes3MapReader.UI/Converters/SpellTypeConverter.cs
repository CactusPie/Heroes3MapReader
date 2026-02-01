using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Heroes3MapReader.Logic.Models.Enums;
using Heroes3MapReader.UI.Resources;

namespace Heroes3MapReader.UI.Converters;

public sealed class SpellTypeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SpellType spellType)
        {
            return SpellNames.ResourceManager.GetString(spellType.ToString()) ?? spellType.ToString();
        }

        return value?.ToString() ?? "Unknown";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
