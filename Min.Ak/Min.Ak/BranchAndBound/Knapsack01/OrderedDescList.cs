using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Min.Ak.BranchAndBound.Knapsack01;

[DebuggerDisplay("{ToString(),nq}")]
internal sealed class OrderedDescList<TValue, TPriority>(Func<TValue, TPriority> prioritySelector) where TPriority : unmanaged, INumber<TPriority>
{
    private readonly List<Node> _nodes = [];

    public void Enqueue(TValue value)
    {
        TPriority priority = prioritySelector(value);
        Node node = new(value, priority);
        int index = SearchIndex(priority);
        _nodes.Insert(index, node);
    }

    public TValue? Dequeue()
    {
        if (_nodes.Count == 0)
        {
            return default;
        }
        TValue value = _nodes[0].Value;
        _nodes.RemoveAt(0);
        return value;
    }

    public void PruneBelow(TPriority priority)
    {
        for (int index = SearchIndex(priority); index < _nodes.Count;)
        {
            _nodes.RemoveAt(index);
        }
    }

    /// <summary>
    /// Returns the insert index for the given priority in the ordered (desc) list.
    /// </summary>
    private int SearchIndex(TPriority priority)
    {
        int lower = 0;
        int upper = _nodes.Count; // exclusive

        while (lower < upper)
        {
            int pivot = lower + ((upper - lower) / 2);
            TPriority pivotPriority = _nodes[pivot].Priority;

            // Descending: higher priorities come first.
            // If our priority is greater, we must go left (smaller index).
            if (priority > pivotPriority)
            {
                upper = pivot;
            }
            else
            {
                // priority <= pivotPriority: go right (insert after equals -> stable)
                lower = pivot + 1;
            }
        }

        return lower;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append('[');
        bool isFirst = true;
        foreach (Node node in _nodes)
        {
            if (isFirst)
            {
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine(",");
            }
            sb.Append(' ', 4);
            sb.Append('(').Append(node.Priority).Append(": ").Append(node.Value).Append(')');
            isFirst = false;
        }
        sb.AppendLine();
        sb.Append(']');
        return sb.ToString();
    }

#if DEBUG
    public string InspectionString => ToString();
#endif

    private readonly record struct Node(TValue Value, TPriority Priority);
}
