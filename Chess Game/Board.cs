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
        MouseState curr, prev;
        int xIndex, yIndex;
        public Vector2 TileSize { get; private set; } = new(40,40);
        private Texture2D tile;
        public Texture2D Tile => tile;
        Texture2D validMoveIndicator;
        public Texture2D Pawn, Rook, Knight, King, Bishop, Queen;
        Texture2D validMoveIndicatorSquare;
        bool pieceChosen = false;
        Piece piece = new();
        DebugMode DebugMode = new();
        public bool isPlayerOne;
        public SpriteFont font;
        bool debug;
        public bool checkMate;

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
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
                debug = true;
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
                debug = false;

            for (int i = 0; i < 8; i += 1)
            {
                for (int j = 0; j < 8; j += 1)
                {
                    bool canMove = pieceChosen && DrawPiece[xIndex, yIndex].CanMove(xIndex, yIndex, i, j) && !piece.Collision(xIndex, yIndex, i, j);
                    Rectangle tilePos = new(i * (int)TileSize.X + x, j * (int)TileSize.Y + y, (int)TileSize.X, (int)TileSize.Y);

                    Color boardColor;

                    // Bestämmer färgen på rutorna.
                    if (i % 2 == j % 2)
                        boardColor = new(0x94, 0x6f, 0x51);
                    else
                        boardColor = new(0xF0, 0xD9, 0xB5);
                    // Om den rutan är där sin valda pjäs finns.
                    if (pieceChosen && i == xIndex && j == yIndex)
                        boardColor = Color.Green;

                    // Ritar själva spelbrädet
                    spriteBatch.Draw(Tile, tilePos, boardColor);
                    
                    // Ritar var pjäsen får flytta och om det är en fiendepjäs på rutan
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
                    // Ritar pjäserna.
                    if (DrawPiece[i, j] != null)
                        DrawPiece[i, j].PieceDraw(spriteBatch, i, j);

                    // Ritar koordinaterna för brädet om debug är PÅ
                    if (debug)
                        DebugMode.BoardCoord(spriteBatch, i * (int)TileSize.X + x, j * (int)TileSize.Y + y, i, j);
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
            font = Game1.Instance.Content.Load<SpriteFont>("Arial");
            
            // Bestämmer var pjäserna ska finnas på spelbrädet för båda sidorna.
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

                    // Bestämmer vilken bokstav som tillhör vilken pjäs
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

                    // Bestämmer positionerna i DrawPiece arrayen var de enskilda pjäserna ska finnas 
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
            curr = Mouse.GetState();

            if (!checkMate && curr.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released && pieceChosen == false)
            {
                Vector2 idxVector = new((curr.X - boardPosition.X) / TileSize.X, (curr.Y - boardPosition.Y) / TileSize.Y);
                xIndex = (int)idxVector.X;
                yIndex = (int)idxVector.Y;

                if (xIndex > -1 && xIndex < 8 && yIndex > -1 && yIndex < 8 && DrawPiece[xIndex, yIndex] != null && DrawPiece[xIndex, yIndex].isBlack == isPlayerOne)
                    pieceChosen = true;

            }
            else if (!checkMate && pieceChosen && curr.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released)
            {
                int xTarget = (int)(curr.X - boardPosition.X) / (int)TileSize.X;
                int yTarget = (int)(curr.Y - boardPosition.Y) / (int)TileSize.Y;

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
            prev = curr;
        }
        public void OnResize(Object sender, EventArgs e)
        {
            GameUI.checkMateButtonSize = new Vector2(100, 30) * Game1.Instance.GraphicsDevice.Viewport.Bounds.Size.ToVector2() / new Vector2(800, 480);
            TileSize = new Vector2(40, 40) * Game1.Instance.GraphicsDevice.Viewport.Bounds.Size.ToVector2() / new Vector2(800, 480);
            Game1.boardPosition = new Vector2(Game1.Instance.GraphicsDevice.Viewport.Bounds.Width / 2 - (TileSize.X * 5), Game1.Instance.GraphicsDevice.Viewport.Bounds.Height / 2 - (TileSize.Y * 4));
        }
    }
}
