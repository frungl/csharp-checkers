using checkers.Models;

namespace checkersTests;

public class PieceTests
{
    [Test]
    public void InitPieceTest()
    {
        var pieceLight = new Piece(new Coordinate(0, 0), true);
        Assert.Multiple(() =>
        {
            Assert.That(pieceLight.GetCoords(), Is.EqualTo(new Coordinate(0, 0)));
            Assert.That(pieceLight.IsLight(), Is.True);
            Assert.That(pieceLight.IsQueen(), Is.False);
        });

        var pieceBlack = new Piece(new Coordinate(3, 5), false, true);
        Assert.Multiple(() =>
        {
            Assert.That(pieceBlack.GetCoords(), Is.EqualTo(new Coordinate(3, 5)));
            Assert.That(pieceBlack.IsLight(), Is.False);
            Assert.That(pieceBlack.IsQueen(), Is.True);
        });
    }

    [Test]
    public void SetQueenTest()
    {
        var pieceLight = new Piece(new Coordinate(0, 0), true);
        Assert.That(pieceLight.IsQueen(), Is.False);
        pieceLight.SetQueen();
        Assert.That(pieceLight.IsQueen(), Is.True);

        var pieceBlack = new Piece(new Coordinate(3, 5), false);
        Assert.That(pieceBlack.IsQueen(), Is.False);
        pieceBlack.SetQueen();
        Assert.That(pieceBlack.IsQueen(), Is.True);
    }

    [Test]
    public void MoveToTest()
    {
        var pieceLight = new Piece(new Coordinate(0, 0), true);
        pieceLight.MoveTo(new Coordinate(1, 1));
        Assert.That(pieceLight.GetCoords(), Is.EqualTo(new Coordinate(1, 1)));

        var pieceBlack = new Piece(new Coordinate(3, 5), false);
        pieceBlack.MoveTo(new Coordinate(2, 4));
        Assert.That(pieceBlack.GetCoords(), Is.EqualTo(new Coordinate(2, 4)));
    }

    [Test]
    public void UpdateQueenStatusTest()
    {
        var piece = new Piece(new Coordinate(1, 4), true);
        Assert.That(piece.IsQueen(), Is.False);
        piece.MoveTo(new Coordinate(0, 3));
        Assert.That(piece.IsQueen(), Is.True);

        var piece2 = new Piece(new Coordinate(2, 0), false, true);
        Assert.That(piece2.IsQueen(), Is.True);
        piece2.MoveTo(new Coordinate(7, 5));
        Assert.That(piece2.IsQueen(), Is.True);
    }

    [Test]
    public void GetMovesSimpleTest()
    {
        var pattern = new[]
        {
            "...w....",
            "........",
            "........",
            "...b.w..",
            "..b.b...",
            ".......b",
            "........",
            "...w....",
        };
        var board = new Board(pattern);
        var pieces = new List<Piece>
        {
            new(new Coordinate(7, 3), true),
            new(new Coordinate(5, 7), false),
            new(new Coordinate(4, 2), false),
            new(new Coordinate(4, 4), false),
            new(new Coordinate(3, 3), false),
            new(new Coordinate(3, 5), true),
            new(new Coordinate(0, 3), true)
        };
        var expectedMoves = new List<Move>()
        {
            new(new Coordinate(7, 3), new HashSet<Coordinate>
            {
                new(6, 2), new(6, 4)
            }, false),
            new(new Coordinate(5, 7), new HashSet<Coordinate>
            {
                new(6, 6)
            }, false),
            new(new Coordinate(4, 2), new HashSet<Coordinate>
            {
                new(5, 1), new(5, 3)
            }, false),
            new(new Coordinate(4, 4), new HashSet<Coordinate>
            {
                new(2, 6)
            }, true),
            new(new Coordinate(3, 3), new HashSet<Coordinate>
            {
            }, false),
            new(new Coordinate(3, 5), new HashSet<Coordinate>
            {
                new(5, 3)
            }, true),
            new(new Coordinate(0, 3), new HashSet<Coordinate>
            {
            }, false)
        };
        var moves = pieces.Select(piece => piece.GetMoves(board)).ToList();
        Assert.That(moves, Is.EquivalentTo(expectedMoves).Using(Move.FromToIsTakingComparer));
    }

    [Test]
    public void GetMovesQueenTest()
    {
        var pattern = new[]
        {
            "........",
            "....W...",
            "...b....",
            "......b.",
            ".....B..",
            "........",
            "........",
            "........"
        };
        var board = new Board(pattern);
        var pieces = new List<Piece>
        {
            new(new Coordinate(1, 4), true, true),
            new(new Coordinate(4, 5), false, true),
        };

        var expectedMoves = new List<Move>()
        {
            new(new Coordinate(1, 4), new HashSet<Coordinate>
            {
                new(4, 7), new(3, 2), new(4, 1), new(5, 0)
            }, true),
            new(new Coordinate(4, 5), new HashSet<Coordinate>
            {
                new(3, 4), new(5, 6), new(6, 7), new(5, 4), new(6, 3), new(7, 2)
            }, false),
        };
        var moves = pieces.Select(piece => piece.GetMoves(board)).ToList();
        Assert.That(moves, Is.EquivalentTo(expectedMoves).Using(Move.FromToIsTakingComparer));
    }
}