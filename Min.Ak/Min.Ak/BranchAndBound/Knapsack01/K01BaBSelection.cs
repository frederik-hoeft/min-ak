using Min.Ak.Model.K01;
using System.Numerics;

namespace Min.Ak.BranchAndBound.Knapsack01;

internal sealed record K01BaBSelection<T>(Knapsack01Option<T> Option, bool Negate) where T : unmanaged, INumber<T>;
