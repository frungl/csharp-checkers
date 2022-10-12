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
                if (expectedPiece != null && (defaultPattern[i][j] == 'W' || defaultPattern[i][j] == 'B'))
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
                if (expectedPiece != null && (pattern[i][j] == 'W' || pattern[i][j] == 'B'))
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

    [Test]
    public void ApplyMoveLightTest()
    {
        var movePattern = new[]
        {
            "........",
            "........",
            "...wB...",
            "...wW...",
            "........",
            "..b..w..",
            "......b.",
            "........",
        };
        var board = new Board(movePattern);
        
        var moveFrom1 = new Coordinate(2, 3);
        var startPiece1 = new Piece(new Coordinate(2, 3), true);
        var moveTo1 = new Coordinate(3, 2);
        var endPiece1 = new Piece(new Coordinate(3, 2), true);
        
        board.ApplyMove(startPiece1, moveTo1, false);
        Assert.Multiple(() =>
        {
            Assert.That(board.GetPiece(moveFrom1), Is.Null);
            Assert.Multiple(() =>
            {
                var piece = board.GetPiece(moveTo1);
                Assert.That(piece?.GetCoords(), Is.EqualTo(endPiece1?.GetCoords()));
                Assert.That(piece?.IsLight(), Is.EqualTo(endPiece1?.IsLight()));
                Assert.That(piece?.IsQueen(), Is.EqualTo(endPiece1?.IsQueen()));
            });
        });
        
        var moveFrom2 = new Coordinate(3, 4);
        var startPiece2 = new Piece(new Coordinate(3, 4), true);
        startPiece2.SetQueen();
        var moveTo2 = new Coordinate(7, 0);
        var endPiece2 = new Piece(new Coordinate(7, 0), true);
        var checkEmpty2 = new Coordinate(5, 2);
        endPiece2.SetQueen();
        
        board.ApplyMove(startPiece2, moveTo2, true);
        Assert.Multiple(() =>
        {
            Assert.That(board.GetPiece(moveFrom2), Is.Null);
            Assert.That(board.GetPiece(checkEmpty2), Is.Null);
            Assert.Multiple(() =>
            {
                var piece = board.GetPiece(moveTo2);
                Assert.That(piece?.GetCoords(), Is.EqualTo(endPiece2?.GetCoords()));
                Assert.That(piece?.IsLight(), Is.EqualTo(endPiece2?.IsLight()));
                Assert.That(piece?.IsQueen(), Is.EqualTo(endPiece2?.IsQueen()));
            });
        });
        
        var moveFrom3 = new Coordinate(5, 5);
        var startPiece3 = new Piece(new Coordinate(5, 5), true);
        var moveTo3 = new Coordinate(7, 7);
        var endPiece3 = new Piece(new Coordinate(7, 7), true);
        var checkEmpty3 = new Coordinate(6, 6);
        endPiece3.SetQueen();
        
        board.ApplyMove(startPiece3, moveTo3, true);
        Assert.Multiple(() =>
        {
            Assert.That(board.GetPiece(moveFrom3), Is.Null);
            Assert.That(board.GetPiece(checkEmpty3), Is.Null);
            Assert.Multiple(() =>
            {
                var piece = board.GetPiece(moveTo3);
                Assert.That(piece?.GetCoords(), Is.EqualTo(endPiece3?.GetCoords()));
                Assert.That(piece?.IsLight(), Is.EqualTo(endPiece3?.IsLight()));
                Assert.That(piece?.IsQueen(), Is.EqualTo(endPiece3?.IsQueen()));
            });
        });
        
        
        var moveFrom4 = new Coordinate(3, 3);
        var startPiece4 = new Piece(new Coordinate(3, 3), true);
        var moveTo4 = new Coordinate(1, 5);
        var endPiece4 = new Piece(new Coordinate(1, 5), true);
        var checkEmpty4 = new Coordinate(2, 4);
        endPiece4.SetQueen();
        
        board.ApplyMove(startPiece4, moveTo4, true);
        Assert.Multiple(() =>
        {
            Assert.That(board.GetPiece(moveFrom4), Is.Null);
            Assert.That(board.GetPiece(checkEmpty4), Is.Null);
            Assert.Multiple(() =>
            {
                var piece = board.GetPiece(moveTo4);
                Assert.That(piece?.GetCoords(), Is.EqualTo(endPiece4?.GetCoords()));
                Assert.That(piece?.IsLight(), Is.EqualTo(endPiece4?.IsLight()));
                Assert.That(piece?.IsQueen(), Is.EqualTo(endPiece4?.IsQueen()));
            });
        });
    }

    [Test]
    public void ApplyMoveDarkTest()
    {
        var movePattern = new[]
        {
            "........",
            "......w.",
            "..w..b..",
            "........",
            "...bB...",
            "...bW...",
            "........",
            "........",
        };
        
        var board = new Board(movePattern);
        
        var moveFrom1 = new Coordinate(5, 3);
        var startPiece1 = new Piece(new Coordinate(5, 3), false);
        var moveTo1 = new Coordinate(4, 2);
        var endPiece1 = new Piece(new Coordinate(4, 2), false);
        
        board.ApplyMove(startPiece1, moveTo1, false);
        Assert.Multiple(() =>
        {
            Assert.That(board.GetPiece(moveFrom1), Is.Null);
            Assert.Multiple(() =>
            {
                var piece = board.GetPiece(moveTo1);
                Assert.That(piece?.GetCoords(), Is.EqualTo(endPiece1?.GetCoords()));
                Assert.That(piece?.IsLight(), Is.EqualTo(endPiece1?.IsLight()));
                Assert.That(piece?.IsQueen(), Is.EqualTo(endPiece1?.IsQueen()));
            });
        });
        
        var moveFrom2 = new Coordinate(4, 4);
        var startPiece2 = new Piece(new Coordinate(4, 4), false);
        startPiece2.SetQueen();
        var moveTo2 = new Coordinate(0, 0);
        var endPiece2 = new Piece(new Coordinate(0, 0), false);
        var checkEmpty2 = new Coordinate(2, 2);
        endPiece2.SetQueen();
        
        board.ApplyMove(startPiece2, moveTo2, true);
        Assert.Multiple(() =>
        {
            Assert.That(board.GetPiece(moveFrom2), Is.Null);
            Assert.That(board.GetPiece(checkEmpty2), Is.Null);
            Assert.Multiple(() =>
            {
                var piece = board.GetPiece(moveTo2);
                Assert.That(piece?.GetCoords(), Is.EqualTo(endPiece2?.GetCoords()));
                Assert.That(piece?.IsLight(), Is.EqualTo(endPiece2?.IsLight()));
                Assert.That(piece?.IsQueen(), Is.EqualTo(endPiece2?.IsQueen()));
            });
        });
        
        var moveFrom3 = new Coordinate(2, 5);
        var startPiece3 = new Piece(new Coordinate(2, 5), false);
        var moveTo3 = new Coordinate(0, 7);
        var endPiece3 = new Piece(new Coordinate(0, 7), false);
        var checkEmpty3 = new Coordinate(1, 6);
        endPiece3.SetQueen();
        
        board.ApplyMove(startPiece3, moveTo3, true);
        Assert.Multiple(() =>
        {
            Assert.That(board.GetPiece(moveFrom3), Is.Null);
            Assert.That(board.GetPiece(checkEmpty3), Is.Null);
            Assert.Multiple(() =>
            {
                var piece = board.GetPiece(moveTo3);
                Assert.That(piece?.GetCoords(), Is.EqualTo(endPiece3?.GetCoords()));
                Assert.That(piece?.IsLight(), Is.EqualTo(endPiece3?.IsLight()));
                Assert.That(piece?.IsQueen(), Is.EqualTo(endPiece3?.IsQueen()));
            });
        });
        
        
        var moveFrom4 = new Coordinate(4, 3);
        var startPiece4 = new Piece(new Coordinate(4, 3), false);
        var moveTo4 = new Coordinate(6, 5);
        var endPiece4 = new Piece(new Coordinate(6, 5), false);
        var checkEmpty4 = new Coordinate(5, 4);
        endPiece4.SetQueen();
        
        board.ApplyMove(startPiece4, moveTo4, true);
        Assert.Multiple(() =>
        {
            Assert.That(board.GetPiece(moveFrom4), Is.Null);
            Assert.That(board.GetPiece(checkEmpty4), Is.Null);
            Assert.Multiple(() =>
            {
                var piece = board.GetPiece(moveTo4);
                Assert.That(piece?.GetCoords(), Is.EqualTo(endPiece4?.GetCoords()));
                Assert.That(piece?.IsLight(), Is.EqualTo(endPiece4?.IsLight()));
                Assert.That(piece?.IsQueen(), Is.EqualTo(endPiece4?.IsQueen()));
            });
        });
    }
};