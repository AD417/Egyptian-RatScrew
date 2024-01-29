// This file does not need to be edited.

using System;
using EgyptianRatScrew.CardGame;

if (false) {
    using var game = new EgyptianRatScrew.Game1();
    game.Run();
} else {
    var manager = new Manager();
    Console.WriteLine($"Player 0 has {manager.players[0].Count} cards");
    Console.WriteLine($"Player 1 has {manager.players[1].Count} cards");
    while (true) {
        Console.WriteLine($"Player {manager.Turn} to play.");
        string[] input = Console.ReadLine().Split(" ");
        int playerNum;
        if (!int.TryParse(input[1], out playerNum)) continue;
        if (playerNum > 1 || playerNum < 0) continue;
        if (input[0] == "slap" || input[0] == "s") {
            if (manager.SlapPile(playerNum)) {
                Console.WriteLine($"Player {playerNum} takes the pile!");
                Console.WriteLine($"Player 0 has ${manager.players[0].Count} cards");
                Console.WriteLine($"Player 1 has ${manager.players[1].Count} cards");
                continue;
            } else {
                Console.WriteLine(
                    $"Illegal move. Player {playerNum} burns a card."
                );
            }
        }
        if (input[0] == "play" || input[0] == "p") {
            if (!manager.PlayCard(playerNum)) {
                Console.WriteLine(
                    $"Playing out of turn. Player {playerNum} burns a card."
                );
            }
        }
        if (manager.pile.Count > 0) {
            //Console.WriteLine($"The first card played was the {manager.pile.First?.Value}");
            Console.WriteLine($"The last card played was the {manager.LastCard()}");
        }
    }
}