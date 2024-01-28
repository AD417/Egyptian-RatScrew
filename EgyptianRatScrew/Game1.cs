using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;

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

		private int x = 0;
		private int y = 0;

		
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
			
			// TODO: Add your initialization logic here
			InputManager.SetWhileHeld(Input.ArcadeButtons.StickUp, () => y--);
			InputManager.SetWhileHeld(Input.ArcadeButtons.StickDown, () => y++);
			InputManager.SetWhileHeld(Input.ArcadeButtons.StickLeft, () => x--);
			InputManager.SetWhileHeld(Input.ArcadeButtons.StickRight, () => x++);
			InputManager.SetOnRelease(Input.ArcadeButtons.A1, () => { x = 0; y = 0; });
			InputManager.MapKeyToButton(Keys.W, Input.ArcadeButtons.StickUp);
			InputManager.MapKeyToButton(Keys.A, Input.ArcadeButtons.StickLeft);
			InputManager.MapKeyToButton(Keys.S, Input.ArcadeButtons.StickDown); 
			InputManager.MapKeyToButton(Keys.D, Input.ArcadeButtons.StickRight);
			InputManager.MapKeyToButton(Keys.R, Input.ArcadeButtons.A1);

			//InputManager.doThing = () => y++;

			windowSize = GraphicsDevice.Viewport.Bounds;
			
			base.Initialize();
		}

		/// <summary>
		/// Performs any setup that requires loaded content before the first frame.
		/// </summary>
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			Asset.LoadContent(Content);
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
			_spriteBatch.Draw(Asset.PlayerCircle, new Vector2(x, y), Color.White);
			
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}