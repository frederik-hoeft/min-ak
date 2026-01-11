using System.Numerics;
using System.Text;

namespace Min.Ak.BranchAndBound.Tsp;

internal readonly record struct TspBabSolution<T> where T : unmanaged, INumber<T>
{
    private readonly TspBabCandidate<T> _candidate;

    public TspBabSolution(TspBabCandidate<T> candidate)
    {
        _candidate = candidate;
    }

    public T TotalCost => _candidate.TotalCost;

    public IEnumerable<string> Path
    {
        get
        {
            foreach ((string name, _) in _candidate.VisitedNames.Elements)
            {
                yield return name;
            }
            if (_candidate.IsClosed)
            {
                yield return _candidate.Bab.NameIndexMap.GetByValue(0);
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("{ TotalCost: ").Append(TotalCost).Append(", Path: ");
        int previousIndex = -1;
        foreach ((string name, int index) in _candidate.VisitedNames.Elements)
        {
            if (previousIndex != -1)
            {
                sb.Append(" -(").Append(_candidate.Bab.DistanceMatrix[previousIndex, index]).Append(")-> ");
            }
            sb.Append(name);
            previousIndex = index;
        }
        if (previousIndex != -1 && _candidate.IsClosed)
        {
            sb.Append(" -(").Append(_candidate.Bab.DistanceMatrix[previousIndex, 0]).Append(")-> ").Append(_candidate.Bab.NameIndexMap.GetByValue(0));
        }
        sb.Append(" }");
        return sb.ToString();
    }
}