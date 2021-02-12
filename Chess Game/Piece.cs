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
        public bool isBlack;

        public void PieceDraw(SpriteBatch spriteBatch, int i, int j)
        {
            Color pieceColor;

            if (isBlack)
            {
                pieceColor = Color.Black;
            }
            else
            {
                pieceColor = Color.White;
            }

            switch (type)
            {
                case PieceType.pawn:
                    spriteBatch.Draw(Board.Instance.pawn, new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
                case PieceType.rook:
                    spriteBatch.Draw(Board.Instance.rook, new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
                case PieceType.king:
                    spriteBatch.Draw(Board.Instance.king, new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
                case PieceType.bishop:
                    spriteBatch.Draw(Board.Instance.bishop, new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
                case PieceType.knight:
                    spriteBatch.Draw(Board.Instance.knight, new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
                case PieceType.queen:
                    spriteBatch.Draw(Board.Instance.queen, new Vector2(i * 40 + Game1.boardPosition.X, j * 40 + Game1.boardPosition.Y), pieceColor);
                    break;
            }
        }
        public bool CanMove(int xIndex, int yIndex, int xTarget, int yTarget)
        {
            return type switch
            {
                PieceType.pawn => xTarget == xIndex && yTarget == yIndex + 2 && yIndex == 1 || (yTarget == yIndex + 1 && xTarget == xIndex),
                PieceType.rook => xTarget == xIndex || yTarget == yIndex,
                PieceType.king => Math.Abs(xTarget - xIndex) == 1,
                PieceType.bishop => Math.Abs(xIndex - xTarget) == Math.Abs(yIndex - yTarget),
                PieceType.knight => (Math.Abs(xTarget - xIndex) == 2 && Math.Abs(yTarget - yIndex) == 1 || (Math.Abs(yTarget - yIndex) == 2 && Math.Abs(xTarget - xIndex) == 1)),
                PieceType.queen => xTarget == xIndex || yTarget == yIndex || Math.Abs(xIndex - xTarget) == Math.Abs(yIndex - yTarget),
                _ => false,
            };
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
