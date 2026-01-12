using System.Numerics;

namespace Min.Ak.DynamicProgramming.KnapsackN;

internal sealed record KnDpSelection<T>(KnapsackNOption<T> Option, int Quantity) where T : unmanaged, INumber<T>;
