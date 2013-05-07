using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace ParticleGame
{
    class SettingsFileManager
    {
        private TextReader reader;
        private TextWriter writer;

        private Dictionary<Settings, string> settings;
        private Dictionary<Settings, string> defaultSettings;

        private static SettingsFileManager instance;

        public SettingsFileManager(string filename)
        {
            InitializeDefaultSettings();
            settings = new Dictionary<Settings, string>();
            try
            {
                reader = new StreamReader(filename);
            }
            catch (FileNotFoundException)
            {
                File.Create(filename);
                reader = new StreamReader(filename);
            }

            bool endReached = false;
            List<string> lines = new List<string>();
            while (!endReached)
            {
                string line = reader.ReadLine();
                if (line == null)
                {
                    endReached = true;
                }
                else
                {
                    lines.Add(line);
                }
            }
            reader.Close();
            reader.Dispose();
            foreach (string line in lines)
            {
                // Ignore comments
                if(line.StartsWith("//")) continue;

                // Split string into key and value
                string[] linePieces = line.Split('=');
                if (1< linePieces.Length && linePieces.Length <3)
                {
                    // Turn the first argument into an enum constant
                    Settings s = (Settings)Enum.Parse(typeof(Settings), linePieces[0].ToUpper(), true);
                    settings.Add(s, linePieces[1]);

                }
            }
            
        }
        /// <summary>
        /// Sets up the values for the default settings, in case some settings are not defined by the settings file.
        /// </summary>
        private void InitializeDefaultSettings()
        {
            defaultSettings = new Dictionary<Settings, string>()
            {
                {Settings.SHOW_HELP_MESSAGE, "true"},
                {Settings.FULLSCREEN, "false"},
                {Settings.WINDOW_HEIGHT, "1024"},
                {Settings.WINDOW_WIDTH, "1280"},
                {Settings.SHOW_FRAMERATE_GRAPH, "true"}
            };
        }
        /// <summary>
        /// Returns the value associated with the setting passed into the method.
        /// Tries to look for the value in the settings file first; if that doesn't work,
        /// it looks it up in the default settings.
        /// </summary>
        /// <param name="setting">The setting of which you want to get the value</param>
        /// <returns>The value of the setting. This is a string, so it needs to be parsed first.</returns>
        public string GetSetting(Settings setting)
        {
            if (settings.ContainsKey(setting))
            {
                return settings[setting];
            }
            else
            {
                return defaultSettings[setting];
            }
        }
        /// <summary>
        /// Returns the SettingsFileManager instance, or creates it if it doesn't exist yet.
        /// If a new SettingsFileManager instance gets created, the game settings will automatically be read from the settings file.
        /// If the settings file doesn't exist yet, it is created.
        /// Do not attempt to create your own SettingsFileManager instance. Use this method instead.
        /// </summary>
        /// <returns>The SettingsFileManager instance you should use.</returns>
        public static SettingsFileManager GetSettingsFileManager()
        {
            if (instance == null)
            {
                instance = new SettingsFileManager("setti.ngs");
            }
            return instance;
        }
        /// <summary>
        /// Saves your current settings to the settings file.
        /// </summary>
        public void SaveSettings()
        {
            string filename = "setti.ngs";
            try
            {
                writer = new StreamWriter(filename);
            }
            catch (FileNotFoundException)
            {
                File.Create(filename);
                writer = new StreamWriter(filename);
            }
            writer.WriteLine(Settings.SHOW_HELP_MESSAGE.ToString() + "=" + settings[Settings.SHOW_HELP_MESSAGE]);
            writer.WriteLine(Settings.FULLSCREEN.ToString() + "=" + settings[Settings.FULLSCREEN]);
        }
        public void SetSetting(Settings type, string value)
        {
            settings[type] = value;
        }
    }
}
