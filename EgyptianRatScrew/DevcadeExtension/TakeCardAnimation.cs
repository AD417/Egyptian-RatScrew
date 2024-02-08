using System;
using EgyptianRatScrew.CardGame.Cards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EgyptianRatScrew.DevcadeExtension;

public class TakeCardAnimation : CardAnimation
{
    internal readonly int playerId;

    private TakeCardAnimation(
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
    
    public static TakeCardAnimation For(CardAnimation anim, int playerId) {
        float finalRotation = anim.CurrentRotation();
        finalRotation /= MathF.PI;
        finalRotation = MathF.Round(finalRotation) * MathF.PI;
        return new TakeCardAnimation(
            initialRotation: anim.CurrentRotation(),
            // TODO: make finalRotation round to closest rotation within Pi/2.
            finalRotation: finalRotation,
            initialPosition: anim.CurrentPosition(),
            finalPosition: Anim.PLAYER_POSITION[playerId] + Anim.CARD_OFFSET,
            card: anim.card,
            playerId: playerId
        );
    }

    protected override float PositionInterpolationFactor()
    {
        return PercentComplete();
    }

    protected override float RotationInterpolationFactor()
    {
        // Quickly rotate to upright, with minimal ending rotation.
        float factor = PercentComplete();
        // Far uglier idea: 2 * integrate(0, percent) {sin(pix)^2 dx}
        //factor = factor * factor * factor;
        factor -= MathF.Sin(factor * MathF.Tau) / MathF.Tau;
        return factor;
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