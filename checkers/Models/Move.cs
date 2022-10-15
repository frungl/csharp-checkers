using System.Collections.Generic;
using checkers.Models.Comparators;

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

    public static IEqualityComparer<Move> FromToIsTakingComparer { get; } = new FromToIsTakingEqualityComparer();
}