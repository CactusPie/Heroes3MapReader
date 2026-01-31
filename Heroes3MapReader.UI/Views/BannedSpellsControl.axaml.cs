using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.UI.Views;

public partial class BannedSpellsControl : UserControl
{
    public static readonly StyledProperty<ObservableCollection<SpellType>?> SpellsProperty =
        AvaloniaProperty.Register<BannedSpellsControl, ObservableCollection<SpellType>?>(
            nameof(Spells));

    public ObservableCollection<SpellType>? Spells
    {
        get => GetValue(SpellsProperty);
        set => SetValue(SpellsProperty, value);
    }

    public BannedSpellsControl()
    {
        InitializeComponent();
    }
}
