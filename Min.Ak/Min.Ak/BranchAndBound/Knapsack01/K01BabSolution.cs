namespace Min.Ak.BranchAndBound.Knapsack01;

internal readonly record struct K01BabSolution
{
    private readonly K01BabCandidate _candidate;

    public K01BabSolution(K01BabCandidate candidate)
    {
        _candidate = candidate;
        Selections = _candidate.GetSelections(includeRejected: false);
    }

    public string Selections { get; }

    public float TotalCost => _candidate.PostselectCost;

    public float TotalGain => _candidate.PostselectGain;
}