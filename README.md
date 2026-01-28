# Heroes 3 Map Reader

A modern, cross-platform utility for reading, analyzing, and visualizing Heroes of Might and Magic III map files (`.h3m`). Built with .NET 8 and Avalonia UI.

Currently supports all vanilla maps as well as HotA maps up to version 1.8 (Bulwark).

## Features

- **Map Parsing**: Reads detailed metadata from `.h3m` files including:
  - Map Name and Description
  - Difficulty, Size, and Format (RoE, AB, SoD, HotA)
  - Player counts and specific player details
  - Victory and Loss conditions
  - Teams and Alliances
- **Minimap Generation**: Renders visual previews of maps including terrain and underground levels.
- **Map Browser**:
  - Scan directories for map files recursively.
  - Filter maps by Size, Player Count, Difficulty, Victory Condition, and Format.
  - Sort and view map details in a responsive grid.
- **Performance**: Multi-threaded loading for handling large collections of maps efficiently.

## Technologies

- **Core**: .NET 8.0
- **UI Framework**: [Avalonia UI](https://avaloniaui.net/) (Cross-platform)
- **MVVM**: CommunityToolkit.Mvvm
- **Imaging**: SixLabors.ImageSharp

## Getting Started

### Build Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Building the Application

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Heroes3MapReader
   ```

2. **Build the solution**
   ```bash
   dotnet build
   ```

3. **Run the UI application**
   ```bash
   cd Heroes3MapReader.UI
   dotnet run
   ```

## Usage

1. Launch the application.
2. Click **"Select Map Directory"** to choose a folder containing `.h3m` files (e.g., your Heroes 3 `Maps` directory).
3. Click **"Load Maps"** to scan the directory.
4. Use the dropdown filters at the top to narrow down the list of maps.
5. Select a map from the list to view its generated minimap and detailed information.

## Project Structure

- **Heroes3MapReader.Logic**: The core class library containing the map parsing logic, GZIP decompression, and data models.
- **Heroes3MapReader.UI**: The Avalonia-based desktop application providing the graphical interface.

## Acknowledgments

- The H3M file reading used in this project was based entirely on [VCMI](https://github.com/vcmi/vcmi) implementation.