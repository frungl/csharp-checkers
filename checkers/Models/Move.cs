using System;
using System.Collections.Generic;

namespace checkers.Models;

public record Move(Coordinate From, HashSet<Coordinate> To, bool IsTaking)
{
    public void AddTo(Coordinate to)
    {
        To.Add(to);
    }
    public bool IsPossible(Coordinate c)
    {
        return To.Contains(c);
    }

    private sealed class FromToIsTakingEqualityComparer : IEqualityComparer<Move>
    {
        public bool Equals(Move? x, Move? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.From.Equals(y.From) && x.To.SetEquals(y.To) && x.IsTaking == y.IsTaking;
        }

        public int GetHashCode(Move obj)
        {
            return HashCode.Combine(obj.From, obj.To, obj.IsTaking);
        }
    }

    public static IEqualityComparer<Move> FromToIsTakingComparer { get; } = new FromToIsTakingEqualityComparer();
}