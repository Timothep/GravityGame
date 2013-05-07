using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace ParticleGame
{
    class DebugFileManager
    {
        private StreamWriter writer;
        public DebugFileManager(string path)
        {
            writer = new StreamWriter(path, true);
        }

        public void WriteLineF(string line)
        {
            writer.WriteLine(line);
            writer.Flush();
        }
        public void WriteLine(string line)
        {
            writer.WriteLine(line);
        }
        public void Flush()
        {
            writer.Flush();
        }
        private static DebugFileManager instance = null;

        public static DebugFileManager GetDebugFileManager()
        {
            if (instance == null)
            {
                instance = new DebugFileManager("commandlog.txt");
            }
            return instance;
        }

        /// <summary>
        /// Flushes the stream, then closes it.
        /// </summary>
        public void Close()
        {
            writer.Flush();
            writer.Close();
        }
    }
}
