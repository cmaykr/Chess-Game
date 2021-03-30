using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    class Board
    {
        MouseState mouse, prev;
        int xIndex, yIndex;
        public int tileSize = 40;
        public Texture2D Tile;
        Texture2D validMoveIndicator;
        public Texture2D Pawn, Rook, Knight, King, Bishop, Queen;
        Texture2D validMoveIndicatorSquare;
        bool pieceChosen = false;
        Piece piece = new();

        public static Board Instance;

        public Board()
        {
            Instance = this;
        }

        public void BoardDraw(SpriteBatch spriteBatch, int x, int y, Piece[,] DrawPiece)
        {
            for (int i = 0; i < 8; i += 1)
            {
                for (int j = 0; j < 8; j += 1)
                {
                    bool canMove = pieceChosen && DrawPiece[xIndex, yIndex].CanMove(xIndex, yIndex, i, j);
                    Rectangle tilePos = new(i * tileSize + x, j * tileSize + y, tileSize, tileSize);

                    if (i % 2 == j % 2)
                    {
                        spriteBatch.Draw(Tile, tilePos, new(0xF4, 0xF4, 0xA2));
                    }
                    else
                    {
                        spriteBatch.Draw(Tile, tilePos, Color.DarkKhaki);
                    }

                    if (canMove && DrawPiece[i, j] == null)
                        spriteBatch.Draw(validMoveIndicator, tilePos, Color.DarkGreen);
                    else if(canMove && DrawPiece[i, j].isBlack != DrawPiece[xIndex, yIndex].isBlack)
                            spriteBatch.Draw(validMoveIndicatorSquare, tilePos, Color.DarkGreen);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (DrawPiece[i, j] != null)
                        DrawPiece[i, j].PieceDraw(spriteBatch, i, j);
                }
            }
        }

        public void PieceContent(Piece[,] DrawPiece)
        {
            Pawn = Game1.Instance.Content.Load<Texture2D>("Pawn");
            Rook = Game1.Instance.Content.Load<Texture2D>("rook");
            Knight = Game1.Instance.Content.Load<Texture2D>("knight");
            King = Game1.Instance.Content.Load<Texture2D>("king");
            Queen = Game1.Instance.Content.Load<Texture2D>("queen");
            Bishop = Game1.Instance.Content.Load<Texture2D>("bishop");

            Tile = Game1.Instance.Content.Load<Texture2D>("Square");
            validMoveIndicator = Game1.Instance.Content.Load<Texture2D>("Small_Dot");
            validMoveIndicatorSquare = Game1.Instance.Content.Load<Texture2D>("SquareDot");

            for (int i = 0; i < 8; i++)
            {
                DrawPiece[i, 1] = new Piece()
                {
                    type = PieceType.pawn,
                    isBlack = true,
                };
                DrawPiece[i, 6] = new Piece()
                {
                    type = PieceType.pawn,
                    isBlack = false,
                };
            }
            DrawPiece[0, 0] = new Piece()
            {
                type = PieceType.rook,
                isBlack = true,
            };
            DrawPiece[7, 0] = new Piece()
            {
                type = PieceType.rook,
                isBlack = true,
            };
            DrawPiece[0, 7] = new Piece()
            {
                type = PieceType.rook,
                isBlack = false,
            };
            DrawPiece[7, 7] = new Piece()
            {
                type = PieceType.rook,
                isBlack = false,
            };
            DrawPiece[1, 0] = new Piece()
            {
                type = PieceType.knight,
                isBlack = true,
            };
            DrawPiece[6, 0] = new Piece()
            {
                type = PieceType.knight,
                isBlack = true,
            };
            DrawPiece[1, 7] = new Piece()
            {
                type = PieceType.knight,
                isBlack = false,
            };
            DrawPiece[6, 7] = new Piece()
            {
                type = PieceType.knight,
                isBlack = false,
            };
            DrawPiece[2, 0] = new Piece()
            {
                type = PieceType.bishop,
                isBlack = true,
            };
            DrawPiece[5, 0] = new Piece()
            {
                type = PieceType.bishop,
                isBlack = true,
            };
            DrawPiece[2, 7] = new Piece()
            {
                type = PieceType.bishop,
                isBlack = false,
            };
            DrawPiece[5, 7] = new Piece()
            {
                type = PieceType.bishop,
                isBlack = false,
            };
            DrawPiece[3, 0] = new Piece()
            {
                type = PieceType.king,
                isBlack = true,
            };
            DrawPiece[3, 7] = new Piece()
            {
                type = PieceType.king,
                isBlack = false,
            };
            DrawPiece[4, 0] = new Piece()
            {
                type = PieceType.queen,
                isBlack = true,
            };
            DrawPiece[4, 7] = new Piece()
            {
                type = PieceType.queen,
                isBlack = false,
            };
        }

        public void PieceMove(Piece[,] DrawPiece, Vector2 boardPosition)
        {
            mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released && pieceChosen == false)
            {
                Vector2 idxVector = new((mouse.X - boardPosition.X) / tileSize, (mouse.Y - boardPosition.Y) / tileSize);
                xIndex = (int)idxVector.X;
                yIndex = (int)idxVector.Y;

                if (DrawPiece[xIndex, yIndex] != null)
                    pieceChosen = true;

            }
            else if (pieceChosen == true && mouse.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released)
            {
                int xTarget = (int)(mouse.X - boardPosition.X) / tileSize;
                int yTarget = (int)(mouse.Y - boardPosition.Y) / tileSize;

                if (xTarget == xIndex && yTarget == yIndex)
                {
                    pieceChosen = false;
                }
                else if (DrawPiece[xIndex, yIndex].CanMove(xIndex, yIndex, xTarget, yTarget) && xTarget < 8 && yTarget < 8 && xTarget > -1 && yTarget > -1)
                {
                    ///////////////////////////////////////
                    // Temporary? Maybe better way?
                    bool collision = false;
                    int tempDist;

                    if (xTarget == xIndex)
                        tempDist = Math.Abs(yTarget - yIndex);
                    else
                        tempDist = Math.Abs(xTarget - xIndex);

                    for (int i = 1; i < tempDist; i++)
                    {
                        if (DrawPiece[xIndex, yIndex].type == PieceType.knight)
                            break;  

                        if (xIndex == xTarget)
                            collision = piece.CheckCollision(xIndex, (yTarget < yIndex) ? yIndex - i : yIndex + i);
                        else if (yIndex == yTarget)
                            collision = piece.CheckCollision((xTarget < xIndex) ? xIndex - i : xIndex + i, yIndex);
                        else if (Math.Abs(xTarget - xIndex) == Math.Abs(yTarget - yIndex))
                            collision = piece.CheckCollision((xTarget < xIndex) ? xIndex - i : xIndex + i, (yTarget < yIndex) ? yIndex - i : yIndex + i);

                        if (collision == true)
                            break;
                    }
                    ///////////////////////////////////////////

                    if ((DrawPiece[xTarget, yTarget] == null || DrawPiece[xTarget, yTarget].isBlack != DrawPiece[xIndex, yIndex].isBlack) && collision != true)
                    {
                        DrawPiece[xIndex, yIndex].hasMoved = true;
                        DrawPiece[xTarget, yTarget] = DrawPiece[xIndex, yIndex];
                        DrawPiece[xIndex, yIndex] = null;
                    }
                }
                pieceChosen = false;
            }
            prev = mouse;
        }
    }
}
