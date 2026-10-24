# SparseVector paper tests

Test cases for the `SparseVector` class, written before the code. Each case gives a concrete input and the expected result. There are 133 cases, and each one is implemented by its own test method in `SparseVectorUnitTest`; the Test column gives the method name.

Case IDs: B01–B32 are the base cases, grouped by function; V01–V20 are boundary and contract cases, where a suffix names the variant (`.NaN`, `.PosInf`, `.NegInf` for the value of `q`, `.Add`, `.Dot` and so on for the operation, `.E` for an empty receiver); X01–X07 are additional cases. In total: B 32, V 94, X 7.

## Notation and common rules

`M = double.MaxValue`; `q` stands in turn for `NaN`, `+∞` and `−∞` (`double.NaN`, `double.PositiveInfinity`, `double.NegativeInfinity`), and each value has its own row. A vector `[a, b, c]` is created by the `double[]` constructor; an empty vector has a positive dimension and no nonzero coordinates. Pairs `(index, value)` describe a traversal. Numbers use a decimal point. In V08 and V18 the relative error of the norm is at most 1e-12; all other values, the structure and the exception types are checked exactly.

A dimension ≤ 0 throws `ArgumentOutOfRangeException`, an empty array throws `ArgumentException` and a null array throws `ArgumentNullException`. `Equals` compares only `SparseVector` objects; null and objects of another type give false. Equality is exact: the same dimension and bitwise-equal nonzero coordinates. `ToString` uses the invariant culture and the "R" format. When an exception is thrown, the operands keep their dimensions, entries and values, and no partial result is returned. A failed constructor creates no object.

In the V19 cases, `t` is a test implementation of `ISparseVector` that returns exactly the listed finite sequence; its dimension is 3 and does not change. Each case breaks only the listed traversal rule. The input sequence is validated completely, including the tail, even when the receiver is empty.

The test implementation `ScriptedSparseVector` returns the given sequence as is, without checks. Its `NonZeroCount` is the length of the sequence; `Get` and all other members except `Dimension`, `NonZeroCount` and `NonZeroEntries` throw `NotSupportedException`.

## Constructors and Get

| ID | Scenario | Input and action | Expected result | Test |
| --- | --- | --- | --- | --- |
| B01 | Zero coordinates | a = [0, 2, 0, -3]; read Dimension, NonZeroCount, Get(0..3) | Dimension = 4; NonZeroCount = 2; Get: 0, 2, 0, -3; traversal [(1,2),(3,-3)] | Constructor_ZeroCoordinates_OmitsZeroEntries |
| B02 | Zero dimension | new SparseVector(0) | ArgumentOutOfRangeException; no object is created | Constructor_ZeroDimension_ThrowsArgumentOutOfRangeException |
| B03 | Null array | new SparseVector((double[])null) | ArgumentNullException; no object is created | Constructor_NullArray_ThrowsArgumentNullException |
| B04 | Deep copy | a = [1, 0, 2]; b = new SparseVector(a); b.Set(0, 7); b.Set(2, 0) | a = [1, 0, 2], NonZeroCount = 2; b = [7, 0, 0], NonZeroCount = 1; both dimensions are 3 | Constructor_Copy_MutationsPreserveOriginal |
| V01 | Default | new SparseVector() | Dimension = 1, NonZeroCount = 0, Get(0) = 0 | Constructor_Default_ReturnsEmptyDimensionOne |
| V02.NaN | Non-finite coordinate | values = [1, double.NaN]; new SparseVector(values) | ArgumentException; no object is created | Constructor_NonFiniteNaN_ThrowsArgumentException |
| V02.PosInf | Non-finite coordinate | values = [1, double.PositiveInfinity]; new SparseVector(values) | ArgumentException; no object is created | Constructor_NonFinitePositiveInfinity_ThrowsArgumentException |
| V02.NegInf | Non-finite coordinate | values = [1, double.NegativeInfinity]; new SparseVector(values) | ArgumentException; no object is created | Constructor_NonFiniteNegativeInfinity_ThrowsArgumentException |
| V17.Ctor | Signed zeros | new SparseVector(new double[] { +0.0, -0.0 }) | No zero entries; Dimension = 2, NonZeroCount = 0; Get(0) = Get(1) = 0; empty traversal | Constructor_SignedZeros_OmitsZeroEntries |
| X01 | Empty array | values = new double[0]; new SparseVector(values) | ArgumentException; no object is created | Constructor_EmptyArray_ThrowsArgumentException |
| X02 | Get at the bound | a = [1, 2]; a.Get(2), index = Dimension | ArgumentOutOfRangeException; a = [1, 2], Dimension = 2, NonZeroCount = 2 unchanged | Get_IndexAtDimension_ThrowsArgumentOutOfRangeException |
| X05 | Negative index in Get | a = [1, 2]; a.Get(-1) | ArgumentOutOfRangeException; a = [1, 2], Dimension = 2, NonZeroCount = 2 unchanged | Get_NegativeIndex_ThrowsArgumentOutOfRangeException |
| X07 | Negative dimension | new SparseVector(-1) | ArgumentOutOfRangeException; no object is created | Constructor_NegativeDimension_ThrowsArgumentOutOfRangeException |

## Set

| ID | Scenario | Input and action | Expected result | Test |
| --- | --- | --- | --- | --- |
| B05 | Entry removal | a = [1, 2]; a.Set(0, 0) | a = [0, 2]; Dimension = 2; NonZeroCount = 1; traversal [(1,2)] | Set_Zero_RemovesEntry |
| B06 | Index out of range | a = [1, 2]; a.Set(2, 7) | ArgumentOutOfRangeException; a = [1, 2], Dimension = 2, NonZeroCount = 2 unchanged | Set_IndexAtDimension_ThrowsArgumentOutOfRangeException |
| V03.NaN | Non-finite coordinate | a = [2]; a.Set(0, double.NaN) | ArgumentException; state [2] unchanged; Dimension = 1, NonZeroCount = 1 | Set_NonFiniteNaN_ThrowsArgumentException |
| V03.PosInf | Non-finite coordinate | a = [2]; a.Set(0, double.PositiveInfinity) | ArgumentException; state [2] unchanged; Dimension = 1, NonZeroCount = 1 | Set_NonFinitePositiveInfinity_ThrowsArgumentException |
| V03.NegInf | Non-finite coordinate | a = [2]; a.Set(0, double.NegativeInfinity) | ArgumentException; state [2] unchanged; Dimension = 1, NonZeroCount = 1 | Set_NonFiniteNegativeInfinity_ThrowsArgumentException |
| V17.Set | Removal by negative zero | a = [2]; a.Set(0, -0.0) | The entry is removed; Dimension = 1, NonZeroCount = 0, Get(0) = 0; empty traversal | Set_NegativeZero_RemovesEntry |
| X06 | Negative index in Set | a = [1, 2]; a.Set(-1, 5) | ArgumentOutOfRangeException; a = [1, 2], Dimension = 2, NonZeroCount = 2 unchanged | Set_NegativeIndex_ThrowsArgumentOutOfRangeException |

## Add

| ID | Scenario | Input and action | Expected result | Test |
| --- | --- | --- | --- | --- |
| B07 | Sum | a = [1, 0, 2]; b = [0, 3, 4]; a.Add(b) | New vector [1, 3, 6], Dimension = 3, NonZeroCount = 3; a and b unchanged | Add_OrdinaryOperands_ReturnsSum |
| B08 | Cancellation | a = [1, 2]; b = [-1, 0]; a.Add(b) | New [0, 2], Dimension = 2, NonZeroCount = 1; a and b unchanged | Add_CancellingCoordinates_ReturnsZeroAndTwo |
| B09 | Independent result | a = [1, 0]; b = [0, 2]; r = a.Add(b); r.Set(0, 9); r.Set(1, 8) | Before Set r = [1, 2]; after r = [9, 8]; a = [1, 0], b = [0, 2] unchanged | Add_ResultMutation_PreservesOperands |
| B10 | Different dimensions | a = [1]; b = [1, 0]; a.Add(b) | ArgumentException; a = [1], b = [1, 0] unchanged; no result | Add_DifferentDimensions_ThrowsArgumentException |
| B11 | Null | a = [1]; a.Add(null) | ArgumentNullException; a = [1] unchanged; no result | Add_NullOperand_ThrowsArgumentNullException |
| V05 | Sum overflow | a = [M]; b = [M]; a.Add(b) | OverflowException; operands unchanged: a = [M], b = [M]; no result | Add_Overflow_ThrowsOverflowException |

## Subtract

| ID | Scenario | Input and action | Expected result | Test |
| --- | --- | --- | --- | --- |
| B12 | Difference | a = [1, 0, 4]; b = [0, 3, 2]; a.Subtract(b) | New [1, -3, 2], Dimension = 3, NonZeroCount = 3; a and b unchanged | Subtract_OrdinaryOperands_ReturnsDifference |
| B13 | Subtracting itself | a = [1, 0, 4]; a.Subtract(a) | New empty vector of dimension 3, NonZeroCount = 0, every Get = 0; a unchanged | Subtract_Self_ReturnsEmptyVector |
| B14 | Different dimensions | a = [1]; b = [1, 0]; a.Subtract(b) | ArgumentException; a = [1], b = [1, 0] unchanged; no result | Subtract_DifferentDimensions_ThrowsArgumentException |
| B15 | Null | a = [1]; a.Subtract(null) | ArgumentNullException; a = [1] unchanged; no result | Subtract_NullOperand_ThrowsArgumentNullException |
| V06 | Difference overflow | a = [M]; b = [-M]; a.Subtract(b) | OverflowException; operands unchanged: a = [M], b = [-M]; no result | Subtract_Overflow_ThrowsOverflowException |

## Multiply

| ID | Scenario | Input and action | Expected result | Test |
| --- | --- | --- | --- | --- |
| B16 | Positive factor | a = [1, 0, -2]; a.Multiply(3) | New [3, 0, -6], Dimension = 3, NonZeroCount = 2; a unchanged | Multiply_PositiveFactor_ReturnsThreeZeroMinusSix |
| B17 | Zero factor | a = [1, 0, -2]; a.Multiply(0) | New empty vector of dimension 3, NonZeroCount = 0, every Get = 0; a unchanged | Multiply_ZeroFactor_ReturnsEmptyVector |
| B18 | Negative factor | a = [1, 0, -2]; a.Multiply(-2) | New [-2, 0, 4], Dimension = 3, NonZeroCount = 2; a unchanged | Multiply_NegativeFactor_ReturnsMinusTwoZeroFour |
| V04.Empty.NaN | Non-finite factor | a = [0]; a.Multiply(double.NaN) | ArgumentException; a = [0] and its dimension 1 unchanged; no result | Multiply_NonFiniteNaNEmptyReceiver_ThrowsArgumentException |
| V04.One.NaN | Non-finite factor | a = [2]; a.Multiply(double.NaN) | ArgumentException; a = [2] and its dimension 1 unchanged; no result | Multiply_NonFiniteNaNOneReceiver_ThrowsArgumentException |
| V04.Empty.PosInf | Non-finite factor | a = [0]; a.Multiply(double.PositiveInfinity) | ArgumentException; a = [0] and its dimension 1 unchanged; no result | Multiply_NonFinitePositiveInfinityEmptyReceiver_ThrowsArgumentException |
| V04.One.PosInf | Non-finite factor | a = [2]; a.Multiply(double.PositiveInfinity) | ArgumentException; a = [2] and its dimension 1 unchanged; no result | Multiply_NonFinitePositiveInfinityOneReceiver_ThrowsArgumentException |
| V04.Empty.NegInf | Non-finite factor | a = [0]; a.Multiply(double.NegativeInfinity) | ArgumentException; a = [0] and its dimension 1 unchanged; no result | Multiply_NonFiniteNegativeInfinityEmptyReceiver_ThrowsArgumentException |
| V04.One.NegInf | Non-finite factor | a = [2]; a.Multiply(double.NegativeInfinity) | ArgumentException; a = [2] and its dimension 1 unchanged; no result | Multiply_NonFiniteNegativeInfinityOneReceiver_ThrowsArgumentException |
| V07 | Product overflow | a = [M]; a.Multiply(2) | OverflowException; operand unchanged: a = [M]; no result | Multiply_Overflow_ThrowsOverflowException |
| V13 | Underflow | a = [1e-200]; a.Multiply(1e-200) | Dimension = 1, NonZeroCount = 0, Get(0) = 0; new empty vector; a unchanged | Multiply_Underflow_ReturnsEmptyVector |

## Norm

| ID | Scenario | Input and action | Expected result | Test |
| --- | --- | --- | --- | --- |
| B19 | Empty vector | a = new SparseVector(3); a.Norm() | 0; a is still an empty vector of dimension 3 | Norm_Empty_ReturnsZero |
| B20 | 3–4–5 triangle | a = [3, 4]; a.Norm() | 5; a = [3, 4] unchanged | Norm_ThreeFour_ReturnsFive |
| V08 | Large coordinates | a = [1e200, 1e200]; a.Norm() | Finite value sqrt(2) · 1e200 within the tolerance; a unchanged | Norm_LargeCoordinates_ReturnsSqrtTwoTimes1E200 |
| V09 | Norm overflow | a = [M, M]; a.Norm() | OverflowException; a = [M, M] unchanged | Norm_Overflow_ThrowsOverflowException |
| V18 | Small coordinates | a = [1e-200, 1e-200]; a.Norm() | Finite nonzero value sqrt(2) · 1e-200 within the tolerance; a unchanged | Norm_SmallCoordinates_ReturnsSqrtTwoTimes1EMinus200 |

## Dot

| ID | Scenario | Input and action | Expected result | Test |
| --- | --- | --- | --- | --- |
| B21 | Common indices | a = [1, 0, 2]; b = [3, 4, 5]; a.Dot(b) | 13; a and b unchanged | Dot_CommonIndices_ReturnsThirteen |
| B22 | No common indices | a = [1, 0, 0]; b = [0, 2, 0]; a.Dot(b) | 0; a and b unchanged | Dot_DisjointIndices_ReturnsZero |
| B23 | Different dimensions | a = [1]; b = [1, 0]; a.Dot(b) | ArgumentException; a = [1], b = [1, 0] unchanged | Dot_DifferentDimensions_ThrowsArgumentException |
| B24 | Null | a = [1]; a.Dot(null) | ArgumentNullException; a = [1] unchanged | Dot_NullOperand_ThrowsArgumentNullException |
| V10 | Product overflow | a = [M]; b = [2]; a.Dot(b) | OverflowException on the product; a = [M], b = [2] unchanged | Dot_ProductOverflow_ThrowsOverflowException |
| V11 | Partial sum overflow | a = [M, M, -M]; b = [1, 1, 1]; a.Dot(b) | OverflowException on the second partial sum, even though the next term could cancel it; a and b unchanged | Dot_IntermediateSumOverflow_ThrowsOverflowException |
| V12 | Summation order | a = [M, -M, 1]; b = [1, 1, 1]; a.Dot(b) | 1: terms are summed in index order; a and b unchanged | Dot_IndexOrderedSummation_ReturnsOne |

## NonZeroEntries

| ID | Scenario | Input and action | Expected result | Test |
| --- | --- | --- | --- | --- |
| B25 | Order | a = new SparseVector(5); a.Set(3, -2); a.Set(0, 1.5); a.Set(2, 4); enumerate a.NonZeroEntries() | Exactly [(0,1.5),(2,4),(3,-2)] in this order; NonZeroCount = 3 | NonZeroEntries_UnorderedInsertion_ReturnsIndexOrder |
| B26 | Empty traversal | a = new SparseVector(3); enumerate a.NonZeroEntries() | Empty sequence; Dimension = 3; NonZeroCount = 0 | NonZeroEntries_EmptyVector_ReturnsEmptySequence |

## Equals and GetHashCode

| ID | Scenario | Input and action | Expected result | Test |
| --- | --- | --- | --- | --- |
| B27 | Exact equality and hash | a = [1, 0, -2]; b = [1, 0, -2]; a.Equals(b); b.Equals(a); compare hashes | Both Equals calls return true; a.GetHashCode() == b.GetHashCode(); a and b unchanged | Equals_IdenticalVectors_ReturnsTrueAndEqualHashes |
| B28 | Different dimensions | a = [1]; b = [1, 0]; a.Equals(b) | false; a and b unchanged; unequal hashes are not required | Equals_DifferentDimensions_ReturnsFalse |
| B29 | Small difference | a = [1]; b = [1 + 1e-12]; a.Equals(b) | false; a and b unchanged | Equals_SmallDifference_ReturnsFalse |
| X03 | Equals with null | a = [1]; a.Equals(null) | false; a = [1] unchanged | Equals_Null_ReturnsFalse |
| X04 | Equals with another type | a = [1]; t = ScriptedSparseVector: Dimension = 1, NonZeroEntries() = [(0,1)]; a.Equals(t) | false: an object of another type with the same data; a = [1] unchanged | Equals_OtherVectorType_ReturnsFalse |

## ApproximatelyEquals

| ID | Scenario | Input and action | Expected result | Test |
| --- | --- | --- | --- | --- |
| B30 | Within tolerance | a = [1, 0]; b = [1.125, 0.125]; a.ApproximatelyEquals(b, 0.125) | true; a and b unchanged | ApproximatelyEquals_WithinTolerance_ReturnsTrue |
| B31 | Outside tolerance | a = [1, 0]; b = [1.25, 0]; a.ApproximatelyEquals(b, 0.125) | false; a and b unchanged | ApproximatelyEquals_OutsideTolerance_ReturnsFalse |
| V15.Equal | Zero tolerance, equal | a = [1]; b = [1]; a.ApproximatelyEquals(b, 0) | true; a and b unchanged | ApproximatelyEquals_ZeroToleranceEqual_ReturnsTrue |
| V15.Differ | Zero tolerance, different | a = [1]; b = [2]; a.ApproximatelyEquals(b, 0) | false; a and b unchanged | ApproximatelyEquals_ZeroToleranceDifferent_ReturnsFalse |
| V16 | Difference overflow | a = [M]; b = [-M]; a.ApproximatelyEquals(b, M) | false, without an overflow exception; a and b unchanged | ApproximatelyEquals_DifferenceOverflow_ReturnsFalse |
| V20.Null | Null | a = [1]; a.ApproximatelyEquals(null, 0) | ArgumentNullException; a = [1] unchanged | ApproximatelyEquals_NullOperand_ThrowsArgumentNullException |
| V20.Dimensions | Different dimensions | a = [1]; b = [1, 0]; a.ApproximatelyEquals(b, 0) | false; dimensions 1 and 2; a and b unchanged | ApproximatelyEquals_DifferentDimensions_ReturnsFalse |
| V14.Minus1 | Invalid tolerance | a = [1]; b = [1]; a.ApproximatelyEquals(b, -1) | ArgumentOutOfRangeException; a = [1], b = [1] unchanged | ApproximatelyEquals_NegativeTolerance_ThrowsArgumentOutOfRangeException |
| V14.NaN | Invalid tolerance | a = [1]; b = [1]; a.ApproximatelyEquals(b, double.NaN) | ArgumentOutOfRangeException; a = [1], b = [1] unchanged | ApproximatelyEquals_NonFiniteToleranceNaN_ThrowsArgumentOutOfRangeException |
| V14.PosInf | Invalid tolerance | a = [1]; b = [1]; a.ApproximatelyEquals(b, double.PositiveInfinity) | ArgumentOutOfRangeException; a = [1], b = [1] unchanged | ApproximatelyEquals_NonFiniteTolerancePositiveInfinity_ThrowsArgumentOutOfRangeException |
| V14.NegInf | Invalid tolerance | a = [1]; b = [1]; a.ApproximatelyEquals(b, double.NegativeInfinity) | ArgumentOutOfRangeException; a = [1], b = [1] unchanged | ApproximatelyEquals_NonFiniteToleranceNegativeInfinity_ThrowsArgumentOutOfRangeException |

## ToString

| ID | Scenario | Input and action | Expected result | Test |
| --- | --- | --- | --- | --- |
| B32 | Format | a = [1.5, 0, 0, -2, 0]; a.ToString() | String `(dim=5; [0]=1.5, [3]=-2)`; a unchanged | ToString_SparseCoordinates_ReturnsSpecifiedFormat |

## Validation of the input ISparseVector

| ID | Scenario | Input and action | Expected result | Test |
| --- | --- | --- | --- | --- |
| V19.01.NaN.Add | Non-finite value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,double.NaN)]; v.Add(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,double.NaN)] unchanged; no result | Add_TraversalNonFiniteValueNaN_ThrowsArgumentException |
| V19.01.NaN.Subtract | Non-finite value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,double.NaN)]; v.Subtract(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,double.NaN)] unchanged; no result | Subtract_TraversalNonFiniteValueNaN_ThrowsArgumentException |
| V19.01.NaN.Dot | Non-finite value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,double.NaN)]; v.Dot(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,double.NaN)] unchanged; no result | Dot_TraversalNonFiniteValueNaN_ThrowsArgumentException |
| V19.01.NaN.ApproximatelyEquals | Non-finite value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,double.NaN)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,double.NaN)] unchanged; no result | ApproximatelyEquals_TraversalNonFiniteValueNaN_ThrowsArgumentException |
| V19.01.PosInf.Add | Non-finite value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,double.PositiveInfinity)]; v.Add(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,double.PositiveInfinity)] unchanged; no result | Add_TraversalNonFiniteValuePositiveInfinity_ThrowsArgumentException |
| V19.01.PosInf.Subtract | Non-finite value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,double.PositiveInfinity)]; v.Subtract(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,double.PositiveInfinity)] unchanged; no result | Subtract_TraversalNonFiniteValuePositiveInfinity_ThrowsArgumentException |
| V19.01.PosInf.Dot | Non-finite value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,double.PositiveInfinity)]; v.Dot(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,double.PositiveInfinity)] unchanged; no result | Dot_TraversalNonFiniteValuePositiveInfinity_ThrowsArgumentException |
| V19.01.PosInf.ApproximatelyEquals | Non-finite value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,double.PositiveInfinity)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,double.PositiveInfinity)] unchanged; no result | ApproximatelyEquals_TraversalNonFiniteValuePositiveInfinity_ThrowsArgumentException |
| V19.01.NegInf.Add | Non-finite value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,double.NegativeInfinity)]; v.Add(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,double.NegativeInfinity)] unchanged; no result | Add_TraversalNonFiniteValueNegativeInfinity_ThrowsArgumentException |
| V19.01.NegInf.Subtract | Non-finite value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,double.NegativeInfinity)]; v.Subtract(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,double.NegativeInfinity)] unchanged; no result | Subtract_TraversalNonFiniteValueNegativeInfinity_ThrowsArgumentException |
| V19.01.NegInf.Dot | Non-finite value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,double.NegativeInfinity)]; v.Dot(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,double.NegativeInfinity)] unchanged; no result | Dot_TraversalNonFiniteValueNegativeInfinity_ThrowsArgumentException |
| V19.01.NegInf.ApproximatelyEquals | Non-finite value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,double.NegativeInfinity)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,double.NegativeInfinity)] unchanged; no result | ApproximatelyEquals_TraversalNonFiniteValueNegativeInfinity_ThrowsArgumentException |
| V19.02.Add | Descending indices | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(1,1),(0,1)]; v.Add(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(1,1),(0,1)] unchanged; no result | Add_TraversalDescendingIndices_ThrowsArgumentException |
| V19.02.Subtract | Descending indices | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(1,1),(0,1)]; v.Subtract(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(1,1),(0,1)] unchanged; no result | Subtract_TraversalDescendingIndices_ThrowsArgumentException |
| V19.02.Dot | Descending indices | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(1,1),(0,1)]; v.Dot(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(1,1),(0,1)] unchanged; no result | Dot_TraversalDescendingIndices_ThrowsArgumentException |
| V19.02.ApproximatelyEquals | Descending indices | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(1,1),(0,1)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(1,1),(0,1)] unchanged; no result | ApproximatelyEquals_TraversalDescendingIndices_ThrowsArgumentException |
| V19.03.Add | Duplicate index | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(0,2)]; v.Add(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(0,2)] unchanged; no result | Add_TraversalDuplicateIndex_ThrowsArgumentException |
| V19.03.Subtract | Duplicate index | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(0,2)]; v.Subtract(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(0,2)] unchanged; no result | Subtract_TraversalDuplicateIndex_ThrowsArgumentException |
| V19.03.Dot | Duplicate index | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(0,2)]; v.Dot(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(0,2)] unchanged; no result | Dot_TraversalDuplicateIndex_ThrowsArgumentException |
| V19.03.ApproximatelyEquals | Duplicate index | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(0,2)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(0,2)] unchanged; no result | ApproximatelyEquals_TraversalDuplicateIndex_ThrowsArgumentException |
| V19.04.Add | Negative index | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(-1,1)]; v.Add(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(-1,1)] unchanged; no result | Add_TraversalNegativeIndex_ThrowsArgumentException |
| V19.04.Subtract | Negative index | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(-1,1)]; v.Subtract(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(-1,1)] unchanged; no result | Subtract_TraversalNegativeIndex_ThrowsArgumentException |
| V19.04.Dot | Negative index | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(-1,1)]; v.Dot(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(-1,1)] unchanged; no result | Dot_TraversalNegativeIndex_ThrowsArgumentException |
| V19.04.ApproximatelyEquals | Negative index | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(-1,1)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(-1,1)] unchanged; no result | ApproximatelyEquals_TraversalNegativeIndex_ThrowsArgumentException |
| V19.05.Add | Index equal to dimension | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(3,1)]; v.Add(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(3,1)] unchanged; no result | Add_TraversalIndexAtDimension_ThrowsArgumentException |
| V19.05.Subtract | Index equal to dimension | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(3,1)]; v.Subtract(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(3,1)] unchanged; no result | Subtract_TraversalIndexAtDimension_ThrowsArgumentException |
| V19.05.Dot | Index equal to dimension | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(3,1)]; v.Dot(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(3,1)] unchanged; no result | Dot_TraversalIndexAtDimension_ThrowsArgumentException |
| V19.05.ApproximatelyEquals | Index equal to dimension | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(3,1)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(3,1)] unchanged; no result | ApproximatelyEquals_TraversalIndexAtDimension_ThrowsArgumentException |
| V19.06.Add | Zero value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,0)]; v.Add(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,0)] unchanged; no result | Add_TraversalZeroValue_ThrowsArgumentException |
| V19.06.Subtract | Zero value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,0)]; v.Subtract(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,0)] unchanged; no result | Subtract_TraversalZeroValue_ThrowsArgumentException |
| V19.06.Dot | Zero value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,0)]; v.Dot(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,0)] unchanged; no result | Dot_TraversalZeroValue_ThrowsArgumentException |
| V19.06.ApproximatelyEquals | Zero value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,0)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,0)] unchanged; no result | ApproximatelyEquals_TraversalZeroValue_ThrowsArgumentException |
| V19.07.Add | Negative zero value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,-0.0)]; v.Add(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,-0.0)] unchanged; no result | Add_TraversalNegativeZeroValue_ThrowsArgumentException |
| V19.07.Subtract | Negative zero value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,-0.0)]; v.Subtract(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,-0.0)] unchanged; no result | Subtract_TraversalNegativeZeroValue_ThrowsArgumentException |
| V19.07.Dot | Negative zero value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,-0.0)]; v.Dot(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,-0.0)] unchanged; no result | Dot_TraversalNegativeZeroValue_ThrowsArgumentException |
| V19.07.ApproximatelyEquals | Negative zero value | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,-0.0)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,-0.0)] unchanged; no result | ApproximatelyEquals_TraversalNegativeZeroValue_ThrowsArgumentException |
| V19.08.NaN.Add | Non-finite tail | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NaN)]; v.Add(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NaN)] unchanged; no result | Add_TraversalTailNonFiniteNaN_ThrowsArgumentException |
| V19.08.NaN.Subtract | Non-finite tail | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NaN)]; v.Subtract(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NaN)] unchanged; no result | Subtract_TraversalTailNonFiniteNaN_ThrowsArgumentException |
| V19.08.NaN.Dot | Non-finite tail | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NaN)]; v.Dot(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NaN)] unchanged; no result | Dot_TraversalTailNonFiniteNaN_ThrowsArgumentException |
| V19.08.NaN.ApproximatelyEquals | Non-finite tail | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NaN)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NaN)] unchanged; no result | ApproximatelyEquals_TraversalTailNonFiniteNaN_ThrowsArgumentException |
| V19.08.PosInf.Add | Non-finite tail | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.PositiveInfinity)]; v.Add(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.PositiveInfinity)] unchanged; no result | Add_TraversalTailNonFinitePositiveInfinity_ThrowsArgumentException |
| V19.08.PosInf.Subtract | Non-finite tail | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.PositiveInfinity)]; v.Subtract(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.PositiveInfinity)] unchanged; no result | Subtract_TraversalTailNonFinitePositiveInfinity_ThrowsArgumentException |
| V19.08.PosInf.Dot | Non-finite tail | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.PositiveInfinity)]; v.Dot(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.PositiveInfinity)] unchanged; no result | Dot_TraversalTailNonFinitePositiveInfinity_ThrowsArgumentException |
| V19.08.PosInf.ApproximatelyEquals | Non-finite tail | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.PositiveInfinity)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.PositiveInfinity)] unchanged; no result | ApproximatelyEquals_TraversalTailNonFinitePositiveInfinity_ThrowsArgumentException |
| V19.08.NegInf.Add | Non-finite tail | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NegativeInfinity)]; v.Add(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NegativeInfinity)] unchanged; no result | Add_TraversalTailNonFiniteNegativeInfinity_ThrowsArgumentException |
| V19.08.NegInf.Subtract | Non-finite tail | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NegativeInfinity)]; v.Subtract(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NegativeInfinity)] unchanged; no result | Subtract_TraversalTailNonFiniteNegativeInfinity_ThrowsArgumentException |
| V19.08.NegInf.Dot | Non-finite tail | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NegativeInfinity)]; v.Dot(t) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NegativeInfinity)] unchanged; no result | Dot_TraversalTailNonFiniteNegativeInfinity_ThrowsArgumentException |
| V19.08.NegInf.ApproximatelyEquals | Non-finite tail | v = [1, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NegativeInfinity)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [1, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NegativeInfinity)] unchanged; no result | ApproximatelyEquals_TraversalTailNonFiniteNegativeInfinity_ThrowsArgumentException |
| V19.E.NaN.Add | Non-finite tail, empty receiver | v = [0, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NaN)]; v.Add(t) | ArgumentException; v = [0, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NaN)] unchanged; no result | Add_TraversalTailNonFiniteNaNEmptyReceiver_ThrowsArgumentException |
| V19.E.NaN.Subtract | Non-finite tail, empty receiver | v = [0, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NaN)]; v.Subtract(t) | ArgumentException; v = [0, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NaN)] unchanged; no result | Subtract_TraversalTailNonFiniteNaNEmptyReceiver_ThrowsArgumentException |
| V19.E.NaN.Dot | Non-finite tail, empty receiver | v = [0, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NaN)]; v.Dot(t) | ArgumentException; v = [0, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NaN)] unchanged; no result | Dot_TraversalTailNonFiniteNaNEmptyReceiver_ThrowsArgumentException |
| V19.E.NaN.ApproximatelyEquals | Non-finite tail, empty receiver | v = [0, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NaN)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [0, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NaN)] unchanged; no result | ApproximatelyEquals_TraversalTailNonFiniteNaNEmptyReceiver_ThrowsArgumentException |
| V19.E.PosInf.Add | Non-finite tail, empty receiver | v = [0, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.PositiveInfinity)]; v.Add(t) | ArgumentException; v = [0, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.PositiveInfinity)] unchanged; no result | Add_TraversalTailNonFinitePositiveInfinityEmptyReceiver_ThrowsArgumentException |
| V19.E.PosInf.Subtract | Non-finite tail, empty receiver | v = [0, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.PositiveInfinity)]; v.Subtract(t) | ArgumentException; v = [0, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.PositiveInfinity)] unchanged; no result | Subtract_TraversalTailNonFinitePositiveInfinityEmptyReceiver_ThrowsArgumentException |
| V19.E.PosInf.Dot | Non-finite tail, empty receiver | v = [0, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.PositiveInfinity)]; v.Dot(t) | ArgumentException; v = [0, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.PositiveInfinity)] unchanged; no result | Dot_TraversalTailNonFinitePositiveInfinityEmptyReceiver_ThrowsArgumentException |
| V19.E.PosInf.ApproximatelyEquals | Non-finite tail, empty receiver | v = [0, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.PositiveInfinity)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [0, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.PositiveInfinity)] unchanged; no result | ApproximatelyEquals_TraversalTailNonFinitePositiveInfinityEmptyReceiver_ThrowsArgumentException |
| V19.E.NegInf.Add | Non-finite tail, empty receiver | v = [0, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NegativeInfinity)]; v.Add(t) | ArgumentException; v = [0, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NegativeInfinity)] unchanged; no result | Add_TraversalTailNonFiniteNegativeInfinityEmptyReceiver_ThrowsArgumentException |
| V19.E.NegInf.Subtract | Non-finite tail, empty receiver | v = [0, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NegativeInfinity)]; v.Subtract(t) | ArgumentException; v = [0, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NegativeInfinity)] unchanged; no result | Subtract_TraversalTailNonFiniteNegativeInfinityEmptyReceiver_ThrowsArgumentException |
| V19.E.NegInf.Dot | Non-finite tail, empty receiver | v = [0, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NegativeInfinity)]; v.Dot(t) | ArgumentException; v = [0, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NegativeInfinity)] unchanged; no result | Dot_TraversalTailNonFiniteNegativeInfinityEmptyReceiver_ThrowsArgumentException |
| V19.E.NegInf.ApproximatelyEquals | Non-finite tail, empty receiver | v = [0, 0, 0]; t: Dimension = 3, NonZeroEntries() = [(0,1),(2,double.NegativeInfinity)]; v.ApproximatelyEquals(t, 0) | ArgumentException; v = [0, 0, 0] unchanged; dimension and sequence of t = [(0,1),(2,double.NegativeInfinity)] unchanged; no result | ApproximatelyEquals_TraversalTailNonFiniteNegativeInfinityEmptyReceiver_ThrowsArgumentException |

