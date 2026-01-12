using System.Numerics;
using System.Text;

namespace Min.Ak.Greedy.KnapsackFractional;

internal sealed class KFGreedySolution<T>(IReadOnlyList<KFGreedySelection<T>> selections) where T : unmanaged, IFloatingPoint<T>
{
    public IReadOnlyList<KFGreedySelection<T>> Selections => selections;

    public T TotalCost { get; } = selections.Aggregate(T.Zero, (sum, selection) => sum + selection.Cost);

    public T TotalGain { get; } = selections.Aggregate(T.Zero, (sum, selection) => sum + selection.Gain);

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("{ TotalCost: ").Append(TotalCost).Append(", TotalGain: ").Append(TotalGain).Append(", Selections: [");
        for (int i = 0; i < selections.Count; ++i)
        {
            if (i > 0)
            {
                sb.Append(", ");
            }
            sb.Append(selections[i].Option.Name).Append(" (").Append(selections[i].Fraction).Append("x)");
        }
        sb.Append("] }");
        return sb.ToString();
    }
}