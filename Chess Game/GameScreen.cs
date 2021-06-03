using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    /// <summary>
    /// Klassen inheritar ifrån Screen klassen. GameScreen är huvudklassen för hela spelrutan.
    /// </summary>
    public class GameScreen : Screen
    {
        public Piece[,] Pieces = new Piece[8, 8];
        readonly Board DrawBoard = new();
        public readonly GameUI GameUI = new();
        public static Vector2 BoardPosition { get; set; }

        public static GameScreen Instance;

        public GameScreen(float playerOneTime, float playerTwoTime, float timeIncrement)
        {
            Instance = this;
            GameUI.PlayerOneTimer = playerOneTime;
            GameUI.PlayerTwoTimer = playerTwoTime;
            GameUI.TimeIncrement = timeIncrement;
        }
        public override void Initialize()
        {
            base.Initialize();

            Game1.Instance.Window.AllowUserResizing = true;
            Game1.Instance.Window.ClientSizeChanged += DrawBoard.OnResize;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            BoardPosition = new Vector2(Game1.Instance.GraphicsDevice.Viewport.Bounds.Width / 2 - (DrawBoard.TileSize.X * 5), Game1.Instance.GraphicsDevice.Viewport.Bounds.Height / 2 - (DrawBoard.TileSize.Y * 4));
            DrawBoard.PieceContent(Pieces);
            GameUI.GameUIContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DrawBoard.PieceMove(Pieces, BoardPosition);
            GameUI.DecrementTimer(gameTime);
            GameUI.GameUIButtons();

            prev = curr;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            DrawBoard.BoardDraw(spriteBatch, (int)BoardPosition.X, (int)BoardPosition.Y, Pieces);
            spriteBatch.End();
        }


        /// <summary>
        /// Metoden kallass när någon vinner eller om det blir oavgjort.
        /// </summary>
        public void EndOfGame()
        {
            Leaderboard leaderboard = Leaderboard.Load();
            var result = new MatchResult { Turns = GameUI.Turns };
            if (GameUI.WhiteWon && GameUI.BlackWon)
                result.Winner = Winner.Draw;
            else if (GameUI.WhiteWon)
                result.Winner = Winner.White;
            else if (GameUI.BlackWon)
                result.Winner = Winner.Black;

            leaderboard.MatchResults.Add(result);
            leaderboard.MatchResults.Sort();
            leaderboard.Save();
        }
    }
}
