using System;
using EgyptianRatScrew.CardGame.Cards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EgyptianRatScrew.DevcadeExtension;

public class TakeCardAnimation : CardAnimation
{
    internal readonly int playerId;

    private readonly Texture2D Asset;
    private readonly Rectangle Region;

    private TakeCardAnimation(
        float initialRotation, float finalRotation, 
        Vector2 initialPosition, Vector2 finalPosition, 
        Card card, int playerId,
        Texture2D asset, Rectangle region
    ) : base(
        initialRotation, finalRotation, 
        initialPosition, finalPosition, 
        card
    ) {
        this.playerId = playerId;
        Asset = asset;
        Region = region;
    }
    
    public static TakeCardAnimation For(CardAnimation anim, int playerId) {
        float finalRotation = anim.CurrentRotation();
        finalRotation /= MathF.PI;
        finalRotation = MathF.Round(finalRotation) * MathF.PI;
        return new TakeCardAnimation(
            initialRotation: anim.CurrentRotation(),
            // TODO: make finalRotation round to closest rotation within Pi/2.
            finalRotation: finalRotation,
            initialPosition: anim.CenterPosition(),
            finalPosition: Anim.PLAYER_POSITION[playerId] + Anim.CARD_OFFSET,
            card: anim.card,
            playerId: playerId,
            asset: anim.Image(),
            region: anim.AtlasRegion()
        );
    }

    protected override float PositionInterpolationFactor() {
        return PercentComplete();
    }

    protected override float RotationInterpolationFactor() {
        // Quickly rotate to upright, with minimal ending rotation.
        float factor = PercentComplete();
        //factor = factor * factor * factor;
        // Far uglier idea: 2 * integrate(0, percent) {sin(pix)^2 dx}
        factor -= MathF.Sin(factor * MathF.Tau) / MathF.Tau;
        return factor;
    }

    internal override Texture2D Image() {
        return Asset;
    }

    internal override Rectangle AtlasRegion() {
        return Region;
    }
}