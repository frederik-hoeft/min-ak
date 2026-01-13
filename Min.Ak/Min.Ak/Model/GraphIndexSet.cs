using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;

namespace Min.Ak.Model;

[DebuggerDisplay("{ToString(),nq}")]
internal readonly struct GraphIndexSet(ulong bitMap) : IEquatable<GraphIndexSet>
{
    private readonly ulong _bitMap = bitMap;

    public bool IsEmpty => _bitMap == 0uL;

    public int Count => BitOperations.PopCount(_bitMap);

    public static int Capacity => 64;

    public bool IsSet(int index) => (_bitMap & (1uL << ValidateIndex(index))) != 0;

    public bool IsSubsetOf(GraphIndexSet other) => (_bitMap & other._bitMap) == _bitMap;

    public GraphIndexSet Union(GraphIndexSet other) => new(_bitMap | other._bitMap);

    public GraphIndexSet Intersect(GraphIndexSet other) => new(_bitMap & other._bitMap);

    public GraphIndexSet Add(int index) => new(_bitMap | (1uL << ValidateIndex(index)));

    public GraphIndexSet Remove(int index) => new(_bitMap & ~(1uL << ValidateIndex(index)));

    public static GraphIndexSet Empty => new(0uL);

    public static GraphIndexSet Of(int a) => new(1uL << ValidateIndex(a));

    public static GraphIndexSet Of(int a, int b) => Of(a).Union(Of(b));

    public static GraphIndexSet Of(int a, int b, int c) => Of(a, b).Union(Of(c));

    public static GraphIndexSet Of(int a, int b, int c, int d) => Of(a, b).Union(Of(c, d));

    public static GraphIndexSet Of(params ReadOnlySpan<int> indices)
    {
        ulong bitMap = 0uL;
        foreach (int index in indices)
        {
            bitMap |= 1uL << ValidateIndex(index);
        }
        return new GraphIndexSet(bitMap);
    }

    public static GraphIndexSet Full(int size)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(size, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(size, 64);
        return new((1uL << size) - 1);
    }

    private static int ValidateIndex(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 63);
        return index;
    }

    public override int GetHashCode() => _bitMap.GetHashCode();

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is GraphIndexSet other && _bitMap == other._bitMap;

    public bool Equals(GraphIndexSet other) => _bitMap == other._bitMap;

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("{ ");
        for (int i = 0; i < 64; ++i)
        {
            if (IsSet(i))
            {
                sb.Append(i).Append(' ');
            }
        }
        sb.Append('}');
        return sb.ToString();
    }

    public Enumerator GetEnumerator() => new(_bitMap);

    internal struct Enumerator(ulong bitMap)
    {
        private bool _initialized;

        public readonly int Current => 63 - BitOperations.LeadingZeroCount(bitMap);

        public bool MoveNext()
        {
            if (bitMap == 0uL)
            {
                return false;
            }
            if (!_initialized)
            {
                _initialized = true;
                return true;
            }
            bitMap &= ~(1uL << Current);
            return bitMap != 0uL;
        }

        public readonly void Dispose() { }

        public readonly void Reset() => throw new NotSupportedException();
    }
}
