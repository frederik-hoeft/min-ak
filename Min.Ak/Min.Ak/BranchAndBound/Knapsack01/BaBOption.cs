namespace Min.Ak.BranchAndBound.Knapsack01;

internal sealed record BaBOption(string Name, float Gain, float Cost)
{
    public float RelativeGain => Gain / Cost;
}
