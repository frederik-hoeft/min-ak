using Min.Ak.Collections;
using Min.Ak.Model.K01;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Min.Ak.BranchAndBound.Knapsack01;

internal sealed class K01BaB<T>(T maxCost, IReadOnlyList<Knapsack01Option<T>> options) where T : unmanaged, INumber<T>
{
    public T MaxCost => maxCost;

    public ImmutableArray<Knapsack01Option<T>> Options { get; } = [.. options.OrderByDescending(o => o.RelativeGain)];

    [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "False positive")]
    public K01BabSolution<T>? Solve()
    {
        PrunablePriorityQueue<K01BabCandidate<T>, T> candidates = new(SortOrder.Maximum, static c => c.MaxGain);
        K01BabCandidate<T> root = new(Bab: this, Selections: []);
        candidates.Enqueue(root);
        K01BabCandidate<T>? bestCandidate = null;
        while (candidates.Dequeue() is { } candidate)
        {
            foreach (K01BabCandidate<T> child in candidate.EnumerateChildren())
            {
                if (candidate.PostselectCost + child.AddedSelection.Cost <= MaxCost && (bestCandidate is null || child.MaxGain > bestCandidate.PostselectGain))
                {
                    candidates.Enqueue(child);
                }
            }
            if (bestCandidate is null || candidate.PostselectGain > bestCandidate.PostselectGain)
            {
                bestCandidate = candidate;
                candidates.PruneWorseThan(candidate.PostselectGain);
            }
        }
        return bestCandidate?.ToSolution();
    }
}
