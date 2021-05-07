using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
namespace Chess_Game
{
    /// <summary>
    /// Klass för alla metoder när man är i debug läge.
    /// Testkod och metoder för att underlätta problemlösning.
    /// </summary>
    class DebugMode
    {
        /// <summary>
        /// Metod för att rita koordinaterna på spelbrädet
        /// </summary>
        /// <param name="spritebatch"></param>
        /// <param name="x">X Positionen för skärmen.</param>
        /// <param name="y">Y Positionen för skärmen.</param>
        /// <param name="xCoord">X Koordinaten för den rutan på spelbrädet.</param>
        /// <param name="yCoord">Y Koordinaten för den rutan på spelbrädet.</param>
        public static void BoardCoord(SpriteBatch spritebatch, int x, int y, int xCoord, int yCoord)
        {
            spritebatch.DrawString(Board.Instance.font, $"{xCoord}, {yCoord}", new Vector2(x, y), Color.Red);
        }
    }
}
