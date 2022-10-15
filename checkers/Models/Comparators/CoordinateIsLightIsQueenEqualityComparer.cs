using System;
using System.Collections.Generic;

namespace checkers.Models.Comparators;

public sealed class CoordinateIsLightIsQueenEqualityComparer : IEqualityComparer<Piece>
{
    public bool Equals(Piece? x, Piece? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.GetCoords().Equals(y.GetCoords()) && x.IsLight() == y.IsLight() && x.IsQueen() == y.IsQueen();
    }

    public int GetHashCode(Piece obj)
    {
        return HashCode.Combine(obj.GetCoords(), obj.IsLight(), obj.IsQueen());
    }
}