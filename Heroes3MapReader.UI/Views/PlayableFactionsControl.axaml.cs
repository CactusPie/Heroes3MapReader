using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.UI.Views;

public partial class PlayableFactionsControl : UserControl
{
    public static readonly StyledProperty<ObservableCollection<FactionType>?> FactionsProperty =
        AvaloniaProperty.Register<PlayableFactionsControl, ObservableCollection<FactionType>?>(
            nameof(Factions));

    public ObservableCollection<FactionType>? Factions
    {
        get => GetValue(FactionsProperty);
        set => SetValue(FactionsProperty, value);
    }

    public PlayableFactionsControl()
    {
        InitializeComponent();
    }
}
