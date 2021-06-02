using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    public class GameScreen : Screen
    {
        public Piece[,] Pieces = new Piece[8, 8];
        readonly Board DrawBoard = new();
        public readonly GameUI GameUI = new();
        public readonly SaveGame SaveGame = new();
        public static Vector2 BoardPosition;

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
    }
}
