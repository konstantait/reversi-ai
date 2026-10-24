using System.Collections.Generic;

namespace Reversi.Algebra
{
    public interface ISparseVector
    {
        int Dimension { get; }

        int NonZeroCount { get; }

        double Get(int index);

        void Set(int index, double value);

        IEnumerable<SparseEntry> NonZeroEntries();

        ISparseVector Add(ISparseVector other);

        ISparseVector Subtract(ISparseVector other);

        ISparseVector Multiply(double k);

        double Norm();

        double Dot(ISparseVector other);

        bool ApproximatelyEquals(ISparseVector other, double tolerance);
    }
}
