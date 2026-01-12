using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;

namespace Min.Ak.DynamicProgramming.KnapsackN;

internal static class KnDpSolver
{
    public static KnDpSolution<T> Solve<T>(int maxCost, ImmutableArray<KnapsackNOption<T>> options) where T : unmanaged, INumber<T>
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxCost);
        DpCell<T>[] dpTable = new DpCell<T>[maxCost + 1];
        for (int optionIndex = 0; optionIndex < options.Length; ++optionIndex)
        {
            KnapsackNOption<T> candidate = options[optionIndex];
            for (int j = candidate.Cost; j < dpTable.Length; ++j)
            {
                T candidateGain = dpTable[j - candidate.Cost].MaxGain + candidate.Gain;
                if (candidateGain > dpTable[j].MaxGain)
                {
                    dpTable[j].MaxGain = candidateGain;
                    dpTable[j].LastSelection = candidate;
                }
            }
        }
        Dictionary<KnapsackNOption<T>, int> selections = [];
        int i = dpTable.Length - 1;
        for (KnapsackNOption<T>? selection = dpTable[i].LastSelection; selection != null && i >= 0; selection = dpTable[i -= selection.Cost].LastSelection)
        {
            _ = selections.TryGetValue(selection, out int count);
            selections[selection] = ++count;
        }
        List<KnDpSelection<T>> results = new(selections.Count);
        foreach ((KnapsackNOption<T> option, int count) in selections)
        {
            results.Add(new KnDpSelection<T>(option, count));
        }
        Debug.Assert(dpTable[^1].MaxGain == results.Aggregate(T.Zero, (totalGain, r) => totalGain + Enumerable.Range(0, r.Quantity).Aggregate(T.Zero, (sum, _) => sum + r.Option.Gain)));
        return new KnDpSolution<T>(dpTable[^1].MaxGain, [.. results]);
    }

    private struct DpCell<T>() where T : unmanaged, INumber<T>
    {
        public T MaxGain = T.Zero;
        public KnapsackNOption<T>? LastSelection = null;

        public override readonly string ToString() => $"(MaxGain: {MaxGain}, LastSelection: {LastSelection?.Name ?? "null"})";
    }
}