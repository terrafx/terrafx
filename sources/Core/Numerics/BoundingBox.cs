// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;

namespace TerraFX.Numerics;

/// <summary>Defines a bounding box.</summary>
public struct BoundingBox
    : IEquatable<BoundingBox>,
      IFormattable,
      ISpanFormattable,
      IUtf8SpanFormattable
{
    /// <summary>Gets a bounding box with zero extent.</summary>
    public static BoundingBox Zero => CreateFromExtent(Vector3.Zero, Vector3.Zero);

    private Vector3 _center;
    private Vector3 _extent;

    /// <summary>Creates a bounding box from a center and extent.</summary>
    /// <param name="center">The center of the bounding box.</param>
    /// <param name="extent">The distance from the center to each side of the bounding box.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox CreateFromExtent(Vector3 center, Vector3 extent)
    {
        BoundingBox result;

        result._center = center;
        result._extent = extent;

        return result;
    }

    /// <summary>Creates a bounding box from a location and size.</summary>
    /// <param name="location">The location of the front-upper-left of the bounding box.</param>
    /// <param name="size">The size of the bounding box.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox CreateFromSize(Vector3 location, Vector3 size)
    {
        BoundingBox result;
        var extent = size * 0.5f;

        result._center = location + extent;
        result._extent = extent;

        return result;
    }

    /// <summary>Gets or sets the center of the bounding box.</summary>
    public Vector3 Center
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return _center;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _center = value;
        }
    }

    /// <summary>Gets or sets the depth of the bounding box.</summary>
    public float Depth
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return _extent.Z * 2.0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _extent = _extent.WithZ(value * 0.5f);
        }
    }

    /// <summary>Gets or sets the distance from the center to each side of the bounding box.</summary>
    public Vector3 Extent
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return _extent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _extent = value;
        }
    }

    /// <summary>Gets or sets the height of the bounding box.</summary>
    public float Height
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return _extent.Y * 2.0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _extent = _extent.WithY(value * 0.5f);
        }
    }

    /// <summary>Gets or sets the location of the front-upper-left of the bounding box.</summary>
    public Vector3 Location
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return _center - _extent;
        }

        set
        {
            _center = value + _extent;
        }
    }

    /// <summary>Gets or sets the size of the bounding box.</summary>
    public Vector3 Size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return _extent * 2.0f;
        }

        set
        {
            _extent = value * 0.5f;
        }
    }

    /// <summary>Gets or sets the width of the bounding box.</summary>
    public float Width
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return _extent.X * 2.0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _extent = _extent.WithX(value * 0.5f);
        }
    }

    /// <summary>Gets or sets the x-coordinate for the location of the front-upper-left of the bounding box.</summary>
    public float X
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return _center.X - _extent.X;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _center = _center.WithX(value + _extent.X);
        }
    }

    /// <summary>Gets or sets the y-coordinate for the location of the front-upper-left of the bounding box.</summary>
    public float Y
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return _center.Y - _extent.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _center = _center.WithY(value + _extent.Y);
        }
    }

    /// <summary>Gets or sets the z-coordinate for the location of the front-upper-left of the bounding box.</summary>
    public float Z
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return _center.Z - _extent.Z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _center = _center.WithZ(value + _extent.Z);
        }
    }

    /// <summary>Compares two bounding boxes to determine equality.</summary>
    /// <param name="left">The bounding box to compare with <paramref name="right" />.</param>
    /// <param name="right">The bounding box to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BoundingBox left, BoundingBox right)
    { 
        return (left._center == right._center)
            && (left._extent == right._extent);
    }

    /// <summary>Compares two bounding boxes instances to determine inequality.</summary>
    /// <param name="left">The bounding box to compare with <paramref name="right" />.</param>
    /// <param name="right">The bounding box to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BoundingBox left, BoundingBox right)
    { 
        return (left._center != right._center)
            || (left._extent != right._extent);
    }

    /// <inheritdoc />
    public override readonly bool Equals(object? obj) => (obj is BoundingBox other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(BoundingBox other)
    {
        return _center.Equals(other._center)
            && _extent.Equals(other._extent);
    }

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(_center, _extent);

    /// <inheritdoc />
    public override readonly string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public readonly string ToString(string? format = null, IFormatProvider? formatProvider = null)
        => $"BoundingBox {{ Center = {Center.ToString(format, formatProvider)}, Extent = {Extent.ToString(format, formatProvider)} }}";

    /// <inheritdoc />
    public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"BoundingBox { Center = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        var partLength = "BoundingBox { Center = ".Length;

        numWritten += partLength;
        destination = destination.Slice(numWritten);

        if (!Center.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!", Extent = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = ", Extent = ".Length;

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!Extent.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!" }".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = " }".Length;

        charsWritten = numWritten + partLength;
        return true;
    }

    /// <inheritdoc />
    public readonly bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"BoundingBox { Center = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        var partLength = "BoundingBox { Center = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(numWritten);

        if (!Center.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!", Extent = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = ", Extent = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!Extent.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!" }"u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = " }"u8.Length;

        bytesWritten = numWritten + partLength;
        return true;
    }
}
