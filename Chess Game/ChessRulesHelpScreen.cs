using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    /// <summary>
    /// Klassen inheritar ifrån Screen klassen och ritar alla spelregler som finns.
    /// </summary>
    class ChessRulesHelpScreen : Screen
    {
        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(Font, "PLACEHOLDER", Game1.ScreenMiddle, Color.Black);
        }
    }
}
