using System;

namespace checkers.Models;

using Coords = System.Tuple<int, int>;

public class Board
{
    public const int BoardSize = 8;
    private Piece?[,] _boardArray = new Piece?[BoardSize, BoardSize];
    public Piece?[,] GetBoardArray() => _boardArray;
    
    public Coords ToCoords(int singleCoord) => new Coords(singleCoord / BoardSize, singleCoord % BoardSize);
    
    public Piece? GetPiece(int x, int y) => _boardArray[x, y];
    public Piece? GetPiece(int singleCoord)
    {
        var coords = ToCoords(singleCoord);
        return GetPiece(coords.Item1, coords.Item2);    
    }
    
    public bool IsOverBoard(int x, int y)
    {
        if(x >= BoardSize || x < 0 || y >= BoardSize || y < 0)
            return true;
        return false;
    }
    public bool IsOverBoard(int singleCoord)
    {
        var coords = ToCoords(singleCoord);
        return IsOverBoard(coords.Item1, coords.Item2);
    }
    
    public bool IsLightField(int x, int y) => (x + y) % 2 == 0;
    public bool IsLightField(int singleCoord)
    {
        var coords = ToCoords(singleCoord);
        return IsLightField(coords.Item1, coords.Item2);
    }
    
    public void ApplyMove(Piece piece, int toX, int toY, bool isTaking)
    {
        var (fromX, fromY) = piece.GetCoords();
        var dirX = Math.Sign(toX - fromX);
        var dirY = Math.Sign(toY - fromY);
        var jumpSize = Math.Max(Math.Abs(toX - fromX), Math.Abs(toY - fromY));
        if (isTaking)
        {
            for (var jump = 1; jump < jumpSize; jump++)
            {
                if (_boardArray[fromX + dirX * jump, fromY + dirY * jump] != null)
                {
                    if(_boardArray[fromX + dirX * jump, fromY + dirY * jump].IsQueen())
                        piece.SetQueen();
                    _boardArray[fromX + dirX * jump, fromY + dirY * jump] = null;
                    break;
                }
            }
        }
        piece.MoveTo(toX, toY);
        _boardArray[fromX, fromY] = null;
        _boardArray[toX, toY] = piece;
    }
    
    private void InitBoard()
    {
        for (var i = 0; i < BoardSize; i++)
        {
            for (var j = 0; j < BoardSize; j++)
            {
                if (i < 3 && (i + j) % 2 == 0)
                {
                    _boardArray[i, j] = new Piece(i, j, true);
                }
                else if (i >= BoardSize - 3 && (i + j) % 2 == 0)
                {
                    _boardArray[i, j] = new Piece(i, j, false);
                }
                else
                {
                    _boardArray[i, j] = null;
                }
            }
        }
    }
    public Board()
    {
        InitBoard();
    }
}