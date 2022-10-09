using System.Collections.Generic;

namespace checkers.Models;

public class Player
{
    private bool LightPlayer;
    
    public bool IsLightPlayer() => LightPlayer;

    public virtual List<Piece> GetAllPieces(Board board)
    {
        var pieces = new List<Piece>();
        for(var i=0;i<Board.BoardSize;i++)
        {
            for(var j=0;j<Board.BoardSize;j++)
            {
                var piece = board.GetPiece(new Coordinate(i, j));
                if(piece != null && piece.IsLight() == LightPlayer){
                    pieces.Add(piece);
                }
            }
        }
        return pieces;
    }
    public virtual List<Move> GetAllPossibleMoves(Board board)
    {
        var moves = new List<Move>();
        var pieces = GetAllPieces(board);
        foreach(var piece in pieces)
        {
            moves.Add(piece.GetMoves(board));
        }
        if(moves.Exists(x => x.IsTaking))
            moves.RemoveAll(x => !x.IsTaking);
        return moves;    
    }
    
    public Player(bool lightPlayer)
    {
        LightPlayer = lightPlayer;
    }
}