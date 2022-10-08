namespace checkers.Models;

public class Piece
{
    private int x;
    private int y;
    private bool isQueen;
    private void SetQueen() => isQueen = true;

    public bool isLight;
    
    public Piece(int x, int y, bool isLight)
    {
        this.x = x;
        this.y = y;
        this.isLight = isLight;
        isQueen = false;
    }

    public void UpdateQueenStatus()
    {
        if (isLight && y == Board.BoardSize - 1 || !isLight && y == 0)
        {
            SetQueen();
        }
    }
    
}