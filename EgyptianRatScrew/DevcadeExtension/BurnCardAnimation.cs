using System;
using EgyptianRatScrew.CardGame.Cards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EgyptianRatScrew.DevcadeExtension;

class BurnCardAnimation : CardAnimation
{
    readonly int playerId;

    private BurnCardAnimation(
        float initialRotation, float finalRotation, 
        Vector2 initialPosition, Vector2 finalPosition, 
        Card card, int playerId
    ) : base(
        initialRotation, finalRotation, 
        initialPosition, finalPosition, 
        card
    ) {
        this.playerId = playerId;
    }

    public static BurnCardAnimation For(Card card, int playerId) {
        float initialRotation = (float) RANDOM.NextDouble() * MathF.Tau;
        float finalRotation = 
                (float) RANDOM.NextDouble() * MathF.Tau * 0.1F 
                + initialRotation;
        
        Vector2 initialPosition = Anim.PLAYER_POSITION[playerId];
        Vector2 finalPosition = Anim.BURN_POSITION;

        return new BurnCardAnimation(
            initialRotation, finalRotation, 
            initialPosition, finalPosition, 
            card, playerId
        );
    }

    protected override float PositionInterpolationFactor()
    {
        float factor = 1 - PercentComplete();
        return 1 - factor * factor;
    }

    protected override float RotationInterpolationFactor()
    {
        return PercentComplete();
    }

    protected override Texture2D Image()
    {
        return Asset.CardBacks;
    }

    protected override Rectangle AtlasRegion()
    {
        int CARD_WIDTH = 88;
        int CARD_HEIGHT = 124;
        int top = playerId / 2 * CARD_HEIGHT;
        int left = playerId % 2 * CARD_WIDTH;
        return new Rectangle(left, top, CARD_WIDTH, CARD_HEIGHT);
    }
}