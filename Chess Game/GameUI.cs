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

        float playerOneTimer = 600f;
        float playerTwoTimer = 600f;

        /// <summary>
        /// Laddar in alla filer som behövs för att rita spelrutan.
        /// </summary>
        public void GameUIContent()
        {
            checkMateButton = new(Game1.Instance.GraphicsDevice.Viewport.Bounds.Width / 2,
                Game1.Instance.GraphicsDevice.Viewport.Bounds.Height / 2);
        }

        /// <summary>
        /// Ritar allt som ska finnas under spelets gång.
        /// </summary>
        public void GameUIDraw(SpriteBatch spriteBatch)
        {
            string gameText;
            checkMateButtonCoord = new((int)checkMateButton.X - 350,
                (int)checkMateButton.Y - 150,
                (int)checkMateButtonSize.X,
                (int)checkMateButtonSize.Y);

            if (Board.Instance.CheckMate && playerOneTimer > 0 && playerTwoTimer > 0)
            {
                gameText = "Checkmate, " + (Board.Instance.isPlayerOne ? "White won" : "Black won");
            }
            else if (playerOneTimer <= 0 || playerTwoTimer <= 0)
            {
                Board.Instance.CheckMate = true;
                gameText = "No time left, " + (Board.Instance.isPlayerOne ? "White won" : "Black won");
            }
            else
            {
                gameText = (!Board.Instance.isPlayerOne) ? "Whites turn" : "Blacks turn";
            }

            spriteBatch.DrawString(Board.Instance.font,
                gameText,
                new Vector2(checkMateButton.X - 350, checkMateButton.Y - 100),
                Color.Black);
            spriteBatch.DrawString(Board.Instance.font,
                $"Time left: {(int)(Board.Instance.isPlayerOne ? playerOneTimer : playerTwoTimer)}",
                new Vector2(checkMateButton.X - 350, checkMateButton.Y),
                Color.Black);
        }

        public void GameUIUpdate(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!Board.Instance.CheckMate)
            {
                if (Board.Instance.isPlayerOne)
                {
                    playerOneTimer -= elapsedTime;
                }
                else
                {
                    playerTwoTimer -= elapsedTime;
                }
            }
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
    }
}
