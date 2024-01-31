using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using Devcade;
using EgyptianRatScrew.CardGame;
using EgyptianRatScrew.DevcadeExtension;
using System.Collections.Generic;

namespace EgyptianRatScrew
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		
		/// <summary>
		/// Stores the window dimensions in a rectangle object for easy use
		/// </summary>
		private Rectangle windowSize;

		/// <summary>
		/// The number of players in this game.
		/// </summary>
		private static readonly int PLAYERS = 4;

		/// <summary>
		/// The internal game state. 
		/// </summary>
		private readonly Manager manager = new Manager(PLAYERS, 1);

		/// <summary>
		/// The list of all displayed Cards. Allows all the played cards
		/// this round to be rendered on top of each other.
		/// </summary>
		private List<CardAnimation> displayedCards = new();

		/// <summary>
		/// The amount of time since the last player played a card. Balances
		/// the game by preventing a player from spamming cards, so someone can
		/// actually slap the deck between plays. 
		/// </summary>
		private TimeSpan timeSinceLastAction = TimeSpan.Zero;

		
		/// <summary>
		/// Game constructor
		/// </summary>
		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = false;
		}

		/// <summary>
		/// Performs any setup that doesn't require loaded content before the first frame.
		/// </summary>
		protected override void Initialize()
		{
			// Sets up the input library
			Input.Initialize();
			//Persistence.Init(); Uncomment if using the persistence section for save and load

			// Set window size if running debug (in release it will be fullscreen)
			#region
#if DEBUG
			_graphics.PreferredBackBufferWidth = 420;
			_graphics.PreferredBackBufferHeight = 980;
			_graphics.ApplyChanges();
#else
			_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
			_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
			_graphics.ApplyChanges();
#endif	
			#endregion

			windowSize = GraphicsDevice.Viewport.Bounds;
			
			// TODO: Add your initialization logic here
			SetupInputs();
			
			base.Initialize();
		}

		private void SetupInputs() {
			// Q  W  E  R
			//  A  S  D  F
			InputManager.MapKeyToButton(Keys.Q, Input.ArcadeButtons.A1);
			InputManager.MapKeyToButton(Keys.W, Input.ArcadeButtons.A2);
			InputManager.MapKeyToButton(Keys.E, Input.ArcadeButtons.A3);
			InputManager.MapKeyToButton(Keys.R, Input.ArcadeButtons.A4);
			InputManager.MapKeyToButton(Keys.A, Input.ArcadeButtons.B1);
			InputManager.MapKeyToButton(Keys.S, Input.ArcadeButtons.B2);
			InputManager.MapKeyToButton(Keys.D, Input.ArcadeButtons.B3);
			InputManager.MapKeyToButton(Keys.F, Input.ArcadeButtons.B4);

			InputManager.SetOnPress(Input.ArcadeButtons.A1, () => Slap(0));
			InputManager.SetOnPress(Input.ArcadeButtons.A2, () => Slap(1));
			InputManager.SetOnPress(Input.ArcadeButtons.A3, () => Slap(2));
			InputManager.SetOnPress(Input.ArcadeButtons.A4, () => Slap(3));

			InputManager.SetOnPress(Input.ArcadeButtons.B1, () => Play(0));
			InputManager.SetOnPress(Input.ArcadeButtons.B2, () => Play(1));
			InputManager.SetOnPress(Input.ArcadeButtons.B3, () => Play(2));
			InputManager.SetOnPress(Input.ArcadeButtons.B4, () => Play(3));
		}

		/// <summary>
		/// Performs any setup that requires loaded content before the first frame.
		/// </summary>
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			Asset.LoadContent(Content);
		}

		private bool canPlay() {
			return timeSinceLastAction > TimeSpan.FromSeconds(0.2);
		}

		private void Slap(int playerId) {
			GameState result = manager.SlapPile(playerId);
			DisplayOutput(playerId, result);

			if (result == GameState.PILE_TAKEN) displayedCards.Clear();
		}

		private void Play(int playerId) {
			if (!canPlay()) return;
			GameState result = manager.PlayCard(playerId);
			DisplayOutput(playerId, result);

			if (result != GameState.PENALTY) {
				displayedCards.Add(new CardAnimation(manager.LastCard(), Anim.PLAYER_POSITION[playerId]));
			}
			timeSinceLastAction = TimeSpan.Zero;
		}

		private void DisplayOutput(int playerId, GameState state) {
			if (state == GameState.PILE_TAKEN) {
				Console.WriteLine($"Player {playerId} takes the pile!");
				for (int i = 0; i < PLAYERS; i++) {
					Console.WriteLine($"Player {i} has {manager.players[i].Count} cards");
				}
				Console.WriteLine($"Player {manager.Turn} to play.");
				return;
			}
			if (state == GameState.PENALTY) {
				Console.WriteLine(
					$"Illegal move. Player {playerId} burns a card."
				);
				return;
			}
			if (manager.Pile.Count > 0) {
        		Console.WriteLine($"The last card played was the {manager.LastCard()}");
			}
			if (state == GameState.CHALLENGE) {
				Console.WriteLine($"Player {manager.Turn} has {manager.ChallengeAttemptsLeft} chances left!");
			}
			// if (state == GameState.CHALLENGE_FAILED) {
			// 	Console.WriteLine($"Player {manager.PlayerChallenging} takes the pile!");
			// 	for (int i = 0; i < PLAYERS; i++) {
			// 		Console.WriteLine($"Player {i} has {manager.players[i].Count} cards");
			// 	}
			// }
			Console.WriteLine($"Player {manager.Turn} to play.");
		}


		/// <summary>
		/// Your main update loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Update(GameTime gameTime)
		{
			Input.Update(); // Updates the state of the input library

			// Exit when both menu buttons are pressed (or escape for keyboard debugging)
			// You can change this but it is suggested to keep the keybind of both menu
			// buttons at once for a graceful exit.
			if (Keyboard.GetState().IsKeyDown(Keys.Escape) ||
				(Input.GetButton(1, Input.ArcadeButtons.Menu) &&
				Input.GetButton(2, Input.ArcadeButtons.Menu)))
			{
				Exit();
			}

			// TODO: Add your update logic here
			InputManager.TickActions();
			timeSinceLastAction += gameTime.ElapsedGameTime;

			base.Update(gameTime);
		}

		/// <summary>
		/// Your main draw loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			
			// Batches all the draw calls for this frame, and then performs them all at once
			_spriteBatch.Begin();
			// TODO: Add your drawing code here

			for (int i = 0; i < PLAYERS; i++) {
				_spriteBatch.Draw(Asset.Decks, Anim.PLAYER_POSITION[i], Asset.DeckPosition(i), Color.White);
			}

			foreach (CardAnimation anim in displayedCards) {
				anim.Draw(_spriteBatch);
				anim.Tick(gameTime.ElapsedGameTime);
			}
			
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}