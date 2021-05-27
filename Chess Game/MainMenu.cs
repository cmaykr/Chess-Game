using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    class MainMenu
    {
        MouseState curr;
        MouseState prev;
        Point mousePos;
        Texture2D PlayButton;
        Rectangle PlayButtonPos;

        public void MenuContent()
        {
            PlayButtonPos = new Rectangle((int)Game1.ScreenMiddle.X - 90, (int)Game1.ScreenMiddle.Y - 150, 120, 40);
        }
        public void MenuDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Screen.Font, "David's Chess Game", new Vector2(Game1.ScreenMiddle.X - 100, Game1.ScreenMiddle.Y - 200), Color.Black);
            if (PlayButtonPos.Contains(mousePos))
            {
                PlayButton = Screen.Button_Selected;
            }
            else
            {
                PlayButton = Screen.Button_Open;
            }
            spriteBatch.Draw(PlayButton, PlayButtonPos, Color.White);
            spriteBatch.DrawString(Screen.Font, "Play", new Vector2(PlayButtonPos.X + 46, PlayButtonPos.Y + 12), Color.Black);
            spriteBatch.End();
        }
        public void MenuUpdate(GameTime gameTime)
        {
            curr = Mouse.GetState();
            mousePos = new Point(curr.X, curr.Y);

            if (PlayButtonPos.Contains(mousePos) && curr.LeftButton == ButtonState.Released && prev.LeftButton == ButtonState.Pressed)
            {
                Game1.Screen = new GameScreen();
                Game1.Screen.Initialize();
                Game1.Screen.LoadContent();
            }

            prev = curr;
        }
    }
}
