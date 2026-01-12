namespace Min.Ak.BranchAndBound.Knapsack01;

internal static class K01BabSolver
{
    public static K01BabSolution? Solve(float maxCost, List<K01BaBOption> options)
    {
        K01BaB bab = new(maxCost, options);
        return bab.Solve();
    }
}
