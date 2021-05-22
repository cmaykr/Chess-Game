using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    public class PieceMovement
    {
        public int Turns { get; private set; } = 0;

        static (int xKing, int yKing) GetKing(Piece[,] Pieces) 
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (Pieces[x, y] != null && Pieces[x, y].type == PieceType.King && Pieces[x, y].isBlack != Board.Instance.IsPlayerOne)
                    {
                        return (x, y);
                    }
                }
            }
            return (-1, -1);
        }
        static bool Check(Piece[,] Pieces)
        {
            var (xKing, yKing) = GetKing(Pieces);

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (Pieces[x, y] != null && Pieces[x, y].CanMove(Pieces, x, y, xKing, yKing) && !Piece.Collision(Pieces, x, y, xKing, yKing) && Pieces[x, y].isBlack != Pieces[xKing, yKing].isBlack)
                    {
                        if (!(Pieces[x, y].type == PieceType.Pawn && x == xKing))
                            return true;
                    }
                }
            }
            return false;
        }

        public static bool WillMoveCauseCheck(Piece[,] Pieces, int xIndex, int yIndex, int xTarget, int yTarget)
        {
            Piece[,] tempBoard = (Piece[,])Pieces.Clone();
            var piece = tempBoard[xIndex, yIndex];
            tempBoard[xIndex, yIndex] = null;
            tempBoard[xTarget, yTarget] = piece;

            return Check(tempBoard);
        }

        public static bool IsCheckMate(Piece[,] Pieces)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (Pieces[x, y] != null && Pieces[x, y].isBlack != Board.Instance.IsPlayerOne)
                            {
                                if (Pieces[x, y].CanMove(Pieces, x, y, i, j) && !Piece.Collision(Pieces, x, y, i, j) && !WillMoveCauseCheck(Pieces, x, y, i, j))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void MoveChosenPiece(Piece[,] Pieces, int xIndex, int yIndex, Vector2 boardPosition)
        {
            int xTarget = (int)(Board.Instance.curr.X - boardPosition.X) / (int)Board.Instance.TileSize.X;
            int yTarget = (int)(Board.Instance.curr.Y - boardPosition.Y) / (int)Board.Instance.TileSize.Y;

            // Kollar om det är tillåtet att flytta pjäsen till den valda positionen.
            if (xTarget == xIndex && yTarget == yIndex)
            {
                Board.Instance.pieceChosen = false;
            }
            else if (xTarget < 8 && yTarget < 8 && xTarget > -1 && yTarget > -1
                && Pieces[xIndex, yIndex].CanMove(Pieces, xIndex, yIndex, xTarget, yTarget))
            {
                if ((
                        Pieces[xTarget, yTarget] == null
                        || Pieces[xTarget, yTarget].isBlack != Pieces[xIndex, yIndex].isBlack
                    )
                    && !Piece.Collision(Pieces, xIndex, yIndex, xTarget, yTarget)
                    && !WillMoveCauseCheck(Pieces, xIndex, yIndex, xTarget, yTarget))
                {
                    Board.Instance.gameUI.AddNotation(Pieces, xIndex, yIndex, xTarget, yTarget);
                    Pieces[xIndex, yIndex].hasMoved = true;
                    Pieces[xTarget, yTarget] = Pieces[xIndex, yIndex];
                    Pieces[xIndex, yIndex] = null;
                    Board.Instance.HasCastled(Pieces, xIndex, yTarget, xTarget);
                    Board.Instance.EnPassant(Pieces);

                    Board.Instance.xLastMove = xIndex;
                    Board.Instance.yLastMove = yIndex;
                    Board.Instance.xLastMoveTarget = xTarget;
                    Board.Instance.yLastMoveTarget = yTarget;
                    Turns += 1;

                    Board.Instance.ApplyTimeIncrement();

                    Board.Instance.IsPlayerOne = !Board.Instance.IsPlayerOne;
                    Board.Instance.CheckMate = IsCheckMate(Pieces);
                }
            }
            Board.Instance.pieceChosen = false;
        }
    }
}
