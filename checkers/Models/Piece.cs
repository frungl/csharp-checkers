using System.Collections.Generic;

namespace checkers.Models;

public class Piece
{
    private int _x;
    private int _y;    
    private readonly bool _isLight;
    private bool _isQueen;
    
    public void SetQueen() => _isQueen = true;
    
    public bool IsLight() => _isLight;
    
    public bool IsQueen() => _isQueen;
    
    public Coordinate GetCoords() => new (_x, _y);
    
    private void UpdateQueenStatus()
    {
        if ((_isLight && _x == Board.BoardSize - 1) || (!_isLight && _x == 0))
        {
            SetQueen();
        }
    }
    
    public void MoveTo(int x, int y)
    {
        (_x, _y) = (x, y);
        UpdateQueenStatus();
    }

    public Move GetMoves(Board board)
    {
        var moves = new Move(new Coordinate(_x, _y), new HashSet<Coordinate>(), false);
        var movesJumps = new Move(new Coordinate(_x, _y), new HashSet<Coordinate>(), true);
        var jumpSize = _isQueen ? 7 : 2;
        var mustJump = false;
        var directions = new List<int> { -1, 1 };
        var possibleDir = _isLight ? 1 : -1;
        foreach (var dirX in directions)
        {
            foreach (var dirY in directions)
            {
                if(dirX == 0 && dirY == 0) 
                    continue;
                
                if(Board.IsOverBoard(new Coordinate(_x + dirX, _y + dirY)))
                    continue;
                
                var findPiece = false;
                
                var piece = board.GetPiece(new Coordinate(_x + dirX, _y + dirY));
                if(piece != null)
                {
                    findPiece = true;
                    if(piece.IsLight() == _isLight)
                        continue;
                }
                
                if(!mustJump && !findPiece && (_isQueen || dirX == possibleDir))
                    moves.AddTo(new Coordinate(_x + dirX, _y + dirY));
                
                for(var jump=2; jump<=jumpSize; jump++)
                {
                    if(Board.IsOverBoard(new Coordinate(_x + dirX * jump, _y + dirY * jump)))
                        break;

                    if (findPiece)
                    {
                        if (board.GetPiece(new Coordinate(_x + dirX * jump, _y + dirY * jump)) != null)
                            break;
                        
                        mustJump = true;
                        movesJumps.AddTo(new Coordinate(_x + dirX * jump, _y + dirY * jump));
                    }
                    else
                    {
                        piece = board.GetPiece(new Coordinate(_x + dirX * jump, _y + dirY * jump));
                        if (piece != null)
                        {
                            findPiece = true;
                            if (piece.IsLight() == _isLight)
                                break;
                        }
                    }


                    if(_isQueen && !mustJump && !findPiece)
                        moves.AddTo(new Coordinate(_x + dirX * jump, _y + dirY * jump));
                }
            }
        }
        return mustJump ? movesJumps : moves;
    }
    
    public Piece(int x, int y, bool isLight) => (_x, _y, _isLight, _isQueen) = (x, y, isLight, false);
}