using Min.Ak.Collections;
using Min.Ak.Model.Tsp;
using System.Collections.Immutable;
using System.Numerics;

namespace Min.Ak.BranchAndBound.Tsp;

internal static class TspBabSolver
{
    public static List<TspBabSolution<T>> Solve<T>(ImmutableArray<string> names, DistanceMatrix<T> distanceMatrix) where T : unmanaged, INumber<T>
    {
        if (names.Length != distanceMatrix.Size)
        {
            throw new ArgumentException("The number of names must match the size of the distance matrix.", nameof(names));
        }
        OrderedBijectiveMap<string, int> nameIndexMap = new(names.Length);
        for (int i = 0; i < names.Length; ++i)
        {
            nameIndexMap.Add(names[i], i);
        }
        TspBab<T> bab = new(distanceMatrix, nameIndexMap);
        return bab.Solve();
    }
}