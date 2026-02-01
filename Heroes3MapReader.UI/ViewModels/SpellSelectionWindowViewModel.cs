using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;
using CommunityToolkit.Mvvm.Input;
using Heroes3MapReader.Logic.Models.Enums;
using Heroes3MapReader.UI.Resources;

namespace Heroes3MapReader.UI.ViewModels;

public partial class SpellSelectionWindowViewModel : ViewModelBase
{
    public ObservableCollection<SpellFilterItemViewModel> SpellFilters { get; } = [];

    public SpellSelectionWindowViewModel()
    {
        var spells = Enum.GetValues<SpellType>()
            .Select(s => new SpellFilterItemViewModel(s))
            .OrderBy(vm => SpellNames.ResourceManager.GetString(vm.Spell.ToString()) ?? vm.Spell.ToString());

        foreach (var spell in spells)
        {
            SpellFilters.Add(spell);
        }
    }

    public void SetSelectedSpells(ObservableCollection<SpellFilterItemViewModel> currentFilters)
    {
        foreach (SpellFilterItemViewModel current in currentFilters)
        {
            SpellFilterItemViewModel? matching = SpellFilters.FirstOrDefault(s => s.Spell == current.Spell);
            if (matching != null)
            {
                matching.IsSelected = current.IsSelected;
            }
        }
    }

    [RelayCommand]
    private void SelectAll()
    {
        foreach (SpellFilterItemViewModel spell in SpellFilters)
        {
            spell.IsSelected = true;
        }
    }

    [RelayCommand]
    private void ClearAll()
    {
        foreach (SpellFilterItemViewModel spell in SpellFilters)
        {
            spell.IsSelected = false;
        }
    }
}
