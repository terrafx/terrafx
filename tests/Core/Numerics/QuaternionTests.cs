using System.Runtime.Intrinsics;
using NUnit.Framework;
using TerraFX.Numerics;
using SysQuaternion = System.Numerics.Quaternion;

namespace TerraFX.UnitTests.Numerics;

/// <summary>Provides a set of tests covering the <see cref="Quaternion" /> struct.</summary>
[TestFixture(TestOf = typeof(Quaternion))]
public class QuaternionTests
{
    /// <summary>Provides validation of the <see cref="Quaternion.Zero" /> property.</summary>
    [Test]
    public static void ZeroTest()
    {
        Assert.That(() => Quaternion.Zero,
            Is.EqualTo(new Quaternion(0.0f, 0.0f, 0.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.Identity" /> property.</summary>
    [Test]
    public static void IdentityTest()
    {
        Assert.That(() => Quaternion.Identity,
            Is.EqualTo(new Quaternion(0.0f, 0.0f, 0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion()" /> constructors.</summary>
    [Test]
    public static void CtorTest()
    {
        var value = new Quaternion();

        Assert.That(() => value.X, Is.EqualTo(0.0f));
        Assert.That(() => value.Y, Is.EqualTo(0.0f));
        Assert.That(() => value.Z, Is.EqualTo(0.0f));
        Assert.That(() => value.W, Is.EqualTo(0.0f));

        value = new Quaternion(0.0f, 1.0f, 2.0f, 3.0f);

        Assert.That(() => value.X, Is.EqualTo(0.0f));
        Assert.That(() => value.Y, Is.EqualTo(1.0f));
        Assert.That(() => value.Z, Is.EqualTo(2.0f));
        Assert.That(() => value.W, Is.EqualTo(3.0f));

        value = new Quaternion(new SysQuaternion(4.0f, 5.0f, 6.0f, 7.0f));

        Assert.That(() => value.X, Is.EqualTo(4.0f));
        Assert.That(() => value.Y, Is.EqualTo(5.0f));
        Assert.That(() => value.Z, Is.EqualTo(6.0f));
        Assert.That(() => value.W, Is.EqualTo(7.0f));

        value = new Quaternion(Vector128.Create(8.0f, 9.0f, 10.0f, 11.0f));

        Assert.That(() => value.X, Is.EqualTo(8.0f));
        Assert.That(() => value.Y, Is.EqualTo(9.0f));
        Assert.That(() => value.Z, Is.EqualTo(10.0f));
        Assert.That(() => value.W, Is.EqualTo(11.0f));
    }

    /// <summary>Provides validation of the <see cref="Quaternion.Angle" /> property.</summary>
    [Test]
    public static void AngleTest()
    {
        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 0.5f).Angle,
            Is.EqualTo(2.0943952f)
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.Axis" /> property.</summary>
    [Test]
    public static void AxisTest()
    {
        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f).Axis,
            Is.EqualTo(new Vector3(0.0f, 1.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.Length" /> property.</summary>
    [Test]
    public static void LengthTest()
    {
        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f).Length,
            Is.EqualTo(3.7416575f)
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.LengthSquared" /> property.</summary>
    [Test]
    public static void LengthSquaredTest()
    {
        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f).LengthSquared,
            Is.EqualTo(14.0f)
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.op_Equality" /> method.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f) == new Quaternion(0.0f, 1.0f, 2.0f, 3.0f),
            Is.True
        );

        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f) == new Quaternion(4.0f, 5.0f, 6.0f, 7.0f),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.op_Inequality" /> method.</summary>
    [Test]
    public static void OpInequalityTest()
    {
        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f) != new Quaternion(0.0f, 1.0f, 2.0f, 3.0f),
            Is.False
        );

        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f) != new Quaternion(4.0f, 5.0f, 6.0f, 7.0f),
            Is.True
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.CompareEqualAll(Quaternion, Quaternion)" /> method.</summary>
    [Test]
    public static void CompareEqualAllTest()
    {
        Assert.That(() => Quaternion.CompareEqualAll(new Quaternion(0.0f, 1.0f, 2.0f, 3.0f), new Quaternion(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.True
        );

        Assert.That(() => Quaternion.CompareEqualAll(new Quaternion(0.0f, 1.0f, 2.0f, 3.0f), new Quaternion(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.Concatenate" /> method.</summary>
    [Test]
    public static void ConcatenateTest()
    {
        Assert.That(() => Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.5f) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0.5f),
            Is.EqualTo(new Quaternion(0.23971277f, 0.23971277f, 0.06120872f, 0.9387913f))
        );

        Assert.That(() => Quaternion.Concatenate(Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.5f), Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0.5f)),
            Is.EqualTo(new Quaternion(0.23971277f, 0.23971277f, -0.06120872f, 0.9387913f))
        );

        Assert.That(() => Quaternion.Concatenate(Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0.5f), Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.5f)),
            Is.EqualTo(new Quaternion(0.23971277f, 0.23971277f, 0.06120872f, 0.9387913f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.Conjugate(Quaternion)" /> method.</summary>
    [Test]
    public static void ConjugateTest()
    {
        Assert.That(() => Quaternion.Conjugate(new Quaternion(1.0f, 2.0f, 3.0f, 4.0f)),
            Is.EqualTo(new Quaternion(-1.0f, -2.0f, -3.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.CreateFromAxisAngle(Vector3, float)" /> method.</summary>
    [Test]
    public static void CreateFromAxisAngleTest()
    {
        Assert.That(() => Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.5f),
            Is.EqualTo(new Quaternion(0.24740396f, 0.0f, 0.0f, 0.9689124f))
        );

        Assert.That(() => Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0.5f),
            Is.EqualTo(new Quaternion(0.0f, 0.24740396f, 0.0f, 0.9689124f))
        );

        Assert.That(() => Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 0.5f),
            Is.EqualTo(new Quaternion(0.0f, 0.0f, 0.24740396f, 0.9689124f))
        );

        Assert.That(() => Quaternion.CreateFromAxisAngle(new Vector3(1.0f, 1.0f, 0.0f), 0.5f),
            Is.EqualTo(new Quaternion(0.17494102f, 0.17494102f, 0.0f, 0.9689124f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.CreateFromMatrix(Matrix4x4)" /> method.</summary>
    [Test]
    public static void CreateFromMatrixTest()
    {
        Assert.That(() => Quaternion.CreateFromMatrix(Matrix4x4.Identity),
            Is.EqualTo(Quaternion.Identity)
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.CreateFromAxisAngle(Vector3, float)" /> method.</summary>
    [Test]
    public static void CreateFromNormalizedAxisAngleTest()
    {
        Assert.That(() => Quaternion.CreateFromNormalizedAxisAngle(Vector3.UnitX, 0.5f),
            Is.EqualTo(new Quaternion(0.24740396f, 0.0f, 0.0f, 0.9689124f))
        );

        Assert.That(() => Quaternion.CreateFromNormalizedAxisAngle(Vector3.UnitY, 0.5f),
            Is.EqualTo(new Quaternion(0.0f, 0.24740396f, 0.0f, 0.9689124f))
        );

        Assert.That(() => Quaternion.CreateFromNormalizedAxisAngle(Vector3.UnitZ, 0.5f),
            Is.EqualTo(new Quaternion(0.0f, 0.0f, 0.24740396f, 0.9689124f))
        );

        Assert.That(() => Quaternion.CreateFromNormalizedAxisAngle(new Vector3(1.0f, 1.0f, 0.0f), 0.5f),
            Is.EqualTo(new Quaternion(0.24740396f, 0.24740396f, 0.0f, 0.9689124f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.CreateFromAxisAngle(Vector3, float)" /> method.</summary>
    [Test]
    public static void CreateFromPitchYawRollTest()
    {
        Assert.That(() => Quaternion.CreateFromPitchYawRoll(new Vector3(0.5f, 0.5f, 0.5f)),
            Is.EqualTo(new Quaternion(0.29156658f, 0.1729548f, 0.1729548f, 0.9247498f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.DotProduct(Quaternion, Quaternion)" /> method.</summary>
    [Test]
    public static void DotProductTest()
    {
        Assert.That(() => Quaternion.DotProduct(new Quaternion(0.0f, 1.0f, 2.0f, 3.0f), new Quaternion(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(38.0f)
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.Inverse(Quaternion)" /> method.</summary>
    [Test]
    public static void InverseTest()
    {
        Assert.That(() => Quaternion.Inverse(new Quaternion(0.24740396f, 0.0f, 0.0f, 0.9689124f)),
            Is.EqualTo(new Quaternion(-0.24740396f, -0.0f, -0.0f, 0.9689124f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.IsAnyInfinity(Quaternion)" /> method.</summary>
    [Test]
    public static void IsAnyInfinityTest()
    {
        Assert.That(() => Quaternion.IsAnyInfinity(new Quaternion(0.0f, 1.0f, 2.0f, float.PositiveInfinity)),
            Is.True
        );

        Assert.That(() => Quaternion.IsAnyInfinity(new Quaternion(0.0f, 1.0f, 2.0f, float.NegativeInfinity)),
            Is.True
        );

        Assert.That(() => Quaternion.IsAnyInfinity(new Quaternion(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.IsAnyNaN(Quaternion)" /> method.</summary>
    [Test]
    public static void IsAnyNaNTest()
    {
        Assert.That(() => Quaternion.IsAnyNaN(new Quaternion(0.0f, 1.0f, 2.0f, float.NaN)),
            Is.True
        );

        Assert.That(() => Quaternion.IsAnyNaN(new Quaternion(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.IsIdentity(Quaternion)" /> method.</summary>
    [Test]
    public static void IsIdentityTest()
    {
        Assert.That(() => Quaternion.IsIdentity(new Quaternion(0.0f, 0.0f, 0.0f, 1.0f)),
            Is.True
        );

        Assert.That(() => Quaternion.IsIdentity(new Quaternion(0.0f, 0.0f, 1.0f, 1.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.Normalize" /> method.</summary>
    [Test]
    public static void NormalizeTest()
    {
        Assert.That(() => Quaternion.Normalize(new Quaternion(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(new Quaternion(0.0f, 0.26726124f, 0.5345225f, 0.8017837f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.AsQuaternion" /> method.</summary>
    [Test]
    public static void AsQuaternionTest()
    {
        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f).AsQuaternion(),
            Is.EqualTo(new SysQuaternion(0.0f, 1.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.AsVector128" /> method.</summary>
    [Test]
    public static void AsVector128Test()
    {
        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f).AsVector128(),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.WithX" /> method.</summary>
    [Test]
    public static void WithXTest()
    {
        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f).WithX(5.0f),
            Is.EqualTo(new Quaternion(5.0f, 1.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.WithY" /> method.</summary>
    [Test]
    public static void WithYTest()
    {
        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f).WithY(5.0f),
            Is.EqualTo(new Quaternion(0.0f, 5.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.WithZ" /> method.</summary>
    [Test]
    public static void WithZTest()
    {
        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f).WithZ(5.0f),
            Is.EqualTo(new Quaternion(0.0f, 1.0f, 5.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="Quaternion.WithW" /> method.</summary>
    [Test]
    public static void WithWTest()
    {
        Assert.That(() => new Quaternion(0.0f, 1.0f, 2.0f, 3.0f).WithW(5.0f),
            Is.EqualTo(new Quaternion(0.0f, 1.0f, 2.0f, 5.0f))
        );
    }
}
