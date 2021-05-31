using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    class LeaderBoardScreen : Screen
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

            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "PLACEHOLDER", Game1.ScreenMiddle, Color.Black);
            spriteBatch.End();
        }
    }
}
