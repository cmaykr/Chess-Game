using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    public class Game1 : Game
    {
        public static Screen Screen = new MainMenuScreen();
        public static Game1 Instance;
        public static Vector2 BoardPosition;
        public static Vector2 ScreenMiddle;
        readonly Board DrawBoard = new();

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += DrawBoard.OnResize;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            BoardPosition = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2 - (DrawBoard.TileSize.X * 5), GraphicsDevice.Viewport.Bounds.Height / 2 - (DrawBoard.TileSize.Y * 4));
            ScreenMiddle = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2, GraphicsDevice.Viewport.Bounds.Height / 2);
            Screen.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            Screen.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            Screen.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
