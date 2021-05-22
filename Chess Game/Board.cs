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
        public MouseState curr, prev;
        int xIndex = 0, yIndex = 0;
        public int xLastMoveTarget, yLastMoveTarget;
        public int xLastMove, yLastMove;
        public Vector2 TileSize { get; private set; } = new(40,40);
        private Texture2D tile;
        public Texture2D Tile => tile;
        Texture2D validMoveIndicator;
        public Texture2D Pawn, Rook, Knight, King, Bishop, Queen;
        Texture2D validMoveIndicatorSquare;
        public bool pieceChosen = false;
        public bool IsPlayerOne = true;
        bool debug;
        public bool CheckMate;
        bool timerRun;
        public bool hasEnPassant;
        public readonly GameUI gameUI = new();
        PieceMovement MovePiece = new();

        float playerTwoTimer = 600f;
        float playerOneTimer = 600f;
        readonly float timeIncrement = 10f;

        public static Board Instance;

        public Board()
        {
            Instance = this;
        }

        public void BoardContent(Piece[,] Pieces)
        {
            PieceContent(Pieces);
            gameUI.GameUIContent();
        }

        public void BoardUpdate(GameTime gameTime, Piece[,] Pieces, Vector2 boardPosition)
        {
            PieceMove(Pieces, boardPosition);
            DecrementTimer(gameTime);
        }

        /// <summary>
        /// Ritar spelbrädet, 
        /// visar också var man kan flytta en pjäs.
        /// </summary>
        public void BoardDraw(SpriteBatch spriteBatch, int x, int y, Piece[,] Pieces)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
                debug = true;
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
                debug = false;

            for (int i = 0; i < 8; i += 1)
            { 
                for (int j = 0; j < 8; j += 1)
                {
                    bool canMove = pieceChosen && Pieces[xIndex, yIndex].CanMove(Pieces, xIndex, yIndex, i, j) && !Piece.Collision(Pieces, xIndex, yIndex, i, j) && !PieceMovement.WillMoveCauseCheck(Pieces, xIndex, yIndex, i, j);
                    Rectangle tilePos = new(i * (int)TileSize.X + x, j * (int)TileSize.Y + y, (int)TileSize.X, (int)TileSize.Y);

                    Color boardColor;

                    // Bestämmer färgen på rutorna.
                    if (i % 2 == j % 2)
                        boardColor = new(0xF0, 0xD9, 0xB5);
                    else
                        boardColor = new(0x94, 0x6f, 0x51);
                    // Om den rutan är där sin valda pjäs finns.
                    if (pieceChosen && i == xIndex && j == yIndex)
                        boardColor = Color.Green;

                    // Ritar själva spelbrädet
                    spriteBatch.Draw(Tile, tilePos, boardColor);
                    
                    // Ritar var pjäsen får flytta och om det är en fiendepjäs på rutan
                    if (canMove && Pieces[i, j] == null)
                        spriteBatch.Draw(validMoveIndicator, tilePos, Color.DarkGreen);
                    else if(canMove && Pieces[i, j].isBlack != Pieces[xIndex, yIndex].isBlack)
                        spriteBatch.Draw(validMoveIndicatorSquare, tilePos, Color.DarkGreen);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // Ritar pjäserna.
                    if (Pieces[i, j] != null)
                        Pieces[i, j].PieceDraw(spriteBatch, i, j);

                    // Ritar koordinaterna för brädet om debug är PÅ
                    if (debug)
                        gameUI.BoardCoord(spriteBatch, i * (int)TileSize.X + x, j * (int)TileSize.Y + y, i, j);
                }
            }
            gameUI.GameUIDraw(spriteBatch, playerOneTimer, playerTwoTimer);
        }

        /// <summary>
        /// Instanserar alla bilderna.
        /// Bestämmer var pjäserna ska finnas i arrayen.
        /// </summary>
        void PieceContent(Piece[,] Pieces)
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
            
            // Bestämmer var pjäserna ska finnas på spelbrädet för båda sidorna.
            string[] pieceCoord = new[]
            {
                "PPPPPPPP",
                "RGBQKBGR"
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
                        'P' => PieceType.Pawn,
                        'R' => PieceType.Rook,
                        'G' => PieceType.Knight,
                        'B' => PieceType.Bishop,
                        'K' => PieceType.King,
                        'Q' => PieceType.Queen,
                        _ => throw new System.Exception("Unkown"),
                    };

                    // Bestämmer positionerna i Pieces arrayen var de enskilda pjäserna ska finnas 
                    Pieces[j, i] = new Piece()
                    {
                        type = type,
                        isBlack = true,
                    };
                    Pieces[j, 7 - i] = new Piece()
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
        /// <param name="Pieces">Sparar positionen för alla pjäser på spelbrädet.</param>
        /// <param name="boardPosition">Positionen för pjäserna på spelrutan.</param>
        void PieceMove(Piece[,] Pieces, Vector2 boardPosition)
        {
            curr = Mouse.GetState();

            if (!CheckMate && curr.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released && pieceChosen == false)
            {
                Vector2 idxVector = new((curr.X - boardPosition.X) / TileSize.X, (curr.Y - boardPosition.Y) / TileSize.Y);
                xIndex = (int)idxVector.X;
                yIndex = (int)idxVector.Y;

                // Kollar om koordinaten man har klickat på är på spelbrädet.
                if (xIndex > -1 && xIndex < 8 && yIndex > -1 && yIndex < 8 && Pieces[xIndex, yIndex] != null && Pieces[xIndex, yIndex].isBlack != IsPlayerOne)
                    pieceChosen = timerRun = true;

            }
            else if (!CheckMate && pieceChosen && curr.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released)
            {
                MovePiece.MoveChosenPiece(Pieces, xIndex, yIndex, boardPosition);
            }
            prev = curr;
        }

        public void HasCastled(Piece[,] Pieces, int xIndex, int yTarget, int xTarget)
        {
            int xCastlingRook;
            if (xTarget < xIndex)
                xCastlingRook = 0;
            else
                xCastlingRook = 7;

            int xDist = Math.Abs(xTarget - xIndex);
            if (((IsPlayerOne) ? yTarget == 7 : yTarget == 0) && Pieces[xTarget, yTarget] != null && xDist == 2 && Pieces[xTarget, yTarget].type == PieceType.King)
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
            if (!timerRun)
            {
                return;
            }

            if (IsPlayerOne)
            {
                playerOneTimer += timeIncrement;
            }
            else
            {
                playerTwoTimer += timeIncrement;
            }
        }

        void DecrementTimer(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!CheckMate && timerRun)
            {
                if (IsPlayerOne)
                {
                    playerOneTimer -= dt;
                }
                else
                {
                    playerTwoTimer -= dt;
                }
            }
        }

        /// <summary>
        /// Funktion som ändrar positionerna och storleken när skärmen ändrar upplösning.
        /// </summary>
        public void OnResize(Object sender, EventArgs e)
        {
            GameUI.checkMateButtonSize = new Vector2(100, 30) * Game1.Instance.GraphicsDevice.Viewport.Bounds.Size.ToVector2() / new Vector2(800, 480);
            TileSize = new Vector2(40, 40) * Game1.Instance.GraphicsDevice.Viewport.Bounds.Size.ToVector2() / new Vector2(800, 480);
            Game1.boardPosition = new Vector2(Game1.Instance.GraphicsDevice.Viewport.Bounds.Width / 2 - (TileSize.X * 5), Game1.Instance.GraphicsDevice.Viewport.Bounds.Height / 2 - (TileSize.Y * 4));
        }
    }
}
