using Min.Ak.Collections;
using Min.Ak.Model.GraphTheory;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Min.Ak.Greedy.AStar;

internal sealed record AStar<T>
(
    OrderedBijectiveMap<string, int> NameIndexMap,
    DistanceMatrix<T> DistanceMatrix,
    Func<int, int, T> Heuristic
) where T : unmanaged, INumber<T>
{
    public AStarSolution<T>? Solve(string start, string target)
    {
        // require infinity sentinel to be positive, obviously should be larger than real values, but that's not our problem to worry about
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(DistanceMatrix.Infinity);
        if (!NameIndexMap.ContainsKey(start) || !NameIndexMap.ContainsKey(target))
        {
            return null;
        }
        int startIndex = NameIndexMap.GetByKey(start);
        int targetIndex = NameIndexMap.GetByKey(target);
        DynamicPriorityQueue<Node, T> priorityQueue = new(SortOrder.Minimum, static node => node.FScore);
        Node[] nodes = new Node[NameIndexMap.Count];
        for (int i = 0; i < nodes.Length; ++i)
        {
            T estimatedInitialDistance = DistanceMatrix.Infinity;
            T minInitialDistance = DistanceMatrix.Infinity;
            if (i == startIndex)
            {
                estimatedInitialDistance = Heuristic(startIndex, targetIndex);
                minInitialDistance = T.Zero;
            }
            Node node = new(aStar: this, index: i)
            {
                FScore = estimatedInitialDistance,
                GScore = minInitialDistance,
            };
            nodes[i] = node;
            priorityQueue.Enqueue(node);
        }
        while (priorityQueue.Dequeue() is { } current)
        {
            if (current.FScore == DistanceMatrix.Infinity)
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
                T tentativeG = current.GScore + distance;
                if (tentativeG < neighbor.GScore)
                {
                    neighbor.GScore = tentativeG;
                    neighbor.FScore = tentativeG + Heuristic(neighbor.Index, targetIndex);
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
        return new AStarSolution<T>(this, result);
    }

    [DebuggerDisplay("{ToString(),nq}")]
    internal sealed class Node(AStar<T> aStar, int index)
    {
        public required T FScore { get; set; }

        public required T GScore { get; set; }

        public int PreviousNodeIndex { get; set; } = -1;

        public AStar<T> AStar => aStar;

        public int Index => index;

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("{ ").Append(AStar.NameIndexMap.GetByValue(Index)).Append(": ");
            sb.Append("FScore = ").Append(FScore).Append(", ");
            sb.Append("MinDistance = ").Append(GScore).Append(", ");
            sb.Append("PreviousNode = ").Append(PreviousNodeIndex == -1 ? "null" : AStar.NameIndexMap.GetByValue(PreviousNodeIndex)).Append(" }");
            return sb.ToString();
        }

        public override int GetHashCode() => Index;
    }
}
