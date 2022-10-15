using checkers.Models;

namespace checkersTests;

public class MoveTests
{
    [Test]
    public void AddToTest()
    {
        var move1 = new Move(new Coordinate(2, 4), new HashSet<Coordinate>(), false);
        move1.AddTo(new Coordinate(1, 1));
        move1.AddTo(new Coordinate(2, 2));
        move1.AddTo(new Coordinate(3, 3));
        var expectedMove1 = new Move(new Coordinate(2, 4), new HashSet<Coordinate> { new(1, 1), new(2, 2), new(3, 3) },
            false);
        Assert.That(move1, Is.EqualTo(expectedMove1).Using(Move.FromToIsTakingComparer));

        var move2 = new Move(new Coordinate(2, 4), new HashSet<Coordinate> { new(4, 7), new(1, -1) }, true);
        move2.AddTo(new Coordinate(1, 6));
        move2.AddTo(new Coordinate(5, 2));
        var expectedMove2 = new Move(new Coordinate(2, 4),
            new HashSet<Coordinate> { new(4, 7), new(1, -1), new(1, 6), new(5, 2) }, true);
        Assert.That(move2, Is.EqualTo(expectedMove2).Using(Move.FromToIsTakingComparer));
    }

    [Test]
    public void IsPossibleTest()
    {
        var move = new Move(new Coordinate(2, 4), new HashSet<Coordinate> { new(4, 7), new(1, -1) }, true);
        move.AddTo(new Coordinate(1, 1));
        move.AddTo(new Coordinate(2, 2));
        move.AddTo(new Coordinate(3, 3));
        Assert.Multiple(() =>
        {
            ;
            Assert.That(move.IsPossible(new Coordinate(2, 5)), Is.False);
            Assert.That(move.IsPossible(new Coordinate(3, 3)), Is.True);
            Assert.That(move.IsPossible(new Coordinate(7, 4)), Is.False);
            Assert.That(move.IsPossible(new Coordinate(4, 7)), Is.True);
            Assert.That(move.IsPossible(new Coordinate(1, -1)), Is.True);
        });
    }
}