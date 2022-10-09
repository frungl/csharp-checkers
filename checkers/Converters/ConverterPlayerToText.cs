using System;
using System.Globalization;
using Avalonia.Data.Converters;
using checkers.Models;

namespace checkers.Converters;

public class ConverterPlayerToText : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is Player player)
        {
            return $"{(player!.IsLightPlayer() ? "White" : "Black")} can move";
        }
        return "";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}