using Heroes3MapReader.UI.ViewModels;
using Heroes3MapReader.UI.Views;

namespace Heroes3MapReader.UI.Factories;

public class SpellSelectionWindowFactory : ISpellSelectionWindowFactory
{
    public SpellSelectionWindow Create()
    {
        return new SpellSelectionWindow
        {
            DataContext = new SpellSelectionWindowViewModel(),
        };
    }
}
