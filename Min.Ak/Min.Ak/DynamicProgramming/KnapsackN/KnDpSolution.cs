using System.Collections.Immutable;
using System.Numerics;
using System.Text;

namespace Min.Ak.DynamicProgramming.KnapsackN;

internal sealed record KnDpSolution<T>(T TotalGain, ImmutableArray<KnDpSelection<T>> Selections) where T : unmanaged, INumber<T>
{
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("{ TotalGain: ").Append(TotalGain).Append(", TotalCost: ").Append(Selections.Aggregate(0, (totalCost, s) => totalCost + (s.Quantity * s.Option.Cost)))
          .Append(", Selections: [");
        for (int i = 0; i < Selections.Length; ++i)
        {
            if (i > 0)
            {
                sb.Append(", ");
            }
            sb.Append(Selections[i].Option.Name).Append(" (").Append(Selections[i].Quantity).Append("x)");
        }
        sb.Append("] }");
        return sb.ToString();
    }
}
