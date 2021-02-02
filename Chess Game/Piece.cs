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
        public void PawnRule(int xIndex, int yIndex, int xTemp, int yTemp, Piece[,] DrawPiece)
        {
            if (xTemp == xIndex && yTemp == yIndex + 2 && DrawPiece[xIndex, yIndex] == DrawPiece[xIndex, 1] || (yTemp ==  yIndex + 1 && xTemp == xIndex))
            {
                DrawPiece[xTemp, yTemp] = DrawPiece[xIndex, yIndex];
                DrawPiece[xIndex, yIndex] = null;

                System.Diagnostics.Debug.WriteLine("Pawn moved");
                System.Diagnostics.Debug.WriteLine("temp: "+ yTemp + " y: "+ yIndex);
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
