using Min.Ak.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Min.Ak.BranchAndBound.Knapsack01;

internal sealed class K01BaB(float maxCost, List<K01BaBOption> options)
{
    public float MaxCost => maxCost;

    public ImmutableArray<K01BaBOption> Options { get; } = [.. options.OrderByDescending(o => o.RelativeGain)];

    [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "False positive")]
    public K01BabSolution? Solve()
    {
        PrunablePriorityQueue<K01BabCandidate, float> candidates = new(SortOrder.Maximum, static c => c.MaxGain);
        K01BabCandidate root = new(Bab: this, Selections: []);
        candidates.Enqueue(root);
        K01BabCandidate? bestCandidate = null;
        while (candidates.Dequeue() is { } candidate)
        {
            foreach (K01BabCandidate child in candidate.EnumerateChildren())
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

    public static K01BaBOption Option(string name, float gain, float cost) => new(name, gain, cost);
}
