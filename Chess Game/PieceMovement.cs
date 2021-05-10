namespace Chess_Game
{
    public class PieceMovement
    {
        bool isCheckMated;

        static (int xKing, int yKing) GetKing(Piece[,] DrawPiece) 
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (DrawPiece[x, y] != null && DrawPiece[x, y].type == PieceType.King && DrawPiece[x, y].isBlack == Board.Instance.isPlayerOne)
                    {
                        return (x, y);
                    }
                }
            }
            return (-1, -1);
        }
        public static bool Check(Piece[,] DrawPiece)
        {
            var (xKing, yKing) = GetKing(DrawPiece);

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (DrawPiece[x, y] != null && DrawPiece[x, y].CanMove(DrawPiece, x, y, xKing, yKing) && !Piece.Collision(DrawPiece, x, y, xKing, yKing) && DrawPiece[x, y].isBlack != DrawPiece[xKing, yKing].isBlack)
                    {
                        if (!(DrawPiece[x, y].type == PieceType.Pawn && x == xKing))
                            return true;
                    }
                }
            }
            return false;
        }

        // Cannot move under pawn, even if nothing can take out the king.
        public static bool WillMoveCauseCheck(Piece[,] DrawPiece, int xIndex, int yIndex, int xTarget, int yTarget)
        {
            Piece[,] tempBoard = (Piece[,])DrawPiece.Clone();
            var piece = tempBoard[xIndex, yIndex];
            tempBoard[xIndex, yIndex] = null;
            tempBoard[xTarget, yTarget] = piece;

            return Check(tempBoard);
        }
    }
}
