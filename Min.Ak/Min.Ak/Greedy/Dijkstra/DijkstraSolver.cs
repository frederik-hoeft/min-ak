using Min.Ak.Collections;
using Min.Ak.Model.GraphTheory;
using System.Numerics;

namespace Min.Ak.Greedy.Dijkstra;

internal static class DijkstraSolver
{
    public static DijkstraSolution<T>? Solve<T>(IReadOnlyList<string> names, DistanceMatrix<T> distanceMatrix, string start, string target) where T : unmanaged, INumber<T>
    {
        OrderedBijectiveMap<string, int> nameIndexMap = new(names.Count);
        for (int i = 0; i < names.Count; i++)
        {
            nameIndexMap.Add(names[i], i);
        }
        Dijkstra<T> dijkstra = new(nameIndexMap, distanceMatrix);
        return dijkstra.Solve(start, target);
    }
}