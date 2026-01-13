using Min.Ak.Model.GraphTheory;

namespace Min.Ak.Greedy.GraphColoring;

internal static class GCGreedySolver
{
    public static void DegreeSortHeuristic(IReadOnlyList<string> names, DistanceMatrix<int> incidenceMatrix, List<GCGreedyNode> nodeOrder)
    {
        int[] degrees = new int[incidenceMatrix.Size];
        for (int i = 0; i < degrees.Length; ++i)
        {
            degrees[i] = incidenceMatrix.Size - incidenceMatrix[i].AsSpan().Count(0);
        }
        nodeOrder.Sort((a, b) => degrees[b.Index].CompareTo(degrees[a.Index]));
    }

    public static GCGreedySolution Solve(IReadOnlyList<string> names, DistanceMatrix<int> incidenceMatrix, Action<IReadOnlyList<string>, DistanceMatrix<int>, List<GCGreedyNode>>? sortHeuristic = null)
    {
        List<GCGreedyNode> nodeOrder = new(names.Count);
        GCGreedyNode[] nodes = new GCGreedyNode[names.Count];
        for (int r = 0; r < nodes.Length; ++r)
        {
            GCGreedyNode node = new(Name: names[r], Index: r);
            nodes[r] = node;
            nodeOrder.Add(node);
        }
        sortHeuristic?.Invoke(names, incidenceMatrix, nodeOrder);
        int colorCount = 0;
        foreach (GCGreedyNode node in nodeOrder)
        {
            for (int color = 0; color < colorCount; ++color)
            {
                for (int neighbor = 0; neighbor < incidenceMatrix.Size; ++neighbor)
                {
                    if (nodes[neighbor].Color == color && incidenceMatrix[node.Index, neighbor] != 0)
                    {
                        goto NEXT_COLOR;
                    }
                }
                node.Color = color;
            NEXT_COLOR:;
            }
            if (node.Color == -1)
            {
                node.Color = colorCount++;
            }
        }
        return new GCGreedySolution(colorCount, nodes);
    }
}