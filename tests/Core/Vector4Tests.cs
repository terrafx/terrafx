using NUnit.Framework;
using TerraFX.Numerics;

namespace TerraFX.UnitTests.Numerics
{
    /// <summary>Unit tests for <see cref="Vector4"/>.</summary>
    public class Vector4Tests
    {
        /// <summary>Ensures that two vectors that are expected to compare equal do, in fact compare equal.</summary>
        [Test]
        public static void VectorsCompareEqual()
        {
            Assert.True(new Vector4(0, 0, 0, 0) == new Vector4(0, 0, 0, 0));
            Assert.False(new Vector4(0, 0, 0, 0) == new Vector4(1, 1, 1, 1));
        }

        /// <summary>Ensures that two vectors that are expected to compare not equal do, in fact compare not equal.</summary>
        [Test]
        public static void VectorsCompareNotEqual()
        {
            Assert.False(new Vector4(0, 0, 0, 0) != new Vector4(0, 0, 0, 0));
            Assert.True(new Vector4(0, 0, 0, 0) != new Vector4(1, 1, 1, 1));
        }

        /// <summary>Ensures that <see cref="Vector4.operator+(Vector4)"/> returns its input unchanced.</summary>
        [Test]
        public static void UnaryPlusReturnsUnchanged()
        {
            Assert.True(+Vector4.Zero == Vector4.Zero);
        }

        /// <summary>Ensures that <see cref="Vector4.operator-(Vector4)"/> returns the negation of its input.</summary>
        [Test]
        public static void UnaryNegationReturnsNegative()
        {
            var minusOne = -new Vector4(1, 1, 1, 1);

            Assert.AreEqual(-1, minusOne.X);
            Assert.AreEqual(-1, minusOne.Y);
            Assert.AreEqual(-1, minusOne.Z);
            Assert.AreEqual(-1, minusOne.W);
        }

        /// <summary>Ensures that <see cref="Vector4.operator+(Vector4,Vector4)"/> returns the vector sum of its components.</summary>
        [Test]
        public static void AdditionReturnsSumOfValues()
        {
            var vector = new Vector4(1, 2, 3, 4) + new Vector4(1, 2, 3, 4);

            Assert.AreEqual(2, vector.X);
            Assert.AreEqual(4, vector.Y);
            Assert.AreEqual(6, vector.Z);
            Assert.AreEqual(8, vector.W);
        }

        /// <summary>Ensures that <see cref="Vector4.operator-(Vector4,Vector4)"/> returns the vector difference of its components.</summary>
        [Test]
        public static void SubtractionReturnsDifferenceOfValues()
        {
            var vector = new Vector4(3, 2, 1, 0) - new Vector4(1, 2, 3, 4);

            Assert.AreEqual(2, vector.X);
            Assert.AreEqual(0, vector.Y);
            Assert.AreEqual(-2, vector.Z);
            Assert.AreEqual(-4, vector.W);
        }

        /// <summary>Ensures that <see cref="Vector4.operator*(Vector4,Vector4)"/> returns the vector product of its components.</summary>
        [Test]
        public static void VectorMultiplicationReturnsProductOfValues()
        {
            var vector = new Vector4(1, 2, 3, 4) * new Vector4(3, 2, 1, 0);

            Assert.AreEqual(3, vector.X);
            Assert.AreEqual(4, vector.Y);
            Assert.AreEqual(3, vector.Z);
            Assert.AreEqual(0, vector.W);
        }

        /// <summary>Ensures that <see cref="Vector4.operator/(Vector4,Vector4)"/> returns the vector division of its components.</summary>
        [Test]
        public static void VectorDivisionReturnsDivisionOfValues()
        {
            var vector = new Vector4(6, 12, 18, 24) / new Vector4(3, 6, 9, 12);

            Assert.AreEqual(2, vector.X);
            Assert.AreEqual(2, vector.Y);
            Assert.AreEqual(2, vector.Z);
            Assert.AreEqual(2, vector.W);
        }

        /// <summary>Ensures that <see cref="Vector4.operator*(Vector4,float)"/> returns a vector with each component multiplied by the given float.</summary>
        [Test]
        public static void ScalarMultiplicationReturnValuesScaled()
        {
            var vector = new Vector4(1, 2, 3, 4) * 5;

            Assert.AreEqual(5, vector.X);
            Assert.AreEqual(10, vector.Y);
            Assert.AreEqual(15, vector.Z);
            Assert.AreEqual(20, vector.W);
        }

        /// <summary>Ensures that <see cref="Vector4.operator/(Vector4,float)"/> returns a vector with each component divided by the given float.</summary>
        [Test]
        public static void ScalarDivisionReturnsValuesScaled()
        {
            var vector = new Vector4(5, 10, 15, 20) / 5;

            Assert.AreEqual(1, vector.X);
            Assert.AreEqual(2, vector.Y);
            Assert.AreEqual(3, vector.Z);
            Assert.AreEqual(4, vector.W);
        }

        /// <summary>Ensures that <see cref="Vector4.Dot(Vector4,Vector4)"/> returns the scalar product of both input vectors.</summary>
        [Test]
        public static void DotProductReturnsScalarProduct()
        {
            var product = Vector4.Dot(new Vector4(1, 0.5f, 0, 0.25f), new Vector4(2, 1, 0, 2.0f));

            Assert.AreEqual(3.0f, product);
        }

        /// <summary>Ensures that <see cref="Vector4.Normalize(Vector4)"/> returns a unit vector.</summary>
        [Test]
        public static void NormalizeReturnsUnitVector()
        {
            Assert.AreEqual(new Vector4(1, 0, 0, 0), Vector4.Normalize(new Vector4(1, 0, 0, 0)));
            Assert.AreEqual(new Vector4(0, 1, 0, 0), Vector4.Normalize(new Vector4(0, 2, 0, 0)));
            Assert.AreEqual(new Vector4(0, 0, 1, 0), Vector4.Normalize(new Vector4(0, 0, 5, 0)));
            Assert.AreEqual(new Vector4(0, 0, 0, 1), Vector4.Normalize(new Vector4(0, 0, 0, 8)));
        }

        /// <summary>Ensures that <see cref="Vector4.Length"/> and <see cref="Vector4.LengthSquared"/> return the magnitude and squared magnitude of the input vector.</summary>
        [Test]
        public static void LengthReturnsMagnitudeOfVector()
        {
            var vector = new Vector4(0, 5, 0, 0);

            Assert.AreEqual(5, vector.Length);
            Assert.AreEqual(25, vector.LengthSquared);
        }
    }
}
