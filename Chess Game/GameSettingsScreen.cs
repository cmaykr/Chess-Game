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
        Rectangle TimeSettingPos;
        Rectangle IncrementTimeSettingPos;
        bool TimeSettingChosen;
        string TimeSetting = "600";
        string keyValue;
        KeyboardState oldkeyboardState;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            TimeSettingPos = new((int)Game1.ScreenMiddle.X - 240, (int)Game1.ScreenMiddle.Y - 200, 120, 40);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (TimeSettingChosen)
            {
                var keyboardState = Keyboard.GetState();
                var keys = keyboardState.GetPressedKeys();


                if (keys.Length > 0 && oldkeyboardState != keyboardState && !keyboardState.IsKeyDown(Keys.Back) && !Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    keyValue = keys[0].ToString();
                    TimeSetting += keyValue;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    TimeSettingChosen = false;
                }
                oldkeyboardState = keyboardState;
            }

            if (curr.LeftButton == ButtonState.Released && prev.LeftButton == ButtonState.Pressed)
            {
                if (TimeSettingPos.Contains(mousePos))
                {
                    TimeSettingChosen = true;
                    TimeSetting = "";
                }
            }

            prev = curr;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.Draw(TimeSettingPos.Contains(mousePos) ? Button_Selected : Button_Open, TimeSettingPos, Color.White);
            spriteBatch.DrawString(Font, "Time:", new Vector2(TimeSettingPos.X - 40, TimeSettingPos.Y + 12), Color.Black);
            spriteBatch.DrawString(Font, $"{TimeSetting}", new Vector2(TimeSettingPos.X + 20, TimeSettingPos.Y + 12), Color.Black);
            spriteBatch.End();
        }
    }
}
