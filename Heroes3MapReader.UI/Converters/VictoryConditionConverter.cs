using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Heroes3MapReader.Logic.Models;

namespace Heroes3MapReader.UI.Converters;

public sealed class VictoryConditionConverter : IValueConverter
{
    public static readonly VictoryConditionConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            null => "All",
            VictoryConditionType condition => condition switch
            {
                VictoryConditionType.AcquireArtifact => "Acquire Artifact",
                VictoryConditionType.AccumulateCreatures => "Accumulate Creatures",
                VictoryConditionType.AccumulateResources => "Accumulate Resources",
                VictoryConditionType.UpgradeSpecificTown => "Upgrade Town",
                VictoryConditionType.BuildGrailStructure => "Build Grail",
                VictoryConditionType.DefeatSpecificHero => "Defeat Hero",
                VictoryConditionType.CaptureTown => "Capture Town",
                VictoryConditionType.DefeatSpecificMonster => "Defeat Monster",
                VictoryConditionType.FlagAllCreatureDwellings => "Flag All Dwellings",
                VictoryConditionType.FlagAllMines => "Flag All Mines",
                VictoryConditionType.TransportArtifact => "Transport Artifact",
                VictoryConditionType.Standard => "Standard",
                _ => "Unknown",
            },
            _ => value.ToString(),
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}