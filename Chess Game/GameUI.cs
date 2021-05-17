using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    class GameUI
    {
        public Vector2 checkMateButton;
        public static Vector2 checkMateButtonSize = new(100, 30);
        public Rectangle checkMateButtonCoord;
        MouseState curr, prev;
        SpriteFont font;

        /// <summary>
        /// Laddar in alla filer som behövs för att rita spelrutan.
        /// </summary>
        public void GameUIContent()
        {
            checkMateButton = new(Game1.Instance.GraphicsDevice.Viewport.Bounds.Width / 2,
                Game1.Instance.GraphicsDevice.Viewport.Bounds.Height / 2);
            font = Game1.Instance.Content.Load<SpriteFont>("Arial");
        }

        /// <summary>
        /// Ritar allt som ska finnas under spelets gång.
        /// </summary>
        public void GameUIDraw(SpriteBatch spriteBatch, float playerOneTimer, float playerTwoTimer)
        {
            string gameText;
            checkMateButtonCoord = new((int)checkMateButton.X - 350,
                (int)checkMateButton.Y - 150,
                (int)checkMateButtonSize.X,
                (int)checkMateButtonSize.Y);

            if (Board.Instance.CheckMate && playerTwoTimer > 0 && playerTwoTimer > 0)
            {
                gameText = "Checkmate, " + (!Board.Instance.IsPlayerOne ? "White won" : "Black won");
            }
            else if (playerTwoTimer <= 0 || playerTwoTimer <= 0)
            {
                Board.Instance.CheckMate = true;
                gameText = "No time left, " + (!Board.Instance.IsPlayerOne ? "White won" : "Black won");
            }
            else
            {
                gameText = (Board.Instance.IsPlayerOne) ? "Whites turn" : "Blacks turn";
            }

            spriteBatch.DrawString(font,
                gameText,
                new Vector2(checkMateButton.X - 350, checkMateButton.Y - 100),
                Color.Black);
            spriteBatch.DrawString(font,
                $"Time left2: {(int)(playerTwoTimer / 60):00}:{(int)(playerTwoTimer % 60):00}",
                new Vector2(checkMateButton.X - 350, checkMateButton.Y),
                Color.Black);
            spriteBatch.DrawString(font,
                $"Time left: {(int)(playerOneTimer / 60):00}:{(int)(playerOneTimer % 60):00}",
                new Vector2(checkMateButton.X - 350, checkMateButton.Y+100),
                Color.Black);
        }

        /// <summary>
        /// Kollar om knappar som finns under en match trycks och bestämmer vad som händer när en knapp har tryckts.
        /// </summary>
        public void GameUIButtons()
        {
            curr = Mouse.GetState();
            var mousePos = new Point(curr.X, curr.Y);

            prev = curr;
        }
        /// <summary>
        /// Metod för att rita koordinaterna på spelbrädet
        /// </summary>
        /// <param name="spritebatch"></param>
        /// <param name="x">X Positionen för skärmen.</param>
        /// <param name="y">Y Positionen för skärmen.</param>
        /// <param name="xCoord">X Koordinaten för den rutan på spelbrädet.</param>
        /// <param name="yCoord">Y Koordinaten för den rutan på spelbrädet.</param>
        public void BoardCoord(SpriteBatch spritebatch, int x, int y, int xCoord, int yCoord)
        {
            spritebatch.DrawString(font, $"{xCoord}, {yCoord}", new Vector2(x, y), Color.Red);
        }
    }
}
