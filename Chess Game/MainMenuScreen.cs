using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    /// <summary>
    /// Klassen inheritar från Screen klassen.
    /// Är huvudklassen för huvudmeny rutan.
    /// </summary>
    class MainMenuScreen : Screen
    {
        MainMenu mainMenu = new();

        public override void Initialize()
        {
            base.Initialize();

            Game1.Instance.Window.AllowUserResizing = false;
            Game1.Instance._graphics.PreferredBackBufferWidth = 800;
            Game1.Instance._graphics.PreferredBackBufferHeight = 480;
            Game1.Instance._graphics.ApplyChanges();
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
