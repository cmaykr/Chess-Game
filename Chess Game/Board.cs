using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chess_Game
{
    class Board
    {
        public Piece[,] pieces = new Piece[8, 8];
        public void BoardDraw(SpriteBatch spriteBatch, int Width, int Height)
        {
            for (int i = 0; i < 8; i += 1)
            {
                for (int j = 0; j < 8; j += 1)
                {
                    if (i % 2 == j % 2)
                    {
                        spriteBatch.Draw(Game1.Instance.Content.Load<Texture2D>("Square"), new Rectangle(i * 40 + Width, j * 40 + Height, 40, 40), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(Game1.Instance.Content.Load<Texture2D>("greenSquare"), new Rectangle(i * 40 + Width, j * 40 + Height, 40, 40), Color.White);
                    }
                }
            }
        }
    }
    class Piece
    {
        public PieceType type;
        public Color pieceColor;

        public void PieceDraw(SpriteBatch spriteBatch, int i, int j)
        {
            if (j <= 1)
                pieceColor = Color.Black;
            else if (j >= 5)
                pieceColor = Color.Red;

            switch (type)
            {
                case PieceType.pawn:
                    spriteBatch.Draw(Game1.Instance.Content.Load<Texture2D>("pawn"), new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
                case PieceType.rook:
                    spriteBatch.Draw(Game1.Instance.Content.Load<Texture2D>("rook"), new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
            }
        }
    }
}

enum PieceType
{
    pawn,
    rook,
    king,
    queen,
    bishop,
    knight
}
