using System.Collections.Immutable;

namespace Min.Ak.Backtracking.KQueens;

internal readonly struct KQueensSolution(int n, ImmutableArray<int> placements)
{
    public KQueensBoard ToBoard()
    {
        int[][] board = new int[n][];
        for (int row = 0; row < board.Length; ++row)
        {
            board[row] = new int[n];
        }
        for (int i = 0; i < placements.Length; ++i)
        {
            int cell = placements[i];
            board[cell / n][cell % n] = 1;
        }
        return new KQueensBoard(board);
    }
}