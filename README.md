# MIN-AK — Algorithms & Complexity (C#)

A compact, educational collection of algorithm implementations written in C#/.NET. The repository is organized by technique and topic (Backtracking, Branch and Bound, Computability) with small, focused examples rather than a single runnable app.

## Project Overview

- Target Framework: .NET 10 (net10.0)
- Output: Console app (demo-driven `Program.cs`)
- Style: Minimal, internal types grouped by topic

Entry point in [Min.Ak/Min.Ak/Program.cs](Min.Ak/Min.Ak/Program.cs). Project file at [Min.Ak/Min.Ak/Min.Ak.csproj](Min.Ak/Min.Ak/Min.Ak.csproj).

## Repository Structure

The source code is organized by algorithmic technique:

- **[Backtracking](Min.Ak/Min.Ak/Backtracking)** — K-Queens problem
- **[BranchAndBound](Min.Ak/Min.Ak/BranchAndBound)** — 0/1 Knapsack, TSP
- **[DynamicProgramming](Min.Ak/Min.Ak/DynamicProgramming)** — N-Knapsack, TSP
- **[Greedy](Min.Ak/Min.Ak/Greedy)** — A*, Dijkstra, Graph Coloring, Kruskal MST, 0/1 Knapsack, Fractional Knapsack
- **[Computability](Min.Ak/Min.Ak/Computability)** — Ackermann function
- **[Collections](Min.Ak/Min.Ak/Collections)** — Custom data structures
- **[Model](Min.Ak/Min.Ak/Model)** — Shared problem models

Solution file: [Min.Ak.slnx](Min.Ak/Min.Ak.slnx) · Project: [Min.Ak.csproj](Min.Ak/Min.Ak/Min.Ak.csproj)

## Algorithms Implemented

- K-Queens (Backtracking)
	- Goal: Place K queens on an N×N board so none attack each other.
	- Method: Backtracking with row/column/diagonal constraints.
	- Types: `Min.Ak.Backtracking.KQueens.KQueens` with `Solve()` returning `List<KQueensSolution>`.

- 0/1 Knapsack (Branch and Bound)
	- Goal: Maximize total gain subject to a cost limit.
	- Method: Branch-and-Bound with item ordering by relative gain.
	- API: `Min.Ak.BranchAndBound.Knapsack01.K01BabSolver.Solve(maxCost, options)` where `options` is `IReadOnlyList<Knapsack01Option<T>>`.

- Traveling Salesman Problem (Branch and Bound)
	- Goal: Compute tours minimizing total distance on a complete graph.
	- Method: Branch-and-Bound with matrix reductions and pruning.
	- API: `Min.Ak.BranchAndBound.Tsp.TspBabSolver.Solve(names, distanceMatrix)` where `names` is `ImmutableArray<string>` and using `Min.Ak.Model.GraphTheory.DistanceMatrix<T>`.

- Ackermann Function (Computability)
	- Goal: Compare recursive vs iterative (stack-based) implementations.
	- Types: `RecursiveAck.Ack(n, m)` and `IterativeAck.Ack(n, m)` under `Min.Ak.Computability.Ackermann`.

- N-Knapsack (Dynamic Programming)
	- Goal: Maximize gain with unlimited quantities of each item, subject to cost limit.
	- Method: Dynamic programming with optimal substructure.
	- API: `Min.Ak.DynamicProgramming.KnapsackN.KnDpSolver.Solve(maxCost, options)`.

- TSP (Dynamic Programming)
	- Goal: Find optimal tour using Held-Karp algorithm.
	- Method: Dynamic programming over subsets of cities.
	- API: `Min.Ak.DynamicProgramming.Tsp.TspDpSolver.Solve(names, distanceMatrix)`.

- 0/1 Knapsack (Greedy Approximation)
	- Goal: Fast approximation for 0/1 Knapsack using greedy heuristic.
	- Method: Sort by gain-to-cost ratio and select greedily.
	- API: `Min.Ak.Greedy.Knapsack01.K01GreedySolver.Solve(maxCost, options)`.

- Fractional Knapsack (Greedy)
	- Goal: Maximize gain when items can be taken fractionally.
	- Method: Greedy by gain-to-cost ratio (optimal for fractional).
	- API: `Min.Ak.Greedy.KnapsackFractional.KFGreedySolver.Solve(maxCost, options)`.

- Graph Coloring (Greedy)
	- Goal: Color graph nodes such that no adjacent nodes share a color.
	- Method: Greedy sequential coloring.
	- API: `Min.Ak.Greedy.GraphColoring.GCGreedySolver.Solve(nodes, adjacencyList)`.

- Dijkstra's Shortest Path (Greedy)
	- Goal: Find shortest path between two nodes in a weighted graph.
	- Method: Greedy algorithm using priority queue with edge relaxation.
	- API: `Min.Ak.Greedy.Dijkstra.DijkstraSolver.Solve(names, distanceMatrix, start, target)`.

- A* Pathfinding (Greedy)
	- Goal: Find shortest path using both distance and heuristic estimates.
	- Method: Best-first search with heuristic guidance.
	- API: `Min.Ak.Greedy.AStar.AStarSolver.Solve(names, distanceMatrix, start, target, heuristic)`.

- Kruskal's Minimum Spanning Tree (Greedy)
	- Goal: Find minimum spanning tree of a weighted undirected graph.
	- Method: Sort edges by weight and add non-cyclic edges to the tree.
	- API: `Min.Ak.Greedy.Kruskal.KruskalSolver.Solve(names, distanceMatrix)`.

## Build & Run

Prerequisite: .NET 10 SDK (net10.0).

Basic commands:

```bash
dotnet build Min.Ak/Min.Ak/Min.Ak.csproj
dotnet run --project Min.Ak/Min.Ak/Min.Ak.csproj
```

Edit [Min.Ak/Min.Ak/Program.cs](Min.Ak/Min.Ak/Program.cs) to switch between demos.

## Quick Usage Examples

K-Queens:

```csharp
using Min.Ak.Backtracking.KQueens;

KQueens solver = new(N: 8, K: 8);
List<KQueensSolution> solutions = solver.Solve();
Console.WriteLine($"Found {solutions.Count} solutions.");
foreach (KQueensSolution solution in solutions)
{
    Console.WriteLine(solution.ToBoard());
}
```

0/1 Knapsack:

```csharp
using Min.Ak.BranchAndBound.Knapsack01;
using Min.Ak.Model.K01;

K01BabSolution<float>? result = K01BabSolver.Solve(maxCost: 600f, options:
[
    Knapsack01Option.Of("A", gain: 201f, cost: 191f),
    Knapsack01Option.Of("B", gain: 141f, cost: 239f),
    Knapsack01Option.Of("C", gain:  48f, cost: 148f),
    Knapsack01Option.Of("D", gain: 232f, cost: 153f),
    Knapsack01Option.Of("E", gain:  50f, cost:  66f),
    Knapsack01Option.Of("F", gain:  79f, cost: 137f),
    Knapsack01Option.Of("G", gain:  38f, cost: 249f),
    Knapsack01Option.Of("H", gain:  73f, cost:  54f),
]);
Console.WriteLine(result?.ToString() ?? "No solution found.");
```

TSP (Branch and Bound):

```csharp
using Min.Ak.BranchAndBound.Tsp;
using Min.Ak.Model.GraphTheory;

List<TspBabSolution<int>> results = TspBabSolver.Solve
(
    ["A", "B", "C", "D", "E"],
    DistanceMatrix.Create(
    [
        [-1,  6, 15, 15,  8],
        [ 7, -1,  9, 11,  5],
        [15, 11, -1,  5, 18],
        [16, 12,  7, -1, 17],
        [ 6,  9, 20, 14, -1]
    ], infinityValue: -1)
);
Console.WriteLine(results.Count == 0 ? "No solution found." : string.Join('\n', results));
```

Dijkstra's Shortest Path:

```csharp
using Min.Ak.Greedy.Dijkstra;
using Min.Ak.Model.GraphTheory;

DijkstraSolution<int>? result = DijkstraSolver.Solve(
    names: ["A", "B", "C", "D"],
    distanceMatrix: DistanceMatrix.Create(
    [
        [int.MaxValue, 1, 4, int.MaxValue],
        [1, int.MaxValue, 2, 5],
        [4, 2, int.MaxValue, 1],
        [int.MaxValue, 5, 1, int.MaxValue]
    ], infinityValue: int.MaxValue),
    start: "A",
    target: "D"
);
Console.WriteLine(result?.ToString() ?? "No path found.");
```

A* Pathfinding:

```csharp
using Min.Ak.Greedy.AStar;
using Min.Ak.Model.GraphTheory;

AStarSolution<int>? result = AStarSolver.Solve(
    names: ["A", "B", "C", "D"],
    distanceMatrix: DistanceMatrix.Create(
    [
        [int.MaxValue, 1, 4, int.MaxValue],
        [1, int.MaxValue, 2, 5],
        [4, 2, int.MaxValue, 1],
        [int.MaxValue, 5, 1, int.MaxValue]
    ], infinityValue: int.MaxValue),
    start: "A",
    target: "D",
    heuristic: (from, to) => ... // define heuristic function here, must be consistent
);
Console.WriteLine(result?.ToString() ?? "No path found.");
```

Kruskal's Minimum Spanning Tree:

```csharp
using Min.Ak.Greedy.Kruskal;
using Min.Ak.Model.GraphTheory;

KruskalSolution<int>? result = KruskalSolver.Solve(
    ["A", "B", "C", "D"],
    DistanceMatrix.Create(
    [
        [-1, 1, 3, -1],
        [1, -1, 2, 4],
        [3, 2, -1, 1],
        [-1, 4, 1, -1]
    ], infinityValue: -1)
);
Console.WriteLine(result?.ToString() ?? "No spanning tree found.");
```

Ackermann (recursive vs iterative):

```csharp
using Min.Ak.Computability.Ackermann;

Console.WriteLine(RecursiveAck.Ack(2, 3));
Console.WriteLine(IterativeAck.Ack(2, 3));
```

## Notes

- The code favors clarity over UI or I/O — adjust `Program.cs` to run specific demos.
- AOT publishing is enabled; try native publishing (SDK and platform permitting):

```bash
dotnet publish Min.Ak/Min.Ak/Min.Ak.csproj -c Release -r win-x64
```

## License

Educational use. No explicit license included.

