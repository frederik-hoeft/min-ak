using Min.Ak.Collections;
using Min.Ak.Model.GraphTheory;
using System.Numerics;

namespace Min.Ak.Greedy.Kruskal;

internal static class KruskalSolver
{
    public static KruskalSolution<T>? Solve<T>(IReadOnlyList<string> names, DistanceMatrix<T> distanceMatrix) where T : unmanaged, INumber<T>
    {
        OrderedBijectiveMap<string, int> nameIndexMap = new(names.Count);
        for (int i = 0; i < names.Count; i++)
        {
            nameIndexMap.Add(names[i], i);
        }
        Kruskal<T> kruskal = new(nameIndexMap, distanceMatrix);
        return kruskal.Solve();
    }
}