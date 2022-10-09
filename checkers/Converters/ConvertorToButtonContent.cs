using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Layout;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using checkers.Models;

namespace checkers.Converters;

public class ConverterToButtonContent : IValueConverter
{
    // value is the current game, parameter is coordinates, targetType is Image or string, culture is whatever
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var piece = (Piece?)value;
        if (piece != null)
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var uri = new Uri($"avares://checkers/Assets/{(piece.IsLight() ? "Light" : "Dark")}{(piece.IsQueen() ? "Queen" : "")}.png");
            return new Image
            {
                Source = new Bitmap(assets.Open(uri)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
        }

        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}