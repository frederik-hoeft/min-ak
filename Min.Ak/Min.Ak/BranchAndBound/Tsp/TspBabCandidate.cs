using Min.Ak.Collections;
using Min.Ak.Model.Tsp;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Min.Ak.BranchAndBound.Tsp;

[DebuggerDisplay("{ToString(),nq}")]
internal sealed class TspBabCandidate<T> where T : unmanaged, INumber<T>
{
    private readonly DistanceMatrix<T> _sourceMatrix;
    private readonly DistanceMatrix<T> _reducedMatrix;

    public T LowerBound { get; }

    public T TotalCost { get; }

    public bool IsClosed { get; }

    public string CurrentName { get; }

    public OrderedBijectiveMap<string, int> VisitedNames { get; }

    public TspBab<T> Bab { get; }

    public TspBabCandidate(TspBab<T> bab, string currentName, OrderedBijectiveMap<string, int> visitedNames, DistanceMatrix<T> matrix, T currentBound, T totalCost, bool isClosed = false)
    {
        Bab = bab;
        CurrentName = currentName;
        VisitedNames = visitedNames;
        IsClosed = isClosed;
        _sourceMatrix = matrix;
        _reducedMatrix = matrix.Clone();
        // current bound + reduction cost
        LowerBound = currentBound + Reduce(ref _reducedMatrix);
        TotalCost = totalCost;
    }

    private static T Reduce(ref DistanceMatrix<T> matrix)
    {
        T cost = T.Zero;
        for (int row = 0; row < matrix.Size; ++row)
        {
            T rowMin = matrix.RowMin(row);
            matrix.RowSubtract(row, rowMin);
            cost += rowMin;
        }
        for (int col = 0; col < matrix.Size; ++col)
        {
            T columnMin = matrix.ColumnMin(col);
            matrix.ColumnSubtract(col, columnMin);
            cost += columnMin;
        }
        return cost;
    }

    public IEnumerable<TspBabCandidate<T>> EnumerateChildren()
    {
        if (IsClosed)
        {
            yield break;
        }

        int currentRow = Bab.NameIndexMap.GetByKey(CurrentName);

        if (VisitedNames.Count == _sourceMatrix.Size)
        {
            T closeCost = _reducedMatrix[currentRow, 0];
            if (closeCost == _reducedMatrix.Infinity)
            {
                // no valid closing path
                yield break;
            }

            DistanceMatrix<T> closeMatrix = _reducedMatrix.Clone();
            closeMatrix.SetRow(currentRow, closeMatrix.Infinity);
            closeMatrix.SetColumn(0, closeMatrix.Infinity);
            closeMatrix[row: 0, column: currentRow] = closeMatrix.Infinity;

            string startName = Bab.NameIndexMap.GetByValue(0);

            yield return new TspBabCandidate<T>(
                Bab,
                startName,
                VisitedNames,
                closeMatrix,
                LowerBound + closeCost,
                TotalCost + Bab.DistanceMatrix[currentRow, 0],
                isClosed: true);
            yield break;
        }

        for (int col = 0; col < _sourceMatrix.Size; ++col)
        {
            if (currentRow == col || VisitedNames.ContainsValue(col))
            {
                continue;
            }
            T childCost = _reducedMatrix[currentRow, col];
            DistanceMatrix<T> childMatrix = _reducedMatrix.Clone();
            childMatrix.SetRow(currentRow, childMatrix.Infinity);
            childMatrix.SetColumn(col, childMatrix.Infinity);
            // prevent returning to the starting point too early
            // col / row swap is intentional
            childMatrix[row: col, column: currentRow] = childMatrix.Infinity;
            string childName = Bab.NameIndexMap.GetByValue(col);
            OrderedBijectiveMap<string, int> childVisitedNames = VisitedNames.Clone();
            childVisitedNames.Add(childName, col);
            T childLowerBound = LowerBound + childCost;
            T childTotalCost = TotalCost + Bab.DistanceMatrix[currentRow, col];
            yield return new TspBabCandidate<T>(Bab, childName, childVisitedNames, childMatrix, childLowerBound, childTotalCost);
        }
    }

    public TspBabSolution<T> ToSolution() => new(this);

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("{ TotalCost: ").Append(TotalCost).Append(", LowerBound: ").Append(LowerBound).Append(", CurrentName: ").Append(CurrentName).Append(", Path: ");
        int previousIndex = -1;
        foreach ((string name, int index) in VisitedNames.Elements)
        {
            if (previousIndex != -1)
            {
                sb.Append(" -(").Append(Bab.DistanceMatrix[previousIndex, index]).Append(")-> ");
            }
            sb.Append(name);
            previousIndex = index;
        }
        if (previousIndex != -1 && IsClosed)
        {
            sb.Append(" -(").Append(Bab.DistanceMatrix[previousIndex, 0]).Append(")-> ").Append(Bab.NameIndexMap.GetByValue(0));
        }
        sb.Append(" }");
        return sb.ToString();
    }
}
