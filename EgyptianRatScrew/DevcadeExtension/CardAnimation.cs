using System;
using EgyptianRatScrew.CardGame.Cards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EgyptianRatScrew.DevcadeExtension;

class CardAnimation {
    /// <summary>
    /// The amount of time that an animation is allowed to take. If the amount
    /// of time recorded is greater than this, then the animation is forcefully
    /// finished.
    /// </summary>
    private static readonly TimeSpan MAX_ANIMATION_TIME = 
            TimeSpan.FromSeconds(0.4);

    /// <summary>
    /// Obligatory source of random numbers.
    /// </summary>
    private static readonly Random RANDOM = new(0);

    /// <summary>
    /// The amount of time this card has existed for.
    /// </summary>
    public TimeSpan animationTime = TimeSpan.Zero;

    /// <summary>
    /// The rotation to use when the animation time is 0.
    /// </summary>
    private float initialRotation;
    /// <summary>
    /// The position to use when the animation time is 0.
    /// </summary>
    private Vector2 initialPosition;
    /// <summary>
    /// The rotation to use when the animation is complete.
    /// </summary>
    private float finalRotation;

    private Vector2 finalPosition = Anim.PILE_POSITION;

    /// <summary>
    /// The card to display.
    /// </summary>
    private readonly Card card;

    /// <summary>
    /// The id of the player who played this card.
    /// </summary>
    private readonly int playerId;

    private bool isOnPile = true;

    public CardAnimation(Card card, Vector2 initialPosition, int playerId) {
        this.card = card;
        this.initialPosition = initialPosition;
        initialRotation = (float)RANDOM.NextDouble() * MathF.Tau;
        finalRotation = (float)RANDOM.NextDouble() * 0.1F * MathF.Tau + initialRotation;
        this.playerId = playerId;
    }




    /// <summary>
    /// Determines whether the animation is complete. This means the animation
    /// time exceeds the length of the animation.
    /// </summary>
    /// <returns>
    ///     True iff the animation is complete, false otherwise.
    /// </returns>
    public bool IsComplete() {
        return animationTime > MAX_ANIMATION_TIME;
    }

    /// <summary>
    /// Determines how far through the animation is, as a decimal from [0, 1].
    /// A complete animation returns 1. 
    /// </summary>
    /// <returns>
    ///     A float between 0 and 1, inclusive.
    /// </returns>
    private float PercentComplete() {
        if (IsComplete()) return 1;
        return (float) (animationTime / MAX_ANIMATION_TIME);
    }

    /// <summary>
    /// Determine the current rotation of the card, in radians. 
    /// </summary>
    /// <returns>
    ///     A float with some positive rotation value based on the animation.
    /// </returns>
    private float CurrentRotation() {
        if (IsComplete()) return finalRotation;
        return initialRotation + (finalRotation - initialRotation) * PercentComplete();
    }

    /// <summary>
    /// Approximately determine how to displace the card such that the center
    /// of rotation is the center of the card. 
    /// </summary>
    /// <returns>
    ///     A vector indicating how the card should be displaced to make the 
    ///     rotation seem to be from the cnter of the card. 
    /// </returns>
    private Vector2 RotationDisplacement() {
        if (!isOnPile) return Vector2.Zero;
        // There's probably still a glaring bug in this code, 
        // but it works respectably, so I'll take it.
        float CARD_DIM_ATAN = 0.953604935255F;
        float CARD_DIM_LENGTH = 76.026311235F;
        float rotationAngle = CurrentRotation() - CARD_DIM_ATAN;
        return new Vector2(
            -CARD_DIM_LENGTH * MathF.Sin(rotationAngle),
            +CARD_DIM_LENGTH * MathF.Cos(rotationAngle)
        );
    }

    /// <summary>
    /// Determine the Vector2 position on the screen to draw the card.
    /// </summary>
    /// <returns>
    ///     A vector indicating where the card should be drawn on the screen.
    /// </returns>
    private Vector2 CurrentPosition() {

        if (IsComplete()) return finalPosition - RotationDisplacement();

        Vector2 dPos = finalPosition - initialPosition;
        float percent = (float) PercentComplete();
        float interpolation = 1 - (1 - percent) * (1 - percent);
        dPos = new Vector2(dPos.X * interpolation, dPos.Y * interpolation);

        return dPos + initialPosition - RotationDisplacement();
    }

    /// <summary>
    /// Determine the location of the specific card to draw from the sprite 
    /// atlas containing all playing cards. 
    /// </summary>
    /// <returns>
    ///     A rectangle containing the pixel bounds of the card that this 
    ///     animation draws.
    /// </returns>
    private Rectangle AtlasCardRegion() {
        int CARD_WIDTH = 88;
        int CARD_HEIGHT = 124;
        int top = (int)card.Suit * CARD_HEIGHT;
        int left = (card.Value-1) * CARD_WIDTH;
        return new Rectangle(left, top, CARD_WIDTH, CARD_HEIGHT);
    }

    /// <summary>
    /// Determine the location of the specific card back to draw from the sprite 
    /// atlas containing all card backsides.
    /// </summary>
    /// <returns>
    ///     A rectangle containing the pixel bounds of the card that this 
    ///     animation draws.
    /// </returns>
    private Rectangle AtlasCardBackRegion() {
        int CARD_WIDTH = 88;
        int CARD_HEIGHT = 124;
        int top = playerId / 2 * CARD_HEIGHT;
        int left = playerId % 2 * CARD_WIDTH;
        return new Rectangle(left, top, CARD_WIDTH, CARD_HEIGHT);
    }

    /// <summary>
    /// Change the animation properties of this card to move it towards a player's pile.
    /// </summary>
    /// <param name="playerId">
    ///     The id of the player who is collecting the cards. 
    /// </param>
    public void SendToPlayer(int playerId) {
        // TODO: make this thing generate a wholly new card animation. This is
        // a bit excessive, honestly.

        // Position is dependent on rotation, so change the position first.
        initialPosition = CurrentPosition() + RotationDisplacement();
        initialRotation = CurrentRotation();

        finalPosition = Anim.PLAYER_POSITION[playerId];
        finalRotation = MathF.Round(initialRotation / MathF.Tau)* MathF.Tau;

        animationTime = TimeSpan.Zero;
        isOnPile = false;
    }

    /// <summary>
    /// Increment the Animation time based on the amount of time that has
    /// passed between frames.
    /// </summary>
    /// <param name="dt">
    ///     The amount of time since the last time the card was drawn. 
    /// </param>
    public void Tick(TimeSpan dt) {
        animationTime += dt;
    }

    /// <summary>
    /// Draw this card to the screen.
    /// </summary>
    /// <param name="sb">
    ///     The Sprite Batch that batches this animation for this frame.
    /// </param>
    public void Draw(SpriteBatch sb) {
        if (PercentComplete() > 0.5 || !isOnPile) {
            sb.Draw(
                texture: Asset.Cards, 
                position: CurrentPosition(), 
                sourceRectangle: AtlasCardRegion(), 
                color: Color.White,
                rotation: CurrentRotation(),
                origin: Vector2.Zero,
                scale: 1,
                effects: SpriteEffects.None,
                layerDepth: 1f
            );
        } else {
            sb.Draw(
                texture: Asset.CardBacks, 
                position: CurrentPosition(), 
                sourceRectangle: AtlasCardBackRegion(), 
                color: Color.White,
                rotation: CurrentRotation(),
                origin: Vector2.Zero,
                scale: 1,
                effects: SpriteEffects.None,
                layerDepth: 1.0f
            );
        }
    }
}