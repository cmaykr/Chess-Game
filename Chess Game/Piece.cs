using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chess_Game
{
    class Piece
    {
        public PieceType type;
        public Color pieceColor;

        public void PieceDraw(SpriteBatch spriteBatch, int i, int j)
        {
            switch (type)
            {
                case PieceType.pawn:
                    spriteBatch.Draw(Game1.Instance.pawn, new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
                case PieceType.rook:
                    spriteBatch.Draw(Game1.Instance.rook, new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
                case PieceType.king:
                    break;
                case PieceType.bishop:
                    break;
                case PieceType.knight:
                    break;
                case PieceType.queen:
                    break;

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
}
