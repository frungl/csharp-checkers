using checkers.Models;

namespace checkersTests;

public class PlayerTests
{
    [Test]
    public void InitPlayerTest()
    {
        var playerLight = new Player(true);
        var playerDark = new Player(false);
        Assert.Multiple(() =>
        {
            Assert.That(playerLight.IsLightPlayer(), Is.True);
            Assert.That(playerDark.IsLightPlayer(), Is.False);
        });
    }

    [Test]
    public void GetAllPiecesTest()
    {
        var pattern = new[]
        {
            "...w.w..",
            "..w.....",
            ".....b..",
            "w..b...W",
            "...B.B..",
            ".......w",
            "W...B...",
            ".......B"
        };
        var board = new Board(pattern);
        var playerLight = new Player(true);
        var playerDark = new Player(false);

        var expectedLightPieces = new List<Piece>();
        var temp = new Piece(new Coordinate(0, 3), true);
        expectedLightPieces.Add(temp);
        temp = new Piece(new Coordinate(0, 5), true);
        expectedLightPieces.Add(temp);
        temp = new Piece(new Coordinate(1, 2), true);
        expectedLightPieces.Add(temp);
        temp = new Piece(new Coordinate(3, 0), true);
        expectedLightPieces.Add(temp);
        temp = new Piece(new Coordinate(3, 7), true, true);
        expectedLightPieces.Add(temp);
        temp = new Piece(new Coordinate(5, 7), true);
        expectedLightPieces.Add(temp);
        temp = new Piece(new Coordinate(6, 0), true, true);
        expectedLightPieces.Add(temp);

        var expectedDarkPieces = new List<Piece>();
        temp = new Piece(new Coordinate(2, 5), false);
        expectedDarkPieces.Add(temp);
        temp = new Piece(new Coordinate(3, 3), false);
        expectedDarkPieces.Add(temp);
        temp = new Piece(new Coordinate(4, 3), false, true);
        expectedDarkPieces.Add(temp);
        temp = new Piece(new Coordinate(4, 5), false, true);
        expectedDarkPieces.Add(temp);
        temp = new Piece(new Coordinate(6, 4), false, true);
        expectedDarkPieces.Add(temp);
        temp = new Piece(new Coordinate(7, 7), false, true);
        expectedDarkPieces.Add(temp);

        var lightPieces = playerLight.GetAllPieces(board);
        var darkPieces = playerDark.GetAllPieces(board);

        Assert.Multiple(() =>
        {
            Assert.That(lightPieces, Has.Count.EqualTo(expectedLightPieces.Count));
            Assert.That(darkPieces, Has.Count.EqualTo(expectedDarkPieces.Count));
        });
        Assert.Multiple(() =>
        {
            Assert.That(lightPieces, Is.EqualTo(expectedLightPieces).Using(Piece.CoordinateIsLightIsQueenComparer));
            Assert.That(darkPieces, Is.EqualTo(expectedDarkPieces).Using(Piece.CoordinateIsLightIsQueenComparer));
        });
    }

    [Test]
    public void GetAllPossibleMovesTest()
    {
        var pattern = new[]
        {
            "....b...",
            "..W.....",
            "........",
            "......w.",
            "........",
            ".w......",
            "...B..b.",
            "...W....",
        };
        var board = new Board(pattern);
        var playerLight = new Player(true);
        var playerDark = new Player(false);

        var expectedLightMoves = new List<Move>
        {
            new(new Coordinate(7, 3), new HashSet<Coordinate>
            {
                new(6, 2), new(6, 4), new(5, 5), new(4, 6), new(3, 7)
            }, false),
            new(new Coordinate(5, 1), new HashSet<Coordinate>
            {
                new(4, 0), new(4, 2)
            }, false),
            new(new Coordinate(3, 6), new HashSet<Coordinate>
            {
                new(2, 5), new(2, 7)
            }, false),
            new(new Coordinate(1, 2), new HashSet<Coordinate>
            {
                new(0, 1), new(0, 3), new(2, 1), new(3, 0), new(2, 3), new(3, 4), new(4, 5), new(5, 6), new(6, 7)
            }, false)
        };
        var expectedDarkMoves = new List<Move>
        {
            new(new Coordinate(6, 3), new HashSet<Coordinate>
            {
                new(2, 7)
            }, true),
        };

        var lightMoves = playerLight.GetAllPossibleMoves(board);
        var darkMoves = playerDark.GetAllPossibleMoves(board);

        Assert.Multiple(() =>
        {
            Assert.That(lightMoves, Has.Count.EqualTo(expectedLightMoves.Count));
            Assert.That(darkMoves, Has.Count.EqualTo(expectedDarkMoves.Count));
        });

        Assert.Multiple(() =>
        {
            Assert.That(lightMoves, Is.EquivalentTo(expectedLightMoves).Using(Move.FromToIsTakingComparer));
            Assert.That(darkMoves, Is.EquivalentTo(expectedDarkMoves).Using(Move.FromToIsTakingComparer));
        });
    }
}