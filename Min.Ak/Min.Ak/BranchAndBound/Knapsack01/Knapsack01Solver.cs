namespace Min.Ak.BranchAndBound.Knapsack01;

internal static class Knapsack01Solver
{
    public static BabSolution? Solve(float maxCost, List<BaBOption> options)
    {
        BaB bab = new(maxCost, options);
        return bab.Solve();
    }
}