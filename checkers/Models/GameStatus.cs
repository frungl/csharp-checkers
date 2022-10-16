namespace checkers.Models;

/// <summary>
/// Helper class for support game status.
/// </summary>
public enum GameStatus
{
    /// <summary>
    /// Light player turn.
    /// </summary>
    LightPlayerTurn,
    /// <summary>
    /// Dark player turn.
    /// </summary>
    DarkPlayerTurn,
    /// <summary>
    /// Light player win.
    /// </summary>
    LightWon,
    /// <summary>
    /// Dark player win.
    /// </summary>
    DarkWon,
    /// <summary>
    /// Draw.
    /// </summary>
    Draw
}