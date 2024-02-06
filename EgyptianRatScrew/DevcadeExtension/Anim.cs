using Microsoft.Xna.Framework;

namespace EgyptianRatScrew.DevcadeExtension;

/// <summary>
/// Relevant constants used in other animation classes. 
/// </summary>
static class Anim {
    /// <summary>
    /// How much the game is scaled by in both dimensions betweem debug and 
    /// release. The default debug is 1.0, and becomes 18/7 (~= 2.5714x) for
    /// release.
    /// </summary>
    // TODO: invert this, so the scale factor is 0.3889 in debug. 
    public static readonly float SCALE_FACTOR = 1.0F;

    /// <summary>
    /// The offset between the top left corner and the center of a card. 
    /// Note that +Y is down. 
    /// </summary>
    public static readonly Vector2 CARD_OFFSET = new(44, 62);

    /// <summary>
    /// The location of the pile on the screen, before scaling.
    /// </summary>
    public static readonly Vector2 PILE_POSITION = new(210, 450);

    public static readonly Vector2 BURN_POSITION = new(210, 750);

    /// <summary>
    /// The position of the top left corner of each of the decks on the screen,
    /// before scaling. 
    /// </summary>
    public static readonly Vector2[] PLAYER_POSITION = {
        new(0, 100), new(330, 100), new(330, 800), new(0, 800)
    };
}