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
        Board DrawBoard = new Board();
        Piece[,] DrawPiece = new Piece[8, 8];
        public static Game1 Instance;
        public static Vector2 boardPosition;
        public Texture2D pawn;

        private GraphicsDeviceManager _graphics;
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
            pawn = Content.Load<Texture2D>("Pawn");


            // Easier or better way to do this?
            for (int i = 0; i < 8; i++)
            {
                DrawPiece[i, 1] = new Piece()
                {
                    type = PieceType.pawn,
                };
                DrawPiece[i, 6] = new Piece()
                {
                    type = PieceType.pawn,
                };
            }
            DrawPiece[0, 0] = new Piece()
            {
                type = PieceType.rook,
            };
            DrawPiece[7, 0] = new Piece()
            {
                type = PieceType.rook,
            };
            DrawPiece[0, 7] = new Piece()
            {
                type = PieceType.rook,
            };
            DrawPiece[7, 7] = new Piece()
            {
                type = PieceType.rook,
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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
