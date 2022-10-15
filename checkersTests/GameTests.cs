using checkers.Models;
namespace checkersTests;

public class GameTests
{
    [Test]
    public void IsPossibleMoveTest()
    {
        var pattern = new[]
        {
            "..W.....",
            "......b.",
            "........",
            "....Wb..",
            "........",
            "...w....",
            "..B.B...",
            "........"
        };
        var gameLight = new Game(pattern, true);
        var gameDark = new Game(pattern, false);

        var pieceLightCheck1 = new Piece(new Coordinate(0, 2), true, true);
        var pieceLightCheck2 = new Piece(new Coordinate(3, 4), true, true);
        var pieceLightCheck3 = new Piece(new Coordinate(5, 3), true);
        Assert.Multiple(() =>
        {
            Assert.That(gameLight.IsPossibleMove(pieceLightCheck1.GetMoves(gameLight.Board)), Is.True);
            Assert.That(gameLight.IsPossibleMove(pieceLightCheck2.GetMoves(gameLight.Board)), Is.True);
            Assert.That(gameLight.IsPossibleMove(pieceLightCheck3.GetMoves(gameLight.Board)), Is.True);
        });
        
        var pieceDarkCheck1 = new Piece(new Coordinate(1, 6), false);
        var pieceDarkCheck2 = new Piece(new Coordinate(3, 5), false);
        var pieceDarkCheck3 = new Piece(new Coordinate(6, 2), false, true);
        var pieceDarkCheck4 = new Piece(new Coordinate(6, 4), false, true);
        
        Assert.Multiple(() =>
        {
            Assert.That(gameDark.IsPossibleMove(pieceDarkCheck1.GetMoves(gameDark.Board)), Is.False);
            Assert.That(gameDark.IsPossibleMove(pieceDarkCheck2.GetMoves(gameDark.Board)), Is.False);
            Assert.That(gameDark.IsPossibleMove(pieceDarkCheck3.GetMoves(gameDark.Board)), Is.True);
            Assert.That(gameDark.IsPossibleMove(pieceDarkCheck4.GetMoves(gameDark.Board)), Is.True);
        });
    }

    [Test]
    public void MovingWhileTakingTest()
    {
        var pattern = new[]
        {
            "..W.....",
            "......b.",
            "........",
            "....Wb..",
            "........",
            "...w....",
            "..B.B...",
            "........"
        };
        var game = new Game(pattern, true);
        var piece = new Piece(new Coordinate(5, 3), true);
        game.ApplyMove(piece, new Coordinate(7, 1));
        
        var pieceCheck1 = new Piece(new Coordinate(0, 2), true, true);
        var pieceCheck2 = new Piece(new Coordinate(3, 4), true, true);
        var pieceCheck3 = new Piece(new Coordinate(7, 1), true, true);
        Assert.Multiple(() =>
        {
            Assert.That(game.IsPossibleMove(pieceCheck1.GetMoves(game.Board)), Is.False);
            Assert.That(game.IsPossibleMove(pieceCheck2.GetMoves(game.Board)), Is.False);
            Assert.That(game.IsPossibleMove(pieceCheck3.GetMoves(game.Board)), Is.True);
        });
    }
}