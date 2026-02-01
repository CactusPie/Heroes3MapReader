using System.Text.Json;
using Heroes3MapReader.Logic.Models;

namespace Heroes3MapReader.Logic.Repositories;

/// <summary>
/// Implementation of ISettingsRepository that stores settings in a JSON file.
/// </summary>
public class SettingsRepository : ISettingsRepository
{
    private readonly string _settingsFilePath;
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsRepository"/> class.
    /// </summary>
    public SettingsRepository()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string appDirectory = Path.Combine(appDataPath, "Heroes3MapReader");
        _settingsFilePath = Path.Combine(appDirectory, "settings.json");
    }

    /// <inheritdoc />
    public AppSettings LoadSettings()
    {
        if (File.Exists(_settingsFilePath))
        {
            string json = File.ReadAllText(_settingsFilePath);
            var settings = JsonSerializer.Deserialize<AppSettings>(json, SerializerOptions);
            return settings ?? new AppSettings();
        }

        return new AppSettings();
    }

    /// <inheritdoc />
    public void SaveSettings(AppSettings settings)
    {
        string? directory = Path.GetDirectoryName(_settingsFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = JsonSerializer.Serialize(settings, SerializerOptions);
        File.WriteAllText(_settingsFilePath, json);
    }
}
