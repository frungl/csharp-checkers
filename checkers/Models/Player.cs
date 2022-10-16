using System.Collections.Generic;
using System.Linq;

namespace checkers.Models;

/// <summary>
/// Represents a player in the game of checkers.
/// </summary>
public class Player
{
    private bool _lightPlayer;

    /// <summary>
    /// Is this player the light player?
    /// </summary>
    /// <returns> True if this player is the light player, false otherwise. </returns>
    public bool IsLightPlayer() => _lightPlayer;

    /// <summary>
    /// Gets the player's pieces.
    /// </summary>
    /// <param name="board"> The board to get the pieces from. </param>
    /// <returns> List of <see cref="Piece"/>. </returns>
    public virtual List<Piece> GetAllPieces(Board board)
    {
        var pieces = new List<Piece>();
        for (var i = 0; i < Board.BoardSize; i++)
        {
            for (var j = 0; j < Board.BoardSize; j++)
            {
                var piece = board.GetPiece(new Coordinate(i, j));
                if (piece != null && piece.IsLight() == _lightPlayer)
                {
                    pieces.Add(piece);
                }
            }
        }

        return pieces;
    }

    /// <summary>
    /// Gets all possible moves for this player.
    /// </summary>
    /// <param name="board"> The board to get the moves from. </param>
    /// <returns> List of <see cref="Move"/>. </returns>
    public virtual List<Move> GetAllPossibleMoves(Board board)
    {
        var pieces = GetAllPieces(board);
        var moves = pieces.Select(piece => piece.GetMoves(board)).ToList();
        if (moves.Exists(x => x.IsTaking))
            moves.RemoveAll(x => !x.IsTaking);
        return moves;
    }

    /// <summary>
    /// Create a new player.
    /// </summary>
    /// <param name="lightPlayer"> Is this player the light player? </param>
    public Player(bool lightPlayer)
    {
        _lightPlayer = lightPlayer;
    }
}