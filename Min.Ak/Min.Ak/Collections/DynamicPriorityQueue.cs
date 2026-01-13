using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Min.Ak.Collections;

[DebuggerDisplay("{ToString(),nq}")]
internal sealed class DynamicPriorityQueue<TValue, TPriority>(SortOrder order, Func<TValue, TPriority> prioritySelector)
    where TValue : notnull
    where TPriority : unmanaged, INumber<TPriority>
{
    private readonly OrderedDictionary<TValue, Node> _nodes = [];

    public void Enqueue(TValue value)
    {
        if (_nodes.ContainsKey(value))
        {
            throw new InvalidOperationException("Value is already present in the priority queue.");
        }
        TPriority priority = prioritySelector(value);
        Node node = new(value, priority);
        int index = SearchIndex(priority);
        _nodes.Insert(index, value, node);
    }

    public bool TryRefresh(TValue value)
    {
        if (!_nodes.Remove(value, out Node node))
        {
            return false;
        }
        Debug.Assert(node.Value.Equals(value));
        Enqueue(value);
        return true;
    }

    public bool Contains(TValue value) => _nodes.ContainsKey(value);

    public TValue? Dequeue()
    {
        if (_nodes.Count == 0)
        {
            return default;
        }
        TValue value = _nodes.GetAt(0).Value.Value;
        _nodes.RemoveAt(0);
        return value;
    }

    public void PruneWorseThan(TPriority priority)
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
        // binary search on a list is fine here, since it's not a linked list, but a growable array.
        // so indexing is O(1), and shifting elements on insert is O(n) anyway.

        int lower = 0;
        int upper = _nodes.Count; // exclusive

        while (lower < upper)
        {
            int pivot = lower + ((upper - lower) / 2);
            TPriority pivotPriority = _nodes.GetAt(pivot).Value.Priority;

            // Descending: higher priorities (may be numerically lower depending on order) come first.
            // If our priority is greater, we must go left (smaller index).
            if (HasHigherPriority(priority, pivotPriority))
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

    private bool HasHigherPriority(TPriority p1, TPriority p2) => order switch
    {
        SortOrder.Minimum => p1 < p2,
        SortOrder.Maximum => p1 > p2,
        _ => throw new InvalidOperationException("Unsupported sort order."),
    };

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append('[');
        bool isFirst = true;
        foreach ((_, Node node) in _nodes)
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
