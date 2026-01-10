using System.Diagnostics;

namespace Min.Ak.Backtracking.KQueens;

/// <summary>
/// Exercise 17 (Backtracking):
/// The k-queens puzzle is defined as follows: Given an 8*8 chessboard, desired is
/// a placement of k queens on the board, so that no queens threaten each other.
/// Define an algorithm solving the k-queens puzzle that uses the Backtracking principle from
/// the lecture.
/// </summary>
internal sealed record KQueens(int N, int K)
{
    private readonly HashSet<int> _placements = new(K);
    private readonly bool[] _rows = new bool[N];
    private readonly bool[] _cols = new bool[N];
    private readonly bool[] _diag1 = new bool[N + N - 1];
    private readonly bool[] _diag2 = new bool[N + N - 1];

    public int CellCount { get; } = N * N;

    public List<KQueensSolution> Solve()
    {
        List<KQueensSolution> solutions = [];
        SolveKQueensBacktracking(solutions, 0);
        return solutions;
    }

    public void SolveKQueensBacktracking(List<KQueensSolution> solutions, int startCell)
    {
        // base case
        if (QueensPlaced == K)
        {
            solutions.Add(SnapshotAsSolution());
            return;
        }
        for (int cell = startCell; cell < CellCount; ++cell)
        {
            if (IsValidPlacement(cell))
            {
                // make choice
                PlaceQueen(cell);
                // recurse
                SolveKQueensBacktracking(solutions, cell);
                // backtrack
                RemoveQueen(cell);
            }
        }
    }

    private ref bool Rows(int row) => ref _rows[row];

    private ref bool Columns(int col) => ref _cols[col];

    private ref bool Diag1(int row, int col) => ref _diag1[row + col];

    private ref bool Diag2(int row, int col) => ref _diag2[row - col + N - 1];

    private bool IsValidPlacement(int row, int col) => !Rows(row) && !Columns(col) && !Diag1(row, col) && !Diag2(row, col);

    private bool IsValidPlacement(int cell) => IsValidPlacement(cell / N, cell % N);

    private int QueensPlaced => _placements.Count;

    private KQueensSolution SnapshotAsSolution()
    {
        Debug.Assert(_placements.Count == K);
        return new KQueensSolution(N, [.. _placements]);
    }

    private void PlaceQueen(int cell)
    {
        int row = cell / N;
        int col = cell % N;
        Debug.Assert(IsValidPlacement(row, col));
        Rows(row) = Columns(col) = Diag1(row, col) = Diag2(row, col) = true;
        _placements.Add(cell);
    }

    private void RemoveQueen(int cell)
    {
        Debug.Assert(_placements.Contains(cell));
        int row = cell / N;
        int col = cell % N;
        Rows(row) = Columns(col) = Diag1(row, col) = Diag2(row, col) = false;
        _placements.Remove(cell);
    }
}
