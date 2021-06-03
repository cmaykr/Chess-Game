using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Chess_Game
{
    class Leaderboard
    {
        public List<MatchResult> MatchResults = new();

        public void Save()
        {
            string output = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText("Leaderboard.json", output);
        }
        public static Leaderboard Load()
        {
            if (File.Exists("Leaderboard.json"))
            {
                string input = File.ReadAllText("Leaderboard.json");

                return JsonConvert.DeserializeObject<Leaderboard>(input);
            }
            else
            {
                Leaderboard leaderboard = new();

                leaderboard.Save();
                return leaderboard;
            }    
        }
    }
}
