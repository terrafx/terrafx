using System.Runtime.Intrinsics;
using NUnit.Framework;
using TerraFX.Numerics;
using static TerraFX.Utilities.VectorUtilities;
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
            Is.EqualTo(Matrix4x4.Create(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.Identity" /> property.</summary>
    [Test]
    public static void IdentityTest()
    {
        Assert.That(() => Matrix4x4.Identity,
            Is.EqualTo(Matrix4x4.Create(UnitX, UnitY, UnitZ, UnitW))
        );
    }

    /// <summary>Provides validation of the <see cref="M:Matrix4x4.Create" /> methods.</summary>
    [Test]
    public static void CreateTest()
    {
        var value = Matrix4x4.Zero;

        Assert.That(() => value.X, Is.EqualTo(Vector4.Zero));
        Assert.That(() => value.Y, Is.EqualTo(Vector4.Zero));
        Assert.That(() => value.Z, Is.EqualTo(Vector4.Zero));
        Assert.That(() => value.W, Is.EqualTo(Vector4.Zero));

        value = Matrix4x4.Create(
            Vector128.Create(00.0f, 01.0f, 02.0f, 03.0f),
            Vector128.Create(04.0f, 05.0f, 06.0f, 07.0f),
            Vector128.Create(08.0f, 09.0f, 10.0f, 11.0f),
            Vector128.Create(12.0f, 13.0f, 14.0f, 15.0f)
        );

        Assert.That(() => value.X, Is.EqualTo(Vector4.Create(00.0f, 01.0f, 02.0f, 03.0f)));
        Assert.That(() => value.Y, Is.EqualTo(Vector4.Create(04.0f, 05.0f, 06.0f, 07.0f)));
        Assert.That(() => value.Z, Is.EqualTo(Vector4.Create(08.0f, 09.0f, 10.0f, 11.0f)));
        Assert.That(() => value.W, Is.EqualTo(Vector4.Create(12.0f, 13.0f, 14.0f, 15.0f)));

        value = Matrix4x4.Create(new SysMatrix4x4(
            16.0f, 17.0f, 18.0f, 19.0f,
            20.0f, 21.0f, 22.0f, 23.0f,
            24.0f, 25.0f, 26.0f, 27.0f,
            28.0f, 29.0f, 30.0f, 31.0f
        ));

        Assert.That(() => value.X, Is.EqualTo(Vector4.Create(16.0f, 17.0f, 18.0f, 19.0f)));
        Assert.That(() => value.Y, Is.EqualTo(Vector4.Create(20.0f, 21.0f, 22.0f, 23.0f)));
        Assert.That(() => value.Z, Is.EqualTo(Vector4.Create(24.0f, 25.0f, 26.0f, 27.0f)));
        Assert.That(() => value.W, Is.EqualTo(Vector4.Create(28.0f, 29.0f, 30.0f, 31.0f)));
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.Determinant" /> property.</summary>
    [Test]
    public static void DeterminantTest()
    {
        Assert.That(() => Matrix4x4.Create(
            UnitX,
            Vector128.Create(0.0f, 2.0f, 0.0f, 0.0f),
            Vector128.Create(0.0f, 0.0f, 3.0f, 0.0f),
            Vector128.Create(1.0f, 2.0f, 3.0f, 1.0f)
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
        Assert.That(() => Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.5f)) * Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0.5f)),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(0.87758255f, +0.00000000f, -0.47942555f, 0.0f),
                Vector128.Create(0.22984886f, +0.87758255f, +0.42073550f, 0.0f),
                Vector128.Create(0.42073550f, -0.47942555f, +0.77015114f, 0.0f),
                UnitW
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
        Assert.That(() => Matrix4x4.CreateFromAffineTransform(AffineTransform.Create(Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.5f), Vector3.One, Vector3.Zero)),
            Is.EqualTo(Matrix4x4.Create(
                UnitX,
                Vector128.Create(0.0f, +0.87758255f, 0.47942555f, 0.0f),
                Vector128.Create(0.0f, -0.47942555f, 0.87758255f, 0.0f),
                UnitW
            ))
        );

        Assert.That(() => Matrix4x4.CreateFromAffineTransform(AffineTransform.Create(Quaternion.Identity, Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Zero)),
            Is.EqualTo(Matrix4x4.Create(
                UnitX,
                Vector128.Create(0.0f, 2.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0f, 3.0f, 0.0f),
                UnitW
            ))
        );

        Assert.That(() => Matrix4x4.CreateFromAffineTransform(AffineTransform.Create(Quaternion.Identity, Vector3.One, Vector3.Create(1.0f, 2.0f, 3.0f))),
            Is.EqualTo(Matrix4x4.Create(
                UnitX,
                UnitY,
                UnitZ,
                Vector128.Create(1.0f, 2.0f, 3.0f, 1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromQuaternion" /> method.</summary>
    [Test]
    public static void CreateFromQuaternionTest()
    {
        Assert.That(() => Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.5f)),
            Is.EqualTo(Matrix4x4.Create(
                UnitX,
                Vector128.Create(0.0f, +0.87758255f, 0.47942555f, 0.0f),
                Vector128.Create(0.0f, -0.47942555f, 0.87758255f, 0.0f),
                UnitW
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromRotationX" /> method.</summary>
    [Test]
    public static void CreateFromRotationXTest()
    {
        Assert.That(() => Matrix4x4.CreateFromRotationX(0.5f),
            Is.EqualTo(Matrix4x4.Create(
                UnitX,
                Vector128.Create(0.0f, +0.87758255f, 0.47942555f, 0.0f),
                Vector128.Create(0.0f, -0.47942555f, 0.87758255f, 0.0f),
                UnitW
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromRotationY" /> method.</summary>
    [Test]
    public static void CreateFromRotationYTest()
    {
        Assert.That(() => Matrix4x4.CreateFromRotationY(0.5f),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(0.87758255f, 0.0f, -0.47942555f, 0.0f),
                UnitY,
                Vector128.Create(0.47942555f, 0.0f, +0.87758255f, 0.0f),
                UnitW
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromRotationZ" /> method.</summary>
    [Test]
    public static void CreateFromRotationZTest()
    {
        Assert.That(() => Matrix4x4.CreateFromRotationZ(0.5f),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(+0.87758255f, 0.47942555f, 0.0f, 0.0f),                
                Vector128.Create(-0.47942555f, 0.87758255f, 0.0f, 0.0f),
                UnitZ,
                UnitW
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromScale" /> method.</summary>
    [Test]
    public static void CreateFromScaleTest()
    {
        Assert.That(() => Matrix4x4.CreateFromScale(Vector3.Create(1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Matrix4x4.Create(
                UnitX,
                Vector128.Create(0.0f, 2.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0f, 3.0f, 0.0f),
                UnitW
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateFromTranslation" /> method.</summary>
    [Test]
    public static void CreateFromTranslationTest()
    {
        Assert.That(() => Matrix4x4.CreateFromTranslation(Vector3.Create(1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Matrix4x4.Create(
                UnitX,
                UnitY,
                UnitZ,
                Vector128.Create(1.0f, 2.0f, 3.0f, 1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateLookAtLH" /> method.</summary>
    [Test]
    public static void CreateLookAtLHTest()
    {
        Assert.That(() => Matrix4x4.CreateLookAtLH(+Vector3.UnitZ, Vector3.Zero, Vector3.UnitY),
            Is.EqualTo(Matrix4x4.Create(
                Negate(UnitX),
                UnitY,
                Negate(UnitZ),
                UnitW.WithZ(+1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateLookAtRH" /> method.</summary>
    [Test]
    public static void CreateLookAtRHTest()
    {
        Assert.That(() => Matrix4x4.CreateLookAtRH(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY),
            Is.EqualTo(Matrix4x4.Create(
                UnitX,
                UnitY,
                UnitZ,
                UnitW.WithZ(-1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateLookToLH" /> method.</summary>
    [Test]
    public static void CreateLookToLHTest()
    {
        Assert.That(() => Matrix4x4.CreateLookToLH(Vector3.UnitZ, -Vector3.UnitZ, Vector3.UnitY),
            Is.EqualTo(Matrix4x4.Create(
                Negate(UnitX),
                UnitY,
                Negate(UnitZ),
                UnitW.WithZ(+1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateLookToRH" /> method.</summary>
    [Test]
    public static void CreateLookToRHTest()
    {
        Assert.That(() => Matrix4x4.CreateLookToRH(Vector3.UnitZ, -Vector3.UnitZ, Vector3.UnitY),
            Is.EqualTo(Matrix4x4.Create(
                UnitX,
                UnitY,
                UnitZ,
                UnitW.WithZ(-1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateOrthographicLH" /> method.</summary>
    [Test]
    public static void CreateOrthographicLHTest()
    {
        Assert.That(() => Matrix4x4.CreateOrthographicLH(1920.0f, 1080.0f, 0.001f, 1000.0f),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(0.0010416667f, 0.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0018518518f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0f, 0.001000001f, 0.0f),
                Vector128.Create(0.0f, 0.0f, -1.000001E-06f, 1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateOrthographicRH" /> method.</summary>
    [Test]
    public static void CreateOrthographicRHTest()
    {
        Assert.That(() => Matrix4x4.CreateOrthographicRH(1920.0f, 1080.0f, 0.001f, 1000.0f),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(0.0010416667f, 0.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0018518518f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0f, -0.001000001f, 0.0f),
                Vector128.Create(0.0f, 0.0f, -1.000001E-06f, 1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateOrthographicOffCenterLH" /> method.</summary>
    [Test]
    public static void CreateOrthographicOffCenterLHTest()
    {
        Assert.That(() => Matrix4x4.CreateOrthographicOffCenterLH(-960.0f, +960.0f, +540.0f, -540.0f, 0.001f, 1000.0f),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(0.0010416667f, 0.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, -0.0018518518f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0f, 0.001000001f, 0.0f),
                Vector128.Create(-0.0f, 0.0f, -1.000001E-06f, 1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreateOrthographicOffCenterRH" /> method.</summary>
    [Test]
    public static void CreateOrthographicOffCenterRHTest()
    {
        Assert.That(() => Matrix4x4.CreateOrthographicOffCenterRH(-960.0f, +960.0f, +540.0f, -540.0f, 0.001f, 1000.0f),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(0.0010416667f, 0.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, -0.0018518518f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0f, -0.001000001f, 0.0f),
                Vector128.Create(-0.0f, 0.0f, -1.000001E-06f, 1.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreatePerspectiveLH" /> method.</summary>
    [Test]
    public static void CreatePerspectiveLHTest()
    {
        Assert.That(() => Matrix4x4.CreatePerspectiveLH(1920.0f, 1080.0f, 0.001f, 1000.0f),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(1.0416667E-06f, 0.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 1.851852E-06f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0f, 1.000001f, 1.0f),
                Vector128.Create(0.0f, 0.0f, -0.001000001f, 0.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreatePerspectiveRH" /> method.</summary>
    [Test]
    public static void CreatePerspectiveRHTest()
    {
        Assert.That(() => Matrix4x4.CreatePerspectiveRH(1920.0f, 1080.0f, 0.001f, 1000.0f),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(1.0416667E-06f, 0.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 1.851852E-06f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0f, -1.000001f, -1.0f),
                Vector128.Create(0.0f, 0.0f, -0.001000001f, 0.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreatePerspectiveFieldOfViewLH" /> method.</summary>
    [Test]
    public static void CreatePerspectiveFieldOfViewLHTest()
    {
        Assert.That(() => Matrix4x4.CreatePerspectiveFieldOfViewLH(1.2217305f, 1.7777778f, 0.001f, 1000.0f),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(0.8033333f, 0.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 1.428148f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0f, 1.000001f, 1.0f),
                Vector128.Create(0.0f, 0.0f, -0.001000001f, 0.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreatePerspectiveFieldOfViewRH" /> method.</summary>
    [Test]
    public static void CreatePerspectiveFieldOfViewRHTest()
    {
        Assert.That(() => Matrix4x4.CreatePerspectiveFieldOfViewRH(1.2217305f, 1.7777778f, 0.001f, 1000.0f),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(0.8033333f, 0.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 1.428148f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0f, -1.000001f, -1.0f),
                Vector128.Create(0.0f, 0.0f, -0.001000001f, 0.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreatePerspectiveOffCenterLH" /> method.</summary>
    [Test]
    public static void CreatePerspectiveOffCenterLHTest()
    {
        Assert.That(() => Matrix4x4.CreatePerspectiveOffCenterLH(-960.0f, +960.0f, +540.0f, -540.0f, 0.001f, 1000.0f),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(1.0416668E-06f, 0.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, -1.851852E-06f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0f, 1.000001f, 1.0f),
                Vector128.Create(0.0f, 0.0f, -0.001000001f, 0.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.CreatePerspectiveOffCenterRH" /> method.</summary>
    [Test]
    public static void CreatePerspectiveOffCenterRHTest()
    {
        Assert.That(() => Matrix4x4.CreatePerspectiveOffCenterRH(-960.0f, +960.0f, +540.0f, -540.0f, 0.001f, 1000.0f),
            Is.EqualTo(Matrix4x4.Create(
                Vector128.Create(1.0416668E-06f, 0.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, -1.851852E-06f, 0.0f, 0.0f),
                Vector128.Create(0.0f, -0.0f, -1.000001f, -1.0f),
                Vector128.Create(0.0f, 0.0f, -0.001000001f, 0.0f)
            ))
        );
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.Inverse(Matrix4x4, out float)" /> method.</summary>
    [Test]
    public static void InverseTest()
    {
        Assert.That(() => {
            var matrix = Matrix4x4.Create(
                UnitX,
                Vector128.Create(0.0f, 2.0f, 0.0f, 0.0f),
                Vector128.Create(0.0f, 0.0f, 3.0f, 0.0f),
                Vector128.Create(1.0f, 2.0f, 3.0f, 1.0f)
            );
            return Matrix4x4.Inverse(matrix, out _);
        }, Is.EqualTo(Matrix4x4.Create(
            Vector128.Create(+1.0f, +0.0f, +0.00000000f, 0.0f),
            Vector128.Create(+0.0f, +0.5f, +0.00000000f, 0.0f),
            Vector128.Create(+0.0f, +0.0f, +0.33333334f, 0.0f),
            Vector128.Create(-1.0f, -1.0f, -1.00000000f, 1.0f)
        )));
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4.IsAnyInfinity(Matrix4x4)" /> method.</summary>
    [Test]
    public static void IsAnyInfinityTest()
    {
        Assert.That(() => {
            var matrix = Matrix4x4.Identity;
            matrix.W = Vector4.Create(0.0f, 0.0f, 0.0f, float.PositiveInfinity);
            return Matrix4x4.IsAnyInfinity(matrix);
        }, Is.True);

        Assert.That(() => {
            var matrix = Matrix4x4.Identity;
            matrix.W = Vector4.Create(0.0f, 0.0f, 0.0f, float.NegativeInfinity);
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
            matrix.W = Vector4.Create(0.0f, 0.0f, 0.0f, float.NaN);
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

    /// <summary>Provides validation of the <see cref="Matrix4x4.AsSystemMatrix4x4" /> method.</summary>
    [Test]
    public static void AsSystemMatrix4x4Test()
    {
        Assert.That(Matrix4x4.Identity.AsSystemMatrix4x4,
            Is.EqualTo(SysMatrix4x4.Identity)
        );
    }
}
