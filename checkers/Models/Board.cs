namespace checkers.Models;

public class Board
{
    public const int BoardSize = 8;
    public Piece?[,] BoardArray = new Piece?[BoardSize, BoardSize];
    
    public Piece?[,] GetBoardArray()
    {
        return BoardArray;
    }
    public Board()
    {
        for (var i = 0; i < BoardSize; i++)
        {
            for (var j = 0; j < BoardSize; j++)
            {
                if (i < 3 && (i + j) % 2 == 0)
                {
                    BoardArray[i, j] = new Piece(i, j, true);
                }
                else if (i > 4 && (i + j) % 2 == 1)
                {
                    BoardArray[i, j] = new Piece(i, j, false);
                }
                else
                {
                    BoardArray[i, j] = null;
                }
            }
        }
    }
}