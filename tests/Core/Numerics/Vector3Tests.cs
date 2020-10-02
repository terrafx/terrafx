using NUnit.Framework;
using TerraFX.Numerics;

namespace TerraFX.UnitTests.Numerics
{
    /// <summary>Unit tests for <see cref="Vector3"/>.</summary>
    public class Vector3Tests
    {
        /// <summary>Ensures that two vectors that are expected to compare equal do, in fact compare equal.</summary>
        [Test]
        public static void VectorsCompareEqual()
        {
            Assert.True(new Vector3(0,0,0) == new Vector3(0,0,0));
            Assert.False(new Vector3(0,0,0) == new Vector3(1,1,1));
        }

        /// <summary>Ensures that two vectors that are expected to compare not equal do, in fact compare not equal.</summary>
        [Test]
        public static void VectorsCompareNotEqual()
        {
            Assert.False(new Vector3(0,0,0) != new Vector3(0,0,0));
            Assert.True(new Vector3(0,0,0) != new Vector3(1,1,1));
        }

        /// <summary>Ensures that <see cref="Vector3.operator+(Vector3)"/> returns its input unchanced.</summary>
        [Test]
        public static void UnaryPlusReturnsUnchanged()
        {
            Assert.True(+Vector3.Zero == Vector3.Zero);
        }

        /// <summary>Ensures that <see cref="Vector3.operator-(Vector3)"/> returns the negation of its input.</summary>
        [Test]
        public static void UnaryNegationReturnsNegative()
        {
            var minusOne = -new Vector3(1,1,1);

            Assert.AreEqual(-1, minusOne.X);
            Assert.AreEqual(-1, minusOne.Y);
            Assert.AreEqual(-1, minusOne.Z);
        }

        /// <summary>Ensures that <see cref="Vector3.operator+(Vector3,Vector3)"/> returns the vector sum of its components.</summary>
        [Test]
        public static void AdditionReturnsSumOfValues()
        {
            var vector = new Vector3(1, 2, 3) + new Vector3(1, 2, 3);

            Assert.AreEqual(2, vector.X);
            Assert.AreEqual(4, vector.Y);
            Assert.AreEqual(6, vector.Z);
        }

        /// <summary>Ensures that <see cref="Vector3.operator-(Vector3,Vector3)"/> returns the vector difference of its components.</summary>
        [Test]
        public static void SubtractionReturnsDifferenceOfValues()
        {
            var vector = new Vector3(3, 2, 1) - new Vector3(1, 2, 3);

            Assert.AreEqual(2, vector.X);
            Assert.AreEqual(0, vector.Y);
            Assert.AreEqual(-2, vector.Z);
        }

        /// <summary>Ensures that <see cref="Vector3.operator*(Vector3,Vector3)"/> returns the vector product of its components.</summary>
        [Test]
        public static void VectorMultiplicationReturnsProductOfValues()
        {
            var vector = new Vector3(1, 2, 3) * new Vector3(3, 2, 1);

            Assert.AreEqual(3, vector.X);
            Assert.AreEqual(4, vector.Y);
            Assert.AreEqual(3, vector.Z);
        }

        /// <summary>Ensures that <see cref="Vector3.operator/(Vector3,Vector3)"/> returns the vector division of its components.</summary>
        [Test]
        public static void VectorDivisionReturnsDivisionOfValues()
        {
            var vector = new Vector3(6, 12, 18) / new Vector3(3, 6, 9);

            Assert.AreEqual(2, vector.X);
            Assert.AreEqual(2, vector.Y);
            Assert.AreEqual(2, vector.Z);
        }

        /// <summary>Ensures that <see cref="Vector3.operator*(Vector3,float)"/> returns a vector with each component multiplied by the given float.</summary>
        [Test]
        public static void ScalarMultiplicationReturnValuesScaled()
        {
            var vector = new Vector3(1, 2, 3) * 5;

            Assert.AreEqual(5, vector.X);
            Assert.AreEqual(10, vector.Y);
            Assert.AreEqual(15, vector.Z);
        }

        /// <summary>Ensures that <see cref="Vector3.operator/(Vector3,float)"/> returns a vector with each component divided by the given float.</summary>
        [Test]
        public static void ScalarDivisionReturnsValuesScaled()
        {
            var vector = new Vector3(5, 10, 15) / 5;

            Assert.AreEqual(1, vector.X);
            Assert.AreEqual(2, vector.Y);
            Assert.AreEqual(3, vector.Z);
        }

        /// <summary>Ensures that <see cref="Vector3.Cross(Vector3,Vector3)"/> returns a vector which is perpendicular to both input vectors.</summary>
        [Test]
        public static void CrossProductReturnsPerpendicularVector()
        {
            var vector = Vector3.Cross(new Vector3(1, 0, 0), new Vector3(0, 0, 1));

            Assert.AreEqual(0, vector.X);
            Assert.AreEqual(-1, vector.Y);
            Assert.AreEqual(0, vector.Z);
        }

        /// <summary>Ensures that <see cref="Vector3.Dot(Vector3,Vector3)"/> returns the scalar product of both input vectors.</summary>
        [Test]
        public static void DotProductReturnsScalarProduct()
        {
            var product = Vector3.Dot(new Vector3(1, 0.5f, 0), new Vector3(2, 1, 0));

            Assert.AreEqual(2.5f, product);
        }

        /// <summary>Ensures that <see cref="Vector3.Normalize(Vector3)"/> returns a unit vector.</summary>
        [Test]
        public static void NormalizeReturnsUnitVector()
        {
            Assert.AreEqual(new Vector3(0, 1, 0), Vector3.Normalize(new Vector3(0, 2, 0)));
            Assert.AreEqual(new Vector3(1, 0, 0), Vector3.Normalize(new Vector3(1, 0, 0)));
            Assert.AreEqual(new Vector3(0, 0, 1), Vector3.Normalize(new Vector3(0, 0, 5)));
        }

        /// <summary>Ensures that <see cref="Vector3.Length"/> and <see cref="Vector3.LengthSquared"/> return the magnitude and squared magnitude of the input vector.</summary>
        [Test]
        public static void LengthReturnsMagnitudeOfVector()
        {
            var vector = new Vector3(0, 5, 0);

            Assert.AreEqual(5, vector.Length);
            Assert.AreEqual(25, vector.LengthSquared);
        }
    }
}
