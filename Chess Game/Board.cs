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
        public void boardDraw(SpriteBatch spriteBatch, int Width, int Height)
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
class Piece
{
    public PieceType type;

    public void pieceDraw(SpriteBatch spriteBatch, int i, int j, Vector2 boardPosition, Texture2D pawn)
    {
        bool isWhite = false;
        if (j < 2)
            isWhite = false;
        else if (j > 6)
            isWhite = true;

        if (isWhite == false)
        {
            switch (type)
            {
                case PieceType.pawn:
                    spriteBatch.Draw(pawn, new Vector2(i*40+boardPosition.X, j * 40 + boardPosition.Y), Color.Black);
                    break;
                case PieceType.rook:
                        
                    break;
            }
        }
        else if (isWhite == true)
        {

        }
    }
}