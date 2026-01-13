using Min.Ak.Collections;
using Min.Ak.Model.GraphTheory;
using System.Numerics;

namespace Min.Ak.Greedy.AStar;

internal static class AStarSolver
{
    /// <summary>
    /// Solves the A* pathfinding problem.
    /// </summary>
    /// <typeparam name="T">The numeric type used for distances.</typeparam>
    /// <param name="names">The list of node names.</param>
    /// <param name="distanceMatrix">The distance matrix representing distances between nodes.</param>
    /// <param name="start">The starting node name.</param>
    /// <param name="target">The target node name.</param>
    /// <param name="heuristic">The heuristic function estimating the additional cost from a node to the target, given the index of the node and the target node.</param>
    /// <returns></returns>
    public static AStarSolution<T>? Solve<T>(
        IReadOnlyList<string> names, 
        DistanceMatrix<T> distanceMatrix, 
        string start, 
        string target,
        Func<int, int, T> heuristic
    ) where T : unmanaged, INumber<T>
    {
        OrderedBijectiveMap<string, int> nameIndexMap = new(names.Count);
        for (int i = 0; i < names.Count; i++)
        {
            nameIndexMap.Add(names[i], i);
        }
        AStar<T> aStar = new(nameIndexMap, distanceMatrix, heuristic);
        return aStar.Solve(start, target);
    }
}