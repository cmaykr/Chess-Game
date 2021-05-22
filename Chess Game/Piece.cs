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
                PieceType.Pawn => Board.Instance.Pawn,
                PieceType.Rook => Board.Instance.Rook,
                PieceType.King => Board.Instance.King,
                PieceType.Bishop => Board.Instance.Bishop,
                PieceType.Knight => Board.Instance.Knight,
                PieceType.Queen => Board.Instance.Queen,
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
        public bool CanMove(Piece[,] Pieces, int xIndex, int yIndex, int xTarget, int yTarget)
        {
            if (xTarget < 0 || xTarget > 8 || yTarget < 0 || yTarget > 8)
                return false;
            if (Pieces[xTarget, yTarget] != null && Pieces[xIndex, yIndex].isBlack == Pieces[xTarget, yTarget].isBlack)
                return false;

            int xTargetTemp = xTarget;
            int yTargetTemp = yTarget;
            int xIndexTemp = xIndex;
            int yIndexTemp = yIndex;

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
                PieceType.Pawn => (xTarget == xIndex && (yTarget == yIndex + 1 
                    || (hasMoved == false && yTarget == yIndex + 2)) 
                    && Game1.Instance.Pieces[xTargetTemp, yTargetTemp] == null) 
                    || (xDist == 1 && yTarget == yIndex + 1 && PawnDiagonalAttack(Pieces, xTargetTemp, yTargetTemp))
                    || EnPassant(Pieces, xIndexTemp, yIndexTemp, xTargetTemp, yTargetTemp),
                PieceType.Rook => xTarget == xIndex || yTarget == yIndex, 
                PieceType.King => (xDist == 1 && yDist == 0) 
                    || (yDist == 1 && xDist == 0) 
                    || (xDist == yDist && (xDist == 1 || yDist == 1))
                    || TryCastling(Pieces, xIndexTemp, yIndexTemp, xTargetTemp, yTargetTemp, xDist),
                PieceType.Bishop => xDist == yDist,
                PieceType.Knight => xDist == 2 && yDist == 1 || (yDist == 2 && xDist == 1),
                PieceType.Queen => xTarget == xIndex || yTarget == yIndex || xDist == yDist,
                _ => false,
            };
        }

        /// <summary>
        /// Metod som returnar om bondepjäsen
        /// kan gå diagonalt för att ta ut en pjäs.
        /// </summary>
        static bool PawnDiagonalAttack(Piece[,] Pieces, int xTarget, int yTarget)
        {
            if (Pieces[xTarget, yTarget] != null)
                return true;
            return false;
        }

        // Doesn't do anything yet
        bool EnPassant(Piece[,] Pieces, int xIndex, int yIndex, int xTarget, int yTarget)
        {
            PieceMovement MovePiece = new();

            int yLastMove = MovePiece.yLastMove;
            int xLastMoveTarget = MovePiece.xLastMoveTarget;
            int yLastMoveTarget = MovePiece.yLastMoveTarget;

            if (Pieces[xLastMoveTarget, yLastMoveTarget].type == PieceType.Pawn && Math.Abs(yLastMoveTarget - yLastMove) == 2)
            {
                if (yIndex == yLastMoveTarget && Math.Abs(xIndex - xLastMoveTarget) == 1)
                {
                    if (xTarget == xLastMoveTarget && yTarget == (isBlack ? yLastMoveTarget + 1 : yLastMoveTarget - 1))
                    {
                        MovePiece.hasEnPassant = true;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Metod för specialregeln Castling, vilket är att kungen flyttar 2 steg åt sidan istället för 1 steg.
        /// Castling använder också ett av tornen för att regeln ska gälla, och både kungen och tornet får inte ha flyttats innan.
        /// </summary>
        /// <param name="BoardPiece"></param>
        /// <param name="xIndex"></param>
        /// <param name="yIndex"></param>
        /// <param name="xTarget"></param>
        /// <param name="yTarget"></param>
        /// <param name="xDist"></param>
        /// <returns>Returnerar true om kungen får castla</returns>
        static bool TryCastling(Piece[,] BoardPiece, int xIndex, int yIndex, int xTarget, int yTarget, int xDist)
        {

            if (BoardPiece[xIndex, yIndex].hasMoved)
                return false;

            int xCastlingRook;
            if (xTarget < xIndex)
                xCastlingRook = 0;
            else
                xCastlingRook = 7;

            if (yTarget == yIndex && xDist == 2 && !Collision(BoardPiece, xIndex, yIndex, xCastlingRook, yTarget))
            {
                if (!BoardPiece[xCastlingRook, yIndex].hasMoved)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returnerar om det är en pjäs mellan den valda pjäsen och den valda positionen.
        /// </summary>
        public static bool Collision(Piece[,] Pieces, int xIndex, int yIndex, int xTarget, int yTarget)
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
                if (Pieces[xIndex, yIndex].type == PieceType.Knight)
                    break;

                int x = (xTarget < xIndex)
                    ? xIndex - i
                    : xIndex + i;
                int y = (yTarget < yIndex)
                    ? yIndex - i
                    : yIndex + i;

                // Kollar om pjäsen flyttas diagonalt, horisontalt eller vertikalt.
                if (xIndex == xTarget)
                    collision = Pieces[xIndex, y] != null;
                else if (yIndex == yTarget)
                    collision = Pieces[x, yIndex] != null;
                else if (Math.Abs(xTarget - xIndex) == Math.Abs(yTarget - yIndex))
                    collision = Pieces[x, y] != null;

                i++;
            }
            return collision;
        }
    }
    public enum PieceType
    {
        Pawn,
        Rook,
        King,
        Queen,
        Bishop,
        Knight
    }
}
