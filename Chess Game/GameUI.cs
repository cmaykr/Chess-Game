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
        Texture2D Button;
        public Vector2 checkMateButton;
        public static Vector2 checkMateButtonSize = new(100, 30);
        public Rectangle checkMateButtonCoord;
        MouseState curr, prev;
        string checkMateButtonText;

        public void GameUIContent()
        {
            Button = Game1.Instance.Content.Load<Texture2D>("Button");
            checkMateButton = new(Game1.Instance.GraphicsDevice.Viewport.Bounds.Width / 2, Game1.Instance.GraphicsDevice.Viewport.Bounds.Height / 2);

        }

        public void GameUIDraw(SpriteBatch spriteBatch)
        {
            checkMateButtonCoord = new((int)checkMateButton.X - 350, (int)checkMateButton.Y - 150, (int)checkMateButtonSize.X, (int)checkMateButtonSize.Y);
            spriteBatch.Draw(Button, checkMateButtonCoord, Color.White);
            if (Board.Instance.checkMate)
            {
                checkMateButtonText = Board.Instance.isPlayerOne ? "White won" : "Black won";
            }
            else
            {
                checkMateButtonText = "Checkmate?";
            }    
            spriteBatch.DrawString(Board.Instance.font, checkMateButtonText, new Vector2(checkMateButtonCoord.X, checkMateButtonCoord.Y), Color.Black);
        }
        public void GameUIMovement()
        {
            curr = Mouse.GetState();
            var mousePos = new Point(curr.X, curr.Y);

            if (curr.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released)
            {
                if (checkMateButtonCoord.Contains(mousePos))
                {
                    Board.Instance.checkMate = true;
                }
            }

            prev = curr;
        }
    }
}
