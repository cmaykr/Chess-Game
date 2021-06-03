using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Chess_Game
{
    /// <summary>
    /// Klassen sparar leaderboard i en fil och läser in den från samma fil.
    /// </summary>
    class Leaderboard
    {
        public List<MatchResult> MatchResults = new();

        /// <summary>
        /// Sparar leaderboarden i en fil.
        /// </summary>
        public void Save()
        {
            string output = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText("Leaderboard.json", output);
        }

        /// <summary>
        /// Läser in leaderboard filen.
        /// </summary>
        /// <returns>Returnerar det som finns i filen.</returns>
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
