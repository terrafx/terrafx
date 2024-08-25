// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.Intrinsics;
using NUnit.Framework;
using TerraFX.Numerics;
using SysVector3 = System.Numerics.Vector3;

namespace TerraFX.UnitTests.Numerics;

/// <summary>Provides a set of tests covering the <see cref="Vector3" /> struct.</summary>
[TestFixture(TestOf = typeof(Vector3))]
public static class Vector3Tests
{
    /// <summary>Provides validation of the <see cref="Vector3.Zero" /> property.</summary>
    [Test]
    public static void ZeroTest()
    {
        Assert.That(() => Vector3.Zero,
            Is.EqualTo(Vector3.Create(0.0f, 0.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.UnitX" /> property.</summary>
    [Test]
    public static void UnitXTest()
    {
        Assert.That(() => Vector3.UnitX,
            Is.EqualTo(Vector3.Create(1.0f, 0.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.UnitY" /> property.</summary>
    [Test]
    public static void UnitYTest()
    {
        Assert.That(() => Vector3.UnitY,
            Is.EqualTo(Vector3.Create(0.0f, 1.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.UnitZ" /> property.</summary>
    [Test]
    public static void UnitZTest()
    {
        Assert.That(() => Vector3.UnitZ,
            Is.EqualTo(Vector3.Create(0.0f, 0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.One" /> property.</summary>
    [Test]
    public static void OneTest()
    {
        Assert.That(() => Vector3.One,
            Is.EqualTo(Vector3.Create(1.0f, 1.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3" /> <c>Create</c> methods.</summary>
    [Test]
    public static void CreateTest()
    {
        var value = Vector3.Zero;

        Assert.That(() => value.X, Is.EqualTo(0.0f));
        Assert.That(() => value.Y, Is.EqualTo(0.0f));
        Assert.That(() => value.Z, Is.EqualTo(0.0f));

        value = Vector3.Create(0.0f, 1.0f, 2.0f);

        Assert.That(() => value.X, Is.EqualTo(0.0f));
        Assert.That(() => value.Y, Is.EqualTo(1.0f));
        Assert.That(() => value.Z, Is.EqualTo(2.0f));

        value = Vector3.Create(3.0f);

        Assert.That(() => value.X, Is.EqualTo(3.0f));
        Assert.That(() => value.Y, Is.EqualTo(3.0f));
        Assert.That(() => value.Z, Is.EqualTo(3.0f));

        value = Vector3.Create(Vector2.Create(4.0f, 5.0f), 6.0f);

        Assert.That(() => value.X, Is.EqualTo(4.0f));
        Assert.That(() => value.Y, Is.EqualTo(5.0f));
        Assert.That(() => value.Z, Is.EqualTo(6.0f));

        value = Vector3.Create(new SysVector3(7.0f, 8.0f, 9.0f));

        Assert.That(() => value.X, Is.EqualTo(7.0f));
        Assert.That(() => value.Y, Is.EqualTo(8.0f));
        Assert.That(() => value.Z, Is.EqualTo(9.0f));

        value = Vector3.Create(Vector128.Create(10.0f, 11.0f, 12.0f, 13.0f));

        Assert.That(() => value.X, Is.EqualTo(10.0f));
        Assert.That(() => value.Y, Is.EqualTo(11.0f));
        Assert.That(() => value.Z, Is.EqualTo(12.0f));
    }

    /// <summary>Provides validation of the <see cref="Vector3.Length" /> property.</summary>
    [Test]
    public static void LengthTest()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f).Length,
            Is.EqualTo(2.236068f)
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.LengthSquared" /> property.</summary>
    [Test]
    public static void LengthSquaredTest()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f).LengthSquared,
            Is.EqualTo(5.0f)
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.op_Equality" /> method.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f) == Vector3.Create(0.0f, 1.0f, 2.0f),
            Is.True
        );

        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f) == Vector3.Create(3.0f, 4.0f, 5.0f),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.op_Inequality" /> method.</summary>
    [Test]
    public static void OpInequalityTest()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f) != Vector3.Create(0.0f, 1.0f, 2.0f),
            Is.False
        );

        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f) != Vector3.Create(3.0f, 4.0f, 5.0f),
            Is.True
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.op_UnaryPlus" /> method.</summary>
    [Test]
    public static void OpUnaryPlusTest()
    {
        Assert.That(() => +Vector3.Create(0.0f, 1.0f, 2.0f),
            Is.EqualTo(Vector3.Create(0.0f, 1.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.op_UnaryNegation" /> method.</summary>
    [Test]
    public static void OpUnaryNegationTest()
    {
        Assert.That(() => -Vector3.Create(1.0f, 2.0f, 3.0f),
            Is.EqualTo(Vector3.Create(-1.0f, -2.0f, -3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.op_Addition" /> method.</summary>
    [Test]
    public static void OpAdditionTest()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f) + Vector3.Create(3.0f, 4.0f, 5.0f),
            Is.EqualTo(Vector3.Create(3.0f, 5.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.op_Subtraction" /> method.</summary>
    [Test]
    public static void OpSubtractionTest()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f) - Vector3.Create(3.0f, 4.0f, 5.0f),
            Is.EqualTo(Vector3.Create(-3.0f, -3.0f, -3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.op_Multiply(Vector3, Vector3)" /> method.</summary>
    [Test]
    public static void OpMultiplyTest()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f) * Vector3.Create(3.0f, 4.0f, 5.0f),
            Is.EqualTo(Vector3.Create(0.0f, 4.0f, 10.0f))
        );

        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f) * 3.0f,
            Is.EqualTo(Vector3.Create(0.0f, 3.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.op_Division(Vector3, Vector3)" /> method.</summary>
    [Test]
    public static void OpDivisionTest()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f) / 3.0f,
            Is.EqualTo(Vector3.Create(0.0f, 0.33333334f, 0.6666667f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.CompareEqualAll(Vector3, Vector3)" /> method.</summary>
    [Test]
    public static void CompareEqualAllTest()
    {
        Assert.That(() => Vector3.CompareEqualAll(Vector3.Create(0.0f, 1.0f, 2.0f), Vector3.Create(0.0f, 1.0f, 2.0f)),
            Is.True
        );

        Assert.That(() => Vector3.CompareEqualAll(Vector3.Create(0.0f, 1.0f, 2.0f), Vector3.Create(3.0f, 4.0f, 5.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.CrossProduct(Vector3, Vector3)" /> method.</summary>
    [Test]
    public static void CrossProductTest()
    {
        Assert.That(() => Vector3.CrossProduct(Vector3.Create(0.0f, 1.0f, 2.0f), Vector3.Create(3.0f, 4.0f, 5.0f)),
            Is.EqualTo(Vector3.Create(-3, 6, -3))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.DotProduct(Vector3, Vector3)" /> method.</summary>
    [Test]
    public static void DotProductTest()
    {
        Assert.That(() => Vector3.DotProduct(Vector3.Create(0.0f, 1.0f, 2.0f), Vector3.Create(3.0f, 4.0f, 5.0f)),
            Is.EqualTo(14.0f)
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.IsAnyInfinity(Vector3)" /> method.</summary>
    [Test]
    public static void IsAnyInfinityTest()
    {
        Assert.That(() => Vector3.IsAnyInfinity(Vector3.Create(0.0f, 1.0f, float.PositiveInfinity)),
            Is.True
        );

        Assert.That(() => Vector3.IsAnyInfinity(Vector3.Create(0.0f, 1.0f, float.NegativeInfinity)),
            Is.True
        );

        Assert.That(() => Vector3.IsAnyInfinity(Vector3.Create(0.0f, 1.0f, 2.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.IsAnyNaN(Vector3)" /> method.</summary>
    [Test]
    public static void IsAnyNaNTest()
    {
        Assert.That(() => Vector3.IsAnyNaN(Vector3.Create(0.0f, 1.0f, float.NaN)),
            Is.True
        );

        Assert.That(() => Vector3.IsAnyNaN(Vector3.Create(0.0f, 1.0f, 2.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.Max" /> method.</summary>
    [Test]
    public static void MaxTest()
    {
        Assert.That(() => Vector3.Max(Vector3.Create(0.0f, 1.0f, 2.0f), Vector3.Create(2.0f, 1.0f, 0.0f)),
            Is.EqualTo(Vector3.Create(2.0f, 1.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.Min" /> method.</summary>
    [Test]
    public static void MinTest()
    {
        Assert.That(() => Vector3.Min(Vector3.Create(-0.0f, -1.0f, -2.0f), Vector3.Create(-2.0f, -1.0f, -0.0f)),
            Is.EqualTo(Vector3.Create(-2.0f, -1.0f, -2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.Normalize" /> method.</summary>
    [Test]
    public static void NormalizeTest()
    {
        Assert.That(() => Vector3.Normalize(Vector3.Create(0.0f, 1.0f, 2.0f)),
            Is.EqualTo(Vector3.Create(0.0f, 0.4472136f, 0.8944272f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.Rotate(Vector3, Quaternion)" /> method.</summary>
    [Test]
    public static void RotateTest()
    {
        Assert.That(() => Vector3.Rotate(Vector3.Create(0.0f, 1.0f, 2.0f), Quaternion.Identity),
            Is.EqualTo(Vector3.Create(0.0f, 1.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.RotateInverse(Vector3, Quaternion)" /> method.</summary>
    [Test]
    public static void RotateInverseTest()
    {
        Assert.That(() => Vector3.RotateInverse(Vector3.Create(0.0f, 1.0f, 2.0f), Quaternion.Identity),
            Is.EqualTo(Vector3.Create(0.0f, 1.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.Sqrt" /> method.</summary>
    [Test]
    public static void SqrtTest()
    {
        Assert.That(() => Vector3.Sqrt(Vector3.Create(0.0f, 1.0f, 2.0f)),
            Is.EqualTo(Vector3.Create(0.0f, 1.0f, 1.4142135f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.Transform(Vector3, Matrix4x4)" /> method.</summary>
    [Test]
    public static void TransformTest()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f) * Matrix4x4.Identity,
            Is.EqualTo(Vector3.Create(0.0f, 1.0f, 2.0f))
        );

        Assert.That(() => Vector3.Transform(Vector3.Create(0.0f, 1.0f, 2.0f), Matrix4x4.Identity),
            Is.EqualTo(Vector3.Create(0.0f, 1.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.TransformNormal(Vector3, Matrix4x4)" /> method.</summary>
    [Test]
    public static void TransformNormalTest()
    {
        Assert.That(() => Vector3.TransformNormal(Vector3.Create(0.0f, 1.0f, 2.0f), Matrix4x4.Identity),
            Is.EqualTo(Vector3.Create(0.0f, 1.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.AsSystemVector3" /> method.</summary>
    [Test]
    public static void AsVector3Test()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f).AsSystemVector3(),
            Is.EqualTo(new SysVector3(0.0f, 1.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.AsVector128" /> method.</summary>
    [Test]
    public static void AsVector128Test()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f).AsVector128(),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 2.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.WithX" /> method.</summary>
    [Test]
    public static void WithXTest()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f).WithX(5.0f),
            Is.EqualTo(Vector3.Create(5.0f, 1.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.WithY" /> method.</summary>
    [Test]
    public static void WithYTest()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f).WithY(5.0f),
            Is.EqualTo(Vector3.Create(0.0f, 5.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector3.WithZ" /> method.</summary>
    [Test]
    public static void WithZTest()
    {
        Assert.That(() => Vector3.Create(0.0f, 1.0f, 2.0f).WithZ(5.0f),
            Is.EqualTo(Vector3.Create(0.0f, 1.0f, 5.0f))
        );
    }
}
