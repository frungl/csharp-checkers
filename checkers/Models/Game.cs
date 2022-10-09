using System;
using System.Collections.Generic;
using System.Linq;

namespace checkers.Models;

using Coords = Tuple<int, int>;

public class Game
{
    public readonly Board _board;
    private Player _playerLight;
    private Player _playerDark;
    private Player _currentPlayer;
    private List<Move> _currentPossibleMoves;
    private bool _isTakingNow;
    
    public Game()
    {
        _board = new Board();
        _playerLight = new Player(true);
        _playerDark = new Player(false);
        _currentPlayer = _playerLight;
        _currentPossibleMoves = _playerLight.GetAllPossibleMoves(_board);
        _isTakingNow = false;
    }

    private void ReversePlayer()
    {
        _isTakingNow = false;
        _currentPlayer = _currentPlayer == _playerLight ? _playerDark : _playerLight;
        _currentPossibleMoves = _currentPlayer.GetAllPossibleMoves(_board);
    }
    
    public bool IsPossibleMove(Move? move)
    {
        if (move == null)
            return !_isTakingNow;
        foreach (var tmp in _currentPossibleMoves)
        {
            if(tmp.FromX == move.FromX && tmp.FromY == move.FromY && tmp.IsTaking == move.IsTaking)
            {
                bool tmpIsPossible = true;
                foreach(var i in move.To)
                    tmpIsPossible &= tmp.To.Contains(i);
                if (tmpIsPossible)
                    return true;
            }
        }

        return false;
    }
    
    public Move? ApplyMove(Piece piece, int toX, int toY)
    {
        var (fromX, fromY) = piece.GetCoords();
        var move = _currentPossibleMoves.Find(t =>
            t.FromX == fromX && t.FromY == fromY && t.To.Contains(new Coords(toX, toY)));
        if (move != null)
        {
            _board.ApplyMove(piece, toX, toY, move.IsTaking);
            if(move.IsTaking)
            {
                _currentPossibleMoves = new List<Move>{ _board.GetPiece(toX, toY)!.getMoves(_board) };
                if (_currentPossibleMoves.First().IsTaking)
                {
                    _isTakingNow = true;
                    return _currentPossibleMoves.First();
                }
            }
            if(!move.IsTaking || !_currentPossibleMoves.Any(t => t.IsTaking))
            {
                ReversePlayer();
            }
        }
        return null;
    }
    
    public Player GetCurrentPlayer() => _currentPlayer;
}