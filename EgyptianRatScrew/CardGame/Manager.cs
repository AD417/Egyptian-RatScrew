using System.Collections.Generic;
using EgyptianRatScrew.CardGame.Cards;

namespace EgyptianRatScrew.CardGame;

public class Manager {
    /// <summary>
    /// The number of players.
    /// </summary>
    public int PlayerCount { get; private set; } = 2;
    /// <summary>
    /// How many decks we are playing with.
    /// </summary>
    public int Decks { get; private set; }= 1;

    /// <summary>
    /// Whose turn it is. 0 indexed; max is <c>playerCount - 1</c>
    /// </summary>
    public int Turn { get; private set; } = 0;

    /// <summary>
    /// The pile that all players play cards onto.
    /// </summary>
    private Deck pile = new();

    /// <summary>
    /// The pile where penalty cards accumulate if people perform an illegal
    /// action, eg: slapping the deck at the wrong time, playing out of turn.
    /// </summary>
    private Deck burn = new();

    /// <summary>
    /// The decks of all the players. 
    /// </summary>
    private Deck[] players;

    public Manager(int playerCount = 2, int decks = 1) {
        PlayerCount = playerCount;
        Decks = decks;

        Reset(PlayerCount, decks);
    }

    public void Reset(int playerCount = 0, int decks = 0) {
        if (playerCount <= 0) playerCount = PlayerCount;
        else PlayerCount = playerCount;

        if (decks <= 0) decks = Decks;
        else Decks = decks;

        Turn = 0;

        pile = new Deck();
        burn = new Deck();

        Deck allCards = Deck.GenerateFullDeck(decks);
        players = allCards.DealAll(playerCount);
    }

    public bool CanSlapPile() {
        LinkedListNode<Card> pileIndex = pile.Last;
        Card top = pileIndex.Value;

        pileIndex = pileIndex.Previous;
        if (top.SameValueAs(pileIndex.Value)) return true;

        pileIndex = pileIndex.Previous;
        if (top.SameValueAs(pileIndex.Value)) return true;

        return false;
    }

    public bool PlayCard(int playerId) {
        Card? played = players[playerId].PlayCard();
        if (played == null) return false;

        if (Turn == playerId) {
            pile.TakeCard(played);
            Turn++;
            Turn %= PlayerCount;
            return true;
        } else {
            burn.TakeCard(played);
            return false;
        }
    }

    public bool SlapPile(int playerId) {
        Deck player = players[playerId];
        if (CanSlapPile()) {
            player.TakeAll(pile);
            player.TakeAll(burn);
            return true;
        } else {
            Card? penalty = player.PlayCard();
            burn.TakeCard(penalty);
            return false;
        }
    }
}