using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EgyptianRatScrew.DevcadeExtension;

public static class Asset {
    public static Texture2D PlayerCircle;

    public static void LoadContent(ContentManager content) {
        PlayerCircle = content.Load<Texture2D>("PlayerCircle");
    }
}