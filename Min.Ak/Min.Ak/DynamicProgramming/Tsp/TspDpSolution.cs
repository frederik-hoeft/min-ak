using Min.Ak.Collections;
using Min.Ak.Model.Tsp;
using System.Numerics;
using System.Text;

namespace Min.Ak.DynamicProgramming.Tsp;

internal sealed class TspDpSolution<T>(TspDp<T> dp, T totalCost, List<int> path) where T : unmanaged, INumber<T>
{
    public T TotalCost => totalCost;

    public IEnumerable<string> Path
    {
        get
        {
            foreach (int node in path)
            {
                yield return dp.NameIndexMap.GetByValue(node);
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("{ TotalCost: ").Append(TotalCost).Append(", Path: ");
        int previousNode = -1;
        foreach (int node in path)
        {
            if (previousNode != -1)
            {
                sb.Append(" -(").Append(dp.DistanceMatrix[previousNode, node]).Append(")-> ");
            }
            sb.Append(dp.NameIndexMap.GetByValue(node));
            previousNode = node;
        }
        sb.Append(" }");
        return sb.ToString();
    }
}