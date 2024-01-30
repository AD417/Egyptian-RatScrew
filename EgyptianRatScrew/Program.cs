// This file does not need to be edited.

using System;
using EgyptianRatScrew.CardGame;

using var game = new EgyptianRatScrew.Game1();
game.Run();

// int PLAYERS = 4;
// var manager = new Manager(playerCount: PLAYERS, decks: 2);
// GameState state = GameState.NORMAL;
// for (int i = 0; i < PLAYERS; i++) {
//     Console.WriteLine($"Player {i} has {manager.players[i].Count} cards");
// }
// while (true) {
//     Console.WriteLine($"Player {manager.Turn} to play.");
//     string[] input = Console.ReadLine().Split(" ");

//     if (input.Length < 2) continue;
//     if (!int.TryParse(input[1], out int playerNum)) continue;
//     if (playerNum > 4 || playerNum < 0) continue;

//     if (input[0] == "slap" || input[0] == "s") {
//         state = manager.SlapPile(playerNum);
//         if (state == GameState.PILE_TAKEN) {
//             Console.WriteLine($"Player {playerNum} takes the pile!");
//             for (int i = 0; i < PLAYERS; i++) {
//                 Console.WriteLine($"Player {i} has {manager.players[i].Count} cards");
//             }
//             continue;
//         } else if (state == GameState.PENALTY) {
//             Console.WriteLine(
//                 $"Illegal move. Player {playerNum} burns a card."
//             );
//         }
//     }
//     if (input[0] == "play" || input[0] == "p") {
//         state = manager.PlayCard(playerNum);
//         if (state == GameState.PENALTY) {
//             Console.WriteLine(
//                 $"Playing out of turn. Player {playerNum} burns a card."
//             );
//         }
//     }
//     if (manager.Pile.Count > 0) {
//         //Console.WriteLine($"The first card played was the {manager.pile.First?.Value}");
//         Console.WriteLine($"The last card played was the {manager.LastCard()}");
//     }
//     if (state == GameState.CHALLENGE) {
//         Console.WriteLine($"Player {manager.Turn} has {manager.ChallengeAttemptsLeft} chances left!");
//     }
//     if (state == GameState.CHALLENGE_FAILED) {
//         Console.WriteLine($"Player {manager.PlayerChallenging} takes the pile!");
//         for (int i = 0; i < PLAYERS; i++) {
//             Console.WriteLine($"Player {i} has {manager.players[i].Count} cards");
//         }
//     }
// }