using System;
using System.Globalization;
using Avalonia.Data.Converters;
using checkers.Models;

namespace checkers.Converters;

public class ConverterGameStatusToText : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is GameStatus gameStatus)
        {
            return gameStatus switch
            {
                GameStatus.Draw => "Game ended in a draw",
                GameStatus.LightWon => "Game ended, light won",
                GameStatus.DarkWon => "Game ended, dark won",
                GameStatus.DarkPlayerTurn => "Dark player turn",
                GameStatus.LightPlayerTurn => "Light player turn",
                _ => ""
            };
        }
        return "";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}