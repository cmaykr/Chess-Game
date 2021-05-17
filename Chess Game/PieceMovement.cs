namespace Chess_Game
{
    public class PieceMovement
    {
        static (int xKing, int yKing) GetKing(Piece[,] DrawPiece) 
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (DrawPiece[x, y] != null && DrawPiece[x, y].type == PieceType.King && DrawPiece[x, y].isBlack != Board.Instance.IsPlayerOne)
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

        public static bool WillMoveCauseCheck(Piece[,] DrawPiece, int xIndex, int yIndex, int xTarget, int yTarget)
        {
            Piece[,] tempBoard = (Piece[,])DrawPiece.Clone();
            var piece = tempBoard[xIndex, yIndex];
            tempBoard[xIndex, yIndex] = null;
            tempBoard[xTarget, yTarget] = piece;

            return Check(tempBoard);
        }

        public static bool isCheckMate(Piece[,] DrawPiece)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (DrawPiece[x, y] != null && DrawPiece[x, y].isBlack != Board.Instance.IsPlayerOne)
                            {
                                if (DrawPiece[x, y].CanMove(DrawPiece, x, y, i, j) && !Piece.Collision(DrawPiece, x, y, i, j) && !WillMoveCauseCheck(DrawPiece, x, y, i, j))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
