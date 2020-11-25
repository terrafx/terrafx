using NUnit.Framework;
using TerraFX.Numerics;

namespace TerraFX.UnitTests.Numerics
{
    /// <summary>Unit tests for <see cref="Vector2"/>.</summary>
    public class Vector2Tests
    {
        /// <summary>Ensures that a vector's components are equal to the parameters used to construct one.</summary>
        [Test]
        public static void ComponentsReturnCorrectValues(
            [Values(1, 2, 3)] float x,
            [Values(1, 2, 3)] float y)
        {
            var vector = new Vector2(x, y);

            Assert.That(vector.X, Is.EqualTo(x));
            Assert.That(vector.Y, Is.EqualTo(y));
        }

        /// <summary>Ensures that two vectors that are expected to compare equal do, in fact compare equal.</summary>
        [Test]
        public static void VectorsCompareEqual()
        {
            Assert.That(new Vector2(0, 0) == new Vector2(0, 0), Is.True);
            Assert.That(new Vector2(0, 0) == new Vector2(1, 1), Is.False);
        }

        /// <summary>Ensures that two vectors that are expected to compare not equal do, in fact compare not equal.</summary>
        [Test]
        public static void VectorsCompareNotEqual()
        {
            Assert.That(new Vector2(0, 0) != new Vector2(0, 0), Is.False);
            Assert.That(new Vector2(0, 0) != new Vector2(1, 1), Is.True);
        }

        /// <summary>Ensures that <see cref="Vector2.operator+(Vector2)"/> returns its input unchanced.</summary>
        [Test]
        public static void UnaryPlusReturnsUnchanged() => Assert.That(+Vector2.Zero == Vector2.Zero, Is.True);

        /// <summary>Ensures that <see cref="Vector2.operator-(Vector2)"/> returns the negation of its input.</summary>
        [Test]
        public static void UnaryNegationReturnsNegative()
        {
            var vector = -new Vector2(1, 1);

            Assert.That(vector, Is.EqualTo(new Vector2(-1, -1)));
        }

        /// <summary>Ensures that <see cref="Vector2.operator+(Vector2,Vector2)"/> returns the vector sum of its components.</summary>
        [Test]
        public static void AdditionReturnsSumOfValues()
        {
            var vector = new Vector2(1, 2) + new Vector2(1, 2);

            Assert.That(vector, Is.EqualTo(new Vector2(2, 4)));
        }

        /// <summary>Ensures that <see cref="Vector2.operator-(Vector2,Vector2)"/> returns the vector difference of its components.</summary>
        [Test]
        public static void SubtractionReturnsDifferenceOfValues()
        {
            var vector = new Vector2(3, 2) - new Vector2(1, 2);

            Assert.That(vector, Is.EqualTo(new Vector2(2, 0)));
        }

        /// <summary>Ensures that <see cref="Vector2.operator*(Vector2,Vector2)"/> returns the vector product of its components.</summary>
        [Test]
        public static void VectorMultiplicationReturnsProductOfValues()
        {
            var vector = new Vector2(1, 2) * new Vector2(3, 2);

            Assert.That(vector, Is.EqualTo(new Vector2(3, 4)));
        }

        /// <summary>Ensures that <see cref="Vector2.operator/(Vector2,Vector2)"/> returns the vector division of its components.</summary>
        [Test]
        public static void VectorDivisionReturnsDivisionOfValues()
        {
            var vector = new Vector2(6, 12) / new Vector2(3, 6);

            Assert.That(vector, Is.EqualTo(new Vector2(2, 2)));
        }

        /// <summary>Ensures that <see cref="Vector2.operator*(Vector2,float)"/> returns a vector with each component multiplied by the given float.</summary>
        [Test]
        public static void ScalarMultiplicationReturnValuesScaled()
        {
            var vector = new Vector2(1, 2) * 5;

            Assert.That(vector, Is.EqualTo(new Vector2(5, 10)));
        }

        /// <summary>Ensures that <see cref="Vector2.operator/(Vector2,float)"/> returns a vector with each component divided by the given float.</summary>
        [Test]
        public static void ScalarDivisionReturnsValuesScaled()
        {
            var vector = new Vector2(5, 10) / 5;

            Assert.That(vector, Is.EqualTo(new Vector2(1, 2)));
        }

        /// <summary>Ensures that <see cref="Vector2.Dot(Vector2,Vector2)"/> returns the scalar product of both input vectors.</summary>
        [Test]
        public static void DotProductReturnsScalarProduct()
        {
            var product = Vector2.Dot(new Vector2(1, 0.5f), new Vector2(2, 1));

            Assert.That(product, Is.EqualTo(2.5f));
        }

        /// <summary>Ensures that <see cref="Vector2.Normalize(Vector2)"/> returns a unit vector.</summary>
        [Test]
        public static void NormalizeReturnsUnitVector()
        {
            Assert.That(Vector2.Normalize(new Vector2(1, 0)), Is.EqualTo(new Vector2(1, 0)));
            Assert.That(Vector2.Normalize(new Vector2(0, 2)), Is.EqualTo(new Vector2(0, 1)));
        }

        /// <summary>Ensures that <see cref="Vector2.Length"/> and <see cref="Vector2.LengthSquared"/> return the magnitude and squared magnitude of the input vector.</summary>
        [Test]
        public static void LengthReturnsMagnitudeOfVector()
        {
            var vector = new Vector2(0, 5);

            Assert.That(vector.Length, Is.EqualTo(5));
            Assert.That(vector.LengthSquared, Is.EqualTo(25));
        }
    }
}
