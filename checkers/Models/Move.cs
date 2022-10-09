using System;
using System.Collections.Generic;
using Coords = System.Tuple<int, int>;

namespace checkers.Models;

public record Move(int FromX, int FromY, List<Coords> To, bool IsTaking)
{
    public void AddTo(int x, int y)
    {
        To.Add(new Coords(x, y));
    }
    public bool IsPossible(int x, int y)
    {
        return To.Contains(new Coords(x, y));
    }
}