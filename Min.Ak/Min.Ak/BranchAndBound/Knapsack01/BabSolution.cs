namespace Min.Ak.BranchAndBound.Knapsack01;

internal readonly record struct BabSolution
{
    private readonly BabCandidate _candidate;

    public BabSolution(BabCandidate candidate)
    {
        _candidate = candidate;
        Selections = _candidate.GetSelections(includeRejected: false);
    }

    public string Selections { get; }

    public float TotalCost => _candidate.PostselectCost;

    public float TotalGain => _candidate.PostselectGain;
}