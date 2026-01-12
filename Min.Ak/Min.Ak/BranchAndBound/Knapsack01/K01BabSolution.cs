using System.Numerics;

namespace Min.Ak.BranchAndBound.Knapsack01;

internal readonly record struct K01BabSolution<T> where T : unmanaged, INumber<T>
{
    private readonly K01BabCandidate<T> _candidate;

    public K01BabSolution(K01BabCandidate<T> candidate)
    {
        _candidate = candidate;
        Selections = _candidate.GetSelections(includeRejected: false);
    }

    public string Selections { get; }

    public T TotalCost => _candidate.PostselectCost;

    public T TotalGain => _candidate.PostselectGain;
}