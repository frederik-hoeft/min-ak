using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Min.Ak.Greedy.Kruskal;

[DebuggerDisplay("{ToString(),nq}")]
internal sealed record KruskalSolution<T>(Kruskal<T> Kruskal, IReadOnlyList<Kruskal<T>.Edge> SelectedEdges) where T : unmanaged, INumber<T>
{
    public T TotalWeight => ComputeTotalWeight();

    private T ComputeTotalWeight()
    {
        T total = T.Zero;
        foreach (Kruskal<T>.Edge edge in SelectedEdges)
        {
            total += edge.Weight;
        }
        return total;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("{ TotalWeight = ").Append(TotalWeight).Append(", Edges = [");
        for (int i = 0; i < SelectedEdges.Count; i++)
        {
            Kruskal<T>.Edge edge = SelectedEdges[i];
            sb.Append(edge);
            if (i < SelectedEdges.Count - 1)
            {
                sb.Append(", ");
            }
        }
        sb.Append("] }");
        return sb.ToString();
    }
}