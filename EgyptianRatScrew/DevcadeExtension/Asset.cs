using EgyptianRatScrew.CardGame.Cards;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EgyptianRatScrew.DevcadeExtension;

public static class Asset {
    /// <summary>
    /// The player circle. A dummy test item.
    /// </summary>
    public static Texture2D PlayerCircle;
    /// <summary>
    /// The sprite atlas containing all 52 cards. 
    /// Each card is 88px wide and 124px tall.
    /// </summary>
    public static Texture2D Cards;

    /// <summary>
    /// The sprite atlas containing the backside of cards based on each player.
    /// Each card is 88px wide and 124px tall.
    /// </summary>
    public static Texture2D CardBacks;

    /// <summary>
    /// The sprite atlas containing the 4 decks for the players. 
    /// Each deck is 88px wide and 140px tall. 
    /// </summary>
    public static Texture2D Decks;

    /// <summary>
    /// The main font used for things.
    /// </summary>
    public static SpriteFont Font;

    /// <summary>
    /// Initialize all the content into this object. 
    /// </summary>
    /// <param name="content">
    ///     The content manager that will load the textures
    /// </param>
    public static void LoadContent(ContentManager content) {
        PlayerCircle = content.Load<Texture2D>("PlayerCircle");
        Cards = content.Load<Texture2D>("All-88x124");
        CardBacks = content.Load<Texture2D>("Card_Back_All-88x124");
        Decks = content.Load<Texture2D>("Card_DeckAll-88x140");
    }

    /// <summary>
    /// Determine the bounding box of a single specific deck in the deck atlas.
    /// </summary>
    /// <param name="playerId">
    ///     The Id of the player whose deck is trying to be drawn. 
    ///     0 = Red, 1 = blue, 2 = green, 3 = black.
    /// </param>
    /// <returns>
    ///     A rectangle that can be fed into <c>SpriteBatch.Draw</c>'s "source"
    ///     parameter.
    /// </returns>
    public static Rectangle DeckPosition(int playerId) {
        int left = playerId % 2 * 88;
        int top = playerId / 2 * 140;
        return new Rectangle(left, top, 88, 140);
    }
}