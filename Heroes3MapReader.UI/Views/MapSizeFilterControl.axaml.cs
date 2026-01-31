using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Heroes3MapReader.UI.ViewModels;

namespace Heroes3MapReader.UI.Views;

public partial class MapSizeFilterControl : UserControl
{
    public static readonly StyledProperty<ObservableCollection<MapSizeFilterItemViewModel>?> MapSizeFiltersProperty =
        AvaloniaProperty.Register<MapSizeFilterControl, ObservableCollection<MapSizeFilterItemViewModel>?>(
            nameof(MapSizeFilters));

    public ObservableCollection<MapSizeFilterItemViewModel>? MapSizeFilters
    {
        get => GetValue(MapSizeFiltersProperty);
        set => SetValue(MapSizeFiltersProperty, value);
    }

    public MapSizeFilterControl()
    {
        InitializeComponent();
    }
}
