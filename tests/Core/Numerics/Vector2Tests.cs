using System.Runtime.Intrinsics;
using NUnit.Framework;
using TerraFX.Numerics;
using SysVector2 = System.Numerics.Vector2;

namespace TerraFX.UnitTests.Numerics;

/// <summary>Provides a set of tests covering the <see cref="Vector2" /> struct.</summary>
[TestFixture(TestOf = typeof(Vector2))]
public class Vector2Tests
{
    /// <summary>Provides validation of the <see cref="Vector2.Zero" /> property.</summary>
    [Test]
    public static void ZeroTest()
    {
        Assert.That(() => Vector2.Zero,
            Is.EqualTo(new Vector2(0.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.UnitX" /> property.</summary>
    [Test]
    public static void UnitXTest()
    {
        Assert.That(() => Vector2.UnitX,
            Is.EqualTo(new Vector2(1.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.UnitY" /> property.</summary>
    [Test]
    public static void UnitYTest()
    {
        Assert.That(() => Vector2.UnitY,
            Is.EqualTo(new Vector2(0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.One" /> property.</summary>
    [Test]
    public static void OneTest()
    {
        Assert.That(() => Vector2.One,
            Is.EqualTo(new Vector2(1.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2()" /> constructors.</summary>
    [Test]
    public static void CtorTest()
    {
        var value = new Vector2();

        Assert.That(() => value.X, Is.EqualTo(0.0f));
        Assert.That(() => value.Y, Is.EqualTo(0.0f));

        value = new Vector2(0.0f, 1.0f);

        Assert.That(() => value.X, Is.EqualTo(0.0f));
        Assert.That(() => value.Y, Is.EqualTo(1.0f));

        value = new Vector2(2.0f);

        Assert.That(() => value.X, Is.EqualTo(2.0f));
        Assert.That(() => value.Y, Is.EqualTo(2.0f));

        value = new Vector2(new SysVector2(3.0f, 4.0f));

        Assert.That(() => value.X, Is.EqualTo(3.0f));
        Assert.That(() => value.Y, Is.EqualTo(4.0f));

        value = new Vector2(Vector128.Create(5.0f, 6.0f, 7.0f, 8.0f));

        Assert.That(() => value.X, Is.EqualTo(5.0f));
        Assert.That(() => value.Y, Is.EqualTo(6.0f));
    }

    /// <summary>Provides validation of the <see cref="Vector2.Length" /> property.</summary>
    [Test]
    public static void LengthTest()
    {
        Assert.That(() => new Vector2(0.0f, 1.0f).Length,
            Is.EqualTo(1.0f)
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.LengthSquared" /> property.</summary>
    [Test]
    public static void LengthSquaredTest()
    {
        Assert.That(() => new Vector2(0.0f, 1.0f).LengthSquared,
            Is.EqualTo(1.0f)
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_Equality" /> method.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        Assert.That(() => new Vector2(0.0f, 1.0f) == new Vector2(0.0f, 1.0f),
            Is.True
        );

        Assert.That(() => new Vector2(0.0f, 1.0f) == new Vector2(2.0f, 3.0f),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_Inequality" /> method.</summary>
    [Test]
    public static void OpInequalityTest()
    {
        Assert.That(() => new Vector2(0.0f, 1.0f) != new Vector2(0.0f, 1.0f),
            Is.False
        );

        Assert.That(() => new Vector2(0.0f, 1.0f) != new Vector2(2.0f, 3.0f),
            Is.True
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_UnaryPlus" /> method.</summary>
    [Test]
    public static void OpUnaryPlusTest()
    {
        Assert.That(() => +new Vector2(0.0f, 1.0f),
            Is.EqualTo(new Vector2(0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_UnaryNegation" /> method.</summary>
    [Test]
    public static void OpUnaryNegationTest()
    {
        Assert.That(() => -new Vector2(1.0f, 2.0f),
            Is.EqualTo(new Vector2(-1.0f, -2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_Addition" /> method.</summary>
    [Test]
    public static void OpAdditionTest()
    {
        Assert.That(() => new Vector2(0.0f, 1.0f) + new Vector2(2.0f, 3.0f),
            Is.EqualTo(new Vector2(2.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_Subtraction" /> method.</summary>
    [Test]
    public static void OpSubtractionTest()
    {
        Assert.That(() => new Vector2(0.0f, 1.0f) - new Vector2(2.0f, 3.0f),
            Is.EqualTo(new Vector2(-2.0f, -2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_Multiply(Vector2, Vector2)" /> method.</summary>
    [Test]
    public static void OpMultiplyTest()
    {
        Assert.That(() => new Vector2(0.0f, 1.0f) * new Vector2(2.0f, 3.0f),
            Is.EqualTo(new Vector2(0.0f, 3.0f))
        );

        Assert.That(() => new Vector2(0.0f, 1.0f) * 2.0f,
            Is.EqualTo(new Vector2(0.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_Division(Vector2, Vector2)" /> method.</summary>
    [Test]
    public static void OpDivisionTest()
    {
        Assert.That(() => new Vector2(0.0f, 1.0f) / new Vector2(2.0f, 3.0f),
            Is.EqualTo(new Vector2(0.0f, 0.33333334f))
        );

        Assert.That(() => new Vector2(0.0f, 1.0f) / 2.0f,
            Is.EqualTo(new Vector2(0.0f, 0.5f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.CompareEqualAll(Vector2, Vector2)" /> method.</summary>
    [Test]
    public static void CompareEqualAllTest()
    {
        Assert.That(() => Vector2.CompareEqualAll(new Vector2(0.0f, 1.0f), new Vector2(0.0f, 1.0f)),
            Is.True
        );

        Assert.That(() => Vector2.CompareEqualAll(new Vector2(0.0f, 1.0f), new Vector2(2.0f, 3.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.DotProduct(Vector2, Vector2)" /> method.</summary>
    [Test]
    public static void DotProductTest()
    {
        Assert.That(() => Vector2.DotProduct(new Vector2(0.0f, 1.0f), new Vector2(2.0f, 3.0f)),
            Is.EqualTo(3.0f)
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.IsAnyInfinity(Vector2)" /> method.</summary>
    [Test]
    public static void IsAnyInfinityTest()
    {
        Assert.That(() => Vector2.IsAnyInfinity(new Vector2(0.0f, float.PositiveInfinity)),
            Is.True
        );

        Assert.That(() => Vector2.IsAnyInfinity(new Vector2(0.0f, float.NegativeInfinity)),
            Is.True
        );

        Assert.That(() => Vector2.IsAnyInfinity(new Vector2(0.0f, 1.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.IsAnyNaN(Vector2)" /> method.</summary>
    [Test]
    public static void IsAnyNaNTest()
    {
        Assert.That(() => Vector2.IsAnyNaN(new Vector2(0.0f, float.NaN)),
            Is.True
        );

        Assert.That(() => Vector2.IsAnyNaN(new Vector2(0.0f, 1.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.Max" /> method.</summary>
    [Test]
    public static void MaxTest()
    {
        Assert.That(() => Vector2.Max(new Vector2(0.0f, 1.0f), new Vector2(1.0f, 0.0f)),
            Is.EqualTo(new Vector2(1.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.Min" /> method.</summary>
    [Test]
    public static void MinTest()
    {
        Assert.That(() => Vector2.Min(new Vector2(-0.0f, -1.0f), new Vector2(-1.0f, -0.0f)),
            Is.EqualTo(new Vector2(-1.0f, -1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.Normalize" /> method.</summary>
    [Test]
    public static void NormalizeTest()
    {
        Assert.That(() => Vector2.Normalize(new Vector2(0.0f, 1.0f)),
            Is.EqualTo(new Vector2(0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.Sqrt" /> method.</summary>
    [Test]
    public static void SqrtTest()
    {
        Assert.That(() => Vector2.Sqrt(new Vector2(0.0f, 1.0f)),
            Is.EqualTo(new Vector2(0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.AsVector2" /> method.</summary>
    [Test]
    public static void AsVector2Test()
    {
        Assert.That(() => new Vector2(0.0f, 1.0f).AsVector2(),
            Is.EqualTo(new SysVector2(0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.AsVector128" /> method.</summary>
    [Test]
    public static void AsVector128Test()
    {
        Assert.That(() => new Vector2(0.0f, 1.0f).AsVector128(),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 0.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.WithX" /> method.</summary>
    [Test]
    public static void WithXTest()
    {
        Assert.That(() => new Vector2(0.0f, 1.0f).WithX(5.0f),
            Is.EqualTo(new Vector2(5.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.WithY" /> method.</summary>
    [Test]
    public static void WithYTest()
    {
        Assert.That(() => new Vector2(0.0f, 1.0f).WithY(5.0f),
            Is.EqualTo(new Vector2(0.0f, 5.0f))
        );
    }
}
