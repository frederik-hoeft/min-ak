using Min.Ak.Collections;
using Min.Ak.Model.GraphTheory;
using System.Collections.Immutable;
using System.Numerics;

namespace Min.Ak.DynamicProgramming.Tsp;

internal static class TspDpSolver
{
    public static TspDpSolution<T> Solve<T>(ImmutableArray<string> names, DistanceMatrix<T> distanceMatrix) where T : unmanaged, INumber<T>
    {
        OrderedBijectiveMap<string, int> nameIndexMap = new(names.Length);
        for (int i = 0; i < names.Length; ++i)
        {
            nameIndexMap.Add(names[i], i);
        }
        TspDp<T> dp = new(distanceMatrix, nameIndexMap);
        return dp.Solve();
    }
}