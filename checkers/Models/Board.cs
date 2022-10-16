using System;
using System.Collections.Generic;
using System.Linq;

namespace checkers.Models;

/// <summary>
/// Represents a board in the game of checkers.
/// </summary>
public class Board
{
    /// <summary>
    /// Size of the board.
    /// </summary>
    public const int BoardSize = 8;

    private readonly Piece?[,] _boardArray = new Piece?[BoardSize, BoardSize];

    /// <summary>
    /// Gets the piece at the specified position.
    /// </summary>
    /// <param name="c"> Coordinate of the piece to get. </param>
    /// <returns> The piece at the specified position. </returns>
    public Piece? GetPiece(Coordinate c) => _boardArray[c.X, c.Y];

    /// <summary>
    /// Is the specified coordinate on the board?
    /// </summary>
    /// <param name="c"> Coordinate to check. </param>
    /// <returns> True if the coordinate is on the board, false otherwise. </returns>
    public static bool IsOverBoard(Coordinate c) => c.X is >= BoardSize or < 0 || c.Y is >= BoardSize or < 0;

    /// <summary>
    /// Is the tile at the specified coordinate light?
    /// </summary>
    /// <param name="c"> Coordinate to check. </param>
    /// <returns> True if the tile is light, false otherwise. </returns>
    public static bool IsLightField(Coordinate c) => (c.X + c.Y) % 2 == 0;

    /// <summary>
    /// Applies the specified move to the board.
    /// </summary>
    /// <param name="piece"> Piece to move. </param>
    /// <param name="to"> Coordinate to move to. </param>
    /// <param name="isTaking"> Is the move a taking move? </param>
    public void ApplyMove(Piece piece, Coordinate to, bool isTaking)
    {
        var (fromX, fromY) = piece.GetCoords();
        var dirX = Math.Sign(to.X - fromX);
        var dirY = Math.Sign(to.Y - fromY);
        var jumpSize = Math.Max(Math.Abs(to.X - fromX), Math.Abs(to.Y - fromY));
        if (isTaking)
        {
            for (var jump = 1; jump < jumpSize; jump++)
            {
                if (_boardArray[fromX + dirX * jump, fromY + dirY * jump] == null)
                    continue;
                if (_boardArray[fromX + dirX * jump, fromY + dirY * jump]!.IsQueen())
                    piece.SetQueen();
                _boardArray[fromX + dirX * jump, fromY + dirY * jump] = null;
                break;
            }
        }

        piece.MoveTo(to);
        _boardArray[fromX, fromY] = null;
        _boardArray[to.X, to.Y] = piece;
    }

    private void InitBoard(IReadOnlyList<string> pattern)
    {
        for (var i = 0; i < BoardSize; i++)
        {
            for (var j = 0; j < BoardSize; j++)
            {
                _boardArray[i, j] = pattern[i][j] switch
                {
                    'w' => new Piece(new Coordinate(i, j), true),
                    'W' => new Piece(new Coordinate(i, j), true, true),
                    'b' => new Piece(new Coordinate(i, j), false),
                    'B' => new Piece(new Coordinate(i, j), false, true),
                    '.' => null,
                    _ => throw new ArgumentException("The pattern contains an invalid character.")
                };
            }
        }
    }

    /// <summary>
    /// Creates a new board.
    /// </summary>
    /// <param name="boardPattern"> Pattern of the board. Can be null to create a default board. </param>
    /// <exception cref="ArgumentException"> The pattern contains an invalid character or is not 8x8. </exception>
    public Board(string[]? boardPattern)
    {
        var finalPatter = boardPattern ?? new[]
        {
            ".b.b.b.b",
            "b.b.b.b.",
            ".b.b.b.b",
            "........",
            "........",
            "w.w.w.w.",
            ".w.w.w.w",
            "w.w.w.w."
        };
        if (finalPatter.Length != BoardSize)
            throw new ArgumentException($"Pattern must be {BoardSize}x{BoardSize}");
        if (finalPatter.Any(x => x.Length != BoardSize))
            throw new ArgumentException($"Pattern must be {BoardSize}x{BoardSize}");

        InitBoard(finalPatter);
    }
}