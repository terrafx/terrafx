using NUnit.Framework;
using TerraFX.Numerics;

namespace TerraFX.UnitTests.Numerics
{
    /// <summary>Unit tests for <see cref="Vector3" />.</summary>
    public class Vector3Tests
    {
        /// <summary>Ensures that a vector's components are equal to the parameters used to construct one.</summary>
        [Test]
        public static void ComponentsReturnCorrectValues(
            [Values(1, 2, 3)] float x,
            [Values(1, 2, 3)] float y,
            [Values(1, 2, 3)] float z)
        {
            var vector = new Vector3(x, y, z);

            Assert.That(vector.X, Is.EqualTo(x));
            Assert.That(vector.Y, Is.EqualTo(y));
            Assert.That(vector.Z, Is.EqualTo(z));
        }

        /// <summary>Ensures that two vectors that are expected to compare equal do, in fact compare equal.</summary>
        [Test]
        public static void VectorsCompareEqual()
        {
            Assert.That(new Vector3(0, 0, 0) == new Vector3(0, 0, 0), Is.True);
            Assert.That(new Vector3(0, 0, 0) == new Vector3(1, 1, 1), Is.False);
        }

        /// <summary>Ensures that two vectors that are expected to compare not equal do, in fact compare not equal.</summary>
        [Test]
        public static void VectorsCompareNotEqual()
        {
            Assert.That(new Vector3(0, 0, 0) != new Vector3(0, 0, 0), Is.False);
            Assert.That(new Vector3(0, 0, 0) != new Vector3(1, 1, 1), Is.True);
        }

        /// <summary>Ensures that <see cref="Vector3.operator+(Vector3)" /> returns its input unchanced.</summary>
        [Test]
        public static void UnaryPlusReturnsUnchanged() => Assert.That(+Vector3.Zero == Vector3.Zero, Is.True);

        /// <summary>Ensures that <see cref="Vector3.operator-(Vector3)" /> returns the negation of its input.</summary>
        [Test]
        public static void UnaryNegationReturnsNegative()
        {
            var vector = -new Vector3(1, 1, 1);

            Assert.That(vector, Is.EqualTo(new Vector3(-1, -1, -1)));
        }

        /// <summary>Ensures that <see cref="Vector3.operator+(Vector3,Vector3)" /> returns the vector sum of its components.</summary>
        [Test]
        public static void AdditionReturnsSumOfValues()
        {
            var vector = new Vector3(1, 2, 3) + new Vector3(1, 2, 3);

            Assert.That(vector, Is.EqualTo(new Vector3(2, 4, 6)));
        }

        /// <summary>Ensures that <see cref="Vector3.operator-(Vector3,Vector3)" /> returns the vector difference of its components.</summary>
        [Test]
        public static void SubtractionReturnsDifferenceOfValues()
        {
            var vector = new Vector3(3, 2, 1) - new Vector3(1, 2, 3);

            Assert.That(vector, Is.EqualTo(new Vector3(2, 0, -2)));
        }

        /// <summary>Ensures that <see cref="Vector3.operator*(Vector3,Vector3)" /> returns the vector product of its components.</summary>
        [Test]
        public static void VectorMultiplicationReturnsProductOfValues()
        {
            var vector = new Vector3(1, 2, 3) * new Vector3(3, 2, 1);

            Assert.That(vector, Is.EqualTo(new Vector3(3, 4, 3)));
        }

        /// <summary>Ensures that <see cref="Vector3.operator/(Vector3,Vector3)" /> returns the vector division of its components.</summary>
        [Test]
        public static void VectorDivisionReturnsDivisionOfValues()
        {
            var vector = new Vector3(6, 12, 18) / new Vector3(3, 6, 9);

            Assert.That(vector, Is.EqualTo(new Vector3(2, 2, 2)));
        }

        /// <summary>Ensures that <see cref="Vector3.operator*(Vector3,float)" /> returns a vector with each component multiplied by the given float.</summary>
        [Test]
        public static void ScalarMultiplicationReturnValuesScaled()
        {
            var vector = new Vector3(1, 2, 3) * 5;

            Assert.That(vector, Is.EqualTo(new Vector3(5, 10, 15)));
        }

        /// <summary>Ensures that <see cref="Vector3.operator/(Vector3,float)" /> returns a vector with each component divided by the given float.</summary>
        [Test]
        public static void ScalarDivisionReturnsValuesScaled()
        {
            var vector = new Vector3(5, 10, 15) / 5;

            Assert.That(vector, Is.EqualTo(new Vector3(1, 2, 3)));
        }

        /// <summary>Ensures that <see cref="Vector3.Cross(Vector3,Vector3)" /> returns a vector which is perpendicular to both input vectors.</summary>
        [Test]
        public static void CrossProductReturnsPerpendicularVector()
        {
            var vector = Vector3.Cross(new Vector3(1, 0, 0), new Vector3(0, 0, 1));

            Assert.That(vector, Is.EqualTo(new Vector3(0, -1, 0)));
        }

        /// <summary>Ensures that <see cref="Vector3.Dot(Vector3,Vector3)" /> returns the scalar product of both input vectors.</summary>
        [Test]
        public static void DotProductReturnsScalarProduct()
        {
            var product = Vector3.Dot(new Vector3(1, 0.5f, 0), new Vector3(2, 1, 0));

            Assert.That(product, Is.EqualTo(2.5f));
        }

        /// <summary>Ensures that <see cref="Vector3.Normalize(Vector3)" /> returns a unit vector.</summary>
        [Test]
        public static void NormalizeReturnsUnitVector()
        {
            Assert.That(Vector3.Normalize(new Vector3(1, 0, 0)), Is.EqualTo(new Vector3(1, 0, 0)));
            Assert.That(Vector3.Normalize(new Vector3(0, 2, 0)), Is.EqualTo(new Vector3(0, 1, 0)));
            Assert.That(Vector3.Normalize(new Vector3(0, 0, 5)), Is.EqualTo(new Vector3(0, 0, 1)));
        }

        /// <summary>Ensures that <see cref="Vector3.Length" /> and <see cref="Vector3.LengthSquared" /> return the magnitude and squared magnitude of the input vector.</summary>
        [Test]
        public static void LengthReturnsMagnitudeOfVector()
        {
            var vector = new Vector3(0, 5, 0);

            Assert.That(vector.Length, Is.EqualTo(5));
            Assert.That(vector.LengthSquared, Is.EqualTo(25));
        }

        /// <summary>Ensures that <see cref="Vector3" /> and <see cref="Matrix3x3" /> multiply properly.</summary>
        [Test]
        public static void VectorMatrix3x3Multiplication()
        {
            var v = new Vector3(1.0f, 2.0f, 3.0f);
            var mIdentity = Matrix3x3.Identity;
            var a = v * mIdentity;
            var b = v;
            b *= mIdentity;
            b *= mIdentity;

            Assert.That(a, Is.EqualTo(v));
            Assert.That(b, Is.EqualTo(v));

            var x = new Vector3(1.0f, 0.0f, 0.0f);
            var y = new Vector3(0.0f, 1.0f, 0.0f);
            var z = new Vector3(0.0f, 0.0f, 1.0f);
            var mX90 = new Matrix3x3(
                new Vector3(1.0f, 0.0f, 0.0f),
                new Vector3(0.0f, 0.0f, 1.0f),
                new Vector3(0.0f, -1.0f, 0.0f));

            var mY90 = new Matrix3x3(
                new Vector3(0.0f, 0.0f, 1.0f),
                new Vector3(0.0f, 1.0f, 0.0f),
                new Vector3(-1.0f, 0.0f, 0.0f));

            var mZ90 = new Matrix3x3(
                new Vector3(0.0f, 1.0f, 0.0f),
                new Vector3(-1.0f, 0.0f, 0.0f),
                new Vector3(0.0f, 0.0f, 1.0f));

            var i = x * mZ90;
            Assert.That(i, Is.EqualTo(y));


            var j = x * mY90;
            Assert.That(j, Is.EqualTo(z));

            var k = y * mX90;
            Assert.That(k, Is.EqualTo(z));
        }

        /// <summary>Ensures that <see cref="Vector3" /> and <see cref="Matrix4x4" /> multiply properly.</summary>
        [Test]
        public static void VectorMatrix4x4Multiplication()
        {
            var v = new Vector3(1.0f, 2.0f, 3.0f);
            var mIdentity = Matrix4x4.Identity;
            var a = Vector3.Transform(v, mIdentity);
            var b = Vector3.Transform(Vector3.Transform(v, mIdentity), mIdentity);

            Assert.That(a, Is.EqualTo(v));
            Assert.That(b, Is.EqualTo(v));

            var x = new Vector3(1.0f, 0.0f, 0.0f);
            var y = new Vector3(0.0f, 1.0f, 0.0f);
            var z = new Vector3(0.0f, 0.0f, 1.0f);

            var mX90 = new Matrix4x4(
                new Vector4(1.0f, 0.0f, 0.0f, 0.0f),
                new Vector4(0.0f, 0.0f, 1.0f, 0.0f),
                new Vector4(0.0f, -1.0f, 0.0f, 0.0f),
                new Vector4(-1.0f, 0.0f, 0.0f, 0.0f));

            var mY90 = new Matrix4x4(
                new Vector4(0.0f, 0.0f, 1.0f, 0.0f),
                new Vector4(0.0f, 1.0f, 0.0f, 0.0f),
                new Vector4(-1.0f, 0.0f, 0.0f, 0.0f),
                new Vector4(-1.0f, 0.0f, 0.0f, 0.0f));

            var mZ90 = new Matrix4x4(
                new Vector4(0.0f, 1.0f, 0.0f, 0.0f),
                new Vector4(-1.0f, 0.0f, 0.0f, 0.0f),
                new Vector4(0.0f, 0.0f, 1.0f, 0.0f),
                new Vector4(-1.0f, 0.0f, 0.0f, 0.0f));

            var i = Vector3.TransformNormal(x, mZ90);
            Assert.That(i, Is.EqualTo(y));

            var j = Vector3.TransformNormal(x, mY90);
            Assert.That(j, Is.EqualTo(z));

            var k = Vector3.TransformNormal(y, mX90);
            Assert.That(k, Is.EqualTo(z));
        }
    }
}
