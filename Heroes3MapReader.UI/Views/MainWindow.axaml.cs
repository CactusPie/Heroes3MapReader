using Avalonia.Controls;
using Avalonia.Input;

namespace Heroes3MapReader.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        FocusManager?.ClearFocus();
    }
}
