using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Chess_Game
{
    public class GameUI
    {
        Vector2 NotationPos { get; set; }
        Rectangle GiveUpButtonPos;
        Rectangle AskDrawButtonPos;
        SpriteFont font;
        List<string> notationList = new();
        public int Turns { get; set; } = 0;

        public float PlayerTwoTimer { get; set; }
        public float PlayerOneTimer { get; set; }
        public float TimeIncrement { get; set; }

        /// <summary>
        /// Laddar in alla filer som behövs för att rita spelrutan.
        /// </summary>
        public void GameUIContent()
        {
            NotationPos = new(Game1.Instance.GraphicsDevice.Viewport.Bounds.Width / 2,
                Game1.Instance.GraphicsDevice.Viewport.Bounds.Height / 2);
            font = Screen.Font;
            GiveUpButtonPos = new((int)Game1.ScreenMiddle.X - 320, (int)Game1.ScreenMiddle.Y + 170, 120, 40);
            AskDrawButtonPos = new((int)Game1.ScreenMiddle.X + 120, (int)Game1.ScreenMiddle.Y + 170, 120, 40);
        }

        /// <summary>
        /// Ritar allt som ska finnas under spelets gång.
        /// </summary>
        public void GameUIDraw(SpriteBatch spriteBatch)
        {
            string gameText;

            if (Board.Instance.CheckMate && PlayerTwoTimer > 0 && PlayerTwoTimer > 0)
            {
                gameText = "Checkmate, " + (!Board.Instance.IsPlayerOne ? "White won" : "Black won");
            }
            else if (PlayerTwoTimer <= 0 || PlayerTwoTimer <= 0)
            {
                Board.Instance.CheckMate = true;
                gameText = "No time left, " + (!Board.Instance.IsPlayerOne ? "White won" : "Black won");
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
                gameText = Board.Instance.IsPlayerOne ? "Whites turn" : "Blacks turn";
            }

            spriteBatch.DrawString(font,
                gameText,
                new Vector2(NotationPos.X - 350, NotationPos.Y - 100),
                Color.Black);
            spriteBatch.DrawString(font,
                $"Blacks Time: {(int)(PlayerTwoTimer / 60):00}:{(int)(PlayerTwoTimer % 60):00}",
                new Vector2(NotationPos.X - 350, NotationPos.Y),
                Color.Black);
            spriteBatch.DrawString(font,
                $"Whites Time: {(int)(PlayerOneTimer / 60):00}:{(int)(PlayerOneTimer % 60):00}",
                new Vector2(NotationPos.X - 350, NotationPos.Y + 100),
                Color.Black);

            spriteBatch.DrawString(font, "Moves:", new Vector2(NotationPos.X + 212, NotationPos.Y - 168), Color.Black);

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
                    if (!Board.Instance.IsPlayerOne)
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
                    spriteBatch.DrawString(font, notationtext, new Vector2(NotationPos.X + ((i % 2 == 0) ? 200 : 270), NotationPos.Y - 150 + ((notationYCoord - 1) * 20)), Color.Black);
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
                PlayerOneTimer += TimeIncrement;
            }
            else
            {
                PlayerTwoTimer += TimeIncrement;
            }
        }

        public void DecrementTimer(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!Board.Instance.CheckMate && Board.Instance.timerRun)
            {
                if (Board.Instance.IsPlayerOne)
                {
                    PlayerOneTimer -= dt;
                }
                else
                {
                    PlayerTwoTimer -= dt;
                }
            }
        }
    }
}
