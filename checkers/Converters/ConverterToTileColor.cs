using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using checkers.ViewModels;

namespace checkers.Converters;

public class ConverterToTileColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var tileType = (Tile)value;
        if (tileType == Tile.Light)
            return new SolidColorBrush(Color.Parse("#e3c16f"));
        if (tileType == Tile.Dark)
            return new SolidColorBrush(Color.Parse("#b88b4a"));
        if (tileType == Tile.Selected)
            return new SolidColorBrush(Color.Parse("#00ff1a"));
        if (tileType == Tile.Highlighted)
            return new SolidColorBrush(Color.Parse("#ff2945"));
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}