using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reversi.Algebra.Tests
{
    [TestClass]
    public class SparseVectorUnitTest
    {
        // B01: Constructing [0, 2, 0, -3] yields dimension 4 with entries [(1,2),(3,-3)] and two nonzero coordinates.
        [TestMethod]
        public void Constructor_ZeroCoordinates_OmitsZeroEntries()
        {
            // Arrange
            double[] values = new double[] { 0, 2, 0, -3 };

            // Act
            var a = new SparseVector(values);

            // Assert
            AssertVector(a, 0, 2, 0, -3);
        }

        // B02: Constructing a vector with dimension 0 throws ArgumentOutOfRangeException.
        [TestMethod]
        public void Constructor_ZeroDimension_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int dimension = 0;

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SparseVector(dimension));
        }

        // B03: Constructing a vector from a null array throws ArgumentNullException.
        [TestMethod]
        public void Constructor_NullArray_ThrowsArgumentNullException()
        {
            // Arrange
            double[] values = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new SparseVector(values));
        }

        // B04: Copying [1, 0, 2] and setting indices 0 to 7 and 2 to 0 yields [7, 0, 0] while the original remains [1, 0, 2].
        [TestMethod]
        public void Constructor_Copy_MutationsPreserveOriginal()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0, 2 });

            // Act
            var b = new SparseVector(a);
            b.Set(0, 7);
            b.Set(2, 0);

            // Assert
            AssertVector(a, 1, 0, 2);
            AssertVector(b, 7, 0, 0);
            Assert.AreNotSame(a, b);
        }

        // V01: The default constructor yields [0] with dimension 1, zero nonzero entries, and an empty traversal.
        [TestMethod]
        public void Constructor_Default_ReturnsEmptyDimensionOne()
        {
            // Arrange

            // Act
            var a = new SparseVector();

            // Assert
            AssertVector(a, 0);
        }

        // V02.NaN: Constructing [1, NaN] throws ArgumentException.
        [TestMethod]
        public void Constructor_NonFiniteNaN_ThrowsArgumentException()
        {
            // Arrange
            double[] values = new double[] { 1, double.NaN };

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new SparseVector(values));
        }

        // V02.PosInf: Constructing [1, double.PositiveInfinity] throws ArgumentException.
        [TestMethod]
        public void Constructor_NonFinitePositiveInfinity_ThrowsArgumentException()
        {
            // Arrange
            double[] values = new double[] { 1, double.PositiveInfinity };

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new SparseVector(values));
        }

        // V02.NegInf: Constructing [1, double.NegativeInfinity] throws ArgumentException.
        [TestMethod]
        public void Constructor_NonFiniteNegativeInfinity_ThrowsArgumentException()
        {
            // Arrange
            double[] values = new double[] { 1, double.NegativeInfinity };

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new SparseVector(values));
        }

        // V17.Ctor: Constructing [+0.0, -0.0] yields dimension 2, zero nonzero entries, zero coordinates, and an empty traversal.
        [TestMethod]
        public void Constructor_SignedZeros_OmitsZeroEntries()
        {
            // Arrange
            double negativeZero = -0.0;
            Assert.IsTrue(double.IsNegativeInfinity(1.0 / negativeZero));

            // Act
            var a = new SparseVector(new double[] { +0.0, negativeZero });

            // Assert
            AssertVector(a, 0, 0);
        }

        // X01: Constructing a vector from an empty double array throws ArgumentException.
        [TestMethod]
        public void Constructor_EmptyArray_ThrowsArgumentException()
        {
            // Arrange
            var values = new double[0];

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new SparseVector(values));
        }

        // X02: Reading index 2 of [1, 2] throws ArgumentOutOfRangeException and leaves [1, 2] unchanged.
        [TestMethod]
        public void Get_IndexAtDimension_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 2 });

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.Get(2));
            AssertVector(a, 1, 2);
        }

        // B05: Setting index 0 of [1, 2] to 0 yields [0, 2] with dimension 2 and one nonzero entry.
        [TestMethod]
        public void Set_Zero_RemovesEntry()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 2 });

            // Act
            a.Set(0, 0);

            // Assert
            AssertVector(a, 0, 2);
        }

        // B06: Setting index 2 of [1, 2] to 7 throws ArgumentOutOfRangeException and leaves [1, 2] unchanged.
        [TestMethod]
        public void Set_IndexAtDimension_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 2 });

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.Set(2, 7));
            AssertVector(a, 1, 2);
        }

        // V03.NaN: Setting index 0 of [2] to NaN throws ArgumentException and leaves [2] unchanged.
        [TestMethod]
        public void Set_NonFiniteNaN_ThrowsArgumentException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 2 });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => a.Set(0, double.NaN));
            AssertVector(a, 2);
        }

        // V03.PosInf: Setting index 0 of [2] to double.PositiveInfinity throws ArgumentException and leaves [2] unchanged.
        [TestMethod]
        public void Set_NonFinitePositiveInfinity_ThrowsArgumentException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 2 });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => a.Set(0, double.PositiveInfinity));
            AssertVector(a, 2);
        }

        // V03.NegInf: Setting index 0 of [2] to double.NegativeInfinity throws ArgumentException and leaves [2] unchanged.
        [TestMethod]
        public void Set_NonFiniteNegativeInfinity_ThrowsArgumentException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 2 });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => a.Set(0, double.NegativeInfinity));
            AssertVector(a, 2);
        }

        // V17.Set: Setting index 0 of [2] to -0.0 removes its entry and yields [0] with dimension 1.
        [TestMethod]
        public void Set_NegativeZero_RemovesEntry()
        {
            // Arrange
            double negativeZero = -0.0;
            Assert.IsTrue(double.IsNegativeInfinity(1.0 / negativeZero));
            var a = new SparseVector(new double[] { 2 });

            // Act
            a.Set(0, negativeZero);

            // Assert
            AssertVector(a, 0);
        }

        // B07: Adding [1, 0, 2] and [0, 3, 4] returns a new [1, 3, 6] and leaves both operands unchanged.
        [TestMethod]
        public void Add_OrdinaryOperands_ReturnsSum()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0, 2 });
            var b = new SparseVector(new double[] { 0, 3, 4 });

            // Act
            var result = a.Add(b);

            // Assert
            AssertVector(result, 1, 3, 6);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            AssertVector(a, 1, 0, 2);
            AssertVector(b, 0, 3, 4);
        }

        // B08: Adding [1, 2] and [-1, 0] returns a new [0, 2] with one nonzero entry and leaves both operands unchanged.
        [TestMethod]
        public void Add_CancellingCoordinates_ReturnsZeroAndTwo()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 2 });
            var b = new SparseVector(new double[] { -1, 0 });

            // Act
            var result = a.Add(b);

            // Assert
            AssertVector(result, 0, 2);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            AssertVector(a, 1, 2);
            AssertVector(b, -1, 0);
        }

        // B09: Adding [1, 0] and [0, 2] yields [1, 2], whose mutation to [9, 8] leaves both operands unchanged.
        [TestMethod]
        public void Add_ResultMutation_PreservesOperands()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0 });
            var b = new SparseVector(new double[] { 0, 2 });

            // Act
            var result = a.Add(b);

            // Assert
            AssertVector(result, 1, 2);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            result.Set(0, 9);
            result.Set(1, 8);
            AssertVector(result, 9, 8);
            AssertVector(a, 1, 0);
            AssertVector(b, 0, 2);
        }

        // B10: Adding [1] and [1, 0] throws ArgumentException and leaves both operands unchanged.
        [TestMethod]
        public void Add_DifferentDimensions_ThrowsArgumentException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var b = new SparseVector(new double[] { 1, 0 });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => a.Add(b));
            AssertVector(a, 1);
            AssertVector(b, 1, 0);
        }

        // B11: Adding null to [1] throws ArgumentNullException and leaves [1] unchanged.
        [TestMethod]
        public void Add_NullOperand_ThrowsArgumentNullException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => a.Add(null));
            AssertVector(a, 1);
        }

        // V05: Adding [double.MaxValue] to [double.MaxValue] throws OverflowException and leaves both operands unchanged.
        [TestMethod]
        public void Add_Overflow_ThrowsOverflowException()
        {
            // Arrange
            var a = new SparseVector(new double[] { double.MaxValue });
            var b = new SparseVector(new double[] { double.MaxValue });

            // Act & Assert
            Assert.ThrowsException<OverflowException>(() => a.Add(b));
            AssertVector(a, double.MaxValue);
            AssertVector(b, double.MaxValue);
        }

        // B12: Subtracting [0, 3, 2] from [1, 0, 4] returns a new [1, -3, 2] and leaves both operands unchanged.
        [TestMethod]
        public void Subtract_OrdinaryOperands_ReturnsDifference()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0, 4 });
            var b = new SparseVector(new double[] { 0, 3, 2 });

            // Act
            var result = a.Subtract(b);

            // Assert
            AssertVector(result, 1, -3, 2);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            AssertVector(a, 1, 0, 4);
            AssertVector(b, 0, 3, 2);
        }

        // B13: Subtracting [1, 0, 4] from itself returns a new empty vector of dimension 3 and leaves [1, 0, 4] unchanged.
        [TestMethod]
        public void Subtract_Self_ReturnsEmptyVector()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0, 4 });

            // Act
            var result = a.Subtract(a);

            // Assert
            AssertVector(result, 0, 0, 0);
            Assert.AreNotSame(a, result);
            AssertVector(a, 1, 0, 4);
        }

        // B14: Subtracting [1, 0] from [1] throws ArgumentException and leaves both operands unchanged.
        [TestMethod]
        public void Subtract_DifferentDimensions_ThrowsArgumentException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var b = new SparseVector(new double[] { 1, 0 });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => a.Subtract(b));
            AssertVector(a, 1);
            AssertVector(b, 1, 0);
        }

        // B15: Subtracting null from [1] throws ArgumentNullException and leaves [1] unchanged.
        [TestMethod]
        public void Subtract_NullOperand_ThrowsArgumentNullException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => a.Subtract(null));
            AssertVector(a, 1);
        }

        // V06: Subtracting [-double.MaxValue] from [double.MaxValue] throws OverflowException and leaves both operands unchanged.
        [TestMethod]
        public void Subtract_Overflow_ThrowsOverflowException()
        {
            // Arrange
            var a = new SparseVector(new double[] { double.MaxValue });
            var b = new SparseVector(new double[] { -double.MaxValue });

            // Act & Assert
            Assert.ThrowsException<OverflowException>(() => a.Subtract(b));
            AssertVector(a, double.MaxValue);
            AssertVector(b, -double.MaxValue);
        }

        // B16: Multiplying [1, 0, -2] by 3 returns a new [3, 0, -6] and leaves [1, 0, -2] unchanged.
        [TestMethod]
        public void Multiply_PositiveFactor_ReturnsThreeZeroMinusSix()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0, -2 });

            // Act
            var result = a.Multiply(3);

            // Assert
            AssertVector(result, 3, 0, -6);
            Assert.AreNotSame(a, result);
            AssertVector(a, 1, 0, -2);
        }

        // B17: Multiplying [1, 0, -2] by 0 returns a new empty vector of dimension 3 and leaves [1, 0, -2] unchanged.
        [TestMethod]
        public void Multiply_ZeroFactor_ReturnsEmptyVector()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0, -2 });

            // Act
            var result = a.Multiply(0);

            // Assert
            AssertVector(result, 0, 0, 0);
            Assert.AreNotSame(a, result);
            AssertVector(a, 1, 0, -2);
        }

        // B18: Multiplying [1, 0, -2] by -2 returns a new [-2, 0, 4] and leaves [1, 0, -2] unchanged.
        [TestMethod]
        public void Multiply_NegativeFactor_ReturnsMinusTwoZeroFour()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0, -2 });

            // Act
            var result = a.Multiply(-2);

            // Assert
            AssertVector(result, -2, 0, 4);
            Assert.AreNotSame(a, result);
            AssertVector(a, 1, 0, -2);
        }

        // V04.Empty.NaN: Multiplying [0] by NaN throws ArgumentException and leaves [0] unchanged.
        [TestMethod]
        public void Multiply_NonFiniteNaNEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 0 });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => a.Multiply(double.NaN));
            AssertVector(a, 0);
        }

        // V04.One.NaN: Multiplying [2] by NaN throws ArgumentException and leaves [2] unchanged.
        [TestMethod]
        public void Multiply_NonFiniteNaNOneReceiver_ThrowsArgumentException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 2 });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => a.Multiply(double.NaN));
            AssertVector(a, 2);
        }

        // V04.Empty.PosInf: Multiplying [0] by double.PositiveInfinity throws ArgumentException and leaves [0] unchanged.
        [TestMethod]
        public void Multiply_NonFinitePositiveInfinityEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 0 });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => a.Multiply(double.PositiveInfinity));
            AssertVector(a, 0);
        }

        // V04.One.PosInf: Multiplying [2] by double.PositiveInfinity throws ArgumentException and leaves [2] unchanged.
        [TestMethod]
        public void Multiply_NonFinitePositiveInfinityOneReceiver_ThrowsArgumentException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 2 });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => a.Multiply(double.PositiveInfinity));
            AssertVector(a, 2);
        }

        // V04.Empty.NegInf: Multiplying [0] by double.NegativeInfinity throws ArgumentException and leaves [0] unchanged.
        [TestMethod]
        public void Multiply_NonFiniteNegativeInfinityEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 0 });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => a.Multiply(double.NegativeInfinity));
            AssertVector(a, 0);
        }

        // V04.One.NegInf: Multiplying [2] by double.NegativeInfinity throws ArgumentException and leaves [2] unchanged.
        [TestMethod]
        public void Multiply_NonFiniteNegativeInfinityOneReceiver_ThrowsArgumentException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 2 });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => a.Multiply(double.NegativeInfinity));
            AssertVector(a, 2);
        }

        // V07: Multiplying [double.MaxValue] by 2 throws OverflowException and leaves the operand unchanged.
        [TestMethod]
        public void Multiply_Overflow_ThrowsOverflowException()
        {
            // Arrange
            var a = new SparseVector(new double[] { double.MaxValue });

            // Act & Assert
            Assert.ThrowsException<OverflowException>(() => a.Multiply(2));
            AssertVector(a, double.MaxValue);
        }

        // V13: Multiplying [1e-200] by 1e-200 returns a new empty vector of dimension 1 and leaves [1e-200] unchanged.
        [TestMethod]
        public void Multiply_Underflow_ReturnsEmptyVector()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1e-200 });

            // Act
            var result = a.Multiply(1e-200);

            // Assert
            AssertVector(result, 0);
            Assert.AreNotSame(a, result);
            AssertVector(a, 1e-200);
        }

        // B19: The norm of an empty vector of dimension 3 is 0 and leaves the vector unchanged.
        [TestMethod]
        public void Norm_Empty_ReturnsZero()
        {
            // Arrange
            var a = new SparseVector(3);
            double expected = 0.0;

            // Act
            double result = a.Norm();

            // Assert
            Assert.AreEqual(expected, result);
            AssertVector(a, 0, 0, 0);
        }

        // B20: The norm of [3, 4] is 5 and leaves [3, 4] unchanged.
        [TestMethod]
        public void Norm_ThreeFour_ReturnsFive()
        {
            // Arrange
            var a = new SparseVector(new double[] { 3, 4 });
            double expected = 5.0;

            // Act
            double result = a.Norm();

            // Assert
            Assert.AreEqual(expected, result);
            AssertVector(a, 3, 4);
        }

        // V08: The norm of [1e200, 1e200] is sqrt(2) * 1e200 within relative tolerance 1e-12 and leaves the vector unchanged.
        [TestMethod]
        public void Norm_LargeCoordinates_ReturnsSqrtTwoTimes1E200()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1e200, 1e200 });
            double expected = Math.Sqrt(2) * 1e200;

            // Act
            double result = a.Norm();

            // Assert
            Assert.IsFalse(double.IsNaN(result) || double.IsInfinity(result));
            Assert.AreEqual(expected, result, Math.Abs(expected) * 1e-12);
            AssertVector(a, 1e200, 1e200);
        }

        // V09: The norm of [double.MaxValue, double.MaxValue] throws OverflowException and leaves the vector unchanged.
        [TestMethod]
        public void Norm_Overflow_ThrowsOverflowException()
        {
            // Arrange
            var a = new SparseVector(new double[] { double.MaxValue, double.MaxValue });

            // Act & Assert
            Assert.ThrowsException<OverflowException>(() => a.Norm());
            AssertVector(a, double.MaxValue, double.MaxValue);
        }

        // V18: The norm of [1e-200, 1e-200] is sqrt(2) * 1e-200 within relative tolerance 1e-12 and leaves the vector unchanged.
        [TestMethod]
        public void Norm_SmallCoordinates_ReturnsSqrtTwoTimes1EMinus200()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1e-200, 1e-200 });
            double expected = Math.Sqrt(2) * 1e-200;

            // Act
            double result = a.Norm();

            // Assert
            Assert.IsFalse(double.IsNaN(result) || double.IsInfinity(result));
            Assert.AreNotEqual(0.0, result);
            Assert.AreEqual(expected, result, Math.Abs(expected) * 1e-12);
            AssertVector(a, 1e-200, 1e-200);
        }

        // B21: The dot product of [1, 0, 2] and [3, 4, 5] is 13 and leaves both operands unchanged.
        [TestMethod]
        public void Dot_CommonIndices_ReturnsThirteen()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0, 2 });
            var b = new SparseVector(new double[] { 3, 4, 5 });

            // Act
            var result = a.Dot(b);

            // Assert
            Assert.AreEqual(13.0, result);
            AssertVector(a, 1, 0, 2);
            AssertVector(b, 3, 4, 5);
        }

        // B22: The dot product of [1, 0, 0] and [0, 2, 0] is 0 and leaves both operands unchanged.
        [TestMethod]
        public void Dot_DisjointIndices_ReturnsZero()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0, 0 });
            var b = new SparseVector(new double[] { 0, 2, 0 });

            // Act
            var result = a.Dot(b);

            // Assert
            Assert.AreEqual(0.0, result);
            AssertVector(a, 1, 0, 0);
            AssertVector(b, 0, 2, 0);
        }

        // B23: The dot product of [1] and [1, 0] throws ArgumentException and leaves both operands unchanged.
        [TestMethod]
        public void Dot_DifferentDimensions_ThrowsArgumentException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var b = new SparseVector(new double[] { 1, 0 });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => a.Dot(b));
            AssertVector(a, 1);
            AssertVector(b, 1, 0);
        }

        // B24: The dot product of [1] and null throws ArgumentNullException and leaves [1] unchanged.
        [TestMethod]
        public void Dot_NullOperand_ThrowsArgumentNullException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => a.Dot(null));
            AssertVector(a, 1);
        }

        // V10: The dot product of [double.MaxValue] and [2] throws OverflowException on multiplication and leaves both operands unchanged.
        [TestMethod]
        public void Dot_ProductOverflow_ThrowsOverflowException()
        {
            // Arrange
            var a = new SparseVector(new double[] { double.MaxValue });
            var b = new SparseVector(new double[] { 2 });

            // Act & Assert
            Assert.ThrowsException<OverflowException>(() => a.Dot(b));
            AssertVector(a, double.MaxValue);
            AssertVector(b, 2);
        }

        // V11: The dot product of [double.MaxValue, double.MaxValue, -double.MaxValue] and [1, 1, 1] throws OverflowException on the second partial sum and leaves both operands unchanged.
        [TestMethod]
        public void Dot_IntermediateSumOverflow_ThrowsOverflowException()
        {
            // Arrange
            var a = new SparseVector(new double[] { double.MaxValue, double.MaxValue, -double.MaxValue });
            var b = new SparseVector(new double[] { 1, 1, 1 });

            // Act & Assert
            Assert.ThrowsException<OverflowException>(() => a.Dot(b));
            AssertVector(a, double.MaxValue, double.MaxValue, -double.MaxValue);
            AssertVector(b, 1, 1, 1);
        }

        // V12: The dot product of [double.MaxValue, -double.MaxValue, 1] and [1, 1, 1] is 1 when accumulated in index order and leaves both operands unchanged.
        [TestMethod]
        public void Dot_IndexOrderedSummation_ReturnsOne()
        {
            // Arrange
            var a = new SparseVector(new double[] { double.MaxValue, -double.MaxValue, 1 });
            var b = new SparseVector(new double[] { 1, 1, 1 });

            // Act
            var result = a.Dot(b);

            // Assert
            Assert.AreEqual(1.0, result);
            AssertVector(a, double.MaxValue, -double.MaxValue, 1);
            AssertVector(b, 1, 1, 1);
        }

        // B25: Inserting (3,-2), (0,1.5), and (2,4) into an empty vector of dimension 5 yields exactly [(0,1.5),(2,4),(3,-2)] in index order.
        [TestMethod]
        public void NonZeroEntries_UnorderedInsertion_ReturnsIndexOrder()
        {
            // Arrange
            var a = new SparseVector(5);
            a.Set(3, -2);
            a.Set(0, 1.5);
            a.Set(2, 4);

            // Act
            var entries = a.NonZeroEntries();

            // Assert
            AssertEntries(entries, new SparseEntry[] { new SparseEntry(0, 1.5), new SparseEntry(2, 4), new SparseEntry(3, -2) });
            AssertVector(a, 1.5, 0, 4, -2, 0);
        }

        // B26: Enumerating an empty vector of dimension 3 yields no entries and leaves its dimension and zero entry count unchanged.
        [TestMethod]
        public void NonZeroEntries_EmptyVector_ReturnsEmptySequence()
        {
            // Arrange
            var a = new SparseVector(3);

            // Act
            var entries = a.NonZeroEntries();

            // Assert
            AssertEntries(entries, new SparseEntry[0]);
            AssertVector(a, 0, 0, 0);
        }

        // B27: Comparing two vectors [1, 0, -2] with Equals in both directions returns true with equal hashes and leaves both vectors unchanged.
        [TestMethod]
        public void Equals_IdenticalVectors_ReturnsTrueAndEqualHashes()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0, -2 });
            var b = new SparseVector(new double[] { 1, 0, -2 });

            // Act
            bool forward = a.Equals(b);
            bool reverse = b.Equals(a);
            int hashA = a.GetHashCode();
            int hashB = b.GetHashCode();

            // Assert
            Assert.IsTrue(forward);
            Assert.IsTrue(reverse);
            Assert.AreEqual(hashA, hashB);
            AssertVector(a, 1, 0, -2);
            AssertVector(b, 1, 0, -2);
        }

        // B28: Comparing [1] with [1, 0] using Equals returns false and leaves both vectors unchanged.
        [TestMethod]
        public void Equals_DifferentDimensions_ReturnsFalse()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var b = new SparseVector(new double[] { 1, 0 });

            // Act
            var result = a.Equals(b);

            // Assert
            Assert.AreEqual(false, result);
            AssertVector(a, 1);
            AssertVector(b, 1, 0);
        }

        // B29: Comparing [1] with [1 + 1e-12] using Equals returns false and leaves both vectors unchanged.
        [TestMethod]
        public void Equals_SmallDifference_ReturnsFalse()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var b = new SparseVector(new double[] { 1 + 1e-12 });

            // Act
            var result = a.Equals(b);

            // Assert
            Assert.AreEqual(false, result);
            AssertVector(a, 1);
            AssertVector(b, 1 + 1e-12);
        }

        // B30: Comparing [1, 0] with [1.125, 0.125] at tolerance 0.125 returns true and leaves both vectors unchanged.
        [TestMethod]
        public void ApproximatelyEquals_WithinTolerance_ReturnsTrue()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0 });
            var b = new SparseVector(new double[] { 1.125, 0.125 });

            // Act
            var result = a.ApproximatelyEquals(b, 0.125);

            // Assert
            Assert.AreEqual(true, result);
            AssertVector(a, 1, 0);
            AssertVector(b, 1.125, 0.125);
        }

        // B31: Comparing [1, 0] with [1.25, 0] at tolerance 0.125 returns false and leaves both vectors unchanged.
        [TestMethod]
        public void ApproximatelyEquals_OutsideTolerance_ReturnsFalse()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 0 });
            var b = new SparseVector(new double[] { 1.25, 0 });

            // Act
            var result = a.ApproximatelyEquals(b, 0.125);

            // Assert
            Assert.AreEqual(false, result);
            AssertVector(a, 1, 0);
            AssertVector(b, 1.25, 0);
        }

        // V15.Equal: Comparing [1] with [1] at tolerance 0 returns true and leaves both vectors unchanged.
        [TestMethod]
        public void ApproximatelyEquals_ZeroToleranceEqual_ReturnsTrue()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var b = new SparseVector(new double[] { 1 });

            // Act
            var result = a.ApproximatelyEquals(b, 0);

            // Assert
            Assert.AreEqual(true, result);
            AssertVector(a, 1);
            AssertVector(b, 1);
        }

        // V15.Differ: Comparing [1] with [2] at tolerance 0 returns false and leaves both vectors unchanged.
        [TestMethod]
        public void ApproximatelyEquals_ZeroToleranceDifferent_ReturnsFalse()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var b = new SparseVector(new double[] { 2 });

            // Act
            var result = a.ApproximatelyEquals(b, 0);

            // Assert
            Assert.AreEqual(false, result);
            AssertVector(a, 1);
            AssertVector(b, 2);
        }

        // V16: Comparing [double.MaxValue] with [-double.MaxValue] at tolerance double.MaxValue returns false without overflow and leaves both vectors unchanged.
        [TestMethod]
        public void ApproximatelyEquals_DifferenceOverflow_ReturnsFalse()
        {
            // Arrange
            var a = new SparseVector(new double[] { double.MaxValue });
            var b = new SparseVector(new double[] { -double.MaxValue });

            // Act
            var result = a.ApproximatelyEquals(b, double.MaxValue);

            // Assert
            Assert.AreEqual(false, result);
            AssertVector(a, double.MaxValue);
            AssertVector(b, -double.MaxValue);
        }

        // V20.Null: Comparing [1] with null at tolerance 0 throws ArgumentNullException and leaves [1] unchanged.
        [TestMethod]
        public void ApproximatelyEquals_NullOperand_ThrowsArgumentNullException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => a.ApproximatelyEquals(null, 0));
            AssertVector(a, 1);
        }

        // V20.Dimensions: Comparing [1] with [1, 0] at tolerance 0 returns false and leaves both vectors unchanged.
        [TestMethod]
        public void ApproximatelyEquals_DifferentDimensions_ReturnsFalse()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var b = new SparseVector(new double[] { 1, 0 });

            // Act
            var result = a.ApproximatelyEquals(b, 0);

            // Assert
            Assert.AreEqual(false, result);
            AssertVector(a, 1);
            AssertVector(b, 1, 0);
        }

        // V14.Minus1: Comparing [1] with [1] at tolerance -1 throws ArgumentOutOfRangeException and leaves both vectors unchanged.
        [TestMethod]
        public void ApproximatelyEquals_NegativeTolerance_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var b = new SparseVector(new double[] { 1 });

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.ApproximatelyEquals(b, -1));
            AssertVector(a, 1);
            AssertVector(b, 1);
        }

        // V14.NaN: Comparing [1] with [1] at tolerance NaN throws ArgumentOutOfRangeException and leaves both vectors unchanged.
        [TestMethod]
        public void ApproximatelyEquals_NonFiniteToleranceNaN_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var b = new SparseVector(new double[] { 1 });

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.ApproximatelyEquals(b, double.NaN));
            AssertVector(a, 1);
            AssertVector(b, 1);
        }

        // V14.PosInf: Comparing [1] with [1] at tolerance double.PositiveInfinity throws ArgumentOutOfRangeException and leaves both vectors unchanged.
        [TestMethod]
        public void ApproximatelyEquals_NonFiniteTolerancePositiveInfinity_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var b = new SparseVector(new double[] { 1 });

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.ApproximatelyEquals(b, double.PositiveInfinity));
            AssertVector(a, 1);
            AssertVector(b, 1);
        }

        // V14.NegInf: Comparing [1] with [1] at tolerance double.NegativeInfinity throws ArgumentOutOfRangeException and leaves both vectors unchanged.
        [TestMethod]
        public void ApproximatelyEquals_NonFiniteToleranceNegativeInfinity_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var b = new SparseVector(new double[] { 1 });

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.ApproximatelyEquals(b, double.NegativeInfinity));
            AssertVector(a, 1);
            AssertVector(b, 1);
        }

        // B32: Formatting [1.5, 0, 0, -2, 0] returns "(dim=5; [0]=1.5, [3]=-2)" and leaves the vector unchanged.
        [TestMethod]
        public void ToString_SparseCoordinates_ReturnsSpecifiedFormat()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1.5, 0, 0, -2, 0 });

            // Act
            string result = a.ToString();

            // Assert
            Assert.AreEqual("(dim=5; [0]=1.5, [3]=-2)", result);
            AssertVector(a, 1.5, 0, 0, -2, 0);
        }

        // V19.01.NaN.Add: Calling Add on [1, 0, 0] with a dimension 3 traversal [(0,double.NaN)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalNonFiniteValueNaN_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, double.NaN) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, double.NaN) });
        }

        // V19.01.NaN.Subtract: Calling Subtract on [1, 0, 0] with a dimension 3 traversal [(0,double.NaN)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalNonFiniteValueNaN_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, double.NaN) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, double.NaN) });
        }

        // V19.01.NaN.Dot: Calling Dot on [1, 0, 0] with a dimension 3 traversal [(0,double.NaN)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalNonFiniteValueNaN_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, double.NaN) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, double.NaN) });
        }

        // V19.01.NaN.ApproximatelyEquals: Calling ApproximatelyEquals on [1, 0, 0] with a dimension 3 traversal [(0,double.NaN)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalNonFiniteValueNaN_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, double.NaN) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, double.NaN) });
        }

        // V19.01.PosInf.Add: Calling Add on [1, 0, 0] with a dimension 3 traversal [(0,double.PositiveInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalNonFiniteValuePositiveInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, double.PositiveInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, double.PositiveInfinity) });
        }

        // V19.01.PosInf.Subtract: Calling Subtract on [1, 0, 0] with a dimension 3 traversal [(0,double.PositiveInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalNonFiniteValuePositiveInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, double.PositiveInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, double.PositiveInfinity) });
        }

        // V19.01.PosInf.Dot: Calling Dot on [1, 0, 0] with a dimension 3 traversal [(0,double.PositiveInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalNonFiniteValuePositiveInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, double.PositiveInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, double.PositiveInfinity) });
        }

        // V19.01.PosInf.ApproximatelyEquals: Calling ApproximatelyEquals on [1, 0, 0] with a dimension 3 traversal [(0,double.PositiveInfinity)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalNonFiniteValuePositiveInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, double.PositiveInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, double.PositiveInfinity) });
        }

        // V19.01.NegInf.Add: Calling Add on [1, 0, 0] with a dimension 3 traversal [(0,double.NegativeInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalNonFiniteValueNegativeInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, double.NegativeInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, double.NegativeInfinity) });
        }

        // V19.01.NegInf.Subtract: Calling Subtract on [1, 0, 0] with a dimension 3 traversal [(0,double.NegativeInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalNonFiniteValueNegativeInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, double.NegativeInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, double.NegativeInfinity) });
        }

        // V19.01.NegInf.Dot: Calling Dot on [1, 0, 0] with a dimension 3 traversal [(0,double.NegativeInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalNonFiniteValueNegativeInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, double.NegativeInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, double.NegativeInfinity) });
        }

        // V19.01.NegInf.ApproximatelyEquals: Calling ApproximatelyEquals on [1, 0, 0] with a dimension 3 traversal [(0,double.NegativeInfinity)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalNonFiniteValueNegativeInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, double.NegativeInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, double.NegativeInfinity) });
        }

        // V19.02.Add: Calling Add on [1, 0, 0] with a dimension 3 traversal [(1,1),(0,1)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalDescendingIndices_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(1, 1), new SparseEntry(0, 1) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(1, 1), new SparseEntry(0, 1) });
        }

        // V19.02.Subtract: Calling Subtract on [1, 0, 0] with a dimension 3 traversal [(1,1),(0,1)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalDescendingIndices_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(1, 1), new SparseEntry(0, 1) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(1, 1), new SparseEntry(0, 1) });
        }

        // V19.02.Dot: Calling Dot on [1, 0, 0] with a dimension 3 traversal [(1,1),(0,1)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalDescendingIndices_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(1, 1), new SparseEntry(0, 1) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(1, 1), new SparseEntry(0, 1) });
        }

        // V19.02.ApproximatelyEquals: Calling ApproximatelyEquals on [1, 0, 0] with a dimension 3 traversal [(1,1),(0,1)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalDescendingIndices_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(1, 1), new SparseEntry(0, 1) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(1, 1), new SparseEntry(0, 1) });
        }

        // V19.03.Add: Calling Add on [1, 0, 0] with a dimension 3 traversal [(0,1),(0,2)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalDuplicateIndex_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(0, 2) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(0, 2) });
        }

        // V19.03.Subtract: Calling Subtract on [1, 0, 0] with a dimension 3 traversal [(0,1),(0,2)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalDuplicateIndex_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(0, 2) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(0, 2) });
        }

        // V19.03.Dot: Calling Dot on [1, 0, 0] with a dimension 3 traversal [(0,1),(0,2)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalDuplicateIndex_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(0, 2) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(0, 2) });
        }

        // V19.03.ApproximatelyEquals: Calling ApproximatelyEquals on [1, 0, 0] with a dimension 3 traversal [(0,1),(0,2)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalDuplicateIndex_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(0, 2) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(0, 2) });
        }

        // V19.04.Add: Calling Add on [1, 0, 0] with a dimension 3 traversal [(-1,1)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalNegativeIndex_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(-1, 1) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(-1, 1) });
        }

        // V19.04.Subtract: Calling Subtract on [1, 0, 0] with a dimension 3 traversal [(-1,1)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalNegativeIndex_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(-1, 1) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(-1, 1) });
        }

        // V19.04.Dot: Calling Dot on [1, 0, 0] with a dimension 3 traversal [(-1,1)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalNegativeIndex_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(-1, 1) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(-1, 1) });
        }

        // V19.04.ApproximatelyEquals: Calling ApproximatelyEquals on [1, 0, 0] with a dimension 3 traversal [(-1,1)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalNegativeIndex_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(-1, 1) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(-1, 1) });
        }

        // V19.05.Add: Calling Add on [1, 0, 0] with a dimension 3 traversal [(3,1)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalIndexAtDimension_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(3, 1) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(3, 1) });
        }

        // V19.05.Subtract: Calling Subtract on [1, 0, 0] with a dimension 3 traversal [(3,1)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalIndexAtDimension_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(3, 1) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(3, 1) });
        }

        // V19.05.Dot: Calling Dot on [1, 0, 0] with a dimension 3 traversal [(3,1)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalIndexAtDimension_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(3, 1) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(3, 1) });
        }

        // V19.05.ApproximatelyEquals: Calling ApproximatelyEquals on [1, 0, 0] with a dimension 3 traversal [(3,1)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalIndexAtDimension_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(3, 1) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(3, 1) });
        }

        // V19.06.Add: Calling Add on [1, 0, 0] with a dimension 3 traversal [(0,0)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalZeroValue_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 0) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 0) });
        }

        // V19.06.Subtract: Calling Subtract on [1, 0, 0] with a dimension 3 traversal [(0,0)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalZeroValue_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 0) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 0) });
        }

        // V19.06.Dot: Calling Dot on [1, 0, 0] with a dimension 3 traversal [(0,0)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalZeroValue_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 0) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 0) });
        }

        // V19.06.ApproximatelyEquals: Calling ApproximatelyEquals on [1, 0, 0] with a dimension 3 traversal [(0,0)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalZeroValue_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 0) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 0) });
        }

        // V19.07.Add: Calling Add on [1, 0, 0] with a dimension 3 traversal [(0,-0.0)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalNegativeZeroValue_ThrowsArgumentException()
        {
            // Arrange
            double negativeZero = -0.0;
            Assert.IsTrue(double.IsNegativeInfinity(1.0 / negativeZero));
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, negativeZero) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, negativeZero) });
        }

        // V19.07.Subtract: Calling Subtract on [1, 0, 0] with a dimension 3 traversal [(0,-0.0)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalNegativeZeroValue_ThrowsArgumentException()
        {
            // Arrange
            double negativeZero = -0.0;
            Assert.IsTrue(double.IsNegativeInfinity(1.0 / negativeZero));
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, negativeZero) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, negativeZero) });
        }

        // V19.07.Dot: Calling Dot on [1, 0, 0] with a dimension 3 traversal [(0,-0.0)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalNegativeZeroValue_ThrowsArgumentException()
        {
            // Arrange
            double negativeZero = -0.0;
            Assert.IsTrue(double.IsNegativeInfinity(1.0 / negativeZero));
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, negativeZero) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, negativeZero) });
        }

        // V19.07.ApproximatelyEquals: Calling ApproximatelyEquals on [1, 0, 0] with a dimension 3 traversal [(0,-0.0)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalNegativeZeroValue_ThrowsArgumentException()
        {
            // Arrange
            double negativeZero = -0.0;
            Assert.IsTrue(double.IsNegativeInfinity(1.0 / negativeZero));
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, negativeZero) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(1, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, negativeZero) });
        }

        // V19.08.NaN.Add: Calling Add on [1, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NaN)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalTailNonFiniteNaN_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });
        }

        // V19.08.NaN.Subtract: Calling Subtract on [1, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NaN)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalTailNonFiniteNaN_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });
        }

        // V19.08.NaN.Dot: Calling Dot on [1, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NaN)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalTailNonFiniteNaN_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });
        }

        // V19.08.NaN.ApproximatelyEquals: Calling ApproximatelyEquals on [1, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NaN)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalTailNonFiniteNaN_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });
        }

        // V19.08.PosInf.Add: Calling Add on [1, 0, 0] with a dimension 3 traversal [(0,1),(2,double.PositiveInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalTailNonFinitePositiveInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });
        }

        // V19.08.PosInf.Subtract: Calling Subtract on [1, 0, 0] with a dimension 3 traversal [(0,1),(2,double.PositiveInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalTailNonFinitePositiveInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });
        }

        // V19.08.PosInf.Dot: Calling Dot on [1, 0, 0] with a dimension 3 traversal [(0,1),(2,double.PositiveInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalTailNonFinitePositiveInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });
        }

        // V19.08.PosInf.ApproximatelyEquals: Calling ApproximatelyEquals on [1, 0, 0] with a dimension 3 traversal [(0,1),(2,double.PositiveInfinity)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalTailNonFinitePositiveInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });
        }

        // V19.08.NegInf.Add: Calling Add on [1, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NegativeInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalTailNonFiniteNegativeInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });
        }

        // V19.08.NegInf.Subtract: Calling Subtract on [1, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NegativeInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalTailNonFiniteNegativeInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });
        }

        // V19.08.NegInf.Dot: Calling Dot on [1, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NegativeInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalTailNonFiniteNegativeInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });
        }

        // V19.08.NegInf.ApproximatelyEquals: Calling ApproximatelyEquals on [1, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NegativeInfinity)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalTailNonFiniteNegativeInfinity_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 1, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 1, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });
        }

        // V19.E.NaN.Add: Calling Add on [0, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NaN)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalTailNonFiniteNaNEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 0, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 0, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });
        }

        // V19.E.NaN.Subtract: Calling Subtract on [0, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NaN)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalTailNonFiniteNaNEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 0, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 0, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });
        }

        // V19.E.NaN.Dot: Calling Dot on [0, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NaN)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalTailNonFiniteNaNEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 0, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 0, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });
        }

        // V19.E.NaN.ApproximatelyEquals: Calling ApproximatelyEquals on [0, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NaN)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalTailNonFiniteNaNEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 0, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 0, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NaN) });
        }

        // V19.E.PosInf.Add: Calling Add on [0, 0, 0] with a dimension 3 traversal [(0,1),(2,double.PositiveInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalTailNonFinitePositiveInfinityEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 0, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 0, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });
        }

        // V19.E.PosInf.Subtract: Calling Subtract on [0, 0, 0] with a dimension 3 traversal [(0,1),(2,double.PositiveInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalTailNonFinitePositiveInfinityEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 0, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 0, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });
        }

        // V19.E.PosInf.Dot: Calling Dot on [0, 0, 0] with a dimension 3 traversal [(0,1),(2,double.PositiveInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalTailNonFinitePositiveInfinityEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 0, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 0, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });
        }

        // V19.E.PosInf.ApproximatelyEquals: Calling ApproximatelyEquals on [0, 0, 0] with a dimension 3 traversal [(0,1),(2,double.PositiveInfinity)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalTailNonFinitePositiveInfinityEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 0, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 0, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.PositiveInfinity) });
        }

        // V19.E.NegInf.Add: Calling Add on [0, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NegativeInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Add_TraversalTailNonFiniteNegativeInfinityEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 0, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Add(t));
            AssertVector(v, 0, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });
        }

        // V19.E.NegInf.Subtract: Calling Subtract on [0, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NegativeInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Subtract_TraversalTailNonFiniteNegativeInfinityEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 0, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Subtract(t));
            AssertVector(v, 0, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });
        }

        // V19.E.NegInf.Dot: Calling Dot on [0, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NegativeInfinity)] throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void Dot_TraversalTailNonFiniteNegativeInfinityEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 0, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.Dot(t));
            AssertVector(v, 0, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });
        }

        // V19.E.NegInf.ApproximatelyEquals: Calling ApproximatelyEquals on [0, 0, 0] with a dimension 3 traversal [(0,1),(2,double.NegativeInfinity)] at tolerance 0 throws ArgumentException and preserves the receiver and supplied traversal.
        [TestMethod]
        public void ApproximatelyEquals_TraversalTailNonFiniteNegativeInfinityEmptyReceiver_ThrowsArgumentException()
        {
            // Arrange
            var v = new SparseVector(new double[] { 0, 0, 0 });
            var t = new ScriptedSparseVector(3, new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => v.ApproximatelyEquals(t, 0));
            AssertVector(v, 0, 0, 0);
            Assert.AreEqual(3, t.Dimension);
            Assert.AreEqual(2, t.NonZeroCount);
            AssertEntries(t.NonZeroEntries(), new SparseEntry[] { new SparseEntry(0, 1), new SparseEntry(2, double.NegativeInfinity) });
        }

        // X03: Comparing [1] with null using Equals returns false and leaves [1] unchanged.
        [TestMethod]
        public void Equals_Null_ReturnsFalse()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });

            // Act
            bool actual = a.Equals(null);

            // Assert
            Assert.IsFalse(actual);
            AssertVector(a, 1);
        }

        // X04: Comparing [1] with a ScriptedSparseVector of dimension 1 and entries [(0,1)] using Equals returns false and leaves [1] unchanged.
        [TestMethod]
        public void Equals_OtherVectorType_ReturnsFalse()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1 });
            var t = new ScriptedSparseVector(1, new SparseEntry[] { new SparseEntry(0, 1) });

            // Act
            bool actual = a.Equals(t);

            // Assert
            Assert.IsFalse(actual);
            AssertVector(a, 1);
        }

        // X05: Reading index -1 of [1, 2] throws ArgumentOutOfRangeException and leaves [1, 2] unchanged.
        [TestMethod]
        public void Get_NegativeIndex_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 2 });

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.Get(-1));
            AssertVector(a, 1, 2);
        }

        // X06: Setting index -1 of [1, 2] to 5 throws ArgumentOutOfRangeException and leaves [1, 2] unchanged.
        [TestMethod]
        public void Set_NegativeIndex_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var a = new SparseVector(new double[] { 1, 2 });

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.Set(-1, 5));
            AssertVector(a, 1, 2);
        }

        // X07: Constructing a vector with dimension -1 throws ArgumentOutOfRangeException.
        [TestMethod]
        public void Constructor_NegativeDimension_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int dimension = -1;

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SparseVector(dimension));
        }

        private static void AssertVector(ISparseVector actual, params double[] expected)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Length, actual.Dimension);
            int count = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual.Get(i), "Coordinate " + i);
                if (expected[i] != 0)
                {
                    Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected[i]),
                        BitConverter.DoubleToInt64Bits(actual.Get(i)), "Coordinate bits " + i);
                    count++;
                }
            }

            Assert.AreEqual(count, actual.NonZeroCount);
            var entries = new SparseEntry[count];
            int position = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != 0)
                {
                    entries[position++] = new SparseEntry(i, expected[i]);
                }
            }

            AssertEntries(actual.NonZeroEntries(), entries);
        }

        private static void AssertEntries(IEnumerable<SparseEntry> actual, SparseEntry[] expected)
        {
            Assert.IsNotNull(actual);
            int position = 0;
            foreach (SparseEntry entry in actual)
            {
                Assert.IsTrue(position < expected.Length, "Unexpected extra entry.");
                Assert.AreEqual(expected[position].Index, entry.Index, "Entry index " + position);
                Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected[position].Value),
                    BitConverter.DoubleToInt64Bits(entry.Value), "Entry value bits " + position);
                position++;
            }

            Assert.AreEqual(expected.Length, position, "Traversal length.");
        }
    }
}
