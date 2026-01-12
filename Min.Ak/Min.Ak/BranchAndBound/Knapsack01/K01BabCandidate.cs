using Min.Ak.Model.K01;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Min.Ak.BranchAndBound.Knapsack01;

[DebuggerDisplay("{ToString(),nq}")]
internal sealed record K01BabCandidate<T>(K01BaB<T> Bab, ImmutableArray<K01BaBSelection<T>> Selections) where T : unmanaged, INumber<T>
{
    private static readonly Knapsack01Option<T> s_dummy = new(string.Empty, T.Zero, T.Zero);

    public T PostselectGain { get; } = Selections.Aggregate(T.Zero, (sum, selection) => sum + selection switch
    {
        { Negate: false } => selection.Option.Gain,
        _ => T.Zero,
    });

    public T PreselectGain => PostselectGain - AddedSelection.Gain;

    public T PostselectCost { get; } = Selections.Aggregate(T.Zero, (sum, selection) => sum + selection switch
    {
        { Negate: false } => selection.Option.Cost,
        _ => T.Zero,
    });

    public T PreselectCost => PostselectCost - AddedSelection.Cost;

    public Knapsack01Option<T> AddedSelection => Selections is [.., { } last] ? last.Option : s_dummy;

    public T MaxGain => PreselectGain + ((Bab.MaxCost - PreselectCost) * AddedSelection.RelativeGain);

    public IEnumerable<K01BabCandidate<T>> EnumerateChildren()
    {
        for (int i = Selections.Length; i < Bab.Options.Length; ++i)
        {
            K01BabCandidate<T> childCandidate = new(Bab,
            [
                // everything we've already chosen or rejected
                .. Selections,
                // reject all options between current length and i
                .. Bab.Options[Selections.Length..Math.Max(i, Selections.Length)].Select(notChosen => new K01BaBSelection<T>(notChosen, Negate: true)),
                // choose option i
                new K01BaBSelection<T>(Bab.Options[i], Negate: false)
            ]);
            yield return childCandidate;
        }
    }

    public string GetSelections(bool includeRejected)
    {
        StringBuilder sb = new();
        foreach (K01BaBSelection<T> selection in Selections)
        {
            if (!includeRejected && selection.Negate)
            {
                continue;
            }
            if (selection.Negate)
            {
                sb.Append('!');
            }
            sb.Append(selection.Option.Name);
        }
        return sb.ToString();
    }

    public override string ToString()
    {
        StringBuilder sb = new(GetSelections(includeRejected: true));
        sb.Append("(Cost: ").Append(PreselectCost).Append(", Gain: ").Append(PreselectGain).Append(", MaxGain: ").Append(MaxGain).Append(')');
        return sb.ToString();
    }

    public K01BabSolution<T> ToSolution() => new(this);
}
