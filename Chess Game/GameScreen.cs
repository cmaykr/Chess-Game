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
            base.LoadContent();

            DrawBoard.PieceContent(Pieces);
            Board.Instance.GameUI.GameUIContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DrawBoard.PieceMove(Pieces, Game1.BoardPosition);
            Board.Instance.GameUI.DecrementTimer(gameTime);
            Board.Instance.GameUI.GameUIButtons();

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
