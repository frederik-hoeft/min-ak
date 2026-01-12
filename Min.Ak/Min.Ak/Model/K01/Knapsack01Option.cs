using System.Numerics;

namespace Min.Ak.Model.K01;

internal static class Knapsack01Option
{
    public static Knapsack01Option<T> Of<T>(string name, T gain, T cost) where T : unmanaged, INumber<T> => new(name, gain, cost);
}

internal sealed record Knapsack01Option<T>(string Name, T Gain, T Cost) where T : unmanaged, INumber<T>
{
    public T RelativeGain => Gain / Cost;
}
