using System;
using System.Collections.Generic;
using NUnit.Framework;
using TerraFX.Numerics;

namespace TerraFX.UnitTests.Numerics
{
    /// <summary>Unit tests for <see cref="Transform" />.</summary>
    public class TransformTests
    {

        private static IEnumerable<TestCaseData> TransformConstructorData
        {
            get
            {
                yield return new TestCaseData(new Quaternion(0, 0, 0, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0));
                yield return new TestCaseData(new Quaternion(1, 0, 0, 1), new Vector3(7, 8, 9), new Vector3(1, 2, 3));
            }
        }

        /// <summary>Ensures that a instance's components are equal to the parameters used to construct one.</summary>
        [Test, TestCaseSource(nameof(TransformConstructorData))]
        public static void ComponentsReturnCorrectValues(
            Quaternion rotation,
            Vector3 scale,
            Vector3 translation)
        {
            var transform = new Transform(rotation, scale, translation);

            Assert.That(transform.Translation, Is.EqualTo(translation));
            Assert.That(transform.Scale, Is.EqualTo(scale));
            Assert.That(transform.Rotation, Is.EqualTo(Quaternion.Normalize(rotation)));
        }

        /// <summary>Ensures that two instances that are expected to compare equal do, in fact compare equal.</summary>
        [Test, TestCaseSource(nameof(TransformConstructorData))]
        public static void TransformsCompareEqual(
            Quaternion rotation,
            Vector3 scale,
            Vector3 translation)
        {
            var a = new Transform(rotation, scale, translation);
            var b = new Transform(rotation, scale, translation);
            var c = new Transform(rotation.WithY(1), scale, translation);

            Assert.That(a == b, Is.True);
            Assert.That(a == c, Is.False);
        }

        /// <summary>Ensures that two instances that are expected to compare not equal do, in fact compare not equal.</summary>
        [Test, TestCaseSource(nameof(TransformConstructorData))]
        public static void TransformsCompareNotEqual(
            Quaternion rotation,
            Vector3 scale,
            Vector3 translation)
        {
            var a = new Transform(rotation, scale, translation);
            var b = new Transform(rotation, scale, translation);
            var c = new Transform(rotation.WithY(1), scale, translation);

            Assert.That(a != c, Is.True);
            Assert.That(a != b, Is.False);
        }

        /// <summary>Ensures that <see cref="Transform.operator+(Transform)" /> returns its input unchanged.</summary>
        [Test, TestCaseSource(nameof(TransformConstructorData))]
        public static void UnaryPlusReturnsUnchanged(
            Quaternion rotation,
            Vector3 scale,
            Vector3 translation)
        {
            var a = new Transform(rotation, scale, translation);

            Assert.That(a == +a, Is.True);
        }

        /// <summary>Ensures that <see cref="Transform.operator-(Transform)" /> returns the inverse of its input.</summary>
        [Test, TestCaseSource(nameof(TransformConstructorData))]
        public static void UnaryNegationReturnsNegative(
            Quaternion rotation,
            Vector3 scale,
            Vector3 translation)
        {
            var a = new Transform(rotation, scale, translation);
            var b = new Transform(-rotation, -scale, -translation);
            var epsilon = Transform.One * 1e-7f;

            Assert.That((-a).EqualEstimate(b, epsilon), Is.True);
        }

        /// <summary>Ensures that <see cref="Transform.operator+(Transform,Transform)" /> returns the concatenation of its components.</summary>
        [Test, TestCaseSource(nameof(TransformConstructorData))]
        public static void AdditionReturnsComponentSum(
            Quaternion rotation,
            Vector3 scale,
            Vector3 translation)
        {
            var a = new Transform(rotation, scale, translation);
            var b = new Transform(rotation.WithY(1), scale, translation);

            Assert.That(a + b == new Transform(a.Rotation + b.Rotation, a.Scale + b.Scale, a.Translation + b.Translation), Is.True);
        }

        /// <summary>Ensures that <see cref="Transform.operator-(Transform,Transform)" /> returns the concatenation of the inverse of the second component.</summary>
        [Test, TestCaseSource(nameof(TransformConstructorData))]
        public static void SubtractionReturnsComponentDifference(
            Quaternion rotation,
            Vector3 scale,
            Vector3 translation)
        {
            var a = new Transform(rotation, scale, translation);
            var b = new Transform(rotation.WithY(1), scale, translation);

            Assert.That(a - b == new Transform(a.Rotation - b.Rotation, a.Scale - b.Scale, a.Translation - b.Translation), Is.True);
        }

        /// <summary>Ensures that Inverse() returns the inverse.</summary>
        [Test]
        public static void InverseReturnsInverse()
        {
            var s05 = MathF.Sqrt(0.5f);
            var a = new Transform(new Quaternion(0, 0, 0, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0));
            var b = new Transform(new Quaternion(s05, 0, 0, s05), new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            var i = new Transform(new Quaternion(0, 0, 0, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0));
            var j = new Transform(new Quaternion(-s05, 0, 0, s05), new Vector3(1, 0.5f, 1 / 3f), new Vector3(-4, -5, -6));

            var epsilon = new Transform(Quaternion.One * 1e-7f, Vector3.One * 1e-7f, Vector3.One * 1e-7f);
            Assert.That(Transform.Inverse(a) == i, Is.True);
            Assert.That(Transform.Inverse(b).EqualEstimate(j, epsilon), Is.True);
            Assert.That(Transform.Inverse(Transform.Inverse(b)).EqualEstimate(b, epsilon), Is.True);
        }


        /// <summary>Ensures that Concatenate() returns the concatenation.</summary>
        [Test]
        public static void ConcatenateReturnsConcatenation()
        {
            var radians90 = 90f * MathF.PI / 180.0f;
            var radians45 = radians90 / 2.0f;
            var s05 = MathF.Sqrt(0.5f);
            var a = Transform.Identity;
            var b = a.WithScale(new Vector3(1, 2, 3));
            var c = a.WithRotation(new Quaternion(s05, 0, 0, s05));
            var d = a.WithTranslation(new Vector3(4, 5, 6));
            var e = Transform.AddRotationAroundSourceX(a, +radians90);
            var f = Transform.AddRotationAroundSourceX(a, -radians90);
            var g = Transform.AddRotationAroundSourceX(a, +radians45);
            var epsilon = new Transform(Quaternion.One * 1e-7f, Vector3.One * 1e-7f, Vector3.One * 1e-7f);

            Assert.That(Transform.Concatenate(a, a) == a, Is.True); // Identity + Identity = Identity
            Assert.That(Transform.Concatenate(a, b) == b, Is.True); // Identity + Scale = Scale
            Assert.That(Transform.Concatenate(a, c) == c, Is.True); // Identity + Rotation = Rotation
            Assert.That(Transform.Concatenate(a, d) == d, Is.True); // Identity + Translation = Translation
            Assert.That(Transform.Concatenate(a, e) == e, Is.True); // Identity + Rotation = Rotation
            Assert.That(Transform.Concatenate(a, f) == f, Is.True); // Identity + Rotation = Rotation
            Assert.That(Transform.Concatenate(e, f) == a, Is.True); // Rotation+90 + Rotation-90 = Identity
            Assert.That(Transform.Concatenate(g, g).EqualEstimate(e, epsilon), Is.True); // Rotation+45 + Rotation+45 = Rotation+90
        }

        /// <summary>Ensures that ToMatrix4x4() returns the matching Matrix4x4.</summary>
        [Test]
        public static void ToMatrix4x4ReturnsMatrix4x4()
        {
            var radians90 = 90f * MathF.PI / 180.0f;
            var a = new Transform(Quaternion.Identity, Vector3.One, Vector3.Zero);
            var b = new Transform(new Quaternion(1f, 0, 0, 1f), new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var mb = new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, 0, 2, 0), new Vector4(0, -3, 0, 0), new Vector4(4, 5, 6, 1));
            var c = new Transform(Quaternion.Identity, Vector3.One, new Vector3(1, 2, 3));
            c = Transform.AddRotationAroundSourceX(c, radians90);
            var mc = new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, -1, 0, 0), new Vector4(1, 2, 3, 1));
            var epsilon = Matrix4x4.One * 1e-6f;

            Assert.That(Transform.ToMatrix4x4(a) == Matrix4x4.Identity, Is.True);
            Assert.That(Matrix4x4.EqualEstimate(Transform.ToMatrix4x4(b), mb, epsilon), Is.True);
            Assert.That(Matrix4x4.EqualEstimate(Transform.ToMatrix4x4(c), mc, epsilon), Is.True);
        }

        /// <summary>Ensures that <see cref="Transform" /> with 90 degree rotations switches to the correct axes.
        /// Note that when the coordinate system is left handed then the left hard curled fingers point in the direction of positive rotation about the thumb axis.</summary>
        [Test]
        public static void RotationsSwitchAxes()
        {
            var radians90 = 90f * MathF.PI / 180.0f;
            var xToY = Transform.AddRotationAroundSourceZ(Transform.Identity, radians90);
            var yToZ = Transform.AddRotationAroundSourceX(Transform.Identity, radians90);
            var zToX = Transform.AddRotationAroundSourceY(Transform.Identity, radians90);
            var x = new Vector4(1f, 0f, 0f, 0f);
            var y = new Vector4(0f, 1f, 0f, 0f);
            var z = new Vector4(0f, 0f, 1f, 0f);
            var yy = x * Transform.ToMatrix4x4(xToY);
            var zz = y * Transform.ToMatrix4x4(yToZ);
            var xx = z * Transform.ToMatrix4x4(zToX);
            var epsilon = Vector4.One * 1e-6f;

            Assert.That(Vector4.EqualEstimate(xx, x, epsilon), Is.True);
            Assert.That(Vector4.EqualEstimate(yy, y, epsilon), Is.True);
            Assert.That(Vector4.EqualEstimate(zz, z, epsilon), Is.True);
        }
    }
}
