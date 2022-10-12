using checkers.Models;

namespace checkersTests;

public class MoveTests {
    [Test]
    public void AddToTest()
    {
        var move = new Move(new Coordinate(2, 4), new HashSet<Coordinate>(), false);
        move.AddTo(new Coordinate(1, 1));
        move.AddTo(new Coordinate(2, 2));
        move.AddTo(new Coordinate(3, 3));
        var expectedFrom = new Coordinate(2, 4);
        var expectedTo = new HashSet<Coordinate> { new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) };
        var expectedIsTaking = false;
        Assert.Multiple(() =>
        {
            Assert.That(move.From, Is.EqualTo(expectedFrom));
            Assert.That(move.To, Is.EqualTo(expectedTo));
            Assert.That(move.IsTaking, Is.EqualTo(expectedIsTaking));
        });
        
        var move2 = new Move(new Coordinate(2, 4), new HashSet<Coordinate>{new Coordinate(4, 7), new Coordinate(1, -1)}, true);
        move2.AddTo(new Coordinate(1, 6));
        move2.AddTo(new Coordinate(5, 2));
        var expectedFrom2 = new Coordinate(2, 4);
        var expectedTo2 = new HashSet<Coordinate> { new Coordinate(4, 7), new Coordinate(1, -1), new Coordinate(1, 6), new Coordinate(5, 2) };
        var expectedIsTaking2 = true;
        Assert.Multiple(() =>
        {
            Assert.That(move2.From, Is.EqualTo(expectedFrom2));
            Assert.That(move2.To, Is.EqualTo(expectedTo2));
            Assert.That(move2.IsTaking, Is.EqualTo(expectedIsTaking2));
        });
    }

    [Test]
    public void IsPossibleTest()
    {
        var move = new Move(new Coordinate(2, 4), new HashSet<Coordinate>{new Coordinate(4, 7), new Coordinate(1, -1)}, true);
        move.AddTo(new Coordinate(1, 1));
        move.AddTo(new Coordinate(2, 2));
        move.AddTo(new Coordinate(3, 3));
        Assert.Multiple(() =>
        {;
            Assert.That(move.IsPossible(new Coordinate(2, 5)), Is.False);
            Assert.That(move.IsPossible(new Coordinate(3, 3)), Is.True);
            Assert.That(move.IsPossible(new Coordinate(7, 4)), Is.False);
            Assert.That(move.IsPossible(new Coordinate(4, 7)), Is.False);
            Assert.That(move.IsPossible(new Coordinate(1, -1)), Is.True);
        });
    }
}