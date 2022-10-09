using System;
using System.Globalization;
using Avalonia.Data.Converters;
using checkers.Models;

namespace checkers.Converters;

public class ConverterPlayerToText : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var player = value as Player;
        if (player.IsLightPlayer())
            return "Light can move";
        return "Dark can move";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}