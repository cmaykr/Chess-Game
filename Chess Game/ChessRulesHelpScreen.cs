using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    /// <summary>
    /// Klassen inheritar ifrån Screen klassen och ritar alla spelregler som finns.
    /// </summary>
    class ChessRulesHelpScreen : Screen
    {
        Texture2D chessRules;

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void LoadContent()
        {
            base.LoadContent();

            chessRules = Game1.Instance.Content.Load<Texture2D>("ChessRules");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(chessRules, new Rectangle((int)Game1.ScreenMiddle.X - 250, (int)Game1.ScreenMiddle.Y - 190, 500, 390), Color.White);
        }
    }
}
