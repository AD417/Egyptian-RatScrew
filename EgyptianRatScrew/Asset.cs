using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public static class Asset {
    public static Texture2D PlayerCircle;

    public static void LoadContent(ContentManager content) {
        PlayerCircle = content.Load<Texture2D>("PlayerCircle");
    }
}