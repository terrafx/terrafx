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

        Assert.That(() => VectorUtilities.CompareTrueAll(Vector128.Create(0x00000000, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF).AsSingle()),
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

        Assert.That(() => VectorUtilities.CompareTrueAny(Vector128.Create(0x00000000, 0x00000000, 0x00000000, 0x00000000).AsSingle()),
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

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromY(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 1.0f, 1.0f, 1.0f))
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

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromW(Vector128{float})" /> method.</summary>
    [Test]
    public static void CreateFromWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 3.0f, 3.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXXYW(Vector128{float})" /> method.</summary>
    public static void CreateFromXXYWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXXYW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 0.0f, 1.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXXYY(Vector128{float})" /> method.</summary>
    public static void CreateFromXXYYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXXYY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 0.0f, 1.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXZXZ(Vector128{float})" /> method.</summary>
    public static void CreateFromXZXZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXZXZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 2.0f, 0.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXZYW(Vector128{float})" /> method.</summary>
    public static void CreateFromXZYWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXZYW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 2.0f, 1.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXZWY(Vector128{float})" /> method.</summary>
    public static void CreateFromXZWYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXZWY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 2.0f, 3.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXWZX(Vector128{float})" /> method.</summary>
    public static void CreateFromXWZXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXWZX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 3.0f, 2.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYXXX(Vector128{float})" /> method.</summary>
    public static void CreateFromYXXXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYXXX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 0.0f, 0.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYXXW(Vector128{float})" /> method.</summary>
    public static void CreateFromYXXWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYXXW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 0.0f, 0.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYXZW(Vector128{float})" /> method.</summary>
    public static void CreateFromYXZWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYXZW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 0.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYXWZ(Vector128{float})" /> method.</summary>
    public static void CreateFromYXWZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYXWZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 0.0f, 3.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYZXY(Vector128{float})" /> method.</summary>
    public static void CreateFromYZXYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYZXY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 2.0f, 0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYZXW(Vector128{float})" /> method.</summary>
    public static void CreateFromYZXWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYZXW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 2.0f, 0.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYWXZ(Vector128{float})" /> method.</summary>
    public static void CreateFromYWXZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYWXZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 3.0f, 0.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZXYX(Vector128{float})" /> method.</summary>
    public static void CreateFromZXYXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZXYX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 0.0f, 1.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZXYW(Vector128{float})" /> method.</summary>
    public static void CreateFromZXYWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZXYW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 0.0f, 1.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZXWY(Vector128{float})" /> method.</summary>
    public static void CreateFromZXWYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZXWY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 0.0f, 3.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZYXW(Vector128{float})" /> method.</summary>
    public static void CreateFromZYXWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYXZW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 1.0f, 0.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZYZW(Vector128{float})" /> method.</summary>
    public static void CreateFromZYZWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZYZW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 1.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZZYY(Vector128{float})" /> method.</summary>
    public static void CreateFromZZYYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZZYY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 2.0f, 1.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZZYW(Vector128{float})" /> method.</summary>
    public static void CreateFromZZYWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZZYW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 2.0f, 1.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZWXY(Vector128{float})" /> method.</summary>
    public static void CreateFromZWXYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZWXY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 3.0f, 0.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZWYZ(Vector128{float})" /> method.</summary>
    public static void CreateFromZWYZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZWYZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 3.0f, 1.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZWZW(Vector128{float})" /> method.</summary>
    public static void CreateFromZWZWTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZWZW(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 3.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromWXYZ(Vector128{float})" /> method.</summary>
    public static void CreateFromWXYZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromWXYZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 0.0f, 1.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromWXWX(Vector128{float})" /> method.</summary>
    public static void CreateFromWXWXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromWXWX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 0.0f, 3.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromWZYX(Vector128{float})" /> method.</summary>
    public static void CreateFromWZYXTest()
    {
        Assert.That(() => VectorUtilities.CreateFromWZYX(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 2.0f, 1.0f, 0.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromWZWY(Vector128{float})" /> method.</summary>
    public static void CreateFromWZWYTest()
    {
        Assert.That(() => VectorUtilities.CreateFromWZWY(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 2.0f, 3.0f, 1.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromWWWZ(Vector128{float})" /> method.</summary>
    public static void CreateFromWWWZTest()
    {
        Assert.That(() => VectorUtilities.CreateFromWWWZ(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 3.0f, 3.0f, 2.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromBYBB(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromBYBBTest()
    {
        Assert.That(() => VectorUtilities.CreateFromBYBB(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(5.0f, 1.0f, 5.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromCCZC(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromCCZCTest()
    {
        Assert.That(() => VectorUtilities.CreateFromCCZC(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(6.0f, 6.0f, 3.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXAAA(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromXAAATest()
    {
        Assert.That(() => VectorUtilities.CreateFromXAAA(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 4.0f, 4.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXXAB(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromXXABTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXXAB(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 0.0f, 4.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXXCC(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromXXCCTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXXCC(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 0.0f, 6.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXYAA(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromXYAATest()
    {
        Assert.That(() => VectorUtilities.CreateFromXYAA(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 4.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXYAC(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromXYACTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXYAC(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 4.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXYCC(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromXYCCTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXYCC(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 6.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXYCD(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromXYCDTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXYCD(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 1.0f, 6.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXZAC(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromXZACTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXZAC(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 2.0f, 4.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXZBD(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromXZBDTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXZBD(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 2.0f, 5.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXWAB(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromXWABTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXWAB(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 3.0f, 4.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXWAD(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromXWADTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXWAD(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 3.0f, 4.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromXWCD(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromXWCDTest()
    {
        Assert.That(() => VectorUtilities.CreateFromXWCD(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(0.0f, 3.0f, 6.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYXAA(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromYXAATest()
    {
        Assert.That(() => VectorUtilities.CreateFromYXAA(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 0.0f, 4.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYZAB(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromYZABTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYZAB(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 2.0f, 4.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYZBC(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromYZBCTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYZBC(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 2.0f, 5.0f, 6.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYZCB(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromYZCBTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYZCB(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 2.0f, 6.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYWBB(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromYWBBTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYWBB(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 3.0f, 5.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYWBD(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromYWBDTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYWBD(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 3.0f, 5.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYWCD(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromYWCDTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYWCD(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 3.0f, 6.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromYWDD(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromYWDDTest()
    {
        Assert.That(() => VectorUtilities.CreateFromYWDD(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(1.0f, 3.0f, 7.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZXDA(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromZXDATest()
    {
        Assert.That(() => VectorUtilities.CreateFromZXDA(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 0.0f, 7.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZYBD(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromZYBDTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZYBD(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 1.0f, 5.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZYCA(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromZYCATest()
    {
        Assert.That(() => VectorUtilities.CreateFromZYCA(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 1.0f, 6.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZZAB(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromZZABTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZZAB(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 2.0f, 4.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZZCB(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromZZCBTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZZCB(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 2.0f, 6.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZZCD(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromZZCDTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZZCD(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 2.0f, 6.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZWCA(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromZWCATest()
    {
        Assert.That(() => VectorUtilities.CreateFromZWCA(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 3.0f, 6.0f, 4.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromZWCB(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromZWCBTest()
    {
        Assert.That(() => VectorUtilities.CreateFromZWCB(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(2.0f, 3.0f, 6.0f, 5.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromWXAD(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromWXADTest()
    {
        Assert.That(() => VectorUtilities.CreateFromWXAD(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 0.0f, 4.0f, 7.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="VectorUtilities.CreateFromWXBC(Vector128{float}, Vector128{float})" /> method.</summary>
    public static void CreateFromWXBCTest()
    {
        Assert.That(() => VectorUtilities.CreateFromWXBC(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f), Vector128.Create(4.0f, 5.0f, 6.0f, 7.0f)),
            Is.EqualTo(Vector128.Create(3.0f, 0.0f, 5.0f, 6.0f))
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
