using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    public class GameScreen : Screen
    {
        public readonly Piece[,] Pieces = new Piece[8, 8];
        readonly Board DrawBoard = new();
        public readonly GameUI GameUI = new();
        public static GameScreen Instance;

        public GameScreen(float time, float timeIncrement)
        {
            Instance = this;
            GameUI.PlayerOneTimer = time;
            GameUI.PlayerTwoTimer = time;
            GameUI.TimeIncrement = timeIncrement;
        }
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            DrawBoard.PieceContent(Pieces);
            GameUI.GameUIContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DrawBoard.PieceMove(Pieces, Game1.BoardPosition);
            GameUI.DecrementTimer(gameTime);
            GameUI.GameUIButtons();

            prev = curr;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            DrawBoard.BoardDraw(spriteBatch, (int)Game1.BoardPosition.X, (int)Game1.BoardPosition.Y, Pieces);
            spriteBatch.End();
        }
    }
}
