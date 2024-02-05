using System;

namespace EgyptianRatScrew.CardGame.Cards;

public readonly struct Card {
    /// <summary>
    /// The card's suit, as a <see cref="CardSuit"/>.
    /// </summary>
    public readonly CardSuit Suit;
    /// <summary>
    /// An integer value representing the face value of the card. Ranges from 
    /// 1 to 13. Values 1, 11, 12, and 13 are given special display names for
    /// their values, depicted as Ace, Jack, Queen, and King respectively.
    /// <para />
    /// This value should always be between 1 and 13 inclusive. 
    /// </summary>
    public readonly int Value;

    /// <summary>
    /// Create a new card instance with the given suit and value parameters.
    /// </summary>
    /// <param name="suit">
    ///     The suit of the card. 
    /// </param>
    /// <param name="value">
    ///     The face value of this card. Must be betwee 1 (Ace) and 13 (King),
    ///     inclusive.
    /// </param>
    /// <exception cref="ArgumentException">
    ///     If the provided face value is not in bounds.
    /// </exception>
    internal Card(CardSuit suit, int value)
    {
        if (value < 1 || value > 13) {
            throw new ArgumentException(
                $"Invalid value for card: '{value}'" + 
                "must be between 1 and 13 inclusive."
            );
        }
        Suit = suit;
        Value = value;
    }

    /// <summary>
    /// Determine the name of the value of this card, for use in debug display
    /// or for other cases where a card image is unavailable.
    /// </summary>
    /// <returns>
    ///     A string in all-caps giving the string name of the card's value, 
    ///     such as `ACE`, `SIX`, or `KING`. 
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If the value stored in this card is invalid.
    /// </exception>
    public readonly string ValueName() {
        return Value switch {
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
            _  => throw new InvalidOperationException(
                $"Illegal card value: {Value}"
            ),
        };
    }

    /// <summary>
    /// Determine if this card is a "Challenge Card". Mechanically, is this 
    /// card a face card or an ace?
    /// </summary>
    /// <returns>
    ///     True iff the card is a face card or ace; false otherwise.
    /// </returns>
    public readonly bool IsChallengeCard() {
        return Value == 1 || Value > 10;
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
    public readonly int ChallengesAllowed() {
        if (!IsChallengeCard()) return int.MaxValue;
        if (Value == 1) return 4;
        return Value - 10;
    }

    /// <summary>
    /// Determine if two cards have the same value. This is ignorant
    /// of the suit of the card(s).
    /// </summary>
    /// <param name="other">
    ///     The other card to check for value equality.
    /// </param>
    /// <returns></returns>
    public readonly bool SameValueAs(Card other) {
        return this.Value == other.Value;
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
    public readonly bool CardsHaveValues(Card other, int firstValue, int secondValue) {
        if (Value == firstValue && other.Value == secondValue) return true;
        return Value == secondValue && other.Value == firstValue;
    }

    /// <summary>
    /// Determine if the two cards can be paired to make "69". 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public readonly bool MakesSixtyNine(Card other) {
        return CardsHaveValues(other, 6, 9);
    }

    /// <summary>
    /// Determine if the two cards are a King-Queen pair.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public readonly bool MakesKingQueen(Card other) {
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
    public readonly bool MakesSequence(Card second, Card third) {
        if (Value+1 == second.Value && second.Value == third.Value-1) {
            return true;
        }
        if (Value-1 == second.Value && second.Value == third.Value+1) {
            return true;
        }
        return false;
    }

    public override readonly string ToString() => $"{ValueName()} OF {Suit}S";
}