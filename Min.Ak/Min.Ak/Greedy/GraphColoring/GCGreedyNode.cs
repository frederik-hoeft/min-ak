namespace Min.Ak.Greedy.GraphColoring;

internal sealed record GCGreedyNode(string Name, int Index)
{
    public int Color { get; set; } = -1;
}
