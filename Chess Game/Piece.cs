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
        public bool hasMoved;

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



            var type1 = type switch
            {
                PieceType.pawn => Board.Instance.pawn,
                PieceType.rook => Board.Instance.rook,
                PieceType.king => Board.Instance.king,
                PieceType.bishop => Board.Instance.bishop,
                PieceType.knight => Board.Instance.knight,
                PieceType.queen => Board.Instance.queen,
                _ => null,
            };

            spriteBatch.Draw(type1, new Vector2(i * Board.Instance.tileSize + Game1.boardPosition.X, j * Board.Instance.tileSize + Game1.boardPosition.Y), pieceColor);
        }
        public bool CanMove(int xIndex, int yIndex, int xTarget, int yTarget)
        {
            return type switch
            {
                PieceType.pawn => xTarget == xIndex && (yTarget == yIndex + 1 || (hasMoved == false && yTarget == yIndex + 2)),
                PieceType.rook => xTarget == xIndex || yTarget == yIndex, 
                PieceType.king => Math.Abs(xTarget - xIndex) == 1 || Math.Abs(yTarget - yIndex) == 1,
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
