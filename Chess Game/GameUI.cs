using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Chess_Game
{
    class GameUI
    {
        public Vector2 checkMateButton;
        public static Vector2 checkMateButtonSize = new(100, 30);
        public Rectangle checkMateButtonCoord;
        Rectangle GiveUpButtonPos;
        Rectangle AskDrawButtonPos;
        SpriteFont font;
        List<string> notationList = new();
        public int Turns { get; set; } = 0;

        public float playerTwoTimer = 600f;
        public float playerOneTimer = 600f;
        readonly float timeIncrement = 3f;

        /// <summary>
        /// Laddar in alla filer som behövs för att rita spelrutan.
        /// </summary>
        public void GameUIContent()
        {
            checkMateButton = new(Game1.Instance.GraphicsDevice.Viewport.Bounds.Width / 2,
                Game1.Instance.GraphicsDevice.Viewport.Bounds.Height / 2);
            font = Screen.Font;
            GiveUpButtonPos = new((int)Game1.ScreenMiddle.X - 320, (int)Game1.ScreenMiddle.Y + 170, 120, 40);
            AskDrawButtonPos = new((int)Game1.ScreenMiddle.X + 120, (int)Game1.ScreenMiddle.Y + 170, 120, 40);
        }

        /// <summary>
        /// Ritar allt som ska finnas under spelets gång.
        /// </summary>
        public void GameUIDraw(SpriteBatch spriteBatch, float playerOneTimer, float playerTwoTimer)
        {
            string gameText;
            checkMateButtonCoord = new((int)checkMateButton.X - 350,
                (int)checkMateButton.Y - 150,
                (int)checkMateButtonSize.X,
                (int)checkMateButtonSize.Y);

            if (Board.Instance.CheckMate && playerTwoTimer > 0 && playerTwoTimer > 0)
            {
                gameText = "Checkmate, " + (Board.Instance.WhiteWon ? "White won" : "Black won");
            }
            else if (playerTwoTimer <= 0 || playerTwoTimer <= 0)
            {
                Board.Instance.CheckMate = true;
                gameText = "No time left, " + (Board.Instance.WhiteWon ? "White won" : "Black won");
            }
            else if (Board.Instance.WhiteWon && Board.Instance.BlackWon)
            {
                gameText = "Draw";
            }
            else if (Board.Instance.GaveUp)
            {
                gameText = (Board.Instance.IsPlayerOne ? "White" : "Black") + " gave up";
            }
            else
            {
                gameText = (Board.Instance.IsPlayerOne) ? "Whites turn" : "Blacks turn";
            }

            spriteBatch.DrawString(font,
                gameText,
                new Vector2(checkMateButton.X - 350, checkMateButton.Y - 100),
                Color.Black);
            spriteBatch.DrawString(font,
                $"Blacks Time: {(int)(playerTwoTimer / 60):00}:{(int)(playerTwoTimer % 60):00}",
                new Vector2(checkMateButton.X - 350, checkMateButton.Y),
                Color.Black);
            spriteBatch.DrawString(font,
                $"Whites Time: {(int)(playerOneTimer / 60):00}:{(int)(playerOneTimer % 60):00}",
                new Vector2(checkMateButton.X - 350, checkMateButton.Y + 100),
                Color.Black);

            spriteBatch.DrawString(font, "Moves:", new Vector2(checkMateButton.X + 212, checkMateButton.Y - 168), Color.Black);

            spriteBatch.Draw(GiveUpButtonPos.Contains(Screen.mousePos) ? Screen.Button_Selected : Screen.Button_Open, GiveUpButtonPos, Color.White);
            spriteBatch.DrawString(font, "Give Up", new Vector2(GiveUpButtonPos.X + 20, GiveUpButtonPos.Y + 12), Color.Black);

            spriteBatch.Draw(AskDrawButtonPos.Contains(Screen.mousePos) ? Screen.Button_Selected : Screen.Button_Open, AskDrawButtonPos, Color.White);
            spriteBatch.DrawString(font, "Draw?", new Vector2(AskDrawButtonPos.X + 60, AskDrawButtonPos.Y + 12), Color.Black);

            NotationDraw(spriteBatch);
        }

        /// <summary>
        /// Kollar om knappar som finns under en match trycks och bestämmer vad som händer när en knapp har tryckts.
        /// </summary>
        public void GameUIButtons()
        {
            if (Screen.curr.LeftButton == ButtonState.Released && Screen.prev.LeftButton == ButtonState.Pressed)
            {
                if (GiveUpButtonPos.Contains(Screen.mousePos))
                {
                    if (Board.Instance.IsPlayerOne)
                    {
                        Board.Instance.WhiteWon = true;
                        Board.Instance.GaveUp = true;
                    }
                    else
                    {
                        Board.Instance.BlackWon = true;
                        Board.Instance.GaveUp = true;
                    }
                }
                if (AskDrawButtonPos.Contains(Screen.mousePos))
                {
                    Board.Instance.WhiteWon = true;
                    Board.Instance.BlackWon = true;
                }
            }
        }

        /// <summary>
        /// Metod för att rita koordinaterna på spelbrädet
        /// </summary>
        /// <param name="spritebatch"></param>
        /// <param name="x">X Positionen för skärmen.</param>
        /// <param name="y">Y Positionen för skärmen.</param>
        /// <param name="xCoord">X Koordinaten för den rutan på spelbrädet.</param>
        /// <param name="yCoord">Y Koordinaten för den rutan på spelbrädet.</param>
        public void BoardCoord(SpriteBatch spritebatch, int x, int y, int xCoord, int yCoord)
        {
            spritebatch.DrawString(font, $"{xCoord}, {yCoord}", new Vector2(x, y), Color.Red);
        }

        public void AddNotation(Piece[,] Pieces, int xIndex, int yIndex, int xTarget, int yTarget)
        {
            string tempNotation = "";
            var MovePiece = Board.Instance.MovePiece;

            if (MovePiece.hasCastled)
            {
                if (xTarget > xIndex)
                    tempNotation += "0-0";
                else
                    tempNotation += "0-0-0";
            }
            else
            {
                tempNotation += Pieces[xIndex, yIndex].type switch
                {
                    PieceType.King => "K",
                    PieceType.Queen => "Q",
                    PieceType.Bishop => "B",
                    PieceType.Rook => "R",
                    PieceType.Knight => "N",
                    _ => "",
                };
                if (Pieces[xTarget, yTarget] != null)
                    tempNotation += 'x';

                tempNotation += (char)('a' + xTarget);
                tempNotation += yTarget;
            }

            notationList.Add(tempNotation);
        }
        void NotationDraw(SpriteBatch spriteBatch)
        {
            int notationYCoord;
            string notationtext;

            for (int i = 0; i < notationList.Count; i++)
            {
                notationYCoord = (int)Math.Ceiling(((double)i + 1) / 2);
                if (i % 2 == 0)
                {
                    notationtext = $"{notationYCoord}:  {notationList[i]}";
                }
                else
                {
                    notationtext = notationList[i];
                }

                if (notationYCoord <= 10)
                {
                    spriteBatch.DrawString(font, notationtext, new Vector2(checkMateButton.X + ((i % 2 == 0) ? 200 : 270), checkMateButton.Y - 150 + ((notationYCoord - 1) * 20)), Color.Black);
                }
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
