using Min.Ak.Model.K01;
using System.Numerics;
using System.Text;

namespace Min.Ak.Greedy.Knapsack01;

internal sealed class K01GreedyApproximation<T>(IReadOnlyList<Knapsack01Option<T>> selections) where T : unmanaged, INumber<T>
{
    public IReadOnlyList<Knapsack01Option<T>> Selections => selections;

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
            sb.Append(selections[i].Name);
        }
        sb.Append("] }");
        return sb.ToString();
    }
}