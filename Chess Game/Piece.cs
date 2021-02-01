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
                    spriteBatch.Draw(Game1.Instance.king, new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
                case PieceType.bishop:
                    spriteBatch.Draw(Game1.Instance.bishop, new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
                case PieceType.knight:
                    spriteBatch.Draw(Game1.Instance.knight, new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
                case PieceType.queen:
                    spriteBatch.Draw(Game1.Instance.queen, new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
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
