using System.Collections.Generic;
using checkers.Models.Comparators;

namespace checkers.Models;

public class Piece
{
    private Coordinate _coordinate;
    private readonly bool _isLight;
    private bool _isQueen;

    public void SetQueen() => _isQueen = true;

    public bool IsLight() => _isLight;

    public bool IsQueen() => _isQueen;

    public Coordinate GetCoords() => _coordinate;

    private void UpdateQueenStatus()
    {
        if ((!_isLight && _coordinate.X == Board.BoardSize - 1) || (_isLight && _coordinate.X == 0))
        {
            SetQueen();
        }
    }

    public void MoveTo(Coordinate moveCoordinate)
    {
        _coordinate = moveCoordinate;
        UpdateQueenStatus();
    }

    // <summary> 
    public Move GetMoves(Board board)
    {
        var moves = new Move(_coordinate, new HashSet<Coordinate>(), false);
        var movesJumps = new Move(_coordinate, new HashSet<Coordinate>(), true);
        var jumpSize = _isQueen ? 7 : 2;
        var mustJump = false;
        var directions = new List<int> { -1, 1 };
        var possibleDir = _isLight ? -1 : 1;
        foreach (var dirX in directions)
        {
            foreach (var dirY in directions)
            {
                if (dirX == 0 && dirY == 0)
                    continue;

                if (Board.IsOverBoard(new Coordinate(_coordinate.X + dirX, _coordinate.Y + dirY)))
                    continue;

                var findPiece = false;

                var piece = board.GetPiece(new Coordinate(_coordinate.X + dirX, _coordinate.Y + dirY));
                if (piece != null)
                {
                    findPiece = true;
                    if (piece.IsLight() == _isLight)
                        continue;
                }

                if (!mustJump && !findPiece && (_isQueen || dirX == possibleDir))
                    moves.AddTo(new Coordinate(_coordinate.X + dirX, _coordinate.Y + dirY));

                for (var jump = 2; jump <= jumpSize; jump++)
                {
                    if (Board.IsOverBoard(new Coordinate(_coordinate.X + dirX * jump, _coordinate.Y + dirY * jump)))
                        break;

                    if (findPiece)
                    {
                        if (board.GetPiece(new Coordinate(_coordinate.X + dirX * jump, _coordinate.Y + dirY * jump)) !=
                            null)
                            break;

                        mustJump = true;
                        movesJumps.AddTo(new Coordinate(_coordinate.X + dirX * jump, _coordinate.Y + dirY * jump));
                    }
                    else
                    {
                        piece = board.GetPiece(new Coordinate(_coordinate.X + dirX * jump,
                            _coordinate.Y + dirY * jump));
                        if (piece != null)
                        {
                            findPiece = true;
                            if (piece.IsLight() == _isLight)
                                break;
                        }
                    }

                    if (_isQueen && !mustJump && !findPiece)
                        moves.AddTo(new Coordinate(_coordinate.X + dirX * jump, _coordinate.Y + dirY * jump));
                }
            }
        }

        return mustJump ? movesJumps : moves;
    }
    
    public Piece(Coordinate coordinate, bool isLight, bool isQueen = false) =>
        (_coordinate, _isLight, _isQueen) = (coordinate, isLight, isQueen);

    public static IEqualityComparer<Piece> CoordinateIsLightIsQueenComparer { get; } =
        new CoordinateIsLightIsQueenEqualityComparer();
}