using System.Collections.Generic;
using checkers.Models.Comparators;

namespace checkers.Models;

/// <summary>
/// A class that represent piece on the board.
/// </summary>
public class Piece
{
    private Coordinate _coordinate;
    private readonly bool _isLight;
    private bool _isQueen;

    /// <summary>
    /// Make the piece a queen.
    /// </summary>
    public void SetQueen() => _isQueen = true;

    /// <summary>
    /// Is the piece light.
    /// </summary>
    /// <returns> True if piece is light, false if piece is dark. </returns>
    public bool IsLight() => _isLight;

    /// <summary>
    /// Is the piece a queen.
    /// </summary>
    /// <returns> True if piece is a queen, false if piece is not a queen. </returns>
    public bool IsQueen() => _isQueen;

    /// <summary>
    /// Get the coordinate of the piece.
    /// </summary>
    /// <returns> Coordinate of the piece. </returns>
    public Coordinate GetCoords() => _coordinate;

    private void UpdateQueenStatus()
    {
        if ((!_isLight && _coordinate.X == Board.BoardSize - 1) || (_isLight && _coordinate.X == 0))
        {
            SetQueen();
        }
    }

    /// <summary>
    /// Move the piece to a specific coordinate.
    /// </summary>
    /// <param name="moveCoordinate"> Coordinate to move to. </param>
    public void MoveTo(Coordinate moveCoordinate)
    {
        _coordinate = moveCoordinate;
        UpdateQueenStatus();
    }

    /// <summary>
    /// Get the possible moves for the piece.
    /// </summary>
    /// <param name="board"> Board to get the moves from. </param>
    /// <returns> Object of class <see cref="Move"/>. </returns>
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

    /// <summary>
    /// Create a new piece.
    /// </summary>
    /// <param name="coordinate"> Coordinate of the piece. </param>
    /// <param name="isLight"> Is the piece light. </param>
    /// <param name="isQueen"> Is the piece a queen, default is false. </param>
    public Piece(Coordinate coordinate, bool isLight, bool isQueen = false) =>
        (_coordinate, _isLight, _isQueen) = (coordinate, isLight, isQueen);

    /// <summary>
    /// Equality comparer for the piece.
    /// </summary>
    public static IEqualityComparer<Piece> CoordinateIsLightIsQueenComparer { get; } =
        new CoordinateIsLightIsQueenEqualityComparer();
}