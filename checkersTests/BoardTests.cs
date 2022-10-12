using checkers.Models;
namespace checkersTests;

public class BoardTests
{
    [Test]
    public void DefaultInitBoardTest()
    {
        var defaultPattern = new[]
        {
            ".w.w.w.w",
            "w.w.w.w.",
            ".w.w.w.w",
            "........",
            "........",
            "b.b.b.b.",
            ".b.b.b.b",
            "b.b.b.b."
        };
        var board = new Board(null);
        for (var i = 0; i < Board.BoardSize; i++)
        {
            for (var j = 0; j < Board.BoardSize; j++)
            {
                var boardPiece = board.GetPiece(new Coordinate(i, j));
                var expectedPiece = defaultPattern[i][j] switch
                {
                    'w' or 'W' => new Piece(new Coordinate(i, j), true),
                    'b' or 'B' => new Piece(new Coordinate(i, j), false),
                    '.' => null,
                    _ => throw new ArgumentOutOfRangeException()
                };
                if(expectedPiece != null && (defaultPattern[i][j] == 'W' || defaultPattern[i][j] == 'B'))
                {
                    expectedPiece.SetQueen();
                }

                Assert.Multiple(() =>
                {
                    Assert.That(boardPiece?.GetCoords(), Is.EqualTo(expectedPiece?.GetCoords()));
                    Assert.That(boardPiece?.IsLight(), Is.EqualTo(expectedPiece?.IsLight()));
                    Assert.That(boardPiece?.IsQueen(), Is.EqualTo(expectedPiece?.IsQueen()));
                });
            }
        }
    }

    [Test]
    public void PatternInitBoardTest()
    {
        var pattern = new[]
        {
            "...W....",
            "..b..B.w",
            "b.......",
            "....W...",
            "W...w...",
            ".w....B.",
            ".......b",
            "b..B....",
        };
        var board = new Board(pattern);
        for (var i = 0; i < Board.BoardSize; i++)
        {
            for (var j = 0; j < Board.BoardSize; j++)
            {
                var boardPiece = board.GetPiece(new Coordinate(i, j));
                var expectedPiece = pattern[i][j] switch
                {
                    'w' or 'W' => new Piece(new Coordinate(i, j), true),
                    'b' or 'B' => new Piece(new Coordinate(i, j), false),
                    '.' => null,
                    _ => throw new ArgumentOutOfRangeException()
                };
                if(expectedPiece != null && (pattern[i][j] == 'W' || pattern[i][j] == 'B'))
                {
                    expectedPiece.SetQueen();
                }

                Assert.Multiple(() =>
                {
                    Assert.That(boardPiece?.GetCoords(), Is.EqualTo(expectedPiece?.GetCoords()));
                    Assert.That(boardPiece?.IsLight(), Is.EqualTo(expectedPiece?.IsLight()));
                    Assert.That(boardPiece?.IsQueen(), Is.EqualTo(expectedPiece?.IsQueen()));
                });
            }
        }
    }
}