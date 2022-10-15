using System;
using System.Collections.Generic;

namespace checkers.Models.Comparators;

public sealed class FromToIsTakingEqualityComparer : IEqualityComparer<Move>
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