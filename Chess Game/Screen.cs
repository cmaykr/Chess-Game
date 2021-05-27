using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    public class Screen
    {
        public static SpriteFont Font { get; private set; }
        public static Texture2D Button_Open { get; private set; }
        public static Texture2D Button_Selected { get; private set; }

        public static MouseState curr;
        public static MouseState prev;
        public static Point mousePos;

        public virtual void Initialize()
        {

        }

        public virtual void LoadContent()
        {
            Font = Game1.Instance.Content.Load<SpriteFont>("Arial");
            Button_Open = Game1.Instance.Content.Load<Texture2D>("Button_Open");
            Button_Selected = Game1.Instance.Content.Load<Texture2D>("Button_Pressed");
        }

        public virtual void Update(GameTime gameTime)
        {
            curr = Mouse.GetState();
            mousePos = new Point(curr.X, curr.Y);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Game1.Screen = new MainMenuScreen();
                Game1.Screen.Initialize();
                Game1.Screen.LoadContent();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
