using System.Collections.Generic;
using checkers.Models.Comparators;

namespace checkers.Models;

/// <summary>
/// A class that represents moves from one position to another.
/// </summary>
/// <param name="From"> The position from which the move starts. </param>
/// <param name="To"> Positions to which the move can be made. </param>
/// <param name="IsTaking"> A boolean value that indicates whether the move is a taking move. </param>
public record Move(Coordinate From, HashSet<Coordinate> To, bool IsTaking)
{
    /// <summary>
    /// Add a position to which the move can be made.
    /// </summary>
    /// <param name="to"> The position to which the move can be made. </param>
    public void AddTo(Coordinate to)
    {
        To.Add(to);
    }

    /// <summary>
    /// Is possible to make a move from the current position to the specified position.
    /// </summary>
    /// <param name="c"> The position to which the move is made. </param>
    /// <returns> True if the move is possible, otherwise false. </returns>
    public bool IsPossible(Coordinate c)
    {
        return To.Contains(c);
    }

    /// <summary>
    /// Equality comparer for the Move class.
    /// </summary>
    public static IEqualityComparer<Move> FromToIsTakingComparer { get; } = new FromToIsTakingEqualityComparer();
}