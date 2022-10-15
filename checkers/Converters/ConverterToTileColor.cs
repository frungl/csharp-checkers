using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using checkers.ViewModels;

namespace checkers.Converters;

public class ConverterToTileColor : IValueConverter
{
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

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}