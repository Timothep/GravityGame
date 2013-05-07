using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Diagnostics;

namespace ParticleGame
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		// Drawing
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteFont font;
		Texture2D dot;
		Texture2D circle;
		Texture2D sdot;


		// Framerate
		Stopwatch stopwatch;
		float framerate;
		List<float> framerates = new List<float>();

		// Screen & Window dimensions
		int sWidth;
		int sHeight;
		int vWidth, vHeight;

		// Mouse input
		int xPan, yPan;
		Vector2 startPoint;
		Vector2 startPosition;

		// Various booleans
		bool isDismissed;
		bool paused;
		bool isConsoleVisible;
		bool inputLocked;
		string consoleInput;

		// Game objects
		private Universe universe;
		private InputListener listener;
		private CommandHandler cHandler;
		private SettingsFileManager sfm;

		public Game1()
		{
			sfm = SettingsFileManager.GetSettingsFileManager();

			InitializeWindow();


			Content.RootDirectory = "Content";
			this.IsMouseVisible = true;

			cHandler = new CommandHandler(this);
			stopwatch = new Stopwatch();
			universe = new Universe();
			listener = new InputListener();

			startPoint = new Vector2();
			startPosition = new Vector2();
		}

		private void InitializeWindow()
		{
			bool fullscreen = bool.Parse(sfm.GetSetting(Settings.FULLSCREEN));

			if (fullscreen)
			{
				sWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				sHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			}
			else
			{
				sWidth = int.Parse(sfm.GetSetting(Settings.WINDOW_WIDTH));
				sHeight = int.Parse(sfm.GetSetting(Settings.WINDOW_HEIGHT));
			}
			graphics = new GraphicsDeviceManager(this);

			graphics.PreferredBackBufferWidth = vWidth = sWidth;
			graphics.PreferredBackBufferHeight = vHeight = sHeight;
			graphics.IsFullScreen = fullscreen;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			BlendState blendState = new BlendState()
			{
				ColorSourceBlend = Blend.SourceAlpha,
				AlphaSourceBlend = Blend.SourceAlpha,
				AlphaDestinationBlend = Blend.InverseSourceAlpha,
				ColorDestinationBlend = Blend.InverseSourceAlpha, // Required for Reach profile
			};
			GraphicsDevice.BlendState = blendState;

			base.Initialize();

			for (int i = 0; i < sWidth; i++)
			{
				framerates.Add(0f);
			}
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			circle = Content.Load<Texture2D>("circle");
			dot = Content.Load<Texture2D>("dot");
			font = Content.Load<SpriteFont>("font");
			sdot = Content.Load<Texture2D>("smoothdot");

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			DebugFileManager.GetDebugFileManager().Close();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (Universe.instance.IsUpdated)
			{
				universe = Universe.instance;
			}

			HandleInput();

			if(!paused)
				universe.Update();

			base.Update(gameTime);
		}
		/// <summary>
		/// Handle the user input.
		/// </summary>
		private void HandleInput()
		{

			listener.Update();
			MouseState mState = Mouse.GetState();

			// Allows the game to exit
			if (listener.CheckKeyRelease(Keys.Escape))
				this.Exit();

			if(!isConsoleVisible && listener.CheckKeypress(Keys.L))
			{
				inputLocked = !inputLocked;
			}

			if (inputLocked)
			{
				return;
			}

			// Removes planets if middle mouse button is pressed
			if (isConsoleVisible)
			{
				if (listener.CheckMousePress("middle"))
				{
					universe.Clear();
				}
				if (listener.CheckKeypress(Keys.Back))
				{
					if (consoleInput.Length > 0)
					{
						consoleInput = consoleInput.Substring(0, consoleInput.Length - 1);
					}
				}
				Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

				foreach (Keys key in pressedKeys)
				{
					if(!listener.CheckKeypress(key))
					{
						continue;
					}
					// Ugly and hacky way of determining which keys were pressed. 
					#region keyswitchblock
					switch (key)
					{
						case Keys.OemPlus:
							consoleInput += "+";
							break;
						case Keys.OemMinus:
							consoleInput += "_";
							break;
						case Keys.A:
							consoleInput += "a";
							break;
						case Keys.B:
							consoleInput += "b";
							break;
						case Keys.C:
							consoleInput += "c";
							break;
						case Keys.D:
							consoleInput += "d";
							break;
						case Keys.E:
							consoleInput += "e";
							break;
						case Keys.F:
							consoleInput += "f";
							break;
						case Keys.G:
							consoleInput += "g";
							break;
						case Keys.H:
							consoleInput += "h";
							break;
						case Keys.I:
							consoleInput += "i";
							break;
						case Keys.J:
							consoleInput += "j";
							break;
						case Keys.K:
							consoleInput += "k";
							break;
						case Keys.L:
							consoleInput += "l";
							break;
						case Keys.M:
							consoleInput += "m";
							break;
						case Keys.N:
							consoleInput += "n";
							break;
						case Keys.O:
							consoleInput += "o";
							break;
						case Keys.P:
							consoleInput += "p";
							break;
						case Keys.Q:
							consoleInput += "q";
							break;
						case Keys.R:
							consoleInput += "r";
							break;
						case Keys.S:
							consoleInput += "s";
							break;
						case Keys.T:
							consoleInput += "t";
							break;
						case Keys.U:
							consoleInput += "u";
							break;
						case Keys.V:
							consoleInput += "v";
							break;
						case Keys.W:
							consoleInput += "w";
							break;
						case Keys.X:
							consoleInput += "x";
							break;
						case Keys.Y:
							consoleInput += "y";
							break;
						case Keys.Z:
							consoleInput += "z";
							break;
						case Keys.Space:
							consoleInput += " ";
							break;
						case Keys.D0:
							consoleInput += "0";
							break;
						case Keys.D1:
							consoleInput += "1";
							break;
						case Keys.D2:
							consoleInput += "2";
							break;
						case Keys.D3:
							consoleInput += "3";
							break;
						case Keys.D4:
							consoleInput += "4";
							break;
						case Keys.D5:
							consoleInput += "5";
							break;
						case Keys.D6:
							consoleInput += "6";
							break;
						case Keys.D7:
							consoleInput += "7";
							break;
						case Keys.D8:
							consoleInput += "8";
							break;
						case Keys.D9:
							consoleInput += "9";
							break;
					}
					#endregion
				}
				if (listener.CheckKeypress(Keys.Enter))
				{
					// Handle commands, clear console, hide console.
					cHandler.ProcessCommand(consoleInput);
					consoleInput = "";
					isConsoleVisible = false;
				}
			}
			else
			{
				if (listener.CheckMousePress("middle") || listener.CheckKeypress(Keys.C))
				{
					universe.Clear();
				}
				if (listener.CheckKeypress(Keys.P))
				{
					paused = !paused;
				}
			}
			

			// Displays the projected planet path if the mouse button is currently being pressed
			if (Mouse.GetState().LeftButton == ButtonState.Pressed)
				universe.CreatePath(new Vector2(startPoint.X - xPan, vHeight - startPoint.Y + yPan), new Vector2((mState.X - startPoint.X) / 100, (startPoint.Y - mState.Y) / 100), mState.ScrollWheelValue / 120 + 2, 1200);
			
			// Saves the starting position of the mouse once the mouse has been pressed, so that a planet can be spawned at this position on mouse button release.
			if (listener.CheckMousePress("left"))
			{
				startPoint.X = mState.X;
				startPoint.Y = mState.Y;
			}
			// Add a planet once the left button has been released. Also remove the projected planet path.
			else if (listener.CheckMouseRelease("left"))
			{
				universe.AddPlanet(new Vector2(startPoint.X - xPan, vHeight - startPoint.Y + yPan), new Vector2((mState.X - startPoint.X) / 100, (startPoint.Y - mState.Y) / 100), mState.ScrollWheelValue / 120 + 2);
				universe.ClearPath();
			}
			// Save the starting position for panning the camera
			if (listener.CheckMousePress("right"))
			{
				startPosition.X = mState.X - xPan;
				startPosition.Y = mState.Y - yPan;
			}
			// Pan the camera
			if (Mouse.GetState().RightButton == ButtonState.Pressed)
			{
				xPan = mState.X - (int)startPosition.X;
				yPan = mState.Y - (int)startPosition.Y;
			}
			// Dismiss the message when space is pressed
			if (listener.CheckKeypress(Keys.Space))
				isDismissed = true;
			if(listener.CheckKeypress(Keys.OemTilde))
			{
				if (isConsoleVisible)
					consoleInput = "";
				isConsoleVisible = !isConsoleVisible;
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			// Calculate the time it took to draw the previous frame
			stopwatch.Stop();
			float frameTime = (float)stopwatch.ElapsedTicks / (float)Stopwatch.Frequency;
			stopwatch.Reset();
			stopwatch.Start();

			// Add the current frametime to the framerate graph
			framerate = 1f / frameTime;
			framerates.Add(frameTime*1000f);
			framerates.RemoveAt(0);

			// Clear the screen
			GraphicsDevice.Clear(Color.Black); 

			// DRAWING CODE GOES BELOW THIS STATEMENT
			spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);

			// If message has not been dismissed yet:
			if (!isDismissed && SettingsFileManager.GetSettingsFileManager().GetSetting(Settings.SHOW_HELP_MESSAGE).Equals("true"))
			{
				spriteBatch.DrawString(font, "Welcome to Baggykiin's gravity game! To get started, here are some basic instructions:", new Vector2((sWidth / 2)-250 + xPan, (sHeight / 2) - 110 + yPan), Color.Orange);
				spriteBatch.DrawString(font, "- Add PLANETS by pressing the LEFT MOUSE BUTTON. CLICK and DRAG to add a planet and immediately give it a VELOCITY and DIRECTION.", new Vector2((sWidth / 2) - 230 + xPan, (sHeight / 2) - 80 + yPan), Color.Orange);
				spriteBatch.DrawString(font, "- You can PAN the camera by HOLDING the RIGHT MOUSE BUTTON and MOVING THE MOUSE.", new Vector2((sWidth / 2) - 230 + xPan, (sHeight / 2) - 60 + yPan), Color.Orange);
				spriteBatch.DrawString(font, "- SCROLLING your mouse wheel allows you to CHANGE THE SIZE (and therefore mass) of the NEXT PLANET you'll be adding.", new Vector2((sWidth / 2) - 230 + xPan, (sHeight / 2) - 40 + yPan), Color.Orange);
				spriteBatch.DrawString(font, "- You can REMOVE ALL PLANETS and paths by pressing the MIDDLE MOUSE BUTTON or the C KEY on your keyboard.", new Vector2((sWidth / 2) - 230 + xPan, (sHeight / 2) - 20 + yPan), Color.Orange);
				spriteBatch.DrawString(font, "To DISMISS this message, press SPACE.", new Vector2((sWidth / 2) - 250 + xPan, (sHeight / 2) + 10 + yPan), Color.Orange);
			}

			// Draw current framerate and planet size
			spriteBatch.DrawString(font, "Framerate: " + Math.Round(framerate) + " FPS", new Vector2(10, 10), Color.White);
			spriteBatch.DrawString(font, "Current planet size: " + Mouse.GetState().ScrollWheelValue / 120 + 2, new Vector2(150, 10), Color.White);

			// If paused, display that the game is paused
			if(paused)
				spriteBatch.DrawString(font, "PAUSED", new Vector2(330, 11), Color.White);

			if (isConsoleVisible)
			{
				spriteBatch.DrawString(font, "CONSOLE>: " + consoleInput, new Vector2(400, 10), Color.White);
			}

			// Draw framerate graph
			if (sfm.GetSetting(Settings.SHOW_FRAMERATE_GRAPH).Equals("true"))
			{
				for (int i = 0; i < framerates.Count; i++)
				{
					spriteBatch.Draw(dot, new Rectangle(0 + i, sHeight - (int)Math.Round(framerates[i]), 1, (int)Math.Round(framerates[i])), Color.Red);
				}
			}

			// Draw pathpoints
			foreach (PathPoint pp in universe.pathPoints)
			{
				//spriteBatch.Draw(sdot, new Rectangle((int)pp.X + xPan - 2, vHeight - (int)pp.Y + yPan - 3, 5, 5), pp.Color);
				spriteBatch.Draw(circle, new Rectangle((int)pp.X + xPan - 1, vHeight - (int)pp.Y + yPan, 1, 1), pp.Color);
			}

			// Draw projected planet path (if existing)
			foreach (PathPoint pp in universe.ghosts)
			{
				spriteBatch.Draw(circle, new Rectangle((int)pp.X + xPan, vHeight - (int)pp.Y + yPan, 1, 1), pp.Color);
			}

			// Draw planets
			for(int i = 0; i < universe.planets.Count; i++)
			{
				GravityObject go = universe.planets[i];
				Color temp = new Color();
				int offset = 2;

				temp.R = go.Color.R;
				temp.G = go.Color.G;
				temp.B = go.Color.B;
				temp.A = 255;

				spriteBatch.Draw(circle, new Rectangle(

						(int)(go.Coordinates.X - go.Radius + xPan), 
						vHeight - (int)(go.Coordinates.Y + go.Radius - yPan), 
						(int) go.Diameter,
						(int) go.Diameter

					), go.Color);
				temp.A = 100;
				spriteBatch.Draw(circle, new Rectangle(

					(int)(go.Coordinates.X - (go.Radius + offset / 2) + xPan),
					vHeight - (int)(go.Coordinates.Y + (go.Radius + offset / 2) - yPan),
					(int)go.Diameter + offset,
					(int)go.Diameter + offset

				), temp);

				offset = 4;
				temp.A = 75;
				spriteBatch.Draw(circle, new Rectangle(

					(int)(go.Coordinates.X - (go.Radius + offset / 2) + xPan),
					vHeight - (int)(go.Coordinates.Y + (go.Radius + offset / 2) - yPan),
					(int)go.Diameter + offset,
					(int)go.Diameter + offset

				), temp);
				offset = 6;
				temp.A = 40;
				spriteBatch.Draw(circle, new Rectangle(

					(int)(go.Coordinates.X - (go.Radius + offset / 2) + xPan),
					vHeight - (int)(go.Coordinates.Y + (go.Radius + offset / 2) - yPan),
					(int)go.Diameter + offset,
					(int)go.Diameter + offset

				), temp);

				offset = 8;
				temp.A = 20;
				spriteBatch.Draw(circle, new Rectangle(

					(int)(go.Coordinates.X - (go.Radius + offset / 2) + xPan),
					vHeight - (int)(go.Coordinates.Y + (go.Radius + offset / 2) - yPan),
					(int)go.Diameter + offset,
					(int)go.Diameter + offset

				), temp);

				go.DisplayInfo(spriteBatch, font);
			}

			// Draw planetline if mouse is pressed
			if (Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				DrawLine(1, Color.Red, startPoint, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}
		private void DrawLine(float width, Color color, Vector2 point1, Vector2 point2)
		{
			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			float length = Vector2.Distance(point1, point2);

			spriteBatch.Draw(dot, point1, null, color,
					   angle, Vector2.Zero, new Vector2(length, width),
					   SpriteEffects.None, 0);
		}
	}
}
