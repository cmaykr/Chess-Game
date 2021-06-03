using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chess_Game
{
    /// <summary>
    /// Klassen innehåller allt för huvudmenyn.
    /// </summary>
    class MainMenu
    {
        Rectangle PlayButtonPos;
        Rectangle RuleButtonPos;
        Rectangle LeaderboardButtonPos;

        /// <summary>
        /// Positionerna av menyknapparna bestäms i metoden.
        /// </summary>
        public void MenuContent()
        {
            PlayButtonPos = new Rectangle((int)Game1.ScreenMiddle.X - 110, (int)Game1.ScreenMiddle.Y - 150, 180, 60);
            RuleButtonPos = new Rectangle((int)Game1.ScreenMiddle.X - 110, (int)Game1.ScreenMiddle.Y - 60, 180, 60);
            LeaderboardButtonPos = new Rectangle((int)Game1.ScreenMiddle.X - 110, (int)Game1.ScreenMiddle.Y + 30, 180, 60);

        }

        /// <summary>
        /// Metoden ritar alla knappar och all text.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="Font"></param>
        /// <param name="mousePos"></param>
        public void MenuDraw(SpriteBatch spriteBatch, SpriteFont Font, Point mousePos)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "David's Chess Game", new Vector2(Game1.ScreenMiddle.X - 100, Game1.ScreenMiddle.Y - 200), Color.Black);

            spriteBatch.Draw(PlayButtonPos.Contains(mousePos) ? Screen.Button_Selected : Screen.Button_Open, PlayButtonPos, Color.White);
            spriteBatch.DrawString(Font, "Play", new Vector2(PlayButtonPos.X + 75, PlayButtonPos.Y + 20), Color.Black);

            spriteBatch.Draw(RuleButtonPos.Contains(mousePos) ? Screen.Button_Selected : Screen.Button_Open, RuleButtonPos, Color.White);
            spriteBatch.DrawString(Font, "Rules", new Vector2(RuleButtonPos.X + 70, RuleButtonPos.Y + 20), Color.Black);

            spriteBatch.Draw(LeaderboardButtonPos.Contains(mousePos) ? Screen.Button_Selected : Screen.Button_Open, LeaderboardButtonPos, Color.White);
            spriteBatch.DrawString(Font, "Leaderboard", new Vector2(LeaderboardButtonPos.X + 45, LeaderboardButtonPos.Y + 20), Color.Black);
            spriteBatch.End();
        }

        /// <summary>
        /// Update metoden för huvudmenyn, uppdateras varje frame.
        /// Metoden kollar om man har tryckt på någon av knapparna.
        /// </summary>
        /// <param name="gameTime">Tid staten för Game klassen.</param>
        /// <param name="curr">Det nuvarande staten för musen.</param>
        /// <param name="prev">Mus staten från förra gången update kallades.</param>
        /// <param name="mousePos">Positionen av musen.</param>
        public void MenuUpdate(GameTime gameTime, MouseState curr, MouseState prev, Point mousePos)
        {

            if (curr.LeftButton == ButtonState.Released && prev.LeftButton == ButtonState.Pressed)
            {
                if (PlayButtonPos.Contains(mousePos))
                {
                    Game1.Screen = new GameSettingsScreen();
                    Game1.Screen.Initialize();
                    Game1.Screen.LoadContent();
                }
                if (RuleButtonPos.Contains(mousePos))
                {
                    Game1.Screen = new ChessRulesHelpScreen();
                    Game1.Screen.Initialize();
                    Game1.Screen.LoadContent();
                }
                if (LeaderboardButtonPos.Contains(mousePos))
                {
                    Game1.Screen = new LeaderBoardScreen();
                    Game1.Screen.Initialize();
                    Game1.Screen.LoadContent();
                }
            }
        }
    }
}
