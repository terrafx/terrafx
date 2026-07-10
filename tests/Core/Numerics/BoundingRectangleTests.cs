// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using NUnit.Framework;
using TerraFX.Numerics;

namespace TerraFX.UnitTests.Numerics;

/// <summary>Provides a set of tests covering the <see cref="BoundingRectangle" /> struct.</summary>
[TestFixture(TestOf = typeof(BoundingRectangle))]
internal static class BoundingRectangleTests
{
    /// <summary>Provides validation of the <see cref="BoundingRectangle.Zero" /> property.</summary>
    [Test]
    public static void ZeroTest()
    {
        Assert.That(() => BoundingRectangle.Zero.Center, Is.EqualTo(Vector2.Zero));
        Assert.That(() => BoundingRectangle.Zero.Extent, Is.EqualTo(Vector2.Zero));
    }

    /// <summary>Provides validation of the <see cref="BoundingRectangle.CreateFromExtent(Vector2, Vector2)" /> method.</summary>
    [Test]
    public static void CreateFromExtentTest()
    {
        var rectangle = BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(4.0f, 5.0f));

        Assert.That(() => rectangle.Center, Is.EqualTo(Vector2.Create(1.0f, 2.0f)));
        Assert.That(() => rectangle.Extent, Is.EqualTo(Vector2.Create(4.0f, 5.0f)));
    }

    /// <summary>Provides validation of the <see cref="BoundingRectangle.CreateFromSize(Vector2, Vector2)" /> method.</summary>
    [Test]
    public static void CreateFromSizeTest()
    {
        var rectangle = BoundingRectangle.CreateFromSize(Vector2.Create(1.0f, 2.0f), Vector2.Create(8.0f, 10.0f));

        Assert.That(() => rectangle.Location, Is.EqualTo(Vector2.Create(1.0f, 2.0f)));
        Assert.That(() => rectangle.Size, Is.EqualTo(Vector2.Create(8.0f, 10.0f)));
        Assert.That(() => rectangle.Center, Is.EqualTo(Vector2.Create(5.0f, 7.0f)));
        Assert.That(() => rectangle.Extent, Is.EqualTo(Vector2.Create(4.0f, 5.0f)));
    }

    /// <summary>Provides validation of the derived getters (Location, Size, Width, Height, X, Y).</summary>
    [Test]
    public static void DerivedGettersTest()
    {
        var rectangle = BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(4.0f, 5.0f));

        Assert.That(() => rectangle.Width, Is.EqualTo(8.0f));
        Assert.That(() => rectangle.Height, Is.EqualTo(10.0f));

        Assert.That(() => rectangle.X, Is.EqualTo(-3.0f));
        Assert.That(() => rectangle.Y, Is.EqualTo(-3.0f));

        Assert.That(() => rectangle.Location, Is.EqualTo(Vector2.Create(-3.0f, -3.0f)));
        Assert.That(() => rectangle.Size, Is.EqualTo(Vector2.Create(8.0f, 10.0f)));
    }

    /// <summary>Provides validation of the scalar extent setters (Width, Height).</summary>
    [Test]
    public static void ExtentSettersTest()
    {
        var rectangle = BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(4.0f, 5.0f));

        rectangle.Width = 20.0f;
        rectangle.Height = 30.0f;

        Assert.That(() => rectangle.Extent, Is.EqualTo(Vector2.Create(10.0f, 15.0f)));
        Assert.That(() => rectangle.Center, Is.EqualTo(Vector2.Create(1.0f, 2.0f)));
    }

    /// <summary>Provides validation of the scalar location setters (X, Y).</summary>
    [Test]
    public static void LocationSettersTest()
    {
        var rectangle = BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(4.0f, 5.0f));

        rectangle.X = 0.0f;
        rectangle.Y = 0.0f;

        Assert.That(() => rectangle.Location, Is.EqualTo(Vector2.Zero));
        Assert.That(() => rectangle.Extent, Is.EqualTo(Vector2.Create(4.0f, 5.0f)));
    }

    /// <summary>Provides validation of the <see cref="BoundingRectangle.op_Equality" /> method.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        var left = BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(4.0f, 5.0f));
        var right = BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(4.0f, 5.0f));
        var other = BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(6.0f, 7.0f));

        Assert.That(() => left == right, Is.True);
        Assert.That(() => left == other, Is.False);
    }

    /// <summary>Provides validation of the <see cref="BoundingRectangle.op_Inequality" /> method.</summary>
    [Test]
    public static void OpInequalityTest()
    {
        var left = BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(4.0f, 5.0f));
        var right = BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(4.0f, 5.0f));
        var other = BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(6.0f, 7.0f));

        Assert.That(() => left != right, Is.False);
        Assert.That(() => left != other, Is.True);
    }

    /// <summary>Provides validation of the <see cref="BoundingRectangle.Equals(BoundingRectangle)" /> and <see cref="BoundingRectangle.Equals(object)" /> methods.</summary>
    [Test]
    public static void EqualsTest()
    {
        var rectangle = BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(4.0f, 5.0f));

        Assert.That(() => rectangle.Equals(rectangle), Is.True);
        Assert.That(() => rectangle.Equals((object)rectangle), Is.True);
        Assert.That(() => rectangle.Equals(BoundingRectangle.Zero), Is.False);
        Assert.That(() => rectangle.Equals(null), Is.False);
        Assert.That(() => rectangle.Equals("not a rectangle"), Is.False);
    }

    /// <summary>Provides validation of the <see cref="BoundingRectangle.GetHashCode()" /> method.</summary>
    [Test]
    public static void GetHashCodeTest()
    {
        Assert.That(() => BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(4.0f, 5.0f)).GetHashCode(),
            Is.EqualTo(BoundingRectangle.CreateFromExtent(Vector2.Create(1.0f, 2.0f), Vector2.Create(4.0f, 5.0f)).GetHashCode())
        );
    }

    /// <summary>Provides validation of the <see cref="BoundingRectangle.ToString(string, IFormatProvider)" /> method.</summary>
    [Test]
    public static void ToStringTest()
    {
        var center = Vector2.Create(1.0f, 2.0f);
        var extent = Vector2.Create(4.0f, 5.0f);
        var rectangle = BoundingRectangle.CreateFromExtent(center, extent);

        var expected = $"BoundingRectangle {{ Center = {center.ToString(null, CultureInfo.InvariantCulture)}, Extent = {extent.ToString(null, CultureInfo.InvariantCulture)} }}";

        Assert.That(() => rectangle.ToString(null, CultureInfo.InvariantCulture), Is.EqualTo(expected));
    }

    /// <summary>Provides validation of the <see cref="BoundingRectangle.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider)" /> method.</summary>
    [Test]
    public static void TryFormatUtf16Test()
    {
        var center = Vector2.Create(1.0f, 2.0f);
        var extent = Vector2.Create(4.0f, 5.0f);
        var rectangle = BoundingRectangle.CreateFromExtent(center, extent);

        var expected = $"BoundingRectangle {{ Center = {center.ToString(null, CultureInfo.InvariantCulture)}, Extent = {extent.ToString(null, CultureInfo.InvariantCulture)} }}";

        Span<char> destination = stackalloc char[128];
        var result = rectangle.TryFormat(destination, out var charsWritten, provider: CultureInfo.InvariantCulture);

        Assert.That(result, Is.True);
        Assert.That(destination.Slice(0, charsWritten).ToString(), Is.EqualTo(expected));
    }

    /// <summary>Provides validation of the <see cref="BoundingRectangle.TryFormat(Span{byte}, out int, ReadOnlySpan{char}, IFormatProvider)" /> method.</summary>
    [Test]
    public static void TryFormatUtf8Test()
    {
        var center = Vector2.Create(1.0f, 2.0f);
        var extent = Vector2.Create(4.0f, 5.0f);
        var rectangle = BoundingRectangle.CreateFromExtent(center, extent);

        var expected = $"BoundingRectangle {{ Center = {center.ToString(null, CultureInfo.InvariantCulture)}, Extent = {extent.ToString(null, CultureInfo.InvariantCulture)} }}";

        Span<byte> utf8Destination = stackalloc byte[128];
        var result = rectangle.TryFormat(utf8Destination, out var bytesWritten, provider: CultureInfo.InvariantCulture);

        Assert.That(result, Is.True);
        Assert.That(Encoding.UTF8.GetString(utf8Destination.Slice(0, bytesWritten)), Is.EqualTo(expected));
    }
}
