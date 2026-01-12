using Min.Ak.Model.K01;
using System.Numerics;

namespace Min.Ak.Greedy.Knapsack01;

internal static class K01GreedySolver
{
    public static K01GreedyApproximation<T> Approximate<T>(T maxCost, IReadOnlyList<Knapsack01Option<T>> options) where T : unmanaged, INumber<T>
    {
        List<Knapsack01Option<T>> selections = [];
        T totalCost = T.Zero;
        foreach (Knapsack01Option<T> option in options.OrderByDescending(o => o.RelativeGain))
        {
            if (totalCost + option.Cost <= maxCost)
            {
                selections.Add(option);
                totalCost += option.Cost;
            }
        }
        return new K01GreedyApproximation<T>(selections);
    }
}