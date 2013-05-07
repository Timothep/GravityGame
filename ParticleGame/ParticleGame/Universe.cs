using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;

namespace ParticleGame
{
	/*
	 * Big container class for all gravity objects.
	 */

	class Universe
	{
		public List<GravityObject> planets = new List<GravityObject>();
		public List<PathPoint> ghosts = new List<PathPoint>();
		public List<PathPoint> pathPoints = new List<PathPoint>();
		public const float GRAVITATIONAL_CONSTANT = 1f; // 

		public bool IsUpdated = false;

		public static Universe instance;

		public Universe()
		{
			instance = this;
		}
		public void Clear()
		{
			planets.Clear();
			pathPoints.Clear();
		}
		public void Update()
		{
			// Do collision detection
			foreach (GravityObject go in planets) {
				foreach (GravityObject go2 in planets) {
					if (go2.Equals(go)) continue;
					go.CheckCollision(go2);
				}
			}
			// Remove all planets marked for deletion during collision detection pass
			for (int i = 0; i < planets.Count; i++) {
				if ((planets[i].IsMarkedDelete)) {
					planets.Remove(planets[i]);
					i--;
				}
			}
			// Calculate the total force on each planet
			foreach (GravityObject go in planets)
			{
				foreach (GravityObject go2 in planets)
				{
					// Skip the loop if the planet tries to calculate its force on itself
					if (go2.Equals(go)) continue;
					go.CalculateForce(go2);
				}
			}
			// Update each planet and add a PathPoint for it
			foreach (GravityObject go in planets)
			{
				pathPoints.Add(new PathPoint((int)go.Coordinates.X, (int)go.Coordinates.Y, 300, go.Color));
				go.Update(planets.IndexOf(go));
				go.ClearForce();

			}
			// Update PathPoints, and remove old ones.
			for (int i = 0; i < pathPoints.Count; i++)
			{
				pathPoints[i].Update();
				if (pathPoints[i].IsOld)
				{
					pathPoints.RemoveAt(i);
					i--;
				}
				
			}
		}
		public void AddPlanet(Vector2 coordinates, Vector2 velocity, float mass)
		{
			planets.Add(new GravityObject(planets.Count, coordinates, velocity, mass));
		}
		public bool CheckPlanetAt(Vector2 position)
		{
			return false;
		}
		public void CreatePath(Vector2 coordinates, Vector2 velocity, float mass, int points)
		{
			ghosts.Clear();
			GravityObject go = new GravityObject(planets.Count, coordinates, velocity, mass, true);
			for (int i = 0; i < points; i++)
			{
				foreach (GravityObject go2 in planets)
				{
					go.CalculateForce(go2);
				}
				go.Update(-1);
				go.ClearForce();
				ghosts.Add(new PathPoint( (int) go.Coordinates.X, (int) go.Coordinates.Y, 600, Color.White));
			}
		}
		public void ClearPath()
		{
			ghosts.Clear();
		}
		public void AddGravityObject(GravityObject go)
		{
			planets.Add(go);
		}
	}
}
