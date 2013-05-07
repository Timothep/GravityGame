using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleGame
{
	/*
	 * This class represents an object affected by gravity, as well as having gravity on its own in addition.
	 * For it to work, it needs to be able to interact with other GravityObjects using their CalculateForce,
	 * Update, ClearForce, and, optionally, CheckCollision methods.
	 * 
	 * Altering the density constant should only be done to finetune the force of gravity on each planet.
	 * To dynamically change the force of gravity at runtime, change the gravity constant in the Universe class instead.
	 */

	class GravityObject
	{
		#region Fields & Accessors

		private List<bool> gObjectsCalculated = new List<bool>(); 

		private static Random rand = new Random();

		private float lastForce;

		private const double density = 0.05;

		private bool isMarkedDelete;
		public bool IsMarkedDelete
		{
			get
			{
				return isMarkedDelete;
			}
			set
			{
				isMarkedDelete = value;
			}
		}
		private Vector2 velocity;
		public Vector2 Velocity
		{
			get
			{
				return velocity;
			}
			set
			{
				velocity = value;
			}
		}
		private Vector2 coordinates;
		public Vector2 Coordinates
		{
			get
			{
				return coordinates;
			}
			set
			{
				coordinates = value;
			}
		}
		private Vector2 force;
		private int index;
		private Color color;
		public Color Color
		{
			get
			{
				return color;
			}
		}
		private float mass;
		public float Mass
		{
			get
			{
				return mass;
			}
			set
			{
				mass = value;
			}
		}
		private float radius;
		public float Radius
		{
			get
			{
				return radius;
			}
		}
		public float Diameter
		{
			get
			{
				return radius * 2;
			}
		}
		public bool IsGhost;
		#endregion

		/// <summary>
		/// Creates a GravityObject instance.
		/// </summary>
		/// <param name="index">The number of the GravityObject in the array it's placed in. Debug value.</param>
		/// <param name="coordinates">The position to spawn the GravityObject at.</param>
		/// <param name="velocity">The initial velocity of the object.</param>
		/// <param name="radius">The initial size of the object.</param>
		public GravityObject(int index, Vector2 coordinates, Vector2 velocity, float radius)
			: this(index, coordinates, velocity, radius, false) {}

		public GravityObject(int index, Vector2 coordinates, Vector2 velocity, float radius, bool isGhost)
		{
			this.color = CreateRandomColor();

			this.index = index;
			this.coordinates = coordinates;
			this.velocity = velocity;
			this.radius = radius;
			this.IsGhost = false;

			mass = (float)(Math.Pow(radius, 3) * density);

			force = new Vector2(0, 0);
		}

		private Microsoft.Xna.Framework.Color CreateRandomColor()
		{
			int a = rand.Next(5);
			int b = rand.Next(5);
			int c = rand.Next(5);
			c = (a == b && b == c) ? rand.Next(5) : c;

			a = a * 64 + rand.Next(64) - 32;
			b = b * 64 + rand.Next(64) - 32;
			c = c * 64 + rand.Next(64) - 32;

			a = a > 255 ? 255 : a;
			b = b > 255 ? 255 : b;
			c = c > 255 ? 255 : c;

			a = a < 0 ? 0 : a;
			b = b < 0 ? 0 : b;
			c = c < 0 ? 0 : c;

			int s = rand.Next(3);
			switch (s) {
				case 0:
					return new Color(a, b, c);
				case 1:
					return new Color(b, c, a);
				case 2:
					return new Color(c, a, b);
				default:
					return Color.White;
			}
		}
		/// <summary>
		/// A simple method for squaring a float, 
		/// as the math class only works with Doubles,
		/// which require for typecasting and therefore make code messier and harder to read.
		/// </summary>
		/// <param name="value">The value to be squared</param>
		/// <returns>The square of the passed value</returns>
		private float square(float value)
		{
			return value * value;
		}

		/// <summary>
		/// Calculates c using a and b, 
		/// with pythagoras' theorem (c = sqrt(a^2 + b^2))
		/// using the float type, for easy integration
		/// </summary>
		/// <param name="a">a, obviously</param>
		/// <param name="b">b, obviously</param>
		/// <returns>c</returns>
		protected float Pythagoras(float a, float b)
		{
			return (float)Math.Sqrt(a * a + b * b); // Yeah Math.Pow() would work too
		}

		/// <summary>
		///	 Calculate the force between the 'this' object and the passed object.
		///	 Force will only be applied on this object, so for a correct calculation of the force
		///	 between object a and b, you need to run both a.CalculateForce(b) and b.CalculateForce(a).
		/// </summary>
		/// <param name="go">The GravityObject against which the force of this object will be calculated.</param>
		public void CalculateForce(GravityObject go)
		{
			float dX = go.coordinates.X - coordinates.X;
			float dY = go.coordinates.Y - coordinates.Y;

			float r = Pythagoras(dY, dX);

			float force = Universe.GRAVITATIONAL_CONSTANT * ((mass * go.Mass) /(float)Math.Pow(r,3));
			lastForce = force;

			this.force.Y += dY * force;
			this.force.X += dX * force;
		}
		public bool Handled(GravityObject go)
		{
			return false;
		}
		/// <summary>
		/// Updates the GravityObject's position and velocity using the force values previously calculated with CalculateForce()
		/// </summary>
		/// <param name="index">Pass the index of the GravityObject to calculate its color</param>
		public void Update(int index)
		{
			this.index = index;
			velocity.X += force.X / mass;
			velocity.Y += force.Y / mass;
			coordinates.X += velocity.X;
			coordinates.Y += velocity.Y;
		}
		/// <summary>
		/// Draws planet-related debug info on the screen.
		/// </summary>
		/// <param name="spriteBatch">The spritebatch to draw the debug info to</param>
		/// <param name="font">The font in which the debug info will be written</param>
		public void DisplayInfo(SpriteBatch spriteBatch, SpriteFont font)
		{
			spriteBatch.DrawString(font, string.Format("GO{0}: M={1} C={2} {3} {4}", index, mass, color.R, color.G, color.B), new Vector2(10, 25 + index * 15), Color.DarkRed);
		}
		/// <summary>
		/// Checks whether this object collides with the passed object,
		/// and if so, destroys the lighter object and increases the mass
		/// of the bigger object by the mass of the lighter object.
		/// If masses are equal, this object takes precendence.
		/// </summary>
		/// <param name="target">The object against which collisions will be checked.</param>
		public void CheckCollision(GravityObject target)
		{
			if (IsMarkedDelete) return;

			float dY = target.coordinates.Y - coordinates.Y;
			float dX = target.coordinates.X - coordinates.X;

			float c = Pythagoras(dY, dX);

			// Compares the distance between the planets to the radius of both planets added together.
			// Evaluates to true if objects have a collision.
			if (c <= (radius + target.Radius))
			{
				int r = (this.color.R + target.color.R / 2);
				int g = (this.color.G + target.color.G / 2);
				int b = (this.color.B + target.color.B / 2);

				float targetSignificance = target.Mass / this.mass;

				if (radius > target.Radius)
				{
					this.mass += target.Mass;

					Vector2 vTargetTotal = target.velocity * new Vector2(targetSignificance, targetSignificance);
					Vector2 vTotal = this.velocity + vTargetTotal;
					this.velocity = vTotal / (1f + targetSignificance);

					this.color = new Color(r, g, b);

					target.IsMarkedDelete = true;
				}
				else if (radius < target.Radius)
				{
					target.Mass += this.mass;

					Vector2 vTargetTotal = target.velocity * new Vector2(targetSignificance, targetSignificance);
					Vector2 vTotal = this.velocity + vTargetTotal;
					target.velocity = vTotal / (1f + targetSignificance);

					target.color = new Color(r, g, b); 

					this.IsMarkedDelete = true;
				}
				else
				{
					this.mass += target.Mass;

					this.coordinates.X += dX/2;
					this.coordinates.Y += dY/2;

					this.velocity = ((this.velocity + target.velocity) / new Vector2(2, 2));

					this.color = new Color(r, g, b);

					target.IsMarkedDelete = true;
				}
				RecalculateSize();
				target.RecalculateSize();
			}
		}
		public void ClearForce()
		{
			force.X = force.Y = 0;
		}
		public void RecalculateSize()
		{
			if (isMarkedDelete) return;
			
			// radius = cuberoot(mass*density/PI)
			//radius = (float) Math.Pow((mass*density/Math.PI), 1.0/3.0);
			radius = (float)Math.Pow((mass / density), 1.0 / 3.0);
		}
	}

}
