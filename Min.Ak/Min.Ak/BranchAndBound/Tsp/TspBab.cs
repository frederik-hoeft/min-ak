using Min.Ak.Collections;
using Min.Ak.Model.GraphTheory;
using System.Numerics;

namespace Min.Ak.BranchAndBound.Tsp;

internal sealed record TspBab<T>(DistanceMatrix<T> DistanceMatrix, OrderedBijectiveMap<string, int> NameIndexMap) where T : unmanaged, INumber<T>
{
    public List<TspBabSolution<T>> Solve()
    {
        DynamicPriorityQueue<TspBabCandidate<T>, T> candidates = new(SortOrder.Minimum, static c => c.LowerBound);
        string startingName = NameIndexMap.GetByValue(0);
        OrderedBijectiveMap<string, int> startingVisited = new(capacity: 1);
        startingVisited.Add(startingName, 0);
        TspBabCandidate<T> root = new(this, startingName, startingVisited, DistanceMatrix, currentBound: T.Zero, totalCost: T.Zero);
        candidates.Enqueue(root);
        List<TspBabCandidate<T>> bestCandidates = [];
        while (candidates.Dequeue() is { } candidate)
        {
            foreach (TspBabCandidate<T> child in candidate.EnumerateChildren())
            {
                if (bestCandidates is not [{ } bestCandidate, ..] || child.LowerBound <= bestCandidate.TotalCost)
                {
                    candidates.Enqueue(child);
                }
            }
            if (candidate.IsClosed)
            {
                if (bestCandidates is [{ TotalCost: { } bestTotalCost }, ..])
                {
                    if (candidate.TotalCost > bestTotalCost)
                    {
                        continue;
                    }
                    if (candidate.TotalCost < bestTotalCost)
                    {
                        bestCandidates.Clear();
                    }
                }
                bestCandidates.Add(candidate);
                candidates.PruneWorseThan(candidate.TotalCost);
            }
        }
        return [..bestCandidates.Select(c => c.ToSolution())];
    }
}
