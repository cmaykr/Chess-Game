using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    public class Piece
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
                PieceType.pawn => Board.Instance.Pawn,
                PieceType.rook => Board.Instance.Rook,
                PieceType.king => Board.Instance.King,
                PieceType.bishop => Board.Instance.Bishop,
                PieceType.knight => Board.Instance.Knight,
                PieceType.queen => Board.Instance.Queen,
                _ => null,
            };

            spriteBatch.Draw(type1, new Vector2(i * Board.Instance.tileSize + Game1.boardPosition.X, j * Board.Instance.tileSize + Game1.boardPosition.Y), pieceColor);
        }
        public bool CanMove(int xIndex, int yIndex, int xTarget, int yTarget)
        {
            int xDist = Math.Abs(xTarget - xIndex);
            int yDist = Math.Abs(yIndex - yTarget);

            return type switch
            {
                PieceType.pawn => xTarget == xIndex && (yTarget == yIndex + 1 || (hasMoved == false && yTarget == yIndex + 2)),
                PieceType.rook => xTarget == xIndex || yTarget == yIndex, 
                PieceType.king => (xDist == 1 && yDist == 0) || (yDist == 1 && xDist == 0) || (xDist == yDist && (xDist == 1 || yDist == 1)),
                PieceType.bishop => xDist == yDist,
                PieceType.knight => (xDist == 2 && yDist == 1 || (yDist == 2 && xDist == 1)),
                PieceType.queen => xTarget == xIndex || yTarget == yIndex || xDist == yDist,
                _ => false,
            };
        }
    }
    public enum PieceType
    {
        pawn,
        rook,
        king,
        queen,
        bishop,
        knight
    }
}
