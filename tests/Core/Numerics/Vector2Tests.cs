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
            Is.EqualTo(Vector2.Create(0.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.UnitX" /> property.</summary>
    [Test]
    public static void UnitXTest()
    {
        Assert.That(() => Vector2.UnitX,
            Is.EqualTo(Vector2.Create(1.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.UnitY" /> property.</summary>
    [Test]
    public static void UnitYTest()
    {
        Assert.That(() => Vector2.UnitY,
            Is.EqualTo(Vector2.Create(0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.One" /> property.</summary>
    [Test]
    public static void OneTest()
    {
        Assert.That(() => Vector2.One,
            Is.EqualTo(Vector2.Create(1.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="M:Vector2.Create" /> methods.</summary>
    [Test]
    public static void CreateTest()
    {
        var value = Vector2.Zero;

        Assert.That(() => value.X, Is.EqualTo(0.0f));
        Assert.That(() => value.Y, Is.EqualTo(0.0f));

        value = Vector2.Create(0.0f, 1.0f);

        Assert.That(() => value.X, Is.EqualTo(0.0f));
        Assert.That(() => value.Y, Is.EqualTo(1.0f));

        value = Vector2.Create(2.0f);

        Assert.That(() => value.X, Is.EqualTo(2.0f));
        Assert.That(() => value.Y, Is.EqualTo(2.0f));

        value = Vector2.Create(new SysVector2(3.0f, 4.0f));

        Assert.That(() => value.X, Is.EqualTo(3.0f));
        Assert.That(() => value.Y, Is.EqualTo(4.0f));

        value = Vector2.Create(Vector128.Create(5.0f, 6.0f, 7.0f, 8.0f));

        Assert.That(() => value.X, Is.EqualTo(5.0f));
        Assert.That(() => value.Y, Is.EqualTo(6.0f));
    }

    /// <summary>Provides validation of the <see cref="Vector2.Length" /> property.</summary>
    [Test]
    public static void LengthTest()
    {
        Assert.That(() => Vector2.Create(0.0f, 1.0f).Length,
            Is.EqualTo(1.0f)
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.LengthSquared" /> property.</summary>
    [Test]
    public static void LengthSquaredTest()
    {
        Assert.That(() => Vector2.Create(0.0f, 1.0f).LengthSquared,
            Is.EqualTo(1.0f)
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_Equality" /> method.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        Assert.That(() => Vector2.Create(0.0f, 1.0f) == Vector2.Create(0.0f, 1.0f),
            Is.True
        );

        Assert.That(() => Vector2.Create(0.0f, 1.0f) == Vector2.Create(2.0f, 3.0f),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_Inequality" /> method.</summary>
    [Test]
    public static void OpInequalityTest()
    {
        Assert.That(() => Vector2.Create(0.0f, 1.0f) != Vector2.Create(0.0f, 1.0f),
            Is.False
        );

        Assert.That(() => Vector2.Create(0.0f, 1.0f) != Vector2.Create(2.0f, 3.0f),
            Is.True
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_UnaryPlus" /> method.</summary>
    [Test]
    public static void OpUnaryPlusTest()
    {
        Assert.That(() => +Vector2.Create(0.0f, 1.0f),
            Is.EqualTo(Vector2.Create(0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_UnaryNegation" /> method.</summary>
    [Test]
    public static void OpUnaryNegationTest()
    {
        Assert.That(() => -Vector2.Create(1.0f, 2.0f),
            Is.EqualTo(Vector2.Create(-1.0f, -2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_Addition" /> method.</summary>
    [Test]
    public static void OpAdditionTest()
    {
        Assert.That(() => Vector2.Create(0.0f, 1.0f) + Vector2.Create(2.0f, 3.0f),
            Is.EqualTo(Vector2.Create(2.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_Subtraction" /> method.</summary>
    [Test]
    public static void OpSubtractionTest()
    {
        Assert.That(() => Vector2.Create(0.0f, 1.0f) - Vector2.Create(2.0f, 3.0f),
            Is.EqualTo(Vector2.Create(-2.0f, -2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_Multiply(Vector2, Vector2)" /> method.</summary>
    [Test]
    public static void OpMultiplyTest()
    {
        Assert.That(() => Vector2.Create(0.0f, 1.0f) * Vector2.Create(2.0f, 3.0f),
            Is.EqualTo(Vector2.Create(0.0f, 3.0f))
        );

        Assert.That(() => Vector2.Create(0.0f, 1.0f) * 2.0f,
            Is.EqualTo(Vector2.Create(0.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.op_Division(Vector2, Vector2)" /> method.</summary>
    [Test]
    public static void OpDivisionTest()
    {
        Assert.That(() => Vector2.Create(0.0f, 1.0f) / Vector2.Create(2.0f, 3.0f),
            Is.EqualTo(Vector2.Create(0.0f, 0.33333334f))
        );

        Assert.That(() => Vector2.Create(0.0f, 1.0f) / 2.0f,
            Is.EqualTo(Vector2.Create(0.0f, 0.5f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.CompareEqualAll(Vector2, Vector2)" /> method.</summary>
    [Test]
    public static void CompareEqualAllTest()
    {
        Assert.That(() => Vector2.CompareEqualAll(Vector2.Create(0.0f, 1.0f), Vector2.Create(0.0f, 1.0f)),
            Is.True
        );

        Assert.That(() => Vector2.CompareEqualAll(Vector2.Create(0.0f, 1.0f), Vector2.Create(2.0f, 3.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.DotProduct(Vector2, Vector2)" /> method.</summary>
    [Test]
    public static void DotProductTest()
    {
        Assert.That(() => Vector2.DotProduct(Vector2.Create(0.0f, 1.0f), Vector2.Create(2.0f, 3.0f)),
            Is.EqualTo(3.0f)
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.IsAnyInfinity(Vector2)" /> method.</summary>
    [Test]
    public static void IsAnyInfinityTest()
    {
        Assert.That(() => Vector2.IsAnyInfinity(Vector2.Create(0.0f, float.PositiveInfinity)),
            Is.True
        );

        Assert.That(() => Vector2.IsAnyInfinity(Vector2.Create(0.0f, float.NegativeInfinity)),
            Is.True
        );

        Assert.That(() => Vector2.IsAnyInfinity(Vector2.Create(0.0f, 1.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.IsAnyNaN(Vector2)" /> method.</summary>
    [Test]
    public static void IsAnyNaNTest()
    {
        Assert.That(() => Vector2.IsAnyNaN(Vector2.Create(0.0f, float.NaN)),
            Is.True
        );

        Assert.That(() => Vector2.IsAnyNaN(Vector2.Create(0.0f, 1.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.Max" /> method.</summary>
    [Test]
    public static void MaxTest()
    {
        Assert.That(() => Vector2.Max(Vector2.Create(0.0f, 1.0f), Vector2.Create(1.0f, 0.0f)),
            Is.EqualTo(Vector2.Create(1.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.Min" /> method.</summary>
    [Test]
    public static void MinTest()
    {
        Assert.That(() => Vector2.Min(Vector2.Create(-0.0f, -1.0f), Vector2.Create(-1.0f, -0.0f)),
            Is.EqualTo(Vector2.Create(-1.0f, -1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.Normalize" /> method.</summary>
    [Test]
    public static void NormalizeTest()
    {
        Assert.That(() => Vector2.Normalize(Vector2.Create(0.0f, 1.0f)),
            Is.EqualTo(Vector2.Create(0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.Sqrt" /> method.</summary>
    [Test]
    public static void SqrtTest()
    {
        Assert.That(() => Vector2.Sqrt(Vector2.Create(0.0f, 1.0f)),
            Is.EqualTo(Vector2.Create(0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.AsSystemVector2" /> method.</summary>
    [Test]
    public static void AsSystemVector2Test()
    {
        Assert.That(() => Vector2.Create(0.0f, 1.0f).AsSystemVector2(),
            Is.EqualTo(new SysVector2(0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.AsVector128" /> method.</summary>
    [Test]
    public static void AsVector128Test()
    {
        Assert.That(() => Vector2.Create(0.0f, 1.0f).AsVector128(),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 0.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.WithX" /> method.</summary>
    [Test]
    public static void WithXTest()
    {
        Assert.That(() => Vector2.Create(0.0f, 1.0f).WithX(5.0f),
            Is.EqualTo(Vector2.Create(5.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Vector2.WithY" /> method.</summary>
    [Test]
    public static void WithYTest()
    {
        Assert.That(() => Vector2.Create(0.0f, 1.0f).WithY(5.0f),
            Is.EqualTo(Vector2.Create(0.0f, 5.0f))
        );
    }
}
