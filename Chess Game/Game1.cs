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
        readonly Board DrawBoard = new();
        public readonly Piece[,] DrawPiece = new Piece[8, 8];
        public static Game1 Instance;
        public static Vector2 boardPosition;
        readonly GameUI gameUI = new();

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

            base.Initialize();
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += DrawBoard.OnResize;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            boardPosition = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2 - (DrawBoard.TileSize.X * 5), GraphicsDevice.Viewport.Bounds.Height / 2 - (DrawBoard.TileSize.Y * 4));
            DrawBoard.BoardContent(DrawPiece);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            DrawBoard.BoardUpdate(gameTime, DrawPiece, boardPosition);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            DrawBoard.BoardDraw(_spriteBatch, (int)boardPosition.X, (int)boardPosition.Y, DrawPiece);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
