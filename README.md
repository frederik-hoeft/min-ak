# MIN-AK — Algorithms & Complexity (C#)

A compact, educational collection of algorithm implementations written in C#/.NET. The repository is organized by technique and topic (Backtracking, Branch and Bound, Computability) with small, focused examples rather than a single runnable app.

## Project Overview

- Target Framework: .NET 10 (net10.0)
- Output: Console app (demo-driven `Program.cs`)
- Style: Minimal, internal types grouped by topic

Entry point in [Min.Ak/Min.Ak/Program.cs](Min.Ak/Min.Ak/Program.cs). Project file at [Min.Ak/Min.Ak/Min.Ak.csproj](Min.Ak/Min.Ak/Min.Ak.csproj).

## Repository Structure (simplified)

- Solution: [Min.Ak/Min.Ak.slnx](Min.Ak/Min.Ak.slnx)
- Project: [Min.Ak/Min.Ak/Min.Ak.csproj](Min.Ak/Min.Ak/Min.Ak.csproj)
- Source (by topic): [Min.Ak/Min.Ak](Min.Ak/Min.Ak)
	- Backtracking: [Backtracking/KQueens](Min.Ak/Min.Ak/Backtracking/KQueens)
	- Branch and Bound: [BranchAndBound/Knapsack01](Min.Ak/Min.Ak/BranchAndBound/Knapsack01), [BranchAndBound/Tsp](Min.Ak/Min.Ak/BranchAndBound/Tsp)
	- Computability: [Computability/Ackermann](Min.Ak/Min.Ak/Computability/Ackermann)
	- Utilities: [Collections](Min.Ak/Min.Ak/Collections), [Model/Tsp](Min.Ak/Min.Ak/Model/Tsp)
- Publishing: [Min.Ak/Min.Ak/Properties/PublishProfiles](Min.Ak/Min.Ak/Properties/PublishProfiles)
- Build outputs (local): [Min.Ak/Min.Ak/artifacts](Min.Ak/Min.Ak/artifacts)

## Algorithms Implemented

- K-Queens (Backtracking)
	- Goal: Place K queens on an N×N board so none attack each other.
	- Method: Backtracking with row/column/diagonal constraints.
	- Types: `Min.Ak.Backtracking.KQueens.KQueens` with `Solve()` returning `List<KQueensSolution>`.

- 0/1 Knapsack (Branch and Bound)
	- Goal: Maximize total gain subject to a cost limit.
	- Method: Branch-and-Bound with item ordering by relative gain.
	- API: `Min.Ak.BranchAndBound.Knapsack01.Knapsack01Solver.Solve(maxCost, options)` where `options` is `List<K01BaBOption>`.

- Traveling Salesman Problem (Branch and Bound)
	- Goal: Compute tours minimizing total distance on a complete graph.
	- Method: Branch-and-Bound with matrix reductions and pruning.
	- API: `Min.Ak.BranchAndBound.Tsp.TspSolver.Solve(names, distanceMatrix)` using `Min.Ak.Model.Tsp.DistanceMatrix<T>`.

- Ackermann Function (Computability)
	- Goal: Compare recursive vs iterative (stack-based) implementations.
	- Types: `RecursiveAck.Ack(n, m)` and `IterativeAck.Ack(n, m)` under `Min.Ak.Computability.Ackermann`.

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

K01BabSolution? result = Knapsack01Solver.Solve(maxCost: 600, options:
[
    K01BaB.Option("A", gain: 201, cost: 191),
    K01BaB.Option("B", gain: 141, cost: 239),
    K01BaB.Option("C", gain:  48, cost: 148),
    K01BaB.Option("D", gain: 232, cost: 153),
    K01BaB.Option("E", gain:  50, cost:  66),
    K01BaB.Option("F", gain:  79, cost: 137),
    K01BaB.Option("G", gain:  38, cost: 249),
    K01BaB.Option("H", gain:  73, cost:  54),
]);
Console.WriteLine(result?.ToString() ?? "No solution found.");
```

TSP (Branch and Bound):

```csharp
using Min.Ak.BranchAndBound.Tsp;
using Min.Ak.Model.Tsp;

List<TspBabSolution<int>> results = TspSolver.Solve
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

