using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Min.Ak.Greedy.Dijkstra;

[DebuggerDisplay("{ToString(),nq}")]
internal sealed record DijkstraSolution<T>(Dijkstra<T> Dijkstra, LinkedList<Dijkstra<T>.Node> Nodes) where T : unmanaged, INumber<T>
{
    public T Distance => Nodes.Last?.Value.MinDistance ?? Dijkstra.DistanceMatrix.Infinity;

    public IEnumerable<string> Path
    {
        get
        {
            foreach (Dijkstra<T>.Node node in Nodes)
            {
                yield return Dijkstra.NameIndexMap.GetByValue(node.Index);
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("{ Distance = ").Append(Distance).Append(", Path = [");
        foreach (Dijkstra<T>.Node node in Nodes)
        {
            if (node.PreviousNodeIndex != -1)
            {
                sb.Append(" -(").Append(Dijkstra.DistanceMatrix[node.PreviousNodeIndex, node.Index]).Append(")-> ");
            }
            sb.Append(Dijkstra.NameIndexMap.GetByValue(node.Index));
        }
        sb.Append("] }");
        return sb.ToString();
    }
}