using Heroes3MapReader.Logic.Models;

namespace Heroes3MapReader.UI.ViewModels;

public sealed class MapItemViewModel : ViewModelBase
{
    public MapItemViewModel(string filePath, MapInfo mapInfo)
    {
        FilePath = filePath;
        Map = mapInfo;
    }

    public string FilePath { get; }
    public MapInfo Map { get; }
}
