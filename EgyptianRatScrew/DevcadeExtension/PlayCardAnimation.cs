using System;
using EgyptianRatScrew.CardGame.Cards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EgyptianRatScrew.DevcadeExtension;

public class PlayCardAnimation : CardAnimation {
    readonly int playerId;

    private PlayCardAnimation(
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

    /// <summary>
    /// Card Animation factory. Creates an animation based on the card played
    /// and the player who played it. 
    /// </summary>
    /// <param name="card"></param>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public static PlayCardAnimation For(Card card, int playerId) {
        float initialRotation = (float) RANDOM.NextDouble() * MathF.Tau;
        float finalRotation = 
                (float) RANDOM.NextDouble() * MathF.Tau * 0.1F 
                + initialRotation;
        
        Vector2 initialPosition = Anim.PLAYER_POSITION[playerId];
        Vector2 finalPosition = Anim.PILE_POSITION;

        return new PlayCardAnimation(
            initialRotation, finalRotation, 
            initialPosition, finalPosition, 
            card, playerId
        );
    }

    protected override float RotationInterpolationFactor() {
        return PercentComplete();
    }

    protected override float PositionInterpolationFactor()
    {
        // The card moves quickly early on, and then slows down to a halt.
        float percent = PercentComplete();
        return 1 - (1 - percent) * (1 - percent);
    }

    protected override Texture2D Image() {
        if (PercentComplete() > 0.5) return Asset.Cards;
        return Asset.CardBacks;
    }

    protected override Rectangle AtlasRegion() {
        if (PercentComplete() > 0.5) return base.AtlasRegion();

        int CARD_WIDTH = 88;
        int CARD_HEIGHT = 124;
        int top = playerId / 2 * CARD_HEIGHT;
        int left = playerId % 2 * CARD_WIDTH;
        return new Rectangle(left, top, CARD_WIDTH, CARD_HEIGHT);
    }
}