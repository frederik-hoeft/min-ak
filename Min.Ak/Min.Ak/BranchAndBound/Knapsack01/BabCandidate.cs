using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace Min.Ak.BranchAndBound.Knapsack01;

[DebuggerDisplay("{ToString(),nq}")]
internal sealed record BabCandidate(BaB Bab, ImmutableArray<BaBSelection> Selections)
{
    private static readonly BaBOption s_dummy = new(string.Empty, 0f, 0f);

    public float PostselectGain { get; } = Selections.Sum(selection => selection switch
    {
        { Negate: false } => selection.Option.Gain,
        _ => 0f,
    });

    public float PreselectGain => PostselectGain - LastOption.Gain;

    public float PostselectCost { get; } = Selections.Sum(selection => selection switch
    {
        { Negate: false } => selection.Option.Cost,
        _ => 0f,
    });

    public float PreselectCost => PostselectCost - LastOption.Cost;

    public BaBOption LastOption => Selections is [.., { } last] ? last.Option : s_dummy;

    public float MaxGain => PreselectGain + (Bab.MaxCost - PreselectCost) * LastOption.RelativeGain;

    public IEnumerable<BabCandidate> EnumerateChildren()
    {
        for (int i = Selections.Length; i < Bab.Options.Length; ++i)
        {
            BabCandidate childCandidate = new(Bab,
            [
                // everything we've already chosen or rejected
                .. Selections,
                // reject all options between current length and i
                .. Bab.Options[Selections.Length..Math.Max(i, Selections.Length)].Select(notChosen => new BaBSelection(notChosen, Negate: true)),
                // choose option i
                new BaBSelection(Bab.Options[i], Negate: false)
            ]);
            yield return childCandidate;
        }
    }

    public string GetSelections(bool includeRejected)
    {
        StringBuilder sb = new();
        foreach (BaBSelection selection in Selections)
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

    public BabSolution ToSolution() => new(this);
}
