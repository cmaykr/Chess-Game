using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    class GameSettingsScreen : Screen
    {
        Vector2 timePos;
        Rectangle time2MinPos, time5MinPos, time10MinPos;
        Vector2 timeIncrementPos;
        Rectangle increment2sPos, increment5sPos, increment10sPos;
        Rectangle startButtonPos;
        float time = 600;
        float time2Min = 120;
        float time5Min = 300;
        float time10Min = 600;
        float timeIncrement = 5;
        float timeIncrement2s = 2;
        float timeIncrement5s = 5;
        float timeIncrement10 = 10;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            timePos = new((int)Game1.ScreenMiddle.X - 350, (int)Game1.ScreenMiddle.Y - 188);
            time2MinPos = new((int)Game1.ScreenMiddle.X - 240, (int)Game1.ScreenMiddle.Y - 200, 120, 40);
            time5MinPos = new((int)Game1.ScreenMiddle.X - 110, (int)Game1.ScreenMiddle.Y - 200, 120, 40);
            time10MinPos = new((int)Game1.ScreenMiddle.X + 20, (int)Game1.ScreenMiddle.Y - 200, 120, 40);
            startButtonPos = new((int)Game1.ScreenMiddle.X - 60, (int)Game1.ScreenMiddle.Y + 150, 120, 40);

            timeIncrementPos = new((int)Game1.ScreenMiddle.X - 350, (int)Game1.ScreenMiddle.Y - 148);
            increment2sPos = new((int)Game1.ScreenMiddle.X - 200, (int)Game1.ScreenMiddle.Y - 160, 120, 40);
            increment5sPos = new((int)Game1.ScreenMiddle.X - 70, (int)Game1.ScreenMiddle.Y - 160, 120, 40);
            increment10sPos = new((int)Game1.ScreenMiddle.X + 60, (int)Game1.ScreenMiddle.Y - 160, 120, 40);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (curr.LeftButton == ButtonState.Released && prev.LeftButton == ButtonState.Pressed)
            {
                if (time2MinPos.Contains(mousePos))
                {
                    time = time2Min;
                }
                if (time5MinPos.Contains(mousePos))
                {
                    time = time5Min;
                }
                if (time10MinPos.Contains(mousePos))
                {
                    time = time10Min;
                }
                if (increment2sPos.Contains(mousePos))
                {
                    timeIncrement = timeIncrement2s;
                }
                if (increment5sPos.Contains(mousePos))
                {
                    timeIncrement = timeIncrement5s;
                }
                if (increment10sPos.Contains(mousePos))
                {
                    timeIncrement = timeIncrement10;
                }
                if (startButtonPos.Contains(mousePos))
                {
                    Game1.Screen = new GameScreen();
                    Game1.Screen.Initialize();
                    Game1.Screen.LoadContent();
                    Board.Instance.GameUI.playerOneTimer = time;
                    Board.Instance.GameUI.playerTwoTimer = time;
                    Board.Instance.GameUI.timeIncrement = timeIncrement;
                }
            }

            prev = curr;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.Draw(time2MinPos.Contains(mousePos) ? Button_Selected : Button_Open, time2MinPos, Color.White);
            spriteBatch.DrawString(Font, "2 Minutes", new Vector2(time2MinPos.X + 20, time2MinPos.Y + 12), Color.Black);

            spriteBatch.Draw(time5MinPos.Contains(mousePos) ? Button_Selected : Button_Open, time5MinPos, Color.White);
            spriteBatch.DrawString(Font, "5 Minutes", new Vector2(time5MinPos.X + 20, time5MinPos.Y + 12), Color.Black);

            spriteBatch.Draw(time10MinPos.Contains(mousePos) ? Button_Selected : Button_Open, time10MinPos, Color.White);
            spriteBatch.DrawString(Font, "10 Minutes", new Vector2(time10MinPos.X + 20, time10MinPos.Y + 12), Color.Black);

            spriteBatch.DrawString(Font, $"Time: {(int)(time / 60):00}:{(int)(time % 60):00}", timePos, Color.Black);

            spriteBatch.Draw(startButtonPos.Contains(mousePos) ? Button_Selected : Button_Open, startButtonPos, Color.White);
            spriteBatch.DrawString(Font, "Start", new Vector2(startButtonPos.X + 45, startButtonPos.Y + 12), Color.Black);

            spriteBatch.DrawString(Font, $"Time increment: {(int)(timeIncrement % 60):00}", timeIncrementPos, Color.Black);

            spriteBatch.Draw(increment2sPos.Contains(mousePos) ? Button_Selected : Button_Open, increment2sPos, Color.White);
            spriteBatch.DrawString(Font, "2 s", new Vector2(increment2sPos.X + 20, increment2sPos.Y + 12), Color.Black);

            spriteBatch.Draw(increment5sPos.Contains(mousePos) ? Button_Selected : Button_Open, increment5sPos, Color.White);
            spriteBatch.DrawString(Font, "5 s", new Vector2(increment5sPos.X + 20, increment5sPos.Y + 12), Color.Black);

            spriteBatch.Draw(increment10sPos.Contains(mousePos) ? Button_Selected : Button_Open, increment10sPos, Color.White);
            spriteBatch.DrawString(Font, "10 s", new Vector2(increment10sPos.X + 20, increment10sPos.Y + 12), Color.Black);
            spriteBatch.End();
        }
    }
}
