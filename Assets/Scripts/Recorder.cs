using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

namespace Cars
{
    public class Recorder : MonoBehaviour
    {
        public static Recorder Singleton;

        private StreamWriter _writer;

        private string _address;

        private string _fileName = "Log";

        void Awake()
        {
            if (!Singleton)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
            }

            else Destroy(gameObject);
        }

        void Start()
        {
            _address = Directory.GetCurrentDirectory().ToString() + @"\" + _fileName + ".txt";
        }

        public void Write(string name, string time)
        {
            _writer = new StreamWriter(_address, true);

            var line = $"{name}|{time}";

            _writer.WriteLine(line);

            _writer.Close();

            _writer = null;
        }

        public List<string> Read()
        {
            if (File.Exists(_address))
            {
                var fileText = string.Empty;

                StreamReader reader = new StreamReader(_address);

                fileText = reader.ReadToEnd();

                reader.Close();

                var lines = fileText.Split('\n').ToList();

                lines.Remove("");

                return lines;
            }

            else return new List<string>();
        }

        public Dictionary<string, string> CurrentLeaderboard()
        {
            return Read()
                .Select(z => new { 
                    name = string.Concat(z.TakeWhile(q => q != '|')), 
                    time = string.Concat(z.SkipWhile(q => q != '|').Skip(1)) })
                .ToDictionary(x => x.name, x => x.time);
        }
    }
}
