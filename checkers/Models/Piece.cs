using System;
using System.Collections.Generic;

namespace checkers.Models;

using Coords = Tuple<int, int>;

public class Piece
{
    private int _x;
    private int _y;    
    private readonly bool _isLight;
    private bool _isQueen;
    
    public void SetQueen() => _isQueen = true;
    
    public bool IsLight() => _isLight;
    
    public bool IsQueen() => _isQueen;
    
    public Coords GetCoords() => new (_x, _y);
    
    public void UpdateQueenStatus()
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

    public Move getMoves(Board board)
    {
        var moves = new Move(_x, _y, new List<Coords>(), false);
        var movesJumps = new Move(_x, _y, new List<Coords>(), true);
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
                
                if(board.IsOverBoard(_x + dirX, _y + dirY))
                    continue;
                
                var findPiece = false;
                
                var piece = board.GetPiece(_x + dirX, _y + dirY);
                if(piece != null)
                {
                    findPiece = true;
                    if(piece.IsLight() == _isLight)
                        continue;
                }
                
                if(!mustJump && !findPiece && (_isQueen || dirX == possibleDir))
                    moves.AddTo(_x + dirX, _y + dirY);
                
                for(var jump=2; jump<=jumpSize; jump++)
                {
                    if(board.IsOverBoard(_x + dirX * jump, _y + dirY * jump))
                        break;

                    if (findPiece)
                    {
                        if (board.GetPiece(_x + dirX * jump, _y + dirY * jump) != null)
                            break;
                        
                        mustJump = true;
                        movesJumps.AddTo(_x + dirX * jump, _y + dirY * jump);
                    }
                    else
                    {
                        piece = board.GetPiece(_x + dirX * jump, _y + dirY * jump);
                        if (piece != null)
                        {
                            findPiece = true;
                            if (piece.IsLight() == _isLight)
                                break;
                        }
                    }


                    if(_isQueen && !mustJump && !findPiece)
                        moves.AddTo(_x + dirX * jump, _y + dirY * jump);
                }
            }
        }
        return mustJump ? movesJumps : moves;
    }
    
    public Piece(int x, int y, bool isLight) => (_x, _y, _isLight, _isQueen) = (x, y, isLight, false);
}