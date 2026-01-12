using System.Text;

namespace Min.Ak.Greedy.GraphColoring;

internal sealed record GCGreedySolution(int ColorCount, IReadOnlyList<GCGreedyNode> Nodes)
{
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("{ ColorCount: ").Append(ColorCount).Append(", Nodes: [");
        for (int i = 0; i < Nodes.Count; ++i)
        {
            if (i > 0)
            {
                sb.Append(", ");
            }
            sb.Append(Nodes[i]);
        }
        sb.Append("] }");
        return sb.ToString();
    }
}