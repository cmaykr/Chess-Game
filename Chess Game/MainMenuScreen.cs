using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    class MainMenuScreen : Screen
    {
        MainMenu mainMenu = new();

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            mainMenu.MenuContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            mainMenu.MenuUpdate(gameTime, curr, prev, mousePos);

            prev = curr;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            mainMenu.MenuDraw(spriteBatch, Font, mousePos);
        }
    }
}
