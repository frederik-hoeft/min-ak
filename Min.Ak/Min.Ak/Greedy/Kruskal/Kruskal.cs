using Min.Ak.Collections;
using Min.Ak.Model.GraphTheory;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Min.Ak.Greedy.Kruskal;

internal sealed record Kruskal<T>(OrderedBijectiveMap<string, int> NameIndexMap, DistanceMatrix<T> DistanceMatrix) where T : unmanaged, INumber<T>
{
    public KruskalSolution<T>? Solve()
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(DistanceMatrix.Size, GraphIndexSet.Capacity);
        if (!DistanceMatrix.IsSymmetric())
        {
            throw new ArgumentException("Distance matrix must be symmetric.", nameof(DistanceMatrix));
        }
        int trees = 0;
        // forest of partial sub-trees
        GraphIndexSet[] forest = new GraphIndexSet[DistanceMatrix.Size];
        DynamicPriorityQueue<Edge, T> priorityQueue = new(SortOrder.Minimum, static e => e.Weight);
        // initialize edges
        for (int u = 0; u < DistanceMatrix.Size; u++)
        {
            for (int v = u + 1; v < DistanceMatrix.Size; v++)
            {
                T weight = DistanceMatrix[u, v];
                if (weight != DistanceMatrix.Infinity)
                {
                    priorityQueue.Enqueue(new Edge(this, u, v, weight));
                }
            }
        }
        List<Edge> selectedEdges = [];
        // loop until we have a single tree spanning all vertices or we run out of edges
        while (selectedEdges.Count < DistanceMatrix.Size - 1 && priorityQueue.Dequeue() is { } edge)
        {
            ref GraphIndexSet firstTree = ref Unsafe.NullRef<GraphIndexSet>();
            bool merged = false;
            GraphIndexSet edgeVertices = GraphIndexSet.Of(edge.U, edge.V);
            for (int i = 0; i < trees; ++i)
            {
                ref GraphIndexSet currentTree = ref forest[i];
                int verticesInTree = currentTree.Intersect(edgeVertices).Count;
                if (verticesInTree == 0)
                {
                    // neither vertex is in this tree, continue searching
                    continue;
                }
                if (verticesInTree == 2)
                {
                    // both vertices are already in the same tree, skip to avoid cycle
                    goto SKIP_EDGE;
                }
                if (Unsafe.IsNullRef(ref firstTree))
                {
                    // found the first tree containing one of the vertices
                    firstTree = ref currentTree;
                }
                else
                {
                    // found the second tree containing the other vertex, merge trees
                    firstTree = firstTree.Union(currentTree);
                    // remove the merged tree from the forest
                    forest[i] = forest[trees - 1];
                    --trees;
                    // we've merged two trees, no need to continue
                    merged = true;
                    break;
                }
            }
            if (Unsafe.IsNullRef(ref firstTree))
            {
                // neither vertex is in any tree, create a new tree
                forest[trees++] = edgeVertices;
            }
            else if (!merged)
            {
                // only one vertex was found in a tree, add the other vertex to that tree
                firstTree = firstTree.Union(edgeVertices);
            }
            selectedEdges.Add(edge);
        SKIP_EDGE:;
        }
        if (selectedEdges.Count != DistanceMatrix.Size - 1)
        {
            return null;
        }
        return new KruskalSolution<T>(this, selectedEdges);
    }

    [DebuggerDisplay("{ToString(),nq}")]
    internal sealed record Edge(Kruskal<T> Kruskal, int U, int V, T Weight)
    {
        public override string ToString() => $"({Kruskal.NameIndexMap.GetByValue(U)} <-({Weight})-> {Kruskal.NameIndexMap.GetByValue(V)})";
    }
}
