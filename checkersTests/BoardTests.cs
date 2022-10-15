using checkers.Models;

namespace checkersTests;

public class BoardTests
{
    private static void AssertBoard(Board board, IReadOnlyList<string> expectedPattern)
    {
        for (var i = 0; i < Board.BoardSize; i++)
        {
            for (var j = 0; j < Board.BoardSize; j++)
            {
                var boardPiece = board.GetPiece(new Coordinate(i, j));
                var expectedPiece = expectedPattern[i][j] switch
                {
                    'w' => new Piece(new Coordinate(i, j), true),
                    'W' => new Piece(new Coordinate(i, j), true, true),
                    'b' => new Piece(new Coordinate(i, j), false),
                    'B' => new Piece(new Coordinate(i, j), false, true),
                    '.' => null,
                    _ => throw new ArgumentOutOfRangeException($"{expectedPattern[i][j]} is not a valid piece")
                };

                Assert.That(boardPiece, Is.EqualTo(expectedPiece).Using(Piece.CoordinateIsLightIsQueenComparer));
            }
        }
    }

    [Test]
    public void DefaultInitBoardTest()
    {
        var defaultPattern = new[]
        {
            ".b.b.b.b",
            "b.b.b.b.",
            ".b.b.b.b",
            "........",
            "........",
            "w.w.w.w.",
            ".w.w.w.w",
            "w.w.w.w."
        };
        var board = new Board(null);
        AssertBoard(board, defaultPattern);
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
        AssertBoard(board, pattern);
    }

    [Test]
    public void IsOverBoardTest()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Board.IsOverBoard(new Coordinate(8, 0)), Is.True);
            Assert.That(Board.IsOverBoard(new Coordinate(0, 8)), Is.True);
            Assert.That(Board.IsOverBoard(new Coordinate(0, 0)), Is.False);
            Assert.That(Board.IsOverBoard(new Coordinate(7, 7)), Is.False);
            Assert.That(Board.IsOverBoard(new Coordinate(3, 4)), Is.False);
            Assert.That(Board.IsOverBoard(new Coordinate(6, 5)), Is.False);
            Assert.That(Board.IsOverBoard(new Coordinate(-1, 0)), Is.True);
            Assert.That(Board.IsOverBoard(new Coordinate(0, -1)), Is.True);
            Assert.That(Board.IsOverBoard(new Coordinate(-1, -12)), Is.True);
            Assert.That(Board.IsOverBoard(new Coordinate(-239, -1)), Is.True);
        });
    }

    [Test]
    public void IsLightFieldTest()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Board.IsLightField(new Coordinate(0, 0)), Is.True);
            Assert.That(Board.IsLightField(new Coordinate(0, 1)), Is.False);
            Assert.That(Board.IsLightField(new Coordinate(0, 2)), Is.True);
            Assert.That(Board.IsLightField(new Coordinate(0, 3)), Is.False);
            Assert.That(Board.IsLightField(new Coordinate(0, 4)), Is.True);
            Assert.That(Board.IsLightField(new Coordinate(0, 5)), Is.False);
            Assert.That(Board.IsLightField(new Coordinate(0, 6)), Is.True);
            Assert.That(Board.IsLightField(new Coordinate(0, 7)), Is.False);
        });
    }


    private static void AssertAfterApplyMove(Board board, Coordinate moveFrom, Piece startPiece, Coordinate moveTo,
        Piece endPiece, Coordinate? checkEmpty)
    {
        board.ApplyMove(startPiece, moveTo, checkEmpty != null);
        Assert.That(board.GetPiece(moveFrom), Is.Null);
        if (checkEmpty != null)
            Assert.That(board.GetPiece(checkEmpty), Is.Null);
        var piece = board.GetPiece(moveTo);
        Assert.That(piece, Is.EqualTo(endPiece).Using(Piece.CoordinateIsLightIsQueenComparer));
    }

    [Test]
    public void ApplyMoveLightTest()
    {
        var movePattern = new[]
        {
            "........",
            "......b.",
            "..b..w..",
            "........",
            "...wW...",
            "...wB...",
            "........",
            "........",
        };
        var board = new Board(movePattern);

        var moveFrom1 = new Coordinate(5, 3);
        var startPiece1 = new Piece(new Coordinate(5, 3), true);
        var moveTo1 = new Coordinate(4, 2);
        var endPiece1 = new Piece(new Coordinate(4, 2), true);

        board.ApplyMove(startPiece1, moveTo1, false);
        AssertAfterApplyMove(board, moveFrom1, startPiece1, moveTo1, endPiece1, null);

        var moveFrom2 = new Coordinate(4, 4);
        var startPiece2 = new Piece(new Coordinate(4, 4), true, true);
        var moveTo2 = new Coordinate(0, 0);
        var endPiece2 = new Piece(new Coordinate(0, 0), true, true);
        var checkEmpty2 = new Coordinate(2, 2);

        board.ApplyMove(startPiece2, moveTo2, true);
        AssertAfterApplyMove(board, moveFrom2, startPiece2, moveTo2, endPiece2, checkEmpty2);

        var moveFrom3 = new Coordinate(2, 5);
        var startPiece3 = new Piece(new Coordinate(2, 5), true);
        var moveTo3 = new Coordinate(0, 7);
        var endPiece3 = new Piece(new Coordinate(0, 7), true, true);
        var checkEmpty3 = new Coordinate(1, 6);

        board.ApplyMove(startPiece3, moveTo3, true);
        AssertAfterApplyMove(board, moveFrom3, startPiece3, moveTo3, endPiece3, checkEmpty3);


        var moveFrom4 = new Coordinate(4, 3);
        var startPiece4 = new Piece(new Coordinate(4, 3), true);
        var moveTo4 = new Coordinate(6, 5);
        var endPiece4 = new Piece(new Coordinate(6, 5), true, true);
        var checkEmpty4 = new Coordinate(5, 4);

        board.ApplyMove(startPiece4, moveTo4, true);
        AssertAfterApplyMove(board, moveFrom4, startPiece4, moveTo4, endPiece4, checkEmpty4);
    }

    [Test]
    public void ApplyMoveDarkTest()
    {
        var movePattern = new[]
        {
            "........",
            "........",
            "...bW...",
            "...bB...",
            "........",
            "..w..b..",
            "......w.",
            "........",
        };

        var board = new Board(movePattern);

        var moveFrom1 = new Coordinate(2, 3);
        var startPiece1 = new Piece(new Coordinate(2, 3), false);
        var moveTo1 = new Coordinate(3, 2);
        var endPiece1 = new Piece(new Coordinate(3, 2), false);

        board.ApplyMove(startPiece1, moveTo1, false);
        AssertAfterApplyMove(board, moveFrom1, startPiece1, moveTo1, endPiece1, null);

        var moveFrom2 = new Coordinate(3, 4);
        var startPiece2 = new Piece(new Coordinate(3, 4), false, true);
        var moveTo2 = new Coordinate(7, 0);
        var endPiece2 = new Piece(new Coordinate(7, 0), false, true);
        var checkEmpty2 = new Coordinate(5, 2);

        board.ApplyMove(startPiece2, moveTo2, true);
        AssertAfterApplyMove(board, moveFrom2, startPiece2, moveTo2, endPiece2, checkEmpty2);

        var moveFrom3 = new Coordinate(5, 5);
        var startPiece3 = new Piece(new Coordinate(5, 5), false);
        var moveTo3 = new Coordinate(7, 7);
        var endPiece3 = new Piece(new Coordinate(7, 7), false, true);
        var checkEmpty3 = new Coordinate(6, 6);

        board.ApplyMove(startPiece3, moveTo3, true);
        AssertAfterApplyMove(board, moveFrom3, startPiece3, moveTo3, endPiece3, checkEmpty3);


        var moveFrom4 = new Coordinate(3, 3);
        var startPiece4 = new Piece(new Coordinate(3, 3), false);
        var moveTo4 = new Coordinate(1, 5);
        var endPiece4 = new Piece(new Coordinate(1, 5), false, true);
        var checkEmpty4 = new Coordinate(2, 4);

        board.ApplyMove(startPiece4, moveTo4, true);
        AssertAfterApplyMove(board, moveFrom4, startPiece4, moveTo4, endPiece4, checkEmpty4);
    }
};