using System.Runtime.Intrinsics;
using NUnit.Framework;
using TerraFX.Numerics;
using SysMatrix4x4 = System.Numerics.Matrix4x4;

namespace TerraFX.UnitTests.Numerics;

/// <summary>Provides a set of tests covering the <see cref="Matrix4x4" /> struct.</summary>
[TestFixture(TestOf = typeof(Matrix4x4))]
public class Matrix4x4Tests
{
    /// <summary>Provides validation of the <see cref="Matrix4x4.Zero" /> property.</summary>
    [Test]
    public static void ZeroTest()
    {
        Assert.That(() => Matrix4x4.Zero,
            Is.EqualTo(new Matrix4x4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.Identity" /> property.</summary>
    [Test]
    public static void IdentityTest()
    {
        Assert.That(() => Matrix4x4.Identity,
            Is.EqualTo(new Matrix4x4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4()" /> constructors.</summary>
    [Test]
    public static void CtorTest()
    {
        var value = new Matrix4x4();

        Assert.That(() => value.X, Is.EqualTo(Vector4.Zero));
        Assert.That(() => value.Y, Is.EqualTo(Vector4.Zero));
        Assert.That(() => value.Z, Is.EqualTo(Vector4.Zero));
        Assert.That(() => value.W, Is.EqualTo(Vector4.Zero));

        value = new Matrix4x4(
            new Vector4(00.0f, 01.0f, 02.0f, 03.0f),
            new Vector4(04.0f, 05.0f, 06.0f, 07.0f),
            new Vector4(08.0f, 09.0f, 10.0f, 11.0f),
            new Vector4(12.0f, 13.0f, 14.0f, 15.0f)
        );

        Assert.That(() => value.X, Is.EqualTo(new Vector4(00.0f, 01.0f, 02.0f, 03.0f)));
        Assert.That(() => value.Y, Is.EqualTo(new Vector4(04.0f, 05.0f, 06.0f, 07.0f)));
        Assert.That(() => value.Z, Is.EqualTo(new Vector4(08.0f, 09.0f, 10.0f, 11.0f)));
        Assert.That(() => value.W, Is.EqualTo(new Vector4(12.0f, 13.0f, 14.0f, 15.0f)));

        value = new Matrix4x4(new SysMatrix4x4(
            16.0f, 17.0f, 18.0f, 19.0f,
            20.0f, 21.0f, 22.0f, 23.0f,
            24.0f, 25.0f, 26.0f, 27.0f,
            28.0f, 29.0f, 30.0f, 31.0f
        ));

        Assert.That(() => value.X, Is.EqualTo(new Vector4(16.0f, 17.0f, 18.0f, 19.0f)));
        Assert.That(() => value.Y, Is.EqualTo(new Vector4(20.0f, 21.0f, 22.0f, 23.0f)));
        Assert.That(() => value.Z, Is.EqualTo(new Vector4(24.0f, 25.0f, 26.0f, 27.0f)));
        Assert.That(() => value.W, Is.EqualTo(new Vector4(28.0f, 29.0f, 30.0f, 31.0f)));
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.Determinant" /> property.</summary>
    [Test]
    public static void DeterminantTest()
    {
        Assert.That(() => new Matrix4x4(
            Vector4.UnitX,
            new Vector4(0.0f, 2.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 0.0f, 3.0f, 0.0f),
            new Vector4(1.0f, 2.0f, 3.0f, 1.0f)
        ).Determinant, Is.EqualTo(6.0f));
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.op_Equality" /> method.</summary>
    [Test]
    public static void OpEqualityTest()
    {
#pragma warning disable CS1718
        Assert.That(() => Matrix4x4.Identity == Matrix4x4.Identity,
            Is.True
        );
#pragma warning restore CS1718

        Assert.That(() => Matrix4x4.Identity == Matrix4x4.Zero,
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.op_Inequality" /> method.</summary>
    [Test]
    public static void OpInequalityTest()
    {
#pragma warning disable CS1718
        Assert.That(() => Matrix4x4.Identity != Matrix4x4.Identity,
            Is.False
        );
#pragma warning restore CS1718

        Assert.That(() => Matrix4x4.Identity != Matrix4x4.Zero,
            Is.True
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.op_Multiply" /> method.</summary>
    [Test]
    public static void OpMultiplyTest()
    {
        Assert.That(() => Matrix4x4.CreateFromRotation(Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.5f)) * Matrix4x4.CreateFromRotation(Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0.5f)),
            Is.EqualTo(new Matrix4x4(
                new Vector4(0.87758255f, +0.00000000f, -0.47942555f, 0.0f),
                new Vector4(0.22984886f, +0.87758255f, +0.42073550f, 0.0f),
                new Vector4(0.42073550f, -0.47942555f, +0.77015114f, 0.0f),
                Vector4.UnitW
            ))
        );
    }

    // OpMultiply

    /// <summary>Provides validation of the <see cref="Matrix4x4.CompareEqualAll(Matrix4x4, in Matrix4x4)" /> method.</summary>
    [Test]
    public static void CompareEqualAllTest()
    {
        Assert.That(() => Matrix4x4.CompareEqualAll(Matrix4x4.Identity, Matrix4x4.Identity),
            Is.True
        );

        Assert.That(() => Matrix4x4.CompareEqualAll(Matrix4x4.Identity, Matrix4x4.Zero),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromAffineTransform(AffineTransform, Vector3)" /> method.</summary>
    [Test]
    public static void CreateFromAffineTransform()
    {
        Assert.That(() => Matrix4x4.CreateFromAffineTransform(new AffineTransform(Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.5f), Vector3.One, Vector3.Zero)),
            Is.EqualTo(new Matrix4x4(
                Vector4.UnitX,
                new Vector4(0.0f, +0.87758255f, 0.47942555f, 0.0f),
                new Vector4(0.0f, -0.47942555f, 0.87758255f, 0.0f),
                Vector4.UnitW
            ))
        );

        Assert.That(() => Matrix4x4.CreateFromAffineTransform(new AffineTransform(Quaternion.Identity, new Vector3(1.0f, 2.0f, 3.0f), Vector3.Zero)),
            Is.EqualTo(new Matrix4x4(
                Vector4.UnitX,
                new Vector4(0.0f, 2.0f, 0.0f, 0.0f),
                new Vector4(0.0f, 0.0f, 3.0f, 0.0f),
                Vector4.UnitW
            ))
        );

        Assert.That(() => Matrix4x4.CreateFromAffineTransform(new AffineTransform(Quaternion.Identity, Vector3.One, new Vector3(1.0f, 2.0f, 3.0f))),
            Is.EqualTo(new Matrix4x4(
                Vector4.UnitX,
                Vector4.UnitY,
                Vector4.UnitZ,
                new Vector4(1.0f, 2.0f, 3.0f, 1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromRotation" /> method.</summary>
    [Test]
    public static void CreateFromRotationTest()
    {
        Assert.That(() => Matrix4x4.CreateFromRotation(Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.5f)),
            Is.EqualTo(new Matrix4x4(
                Vector4.UnitX,
                new Vector4(0.0f, +0.87758255f, 0.47942555f, 0.0f),
                new Vector4(0.0f, -0.47942555f, 0.87758255f, 0.0f),
                Vector4.UnitW
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromRotationX" /> method.</summary>
    [Test]
    public static void CreateFromRotationXTest()
    {
        Assert.That(() => Matrix4x4.CreateFromRotationX(0.5f),
            Is.EqualTo(new Matrix4x4(
                Vector4.UnitX,
                new Vector4(0.0f, +0.87758255f, 0.47942555f, 0.0f),
                new Vector4(0.0f, -0.47942555f, 0.87758255f, 0.0f),
                Vector4.UnitW
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromRotationY" /> method.</summary>
    [Test]
    public static void CreateFromRotationYTest()
    {
        Assert.That(() => Matrix4x4.CreateFromRotationY(0.5f),
            Is.EqualTo(new Matrix4x4(
                new Vector4(0.87758255f, 0.0f, -0.47942555f, 0.0f),
                Vector4.UnitY,
                new Vector4(0.47942555f, 0.0f, +0.87758255f, 0.0f),
                Vector4.UnitW
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromRotationZ" /> method.</summary>
    [Test]
    public static void CreateFromRotationZTest()
    {
        Assert.That(() => Matrix4x4.CreateFromRotationZ(0.5f),
            Is.EqualTo(new Matrix4x4(
                new Vector4(+0.87758255f, 0.47942555f, 0.0f, 0.0f),                
                new Vector4(-0.47942555f, 0.87758255f, 0.0f, 0.0f),
                Vector4.UnitZ,
                Vector4.UnitW
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromScale" /> method.</summary>
    [Test]
    public static void CreateFromScaleTest()
    {
        Assert.That(() => Matrix4x4.CreateFromScale(new Vector3(1.0f, 2.0f, 3.0f)),
            Is.EqualTo(new Matrix4x4(
                Vector4.UnitX,
                new Vector4(0.0f, 2.0f, 0.0f, 0.0f),
                new Vector4(0.0f, 0.0f, 3.0f, 0.0f),
                Vector4.UnitW
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromTranslation" /> method.</summary>
    [Test]
    public static void CreateFromTranslationTest()
    {
        Assert.That(() => Matrix4x4.CreateFromTranslation(new Vector3(1.0f, 2.0f, 3.0f)),
            Is.EqualTo(new Matrix4x4(
                Vector4.UnitX,
                Vector4.UnitY,
                Vector4.UnitZ,
                new Vector4(1.0f, 2.0f, 3.0f, 1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateLookAtLH" /> method.</summary>
    [Test]
    public static void CreateLookAtLHTest()
    {
        Assert.That(() => Matrix4x4.CreateLookAtLH(+Vector3.UnitZ, Vector3.Zero, Vector3.UnitY),
            Is.EqualTo(new Matrix4x4(
                -Vector4.UnitX,
                +Vector4.UnitY,
                -Vector4.UnitZ,
                +Vector4.UnitW.WithZ(+1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateLookAtRH" /> method.</summary>
    [Test]
    public static void CreateLookAtRHTest()
    {
        Assert.That(() => Matrix4x4.CreateLookAtRH(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY),
            Is.EqualTo(new Matrix4x4(
                +Vector4.UnitX,
                +Vector4.UnitY,
                +Vector4.UnitZ,
                +Vector4.UnitW.WithZ(-1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateLookToLH" /> method.</summary>
    [Test]
    public static void CreateLookToLHTest()
    {
        Assert.That(() => Matrix4x4.CreateLookToLH(Vector3.UnitZ, -Vector3.UnitZ, Vector3.UnitY),
            Is.EqualTo(new Matrix4x4(
                -Vector4.UnitX,
                +Vector4.UnitY,
                -Vector4.UnitZ,
                +Vector4.UnitW.WithZ(+1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateLookToRH" /> method.</summary>
    [Test]
    public static void CreateLookToRHTest()
    {
        Assert.That(() => Matrix4x4.CreateLookToRH(Vector3.UnitZ, -Vector3.UnitZ, Vector3.UnitY),
            Is.EqualTo(new Matrix4x4(
                +Vector4.UnitX,
                +Vector4.UnitY,
                +Vector4.UnitZ,
                +Vector4.UnitW.WithZ(-1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.Inverse(Matrix4x4, out float)" /> method.</summary>
    [Test]
    public static void InverseTest()
    {
        Assert.That(() => {
            var matrix = new Matrix4x4(
                Vector4.UnitX,
                new Vector4(0.0f, 2.0f, 0.0f, 0.0f),
                new Vector4(0.0f, 0.0f, 3.0f, 0.0f),
                new Vector4(1.0f, 2.0f, 3.0f, 1.0f)
            );
            return Matrix4x4.Inverse(matrix, out _);
        }, Is.EqualTo(new Matrix4x4(
            new Vector4(+1.0f, +0.0f, +0.00000000f, 0.0f),
            new Vector4(+0.0f, +0.5f, +0.00000000f, 0.0f),
            new Vector4(+0.0f, +0.0f, +0.33333334f, 0.0f),
            new Vector4(-1.0f, -1.0f, -1.00000000f, 1.0f)
        )));
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.IsAnyInfinity(Matrix4x4)" /> method.</summary>
    [Test]
    public static void IsAnyInfinityTest()
    {
        Assert.That(() => {
            var matrix = Matrix4x4.Identity;
            matrix.W = new Vector4(0.0f, 0.0f, 0.0f, float.PositiveInfinity);
            return Matrix4x4.IsAnyInfinity(matrix);
        }, Is.True);

        Assert.That(() => {
            var matrix = Matrix4x4.Identity;
            matrix.W = new Vector4(0.0f, 0.0f, 0.0f, float.NegativeInfinity);
            return Matrix4x4.IsAnyInfinity(matrix);
        }, Is.True);

        Assert.That(() => Matrix4x4.IsAnyInfinity(Matrix4x4.Identity),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.IsAnyNaN(Matrix4x4)" /> method.</summary>
    [Test]
    public static void IsAnyNaNTest()
    {
        Assert.That(() => {
            var matrix = Matrix4x4.Identity;
            matrix.W = new Vector4(0.0f, 0.0f, 0.0f, float.NaN);
            return Matrix4x4.IsAnyNaN(matrix);
        }, Is.True);

        Assert.That(() => Matrix4x4.IsAnyNaN(Matrix4x4.Identity),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.IsIdentity(Matrix4x4)" /> method.</summary>
    [Test]
    public static void IsIdentityTest()
    {
        Assert.That(() => Matrix4x4.IsIdentity(Matrix4x4.Identity),
            Is.True
        );

        Assert.That(() => Matrix4x4.IsIdentity(Matrix4x4.Zero),
            Is.False
        );
    }

    // Transpose

    /// <summary>Provides validation of the <see cref="Matrix4x4.AsMatrix4x4" /> method.</summary>
    [Test]
    public static void AsMatrix4x4Test()
    {
        Assert.That(() => Matrix4x4.Identity.AsMatrix4x4(),
            Is.EqualTo(SysMatrix4x4.Identity)
        );
    }
}
