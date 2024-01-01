// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.CompilerServices;

namespace TerraFX.Numerics;

/// <summary>Defines extension methods for types in the numerics namespace.</summary>
public static class NumericsExtensions
{
    /// <summary>Sets the rotation of the affine transform.</summary>
    /// <param name="self">The affine transform.</param>
    /// <param name="rotation">The new rotation for the affine transform.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref AffineTransform SetRotation(this ref AffineTransform self, Quaternion rotation)
    {
        self.Rotation = rotation;
        return ref self;
    }

    /// <summary>Sets the scale of the affine transform.</summary>
    /// <param name="self">The affine transform.</param>
    /// <param name="scale">The new scale for the affine transform.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref AffineTransform SetScale(this ref AffineTransform self, Vector3 scale)
    {
        self.Scale = scale;
        return ref self;
    }

    /// <summary>Sets the translation of the affine transform.</summary>
    /// <param name="self">The affine transform.</param>
    /// <param name="translation">The new translation for the affine transform.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref AffineTransform SetRotation(this ref AffineTransform self, Vector3 translation)
    {
        self.Translation = translation;
        return ref self;
    }

    /// <summary>Sets the center of the bounding box.</summary>
    /// <param name="self">The bounding box.</param>
    /// <param name="center">The new center for the bounding box.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingBox SetCenter(this ref BoundingBox self, Vector3 center)
    {
        self.Center = center;
        return ref self;
    }

    /// <summary>Sets the depth for the bounding box.</summary>
    /// <param name="self">The bounding box.</param>
    /// <param name="depth">The new depth for the bounding box.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingBox SetDepth(this ref BoundingBox self, float depth)
    {
        self.Depth = depth;
        return ref self;
    }

    /// <summary>Sets the extent of the bounding box.</summary>
    /// <param name="self">The bounding box.</param>
    /// <param name="extent">The new distance from the center to each side of the bounding box.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingBox SetExtent(this ref BoundingBox self, Vector3 extent)
    {
        self.Extent = extent;
        return ref self;
    }

    /// <summary>Sets the height for the bounding box.</summary>
    /// <param name="self">The bounding box.</param>
    /// <param name="height">The new height for the bounding box.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingBox SetHeight(this ref BoundingBox self, float height)
    {
        self.Height = height;
        return ref self;
    }

    /// <summary>Sets the location of the bounding box.</summary>
    /// <param name="self">The bounding box.</param>
    /// <param name="location">The new location of the front-upper-left for the bounding box.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingBox SetLocation(this ref BoundingBox self, Vector3 location)
    {
        self.Location = location;
        return ref self;
    }

    /// <summary>Sets the size of the bounding box.</summary>
    /// <param name="self">The bounding box.</param>
    /// <param name="size">The new size for the bounding box.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingBox SetSize(this ref BoundingBox self, Vector3 size)
    {
        self.Size = size;
        return ref self;
    }

    /// <summary>Sets the width for the bounding box.</summary>
    /// <param name="self">The bounding box.</param>
    /// <param name="width">The new width for the bounding box.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingBox SetWidth(this ref BoundingBox self, float width)
    {
        self.Width = width;
        return ref self;
    }

    /// <summary>Sets the x-coordinate of the front-upper-left for the bounding box.</summary>
    /// <param name="self">The bounding box.</param>
    /// <param name="x">The new x-coordinate of the front-upper-left for the bounding box.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingBox SetX(this ref BoundingBox self, float x)
    {
        self.X = x;
        return ref self;
    }

    /// <summary>Sets the y-coordinate of the front-upper-left for the bounding box.</summary>
    /// <param name="self">The bounding box.</param>
    /// <param name="y">The new y-coordinate of the front-upper-left for the bounding box.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingBox SetY(this ref BoundingBox self, float y)
    {
        self.Y = y;
        return ref self;
    }

    /// <summary>Sets the z-coordinate of the front-upper-left for the bounding box.</summary>
    /// <param name="self">The bounding box.</param>
    /// <param name="z">The new z-coordinate of the front-upper-left for the bounding box.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingBox SetZ(this ref BoundingBox self, float z)
    {
        self.Z = z;
        return ref self;
    }

    /// <summary>Sets the center of the bounding rectangle.</summary>
    /// <param name="self">The bounding rectangle.</param>
    /// <param name="center">The new center for the bounding rectangle.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingRectangle SetCenter(this ref BoundingRectangle self, Vector2 center)
    {
        self.Center = center;
        return ref self;
    }

    /// <summary>Sets the extent of the bounding rectangle.</summary>
    /// <param name="self">The bounding rectangle.</param>
    /// <param name="extent">The new distance from the center to each side of the bounding rectangle.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingRectangle SetExtent(this ref BoundingRectangle self, Vector2 extent)
    {
        self.Extent = extent;
        return ref self;
    }

    /// <summary>Sets the height for the bounding rectangle.</summary>
    /// <param name="self">The bounding rectangle.</param>
    /// <param name="height">The new height for the bounding rectangle.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingRectangle SetHeight(this ref BoundingRectangle self, float height)
    {
        self.Height = height;
        return ref self;
    }

    /// <summary>Sets the location of the bounding rectangle.</summary>
    /// <param name="self">The bounding rectangle.</param>
    /// <param name="location">The new location of the front-upper-left for the bounding rectangle.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingRectangle SetLocation(this ref BoundingRectangle self, Vector2 location)
    {
        self.Location = location;
        return ref self;
    }

    /// <summary>Sets the size of the bounding rectangle.</summary>
    /// <param name="self">The bounding rectangle.</param>
    /// <param name="size">The new size for the bounding rectangle.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingRectangle SetSize(this ref BoundingRectangle self, Vector2 size)
    {
        self.Size = size;
        return ref self;
    }

    /// <summary>Sets the width for the bounding rectangle.</summary>
    /// <param name="self">The bounding rectangle.</param>
    /// <param name="width">The new width for the bounding rectangle.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingRectangle SetWidth(this ref BoundingRectangle self, float width)
    {
        self.Width = width;
        return ref self;
    }

    /// <summary>Sets the x-coordinate of the upper-left for the bounding rectangle.</summary>
    /// <param name="self">The bounding rectangle.</param>
    /// <param name="x">The new x-coordinate of the upper-left for the bounding rectangle.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingRectangle SetX(this ref BoundingRectangle self, float x)
    {
        self.X = x;
        return ref self;
    }

    /// <summary>Sets the y-coordinate of the upper-left for the bounding rectangle.</summary>
    /// <param name="self">The bounding rectangle.</param>
    /// <param name="y">The new y-coordinate of the upper-left for the bounding rectangle.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref BoundingRectangle SetY(this ref BoundingRectangle self, float y)
    {
        self.Y = y;
        return ref self;
    }

    /// <summary>Sets the x-component of the matrix.</summary>
    /// <param name="self">The matrix.</param>
    /// <param name="x">The new x-component for the matrix.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Matrix4x4 SetX(this ref Matrix4x4 self, Vector4 x)
    {
        self.X = x;
        return ref self;
    }

    /// <summary>Sets the y-component of the matrix.</summary>
    /// <param name="self">The matrix.</param>
    /// <param name="y">The new y-component for the matrix.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Matrix4x4 SetY(this ref Matrix4x4 self, Vector4 y)
    {
        self.Y = y;
        return ref self;
    }

    /// <summary>Sets the z-component of the matrix.</summary>
    /// <param name="self">The matrix.</param>
    /// <param name="z">The new z-component for the matrix.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Matrix4x4 SetZ(this ref Matrix4x4 self, Vector4 z)
    {
        self.Z = z;
        return ref self;
    }

    /// <summary>Sets the w-component of the matrix.</summary>
    /// <param name="self">The matrix.</param>
    /// <param name="w">The new w-component for the matrix.</param>
    /// <returns><paramref name="self" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Matrix4x4 SetW(this ref Matrix4x4 self, Vector4 w)
    {
        self.W = w;
        return ref self;
    }
}
