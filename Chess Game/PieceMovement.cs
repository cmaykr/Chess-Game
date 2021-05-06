namespace Chess_Game
{
    public class PieceMovement
    {
        /*public static PieceMovement Instance;
        public bool isCheckMate;

        public PieceMovement(bool called)
        {
            Instance = this;
        }
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

            var (xKing, yKing) = GetKing(Tempboard);

            return Check(Tempboard, xKing, yKing);
        }

        public void CheckMate(Piece[,] DrawPiece)
        {
            isCheckMate = false;
            int validMoves = 0;
            bool isChecked;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var (xKing, yKing) = GetKing(DrawPiece);
                    isChecked = Check(DrawPiece, xKing, yKing);
                    if (DrawPiece[xKing, yKing].CanMove(DrawPiece, xKing, yKing, i, j) && !DrawPiece[xKing,yKing].Collision(DrawPiece, xKing, yKing, i, j) && !WillMoveCauseCheck(DrawPiece, xKing, yKing, i, j))
                    {
                        validMoves++;
                    }
                    if (validMoves == 0 && isChecked)
                    {
                        isCheckMate = true;
                    }
                }
            }
            System.Console.WriteLine(isCheckMate);
        }*/
    }
}
