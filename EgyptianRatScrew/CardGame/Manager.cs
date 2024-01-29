using System;
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
    public Deck Pile { get; private set; } = new();

    /// <summary>
    /// The pile where penalty cards accumulate if people perform an illegal
    /// action, eg: slapping the deck at the wrong time, playing out of turn.
    /// </summary>
    private Deck burn = new();

    /// <summary>
    /// The decks of all the players. 
    /// </summary>
    public Deck[] players { get; private set; }

    /// <summary>
    /// Whether or not a face card challenge is going on. 
    /// </summary>
    public bool InChallenge { get; private set; }= false;
    /// <summary>
    /// If a face card challenge is going on, who the challenger is. The
    /// challenged is stored as <c>Turn</c>.
    /// </summary>
    public int PlayerChallenging { get; private set; }= 0;
    /// <summary>
    /// How many attempts the challenged player has to play a face card.
    /// </summary>
    public int ChallengeAttemptsLeft {get; private set; }= int.MaxValue;

    public Manager(int playerCount = 2, int decks = 1) {
        PlayerCount = playerCount;
        Decks = decks;

        Reset(PlayerCount, decks);
    }

    /// <summary>
    /// Reset the entire game, including the pile and burn pile, all players'
    /// hands, and even setup variables (passed as parameters). 
    /// </summary>
    /// <param name="playerCount">
    ///     The number of players in the game. If unspecified or less than 1, 
    ///     uses the same values as the last game.
    /// </param>
    /// <param name="decks">
    ///     The number of decks to use. The number of cards in play will be 
    ///     <c>52 * decks</c>. If unspecified or not positive, uses the same
    ///     value as the last game. 
    /// </param>
    public void Reset(int playerCount = 0, int decks = 0) {
        if (playerCount <= 1) playerCount = PlayerCount;
        else PlayerCount = playerCount;

        if (decks <= 0) decks = Decks;
        else Decks = decks;

        Turn = 0;

        ResetPiles();

        Deck allCards = Deck.GenerateFullDeck(decks);
        players = allCards.DealAll(playerCount);
    }

    /// <summary>
    /// Reset the played and burn piles. Usually used after a player picks up
    /// the cards, as that operation does not affect the piles.
    /// </summary>
    public void ResetPiles() {
        Pile = new Deck();
        burn = new Deck();
    }

    /// <summary>
    /// Get the last card played to the pile(the card currently on top).
    /// </summary>
    /// <returns>
    ///     The card on top of the pile.
    /// </returns>
    public Card LastCard() {
        return Pile.Last.Value;
    }

    /// <summary>
    /// Set <c>Turn</c> to the next player to act after the current turn. This
    /// skips players who have run out of cards. 
    /// </summary>
    private void UpdateTurn() {
        do { 
            Turn++; 
            Turn %= PlayerCount; 
        } while (players[Turn].Count == 0);
    }

    /// <summary>
    /// Determine if the pile is in a state where it can be slapped. The exact
    /// criteria for the pile to be slapped are dependent on various factors.
    /// </summary>
    /// <returns>
    ///     True iff the pile can be slapped in its current configuration. 
    ///     False otherwise.
    /// </returns>
    public bool CanSlapPile() {
        // Contains the full Brandreth ruleset. 
        // TODO: prune this ruleset based on what the players want.
        if (Pile.Count == 0) return false;
        LinkedListNode<Card> pileIndex = Pile.Last;
        Card top = pileIndex.Value;

        // SECOND FROM TOP.
        if (pileIndex.Previous == null) return false;
        pileIndex = pileIndex.Previous;
        Card second = pileIndex.Value;
        // Doubles
        if (top.SameValueAs(second)) return true;
        // King/Queen
        if (top.MakesKingQueen(second)) return true;
        // Sixty-Nine
        if (top.MakesSixtyNine(second)) return true;

        // THIRD FROM TOP.
        if (pileIndex.Previous == null) return false;
        pileIndex = pileIndex.Previous;
        Card third = pileIndex.Value;
        // Sandwich
        if (top.SameValueAs(third)) return true;
        // Divorce
        if (top.MakesKingQueen(third)) return true;
        // Sequence
        if (top.MakesSequence(second, third)) return true;

        // BOTTOM CARD
        Card bottom = Pile.First.Value;
        // Top-bottom
        if (top.SameValueAs(bottom)) return true;

        return false;
    }

    /// <summary>
    /// Attempt to play a card to the pile, if it is this player's turn. If it
    /// is not, then the player must burn a card instead.
    /// </summary>
    /// <param name="playerId">
    ///     The ID of the player to penalize. Assumed to be a valid player ID.
    /// </param>
    /// <returns></returns>
    public GameState PlayCard(int playerId) {
        Deck player = players[playerId];
        if (Turn != playerId) {
            burn.TakeCard(player.PlayCard());
            return GameState.PENALTY;
        }
        if (player.Count == 0) {
            throw new InvalidOperationException(
                $"Player {playerId} ran out of cards; unable to play."
            );
        }

        Card played = player.PlayCard().Value;
        Pile.TakeCard(played);
        UpdateTurn();

        // if (played.IsChallengeCard()) {
        //     InChallenge = true;
        //     PlayerChallenging = Turn;
        //     ChallengeAttemptsLeft = played.ChallengesAllowed();
        //     UpdateTurn();
        // }
        // if (!InChallenge) {
        //     UpdateTurn();
        // }

        return GameState.NORMAL;
    }

    /// <summary>
    /// Attempt to slap the pile to take all the cards. If the pile's status
    /// allows it to be slapped, then the player takes all the cards.
    /// </summary>
    /// <param name="playerId">
    ///     The ID of the player to penalize. Assumed to be a valid player ID.
    /// </param>
    /// <returns></returns>
    public GameState SlapPile(int playerId) {
        Deck player = players[playerId];
        if (CanSlapPile()) {
            TakePile(playerId);
            Turn = playerId;
            return GameState.PILE_TAKEN;
        } else {
            Card? penalty = player.PlayCard();
            burn.TakeCard(penalty);
            return GameState.PENALTY;
        }
    }

    /// <summary>
    /// Have a player take the entirety of the played and burned cards. This 
    /// usually happens when they slap the deck or win a challenge against 
    /// another player.
    /// </summary>
    /// <param name="playerId">
    ///     The ID of the player to penalize. Assumed to be a valid player ID.
    /// </param>
    public void TakePile(int playerId) {
        Deck player = players[playerId];
        player.TakeAll(Pile);
        player.TakeAll(burn);
        ResetPiles();
    }

    /// <summary>
    /// Penalize a player. This involves the player playing a card to the
    /// burn pile. No one gets to see this card.
    /// </summary>
    /// <param name="playerId">
    ///     The ID of the player to penalize. Assumed to be a valid player ID.
    /// </param>
    public void Penalize(int playerId) {
        burn.TakeCard(players[playerId].PlayCard());
    }
}