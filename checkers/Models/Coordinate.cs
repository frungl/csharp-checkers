namespace checkers.Models;

/// <summary>
/// Coordinates of a tile on the board.
/// </summary>
/// <param name="X"> X coordinate of the tile. </param>
/// <param name="Y"> Y coordinate of the tile. </param>
public record Coordinate(int X, int Y);