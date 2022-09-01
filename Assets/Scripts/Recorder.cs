using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars
{
    public static class Recorder 
    {
        private static StreamWriter _writer;

        private static readonly string Address = Directory.GetCurrentDirectory() + @"\" + "Log" + ".txt";

        public static void Write(string name, string time)
        {
            _writer = new StreamWriter(Address, true);

            var line = $"{name}|{time}";

            _writer.WriteLine(line);

            _writer.Close();

            _writer = null;
        }

        private static IEnumerable<string> Read()
        {
            if (File.Exists(Address))
            {
                StreamReader reader = new StreamReader(Address);

                string fileText = reader.ReadToEnd();

                reader.Close();

                var lines = fileText.Split('\n').ToList();

                lines.Remove("");

                return lines;
            }

            else return new List<string>();
        }

        public static Dictionary<string, string> CurrentLeaderboard()
        {
            return Read()
                .Select(z => new { 
                    name = string.Concat(z.TakeWhile(q => q != '|')), 
                    time = string.Concat(z.SkipWhile(q => q != '|').Skip(1)) })
                .ToDictionary(x => x.name, x => x.time);
        }
    }
}
