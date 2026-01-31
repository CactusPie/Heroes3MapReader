using CommunityToolkit.Mvvm.ComponentModel;
using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.UI.ViewModels;

public partial class MapSizeFilterItemViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isSelected;

    public MapSize MapSize { get; }
    public string Name => MapSize.ToString();

    public MapSizeFilterItemViewModel(MapSize mapSize)
    {
        MapSize = mapSize;
    }
}
