using System.Numerics;

namespace Min.Ak.DynamicProgramming.KnapsackN;

internal static class KnDpOption
{
    public static KnapsackNOption<T> Of<T>(string name, T gain, int cost) where T : unmanaged, INumber<T> =>
        new(name, gain, cost);
}

internal sealed record KnapsackNOption<T>(string Name, T Gain, int Cost) where T : unmanaged, INumber<T>
{
    public override int GetHashCode() => Name.GetHashCode();
}