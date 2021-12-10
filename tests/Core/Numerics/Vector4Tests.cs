using System.Runtime.Intrinsics;
using NUnit.Framework;
using TerraFX.Numerics;
using SysVector4 = System.Numerics.Vector4;

namespace TerraFX.UnitTests.Numerics;

/// <summary>Provides a set of tests covering the <see cref="Vector4" /> struct.</summary>
[TestFixture(TestOf = typeof(Vector4))]
public class Vector4Tests
{
    /// <summary>Provides validation of the <see cref="Vector4.Zero" /> property.</summary>
    [Test]
    public static void ZeroTest()
    {
        Assert.That(() => Vector4.Zero,
            Is.EqualTo(new Vector4(0.0f, 0.0f, 0.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.UnitX" /> property.</summary>
    [Test]
    public static void UnitXTest()
    {
        Assert.That(() => Vector4.UnitX,
            Is.EqualTo(new Vector4(1.0f, 0.0f, 0.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.UnitY" /> property.</summary>
    [Test]
    public static void UnitYTest()
    {
        Assert.That(() => Vector4.UnitY,
            Is.EqualTo(new Vector4(0.0f, 1.0f, 0.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.UnitZ" /> property.</summary>
    [Test]
    public static void UnitZTest()
    {
        Assert.That(() => Vector4.UnitZ,
            Is.EqualTo(new Vector4(0.0f, 0.0f, 1.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.UnitW" /> property.</summary>
    [Test]
    public static void UnitWTest()
    {
        Assert.That(() => Vector4.UnitW,
            Is.EqualTo(new Vector4(0.0f, 0.0f, 0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.One" /> property.</summary>
    [Test]
    public static void OneTest()
    {
        Assert.That(() => Vector4.One,
            Is.EqualTo(new Vector4(1.0f, 1.0f, 1.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4()" /> constructors.</summary>
    [Test]
    public static void CtorTest()
    {
        var value = new Vector4();

        Assert.That(() => value.X, Is.EqualTo(0.0f));
        Assert.That(() => value.Y, Is.EqualTo(0.0f));
        Assert.That(() => value.Z, Is.EqualTo(0.0f));
        Assert.That(() => value.W, Is.EqualTo(0.0f));

        value = new Vector4(0.0f, 1.0f, 2.0f, 3.0f);

        Assert.That(() => value.X, Is.EqualTo(0.0f));
        Assert.That(() => value.Y, Is.EqualTo(1.0f));
        Assert.That(() => value.Z, Is.EqualTo(2.0f));
        Assert.That(() => value.W, Is.EqualTo(3.0f));

        value = new Vector4(5.0f);

        Assert.That(() => value.X, Is.EqualTo(5.0f));
        Assert.That(() => value.Y, Is.EqualTo(5.0f));
        Assert.That(() => value.Z, Is.EqualTo(5.0f));
        Assert.That(() => value.W, Is.EqualTo(5.0f));

        value = new Vector4(new Vector2(6.0f, 7.0f), 8.0f, 9.0f);

        Assert.That(() => value.X, Is.EqualTo(6.0f));
        Assert.That(() => value.Y, Is.EqualTo(7.0f));
        Assert.That(() => value.Z, Is.EqualTo(8.0f));
        Assert.That(() => value.W, Is.EqualTo(9.0f));

        value = new Vector4(new Vector3(10.0f, 11.0f, 12.0f), 13.0f);

        Assert.That(() => value.X, Is.EqualTo(10.0f));
        Assert.That(() => value.Y, Is.EqualTo(11.0f));
        Assert.That(() => value.Z, Is.EqualTo(12.0f));
        Assert.That(() => value.W, Is.EqualTo(13.0f));

        value = new Vector4(new SysVector4(14.0f, 15.0f, 16.0f, 17.0f));

        Assert.That(() => value.X, Is.EqualTo(14.0f));
        Assert.That(() => value.Y, Is.EqualTo(15.0f));
        Assert.That(() => value.Z, Is.EqualTo(16.0f));
        Assert.That(() => value.W, Is.EqualTo(17.0f));

        value = new Vector4(Vector128.Create(18.0f, 19.0f, 20.0f, 21.0f));

        Assert.That(() => value.X, Is.EqualTo(18.0f));
        Assert.That(() => value.Y, Is.EqualTo(19.0f));
        Assert.That(() => value.Z, Is.EqualTo(20.0f));
        Assert.That(() => value.W, Is.EqualTo(21.0f));
    }

    /// <summary>Provides validation of the <see cref="Vector4.Length" /> property.</summary>
    [Test]
    public static void LengthTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f).Length,
            Is.EqualTo(3.7416575f)
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.LengthSquared" /> property.</summary>
    [Test]
    public static void LengthSquaredTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f).LengthSquared,
            Is.EqualTo(14.0f)
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.op_Equality" /> method.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f) == new Vector4(0.0f, 1.0f, 2.0f, 3.0f),
            Is.True
        );

        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f) == new Vector4(4.0f, 5.0f, 6.0f, 7.0f),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.op_Inequality" /> method.</summary>
    [Test]
    public static void OpInequalityTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f) != new Vector4(0.0f, 1.0f, 2.0f, 3.0f),
            Is.False
        );

        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f) != new Vector4(4.0f, 5.0f, 6.0f, 7.0f),
            Is.True
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.op_UnaryPlus" /> method.</summary>
    [Test]
    public static void OpUnaryPlusTest()
    {
        Assert.That(() => +new Vector4(0.0f, 1.0f, 2.0f, 3.0f),
            Is.EqualTo(new Vector4(0.0f, 1.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.op_UnaryNegation" /> method.</summary>
    [Test]
    public static void OpUnaryNegationTest()
    {
        Assert.That(() => -new Vector4(1.0f, 2.0f, 3.0f, 4.0f),
            Is.EqualTo(new Vector4(-1.0f, -2.0f, -3.0f, -4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.op_Addition" /> method.</summary>
    [Test]
    public static void OpAdditionTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f) + new Vector4(4.0f, 5.0f, 6.0f, 7.0f),
            Is.EqualTo(new Vector4(4.0f, 6.0f, 8.0f, 10.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.op_Subtraction" /> method.</summary>
    [Test]
    public static void OpSubtractionTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f) - new Vector4(4.0f, 5.0f, 6.0f, 7.0f),
            Is.EqualTo(new Vector4(-4.0f, -4.0f, -4.0f, -4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.op_Multiply(Vector4, Vector4)" /> method.</summary>
    [Test]
    public static void OpMultiplyTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f) * new Vector4(4.0f, 5.0f, 6.0f, 7.0f),
            Is.EqualTo(new Vector4(0.0f, 5.0f, 12.0f, 21.0f))
        );

        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f) * 4.0f,
            Is.EqualTo(new Vector4(0.0f, 4.0f, 8.0f, 12.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.op_Division(Vector4, Vector4)" /> method.</summary>
    [Test]
    public static void OpDivisionTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f) / new Vector4(4.0f, 5.0f, 6.0f, 7.0f),
            Is.EqualTo(new Vector4(0.0f, 0.2f, 0.33333334f, 0.42857143f))
        );

        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f) / 4.0f,
            Is.EqualTo(new Vector4(0.0f, 0.25f, 0.5f, 0.75f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.CompareEqualAll(Vector4, Vector4)" /> method.</summary>
    [Test]
    public static void CompareEqualAllTest()
    {
        Assert.That(() => Vector4.CompareEqualAll(new Vector4(0.0f, 1.0f, 2.0f, 3.0f), new Vector4(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.True
        );

        Assert.That(() => Vector4.CompareEqualAll(new Vector4(0.0f, 1.0f, 2.0f, 3.0f), new Vector4(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.DotProduct(Vector4, Vector4)" /> method.</summary>
    [Test]
    public static void DotProductTest()
    {
        Assert.That(() => Vector4.DotProduct(new Vector4(0.0f, 1.0f, 2.0f, 3.0f), new Vector4(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(38.0f)
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.IsAnyInfinity(Vector4)" /> method.</summary>
    [Test]
    public static void IsAnyInfinityTest()
    {
        Assert.That(() => Vector4.IsAnyInfinity(new Vector4(0.0f, 1.0f, 2.0f, float.PositiveInfinity)),
            Is.True
        );

        Assert.That(() => Vector4.IsAnyInfinity(new Vector4(0.0f, 1.0f, 2.0f, float.NegativeInfinity)),
            Is.True
        );

        Assert.That(() => Vector4.IsAnyInfinity(new Vector4(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.IsAnyNaN(Vector4)" /> method.</summary>
    [Test]
    public static void IsAnyNaNTest()
    {
        Assert.That(() => Vector4.IsAnyNaN(new Vector4(0.0f, 1.0f, 2.0f, float.NaN)),
            Is.True
        );

        Assert.That(() => Vector4.IsAnyNaN(new Vector4(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.Max" /> method.</summary>
    [Test]
    public static void MaxTest()
    {
        Assert.That(() => Vector4.Max(new Vector4(0.0f, 1.0f, 2.0f, 3.0f), new Vector4(3.0f, 2.0f, 1.0f, 0.0f)),
            Is.EqualTo(new Vector4(3.0f, 2.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.Min" /> method.</summary>
    [Test]
    public static void MinTest()
    {
        Assert.That(() => Vector4.Min(new Vector4(-0.0f, -1.0f, -2.0f, -3.0f), new Vector4(-3.0f, -2.0f, -1.0f, -0.0f)),
            Is.EqualTo(new Vector4(-3.0f, -2.0f, -2.0f, -3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.Normalize" /> method.</summary>
    [Test]
    public static void NormalizeTest()
    {
        Assert.That(() => Vector4.Normalize(new Vector4(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(new Vector4(0.0f, 0.26726124f, 0.5345225f, 0.8017837f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.Sqrt" /> method.</summary>
    [Test]
    public static void SqrtTest()
    {
        Assert.That(() => Vector4.Sqrt(new Vector4(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(new Vector4(0.0f, 1.0f, 1.4142135f, 1.7320508f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.Transform(Vector4, Matrix4x4)" /> method.</summary>
    [Test]
    public static void TransformTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f) * Matrix4x4.Identity,
            Is.EqualTo(new Vector4(0.0f, 1.0f, 2.0f, 3.0f))
        );

        Assert.That(() => Vector4.Transform(new Vector4(0.0f, 1.0f, 2.0f, 3.0f), Matrix4x4.Identity),
            Is.EqualTo(new Vector4(0.0f, 1.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.AsVector4" /> method.</summary>
    [Test]
    public static void AsVector4Test()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f).AsVector4(),
            Is.EqualTo(new SysVector4(0.0f, 1.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.AsVector128" /> method.</summary>
    [Test]
    public static void AsVector128Test()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f).AsVector128(),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.WithX" /> method.</summary>
    [Test]
    public static void WithXTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f).WithX(5.0f),
            Is.EqualTo(new Vector4(5.0f, 1.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.WithY" /> method.</summary>
    [Test]
    public static void WithYTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f).WithY(5.0f),
            Is.EqualTo(new Vector4(0.0f, 5.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.WithZ" /> method.</summary>
    [Test]
    public static void WithZTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f).WithZ(5.0f),
            Is.EqualTo(new Vector4(0.0f, 1.0f, 5.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector4.WithW" /> method.</summary>
    [Test]
    public static void WithWTest()
    {
        Assert.That(() => new Vector4(0.0f, 1.0f, 2.0f, 3.0f).WithW(5.0f),
            Is.EqualTo(new Vector4(0.0f, 1.0f, 2.0f, 5.0f))
        );
    }
}
