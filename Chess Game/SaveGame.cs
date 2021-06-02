using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Chess_Game
{
    public class SaveGame
    {
        public Piece[,] currentBoard;
        public int turns;
        public List<string> notationList = new();
        public bool isPlayerOne;

        public float playerOneTimer;
        public float playerTwoTimer;
        public float timeIncrement;

        public void Save()
        {
            currentBoard = GameScreen.Instance.Pieces;
            turns = GameScreen.Instance.GameUI.Turns;
            notationList = GameScreen.Instance.GameUI.NotationList;
            isPlayerOne = Board.Instance.IsPlayerOne;
            playerOneTimer = GameScreen.Instance.GameUI.PlayerOneTimer;
            playerTwoTimer = GameScreen.Instance.GameUI.PlayerTwoTimer;
            timeIncrement = GameScreen.Instance.GameUI.TimeIncrement;

            string output = JsonConvert.SerializeObject(this);

            File.WriteAllText("Davids-SaveGame.json", output);
        }

        public void LoadGame()
        {
            string savedGame = File.ReadAllText("Davids-SaveGame.json");
            SaveGame loadedGame = JsonConvert.DeserializeObject<SaveGame>(savedGame);

            Game1.Screen = new GameScreen(loadedGame.playerOneTimer, loadedGame.playerTwoTimer, loadedGame.timeIncrement);
            Game1.Screen.Initialize();
            Game1.Screen.LoadContent();

            GameScreen.Instance.Pieces = loadedGame.currentBoard;
            GameScreen.Instance.GameUI.Turns = loadedGame.turns;
            GameScreen.Instance.GameUI.NotationList = loadedGame.notationList;
            Board.Instance.IsPlayerOne = loadedGame.isPlayerOne;
        }
    }
}
