namespace Min.Ak.BranchAndBound.Knapsack01;

internal sealed record K01BaBOption(string Name, float Gain, float Cost)
{
    public float RelativeGain => Gain / Cost;
}
