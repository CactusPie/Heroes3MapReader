using CommunityToolkit.Mvvm.ComponentModel;
using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.UI.ViewModels;

public partial class FactionFilterItemViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isSelected;

    public FactionType Faction { get; }
    public string Name => Faction.ToString();

    public FactionFilterItemViewModel(FactionType faction)
    {
        Faction = faction;
    }
}
