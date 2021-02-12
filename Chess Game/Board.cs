using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chess_Game
{
    class Board
    {
        MouseState mouse, prev;
        int xIndex, yIndex = -1;
        bool pieceChosen = false;
        public Texture2D pawn, rook, knight, king, bishop, queen;

        public static Board Instance;

        public Board()
        {
            Instance = this;
        }

        public void BoardDraw(SpriteBatch spriteBatch, int Width, int Height)
        {
            for (int i = 0; i < 8; i += 1)
            {
                for (int j = 0; j < 8; j += 1)
                {
                    if (i % 2 == j % 2)
                    {
                        spriteBatch.Draw(Game1.Instance.Content.Load<Texture2D>("Square"), new Rectangle(i * 40 + Width, j * 40 + Height, 40, 40), new Color(244,244,162));
                    }
                    else
                    {
                        spriteBatch.Draw(Game1.Instance.Content.Load<Texture2D>("Square"), new Rectangle(i * 40 + Width, j * 40 + Height, 40, 40), Color.DarkKhaki);
                    }
                }
            }
        }

        public void PieceContent(Piece[,] DrawPiece)
        {

            pawn = Game1.Instance.Content.Load<Texture2D>("Pawn");
            rook = Game1.Instance.Content.Load<Texture2D>("rook");
            knight = Game1.Instance.Content.Load<Texture2D>("knight");
            king = Game1.Instance.Content.Load<Texture2D>("king");
            queen = Game1.Instance.Content.Load<Texture2D>("queen");
            bishop = Game1.Instance.Content.Load<Texture2D>("bishop");

            // Easier or better way to do this?
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
                Vector2 idxVector = new Vector2((mouse.X - boardPosition.X) / 40, (mouse.Y - boardPosition.Y) / 40);
                xIndex = (int)idxVector.X;
                yIndex = (int)idxVector.Y;

                if (DrawPiece[xIndex, yIndex] != null)
                    pieceChosen = true;

                System.Diagnostics.Debug.WriteLine("x: " + xIndex + " y: " + yIndex);
            }
            else if (pieceChosen == true && mouse.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released)
            {
                int xTarget = (int)(mouse.X - boardPosition.X) / 40;
                int yTarget = (int)(mouse.Y - boardPosition.Y) / 40;

                if (xTarget == xIndex && yTarget == yIndex)
                {
                    pieceChosen = false;
                    System.Diagnostics.Debug.WriteLine(pieceChosen);
                }
                else if (DrawPiece[xIndex, yIndex].CanMove(xIndex, yIndex, xTarget, yTarget))
                {
                    DrawPiece[xTarget, yTarget] = DrawPiece[xIndex, yIndex];
                    DrawPiece[xIndex, yIndex] = null;

                    System.Diagnostics.Debug.WriteLine("Pawn moved");
                    System.Diagnostics.Debug.WriteLine("temp: " + yTarget + " y: " + yIndex);
                    
                }
                System.Diagnostics.Debug.WriteLine("x-y: " + Math.Abs(xTarget - xIndex));
                System.Diagnostics.Debug.WriteLine("xTemp: " + xTarget + " yTemp: " + yTarget);
                pieceChosen = false;
            }
            prev = mouse;
        }
    }
}