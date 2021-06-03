using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

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

        public static void Save()
        {
            SaveGame saveGame = new();
            saveGame.currentBoard = GameScreen.Instance.Pieces;
            saveGame.turns = GameScreen.Instance.GameUI.Turns;
            saveGame.notationList = GameScreen.Instance.GameUI.NotationList;
            saveGame.isPlayerOne = Board.Instance.IsPlayerOne;
            saveGame.playerOneTimer = GameScreen.Instance.GameUI.PlayerOneTimer;
            saveGame.playerTwoTimer = GameScreen.Instance.GameUI.PlayerTwoTimer;
            saveGame.timeIncrement = GameScreen.Instance.GameUI.TimeIncrement;

            string output = JsonConvert.SerializeObject(saveGame);

            File.WriteAllText("Davids-SaveGame.json", output);
        }

        public static void LoadGame()
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
