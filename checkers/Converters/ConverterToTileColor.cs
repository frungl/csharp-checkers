using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using checkers.ViewModels;

namespace checkers.Converters;

/// <summary>
/// Converts a <see cref="Tile"/> to a <see cref="SolidColorBrush"/> representing the tile's color.
/// </summary>
public class ConverterToTileColor : IValueConverter
{
    /// <inheritdoc cref="ConverterToTileColor"/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Tile tileType)
            return "";
        return tileType switch
        {
            Tile.Light => new SolidColorBrush(Color.Parse("#e3c16f")),
            Tile.Dark => new SolidColorBrush(Color.Parse("#b88b4a")),
            Tile.Selected => new SolidColorBrush(Color.Parse("#8f241d")),
            Tile.Highlighted => new SolidColorBrush(Color.Parse("#41f067")),
            _ => ""
        };
    }

    /// <summary>
    /// not supported
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}