using Heroes3MapReader.Logic.Models;
using Heroes3MapReader.Logic.Models.Enums;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Heroes3MapReader.Logic;

/// <summary>
/// Generates minimap visualizations from map terrain data
/// </summary>
public sealed class MinimapGenerator
{
    private readonly Dictionary<TerrainType, Color> _terrainColors;
    private readonly Dictionary<RoadType, Color> _roadColors;
    private readonly Dictionary<RiverType, Color> _riverColors;

    /// <summary>
    /// Initialize minimap generator with default terrain colors
    /// </summary>
    public MinimapGenerator()
    {
        _terrainColors = new Dictionary<TerrainType, Color>
        {
            { TerrainType.Dirt, Color.ParseHex("#8B7355") },
            { TerrainType.Sand, Color.ParseHex("#F4E4BC") },
            { TerrainType.Grass, Color.ParseHex("#4A7023") },
            { TerrainType.Snow, Color.ParseHex("#E8F4F8") },
            { TerrainType.Swamp, Color.ParseHex("#3E5B3C") },
            { TerrainType.Rough, Color.ParseHex("#6B5D4F") },
            { TerrainType.Subterranean, Color.ParseHex("#2B2522") },
            { TerrainType.Lava, Color.ParseHex("#D32F2F") },
            { TerrainType.Water, Color.ParseHex("#2E5C8A") },
            { TerrainType.Rock, Color.ParseHex("#4A4A4A") },
        };

        _roadColors = new Dictionary<RoadType, Color>
        {
            { RoadType.None, Color.Transparent },
            { RoadType.Dirt, Color.ParseHex("#A0826D") },
            { RoadType.Gravel, Color.ParseHex("#9E9E9E") },
            { RoadType.Cobblestone, Color.ParseHex("#757575") },
        };

        _riverColors = new Dictionary<RiverType, Color>
        {
            { RiverType.None, Color.Transparent },
            { RiverType.Clear, Color.ParseHex("#4FC3F7") },
            { RiverType.Icy, Color.ParseHex("#B3E5FC") },
            { RiverType.Muddy, Color.ParseHex("#6D4C41") },
            { RiverType.Lava, Color.ParseHex("#FF5722") },
        };
    }

    /// <summary>
    /// Initialize minimap generator with custom terrain colors
    /// </summary>
    /// <param name="terrainColors">Custom terrain color mapping</param>
    public MinimapGenerator(Dictionary<TerrainType, Color> terrainColors)
    {
        _terrainColors = terrainColors;
        _roadColors = new Dictionary<RoadType, Color>();
        _riverColors = new Dictionary<RiverType, Color>();
    }

    /// <summary>
    /// Generate a minimap image from map terrain data
    /// </summary>
    /// <param name="mapInfo">Map information with terrain data</param>
    /// <param name="scale">Scale factor for the image (1 = 1 pixel per tile)</param>
    /// <param name="includeUnderground">Whether to include underground level (placed side by side)</param>
    /// <returns>Image as byte array (PNG format)</returns>
    public byte[] GenerateMinimap(MapInfo mapInfo, int scale = 1, bool includeUnderground = false)
    {
        if (mapInfo.SurfaceTerrain == null)
        {
            throw new InvalidOperationException("Map has no terrain data. Use ReadMapWithTerrain() to load terrain.");
        }

        if (scale < 1)
        {
            scale = 1;
        }

        int mapWidth = mapInfo.Width;
        int mapHeight = mapInfo.Height;

        // Calculate final image dimensions
        int imageWidth = mapWidth * scale;
        int imageHeight = mapHeight * scale;

        if (includeUnderground && mapInfo.HasUnderground && mapInfo.UndergroundTerrain != null)
        {
            imageWidth *= 2; // Place maps side by side
        }

        using var image = new Image<Rgba32>(imageWidth, imageHeight);

        // Draw surface terrain
        DrawTerrain(image, mapInfo.SurfaceTerrain, 0, 0, mapWidth, mapHeight, scale);

        // Draw underground terrain if requested
        if (includeUnderground && mapInfo.HasUnderground && mapInfo.UndergroundTerrain != null)
        {
            DrawTerrain(image, mapInfo.UndergroundTerrain, mapWidth * scale, 0, mapWidth, mapHeight, scale);
        }

        // Export to byte array
        using var ms = new MemoryStream();
        image.SaveAsPng(ms);
        return ms.ToArray();
    }

    private void DrawTerrain(Image<Rgba32> image, TerrainTile[,] terrain, int offsetX, int offsetY, int width, int height, int scale)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                TerrainTile tile = terrain[x, y];
                Rgba32 color = GetTileColor(tile);

                // Draw scaled pixel
                for (int sy = 0; sy < scale; sy++)
                {
                    for (int sx = 0; sx < scale; sx++)
                    {
                        int px = offsetX + (x * scale) + sx;
                        int py = offsetY + (y * scale) + sy;
                        image[px, py] = color;
                    }
                }
            }
        }
    }

    private Rgba32 GetTileColor(TerrainTile tile)
    {
        // Base terrain color
        Color baseColor = _terrainColors.TryGetValue(tile.TerrainType, out Color terrain)
            ? terrain
            : Color.Gray;

        // Adjust color based on terrain sprite variation
        if (tile.TerrainSprite > 0)
        {
            Color terrainColor;
            if (Enum.IsDefined(typeof(TerrainType), tile.TerrainType))
            {
                if (!_terrainColors.TryGetValue(tile.TerrainType, out terrainColor))
                {
                    terrainColor = Color.Transparent;
                }
            }
            else
            {
                terrainColor = _terrainColors[TerrainType.Dirt];
            }

            if (terrainColor != Color.Transparent)
            {
                // Add a slight variation based on the sprite index
                float variation = (tile.TerrainSprite % 10) / 100.0f; // 0-9% variation
                var baseRgba = baseColor.ToPixel<Rgba32>();
                byte r = (byte)Math.Min(255, baseRgba.R * (1 + variation));
                byte g = (byte)Math.Min(255, baseRgba.G * (1 + variation));
                byte b = (byte)Math.Min(255, baseRgba.B * (1 + variation));
                baseColor = Color.FromRgb(r, g, b);
            }
        }

        // Blend with river if present
        if (tile.RiverType != RiverType.None)
        {
            Color riverColor;
            if (Enum.IsDefined(typeof(RiverType), tile.RiverType))
            {
                if (!_riverColors.TryGetValue(tile.RiverType, out riverColor))
                {
                    riverColor = Color.Transparent;
                }
            }
            else
            {
                riverColor = _riverColors[RiverType.Clear];
            }

            if (riverColor != Color.Transparent)
            {
                baseColor = BlendColors(baseColor, riverColor, 0.6f);

                // Adjust color based on river sprite variation
                if (tile.RiverSprite > 0)
                {
                    // Add a slight variation based on the sprite index
                    float variation = (tile.RiverSprite % 5) / 50.0f; // 0-2% variation
                    var baseRgba = baseColor.ToPixel<Rgba32>();
                    byte r = (byte)Math.Min(255, baseRgba.R * (1 + variation));
                    byte g = (byte)Math.Min(255, baseRgba.G * (1 + variation));
                    byte b = (byte)Math.Min(255, baseRgba.B * (1 + variation));
                    baseColor = Color.FromRgb(r, g, b);
                }
            }
        }

        // Blend with road if present
        if (tile.RoadType != RoadType.None)
        {
            Color roadColor;
            if (Enum.IsDefined(typeof(RoadType), tile.RoadType))
            {
                if (!_roadColors.TryGetValue(tile.RoadType, out roadColor))
                {
                    roadColor = Color.Transparent;
                }
            }
            else
            {
                roadColor = _roadColors[RoadType.Dirt];
            }

            if (roadColor != Color.Transparent)
            {
                baseColor = BlendColors(baseColor, roadColor, 0.4f);
                // Adjust color based on river sprite variation
                if (tile.RoadSprite > 0)
                {
                    // Add a slight variation based on the sprite index
                    float variation = (tile.RoadSprite % 10) / 100.0f; // 0-9% variation
                    var baseRgba = baseColor.ToPixel<Rgba32>();
                    byte r = (byte)Math.Min(255, baseRgba.R * (1 + variation));
                    byte g = (byte)Math.Min(255, baseRgba.G * (1 + variation));
                    byte b = (byte)Math.Min(255, baseRgba.B * (1 + variation));
                    baseColor = Color.FromRgb(r, g, b);
                }
            }
        }

        return new Rgba32(baseColor.ToPixel<Rgba32>().PackedValue);
    }

    private static Color BlendColors(Color baseColor, Color overlayColor, float overlayAlpha)
    {
        var baseRgba = baseColor.ToPixel<Rgba32>();
        var overlayRgba = overlayColor.ToPixel<Rgba32>();

        byte r = (byte)(baseRgba.R * (1 - overlayAlpha) + overlayRgba.R * overlayAlpha);
        byte g = (byte)(baseRgba.G * (1 - overlayAlpha) + overlayRgba.G * overlayAlpha);
        byte b = (byte)(baseRgba.B * (1 - overlayAlpha) + overlayRgba.B * overlayAlpha);

        return Color.FromRgb(r, g, b);
    }
}
