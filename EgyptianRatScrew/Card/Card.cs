using System;

namespace EgyptianRatScrew.Card;

public struct Card {
    readonly CardFace face;
    readonly CardValue value;

    public Card(CardFace face, CardValue value)
    {
        this.face = face;
        this.value = value;
    }

    public override string ToString() => $"{value} OF {face}S";
}