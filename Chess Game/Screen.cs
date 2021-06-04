using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chess_Game
{
    /// <summary>
    /// Basklassen för alla rutor som används i spelet.
    /// Hanterar också alla variabler som används i varenda ruta.
    /// </summary>
    public class Screen
    {
        public static SpriteFont Font { get; private set; }
        public static Texture2D Button_Open { get; private set; }
        public static Texture2D Button_Selected { get; private set; }

        public static MouseState curr;
        public static MouseState prev;
        public static Point mousePos;

        KeyboardState key;

        /// <summary>
        /// Initialiserar alla variabler som används för alla rutor.
        /// Går att overrida för de enskilda rutorna. För att kunna kontrollera vad som ska initialiseras för varje ruta.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Laddar all grafik som används för alla rutor.
        /// Går att overrida för de enskilda rutorna. För att kunna kontrollera vad som ska laddas för varje ruta.
        /// </summary>
        public virtual void LoadContent()
        {
            Font = Game1.Instance.Content.Load<SpriteFont>("Arial");
            Button_Open = Game1.Instance.Content.Load<Texture2D>("Button_Open");
            Button_Selected = Game1.Instance.Content.Load<Texture2D>("Button_Pressed");
        }

        /// <summary>
        /// Metoden kallas varenda frame och innehåller allt som behöver uppdateras för alla rutor.
        /// Går att overrida för de enskilda rutorna. För att kunna kontrollera vad som ska uppdateras i varje ruta.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            curr = Mouse.GetState();
            key = Keyboard.GetState();
            mousePos = new Point(curr.X, curr.Y);

            if (key.IsKeyDown(Keys.Back))
            {
                Game1.Screen = new MainMenuScreen();
                Game1.Screen.Initialize();
                Game1.Screen.LoadContent();
            }
            if (Game1.Screen is MainMenuScreen && key.IsKeyDown(Keys.Escape))
            {
                Game1.Instance.Exit();
            }
        }

        /// <summary>
        /// Ritar text och knappar som behövs för varenda ruta. Kallas varenda frame.
        /// Går att overrida för den enskilda scenerna. För att kunna kontrollera vad som ska ritas i varje ruta.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            string exitHelp = (Game1.Screen is MainMenuScreen) ? "Press ESC to Exit game" : "Press Backspace to Exit current screen.";
            spriteBatch.DrawString(Font, exitHelp, new Vector2(Game1.ScreenMiddle.X + 150, Game1.ScreenMiddle.Y - 212), Color.Black);
        }
    }
}
