using Min.Ak.Collections;
using Min.Ak.Model.Tsp;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Min.Ak.Greedy.Dijkstra;

internal sealed record Dijkstra<T>(OrderedBijectiveMap<string, int> NameIndexMap, DistanceMatrix<T> DistanceMatrix) where T : unmanaged, INumber<T>
{
    public DijkstraSolution<T>? Solve(string start, string target)
    {
        // require infinity sentinel to be positive, obviously should be larger than real values, but that's not our problem to worry about
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(DistanceMatrix.Infinity);
        if (!NameIndexMap.ContainsKey(start) || !NameIndexMap.ContainsKey(target))
        {
            return null;
        }
        int startIndex = NameIndexMap.GetByKey(start);
        int targetIndex = NameIndexMap.GetByKey(target);
        DynamicPriorityQueue<Node, T> priorityQueue = new(SortOrder.Minimum, static node => node.MinDistance);
        Node[] nodes = new Node[NameIndexMap.Count];
        for (int i = 0; i < nodes.Length; ++i)
        {
            T initialDistance = DistanceMatrix.Infinity;
            if (i == startIndex)
            {
                initialDistance = T.Zero;
            }
            Node node = new(this, i)
            {
                MinDistance = initialDistance,
            };
            nodes[i] = node;
            priorityQueue.Enqueue(node);
        }
        while (priorityQueue.Dequeue() is { } current)
        {
            if (current.MinDistance == DistanceMatrix.Infinity)
            {
                // can't reach target
                return null;
            }
            if (current.Index == targetIndex)
            {
                // reached target
                break;
            }
            foreach (int neigh in DistanceMatrix.Neighbors(current.Index))
            {
                Node neighbor = nodes[neigh];
                // if the node isn't in the PQ then we already visited it
                if (neigh == current.Index || !priorityQueue.Contains(neighbor))
                {
                    continue;
                }
                T distance = DistanceMatrix[current.Index, neighbor.Index];
                Debug.Assert(distance != DistanceMatrix.Infinity);
                T distanceToNeighborViaCurrent = current.MinDistance + distance;
                if (neighbor.MinDistance == DistanceMatrix.Infinity || distanceToNeighborViaCurrent < neighbor.MinDistance)
                {
                    neighbor.MinDistance = distanceToNeighborViaCurrent;
                    neighbor.PreviousNodeIndex = current.Index;
                    bool refreshed = priorityQueue.TryRefresh(neighbor);
                    Debug.Assert(refreshed);
                }
            }
        }
        // construct result
        LinkedList<Node> result = [];
        Node? n = null;
        for (int i = targetIndex; i != -1; i = n.PreviousNodeIndex)
        {
            n = nodes[i];
            result.AddFirst(n);
        }
        if (result.First?.Value.Index != startIndex)
        {
            return null;
        }
        return new DijkstraSolution<T>(this, result);
    }

    [DebuggerDisplay("{ToString(),nq}")]
    internal sealed class Node(Dijkstra<T> dijkstra, int index)
    {
        public required T MinDistance { get; set; }

        public int PreviousNodeIndex { get; set; } = -1;

        public Dijkstra<T> Dijkstra => dijkstra;

        public int Index => index;

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("{ ").Append(Dijkstra.NameIndexMap.GetByValue(Index)).Append(": ");
            sb.Append("MinDistance = ").Append(MinDistance).Append(", ");
            sb.Append("PreviousNode = ").Append(PreviousNodeIndex == -1 ? "null" : Dijkstra.NameIndexMap.GetByValue(PreviousNodeIndex)).Append(" }");
            return sb.ToString();
        }

        public override int GetHashCode() => Index;
    }
}
