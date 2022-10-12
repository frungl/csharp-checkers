using System.Collections.Generic;
using System.Linq;

namespace checkers.Models;

public class Player
{
    private bool _lightPlayer;
    
    public bool IsLightPlayer() => _lightPlayer;

    public virtual List<Piece> GetAllPieces(Board board)
    {
        var pieces = new List<Piece>();
        for(var i=0;i<Board.BoardSize;i++)
        {
            for(var j=0;j<Board.BoardSize;j++)
            {
                var piece = board.GetPiece(new Coordinate(i, j));
                if(piece != null && piece.IsLight() == _lightPlayer){
                    pieces.Add(piece);
                }
            }
        }
        return pieces;
    }
    public virtual List<Move> GetAllPossibleMoves(Board board)
    {
        var pieces = GetAllPieces(board);
        var moves = pieces.Select(piece => piece.GetMoves(board)).ToList();
        if(moves.Exists(x => x.IsTaking))
            moves.RemoveAll(x => !x.IsTaking);
        return moves;
    }
    
    public Player(bool lightPlayer)
    {
        _lightPlayer = lightPlayer;
    }
}