using Heroes3MapReader.Logic.Models;

namespace Heroes3MapReader.Logic.Repositories;

/// <summary>
/// Interface for managing application settings persistence.
/// </summary>
public interface ISettingsRepository
{
    /// <summary>
    /// Loads the application settings from storage.
    /// </summary>
    /// <returns>The loaded settings, or default settings if none exist.</returns>
    AppSettings LoadSettings();

    /// <summary>
    /// Saves the application settings to storage.
    /// </summary>
    /// <param name="settings">The settings to save.</param>
    void SaveSettings(AppSettings settings);
}
