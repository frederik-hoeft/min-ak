using Min.Ak.Greedy.Dijkstra;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Min.Ak.Greedy.AStar;

[DebuggerDisplay("{ToString(),nq}")]
internal sealed record AStarSolution<T>(AStar<T> AStar, LinkedList<AStar<T>.Node> Nodes) where T : unmanaged, INumber<T>
{
    public T Distance => Nodes.Last?.Value.GScore ?? AStar.DistanceMatrix.Infinity;

    public IEnumerable<string> Path
    {
        get
        {
            foreach (AStar<T>.Node node in Nodes)
            {
                yield return AStar.NameIndexMap.GetByValue(node.Index);
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("{ Distance = ").Append(Distance).Append(", Path = [");
        foreach (AStar<T>.Node node in Nodes)
        {
            if (node.PreviousNodeIndex != -1)
            {
                sb.Append(" -(").Append(AStar.DistanceMatrix[node.PreviousNodeIndex, node.Index]).Append(")-> ");
            }
            sb.Append(AStar.NameIndexMap.GetByValue(node.Index));
        }
        sb.Append("] }");
        return sb.ToString();
    }
}