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

            Rectangle piecePos = new(
                i * Board.Instance.tileSize + (int)Game1.boardPosition.X,
                j * Board.Instance.tileSize + (int)Game1.boardPosition.Y,
                Board.Instance.tileSize,
                Board.Instance.tileSize
            );
            spriteBatch.Draw(type1, piecePos, pieceColor);
        }
        public bool CanMove(int xIndex, int yIndex, int xTarget, int yTarget)
        {
            if (!isBlack)
            {
                yIndex = 7 - yIndex;
                yTarget = 7 - yTarget;
                xIndex = 7 - xIndex;
                xTarget = 7 - xTarget;
            }
            int xDist = Math.Abs(xTarget - xIndex);
            int yDist = Math.Abs(yIndex - yTarget);

            return type switch
            {
                PieceType.pawn => xTarget == xIndex && (yTarget == yIndex + 1 || (hasMoved == false && yTarget == yIndex + 2)),
                PieceType.rook => xTarget == xIndex || yTarget == yIndex, 
                PieceType.king => (xDist == 1 && yDist == 0) || (yDist == 1 && xDist == 0) || (xDist == yDist && (xDist == 1 || yDist == 1)),
                PieceType.bishop => xDist == yDist,
                PieceType.knight => xDist == 2 && yDist == 1 || (yDist == 2 && xDist == 1),
                PieceType.queen => xTarget == xIndex || yTarget == yIndex || xDist == yDist,
                _ => false,
            };
        }
        public bool CheckCollision(int x, int y)
        {
            if (Game1.Instance.DrawPiece[x, y] == null)
                return false;

            return true;
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
