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
        Rectangle saveGame;
        SpriteFont font;
        public List<string> NotationList { get; set; } = new();
        public int Turns { get; set; } = 0;
        bool WhiteWon;
        bool BlackWon;
        bool GaveUp;


        public float PlayerTwoTimer { get; set; }
        public float PlayerOneTimer { get; set; }
        public float TimeIncrement { get; set; }

        /// <summary>
        /// Laddar in alla filer som behövs för att rita spelrutan.
        /// </summary>
        public void GameUIContent()
        {
            font = Screen.Font;
        }

        /// <summary>
        /// Ritar allt som ska finnas under spelets gång.
        /// </summary>
        public void GameUIDraw(SpriteBatch spriteBatch)
        {
            string gameText;
            int xScale = (int)Board.Instance.windowScale.X;
            int yScale = (int)Board.Instance.windowScale.Y;

            GiveUpButtonPos = new((int)Game1.ScreenMiddle.X * xScale - 320 * xScale, (int)Game1.ScreenMiddle.Y * yScale + 180 * yScale, 120 * xScale, 40 * yScale);
            NotationPos = new(Game1.Instance.GraphicsDevice.Viewport.Bounds.Width / 2, Game1.Instance.GraphicsDevice.Viewport.Bounds.Height / 2);
            AskDrawButtonPos = new((int)Game1.ScreenMiddle.X * xScale + 120 * xScale, (int)Game1.ScreenMiddle.Y * yScale + 180 * yScale, 120 * xScale, 40 * yScale);
            saveGame = new((int)Game1.ScreenMiddle.X * xScale - 340 * xScale, (int)Game1.ScreenMiddle.Y * yScale - 200 * yScale, 120 * xScale, 40 * yScale);

            if (Board.Instance.CheckMate && PlayerTwoTimer > 0 && PlayerTwoTimer > 0)
            {
                gameText = "Checkmate, " + (!Board.Instance.IsPlayerOne ? "White won" : "Black won");
            }
            else if (PlayerTwoTimer <= 0 || PlayerTwoTimer <= 0)
            {
                Board.Instance.CheckMate = true;
                gameText = "No time left, " + (!Board.Instance.IsPlayerOne ? "White won" : "Black won");
            }
            else if (WhiteWon && BlackWon)
            {
                gameText = "Draw";
            }
            else if (GaveUp)
            {
                gameText = (Board.Instance.IsPlayerOne ? "White" : "Black") + " gave up";
            }
            else
            {
                gameText = Board.Instance.IsPlayerOne ? "Whites turn" : "Blacks turn";
            }

            spriteBatch.DrawString(font,
                gameText,
                new Vector2(NotationPos.X - 350 * xScale, NotationPos.Y - 100 * yScale),
                Color.Black);
            spriteBatch.DrawString(font,
                $"Blacks Time: {(int)(PlayerTwoTimer / 60):00}:{(int)(PlayerTwoTimer % 60):00}",
                new Vector2(NotationPos.X - 350 * xScale, NotationPos.Y),
                Color.Black);
            spriteBatch.DrawString(font,
                $"Whites Time: {(int)(PlayerOneTimer / 60):00}:{(int)(PlayerOneTimer % 60):00}",
                new Vector2(NotationPos.X - 350 * xScale, NotationPos.Y + 100 * yScale),
                Color.Black);

            Vector2 NotationMovePos = new(NotationPos.X + 212 * xScale, NotationPos.Y - 168 * yScale);
            spriteBatch.DrawString(font, "Moves:", NotationMovePos, Color.Black);

            spriteBatch.Draw(GiveUpButtonPos.Contains(Screen.mousePos) ? Screen.Button_Selected : Screen.Button_Open, GiveUpButtonPos, Color.White);
            spriteBatch.DrawString(font, "Give Up", new Vector2(GiveUpButtonPos.X + 20 * xScale, GiveUpButtonPos.Y + 12 * yScale), Color.Black);

            spriteBatch.Draw(AskDrawButtonPos.Contains(Screen.mousePos) ? Screen.Button_Selected : Screen.Button_Open, AskDrawButtonPos, Color.White);
            spriteBatch.DrawString(font, "Draw?", new Vector2(AskDrawButtonPos.X + 60 * xScale, AskDrawButtonPos.Y + 12 * yScale), Color.Black);

            spriteBatch.Draw(saveGame.Contains(Screen.mousePos) ? Screen.Button_Selected : Screen.Button_Open, saveGame, Color.White);
            spriteBatch.DrawString(font, "Save game?", new Vector2(saveGame.X + 20, saveGame.Y + 12), Color.Black);

            NotationDraw(spriteBatch, xScale, yScale);
        }

        /// <summary>
        /// Kollar vilken tangent man trycker ner.
        /// Returnerar av typen PieceType utifrån vilken tangent som trycks ner.
        /// </summary>
        /// <returns>Returnerar av typen PieceType.</returns>
        public static PieceType PromotionUI()
        {
            var keyboard = Keyboard.GetState().GetPressedKeys();

            if (keyboard.Length <= 0)
                return PieceType.Pawn;

            // Kollar vilken bokstav man tryckt ner och returnerar rätt typ.
            return keyboard[0] switch
            {
                Keys.Q => PieceType.Queen,
                Keys.R => PieceType.Rook,
                Keys.G => PieceType.Knight,
                Keys.B => PieceType.Bishop,
                _ => PieceType.Pawn,
            };
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
                        WhiteWon = true;
                        GaveUp = true;
                    }
                    else
                    {
                        BlackWon = true;
                        GaveUp = true;
                    }
                }
                if (AskDrawButtonPos.Contains(Screen.mousePos))
                {
                    WhiteWon = true;
                    BlackWon = true;
                }
                if (saveGame.Contains(Screen.mousePos))
                {
                    GameScreen.Instance.SaveGame.Save();
                    Game1.Screen = new MainMenuScreen();
                    Game1.Screen.Initialize();
                    Game1.Screen.LoadContent();
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
                tempNotation += 7 - yTarget + 1;
            }

            NotationList.Add(tempNotation);
        }
        void NotationDraw(SpriteBatch spriteBatch, int xScale, int yScale)
        {
            int notationYCoord;
            string notationtext;

            for (int i = 0; i < NotationList.Count; i++)
            {
                notationYCoord = (int)Math.Ceiling(((double)i + 1) / 2);
                if (i % 2 == 0)
                {
                    notationtext = $"{notationYCoord}:  {NotationList[i]}";
                }
                else
                {
                    notationtext = NotationList[i];
                }

                if (notationYCoord <= 10)
                {
                    spriteBatch.DrawString(font, notationtext, new Vector2(NotationPos.X + ((i % 2 == 0) ? 200 : 270) * xScale, NotationPos.Y - 150 * yScale + ((notationYCoord - 1) * 20) * yScale), Color.Black);
                }
            }
        }

        public void ApplyTimeIncrement()
        {
            if (!Board.Instance.TimerRun)
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

            if (!Board.Instance.CheckMate && Board.Instance.TimerRun)
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
