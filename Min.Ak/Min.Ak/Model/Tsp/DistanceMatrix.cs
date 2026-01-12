using Min.Ak.Greedy.GraphColoring;
using System.Numerics;
using System.Text;

namespace Min.Ak.Model.Tsp;

internal static class DistanceMatrix
{
    public static DistanceMatrix<T> Create<T>(int size, T infinityValue) where T : unmanaged, INumber<T>
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(size, 1, nameof(size));
        T[][] matrix = new T[size][];
        for (int i = 0; i < size; ++i)
        {
            matrix[i] = new T[size];
        }
        return new DistanceMatrix<T>(matrix, infinityValue);
    }

    public static DistanceMatrix<T> Create<T>(int size) where T : unmanaged, IFloatingPointIeee754<T> =>
        Create(size, T.PositiveInfinity);

    public static DistanceMatrix<T> Create<T>(T[][] data, T infinityValue) where T : unmanaged, INumber<T>
    {
        DistanceMatrix<T> matrix = CreateUnchecked(data, infinityValue);
        for (int i = 0; i < matrix.Size; ++i)
        {
            T diagonalValue = matrix[i,i];
            if (diagonalValue != T.Zero && diagonalValue != infinityValue)
            {
                throw new ArgumentException("Diagonal elements must be zero or infinity.", nameof(data));
            }
        }
        return matrix;
    }

    public static DistanceMatrix<T> Create<T>(T[][] data) where T : unmanaged, IFloatingPointIeee754<T> =>
        Create(data, T.PositiveInfinity);

    public static DistanceMatrix<T> CreateUnchecked<T>(T[][] data, T infinityValue) where T : unmanaged, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        ArgumentOutOfRangeException.ThrowIfLessThan(data.Length, 1, nameof(data));
        // ensure square matrix
        int size = data.Length;
        for (int i = 0; i < size; ++i)
        {
            if (data[i].Length != size)
            {
                throw new ArgumentException("Input data must be a square matrix.", nameof(data));
            }
        }
        // not checking the values
        return new DistanceMatrix<T>(data, infinityValue);
    }
}

internal readonly struct DistanceMatrix<T>(T[][] matrix, T infinityValue) where T : unmanaged, INumber<T>
{
    public ref T this[int row, int column] => ref matrix[row][column];

    public T[] this[int row] => matrix[row];

    public T Infinity => infinityValue;

    public int Size => matrix.Length;

    public T RowMin(int row)
    {
        T min = infinityValue;
        T[] matrixRow = matrix[row];
        for (int i = 0; i < matrixRow.Length; ++i)
        {
            T value = matrixRow[i];
            if (value != infinityValue && (value < min || min == infinityValue))
            {
                min = value;
            }
        }
        if (min == infinityValue)
        {
            return T.Zero;
        }
        return min;
    }

    public T ColumnMin(int column)
    {
        T min = infinityValue;
        for (int i = 0; i < Size; ++i)
        {
            T value = matrix[i][column];
            if (value != infinityValue && (value < min || min == infinityValue))
            {
                min = value;
            }
        }
        if (min == infinityValue)
        {
            return T.Zero;
        }
        return min;
    }

    public DistanceMatrix<T> Clone()
    {
        T[][] newMatrix = new T[Size][];
        for (int i = 0; i < Size; ++i)
        {
            newMatrix[i] = new T[Size];
            Array.Copy(matrix[i], newMatrix[i], Size);
        }
        return new DistanceMatrix<T>(newMatrix, infinityValue);
    }

    public void SetRow(int row, T value)
    {
        T[] matrixRow = matrix[row];
        for (int i = 0; i < matrixRow.Length; ++i)
        {
            ref T current = ref matrixRow[i];
            if (current != infinityValue)
            {
                current = value;
            }
        }
    }

    public void SetColumn(int column, T value)
    {
        for (int i = 0; i < Size; ++i)
        {
            ref T current = ref matrix[i][column];
            if (current != infinityValue)
            {
                current = value;
            }
        }
    }

    public void RowAdd(int row, T value)
    {
        T[] matrixRow = matrix[row];
        for (int i = 0; i < matrixRow.Length; ++i)
        {
            ref T current = ref matrixRow[i];
            if (current != infinityValue)
            {
                current += value;
            }
        }
    }

    public void ColumnAdd(int column, T value)
    {
        for (int i = 0; i < Size; ++i)
        {
            ref T current = ref matrix[i][column];
            if (current != infinityValue)
            {
                current += value;
            }
        }
    }

    public void RowSubtract(int row, T value) => RowAdd(row, -value);

    public void ColumnSubtract(int column, T value) => ColumnAdd(column, -value);

    public override string ToString()
    {
        StringBuilder sb = new();
        int n = Size;

        // 1) compute column width
        int cellWidth = 1; // minimum
        for (int i = 0; i < n; ++i)
        {
            cellWidth = Math.Max(cellWidth, i.ToString().Length);
        }

        for (int r = 0; r < n; ++r)
        {
            for (int c = 0; c < n; ++c)
            {
                T v = matrix[r][c];
                int len = (v == infinityValue)
                    ? 1 // 'infiniy'
                    : v.ToString()!.Length;

                cellWidth = Math.Max(cellWidth, len);
            }
        }

        // add one space padding per column
        cellWidth += 1;

        // 2) header
        sb.Append(' ', cellWidth + 1);
        for (int i = 0; i < n; ++i)
        {
            sb.Append(i.ToString().PadLeft(cellWidth));
        }
        sb.AppendLine();

        // 3) top border
        sb.Append(' ', cellWidth)
          .Append('+')
          .Append('-', (cellWidth * n) + 1)
          .Append('+')
          .AppendLine();

        // 4) rows
        for (int row = 0; row < n; ++row)
        {
            sb.Append(row.ToString().PadLeft(cellWidth - 1))
              .Append(" |");

            for (int col = 0; col < n; ++col)
            {
                T value = matrix[row][col];
                string s = (value == infinityValue)
                    ? "\u221e"
                    : value.ToString()!;

                sb.Append(s.PadLeft(cellWidth));
            }

            sb.Append(" |").AppendLine();
        }

        // 5) bottom border
        sb.Append(' ', cellWidth)
          .Append('+')
          .Append('-', (cellWidth * n) + 1)
          .Append('+')
          .AppendLine();

        return sb.ToString();
    }

#if DEBUG
    public string InspectionString => ToString();
#endif
}