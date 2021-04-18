using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    /// <summary>
    /// Board klassen är den som ritar spelbrädet och instanserar pjäserna,
    /// Board används också för att flytta pjäserna.
    /// </summary>
    class Board
    {
        MouseState mouse, prev;
        int xIndex, yIndex;
        public int TileSize { get; init; } = 40;
        private Texture2D tile;
        public Texture2D Tile => tile;
        Texture2D validMoveIndicator;
        public Texture2D Pawn, Rook, Knight, King, Bishop, Queen;
        Texture2D validMoveIndicatorSquare;
        bool pieceChosen = false;
        Piece piece = new();
        bool isPlayerOne;

        public static Board Instance;

        public Board()
        {
            Instance = this;
        }

        /// <summary>
        /// Ritar spelbrädet, 
        /// visar också var man kan flytta en pjäs.
        /// </summary>
        public void BoardDraw(SpriteBatch spriteBatch, int x, int y, Piece[,] DrawPiece)
        {
            for (int i = 0; i < 8; i += 1)
            {
                for (int j = 0; j < 8; j += 1)
                {
                    bool canMove = pieceChosen && DrawPiece[xIndex, yIndex].CanMove(xIndex, yIndex, i, j) && !piece.Collision(xIndex, yIndex, i, j);
                    Rectangle tilePos = new(i * TileSize + x, j * TileSize + y, TileSize, TileSize);

                    if (i % 2 == j % 2)
                    {
                        spriteBatch.Draw(Tile, tilePos, new(0x94, 0x6f, 0x51));
                    }
                    else
                    {
                        spriteBatch.Draw(Tile, tilePos, new(0xF0, 0xD9, 0xB5));
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

        /// <summary>
        /// Instanserar alla bilderna.
        /// Bestämmer var pjäserna ska finnas i arrayen.
        /// </summary>
        public void PieceContent(Piece[,] DrawPiece)
        {
            Pawn = Game1.Instance.Content.Load<Texture2D>("Pawn");
            Rook = Game1.Instance.Content.Load<Texture2D>("rook");
            Knight = Game1.Instance.Content.Load<Texture2D>("knight");
            King = Game1.Instance.Content.Load<Texture2D>("king");
            Queen = Game1.Instance.Content.Load<Texture2D>("queen");
            Bishop = Game1.Instance.Content.Load<Texture2D>("bishop");

            tile = Game1.Instance.Content.Load<Texture2D>("Square");
            validMoveIndicator = Game1.Instance.Content.Load<Texture2D>("Small_Dot");
            validMoveIndicatorSquare = Game1.Instance.Content.Load<Texture2D>("SquareDot");

            string[] pieceCoord = new[]
            {
                "PPPPPPPP",
                "RGBKQBGR"
            };

            for (int i = 0; i < pieceCoord.Length; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    char charPiece = pieceCoord[pieceCoord.Length - i - 1][j];

                    var type = new Piece().type;

                    type = charPiece switch
                    {
                        'P' => PieceType.pawn,
                        'R' => PieceType.rook,
                        'G' => PieceType.knight,
                        'B' => PieceType.bishop,
                        'K' => PieceType.king,
                        'Q' => PieceType.queen,
                        _ => throw new System.Exception("Unkown"),
                    };

                    DrawPiece[j, i] = new Piece()
                    {
                        type = type,
                        isBlack = true,
                    };
                    DrawPiece[j, 7 - i] = new Piece()
                    {
                        type = type,
                        isBlack = false,
                    };
                }
            }
        }

        /// <summary>
        /// Metod för att välja och flytta pjäserna.
        /// </summary>
        public void PieceMove(Piece[,] DrawPiece, Vector2 boardPosition)
        {
            mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released && pieceChosen == false)
            {
                Vector2 idxVector = new((mouse.X - boardPosition.X) / TileSize, (mouse.Y - boardPosition.Y) / TileSize);
                xIndex = (int)idxVector.X;
                yIndex = (int)idxVector.Y;

                if (xIndex > -1 && xIndex < 8 && yIndex > -1 && yIndex < 8 && DrawPiece[xIndex, yIndex] != null && DrawPiece[xIndex, yIndex].isBlack == isPlayerOne)
                    pieceChosen = true;

            }
            else if (pieceChosen == true && mouse.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released)
            {
                int xTarget = (int)(mouse.X - boardPosition.X) / TileSize;
                int yTarget = (int)(mouse.Y - boardPosition.Y) / TileSize;

                if (xTarget == xIndex && yTarget == yIndex)
                {
                    pieceChosen = false;
                }
                else if (DrawPiece[xIndex, yIndex].CanMove(xIndex, yIndex, xTarget, yTarget) 
                    && xTarget < 8 && yTarget < 8 
                    && xTarget > -1 && yTarget > -1)
                {
                    if ((DrawPiece[xTarget, yTarget] == null || DrawPiece[xTarget, yTarget].isBlack != DrawPiece[xIndex, yIndex].isBlack) && !piece.Collision(xIndex, yIndex, xTarget, yTarget))
                    {
                        isPlayerOne = !isPlayerOne;
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
