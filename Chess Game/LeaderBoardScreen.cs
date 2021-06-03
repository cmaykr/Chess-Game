using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Chess_Game
{
    class LeaderBoardScreen : Screen
    {
        Leaderboard leaderboard = Leaderboard.Load();

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
            spriteBatch.DrawString(Font, "Leaderboard:", new Vector2(Game1.ScreenMiddle.X - 300, Game1.ScreenMiddle.Y - 220), Color.Black);
            for (int i = 0; i < leaderboard.MatchResults.Count; i++)
            {
                string result = "";
                result += "Rounds: " + (int)Math.Ceiling(((double)leaderboard.MatchResults[i].Turns + 1) / 2) + "  ";
                switch (leaderboard.MatchResults[i].Winner)
                {
                    case Winner.White:
                        result += "White won";
                        break;
                    case Winner.Black:
                        result += "Black won";
                        break;
                    case Winner.Draw:
                        result += "Draw";
                        break;
                }

                spriteBatch.DrawString(Font, $"{i + 1}: {result}", new Vector2(Game1.ScreenMiddle.X - 300, Game1.ScreenMiddle.Y - 200+ 20 * i), Color.Black);
            }
            spriteBatch.End();
        }
    }
}
