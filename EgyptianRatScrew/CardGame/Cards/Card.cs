using System;

namespace EgyptianRatScrew.CardGame.Cards;

public struct Card {
    readonly CardSuit suit;
    readonly int value;

    public Card(CardSuit suit, int value)
    {
        if (value < 0 || value > 13) throw new ArgumentException($"Invali value: {value}");
        this.suit = suit;
        this.value = value;
    }

    public string ValueName() {
        return value switch {
            1  => "ACE",
            2  => "TWO",
            3  => "THREE",
            4  => "FOUR",
            5  => "FIVE",
            6  => "SIX",
            7  => "SEVEN",
            8  => "EIGHT",
            9  => "NINE",
            10 => "TEN",
            11 => "JACK",
            12 => "QUEEN",
            13 => "KING",
            _  => throw new InvalidOperationException("Illegal card value"),
        };
    }

    /// <summary>
    /// Determine if this card is a "Challenge Card". Mechanically, is this 
    /// card a face card or an ace?
    /// </summary>
    /// <returns>
    ///     True iff the card is a face card or ace; false otherwise.
    /// </returns>
    public bool IsChallengeCard() {
        return value == 1 || value > 10;
    }

    /// <summary>
    /// Determine the number of attempts a player may have to "respond" to a
    /// challenge card. Mechanically, this is the number of attempts a player
    /// has to play a face card. 
    /// </summary>
    /// <returns>
    ///     a value between 1 and 4 representing the numnber of attempts. If 
    ///     this is not a challenge card, returns an arbitrarily large number
    ///     instead.
    /// </returns>
    public int ChallengesAllowed() {
        if (!IsChallengeCard()) return int.MaxValue;
        if (value == 1) return 4;
        return value - 10;
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
    public bool CardsHaveValues(Card other, int firstValue, int secondValue) {
        if (value == firstValue && other.value == secondValue) return true;
        return value == secondValue && other.value == firstValue;
    }

    /// <summary>
    /// Determine if the two cards can be paired to make "69". 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool MakesSixtyNine(Card other) {
        return CardsHaveValues(other, 6, 9);
    }

    /// <summary>
    /// Determine if the two cards are a King-Queen pair.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool MakesKingQueen(Card other) {
        return CardsHaveValues(other, 12, 13);
    }

    /// <summary>
    /// Determine if this card and the other cards provided form an ascending
    /// or descending sequence of numbers. For example: 7, 8, 9, or Q, J, 10.
    /// </summary>
    /// <param name="second">
    ///     The second card in the sequence. Always has the middle value. 
    /// </param>
    /// <param name="third">
    ///     The third card in the sequence. Has either the highest or lowest
    ///     value in the sequence.
    /// </param>
    /// <returns>
    ///     True iff the cards form a sequence; false otherwise.
    /// </returns>
    public bool MakesSequence(Card second, Card third) {
        if (value+1 == second.value && second.value == third.value-1) {
            return true;
        }
        if (value-1 == second.value && second.value == third.value+1) {
            return true;
        }
        return false;
    }

    public override string ToString() => $"{ValueName()} OF {suit}S";
}