using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Chess_Game
{
    /// <summary>
    /// Board klassen är den som ritar spelbrädet och instanserar pjäserna,
    /// Board används också för att flytta pjäserna.
    /// </summary>
    class Board
    {
        int xIndex = 0;
        int yIndex = 0;
        public int XTarget { get; private set; }
        public int YTarget { get; private set; }
        public Vector2 TileSize { get; private set; } = new(40, 40);
        Texture2D tile;
        public Texture2D Tile => tile;
        Texture2D validMoveIndicator;
        public Texture2D Pawn, Rook, Knight, King, Bishop, Queen;
        Texture2D validMoveIndicatorSquare;
        Texture2D chosenPiece;
        Color chosenPieceColor;
        bool clickMove;
        bool promotingPiece;
        public bool pieceChosen = false;
        public bool IsPlayerOne { get; set; } = true;
        bool debug;
        public bool CheckMate;
        public bool TimerRun { get; private set; }
        public Vector2 WindowScale { get; private set; } = new(1, 1);

        public PieceMovement MovePiece = new();

        public static Board Instance;

        public Board()
        {
            Instance = this;
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
                    bool canMove = pieceChosen
                        && Pieces[xIndex, yIndex].CanMove(Pieces, xIndex, yIndex, i, j)
                        && !Piece.Collision(Pieces, xIndex, yIndex, i, j)
                        && !PieceMovement.WillMoveCauseCheck(Pieces, xIndex, yIndex, i, j);

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
                    else if (canMove && Pieces[i, j].isBlack != Pieces[xIndex, yIndex].isBlack)
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
                        GameScreen.Instance.GameUI.BoardCoord(spriteBatch, i * (int)TileSize.X + x, j * (int)TileSize.Y + y, i, j);
                }
            }
            GameScreen.Instance.GameUI.GameUIDraw(spriteBatch);

            DragPiece(Pieces, spriteBatch);
        }

        /// <summary>
        /// Instanserar alla bilderna.
        /// Bestämmer var pjäserna ska finnas i arrayen.
        /// </summary>
        public void PieceContent(Piece[,] Pieces)
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
                        _ => throw new System.Exception("Unknown"),
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
        /// Metod för att välja pjäs för att flytta.
        /// </summary>
        /// <param name="Pieces">Sparar positionen för alla pjäser på spelbrädet.</param>
        /// <param name="boardPosition">Positionen för pjäserna på spelrutan.</param>
        public void PieceMove(Piece[,] Pieces, Vector2 boardPosition)
        {
            if (promotingPiece)
            {
                PawnPromotion(Pieces, XTarget, YTarget);
            }
            // if Satsen bestämmer vilken position den valda pjäsen har, den bestämmer också var man vill flytta pjäsen
            // genom att dra eller klicka med musen.
            if (!CheckMate && Screen.curr.LeftButton == ButtonState.Pressed && pieceChosen == false && !promotingPiece)
            {
                Vector2 idxVector = new((Screen.curr.X - boardPosition.X) / TileSize.X, (Screen.curr.Y - boardPosition.Y) / TileSize.Y);
                xIndex = (int)idxVector.X;
                yIndex = (int)idxVector.Y;

                // Kollar om koordinaten man har klickat på är på spelbrädet.
                if (xIndex > -1 && xIndex < 8 && yIndex > -1 && yIndex < 8 && Pieces[xIndex, yIndex] != null && Pieces[xIndex, yIndex].isBlack != IsPlayerOne)
                    pieceChosen = TimerRun = true;

            }
            else if (!promotingPiece && !CheckMate && pieceChosen && Screen.curr.LeftButton == ButtonState.Pressed && Screen.prev.LeftButton == ButtonState.Released && clickMove)
            {
                XTarget = (int)(Screen.curr.X - boardPosition.X) / (int)TileSize.X;
                YTarget = (int)(Screen.curr.Y - boardPosition.Y) / (int)TileSize.Y;

                // Kollar om pjäsen får flytta dit.
                if (MovePiece.MoveChosenPiece(Pieces, xIndex, yIndex, boardPosition, XTarget, YTarget))
                {
                    if (Pieces[XTarget, YTarget].type == PieceType.Pawn && (YTarget == 0 || YTarget == 7))
                    {
                        promotingPiece = true;
                    }
                    else if (!promotingPiece)
                    {
                        MovenPiece(Pieces);
                        clickMove = false;
                    }
                }
            }
            else if (!CheckMate && pieceChosen && Screen.prev.LeftButton == ButtonState.Pressed && Screen.curr.LeftButton == ButtonState.Released)
            {
                XTarget = (int)(Screen.curr.X - boardPosition.X) / (int)TileSize.X;
                YTarget = (int)(Screen.curr.Y - boardPosition.Y) / (int)TileSize.Y;

                // Kollar om positionen man släppte pjäsen på är samma position den var på.
                if (XTarget == xIndex && YTarget == yIndex)
                {
                    clickMove = true;
                }
                else
                {
                    if (MovePiece.MoveChosenPiece(Pieces, xIndex, yIndex, boardPosition, XTarget, YTarget))
                    {
                        if (Pieces[XTarget, YTarget].type == PieceType.Pawn && (YTarget == 0 || YTarget == 7))
                        {
                            promotingPiece = true;
                        }
                        else
                        {
                            MovenPiece(Pieces);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Metoden uppdaterar alla variabler för det nuvarande spelet.
        /// Metoden kollar också tid och schackmatt.
        /// </summary>
        /// <param name="Pieces">Spelbrädet som användas.</param>
        void MovenPiece(Piece[,] Pieces)
        {
            MovePiece.xLastMove = xIndex;
            MovePiece.yLastMove = yIndex;

            MovePiece.xLastMoveTarget = XTarget;
            MovePiece.yLastMoveTarget = YTarget;
            GameScreen.Instance.GameUI.Turns += 1;

            GameScreen.Instance.GameUI.ApplyTimeIncrement();

            IsPlayerOne = !IsPlayerOne;
            CheckMate = PieceMovement.IsCheckMate(Pieces);
            if (CheckMate)
            {
                GameScreen.Instance.EndOfGame();
            }
        }

        /// <summary>
        /// Metod som promotar den valda bonden till den valda pjäsen.
        /// </summary>
        /// <param name="Pieces">Spelbrädet som användas.</param>
        /// <param name="x">X koordinaten för den promotade bonden.</param>
        /// <param name="y">Y koordinaten för den promotade bonden.</param>
        void PawnPromotion(Piece[,] Pieces, int x, int y)
        {
            var promotedPiece = GameUI.PromotionUI();

            if (promotedPiece != PieceType.Pawn)
            {
                Pieces[x, y].type = promotedPiece;
                promotingPiece = false;
                MovenPiece(Pieces);
            }
        }
        /// <summary>
        /// Metod som ritar pjäsen vid positionen av musen.
        /// </summary>
        /// <param name="Pieces">Spelbrädet pjäserna är på.</param>
        /// <param name="spriteBatch">Variabel för att kunna rita sprites i spelet.</param>
        void DragPiece(Piece[,] Pieces, SpriteBatch spriteBatch)
        {
            // Kollar så man har valt en pjäs och om vänsterklick är nertryckt.
            if (pieceChosen && Screen.curr.LeftButton == ButtonState.Pressed)
            {
                chosenPiece = Pieces[xIndex, yIndex].type switch
                {
                    PieceType.Pawn => Pawn,
                    PieceType.Rook => Rook,
                    PieceType.King => King,
                    PieceType.Bishop => Bishop,
                    PieceType.Knight => Knight,
                    PieceType.Queen => Queen,
                    _ => null,
                };
                if (Pieces[xIndex, yIndex].isBlack)
                    chosenPieceColor = new(16, 19, 20);
                else
                    chosenPieceColor = Color.White;

                Rectangle piecePos = new(Screen.curr.X - 20, Screen.curr.Y - 20, (int)TileSize.X, (int)TileSize.Y);
                spriteBatch.Draw(chosenPiece, piecePos, chosenPieceColor);
            }
        }

        /// <summary>
        /// Funktion som ändrar positionerna och storleken när skärmen ändrar upplösning.
        /// </summary>
        public void OnResize(object sender, EventArgs e)
        {
            WindowScale = Game1.Instance.GraphicsDevice.Viewport.Bounds.Size.ToVector2() / new Vector2(800, 480);
            TileSize = new Vector2(40, 40) * WindowScale;
            Console.WriteLine(WindowScale);
            GameScreen.BoardPosition = new Vector2(
                Game1.Instance.GraphicsDevice.Viewport.Bounds.Width / 2 - (TileSize.X * 5),
                Game1.Instance.GraphicsDevice.Viewport.Bounds.Height / 2 - (TileSize.Y * 4)
                );
            Console.WriteLine(TileSize);
        }
    }
}
