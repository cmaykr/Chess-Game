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

        /// <summary>
        /// Metod för att bestämma position, färg och bilden för pjäserna.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="xCoord">Positionen på spelbrädet i X-led</param>
        /// <param name="yCoord">Positionen på spelbrädet i Y-led</param>
        public void PieceDraw(SpriteBatch spriteBatch, int xCoord, int yCoord)
        {
            Color pieceColor;

            if (isBlack)
            {
                pieceColor = new(16, 19, 20);
            }
            else
            {
                pieceColor = Color.White;
            }

            // Bestämmer vilken bild som pjäserna ska använda.
            var type = this.type switch
            {
                PieceType.pawn => Board.Instance.Pawn,
                PieceType.rook => Board.Instance.Rook,
                PieceType.king => Board.Instance.King,
                PieceType.bishop => Board.Instance.Bishop,
                PieceType.knight => Board.Instance.Knight,
                PieceType.queen => Board.Instance.Queen,
                _ => null,
            };

            // Bestämmer positionen alla pjäserna ska ha på skärmen.
            Rectangle piecePos = new(
                xCoord * (int)Board.Instance.TileSize.X + (int)Game1.boardPosition.X,
                yCoord * (int)Board.Instance.TileSize.Y + (int)Game1.boardPosition.Y,
                (int)Board.Instance.TileSize.X,
                (int)Board.Instance.TileSize.Y
            );
            spriteBatch.Draw(type, piecePos, pieceColor);
        }

        /// <summary>
        /// Metod returnar om pjäsen får flytta till den positionen man valt.
        /// </summary>
        /// <param name="xIndex">X koordinaten på spelbrädet för den valda pjäsen.</param>
        /// <param name="yIndex">Y koordinaten på spelbrädet för den valda pjäsen.</param>
        /// <param name="xTarget">X koordinaten på spelbrädet man vill flytta pjäsen till</param>
        /// <param name="yTarget">Y koordinaten på spelbrädet man vill flytta pjäsen till</param>
        /// <returns></returns>
        public bool CanMove(Piece[,] DrawPiece, int xIndex, int yIndex, int xTarget, int yTarget)
        {
            if (DrawPiece[xTarget, yTarget] != null && DrawPiece[xIndex, yIndex].isBlack == DrawPiece[xTarget, yTarget].isBlack)
                return false;

            int xTemp = xTarget;
            int yTemp = yTarget;
            if (!isBlack)
            {
                yIndex = 7 - yIndex;
                yTarget = 7 - yTarget;
                xIndex = 7 - xIndex;
                xTarget = 7 - xTarget;
            }
            int xDist = Math.Abs(xTarget - xIndex);
            int yDist = Math.Abs(yIndex - yTarget);

            // returnerar hur den valda pjäsen kan flyttas.
            return type switch
            {
                PieceType.pawn => (xTarget == xIndex && (yTarget == yIndex + 1 
                    || (hasMoved == false && yTarget == yIndex + 2)) 
                    && Game1.Instance.DrawPiece[xTemp, yTemp] == null) 
                    || (xDist == 1 && yTarget == yIndex + 1 && PawnDiagonalAttack(DrawPiece, xTemp, yTemp)),
                PieceType.rook => xTarget == xIndex || yTarget == yIndex, 
                PieceType.king => (xDist == 1 && yDist == 0) 
                    || (yDist == 1 && xDist == 0) 
                    || (xDist == yDist && (xDist == 1 || yDist == 1)),
                PieceType.bishop => xDist == yDist,
                PieceType.knight => xDist == 2 && yDist == 1 || (yDist == 2 && xDist == 1),
                PieceType.queen => xTarget == xIndex || yTarget == yIndex || xDist == yDist,
                _ => false,
            };
        }

        /// <summary>
        /// Metod som returnar om bondepjäsen
        /// kan gå diagonalt för att ta ut en pjäs.
        /// </summary>
        static bool PawnDiagonalAttack(Piece[,] DrawPiece, int xTarget, int yTarget)
        {
            if (DrawPiece[xTarget, yTarget] != null)
                return true;
            return false;
        }

        /// <summary>
        /// Returnerar om det är en pjäs mellan den valda pjäsen och den valda positionen.
        /// </summary>
        public bool Collision(Piece[,] DrawPiece, int xIndex, int yIndex, int xTarget, int yTarget)
        {
            bool collision = false;
            int tempDist;

            if (xTarget == xIndex)
                tempDist = Math.Abs(yTarget - yIndex);
            else
                tempDist = Math.Abs(xTarget - xIndex);
            int i = 1;

            // Går igenom alla rutor mellan två pjäser och kollar om det är en annan pjäs mellan dem. 
            while (i < tempDist && collision == false)
            {
                if (DrawPiece[xIndex, yIndex].type == PieceType.knight)
                    break;

                int x = (xTarget < xIndex)
                    ? xIndex - i
                    : xIndex + i;
                int y = (yTarget < yIndex)
                    ? yIndex - i
                    : yIndex + i;

                // Kollar om pjäsen flyttas diagonalt, horisontalt eller vertikalt.
                if (xIndex == xTarget)
                    collision = DrawPiece[xIndex, y] != null;
                else if (yIndex == yTarget)
                    collision = DrawPiece[x, yIndex] != null;
                else if (Math.Abs(xTarget - xIndex) == Math.Abs(yTarget - yIndex))
                    collision = DrawPiece[x, y] != null;

                i++;
            }
            return collision;
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
