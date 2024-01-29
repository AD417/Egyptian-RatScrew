namespace EgyptianRatScrew.CardGame;

public enum GameState {
    /// <summary>
    /// When a card is played and the game continues as normal.
    /// </summary>
    NORMAL,
    /// <summary>
    /// When an illegal play is made; usually means the last to play has burned
    /// a card.
    /// </summary>
    PENALTY,
    /// <summary>
    /// When a player takes the pile, for some generic reason. Usually occurs
    /// when a player slaps the pile.
    /// </summary>
    PILE_TAKEN,
    /// <summary>
    /// Occurs when face cards indicate challenges and a player plays a face
    /// card. 
    /// </summary>
    CHALLENGE,
    /// <summary>
    /// Occurs when face cards indicate challenges and a player has run out of
    /// chances to play another face card. Indicates the challenger takes all
    /// the cards. 
    /// </summary>
    CHALLENGE_FAILED
}