using System.Collections.Immutable;

namespace Min.Ak.BranchAndBound.Knapsack01;

internal sealed class BaB(float maxCost, List<BaBOption> options)
{
    public float MaxCost => maxCost;

    public ImmutableArray<BaBOption> Options { get; } = [.. options.OrderByDescending(o => o.RelativeGain)];

    public BabSolution? Solve()
    {
        OrderedDescList<BabCandidate, float> candidates = new(static c => c.MaxGain);
        BabCandidate root = new(Bab: this, Selections: []);
        candidates.Enqueue(root);
        BabCandidate? bestCandidate = null;
        while (candidates.Dequeue() is { } candidate)
        {
            foreach (BabCandidate child in candidate.EnumerateChildren())
            {
                if (candidate.PostselectCost + child.LastOption.Cost <= MaxCost && (bestCandidate is null || child.MaxGain > bestCandidate.PostselectGain))
                {
                    candidates.Enqueue(child);
                }
            }
            if (bestCandidate is null || candidate.PostselectGain > bestCandidate.PostselectGain)
            {
                bestCandidate = candidate;
                candidates.PruneBelow(candidate.PostselectGain);
            }
        }
        return bestCandidate?.ToSolution();
    }

    public static BaBOption Option(string name, float gain, float cost) => new(name, gain, cost);
}
