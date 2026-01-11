using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Min.Ak.Collections;

[DebuggerDisplay("{ToString(),nq}")]
internal sealed class OrderedBijectiveMap<TKey, TValue>(int capacity = 4) where TKey : notnull where TValue : notnull
{
    private readonly OrderedDictionary<TKey, TValue> _forward = new(capacity);
    private readonly OrderedDictionary<TValue, TKey> _backward = new(capacity);

    public void Add(TKey key, TValue value)
    {
        if (_forward.ContainsKey(key))
        {
            throw new ArgumentException("Key already exists in the map.", nameof(key));
        }
        if (_backward.ContainsKey(value))
        {
            throw new ArgumentException("Value already exists in the map.", nameof(value));
        }
        _forward.Add(key, value);
        _backward.Add(value, key);
    }

    public bool TryGetByKey(TKey key, [NotNullWhen(true)] out TValue? value) => _forward.TryGetValue(key, out value);

    public bool TryGetByValue(TValue value, [NotNullWhen(true)] out TKey? key) => _backward.TryGetValue(value, out key);

    public TValue GetByKey(TKey key) => _forward[key];

    public TKey GetByValue(TValue value) => _backward[value];

    public bool ContainsKey(TKey key) => _forward.ContainsKey(key);

    public bool ContainsValue(TValue value) => _backward.ContainsKey(value);

    public int Count => _forward.Count;

    public IEnumerable<KeyValuePair<TKey, TValue>> Elements => _forward;

    public void Clear()
    {
        _forward.Clear();
        _backward.Clear();
    }

    public OrderedBijectiveMap<TKey, TValue> Clone()
    {
        OrderedBijectiveMap<TKey, TValue> clone = new(Count);
        foreach ((TKey key, TValue value) in _forward)
        {
            clone.Add(key, value);
        }
        return clone;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append('{');
        bool first = true;
        foreach ((TKey key, TValue value) in _forward)
        {
            if (!first)
            {
                sb.Append(", ");
            }
            sb.Append(key).Append(" <-> ").Append(value);
            first = false;
        }
        sb.Append('}');
        return sb.ToString();
    }
}
