using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Min.Ak.Backtracking.KQueens;

[DebuggerDisplay("{ToString(),nq}")]
internal readonly struct KQueensBoard(int[][] board)
{
    public int Size => board.Length;

    [Conditional("DEBUG")]
    public void AssertValid()
    {
        // avoid dumb mistakes
        Debug.Assert(board != null);
        for (int i = 0; i < board.Length; ++i)
        {
            Debug.Assert(board[i] != null);
            Debug.Assert(board[i].Length == board.Length);
        }
    }

    // e.g., n = 4
    //   0 1 2 3
    // 0 x x x x
    // 1 x x x x
    // 2 x x x x
    // 3 x x x x
    [SuppressMessage("Style", "IDE0011:Add braces", Justification = "Empty loop bodies.")]
    public bool IsValidSolution()
    {
        AssertValid();
        int n = Size;
        // check rows
        for (int row = 0; row < n; ++row)
        {
            int queensInRow = 0;
            for (int col = 0; col < n && queensInRow <= 1; queensInRow += board[row][col++]) ;
            if (queensInRow > 1)
            {
                return false;
            }
        }
        // check cols
        for (int col = 0; col < n; ++col)
        {
            int queensInCol = 0;
            for (int row = 0; row < n && queensInCol <= 1; queensInCol += board[row++][col]) ;
            if (queensInCol > 1)
            {
                return false;
            }
        }
        // check diags
        for (int x = n + n - 1; x >= 0; --x)
        {
            // start indices for x from 0 to 2n - 1:
            // 0: row, col = 0, 0
            // 1: row, col = 0, 1
            // 2: row, col = 1, 0
            // 3: row, col = 0, 2
            // 4: row, col = 2, 0
            // 5: row, col = 0, 3
            // 6: row, col = 3, 0
            // => x even: row = intdiv(x / 2), col = 0
            // => x odd: row = 0, col = intdiv(x / 2) + 1
            int odd = -(x & 1);
            int even = ~odd;
            int rowStart = even & (x >> 1);
            int colStart = odd & ((x >> 1) + 1);

            int queensInDiag = 0;
            for (int row = rowStart, col = colStart; row < n && col < n && queensInDiag <= 1; queensInDiag += board[row++][col++]) ;
            if (queensInDiag > 1)
            {
                return false;
            }
            // start indices for x from 0 to 2n - 1:
            // 0: row, col = 0, 4
            // 1: row, col = 0, 3
            // 2: row, col = 1, 4
            // 3: row, col = 0, 2
            // 4: row, col = 2, 4
            // 5: row, col = 0, 1
            // 6: row, col = 3, 4
            // => x even: row = intdiv(x / 2), col = n - 1
            // => x odd: row = 0, col = n - 2 - intdiv(x / 2)
            // rowStart is identical to above
            colStart = (even & (n - 1)) | (odd & (n - 2 - (x >> 1)));
            queensInDiag = 0;
            for (int row = rowStart, col = colStart; row < n && col >= 0 && queensInDiag <= 1; queensInDiag += board[row++][col--]) ;
            if (queensInDiag > 1)
            {
                return false;
            }
        }
        return true;
    }

    public override string ToString()
    {
        AssertValid();
        StringBuilder sb = new();
        int n = Size;
        sb.Append(' ', 3);
        for (int i = 0; i < n; ++i)
        {
            sb.Append(i);
            if (i != n - 1)
            {
                sb.Append(' ');
            }
        }
        sb.AppendLine();
        sb.Append("  +").Append('-', n + n - 1).Append('+').AppendLine();
        for (int row = 0; row < n; ++row)
        {
            sb.Append(row).Append(" |");
            for (int col = 0; col < n; ++col)
            {
                sb.Append(board[row][col]);
                if (col != n - 1)
                {
                    sb.Append(' ');
                }
            }
            sb.Append('|').AppendLine();
        }
        sb.Append("  +").Append('-', n + n - 1).Append('+').AppendLine();
        return sb.ToString();
    }
}