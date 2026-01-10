namespace Min.Ak.BranchAndBound.Knapsack01;

internal readonly record struct BabSolution
{
    private readonly BabCandidate _candidate;

    public BabSolution(BabCandidate candidate) => _candidate = candidate;

    public float TotalCost => _candidate.PostselectCost;

    public float TotalGain => _candidate.PostselectGain;
}