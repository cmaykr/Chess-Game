using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    public class GameScreen : Screen
    {
        public readonly Piece[,] Pieces = new Piece[8, 8];
        readonly Board DrawBoard = new();

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            DrawBoard.PieceContent(Pieces);
            Board.Instance.GameUI.GameUIContent();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            DrawBoard.PieceMove(Pieces, Game1.boardPosition);
            Board.Instance.GameUI.DecrementTimer(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            DrawBoard.BoardDraw(spriteBatch, (int)Game1.boardPosition.X, (int)Game1.boardPosition.Y, Pieces);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}
