using System;

namespace EgyptianRatScrew.CardGame.Cards;

public struct Card {
    readonly CardFace face;
    readonly CardValue value;

    public Card(CardFace face, CardValue value)
    {
        this.face = face;
        this.value = value;
    }

    public bool SameValueAs(Card other) {
        return this.value == other.value;
    }

    public override string ToString() => $"{value} OF {face}S";
}