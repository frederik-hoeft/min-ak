using Min.Ak.Model.K01;
using System.Numerics;

namespace Min.Ak.Greedy.KnapsackFractional;

internal sealed record KFGreedySelection<T>(Knapsack01Option<T> Option, T Fraction) where T : unmanaged, IFloatingPoint<T>
{
    public T Gain => Option.Gain * Fraction;

    public T Cost => Option.Cost * Fraction;
}
