// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using NUnit.Framework;
using TerraFX.Numerics;

namespace TerraFX.UnitTests.Numerics;

/// <summary>Provides a set of tests covering the <see cref="BoundingBox" /> struct.</summary>
[TestFixture(TestOf = typeof(BoundingBox))]
internal static class BoundingBoxTests
{
    /// <summary>Provides validation of the <see cref="BoundingBox.Zero" /> property.</summary>
    [Test]
    public static void ZeroTest()
    {
        Assert.That(() => BoundingBox.Zero.Center, Is.EqualTo(Vector3.Zero));
        Assert.That(() => BoundingBox.Zero.Extent, Is.EqualTo(Vector3.Zero));
    }

    /// <summary>Provides validation of the <see cref="BoundingBox.CreateFromExtent(Vector3, Vector3)" /> method.</summary>
    [Test]
    public static void CreateFromExtentTest()
    {
        var box = BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(4.0f, 5.0f, 6.0f));

        Assert.That(() => box.Center, Is.EqualTo(Vector3.Create(1.0f, 2.0f, 3.0f)));
        Assert.That(() => box.Extent, Is.EqualTo(Vector3.Create(4.0f, 5.0f, 6.0f)));
    }

    /// <summary>Provides validation of the <see cref="BoundingBox.CreateFromSize(Vector3, Vector3)" /> method.</summary>
    [Test]
    public static void CreateFromSizeTest()
    {
        var box = BoundingBox.CreateFromSize(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(8.0f, 10.0f, 12.0f));

        Assert.That(() => box.Location, Is.EqualTo(Vector3.Create(1.0f, 2.0f, 3.0f)));
        Assert.That(() => box.Size, Is.EqualTo(Vector3.Create(8.0f, 10.0f, 12.0f)));
        Assert.That(() => box.Center, Is.EqualTo(Vector3.Create(5.0f, 7.0f, 9.0f)));
        Assert.That(() => box.Extent, Is.EqualTo(Vector3.Create(4.0f, 5.0f, 6.0f)));
    }

    /// <summary>Provides validation of the derived getters (Location, Size, Width, Height, Depth, X, Y, Z).</summary>
    [Test]
    public static void DerivedGettersTest()
    {
        var box = BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(4.0f, 5.0f, 6.0f));

        Assert.That(() => box.Width, Is.EqualTo(8.0f));
        Assert.That(() => box.Height, Is.EqualTo(10.0f));
        Assert.That(() => box.Depth, Is.EqualTo(12.0f));

        Assert.That(() => box.X, Is.EqualTo(-3.0f));
        Assert.That(() => box.Y, Is.EqualTo(-3.0f));
        Assert.That(() => box.Z, Is.EqualTo(-3.0f));

        Assert.That(() => box.Location, Is.EqualTo(Vector3.Create(-3.0f, -3.0f, -3.0f)));
        Assert.That(() => box.Size, Is.EqualTo(Vector3.Create(8.0f, 10.0f, 12.0f)));
    }

    /// <summary>Provides validation of the scalar extent setters (Width, Height, Depth).</summary>
    [Test]
    public static void ExtentSettersTest()
    {
        var box = BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(4.0f, 5.0f, 6.0f));

        box.Width = 20.0f;
        box.Height = 30.0f;
        box.Depth = 40.0f;

        Assert.That(() => box.Extent, Is.EqualTo(Vector3.Create(10.0f, 15.0f, 20.0f)));
        Assert.That(() => box.Center, Is.EqualTo(Vector3.Create(1.0f, 2.0f, 3.0f)));
    }

    /// <summary>Provides validation of the scalar location setters (X, Y, Z).</summary>
    [Test]
    public static void LocationSettersTest()
    {
        var box = BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(4.0f, 5.0f, 6.0f));

        box.X = 0.0f;
        box.Y = 0.0f;
        box.Z = 0.0f;

        Assert.That(() => box.Location, Is.EqualTo(Vector3.Zero));
        Assert.That(() => box.Extent, Is.EqualTo(Vector3.Create(4.0f, 5.0f, 6.0f)));
    }

    /// <summary>Provides validation of the <see cref="BoundingBox.op_Equality" /> method.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        var left = BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(4.0f, 5.0f, 6.0f));
        var right = BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(4.0f, 5.0f, 6.0f));
        var other = BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(7.0f, 8.0f, 9.0f));

        Assert.That(() => left == right, Is.True);
        Assert.That(() => left == other, Is.False);
    }

    /// <summary>Provides validation of the <see cref="BoundingBox.op_Inequality" /> method.</summary>
    [Test]
    public static void OpInequalityTest()
    {
        var left = BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(4.0f, 5.0f, 6.0f));
        var right = BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(4.0f, 5.0f, 6.0f));
        var other = BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(7.0f, 8.0f, 9.0f));

        Assert.That(() => left != right, Is.False);
        Assert.That(() => left != other, Is.True);
    }

    /// <summary>Provides validation of the <see cref="BoundingBox.Equals(BoundingBox)" /> and <see cref="BoundingBox.Equals(object)" /> methods.</summary>
    [Test]
    public static void EqualsTest()
    {
        var box = BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(4.0f, 5.0f, 6.0f));

        Assert.That(() => box.Equals(box), Is.True);
        Assert.That(() => box.Equals((object)box), Is.True);
        Assert.That(() => box.Equals(BoundingBox.Zero), Is.False);
        Assert.That(() => box.Equals(null), Is.False);
        Assert.That(() => box.Equals("not a box"), Is.False);
    }

    /// <summary>Provides validation of the <see cref="BoundingBox.GetHashCode()" /> method.</summary>
    [Test]
    public static void GetHashCodeTest()
    {
        Assert.That(() => BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(4.0f, 5.0f, 6.0f)).GetHashCode(),
            Is.EqualTo(BoundingBox.CreateFromExtent(Vector3.Create(1.0f, 2.0f, 3.0f), Vector3.Create(4.0f, 5.0f, 6.0f)).GetHashCode())
        );
    }

    /// <summary>Provides validation of the <see cref="BoundingBox.ToString(string, IFormatProvider)" /> method.</summary>
    [Test]
    public static void ToStringTest()
    {
        var center = Vector3.Create(1.0f, 2.0f, 3.0f);
        var extent = Vector3.Create(4.0f, 5.0f, 6.0f);
        var box = BoundingBox.CreateFromExtent(center, extent);

        var expected = $"BoundingBox {{ Center = {center.ToString(null, CultureInfo.InvariantCulture)}, Extent = {extent.ToString(null, CultureInfo.InvariantCulture)} }}";

        Assert.That(() => box.ToString(null, CultureInfo.InvariantCulture), Is.EqualTo(expected));
    }

    /// <summary>Provides validation of the <see cref="BoundingBox.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider)" /> method.</summary>
    [Test]
    public static void TryFormatUtf16Test()
    {
        var center = Vector3.Create(1.0f, 2.0f, 3.0f);
        var extent = Vector3.Create(4.0f, 5.0f, 6.0f);
        var box = BoundingBox.CreateFromExtent(center, extent);

        var expected = $"BoundingBox {{ Center = {center.ToString(null, CultureInfo.InvariantCulture)}, Extent = {extent.ToString(null, CultureInfo.InvariantCulture)} }}";

        Span<char> destination = stackalloc char[128];
        var result = box.TryFormat(destination, out var charsWritten, provider: CultureInfo.InvariantCulture);

        Assert.That(result, Is.True);
        Assert.That(destination.Slice(0, charsWritten).ToString(), Is.EqualTo(expected));
    }

    /// <summary>Provides validation of the <see cref="BoundingBox.TryFormat(Span{byte}, out int, ReadOnlySpan{char}, IFormatProvider)" /> method.</summary>
    [Test]
    public static void TryFormatUtf8Test()
    {
        var center = Vector3.Create(1.0f, 2.0f, 3.0f);
        var extent = Vector3.Create(4.0f, 5.0f, 6.0f);
        var box = BoundingBox.CreateFromExtent(center, extent);

        var expected = $"BoundingBox {{ Center = {center.ToString(null, CultureInfo.InvariantCulture)}, Extent = {extent.ToString(null, CultureInfo.InvariantCulture)} }}";

        Span<byte> utf8Destination = stackalloc byte[128];
        var result = box.TryFormat(utf8Destination, out var bytesWritten, provider: CultureInfo.InvariantCulture);

        Assert.That(result, Is.True);
        Assert.That(Encoding.UTF8.GetString(utf8Destination.Slice(0, bytesWritten)), Is.EqualTo(expected));
    }
}
