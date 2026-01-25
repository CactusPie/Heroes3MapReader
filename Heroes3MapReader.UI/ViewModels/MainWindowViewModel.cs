using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Heroes3MapReader.Logic;
using Heroes3MapReader.Logic.Models;
using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanLoadMaps))]
    private string _directoryPath = string.Empty;

    [ObservableProperty]
    private string _statusMessage = "Select a directory to load maps";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanLoadMaps))]
    private bool _isLoading;

    [ObservableProperty]
    private MapSize? _selectedSize;

    [ObservableProperty]
    private int? _selectedPlayerCount;

    [ObservableProperty]
    private MapDifficulty? _selectedDifficulty;

    [ObservableProperty]
    private VictoryConditionType? _selectedVictoryCondition;

    [ObservableProperty]
    private MapFormat? _selectedFormat;

    [ObservableProperty]
    private MapItemViewModel? _selectedMap;

    [ObservableProperty]
    private byte[]? _minimapImage;

    private readonly List<MapItemViewModel> _allMaps = [];

    private readonly IMapReaderFactory _mapReaderFactory;
    private readonly IStorageProvider _storageProvider;

    public MainWindowViewModel(IMapReaderFactory mapReaderFactory, IStorageProvider storageProvider)
    {
        _mapReaderFactory = mapReaderFactory;
        _storageProvider = storageProvider;
        MapSizes = Enum.GetValues<MapSize>().Cast<MapSize?>().Prepend(null).ToList();
        PlayerCounts = Enumerable.Range(1, 8).Cast<int?>().Prepend(null).ToList();
        Difficulties = Enum.GetValues<MapDifficulty>().Cast<MapDifficulty?>().Prepend(null).ToList();
        VictoryConditions = Enum.GetValues<VictoryConditionType>().Cast<VictoryConditionType?>().Prepend(null).ToList();
        MapFormats = Enum.GetValues<MapFormat>().Cast<MapFormat?>().Prepend(null).ToList();
    }

    public ObservableCollection<MapItemViewModel> FilteredMaps { get; } = [];
    public List<MapSize?> MapSizes { get; }
    public List<int?> PlayerCounts { get; }
    public List<MapDifficulty?> Difficulties { get; }
    public List<VictoryConditionType?> VictoryConditions { get; }
    public List<MapFormat?> MapFormats { get; }

    public bool CanLoadMaps => !string.IsNullOrWhiteSpace(DirectoryPath) && !IsLoading;

    partial void OnSelectedSizeChanged(MapSize? value)
    {
        ApplyFiltersAndSort();
    }

    partial void OnSelectedPlayerCountChanged(int? value)
    {
        ApplyFiltersAndSort();
    }

    partial void OnSelectedDifficultyChanged(MapDifficulty? value)
    {
        ApplyFiltersAndSort();
    }

    partial void OnSelectedVictoryConditionChanged(VictoryConditionType? value)
    {
        ApplyFiltersAndSort();
    }

    partial void OnSelectedFormatChanged(MapFormat? value)
    {
        ApplyFiltersAndSort();
    }

    partial void OnSelectedMapChanged(MapItemViewModel? value)
    {
        if (value != null)
        {
            GenerateMinimap(value);
        }
        else
        {
            MinimapImage = null;
        }
    }

    private void GenerateMinimap(MapItemViewModel mapViewModel)
    {
        try
        {
            var generator = new MinimapGenerator();
            MinimapImage = generator.GenerateMinimap(mapViewModel.Map, scale: 2, includeUnderground: mapViewModel.Map.HasUnderground);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error generating minimap: {ex.Message}";
            MinimapImage = null;
        }
    }

    [RelayCommand]
    private async Task BrowseDirectory()
    {
        var options = new FolderPickerOpenOptions
        {
            Title = "Select Map Directory",
            AllowMultiple = false,
        };

        IReadOnlyList<IStorageFolder> results = await _storageProvider.OpenFolderPickerAsync(options);

        if (results.Count > 0 && results[0].TryGetLocalPath() is string resultPath)
        {
            DirectoryPath = resultPath;
            StatusMessage = $"Selected directory: {resultPath}";
        }
        else
        {
            StatusMessage = "No directory selected.";
        }
    }

    [RelayCommand]
    private async Task LoadMaps()
    {
        if (string.IsNullOrWhiteSpace(DirectoryPath) || !Directory.Exists(DirectoryPath))
        {
            StatusMessage = "Invalid directory path";
            return;
        }

        IsLoading = true;
        StatusMessage = "Scanning for maps...";
        _allMaps.Clear();
        FilteredMaps.Clear();

        try
        {
            string[] mapFiles = Directory.GetFiles(DirectoryPath, "*.h3m", SearchOption.AllDirectories);
            int totalFiles = mapFiles.Length;

            if (totalFiles == 0)
            {
                StatusMessage = "No map files found in directory";
                return;
            }

            var loadedCount = 0;
            var failedCount = 0;

            // Process maps on background thread
            await Task.Run(() =>
            {
                void ProcessMap(string mapFile, IMapReader mapReader)
                {
                    try
                    {
                        MapInfo mapInfo = mapReader.ReadMap(mapFile);
                        var mapViewModel = new MapItemViewModel(mapFile, mapInfo);

                        // Update UI on UI thread
                        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                        {
                            _allMaps.Add(mapViewModel);
                            loadedCount++;
                            StatusMessage = $"Loading maps... {loadedCount + failedCount}/{totalFiles}";
                        });
                    }
                    catch (Exception)
                    {
                        // Skip maps that fail to load - continue with the rest
                        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                        {
                            failedCount++;
                            StatusMessage = $"Loading maps... {loadedCount + failedCount}/{totalFiles}";
                        });
                    }
                }

                if (totalFiles <= 100)
                {
                    IMapReader mapReader = _mapReaderFactory.Create();
                    for (int i = 0; i < totalFiles; i++)
                    {
                        ProcessMap(mapFiles[i], mapReader);
                    }
                }
                else
                {
                    int maxThreads = Math.Min(Environment.ProcessorCount, 16);
                    var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxThreads };

                    Parallel.ForEach(
                        mapFiles,
                        parallelOptions,
                        () => _mapReaderFactory.Create(),
                        (mapFile, _, mapReader) =>
                        {
                            ProcessMap(mapFile, mapReader);
                            return mapReader;
                        },
                        _ => { }
                    );
                }
            });

            // Wait a moment for final UI updates to complete
            await Task.Delay(100);

            if (failedCount > 0)
            {
                StatusMessage = $"Loaded {loadedCount} of {totalFiles} maps ({failedCount} failed)";
            }
            else
            {
                StatusMessage = $"Loaded {loadedCount} of {totalFiles} maps";
            }

            ApplyFiltersAndSort();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading maps: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ApplyFiltersAndSort()
    {
        IEnumerable<MapItemViewModel> filtered = _allMaps.AsEnumerable();

        if (SelectedSize.HasValue)
        {
            filtered = filtered.Where(m => m.Map.Size == SelectedSize.Value);
        }

        if (SelectedPlayerCount.HasValue)
        {
            filtered = filtered.Where(m => m.Map.PlayerCount == SelectedPlayerCount.Value);
        }

        if (SelectedDifficulty.HasValue)
        {
            filtered = filtered.Where(m => m.Map.Difficulty == SelectedDifficulty.Value);
        }

        if (SelectedVictoryCondition.HasValue)
        {
            filtered = filtered.Where(m => (m.Map.VictoryCondition?.Type ?? VictoryConditionType.Standard) == SelectedVictoryCondition.Value);
        }

        if (SelectedFormat.HasValue)
        {
            filtered = filtered.Where(m => m.Map.Format == SelectedFormat.Value);
        }

        FilteredMaps.Clear();
        foreach (MapItemViewModel map in filtered)
        {
            FilteredMaps.Add(map);
        }

        int filterCount = _allMaps.Count - FilteredMaps.Count;
        if (filterCount > 0)
        {
            StatusMessage = $"Showing {FilteredMaps.Count} of {_allMaps.Count} maps ({filterCount} filtered)";
        }
    }

    [RelayCommand]
    private void ClearFilters()
    {
        SelectedSize = null;
        SelectedPlayerCount = null;
        SelectedDifficulty = null;
        SelectedVictoryCondition = null;
        SelectedFormat = null;
    }
}
