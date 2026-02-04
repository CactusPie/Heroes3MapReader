using Avalonia.Controls;
using Avalonia.Input;

namespace Heroes3MapReader.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var dataGrid = this.FindControl<DataGrid>("MapsDataGrid");
        if (dataGrid != null)
        {
            dataGrid.SelectionChanged += (s, e) =>
            {
                if (dataGrid.SelectedItem != null)
                {
                    dataGrid.ScrollIntoView(dataGrid.SelectedItem, null);
                }
            };
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        FocusManager?.ClearFocus();
    }
}
