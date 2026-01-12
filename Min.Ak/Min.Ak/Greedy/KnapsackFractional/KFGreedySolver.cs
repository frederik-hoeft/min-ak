using Min.Ak.Model.K01;
using System.Numerics;

namespace Min.Ak.Greedy.KnapsackFractional;

internal static class KFGreedySolver
{
    public static KFGreedySolution<T> Solve<T>(T maxCost, IReadOnlyList<Knapsack01Option<T>> options) where T : unmanaged, IFloatingPoint<T>
    {
        List<KFGreedySelection<T>> selections = [];
        T totalCost = T.Zero;
        foreach (Knapsack01Option<T> option in options.OrderByDescending(o => o.RelativeGain))
        {
            if (totalCost + option.Cost <= maxCost)
            {
                selections.Add(new KFGreedySelection<T>(option, T.One));
                totalCost += option.Cost;
            }
            else
            {
                selections.Add(new KFGreedySelection<T>(option, (maxCost - totalCost) / option.Cost));
                break;
            }
        }
        return new KFGreedySolution<T>(selections);
    }
}