using NUnit.Framework;
using TerraFX.Numerics;

namespace TerraFX.UnitTests.Numerics
{
    /// <summary>Unit tests for <see cref="Vector4" />.</summary>
    public class Vector4Tests
    {
        /// <summary>Ensures that a vector's components are equal to the parameters used to construct one.</summary>
        [Test]
        public static void ComponentsReturnCorrectValues(
            [Values(1, 2, 3)] float x,
            [Values(1, 2, 3)] float y,
            [Values(1, 2, 3)] float z,
            [Values(1, 2, 3)] float w)
        {
            var vector = new Vector4(x, y, z, w);

            Assert.That(vector.X, Is.EqualTo(x));
            Assert.That(vector.Y, Is.EqualTo(y));
            Assert.That(vector.Z, Is.EqualTo(z));
            Assert.That(vector.W, Is.EqualTo(w));
        }

        /// <summary>Ensures that two vectors that are expected to compare equal do, in fact compare equal.</summary>
        [Test]
        public static void VectorsCompareEqual()
        {
            Assert.That(new Vector4(0, 0, 0, 0) == new Vector4(0, 0, 0, 0), Is.True);
            Assert.That(new Vector4(0, 0, 0, 0) == new Vector4(1, 1, 1, 1), Is.False);
        }

        /// <summary>Ensures that two vectors that are expected to compare not equal do, in fact compare not equal.</summary>
        [Test]
        public static void VectorsCompareNotEqual()
        {
            Assert.That(new Vector4(0, 0, 0, 0) != new Vector4(0, 0, 0, 0), Is.False);
            Assert.That(new Vector4(0, 0, 0, 0) != new Vector4(1, 1, 1, 1), Is.True);
        }

        /// <summary>Ensures that <see cref="Vector4.operator+(Vector4)" /> returns its input unchanced.</summary>
        [Test]
        public static void UnaryPlusReturnsUnchanged() => Assert.That(+Vector4.Zero == Vector4.Zero, Is.True);

        /// <summary>Ensures that <see cref="Vector4.operator-(Vector4)" /> returns the negation of its input.</summary>
        [Test]
        public static void UnaryNegationReturnsNegative()
        {
            var vector = -new Vector4(1, 1, 1, 1);

            Assert.That(vector, Is.EqualTo(new Vector4(-1, -1, -1, -1)));
        }

        /// <summary>Ensures that <see cref="Vector4.operator+(Vector4,Vector4)" /> returns the vector sum of its components.</summary>
        [Test]
        public static void AdditionReturnsSumOfValues()
        {
            var vector = new Vector4(1, 2, 3, 4) + new Vector4(1, 2, 3, 4);

            Assert.That(vector, Is.EqualTo(new Vector4(2, 4, 6, 8)));
        }

        /// <summary>Ensures that <see cref="Vector4.operator-(Vector4,Vector4)" /> returns the vector difference of its components.</summary>
        [Test]
        public static void SubtractionReturnsDifferenceOfValues()
        {
            var vector = new Vector4(4, 3, 2, 1) - new Vector4(1, 2, 3, 4);

            Assert.That(vector, Is.EqualTo(new Vector4(3, 1, -1, -3)));
        }

        /// <summary>Ensures that <see cref="Vector4.operator*(Vector4,Vector4)" /> returns the vector product of its components.</summary>
        [Test]
        public static void VectorMultiplicationReturnsProductOfValues()
        {
            var vector = new Vector4(1, 2, 3, 4) * new Vector4(4, 3, 2, 1);

            Assert.That(vector, Is.EqualTo(new Vector4(4, 6, 6, 4)));
        }

        /// <summary>Ensures that <see cref="Vector4.operator/(Vector4,Vector4)" /> returns the vector division of its components.</summary>
        [Test]
        public static void VectorDivisionReturnsDivisionOfValues()
        {
            var vector = new Vector4(6, 12, 18, 24) / new Vector4(3, 6, 9, 12);

            Assert.That(vector, Is.EqualTo(new Vector4(2, 2, 2, 2)));
        }

        /// <summary>Ensures that <see cref="Vector4.operator*(Vector4,float)" /> returns a vector with each component multiplied by the given float.</summary>
        [Test]
        public static void ScalarMultiplicationReturnValuesScaled()
        {
            var vector = new Vector4(1, 2, 3, 4) * 5;

            Assert.That(vector, Is.EqualTo(new Vector4(5, 10, 15, 20)));
        }

        /// <summary>Ensures that <see cref="Vector4.operator/(Vector4,float)" /> returns a vector with each component divided by the given float.</summary>
        [Test]
        public static void ScalarDivisionReturnsValuesScaled()
        {
            var vector = new Vector4(5, 10, 15, 20) / 5;

            Assert.That(vector, Is.EqualTo(new Vector4(1, 2, 3, 4)));
        }

        /// <summary>Ensures that <see cref="Vector4.Dot(Vector4,Vector4)" /> returns the scalar product of both input vectors.</summary>
        [Test]
        public static void DotProductReturnsScalarProduct()
        {
            var product = Vector4.Dot(new Vector4(1, 0.5f, 0, 0.25f), new Vector4(2, 1, 0, 2.0f));

            Assert.That(product, Is.EqualTo(3.0f));
        }

        /// <summary>Ensures that <see cref="Vector4.Normalize(Vector4)" /> returns a unit vector.</summary>
        [Test]
        public static void NormalizeReturnsUnitVector()
        {
            Assert.That(Vector4.Normalize(new Vector4(1, 0, 0, 0)), Is.EqualTo(new Vector4(1, 0, 0, 0)));
            Assert.That(Vector4.Normalize(new Vector4(0, 2, 0, 0)), Is.EqualTo(new Vector4(0, 1, 0, 0)));
            Assert.That(Vector4.Normalize(new Vector4(0, 0, 3, 0)), Is.EqualTo(new Vector4(0, 0, 1, 0)));
            Assert.That(Vector4.Normalize(new Vector4(0, 0, 0, 5)), Is.EqualTo(new Vector4(0, 0, 0, 1)));
        }

        /// <summary>Ensures that <see cref="Vector4.Length" /> and <see cref="Vector4.LengthSquared" /> return the magnitude and squared magnitude of the input vector.</summary>
        [Test]
        public static void LengthReturnsMagnitudeOfVector()
        {
            var vector = new Vector4(0, 5, 0, 0);

            Assert.That(vector.Length, Is.EqualTo(5));
            Assert.That(vector.LengthSquared, Is.EqualTo(25));
        }
    }
}
