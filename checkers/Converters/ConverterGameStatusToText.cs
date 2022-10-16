using System;
using System.Globalization;
using Avalonia.Data.Converters;
using checkers.Models;

namespace checkers.Converters;

/// <summary>
/// Converts a <see cref="GameStatus"/> to a <see cref="string"/> representing the game status.
/// </summary>
public class ConverterGameStatusToText : IValueConverter
{
    /// <inheritdoc cref="ConverterGameStatusToText"/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is GameStatus gameStatus)
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

    /// <summary>
    /// not supported
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}