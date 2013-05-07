using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Microsoft.Xna.Framework;

namespace ParticleGame
{
	class SaveGameManager
	{
		private static SaveGameManager instance;

		public static SaveGameManager GetSaveGameManager()
		{
			if (instance == null)
			{
				instance = new SaveGameManager();
			}
			return instance;
		}
		public void Save(Universe universe, string filename)
		{
			if (!Directory.Exists("saves")) Directory.CreateDirectory("saves");
			TextWriter writer = new StreamWriter(filename);
			foreach (GravityObject go in universe.planets)
			{
				writer.WriteLine("{");
				writer.WriteLine("\t" + go.Coordinates.X);
				writer.WriteLine("\t" + go.Coordinates.Y);
				writer.WriteLine("\t" + go.Velocity.X);
				writer.WriteLine("\t" + go.Velocity.Y);
				writer.WriteLine("\t" + go.Radius);
				writer.WriteLine("}");
			}
			writer.Flush();
			writer.Close();
		}
		public Universe Load(string filename)
		{
			TextReader reader = new StreamReader(filename);

			GravityObject var;
			List<GravityObject> planets = new List<GravityObject>();
			int index = 0;
			string line = reader.ReadLine();
			while (line != null)
			{
				if (line.Contains("{"))
				{
					line = reader.ReadLine();
					float x = float.Parse(line.Substring(1, line.Length - 1));
					line = reader.ReadLine();
					float y = float.Parse(line.Substring(1, line.Length - 1));
					Vector2 coordinates = new Vector2(x, y);

					line = reader.ReadLine();
					float xv = float.Parse(line.Substring(1, line.Length - 1));
					line = reader.ReadLine();
					float yv = float.Parse(line.Substring(1, line.Length - 1));
					Vector2 velocity = new Vector2(xv, yv);

					line = reader.ReadLine();
					DebugFileManager.GetDebugFileManager().WriteLineF(line);
					float radius = float.Parse(line.Substring(1, line.Length - 1));

					var = new GravityObject(index, coordinates, velocity, radius);
					planets.Add(var);
					index++;

					if (!reader.ReadLine().Contains("}"))
					{
						DebugFileManager.GetDebugFileManager().WriteLineF("There was an error in loading the planet data. Please make sure the savegame is not corrupted.");
						Environment.Exit(0);
					}
				}
				line = reader.ReadLine();
			}
			Universe universe = new Universe();
			foreach (GravityObject go in planets)
			{
				universe.AddGravityObject(go);
			}
			DebugFileManager dfm = DebugFileManager.GetDebugFileManager();
			dfm.WriteLine("[SAVEGAME LOADING INFORMATION]");
			dfm.WriteLine(" We managed to load the savegame. Here are the results generated from the savefile:");
			foreach (GravityObject go in universe.planets)
			{
				dfm.WriteLine("\t[GRAVITYOBJECT START]");
				dfm.WriteLine("\t "+ go.Diameter);
				dfm.WriteLine("\t[GRAVITYOBJECT END]");
			}
			dfm.WriteLine("[SAVEGAME LOADING INFORMATION END]");
			dfm.Flush();
			return universe;
		}
	}
}
