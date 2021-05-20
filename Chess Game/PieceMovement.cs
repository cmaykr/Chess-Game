namespace Chess_Game
{
    public class PieceMovement
    {
        static (int xKing, int yKing) GetKing(Piece[,] Pieces) 
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (Pieces[x, y] != null && Pieces[x, y].type == PieceType.King && Pieces[x, y].isBlack != Board.Instance.IsPlayerOne)
                    {
                        return (x, y);
                    }
                }
            }
            return (-1, -1);
        }
        static bool Check(Piece[,] Pieces)
        {
            var (xKing, yKing) = GetKing(Pieces);

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (Pieces[x, y] != null && Pieces[x, y].CanMove(Pieces, x, y, xKing, yKing) && !Piece.Collision(Pieces, x, y, xKing, yKing) && Pieces[x, y].isBlack != Pieces[xKing, yKing].isBlack)
                    {
                        if (!(Pieces[x, y].type == PieceType.Pawn && x == xKing))
                            return true;
                    }
                }
            }
            return false;
        }

        public static bool WillMoveCauseCheck(Piece[,] Pieces, int xIndex, int yIndex, int xTarget, int yTarget)
        {
            Piece[,] tempBoard = (Piece[,])Pieces.Clone();
            var piece = tempBoard[xIndex, yIndex];
            tempBoard[xIndex, yIndex] = null;
            tempBoard[xTarget, yTarget] = piece;

            return Check(tempBoard);
        }

        public static bool IsCheckMate(Piece[,] Pieces)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (Pieces[x, y] != null && Pieces[x, y].isBlack != Board.Instance.IsPlayerOne)
                            {
                                if (Pieces[x, y].CanMove(Pieces, x, y, i, j) && !Piece.Collision(Pieces, x, y, i, j) && !WillMoveCauseCheck(Pieces, x, y, i, j))
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
