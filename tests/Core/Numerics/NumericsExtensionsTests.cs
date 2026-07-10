// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;
using TerraFX.Numerics;

namespace TerraFX.UnitTests.Numerics;

/// <summary>Provides a set of tests covering the <see cref="NumericsExtensions" /> static class.</summary>
[TestFixture(TestOf = typeof(NumericsExtensions))]
internal static class NumericsExtensionsTests
{
    /// <summary>Provides validation of the <see cref="AffineTransform" /> setters.</summary>
    [Test]
    public static void AffineTransformSettersTest()
    {
        var transform = AffineTransform.Identity;

        var rotation = Quaternion.Create(0.0f, 0.0f, 0.0f, 1.0f);
        var scale = Vector3.Create(2.0f, 3.0f, 4.0f);
        var translation = Vector3.Create(5.0f, 6.0f, 7.0f);

        var result = transform.SetRotation(rotation)
                                      .SetScale(scale)
                                      .SetTranslation(translation);

        Assert.That(() => result.Rotation, Is.EqualTo(rotation));
        Assert.That(() => result.Scale, Is.EqualTo(scale));
        Assert.That(() => result.Translation, Is.EqualTo(translation));
    }

    /// <summary>Provides validation of the <see cref="BoundingBox" /> setters.</summary>
    [Test]
    public static void BoundingBoxSettersTest()
    {
        var box = BoundingBox.Zero;

        var result = box.SetCenter(Vector3.Create(1.0f, 2.0f, 3.0f))
                                .SetExtent(Vector3.Create(4.0f, 5.0f, 6.0f));

        Assert.That(() => result.Center, Is.EqualTo(Vector3.Create(1.0f, 2.0f, 3.0f)));
        Assert.That(() => result.Extent, Is.EqualTo(Vector3.Create(4.0f, 5.0f, 6.0f)));

        result = box.SetWidth(8.0f)
                        .SetHeight(10.0f)
                        .SetDepth(12.0f);

        Assert.That(() => result.Width, Is.EqualTo(8.0f));
        Assert.That(() => result.Height, Is.EqualTo(10.0f));
        Assert.That(() => result.Depth, Is.EqualTo(12.0f));

        result = box.SetX(-1.0f)
                        .SetY(-2.0f)
                        .SetZ(-3.0f);

        Assert.That(() => result.X, Is.EqualTo(-1.0f));
        Assert.That(() => result.Y, Is.EqualTo(-2.0f));
        Assert.That(() => result.Z, Is.EqualTo(-3.0f));

        result = box.SetSize(Vector3.Create(2.0f, 4.0f, 6.0f))
                        .SetLocation(Vector3.Create(0.0f, 0.0f, 0.0f));

        Assert.That(() => result.Location, Is.EqualTo(Vector3.Create(0.0f, 0.0f, 0.0f)));
        Assert.That(() => result.Size, Is.EqualTo(Vector3.Create(2.0f, 4.0f, 6.0f)));
    }

    /// <summary>Provides validation of the <see cref="BoundingRectangle" /> setters.</summary>
    [Test]
    public static void BoundingRectangleSettersTest()
    {
        var rectangle = BoundingRectangle.Zero;

        var result = rectangle.SetCenter(Vector2.Create(1.0f, 2.0f))
                                      .SetExtent(Vector2.Create(4.0f, 5.0f));

        Assert.That(() => result.Center, Is.EqualTo(Vector2.Create(1.0f, 2.0f)));
        Assert.That(() => result.Extent, Is.EqualTo(Vector2.Create(4.0f, 5.0f)));

        result = rectangle.SetWidth(8.0f)
                              .SetHeight(10.0f);

        Assert.That(() => result.Width, Is.EqualTo(8.0f));
        Assert.That(() => result.Height, Is.EqualTo(10.0f));

        result = rectangle.SetX(-1.0f)
                              .SetY(-2.0f);

        Assert.That(() => result.X, Is.EqualTo(-1.0f));
        Assert.That(() => result.Y, Is.EqualTo(-2.0f));

        result = rectangle.SetSize(Vector2.Create(2.0f, 4.0f))
                              .SetLocation(Vector2.Create(0.0f, 0.0f));

        Assert.That(() => result.Location, Is.EqualTo(Vector2.Create(0.0f, 0.0f)));
        Assert.That(() => result.Size, Is.EqualTo(Vector2.Create(2.0f, 4.0f)));
    }

    /// <summary>Provides validation of the <see cref="Matrix4x4" /> setters.</summary>
    [Test]
    public static void Matrix4x4SettersTest()
    {
        var matrix = Matrix4x4.Identity;

        var x = Vector4.Create(1.0f, 2.0f, 3.0f, 4.0f);
        var y = Vector4.Create(5.0f, 6.0f, 7.0f, 8.0f);
        var z = Vector4.Create(9.0f, 10.0f, 11.0f, 12.0f);
        var w = Vector4.Create(13.0f, 14.0f, 15.0f, 16.0f);

        var result = matrix.SetX(x)
                                   .SetY(y)
                                   .SetZ(z)
                                   .SetW(w);

        Assert.That(() => result.X, Is.EqualTo(x));
        Assert.That(() => result.Y, Is.EqualTo(y));
        Assert.That(() => result.Z, Is.EqualTo(z));
        Assert.That(() => result.W, Is.EqualTo(w));
    }
}
