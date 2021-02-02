using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess_Game
{
    public class Game1 : Game
    {
        readonly Board DrawBoard = new Board();
        readonly Piece[,] DrawPiece = new Piece[8, 8];
        public static Game1 Instance;
        public static Vector2 boardPosition;

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
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            boardPosition = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2 - 200, GraphicsDevice.Viewport.Bounds.Height / 2 - 160);
            

            DrawBoard.PieceContent(DrawPiece);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            DrawBoard.PieceMove(DrawPiece, boardPosition);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            DrawBoard.BoardDraw(_spriteBatch, GraphicsDevice.Viewport.Bounds.Width / 2 - 200, GraphicsDevice.Viewport.Bounds.Height / 2 - 160);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (DrawPiece[i, j] != null)
                        DrawPiece[i, j].PieceDraw(_spriteBatch, i, j);
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
