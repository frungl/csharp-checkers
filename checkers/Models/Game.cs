using System.Collections.Generic;
using System.Linq;

namespace checkers.Models;

/// <summary>
/// Represents a game of checkers.
/// </summary>
public class Game
{
    /// <summary>
    /// Board of the game.
    /// </summary>
    public readonly Board Board;

    private readonly Player _playerLight;
    private readonly Player _playerDark;
    private Player _currentPlayer;
    private List<Move> _currentPossibleMoves;
    private bool _isTakingNow;
    private GameStatus _gameStatus;

    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="boardPattern"> Pattern of the board. </param>
    /// <param name="isLightFirst"> Is light player can move first. </param>
    public Game(string[]? boardPattern, bool isLightFirst)
    {
        Board = new Board(boardPattern);
        _playerLight = new Player(true);
        _playerDark = new Player(false);
        _currentPlayer = isLightFirst ? _playerLight : _playerDark;
        _currentPossibleMoves = _currentPlayer.GetAllPossibleMoves(Board);
        _isTakingNow = false;
        _gameStatus = isLightFirst ? GameStatus.LightPlayerTurn : GameStatus.DarkPlayerTurn;
    }

    /// <summary>
    /// Gets the current game status.
    /// </summary>
    /// <returns> Current game status. </returns>
    public GameStatus GetGameStatus()
    {
        return _gameStatus;
    }

    private void ReversePlayer()
    {
        _isTakingNow = false;
        _currentPlayer = _currentPlayer == _playerLight ? _playerDark : _playerLight;
        _gameStatus = _currentPlayer.IsLightPlayer() ? GameStatus.LightPlayerTurn : GameStatus.DarkPlayerTurn;
        _currentPossibleMoves = _currentPlayer.GetAllPossibleMoves(Board);
        if (_currentPossibleMoves.Count != 0)
            return;
        if (_currentPlayer.GetAllPieces(Board).Count == 0)
        {
            _gameStatus = _gameStatus == GameStatus.DarkPlayerTurn ? GameStatus.LightWon : GameStatus.DarkWon;
        }
        else
        {
            _gameStatus = GameStatus.Draw;
        }
    }

    /// <summary>
    /// Checks if the move is possible.
    /// </summary>
    /// <param name="move"> Move to check. </param>
    /// <returns> True if the move is possible, false otherwise. </returns>
    public bool IsPossibleMove(Move? move)
    {
        if (move == null)
            return !_isTakingNow;
        return _currentPossibleMoves.Any(e =>
            e.From == move.From && e.To.SetEquals(move.To) && e.IsTaking == move.IsTaking);
    }

    /// <summary>
    /// Applies the move to the board.
    /// </summary>
    /// <param name="piece"> Piece to move. </param>
    /// <param name="to"> Coordinate to move to. </param>
    /// <returns> Move that was applied. </returns>
    public Move? ApplyMove(Piece piece, Coordinate to)
    {
        var (fromX, fromY) = piece.GetCoords();
        var move = _currentPossibleMoves.Find(t =>
            t.From.X == fromX && t.From.Y == fromY && t.To.Contains(to));
        if (move == null)
            return null;
        Board.ApplyMove(piece, to, move.IsTaking);
        if (move.IsTaking)
        {
            _currentPossibleMoves = new List<Move> { Board.GetPiece(to)!.GetMoves(Board) };
            if (_currentPossibleMoves.First().IsTaking)
            {
                _isTakingNow = true;
                return _currentPossibleMoves.First();
            }
        }

        if (!move.IsTaking || !_currentPossibleMoves.Any(t => t.IsTaking))
        {
            ReversePlayer();
        }

        return null;
    }
}