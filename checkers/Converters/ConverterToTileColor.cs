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
            Tile.Selected => new SolidColorBrush(Color.Parse("#ff2945")),
            Tile.Highlighted => new SolidColorBrush(Color.Parse("#00ff1a")),
            _ => ""
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}