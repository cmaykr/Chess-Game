using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Chess_Game
{
    /// <summary>
    /// GameUI klassen håller alla variabler och metoder som behövs under en match som inte är själva spelbrädet.
    /// </summary>
    public class GameUI
    {
        Vector2 NotationPos { get; set; }
        Rectangle GiveUpButtonPos;
        Rectangle AskDrawButtonPos;
        Rectangle saveGame;
        SpriteFont font;
        public List<string> NotationList { get; set; } = new();
        public int Turns { get; set; } = 0;
        public bool WhiteWon { get; private set; }
        public bool BlackWon { get; private set; }
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
        /// Ritar alla knappar som finns under matchen. Ritar också all text på knapparna.
        /// </summary>
        public void GameUIDraw(SpriteBatch spriteBatch)
        {
            string gameText;
            int xScale = (int)Board.Instance.WindowScale.X;
            int yScale = (int)Board.Instance.WindowScale.Y;

            GiveUpButtonPos = new((int)Game1.ScreenMiddle.X * xScale - 320 * xScale, (int)Game1.ScreenMiddle.Y * yScale + 180 * yScale, 120 * xScale, 40 * yScale);
            NotationPos = new(Game1.Instance.GraphicsDevice.Viewport.Bounds.Width / 2, Game1.Instance.GraphicsDevice.Viewport.Bounds.Height / 2);
            AskDrawButtonPos = new((int)Game1.ScreenMiddle.X * xScale + 120 * xScale, (int)Game1.ScreenMiddle.Y * yScale + 180 * yScale, 120 * xScale, 40 * yScale);
            saveGame = new((int)Game1.ScreenMiddle.X * xScale - 340 * xScale, (int)Game1.ScreenMiddle.Y * yScale - 200 * yScale, 120 * xScale, 40 * yScale);


            // Ändrar texten beroende på om spelet är igång eller har avslutats.
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

            Vector2 NotationMovePos = new(NotationPos.X + 212 * xScale, NotationPos.Y - 180 * yScale);
            spriteBatch.DrawString(font, "Last 10 Moves:", NotationMovePos, Color.Black);

            spriteBatch.Draw(GiveUpButtonPos.Contains(Screen.mousePos) ? Screen.Button_Selected : Screen.Button_Open, GiveUpButtonPos, Color.White);
            spriteBatch.DrawString(font, "Give Up", new Vector2(GiveUpButtonPos.X + 20 * xScale, GiveUpButtonPos.Y + 12 * yScale), Color.Black);

            spriteBatch.Draw(AskDrawButtonPos.Contains(Screen.mousePos) ? Screen.Button_Selected : Screen.Button_Open, AskDrawButtonPos, Color.White);
            spriteBatch.DrawString(font, "Draw?", new Vector2(AskDrawButtonPos.X + 60 * xScale, AskDrawButtonPos.Y + 12 * yScale), Color.Black);

            if (!Board.Instance.PromotingPiece && !Board.Instance.CheckMate)
            {
                spriteBatch.Draw(saveGame.Contains(Screen.mousePos) ? Screen.Button_Selected : Screen.Button_Open, saveGame, Color.White);
                spriteBatch.DrawString(font, "Save game?", new Vector2(saveGame.X + 20, saveGame.Y + 12), Color.Black);
            }

            NotationDraw(spriteBatch, xScale, yScale);

            if (Board.Instance.PromotingPiece)
                PromotionUI(spriteBatch, xScale, yScale);
        }

        /// <summary>
        /// Kollar vilken tangent man trycker ner.
        /// Returnerar av typen PieceType utifrån vilken tangent som trycks ner.
        /// </summary>
        /// <returns>Returnerar en pjäs.</returns>
        public static PieceType Promotion()
        {
            var keyboard = Keyboard.GetState().GetPressedKeys();

            if (keyboard.Length <= 0)
                return PieceType.Pawn;

            // Kollar vilken bokstav man tryckt ner och returnerar rätt typ.
            return keyboard[0] switch
            {
                Keys.D1 => PieceType.Queen,
                Keys.D2 => PieceType.Rook,
                Keys.D3 => PieceType.Knight,
                Keys.D4 => PieceType.Bishop,
                _ => PieceType.Pawn,
            };
        }

        /// <summary>
        /// Ritar under matchen vilka pjäser man får promota till.
        /// Den ritar också vilka tangenter man ska trycka för pjäserna.
        /// </summary>
        /// <param name="xScale">X skalan för spelrutan. Jämfört med originalupplösningen.</param>
        /// <param name="yScale">Y skalan för spelrutan. Jämfört med originalupplösningen.</param>
        public void PromotionUI(SpriteBatch spriteBatch, int xScale, int yScale)
        {
            spriteBatch.DrawString(font, "Press key for the piece you want to promote to. You MUST promote", new Vector2(Game1.ScreenMiddle.X * xScale - 340 * xScale, Game1.ScreenMiddle.Y * yScale - 240 * yScale), Color.Black);
            spriteBatch.DrawString(font, "1 = Queen, 2 = Rook, 3 = Knight, 4 = Bishop", new Vector2(Game1.ScreenMiddle.X * xScale - 340 * xScale, Game1.ScreenMiddle.Y * yScale - 220 * yScale), Color.Black);
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
                    GameScreen.Instance.EndOfGame();
                }
                if (AskDrawButtonPos.Contains(Screen.mousePos))
                {
                    WhiteWon = true;
                    BlackWon = true;
                    GameScreen.Instance.EndOfGame();
                }
                if (saveGame.Contains(Screen.mousePos) && !Board.Instance.PromotingPiece && !Board.Instance.CheckMate)
                {
                    SaveGame.Save();
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

        /// <summary>
        /// Lägger till en notering i noteringslistan för varje drag man gör.
        /// </summary>
        /// <param name="Pieces">Spelbrädet som används.</param>
        /// <param name="xIndex">X värdet på positionen där pjäsen är.</param>
        /// <param name="yIndex">Y värdet på positionen där pjäsen är.</param>
        /// <param name="xTarget">X värdet där pjäsen flyttar till.</param>
        /// <param name="yTarget">Y värdet där pjäsen flyttar till.</param>
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

        /// <summary>
        /// Ritar noteringen under matchen.
        /// </summary>
        /// <param name="xScale">X skalan för spelrutan. Jämfört med originalupplösningen.</param>
        /// <param name="yScale">Y skalan för spelrutan. Jämfört med originalupplösningen.</param>
        void NotationDraw(SpriteBatch spriteBatch, int xScale, int yScale)
        {
            int notationYCoord;
            int rounds = 0;
            string notationtext;
            int startValue = 0;
            if (NotationList.Count >= 20)
            {
                startValue = NotationList.Count - 20;
            }
            for (int i = startValue; i < NotationList.Count; i++)
            {
                notationYCoord = (int)Math.Ceiling(((double)rounds + 1) / 2);
                if (i % 2 == 0)
                {
                    notationtext = $"{notationYCoord}:  {NotationList[i]}";
                }
                else
                {
                    notationtext = NotationList[i];
                }

                spriteBatch.DrawString(font, notationtext, new Vector2(NotationPos.X + ((i % 2 == 0) ? 200 : 270) * xScale, NotationPos.Y - 150 * yScale + ((notationYCoord - 1) * 20) * yScale), Color.Black);
                rounds += 1;
            }
        }

        /// <summary>
        /// Ökar tiden med en bestämd summa på den nuvarandes spelarens tid. Tiden räknas i sekunder.
        /// </summary>
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

        /// <summary>
        /// Tar bort tid med en bestämd summa på den nuvarandes spelares tid. Tiden räknas i sekunder.
        /// </summary>
        /// <param name="gameTime">Tid staten för Game</param>
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
