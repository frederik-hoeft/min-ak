using Min.Ak.Model.Tsp;

namespace Min.Ak.Greedy.Kruskal;

internal static class KruskalSample
{
    public static void RunSample()
    {
        KruskalSolution<int>? result = KruskalSolver.Solve
        (
            ["A", "B", "C", "D", "E", "F", "G"],
            DistanceMatrix.Create(
            [
                [-1,  7, -1,  5, -1, -1, -1],
                [ 7, -1,  8,  9,  7, -1, -1],
                [-1,  8, -1, -1,  5, -1, -1],
                [ 5,  9, -1, -1, 15,  6, -1],
                [-1,  7,  5, 15, -1,  8,  9],
                [-1, -1, -1,  6,  8, -1, 11],
                [-1, -1, -1, -1,  9, 11, -1],
            ], infinityValue: -1)
        );
        Console.WriteLine(result?.ToString() ?? "No spanning tree found.");
    }
}
