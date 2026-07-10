// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Runtime.Intrinsics;
using System.Text;
using NUnit.Framework;
using TerraFX.Numerics;

namespace TerraFX.UnitTests.Numerics;

/// <summary>Provides a set of tests covering the <see cref="ColorRgba" /> struct.</summary>
[TestFixture(TestOf = typeof(ColorRgba))]
internal static class ColorRgbaTests
{
    /// <summary>Provides validation of the <see cref="ColorRgba(float, float, float, float)" /> constructor.</summary>
    [Test]
    public static void ComponentConstructorTest()
    {
        var value = new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f);

        Assert.That(() => value.Red, Is.EqualTo(0.0f));
        Assert.That(() => value.Green, Is.EqualTo(1.0f));
        Assert.That(() => value.Blue, Is.EqualTo(2.0f));
        Assert.That(() => value.Alpha, Is.EqualTo(3.0f));
    }

    /// <summary>Provides validation of the <see cref="ColorRgba(Vector128{float})" /> constructor.</summary>
    [Test]
    public static void VectorConstructorTest()
    {
        var value = new ColorRgba(Vector128.Create(0.0f, 1.0f, 2.0f, 3.0f));

        Assert.That(() => value.Red, Is.EqualTo(0.0f));
        Assert.That(() => value.Green, Is.EqualTo(1.0f));
        Assert.That(() => value.Blue, Is.EqualTo(2.0f));
        Assert.That(() => value.Alpha, Is.EqualTo(3.0f));
    }

    /// <summary>Provides validation of the <see cref="ColorRgba.op_Equality" /> method.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f) == new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f), Is.True);
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f) == new ColorRgba(4.0f, 5.0f, 6.0f, 7.0f), Is.False);
    }

    /// <summary>Provides validation of the <see cref="ColorRgba.op_Inequality" /> method.</summary>
    [Test]
    public static void OpInequalityTest()
    {
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f) != new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f), Is.False);
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f) != new ColorRgba(4.0f, 5.0f, 6.0f, 7.0f), Is.True);
    }

    /// <summary>Provides validation of the <see cref="ColorRgba.Equals(ColorRgba)" /> and <see cref="ColorRgba.Equals(object)" /> methods.</summary>
    [Test]
    public static void EqualsTest()
    {
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).Equals(new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f)), Is.True);
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).Equals(new ColorRgba(4.0f, 5.0f, 6.0f, 7.0f)), Is.False);

        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).Equals((object)new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f)), Is.True);
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).Equals(null), Is.False);
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).Equals("not a color"), Is.False);
    }

    /// <summary>Provides validation of the <see cref="ColorRgba.GetHashCode()" /> method.</summary>
    [Test]
    public static void GetHashCodeTest()
    {
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).GetHashCode(),
            Is.EqualTo(new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).GetHashCode())
        );
    }

    /// <summary>Provides validation of the <see cref="ColorRgba.ToString(string, IFormatProvider)" /> method.</summary>
    [Test]
    public static void ToStringTest()
    {
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).ToString(null, CultureInfo.InvariantCulture),
            Is.EqualTo("ColorRgba { Red = 0, Green = 1, Blue = 2, Alpha = 3 }")
        );
    }

    /// <summary>Provides validation of the <see cref="ColorRgba.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider)" /> method.</summary>
    [Test]
    public static void TryFormatUtf16Test()
    {
        Span<char> destination = stackalloc char[64];

        var result = new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).TryFormat(destination, out var charsWritten, provider: CultureInfo.InvariantCulture);

        Assert.That(result, Is.True);
        Assert.That(destination.Slice(0, charsWritten).ToString(), Is.EqualTo("ColorRgba { Red = 0, Green = 1, Blue = 2, Alpha = 3 }"));
    }

    /// <summary>Provides validation of the <see cref="ColorRgba.TryFormat(Span{byte}, out int, ReadOnlySpan{char}, IFormatProvider)" /> method.</summary>
    [Test]
    public static void TryFormatUtf8Test()
    {
        Span<byte> utf8Destination = stackalloc byte[64];

        var result = new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).TryFormat(utf8Destination, out var bytesWritten, provider: CultureInfo.InvariantCulture);

        Assert.That(result, Is.True);
        Assert.That(Encoding.UTF8.GetString(utf8Destination.Slice(0, bytesWritten)), Is.EqualTo("ColorRgba { Red = 0, Green = 1, Blue = 2, Alpha = 3 }"));
    }

    /// <summary>Provides validation of the <see cref="ColorRgba.WithRed(float)" /> method.</summary>
    [Test]
    public static void WithRedTest()
    {
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).WithRed(5.0f),
            Is.EqualTo(new ColorRgba(5.0f, 1.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="ColorRgba.WithGreen(float)" /> method.</summary>
    [Test]
    public static void WithGreenTest()
    {
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).WithGreen(5.0f),
            Is.EqualTo(new ColorRgba(0.0f, 5.0f, 2.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="ColorRgba.WithBlue(float)" /> method.</summary>
    [Test]
    public static void WithBlueTest()
    {
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).WithBlue(5.0f),
            Is.EqualTo(new ColorRgba(0.0f, 1.0f, 5.0f, 3.0f))
        );
    }

    /// <summary>Provides validation of the <see cref="ColorRgba.WithAlpha(float)" /> method.</summary>
    [Test]
    public static void WithAlphaTest()
    {
        Assert.That(() => new ColorRgba(0.0f, 1.0f, 2.0f, 3.0f).WithAlpha(5.0f),
            Is.EqualTo(new ColorRgba(0.0f, 1.0f, 2.0f, 5.0f))
        );
    }
}
