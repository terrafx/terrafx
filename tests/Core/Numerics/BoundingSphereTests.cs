// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using NUnit.Framework;
using TerraFX.Numerics;

namespace TerraFX.UnitTests.Numerics;

/// <summary>Provides a set of tests covering the <see cref="BoundingSphere" /> struct.</summary>
[TestFixture(TestOf = typeof(BoundingSphere))]
internal static class BoundingSphereTests
{
    /// <summary>Provides validation of the <see cref="BoundingSphere.Zero" /> property.</summary>
    [Test]
    public static void ZeroTest()
    {
        Assert.That(() => BoundingSphere.Zero.Center, Is.EqualTo(Vector3.Zero));
        Assert.That(() => BoundingSphere.Zero.Radius, Is.EqualTo(0.0f));
    }

    /// <summary>Provides validation of the <see cref="BoundingSphere.CreateFromRadius(Vector3, float)" /> method.</summary>
    [Test]
    public static void CreateFromRadiusTest()
    {
        var sphere = BoundingSphere.CreateFromRadius(Vector3.Create(1.0f, 2.0f, 3.0f), 5.0f);

        Assert.That(() => sphere.Center, Is.EqualTo(Vector3.Create(1.0f, 2.0f, 3.0f)));
        Assert.That(() => sphere.Radius, Is.EqualTo(5.0f));
    }

    /// <summary>Provides validation of the <see cref="BoundingSphere.Center" /> and <see cref="BoundingSphere.Radius" /> setters.</summary>
    [Test]
    public static void SettersTest()
    {
        var sphere = BoundingSphere.Zero;

        sphere.Center = Vector3.Create(1.0f, 2.0f, 3.0f);
        sphere.Radius = 5.0f;

        Assert.That(() => sphere.Center, Is.EqualTo(Vector3.Create(1.0f, 2.0f, 3.0f)));
        Assert.That(() => sphere.Radius, Is.EqualTo(5.0f));
    }

    /// <summary>Provides validation of the <see cref="BoundingSphere.op_Equality" /> method.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        var left = BoundingSphere.CreateFromRadius(Vector3.Create(1.0f, 2.0f, 3.0f), 5.0f);
        var right = BoundingSphere.CreateFromRadius(Vector3.Create(1.0f, 2.0f, 3.0f), 5.0f);
        var other = BoundingSphere.CreateFromRadius(Vector3.Create(1.0f, 2.0f, 3.0f), 6.0f);

        Assert.That(() => left == right, Is.True);
        Assert.That(() => left == other, Is.False);
    }

    /// <summary>Provides validation of the <see cref="BoundingSphere.op_Inequality" /> method.</summary>
    [Test]
    public static void OpInequalityTest()
    {
        var left = BoundingSphere.CreateFromRadius(Vector3.Create(1.0f, 2.0f, 3.0f), 5.0f);
        var right = BoundingSphere.CreateFromRadius(Vector3.Create(1.0f, 2.0f, 3.0f), 5.0f);
        var other = BoundingSphere.CreateFromRadius(Vector3.Create(1.0f, 2.0f, 3.0f), 6.0f);

        Assert.That(() => left != right, Is.False);
        Assert.That(() => left != other, Is.True);
    }

    /// <summary>Provides validation of the <see cref="BoundingSphere.Equals(BoundingSphere)" /> and <see cref="BoundingSphere.Equals(object)" /> methods.</summary>
    [Test]
    public static void EqualsTest()
    {
        var sphere = BoundingSphere.CreateFromRadius(Vector3.Create(1.0f, 2.0f, 3.0f), 5.0f);

        Assert.That(() => sphere.Equals(sphere), Is.True);
        Assert.That(() => sphere.Equals((object)sphere), Is.True);
        Assert.That(() => sphere.Equals(BoundingSphere.Zero), Is.False);
        Assert.That(() => sphere.Equals(null), Is.False);
        Assert.That(() => sphere.Equals("not a sphere"), Is.False);
    }

    /// <summary>Provides validation of the <see cref="BoundingSphere.GetHashCode()" /> method.</summary>
    [Test]
    public static void GetHashCodeTest()
    {
        Assert.That(() => BoundingSphere.CreateFromRadius(Vector3.Create(1.0f, 2.0f, 3.0f), 5.0f).GetHashCode(),
            Is.EqualTo(BoundingSphere.CreateFromRadius(Vector3.Create(1.0f, 2.0f, 3.0f), 5.0f).GetHashCode())
        );
    }

    /// <summary>Provides validation of the <see cref="BoundingSphere.ToString(string, IFormatProvider)" /> method.</summary>
    [Test]
    public static void ToStringTest()
    {
        var center = Vector3.Create(1.0f, 2.0f, 3.0f);
        var sphere = BoundingSphere.CreateFromRadius(center, 5.0f);

        var expected = $"BoundingSphere {{ Center = {center.ToString(null, CultureInfo.InvariantCulture)}, Radius = {5.0f.ToString(null, CultureInfo.InvariantCulture)} }}";

        Assert.That(() => sphere.ToString(null, CultureInfo.InvariantCulture), Is.EqualTo(expected));
    }

    /// <summary>Provides validation of the <see cref="BoundingSphere.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider)" /> method.</summary>
    [Test]
    public static void TryFormatUtf16Test()
    {
        var center = Vector3.Create(1.0f, 2.0f, 3.0f);
        var sphere = BoundingSphere.CreateFromRadius(center, 5.0f);

        var expected = $"BoundingSphere {{ Center = {center.ToString(null, CultureInfo.InvariantCulture)}, Radius = {5.0f.ToString(null, CultureInfo.InvariantCulture)} }}";

        Span<char> destination = stackalloc char[128];
        var result = sphere.TryFormat(destination, out var charsWritten, provider: CultureInfo.InvariantCulture);

        Assert.That(result, Is.True);
        Assert.That(destination.Slice(0, charsWritten).ToString(), Is.EqualTo(expected));
    }

    /// <summary>Provides validation of the <see cref="BoundingSphere.TryFormat(Span{byte}, out int, ReadOnlySpan{char}, IFormatProvider)" /> method.</summary>
    [Test]
    public static void TryFormatUtf8Test()
    {
        var center = Vector3.Create(1.0f, 2.0f, 3.0f);
        var sphere = BoundingSphere.CreateFromRadius(center, 5.0f);

        var expected = $"BoundingSphere {{ Center = {center.ToString(null, CultureInfo.InvariantCulture)}, Radius = {5.0f.ToString(null, CultureInfo.InvariantCulture)} }}";

        Span<byte> utf8Destination = stackalloc byte[128];
        var result = sphere.TryFormat(utf8Destination, out var bytesWritten, provider: CultureInfo.InvariantCulture);

        Assert.That(result, Is.True);
        Assert.That(Encoding.UTF8.GetString(utf8Destination.Slice(0, bytesWritten)), Is.EqualTo(expected));
    }
}
