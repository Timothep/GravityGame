using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleGame
{
	class Slider
	{
		private Rectangle sliderBar;
		private Rectangle bounds;
		private Rectangle buttonPosition;
		private Vector2 delta;

		private string texturePath;

		private Texture2D texture;

		private ButtonState PreviousButtonState;
		private bool grabbed;

		public Slider(Rectangle sliderBar, Rectangle buttonPosition, Rectangle bounds, string texturePath)
		{
			this.sliderBar = sliderBar;
			this.bounds = bounds;
			this.buttonPosition = buttonPosition;
			this.texturePath = texturePath;
			PreviousButtonState = ButtonState.Released;
		}
		public void LoadContent(ContentManager c)
		{
			texture = c.Load<Texture2D>(texturePath);
		}

		public int XValueAbs { get { return buttonPosition.X - bounds.X; } }

		public float XValuePerc { get { return (buttonPosition.X - bounds.X) / (float)(bounds.Width - buttonPosition.Width) * 100f; } }

		public int YValueAbs { get { return buttonPosition.Y - bounds.Y; } }

		public float YValuePerc { get { return (buttonPosition.Y - bounds.Y) / (float)(bounds.Height - buttonPosition.Height) * 100f; } }

		public void Update()
		{
			MouseState ms = Mouse.GetState();
			int x = ms.X;
			int y = ms.Y;
			if (ms.LeftButton == ButtonState.Pressed && PreviousButtonState == ButtonState.Released && ms.X >= buttonPosition.X && ms.X <= buttonPosition.X + buttonPosition.Width && ms.Y >= buttonPosition.Y && ms.Y <= buttonPosition.Y + buttonPosition.Height) {
				delta = new Vector2(ms.X - buttonPosition.X, ms.Y - buttonPosition.Y);
				grabbed = true;
			} else if (grabbed && ms.LeftButton == ButtonState.Pressed) {
				buttonPosition.X = ms.X - (int)delta.X;
				buttonPosition.Y = ms.Y - (int)delta.Y;
			} else if (grabbed && ms.LeftButton == ButtonState.Released) {
				grabbed = false;
			}
			if (buttonPosition.X < bounds.X) buttonPosition.X = bounds.X;
			if (buttonPosition.Y < bounds.Y) buttonPosition.Y = bounds.Y;
			if (buttonPosition.X > bounds.X + bounds.Width - buttonPosition.Width) buttonPosition.X = bounds.X + bounds.Width - buttonPosition.Width;
			if (buttonPosition.Y > bounds.Y + bounds.Height) buttonPosition.Y = bounds.Y + bounds.Height;
			PreviousButtonState = Mouse.GetState().LeftButton;
		}
		public void Draw(SpriteBatch sb)
		{
			sb.Draw(texture, sliderBar, Color.White);
			sb.Draw(texture, buttonPosition, Color.Blue);
		}
	}
}
