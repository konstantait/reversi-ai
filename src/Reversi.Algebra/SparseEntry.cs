namespace Reversi.Algebra
{
    public readonly struct SparseEntry
    {
        public SparseEntry(int index, double value)
        {
            Index = index;
            Value = value;
        }

        public int Index { get; }

        public double Value { get; }
    }
}
