using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Heroes3MapReader.Logic.Models;
using Heroes3MapReader.UI.Resources;

namespace Heroes3MapReader.UI.Converters;

public sealed class VictoryConditionConverter : IValueConverter
{
    public static readonly VictoryConditionConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return VictoryConditionNames.ResourceManager.GetString("All");
        }

        if (value is VictoryConditionType condition)
        {
            return VictoryConditionNames.ResourceManager.GetString(condition.ToString()) ?? condition.ToString();
        }

        return value.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}