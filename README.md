# MIN-AK — Algorithms & Complexity (C#)

A compact, educational collection of algorithm implementations written in C#/.NET. The repository is organized by technique and topic (Backtracking, Branch and Bound, Computability) with small, focused examples rather than a single runnable app.

## Project Overview

- Target Framework: .NET 10 (net10.0)
- Output: Console app (Program prints "Hello World!" by default)
- Style: Minimal, internal classes inside namespaces per topic

See the entry point in [Min.Ak/Min.Ak/Program.cs](Min.Ak/Min.Ak/Program.cs) and project file in [Min.Ak/Min.Ak/Min.Ak.csproj](Min.Ak/Min.Ak/Min.Ak.csproj).

## Repository Structure

- Backtracking
	- K-Queens: [Min.Ak/Min.Ak/Backtracking/KQueens](Min.Ak/Min.Ak/Backtracking/KQueens)
		- Core solver: [KQueens.cs](Min.Ak/Min.Ak/Backtracking/KQueens/KQueens.cs)
		- Board/model: [KQueensBoard.cs](Min.Ak/Min.Ak/Backtracking/KQueens/KQueensBoard.cs)
		- Solution snapshot: [KQueensSolution.cs](Min.Ak/Min.Ak/Backtracking/KQueens/KQueensSolution.cs)
- Branch and Bound
	- 0/1 Knapsack: [Min.Ak/Min.Ak/BranchAndBound/Knapsack01](Min.Ak/Min.Ak/BranchAndBound/Knapsack01)
		- API surface: [Knapsack01Solver.cs](Min.Ak/Min.Ak/BranchAndBound/Knapsack01/Knapsack01Solver.cs)
		- Option model: [BaBOption.cs](Min.Ak/Min.Ak/BranchAndBound/Knapsack01/BaBOption.cs)
		- Algorithm internals: [BaB.cs](Min.Ak/Min.Ak/BranchAndBound/Knapsack01/BaB.cs), [BabCandidate.cs](Min.Ak/Min.Ak/BranchAndBound/Knapsack01/BabCandidate.cs), [BaBSelection.cs](Min.Ak/Min.Ak/BranchAndBound/Knapsack01/BaBSelection.cs), [BabSolution.cs](Min.Ak/Min.Ak/BranchAndBound/Knapsack01/BabSolution.cs), [OrderedDescList.cs](Min.Ak/Min.Ak/BranchAndBound/Knapsack01/OrderedDescList.cs)
- Computability
	- Ackermann Function: [Min.Ak/Min.Ak/Computability/Ackermann](Min.Ak/Min.Ak/Computability/Ackermann)
		- Recursive: [RecursiveAck.cs](Min.Ak/Min.Ak/Computability/Ackermann/RecursiveAck.cs)
		- Iterative (stack simulation): [IterativeAck.cs](Min.Ak/Min.Ak/Computability/Ackermann/IterativeAck.cs)

## Algorithms Implemented

- K-Queens (Backtracking)
	- Goal: Place K queens on an N×N board so none attack each other.
	- Method: Backtracking with incremental constraints across rows, columns, and diagonals.
	- Entry type: Min.Ak.Backtracking.KQueens.KQueens (internal sealed record with Solve() returning a list of solutions).

- 0/1 Knapsack (Branch and Bound)
	- Goal: Maximize total gain without exceeding a cost (capacity) constraint.
	- Method: Branch and Bound with item ordering by relative gain.
	- Entry API: Min.Ak.BranchAndBound.Knapsack01.Knapsack01Solver.Solve(maxCost, options) where options is a list of BaBOption(Name, Gain, Cost).

- Ackermann Function (Computability)
	- Goal: Compare recursive vs iterative (stack-based) implementations of the Ackermann function.
	- Types: RecursiveAck.Ack(n, m) and IterativeAck.Ack(n, m) under Min.Ak.Computability.Ackermann.

## Build & Run

Prerequisites: .NET 10 SDK (or latest preview) to target net10.0.

Basic commands:

```bash
dotnet build Min.Ak/Min.Ak/Min.Ak.csproj
dotnet run --project Min.Ak/Min.Ak/Min.Ak.csproj
```

The default Program.cs just writes "Hello World!". To experiment, add calls to the algorithms in Program.cs.

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

BabSolution? result = Knapsack01Solver.Solve(maxCost: 600, options:
[
    BaB.Option("A", gain: 201, cost: 191),
    BaB.Option("B", gain: 141, cost: 239),
    BaB.Option("C", gain: 48, cost: 148),
    BaB.Option("D", gain: 232, cost: 153),
    BaB.Option("E", gain: 50, cost: 66),
    BaB.Option("F", gain: 79, cost: 137),
    BaB.Option("G", gain: 38, cost: 249),
    BaB.Option("H", gain: 73, cost: 54),
]);
Console.WriteLine(result?.ToString() ?? "No solution found.");
```

Ackermann (recursive vs iterative):

```csharp
using Min.Ak.Computability.Ackermann;

Console.WriteLine(RecursiveAck.Ack(2, 3));
Console.WriteLine(IterativeAck.Ack(2, 3));
```

## Notes

- The code favors clarity over UI or I/O — adjust Program.cs to run specific demos.
- AOT publishing is enabled in the project file; you can try native publishing (SDK and platform permitting):

```bash
dotnet publish Min.Ak/Min.Ak/Min.Ak.csproj -c Release -r win-x64
```

## License

Educational use. No explicit license included.

