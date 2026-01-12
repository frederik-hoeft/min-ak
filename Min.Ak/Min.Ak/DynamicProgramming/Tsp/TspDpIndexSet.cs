using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;

namespace Min.Ak.DynamicProgramming.Tsp;

[DebuggerDisplay("{ToString(),nq}")]
internal readonly struct TspDpIndexSet(ulong bitMap) : IEquatable<TspDpIndexSet>
{
    private readonly ulong _bitMap = bitMap;

    public bool IsEmpty => _bitMap == 0uL;

    public int Count => BitOperations.PopCount(_bitMap);

    public bool IsSet(int index) => (_bitMap & (1uL << ValidateIndex(index))) != 0;

    public bool IsSubsetOf(TspDpIndexSet other) => (_bitMap & other._bitMap) == _bitMap;

    public TspDpIndexSet Add(int index) => new(_bitMap | (1uL << ValidateIndex(index)));

    public TspDpIndexSet Remove(int index) => new(_bitMap & ~(1uL << ValidateIndex(index)));

    public static TspDpIndexSet Empty => new(0uL);

    public static TspDpIndexSet Full(int size)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(size, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(size, 63);
        return new((1uL << size) - 1);
    }

    private static int ValidateIndex(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 63);
        return index;
    }

    public override int GetHashCode() => _bitMap.GetHashCode();

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is TspDpIndexSet other && _bitMap == other._bitMap;

    public bool Equals(TspDpIndexSet other) => _bitMap == other._bitMap;

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
