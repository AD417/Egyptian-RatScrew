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
    /// The sprite atlas containing all 52 cards. Each card is 88px wide and 124px tall.
    /// </summary>
    public static Texture2D Cards;

    /// <summary>
    /// The main font used for things.
    /// </summary>
    public static SpriteFont Font;

    public static void LoadContent(ContentManager content) {
        PlayerCircle = content.Load<Texture2D>("PlayerCircle");
        Cards = content.Load<Texture2D>("All-88x124");
    }

    public static void DrawCard(SpriteBatch sb, Card card, Vector2 pos, int scale = 2) {
        int CARD_WIDTH = 88;
        int CARD_HEIGHT = 124;
        Rectangle destination = new((int)pos.X, (int)pos.Y, CARD_WIDTH * scale, CARD_HEIGHT * scale);
        
        int top = (int)card.Suit * CARD_HEIGHT;
        int left = (card.Value-1) * CARD_WIDTH;
        Rectangle source = new Rectangle(left, top, CARD_WIDTH, CARD_HEIGHT);

        //sb.Draw(Cards, destination, source, Color.White);
        sb.Draw(Cards, pos, source, Color.White, 294, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
    }
}