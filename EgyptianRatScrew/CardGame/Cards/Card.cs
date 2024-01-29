using System;

namespace EgyptianRatScrew.CardGame.Cards;

public struct Card {
    readonly CardSuit suit;
    readonly CardValue value;

    public Card(CardSuit suit, CardValue value)
    {
        this.suit = suit;
        this.value = value;
    }

    /// <summary>
    /// Determine if two cards have the same value. This is ignorant
    /// of the suit of the card(s).
    /// </summary>
    /// <param name="other">
    ///     The other card to check for value equality.
    /// </param>
    /// <returns></returns>
    public bool SameValueAs(Card other) {
        return this.value == other.value;
    }

    /// <summary>
    /// Determine if this card and the other card have the two card values
    /// provided. 
    /// </summary>
    /// <param name="other">
    ///     The second card to use in the check. 
    /// </param>
    /// <param name="firstValue">
    ///     One of the two values that either card must have. Should be
    ///     different from the second value.
    /// </param>
    /// <param name="secondValue">
    ///     One of the two values that either card must have. Should be
    ///     different from the first value.
    /// </param>
    /// <returns>
    ///     True iff both values are represented on the cards; false otherwise.
    /// </returns>
    public bool CardsHaveValues(
            Card other, CardValue firstValue, 
            CardValue secondValue
    ) {
        if (value == firstValue && other.value == secondValue) return true;
        return value == secondValue && other.value == firstValue;
    }

    /// <summary>
    /// Determine if the two cards can be paired to make "69". 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool MakesSixtyNine(Card other) {
        return CardsHaveValues(other, CardValue.SIX, CardValue.NINE);
    }

    /// <summary>
    /// Determine if the two cards are a King-Queen pair.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool MakesKingQueen(Card other) {
        return CardsHaveValues(other, CardValue.QUEEN, CardValue.KING);
    }

    public override string ToString() => $"{value} OF {suit}S";
}