using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Heroes3MapReader.UI.Views;

public partial class SpellSelectionWindow : Window
{
    public SpellSelectionWindow()
    {
        InitializeComponent();
    }

    private void OnApplyClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
