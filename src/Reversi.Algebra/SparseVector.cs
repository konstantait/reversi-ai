using System;
using System.Collections.Generic;
using System.Globalization;

namespace Reversi.Algebra
{
    public sealed class SparseVector : ISparseVector
    {
        private int dimension;
        private Node head;

        private sealed class Node
        {
            public int Index;
            public double Value;
            public Node Next;

            public Node(int index, double value)
            {
                Index = index;
                Value = value;
            }
        }

        public SparseVector()
        {
            dimension = 1;
        }

        public SparseVector(int dimension)
        {
            if (dimension <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dimension));
            }

            this.dimension = dimension;
        }

        public SparseVector(double[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Length == 0)
            {
                throw new ArgumentException("The array must not be empty.", nameof(values));
            }

            foreach (double value in values)
            {
                if (!IsFiniteValue(value))
                {
                    throw new ArgumentException("Coordinates must be finite.", nameof(values));
                }
            }

            dimension = values.Length;
            Node tail = null;
            for (int index = 0; index < values.Length; index++)
            {
                AppendNonZero(ref head, ref tail, index, values[index]);
            }
        }

        public SparseVector(SparseVector other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            dimension = other.dimension;
            Node tail = null;
            for (Node node = other.head; node != null; node = node.Next)
            {
                AppendNonZero(ref head, ref tail, node.Index, node.Value);
            }
        }

        public int Dimension
        {
            get { return dimension; }
        }

        public int NonZeroCount
        {
            get
            {
                int count = 0;
                for (Node node = head; node != null; node = node.Next)
                {
                    count++;
                }

                return count;
            }
        }

        public double Get(int index)
        {
            ValidateIndex(index);
            Node node = head;
            while (node != null && node.Index < index)
            {
                node = node.Next;
            }

            return node != null && node.Index == index ? node.Value : 0.0;
        }

        public void Set(int index, double value)
        {
            ValidateIndex(index);
            if (!IsFiniteValue(value))
            {
                throw new ArgumentException("The coordinate must be finite.", nameof(value));
            }

            Node previous = null;
            Node node = head;
            while (node != null && node.Index < index)
            {
                previous = node;
                node = node.Next;
            }

            if (node != null && node.Index == index)
            {
                if (value != 0.0)
                {
                    node.Value = value;
                }
                else if (previous == null)
                {
                    head = node.Next;
                }
                else
                {
                    previous.Next = node.Next;
                }
            }
            else if (value != 0.0)
            {
                Node inserted = new Node(index, value);
                inserted.Next = node;
                if (previous == null)
                {
                    head = inserted;
                }
                else
                {
                    previous.Next = inserted;
                }
            }
        }

        public IEnumerable<SparseEntry> NonZeroEntries()
        {
            for (Node node = head; node != null; node = node.Next)
            {
                yield return new SparseEntry(node.Index, node.Value);
            }
        }

        public ISparseVector Add(ISparseVector other)
        {
            return Combine(other, false);
        }

        public ISparseVector Subtract(ISparseVector other)
        {
            return Combine(other, true);
        }

        public ISparseVector Multiply(double k)
        {
            if (!IsFiniteValue(k))
            {
                throw new ArgumentException("The factor must be finite.", nameof(k));
            }

            if (k == 0.0)
            {
                return new SparseVector(dimension);
            }

            Node result = null;
            Node tail = null;
            for (Node node = head; node != null; node = node.Next)
            {
                double value = node.Value * k;
                if (!IsFiniteValue(value))
                {
                    throw new OverflowException();
                }

                AppendNonZero(ref result, ref tail, node.Index, value);
            }

            return new SparseVector(dimension, result);
        }

        public double Norm()
        {
            if (head == null)
            {
                return 0.0;
            }

            // Scale before squaring to preserve very large and very small norms.
            double scale = 0.0;
            for (Node node = head; node != null; node = node.Next)
            {
                scale = Math.Max(scale, Math.Abs(node.Value));
            }

            double sum = 0.0;
            for (Node node = head; node != null; node = node.Next)
            {
                double scaled = node.Value / scale;
                sum += scaled * scaled;
            }

            double result = scale * Math.Sqrt(sum);
            if (!IsFiniteValue(result))
            {
                throw new OverflowException();
            }

            return result;
        }

        public double Dot(ISparseVector other)
        {
            int otherDimension;
            Node right = ValidateOperand(other, out otherDimension);
            if (dimension != otherDimension)
            {
                throw new ArgumentException("Dimensions must match.", nameof(other));
            }

            Node left = head;
            double sum = 0.0;
            while (left != null && right != null)
            {
                if (left.Index < right.Index)
                {
                    left = left.Next;
                }
                else if (right.Index < left.Index)
                {
                    right = right.Next;
                }
                else
                {
                    // Check each product and partial sum in index order.
                    double product = left.Value * right.Value;
                    if (!IsFiniteValue(product))
                    {
                        throw new OverflowException();
                    }

                    sum += product;
                    if (!IsFiniteValue(sum))
                    {
                        throw new OverflowException();
                    }

                    left = left.Next;
                    right = right.Next;
                }
            }

            return sum;
        }

        public bool ApproximatelyEquals(ISparseVector other, double tolerance)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (tolerance < 0.0 || !IsFiniteValue(tolerance))
            {
                throw new ArgumentOutOfRangeException(nameof(tolerance));
            }

            int otherDimension;
            Node right = ValidateOperand(other, out otherDimension);
            if (dimension != otherDimension)
            {
                return false;
            }

            Node left = head;
            while (left != null || right != null)
            {
                double difference;
                if (right == null || (left != null && left.Index < right.Index))
                {
                    difference = left.Value;
                    left = left.Next;
                }
                else if (left == null || right.Index < left.Index)
                {
                    difference = -right.Value;
                    right = right.Next;
                }
                else
                {
                    difference = left.Value - right.Value;
                    left = left.Next;
                    right = right.Next;
                }

                if (!(Math.Abs(difference) <= tolerance))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            SparseVector other = obj as SparseVector;
            if (other == null || dimension != other.dimension)
            {
                return false;
            }

            Node left = head;
            Node right = other.head;
            while (left != null && right != null)
            {
                if (left.Index != right.Index ||
                    BitConverter.DoubleToInt64Bits(left.Value) !=
                    BitConverter.DoubleToInt64Bits(right.Value))
                {
                    return false;
                }

                left = left.Next;
                right = right.Next;
            }

            return left == null && right == null;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + dimension;
                for (Node node = head; node != null; node = node.Next)
                {
                    hash = hash * 31 + node.Index;
                    hash = hash * 31 + BitConverter.DoubleToInt64Bits(node.Value).GetHashCode();
                }

                return hash;
            }
        }

        public override string ToString()
        {
            string result = "(dim=" + dimension.ToString(CultureInfo.InvariantCulture);
            string separator = "; ";
            for (Node node = head; node != null; node = node.Next)
            {
                result += separator + "[" + node.Index.ToString(CultureInfo.InvariantCulture) +
                    "]=" + node.Value.ToString("R", CultureInfo.InvariantCulture);
                separator = ", ";
            }

            return result + ")";
        }

        private SparseVector(int dimension, Node head)
        {
            this.dimension = dimension;
            this.head = head;
        }

        private static bool IsFiniteValue(double value)
        {
            return !(double.IsNaN(value) || double.IsInfinity(value));
        }

        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= dimension)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        private static void AppendNonZero(ref Node first, ref Node tail, int index, double value)
        {
            if (value == 0.0)
            {
                return;
            }

            Node node = new Node(index, value);
            if (tail == null)
            {
                first = node;
            }
            else
            {
                tail.Next = node;
            }

            tail = node;
        }

        private static Node ValidateOperand(ISparseVector other, out int otherDimension)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            otherDimension = other.Dimension;
            if (otherDimension <= 0)
            {
                throw new ArgumentException("The dimension must be positive.", nameof(other));
            }

            // Validate the entire traversal once, retaining an independent chain.
            Node first = null;
            Node tail = null;
            int previousIndex = -1;
            foreach (SparseEntry entry in other.NonZeroEntries())
            {
                if (entry.Index < 0 || entry.Index >= otherDimension ||
                    !IsFiniteValue(entry.Value) || entry.Value == 0.0 ||
                    entry.Index <= previousIndex)
                {
                    throw new ArgumentException("Invalid sparse traversal.", nameof(other));
                }

                AppendNonZero(ref first, ref tail, entry.Index, entry.Value);
                previousIndex = entry.Index;
            }

            return first;
        }

        private SparseVector Combine(ISparseVector other, bool subtract)
        {
            int otherDimension;
            Node right = ValidateOperand(other, out otherDimension);
            if (dimension != otherDimension)
            {
                throw new ArgumentException("Dimensions must match.", nameof(other));
            }

            Node left = head;
            Node result = null;
            Node tail = null;
            while (left != null || right != null)
            {
                int index;
                double value;
                if (right == null || (left != null && left.Index < right.Index))
                {
                    index = left.Index;
                    value = left.Value;
                    left = left.Next;
                }
                else if (left == null || right.Index < left.Index)
                {
                    index = right.Index;
                    value = subtract ? -right.Value : right.Value;
                    right = right.Next;
                }
                else
                {
                    index = left.Index;
                    value = subtract ? left.Value - right.Value : left.Value + right.Value;
                    left = left.Next;
                    right = right.Next;
                }

                if (!IsFiniteValue(value))
                {
                    throw new OverflowException();
                }

                AppendNonZero(ref result, ref tail, index, value);
            }

            return new SparseVector(dimension, result);
        }
    }
}
