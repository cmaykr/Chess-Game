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
        public bool hasEnPassant;
        public int xLastMoveTarget, yLastMoveTarget;
        public int xLastMove, yLastMove;

        public float playerTwoTimer = 600f;
        public float playerOneTimer = 600f;
        readonly float timeIncrement = 10f;

        /// <summary>
        /// Hämtar positionen av motsatta spelarens kung.
        /// </summary>
        /// <param name="Pieces">Spelbrädet som metoden hämtar kungens koordinater ifrån.</param>
        /// <returns>Returnerar x och y koordinaterna för kungen.</returns>
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

        /// <summary>
        /// Kollar om kungen är schackad på det valda spelbrädet.
        /// </summary>
        /// <param name="Pieces">Själva spelbrädet som metoden använder.</param>
        /// <returns>Returnerar true om det är schack.</returns>
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

        /// <summary>
        /// Metod som kollar om draget man gjort kommer orsaka schack för sin egna kung.
        /// </summary>
        /// <param name="Pieces">Spelbrädet som används.</param>
        /// <param name="xIndex">X koordinaten för den valda pjäsen.</param>
        /// <param name="yIndex">Y koordinaten för den valda pjäsen.</param>
        /// <param name="xTarget">X koordinaten för dit den valda pjäsen ska flytta till.</param>
        /// <param name="yTarget">Y koordinaten för dit den valda pjäsen ska flytta till.</param>
        /// <returns>Returnerar om det draget man gjort kommer schacka sin kung.</returns>
        public static bool WillMoveCauseCheck(Piece[,] Pieces, int xIndex, int yIndex, int xTarget, int yTarget)
        {
            Piece[,] tempBoard = (Piece[,])Pieces.Clone();
            var piece = tempBoard[xIndex, yIndex];
            tempBoard[xIndex, yIndex] = null;
            tempBoard[xTarget, yTarget] = piece;

            return Check(tempBoard);
        }

        /// <summary>
        /// Metod som kollar om sin egna kung är i schackmatt.
        /// </summary>
        /// <param name="Pieces">Spelbrädet som används till metoden.</param>
        /// <returns>Returnerar true om ingen av spelarens pjäser kan flytta och kungen är schackad.</returns>
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

        /// <summary>
        /// Metod för att kolla om den valda pjäsen får flytta till en ruta.
        /// Metoden flyttar också den valda pjäsen.
        /// </summary>
        /// <param name="Pieces"></param>
        /// <param name="xIndex"></param>
        /// <param name="yIndex"></param>
        /// <param name="boardPosition"></param>
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
                    HasCastled(Pieces, xIndex, yTarget, xTarget);
                    EnPassant(Pieces);

                    xLastMove = xIndex;
                    yLastMove = yIndex;
                    xLastMoveTarget = xTarget;
                    yLastMoveTarget = yTarget;
                    Turns += 1;

                    ApplyTimeIncrement();

                    Board.Instance.IsPlayerOne = !Board.Instance.IsPlayerOne;
                    Board.Instance.CheckMate = IsCheckMate(Pieces);
                }
            }
            Board.Instance.pieceChosen = false;
        }

        public void HasCastled(Piece[,] Pieces, int xIndex, int yTarget, int xTarget)
        {
            int xCastlingRook;
            if (xTarget < xIndex)
                xCastlingRook = 0;
            else
                xCastlingRook = 7;

            int xDist = Math.Abs(xTarget - xIndex);
            if ((Board.Instance.IsPlayerOne ? yTarget == 7 : yTarget == 0) && Pieces[xTarget, yTarget] != null && xDist == 2 && Pieces[xTarget, yTarget].type == PieceType.King)
            {
                Pieces[xCastlingRook, yTarget].hasMoved = true;
                if (xTarget < xIndex)
                    Pieces[xIndex - 1, yTarget] = Pieces[xCastlingRook, yTarget];
                else
                    Pieces[xIndex + 1, yTarget] = Pieces[xCastlingRook, yTarget];
                Pieces[xCastlingRook, yTarget] = null;
            }
        }

        public void EnPassant(Piece[,] Pieces)
        {
            if (hasEnPassant)
            {
                Pieces[xLastMoveTarget, yLastMoveTarget] = null;
                hasEnPassant = false;
            }
        }

        public void ApplyTimeIncrement()
        {
            if (!Board.Instance.timerRun)
            {
                return;
            }

            if (Board.Instance.IsPlayerOne)
            {
                playerOneTimer += timeIncrement;
            }
            else
            {
                playerTwoTimer += timeIncrement;
            }
        }

        public void DecrementTimer(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!Board.Instance.CheckMate && Board.Instance.timerRun)
            {
                if (Board.Instance.IsPlayerOne)
                {
                    playerOneTimer -= dt;
                }
                else
                {
                    playerTwoTimer -= dt;
                }
            }
        }
    }
}
