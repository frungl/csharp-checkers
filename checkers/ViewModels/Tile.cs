namespace checkers.ViewModels;

/// <summary>
/// Helper class for support highlighting tiles in the board
/// </summary>
public enum Tile
{
    /// <summary>
    /// Light tile
    /// </summary>
    Light,
    /// <summary>
    /// Dark tile
    /// </summary>
    Dark,
    /// <summary>
    /// Selected tile - this is the tile that the user has clicked on
    /// </summary>
    Selected,
    /// <summary>
    /// Highlighted tile - this is the tile that the user can move to
    /// </summary>
    Highlighted
}