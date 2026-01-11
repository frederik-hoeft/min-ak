using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace Min.Ak.BranchAndBound.Knapsack01;

[DebuggerDisplay("{ToString(),nq}")]
internal sealed record K01BabCandidate(K01BaB Bab, ImmutableArray<K01BaBSelection> Selections)
{
    private static readonly K01BaBOption s_dummy = new(string.Empty, 0f, 0f);

    public float PostselectGain { get; } = Selections.Sum(selection => selection switch
    {
        { Negate: false } => selection.Option.Gain,
        _ => 0f,
    });

    public float PreselectGain => PostselectGain - AddedSelection.Gain;

    public float PostselectCost { get; } = Selections.Sum(selection => selection switch
    {
        { Negate: false } => selection.Option.Cost,
        _ => 0f,
    });

    public float PreselectCost => PostselectCost - AddedSelection.Cost;

    public K01BaBOption AddedSelection => Selections is [.., { } last] ? last.Option : s_dummy;

    public float MaxGain => PreselectGain + ((Bab.MaxCost - PreselectCost) * AddedSelection.RelativeGain);

    public IEnumerable<K01BabCandidate> EnumerateChildren()
    {
        for (int i = Selections.Length; i < Bab.Options.Length; ++i)
        {
            K01BabCandidate childCandidate = new(Bab,
            [
                // everything we've already chosen or rejected
                .. Selections,
                // reject all options between current length and i
                .. Bab.Options[Selections.Length..Math.Max(i, Selections.Length)].Select(notChosen => new K01BaBSelection(notChosen, Negate: true)),
                // choose option i
                new K01BaBSelection(Bab.Options[i], Negate: false)
            ]);
            yield return childCandidate;
        }
    }

    public string GetSelections(bool includeRejected)
    {
        StringBuilder sb = new();
        foreach (K01BaBSelection selection in Selections)
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

    public K01BabSolution ToSolution() => new(this);
}
