using CommunityToolkit.Mvvm.ComponentModel;
using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.UI.ViewModels;

public partial class SpellFilterItemViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isSelected;

    public SpellType Spell { get; }
    public string Name => Spell.ToString();

    public SpellFilterItemViewModel(SpellType spell)
    {
        Spell = spell;
    }
}
