// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.Intrinsics;
using NUnit.Framework;
using TerraFX.Utilities;

namespace TerraFX.UnitTests.Utilities;

/// <summary>Provides a set of tests covering the <see cref="VectorUtilities" /> static class.</summary>
[TestFixture(TestOf = typeof(VectorUtilities))]
public static class VectorUtilitiesTests
{
    /// <summary>Provides validation of the <see cref="VectorUtilities.Abs(Vector128{float})" /> method.</summary>
    [Test]
    public static void AbsTest()
    {
        Assert.That(() => VectorUtilities.Abs(Vector128.Create(-1.0f, 2.0f, -3.0f, 4.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.Add(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void AddTest()
    {
        Assert.That(() => VectorUtilities.Add(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(4.0f, 6.0f, 8.0f, 10.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.BitwiseAnd(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void BitwiseAndTest()
    {
        Assert.That(() => VectorUtilities.BitwiseAnd(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f), Vector128.Create(0xFFFFFFFF, 0x00000000, 0xFFFFFFFF, 0x00000000).AsSingle()),
            Is.EqualTo(Vector128.Create(1.0f, 0.0f, 3.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.BitwiseAndNot(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void BitwiseAndNotTest()
    {
        Assert.That(() => VectorUtilities.BitwiseAndNot(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f), Vector128.Create(0xFFFFFFFF, 0x00000000, 0xFFFFFFFF, 0x00000000).AsSingle()),
            Is.EqualTo(Vector128.Create(0.0f, 2.0f, 0.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.BitwiseOr(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void BitwiseOrTest()
    {
        Assert.That(() => VectorUtilities.BitwiseOr(Vector128.Create(-0.0f, 0.0f, -0.0f, 0.0f), Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f)),
            Is.EqualTo(Vector128.Create(-1.0f, 2.0f, -3.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.BitwiseXor(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void BitwiseXorTest()
    {
        Assert.That(() => VectorUtilities.BitwiseXor(Vector128.Create(-0.0f, -0.0f, -0.0f, -0.0f), Vector128.Create(-1.0f, 2.0f, -3.0f, 4.0f)),
            Is.EqualTo(Vector128.Create(1.0f, -2.0f, 3.0f, -4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CompareEqual(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CompareEqualTest()
    {
        Assert.That(() => VectorUtilities.CompareEqual(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f), Vector128.Create(1.0f, -2.0f, 3.0f, -4.0f)).AsUInt32(),
            Is.EqualTo(Vector128.Create(0xFFFFFFFF, 0x00000000, 0xFFFFFFFF, 0x00000000))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CompareEqualAll(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CompareEqualAllTest()
    {
        Assert.That(() => VectorUtilities.CompareEqualAll(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f), Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f)),
            Is.True
        );

        Assert.That(() => VectorUtilities.CompareEqualAll(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f), Vector128.Create(1.0f, -2.0f, 3.0f, -4.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CompareGreaterThan(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CompareGreaterThanTest()
    {
        Assert.That(() => VectorUtilities.CompareGreaterThan(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f), Vector128.Create(1.0f, -2.0f, 3.0f, -4.0f)).AsUInt32(),
            Is.EqualTo(Vector128.Create(0x00000000, 0xFFFFFFFF, 0x00000000, 0xFFFFFFFF))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CompareGreaterThanOrEqual(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CompareGreaterThanOrEqualTest()
    {
        Assert.That(() => VectorUtilities.CompareGreaterThanOrEqual(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f), Vector128.Create(1.0f, -2.0f, 3.0f, -4.0f)).AsUInt32(),
            Is.EqualTo(Vector128.Create(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CompareLessThan(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CompareLessThanTest()
    {
        Assert.That(() => VectorUtilities.CompareLessThan(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f), Vector128.Create(1.0f, -2.0f, 3.0f, -4.0f)).AsUInt32(),
            Is.EqualTo(Vector128<uint>.Zero)
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CompareGreaterThanOrEqual(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CompareLessThanOrEqualTest()
    {
        Assert.That(() => VectorUtilities.CompareLessThanOrEqual(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f), Vector128.Create(1.0f, -2.0f, 3.0f, -4.0f)).AsUInt32(),
            Is.EqualTo(Vector128.Create(0xFFFFFFFF, 0x00000000, 0xFFFFFFFF, 0x00000000))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CompareNotEqualAny(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CompareNotEqualAnyTest()
    {
        Assert.That(() => VectorUtilities.CompareNotEqualAny(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f), Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f)),
            Is.False
        );

        Assert.That(() => VectorUtilities.CompareNotEqualAny(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f), Vector128.Create(1.0f, -2.0f, 3.0f, -4.0f)),
            Is.True
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CompareTrueAll(Vector128{float})" /> method.</summary>
    [Test]
    public static void CompareTrueAllTest()
    {
        Assert.That(() => VectorUtilities.CompareTrueAll(Vector128.Create(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF).AsSingle()),
            Is.True
        );

        Assert.That(() => VectorUtilities.CompareTrueAll(Vector128.Create(0x0FFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF).AsSingle()),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CompareTrueAny(Vector128{float})" /> method.</summary>
    [Test]
    public static void CompareTrueAnyTest()
    {
        Assert.That(() => VectorUtilities.CompareTrueAny(Vector128.Create(0xFFFFFFFF, 0x00000000, 0x00000000, 0x00000000).AsSingle()),
            Is.True
        );

        Assert.That(() => VectorUtilities.CompareTrueAny(Vector128.Create(0x0FFFFFFF, 0x00000000, 0x00000000, 0x00000000).AsSingle()),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromX(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 0.0f, 0.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXXXY(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXXXYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXXXY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 0.0f, 4.0f, 5.0f))
        );
    }


    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXXYW(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXXYWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXXYW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 0.0f, 1.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXXYY(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXXYYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXXYY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 0.0f, 1.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXXZZ(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXXZZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXXZZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 0.0f, 6.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXYXX(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXYXXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXYXX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 4.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXYXZ(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXYXZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXYXZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 4.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXYZZ(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXYZZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXYZZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 6.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXYZW(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXYZWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXYZW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 6.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXZXZ(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXZXZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXZXZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 2.0f, 0.0f, 2.0f))
        );

        Assert.That(() => VectorUtilities.CreateFromXZXZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 2.0f, 4.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXZYW(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXZYWTest()
    {

        Assert.That(() => VectorUtilities.CreateFromXZYW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 2.0f, 1.0f, 3.0f))
        );

        Assert.That(() => VectorUtilities.CreateFromXZYW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 2.0f, 5.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXZWY(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXZWYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXZWY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 2.0f, 3.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXWXY(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXWXYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXWXY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 3.0f, 4.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXWZX(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXWZXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXWZX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 3.0f, 2.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXWZW(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromXWZWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXWZW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 3.0f, 6.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromY(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 1.0f, 1.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYXXX(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYXXXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYXXX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 0.0f, 0.0f, 0.0f))
        );

        Assert.That(() => VectorUtilities.CreateFromYXXX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 0.0f, 4.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYXXW(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYXXWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYXXW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 0.0f, 0.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYXWZ(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYXWZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYXWZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 0.0f, 3.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYZXY(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYZXYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYZXY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 2.0f, 0.0f, 1.0f))
        );

        Assert.That(() => VectorUtilities.CreateFromYZXY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 2.0f, 4.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYZXW(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYZXWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYZXW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 2.0f, 0.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYZYZ(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYZYZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYZYZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 2.0f, 5.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYZZY(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYZZYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYZZY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 2.0f, 6.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYWXZ(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYWXZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYWXZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 3.0f, 0.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYWYY(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYWYYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYWYY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 3.0f, 5.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYWYW(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYWYWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYWYW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 3.0f, 5.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYWZW(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYWZWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYWZW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 3.0f, 6.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYWWW(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYWWWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYWWW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 3.0f, 7.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZ(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 2.0f, 2.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZXYX(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZXYXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZXYX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 0.0f, 1.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZXYW(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZXYWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZXYW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 0.0f, 1.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZXWX(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZXWXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZXWX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 0.0f, 7.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZXWY(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZXWYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZXWY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 0.0f, 3.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZYYW(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZYYWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZYYW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 1.0f, 5.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZYZX(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZYZXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZYZX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 1.0f, 6.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZYZW(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZYZWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZYZW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 1.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZZXY(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZZXYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZZXY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 2.0f, 4.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZZYY(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZZYYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZZYY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 2.0f, 1.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZZYW(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZZYWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZZYW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 2.0f, 1.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZZZY(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZZZYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZZZY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 2.0f, 6.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZZZW(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZZZWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZZZW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 2.0f, 6.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZWXY(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZWXYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZWXY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 3.0f, 0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZWYZ(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZWYZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZWYZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 3.0f, 1.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZWZX(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZWZXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZWZX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 3.0f, 6.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZWZY(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZWZYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZWZY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 3.0f, 6.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZWZW(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromZWZWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZWZW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 3.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromW(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 3.0f, 3.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromWXYZ(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromWXYZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromWXYZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 0.0f, 1.0f, 2.0f))
        );

        Assert.That(() => VectorUtilities.CreateFromWXYZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 0.0f, 5.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromWXWX(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromWXWXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromWXWX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 0.0f, 3.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromWZYX(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromWZYXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromWZYX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 2.0f, 1.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromWZWY(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromWZWYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromWZWY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 2.0f, 3.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromWWWZ(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromWWWZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromWWWZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 3.0f, 3.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CrossProduct(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void CrossProductTest()
    {
        Assert.That(() => VectorUtilities.CrossProduct(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(-4.0f, 8.0f, -4.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.Divide(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void DivideTest()
    {
        Assert.That(() => VectorUtilities.Divide(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 0.2f, 0.33333334f, 0.42857143f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.DotProduct(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void DotProductTest()
    {
        Assert.That(() => VectorUtilities.DotProduct(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(38.0f, 38.0f, 38.0f, 38.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.ElementwiseSelect(Vector128{float}, Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void ElementwiseSelectTest()
    {
        Assert.That(() => VectorUtilities.ElementwiseSelect(Vector128.Create(0xFFFFFFFF, 0x00000000, 0xFFFFFFFF, 0x00000000).AsSingle(), Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 5.0f, 2.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.InterleaveLower(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void InterleaveLowerTest()
    {
        Assert.That(() => VectorUtilities.InterleaveLower(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 4.0f, 1.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.InterleaveUpper(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void InterleaveUpperTest()
    {
        Assert.That(() => VectorUtilities.InterleaveUpper(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 6.0f, 3.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.IsAnyInfinity(Vector128{float})" /> method.</summary>
    [Test]
    public static void IsAnyInfinityTest()
    {
        Assert.That(() => VectorUtilities.IsAnyInfinity(Vector128.Create(0.0f, 1.0f, 2.0f, float.PositiveInfinity)),
            Is.True
        );

        Assert.That(() => VectorUtilities.IsAnyInfinity(Vector128.Create(0.0f, 1.0f, 2.0f, float.NegativeInfinity)),
            Is.True
        );

        Assert.That(() => VectorUtilities.IsAnyInfinity(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.IsAnyNaN(Vector128{float})" /> method.</summary>
    [Test]
    public static void IsAnyNaNTest()
    {
        Assert.That(() => VectorUtilities.IsAnyNaN(Vector128.Create(0.0f, 1.0f, 2.0f, float.NaN)),
            Is.True
        );

        Assert.That(() => VectorUtilities.IsAnyNaN(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.Length(Vector128{float})" /> method.</summary>
    [Test]
    public static void LengthTest()
    {
        Assert.That(() => VectorUtilities.Length(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.7416575f, 3.7416575f, 3.7416575f, 3.7416575f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.LengthSquared(Vector128{float})" /> method.</summary>
    [Test]
    public static void LengthSquaredTest()
    {
        Assert.That(() => VectorUtilities.LengthSquared(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(14.0f, 14.0f, 14.0f, 14.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.Max(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MaxTest()
    {
        Assert.That(() => VectorUtilities.Max(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(3.0f, 2.0f, 1.0f, 0.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 2.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.Min(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MinTest()
    {
        Assert.That(() => VectorUtilities.Min(Vector128.Create(-0.0f, -1.0f, -2.0f, -3.0f), Vector128.Create(-3.0f, -2.0f, -1.0f, -0.0f)),
            Is.EqualTo(Vector128.Create(-3.0f, -2.0f, -2.0f, -3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.Multiply(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MultiplyTest()
    {
        Assert.That(() => VectorUtilities.Multiply(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 5.0f, 12.0f, 21.0f))
        );

        Assert.That(() => VectorUtilities.Multiply(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), 4.0f),
            Is.EqualTo(Vector128.Create(0.0f, 4.0f, 8.0f, 12.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.MultiplyAdd(Vector128{float}, Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MultiplyAddTest()
    {
        Assert.That(() => VectorUtilities.MultiplyAdd(Vector128.Create(10.0f, 10.0f, 10.0f, 10.0f), Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(10.0f, 15.0f, 22.0f, 31.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.MultiplyAddByX(Vector128{float}, Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MultiplyAddByXTest()
    {
        Assert.That(() => VectorUtilities.MultiplyAddByX(Vector128.Create(10.0f, 10.0f, 10.0f, 10.0f), Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(10.0f, 14.0f, 18.0f, 22.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.MultiplyAddByY(Vector128{float}, Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MultiplyAddByYTest()
    {
        Assert.That(() => VectorUtilities.MultiplyAddByY(Vector128.Create(10.0f, 10.0f, 10.0f, 10.0f), Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(10.0f, 15.0f, 20.0f, 25.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.MultiplyAddByZ(Vector128{float}, Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MultiplyAddByZTest()
    {
        Assert.That(() => VectorUtilities.MultiplyAddByZ(Vector128.Create(10.0f, 10.0f, 10.0f, 10.0f), Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(10.0f, 16.0f, 22.0f, 28.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.MultiplyAddByW(Vector128{float}, Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MultiplyAddByWTest()
    {
        Assert.That(() => VectorUtilities.MultiplyAddByW(Vector128.Create(10.0f, 10.0f, 10.0f, 10.0f), Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(10.0f, 17.0f, 24.0f, 31.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.MultiplyAddNegated(Vector128{float}, Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MultiplyAddNegatedTest()
    {
        Assert.That(() => VectorUtilities.MultiplyAddNegated(Vector128.Create(10.0f, 10.0f, 10.0f, 10.0f), Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(10.0f, 5.0f, -2.0f, -11.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.MultiplyByX(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MultiplyByXTest()
    {
        Assert.That(() => VectorUtilities.MultiplyByX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 4.0f, 8.0f, 12.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.MultiplyByY(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MultiplyByYTest()
    {
        Assert.That(() => VectorUtilities.MultiplyByY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 5.0f, 10.0f, 15.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.MultiplyByZ(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MultiplyByZTest()
    {
        Assert.That(() => VectorUtilities.MultiplyByZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 6.0f, 12.0f, 18.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.MultiplyByW(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void MultiplyByWTest()
    {
        Assert.That(() => VectorUtilities.MultiplyByW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 7.0f, 14.0f, 21.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.Negate(Vector128{float})" /> method.</summary>
    [Test]
    public static void NegateTest()
    {
        Assert.That(() => VectorUtilities.Negate(Vector128.Create(1.0f, -2.0f, 3.0f, -4.0f)),
            Is.EqualTo(Vector128.Create(-1.0f, 2.0f, -3.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.Normalize(Vector128{float})" /> method.</summary>
    [Test]
    public static void NormalizeTest()
    {
        Assert.That(() => VectorUtilities.Normalize(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 0.26726124f, 0.5345225f, 0.8017837f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.QuaternionConjugate(Vector128{float})" /> method.</summary>
    [Test]
    public static void QuaternionConjugateTest()
    {
        Assert.That(() => VectorUtilities.QuaternionConjugate(Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f)),
            Is.EqualTo(Vector128.Create(-1.0f, -2.0f, -3.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.Sqrt(Vector128{float})" /> method.</summary>
    [Test]
    public static void SqrtTest()
    {
        Assert.That(() => VectorUtilities.Sqrt(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 1.4142135f, 1.7320508f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.Subtract(Vector128{float}, Vector128{float})" /> method.</summary>
    [Test]
    public static void SubtractTest()
    {
        Assert.That(() => VectorUtilities.Subtract(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(-4.0f, -4.0f, -4.0f, -4.0f))
        );
    }
}
