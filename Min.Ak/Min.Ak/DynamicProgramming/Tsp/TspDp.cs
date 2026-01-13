using Min.Ak.Collections;
using Min.Ak.Model;
using Min.Ak.Model.Tsp;
using System.Diagnostics;
using System.Numerics;

namespace Min.Ak.DynamicProgramming.Tsp;

internal sealed record TspDp<T>(DistanceMatrix<T> DistanceMatrix, OrderedBijectiveMap<string, int> NameIndexMap) where T : unmanaged, INumber<T>
{
    public TspDpSolution<T> Solve()
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(NameIndexMap.Count, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(NameIndexMap.Count, 30);
        Dictionary<GraphIndexSet, DpCell[]> dpTable = new(capacity: 1 << NameIndexMap.Count);

        Queue<GraphIndexSet> queue = new();
        GraphIndexSet root = GraphIndexSet.Empty;
        queue.Enqueue(root);
        while (queue.TryDequeue(out GraphIndexSet set))
        {
            if (dpTable.ContainsKey(set))
            {
                continue;
            }
            DpCell[] row = new DpCell[NameIndexMap.Count];
            for (int j = 0; j < NameIndexMap.Count; ++j)
            {
                row[j] = Route(j, set, dpTable);
                if (j != 0 && !set.IsSet(j))
                {
                    queue.Enqueue(set.Add(j));
                }
            }
            dpTable[set] = row;
        }
        int currentIndex = 0;
        GraphIndexSet indexSet = GraphIndexSet.Full(NameIndexMap.Count);
        // we are starting from index 0, so we can just use that (distance to itself is 0)
        // d(1,1) + DP[1,{2,3,4}]
        DpCell final = dpTable[indexSet.Remove(currentIndex)][currentIndex];
        List<int> path = new(NameIndexMap.Count + 1);
        while (currentIndex != -1)
        {
            path.Add(currentIndex);
            indexSet = indexSet.Remove(currentIndex);
            currentIndex = dpTable[indexSet][currentIndex].PreviousIndex;
        }
        path.Add(0); // return to starting point, already included in cost, but not in path
        return new TspDpSolution<T>(this, final.MinCost, path);
    }

    private DpCell Route(int i, GraphIndexSet set, Dictionary<GraphIndexSet, DpCell[]> dpTable)
    {
        if (set.IsEmpty)
        {
            return new DpCell
            {
                MinCost = DistanceMatrix[i, 0],
                PreviousIndex = -1,
            };
        }
        DpCell min = new()
        {
            MinCost = DistanceMatrix.Infinity,
            PreviousIndex = -1,
        };
        foreach (int j in set)
        {
            T cost = DistanceMatrix[i, j];
            T pathCost = dpTable[set.Remove(j)][j].MinCost;
            if (cost == DistanceMatrix.Infinity || pathCost == DistanceMatrix.Infinity)
            {
                continue;
            }
            T totalCost = cost + pathCost;
            if (min.MinCost == DistanceMatrix.Infinity || totalCost < min.MinCost)
            {
                min.MinCost = totalCost;
                min.PreviousIndex = j;
            }
        }
        return min;
    }

    [DebuggerDisplay("{ToString(),nq}")]
    private struct DpCell
    {
        public T MinCost;
        public int PreviousIndex;

        public override readonly string ToString() => $"(MinCost: {MinCost}, Previous: {PreviousIndex})";
    }
}
