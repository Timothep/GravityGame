using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParticleGame
{
	class CommandHandler
	{
		private Game1 game;
		public CommandHandler(Game1 game)
		{
			this.game = game;
		}
		public void ProcessCommand(string command)
		{
			string[] args = BreakUpCommand(command);

			DebugFileManager.GetDebugFileManager().WriteLine("[COMMAND START]");
			foreach (string arg in args)
			{
				DebugFileManager.GetDebugFileManager().WriteLine("\""+ arg + "\"");
			}
			DebugFileManager.GetDebugFileManager().WriteLine("[COMMAND END]");

			if (args.Length == 2 && args[0].Equals("del"))
			{
				int id;
				if(args[1].EndsWith("+") && (id = int.Parse(args[1].Substring(0,args[1].Length-1))) < Universe.instance.planets.Count)
				{
					while(id < Universe.instance.planets.Count)
					{
						Universe.instance.planets.RemoveAt(id);
					}
				}
				else if ((id = int.Parse(args[1].Substring(0, args[1].Length))) < Universe.instance.planets.Count)
				{
					Universe.instance.planets.RemoveAt(int.Parse(args[1]));
				}
				
			}
			else if(args.Length == 2 && args[0].Equals("save"))
			{
				SaveGameManager.GetSaveGameManager().Save(Universe.instance, "saves/" + args[1] + ".uni");
			}
			else if (args.Length == 2 && args[0].Equals("load"))
			{
				Universe.instance = SaveGameManager.GetSaveGameManager().Load("saves/" + args[1] + ".uni");
				Universe.instance.IsUpdated = true;
			}
			else if (args.Length == 3 && args[0].Equals("set"))
			{
				SettingsFileManager.GetSettingsFileManager().SetSetting((Settings)Enum.Parse(typeof(Settings), args[1].ToUpper(), true), args[2]);
			}
			else if (args.Length == 1 && args[0].Equals("savesettings"))
			{
				SettingsFileManager.GetSettingsFileManager().SaveSettings();
			}
		}
		private string[] BreakUpCommand(string command)
		{
			return command.Split(' ');
		}
	}
}
