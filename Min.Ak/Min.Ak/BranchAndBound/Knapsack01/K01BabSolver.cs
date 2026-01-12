using Min.Ak.Model.K01;
using System.Numerics;

namespace Min.Ak.BranchAndBound.Knapsack01;

internal static class K01BabSolver
{
    public static K01BabSolution<T>? Solve<T>(T maxCost, IReadOnlyList<Knapsack01Option<T>> options) where T : unmanaged, INumber<T>
    {
        K01BaB<T> bab = new(maxCost, options);
        return bab.Solve();
    }
}
