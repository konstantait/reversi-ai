using System;
using System.Collections.Generic;

namespace Reversi.Algebra.Tests
{
    // Supplies the exact traversal required by a test, including invalid entries.
    internal sealed class ScriptedSparseVector : ISparseVector
    {
        private readonly int dimension;
        private readonly SparseEntry[] entries;

        public ScriptedSparseVector(int dimension, SparseEntry[] entries)
        {
            this.dimension = dimension;
            this.entries = (SparseEntry[])entries.Clone();
        }

        public int Dimension
        {
            get { return dimension; }
        }

        public int NonZeroCount
        {
            get { return entries.Length; }
        }

        public IEnumerable<SparseEntry> NonZeroEntries()
        {
            foreach (SparseEntry entry in entries)
            {
                yield return entry;
            }
        }

        public double Get(int index)
        {
            throw new NotSupportedException();
        }

        public void Set(int index, double value)
        {
            throw new NotSupportedException();
        }

        public ISparseVector Add(ISparseVector other)
        {
            throw new NotSupportedException();
        }

        public ISparseVector Subtract(ISparseVector other)
        {
            throw new NotSupportedException();
        }

        public ISparseVector Multiply(double k)
        {
            throw new NotSupportedException();
        }

        public double Norm()
        {
            throw new NotSupportedException();
        }

        public double Dot(ISparseVector other)
        {
            throw new NotSupportedException();
        }

        public bool ApproximatelyEquals(ISparseVector other, double tolerance)
        {
            throw new NotSupportedException();
        }
    }
}
