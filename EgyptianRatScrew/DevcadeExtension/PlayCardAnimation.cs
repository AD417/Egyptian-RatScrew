using System;
using EgyptianRatScrew.CardGame.Cards;
using EgyptianRatScrew.DevcadeExtension;
using Microsoft.Xna.Framework;

class PlayCardAnimation : CardAnimation {
    readonly int playerId;

    private PlayCardAnimation(
        float initRot, float finalRot, 
        Vector2 initPos, Vector2 finPos, 
        Card card, int playerId
    ) : base(initRot, finalRot, initPos, finPos, card) {
        this.playerId = playerId;
    }

    /// <summary>
    /// Card Animation factory. Creates an animation based on the card played
    /// and the player who played it. 
    /// </summary>
    /// <param name="card"></param>
    /// <param name="playerId"></param>
    /// <returns></returns>
    private static PlayCardAnimation ForCard(Card card, int playerId) {
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
}