using System;
using System.Collections.Generic;
using System.Linq;

namespace checkers.Models;

public class Board
{
    public const int BoardSize = 8;
    private readonly Piece?[,] _boardArray = new Piece?[BoardSize, BoardSize];

    public Piece? GetPiece(Coordinate c) => _boardArray[c.X, c.Y];

    public static bool IsOverBoard(Coordinate c) => c.X is >= BoardSize or < 0 || c.Y is >= BoardSize or < 0;

    public static bool IsLightField(Coordinate c) => (c.X + c.Y) % 2 == 0;

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
                _boardArray[i,j] = pattern[i][j] switch
                {
                    'w' or 'W' => new Piece(new Coordinate(i, j), true),
                    'b' or 'B' => new Piece(new Coordinate(i, j), false),
                    '.' => null,
                    _ => throw new ArgumentException("The pattern contains an invalid character.")
                };
                if(pattern[i][j] == 'W' || pattern[i][j] == 'B')
                {
                    _boardArray[i, j]!.SetQueen();
                }
            }
        }
    }

    public Board(string[]? boardPattern)
    {
        var finalPatter = boardPattern ?? new []
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
        if(finalPatter.Length != BoardSize)
            throw new ArgumentException($"Pattern must be {BoardSize}x{BoardSize}");
        if(finalPatter.Any(x => x.Length != BoardSize))
            throw new ArgumentException($"Pattern must be {BoardSize}x{BoardSize}");
        
        InitBoard(finalPatter);
    }
}