using Microsoft.Xna.Framework;

namespace EgyptianRatScrew.DevcadeExtension;

static class Anim {
    public static readonly float SCALE_FACTOR = 1.0F;

    public static readonly Vector2 PILE_POSITION = new Vector2(210, 450);

    public static readonly Vector2[] PLAYER_POSITION = {
        new(0, 100), new(330, 100), new(330, 800), new(0, 800)
    };
}