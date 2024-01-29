using System;
using System.Collections.Generic;

namespace EgyptianRatScrew.CardGame.Cards;

public class Deck : LinkedList<Card> {
    /// <summary>
    /// Generates a random list containing all the cards in a standard
    /// deck of cards. The size of this list is a multiple of 52, based on
    /// how many decks should be shuffled together at creation time.
    /// </summary>
    /// <param name="decks">
    ///     The number of decks that should be shuffled together to create the
    ///     starting deck.
    /// </param>
    /// <returns>
    ///     A deck containing <c>decks</c> copies of each of the 52 cards in a 
    ///     standard deck, in a random order.
    /// </returns>
    public static Deck GenerateFullDeck(int decks = 1) {
        Deck cards = new();

        var allFaces = (CardSuit[])Enum.GetValues(typeof(CardSuit));

        foreach (CardSuit suit in allFaces) {
            for (int value = 1; value <= 13; value++) {
                for (int i = 0; i < decks; i++) {
                    cards.AddLast(new Card(suit, value));
                }
            }
        }

        cards.Shuffle();

        return cards;
    }

    public Card? PlayCard() {
        if (Count == 0) return null;

        Card playedCard = First.Value;
        RemoveFirst();
        return playedCard;
    }

    /// <summary>
    /// Shuffles the deck in-place.
    /// </summary>
    public void Shuffle() {
        // https://stackoverflow.com/questions/35628552/shuffle-a-linkedlist
        // Take all the nodes, and put them into an array. 
        if (Count < 2) return;
        Random generator = new Random();
        var result = new LinkedListNode<Card>[Count];
        int i = 0;
        for (var node = First; node != null; node = node.Next) {
            int j = generator.Next(i + 1);
            if (i != j) result[i] = result[j];
            result[j] = node;
            i++;
        }

        // Then overwrite the linked list with the shuffled list. 
        Clear();
        AddFirst(result[0]);
        for (i = 1; i < result.Length; i++) {
            AddAfter(result[i-1], result[i]);
        }
    }

    /// <summary>
    /// Take the current deck, and deal it into a numnber of smaller decks
    /// for some number of players. 
    /// Each card from the deck is dealt to each player sequentially until all
    /// the cards have been dealt out. In the event the number of cards is not 
    /// evenly divisible, the first few dealt decks will have an additional 
    /// card compared to the final deck. 
    /// This operation preserves this deck and its order.
    /// </summary>
    /// <param name="decks">
    ///     The number of decks that this deck should be dealt into.
    /// </param>
    /// <returns>
    ///     An array of decks, each containing at least <c>Count / decks</c> 
    ///     (rounded down) cards. 
    /// </returns>
    public Deck[] DealAll(int decks = 2) {
        Deck[] splitDecks = new Deck[decks];
        int deck;
        for (deck = 0; deck < decks; deck++) {
            splitDecks[deck] = new Deck();
        }

        deck = 0;
        for (var card = First; card != null; card = card.Next) {
            splitDecks[deck++].AddLast(card.Value);
            deck %= decks;
        }

        return splitDecks;
    }

    public void TakeCard(Card? card) {
        if (card == null) return;
        AddLast((Card)card);
    }

    /// <summary>
    /// Takes all the cards in the provided pile, and appends them to the end
    /// of this one. This is an in-place operation for this Deck that does not
    /// affect the contents of the pile the cards are taken from. 
    /// </summary>
    /// <param name="pile">
    ///     A second "deck" of cards to add to the end of this one. 
    /// </param>
    public void TakeAll(Deck pile) {
        var card = pile.First;
        for (int i = 0; i < pile.Count; i++) {
            AddLast(card.Value);
            card = card.Next;
        }
    }
}