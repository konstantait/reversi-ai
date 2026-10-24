using System;
using System.Collections.Generic;

namespace Reversi.Algebra
{
    public sealed class SparseVector : ISparseVector
    {
        // Storage is reserved for the linked-list implementation.
        private int dimension;
        private Node head;

        private sealed class Node
        {
            public int Index;
            public double Value;
            public Node Next;
        }

        public SparseVector()
        {
            throw new NotImplementedException();
        }

        public SparseVector(int dimension)
        {
            throw new NotImplementedException();
        }

        public SparseVector(double[] values)
        {
            throw new NotImplementedException();
        }

        public SparseVector(SparseVector other)
        {
            throw new NotImplementedException();
        }

        public int Dimension
        {
            get { throw new NotImplementedException(); }
        }

        public int NonZeroCount
        {
            get { throw new NotImplementedException(); }
        }

        public double Get(int index)
        {
            throw new NotImplementedException();
        }

        public void Set(int index, double value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SparseEntry> NonZeroEntries()
        {
            throw new NotImplementedException();
        }

        public ISparseVector Add(ISparseVector other)
        {
            throw new NotImplementedException();
        }

        public ISparseVector Subtract(ISparseVector other)
        {
            throw new NotImplementedException();
        }

        public ISparseVector Multiply(double k)
        {
            throw new NotImplementedException();
        }

        public double Norm()
        {
            throw new NotImplementedException();
        }

        public double Dot(ISparseVector other)
        {
            throw new NotImplementedException();
        }

        public bool ApproximatelyEquals(ISparseVector other, double tolerance)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
