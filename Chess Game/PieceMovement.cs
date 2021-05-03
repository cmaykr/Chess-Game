namespace Chess_Game
{
    public class PieceMovement
    {

        public static (int xKing, int yKing) GetKing(Piece[,] DrawPiece) 
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (DrawPiece[x, y] != null && DrawPiece[x, y].type == PieceType.king && DrawPiece[x, y].isBlack == Board.Instance.isPlayerOne)
                    {
                        return (x, y);
                    }
                }
            }

            return (-1, -1);
        }
        public static bool Check(Piece[,] DrawPiece, int xKing, int yKing)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (DrawPiece[x, y] != null && DrawPiece[x, y].CanMove(DrawPiece, x, y, xKing, yKing) && !DrawPiece[x, y].Collision(DrawPiece, x, y, xKing, yKing) && DrawPiece[x, y].isBlack != DrawPiece[xKing, yKing].isBlack)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool WillMoveCauseCheck(Piece[,] DrawPiece, int xIndex, int yIndex, int xTarget, int yTarget)
        {
            Piece[,] Tempboard = (Piece[,])DrawPiece.Clone();
            var piece = Tempboard[xIndex, yIndex];
            Tempboard[xIndex, yIndex] = null;
            Tempboard[xTarget, yTarget] = piece;

            var king = GetKing(Tempboard);

            return (Check(Tempboard, king.xKing, king.yKing));
        }
    }
}