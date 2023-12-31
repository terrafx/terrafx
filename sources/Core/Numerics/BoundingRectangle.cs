// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;

namespace TerraFX.Numerics;

/// <summary>Defines a bounding rectangle.</summary>
public struct BoundingRectangle
    : IEquatable<BoundingRectangle>,
      IFormattable,
      ISpanFormattable,
      IUtf8SpanFormattable
{
    /// <summary>Gets a bounding rectangle with zero extent.</summary>
    public static BoundingRectangle Zero => CreateFromExtent(Vector2.Zero, Vector2.Zero);

    private Vector2 _center;
    private Vector2 _extent;

    /// <summary>Creates a bounding rectangle from a center and extent.</summary>
    /// <param name="center">The center of the bounding rectangle.</param>
    /// <param name="extent">The distance from the center to each side of the bounding rectangle.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingRectangle CreateFromExtent(Vector2 center, Vector2 extent)
    {
        BoundingRectangle result;

        result._center = center;
        result._extent = extent;

        return result;
    }

    /// <summary>Creates a bounding rectangle from a location and size.</summary>
    /// <param name="location">The location of the upper-left of the bounding rectangle.</param>
    /// <param name="size">The size of the bounding rectangle.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingRectangle CreateFromSize(Vector2 location, Vector2 size)
    {
        BoundingRectangle result;
        var extent = size * 0.5f;

        result._center = location + extent;
        result._extent = extent;

        return result;
    }

    /// <summary>Gets or sets the center of the bounding rectangle.</summary>
    public Vector2 Center
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _center;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _center = value;
        }
    }

    /// <summary>Gets or sets the distance from the center to each side of the bounding rectangle.</summary>
    public Vector2 Extent
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _extent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _extent = value;
        }
    }

    /// <summary>Gets or sets the height of the bounding rectangle.</summary>
    public float Height
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _extent.Y * 2.0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _extent = _extent.WithY(value * 0.5f);
        }
    }

    /// <summary>Gets or sets the location of the upper-left of the bounding rectangle.</summary>
    public Vector2 Location
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _center - _extent;
        }

        set
        {
            _center = value + _extent;
        }
    }

    /// <summary>Gets or sets the size of the bounding rectangle.</summary>
    public Vector2 Size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _extent * 2.0f;
        }

        set
        {
            _extent = value * 0.5f;
        }
    }

    /// <summary>Gets or sets the width of the bounding rectangle.</summary>
    public float Width
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _extent.X * 2.0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _extent = _extent.WithX(value * 0.5f);
        }
    }

    /// <summary>Gets or sets the x-coordinate for the location of the upper-left of the bounding rectangle.</summary>
    public float X
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _center.X - _extent.X;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _center = _center.WithX(value + _extent.X);
        }
    }

    /// <summary>Gets or sets the y-coordinate for the location of the upper-left of the bounding rectangle.</summary>
    public float Y
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _center.Y - _extent.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _center = _center.WithY(value + _extent.Y);
        }
    }

    /// <summary>Compares two bounding rectangles to determine equality.</summary>
    /// <param name="left">The bounding rectangle to compare with <paramref name="right" />.</param>
    /// <param name="right">The bounding rectangle to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BoundingRectangle left, BoundingRectangle right)
    {
        return (left._center == right._center)
            && (left._extent == right._extent);
    }

    /// <summary>Compares two bounding rectangles instances to determine inequality.</summary>
    /// <param name="left">The bounding rectangle to compare with <paramref name="right" />.</param>
    /// <param name="right">The bounding rectangle to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BoundingRectangle left, BoundingRectangle right)
    {
        return (left._center != right._center)
            || (left._extent != right._extent);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is BoundingRectangle other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(BoundingRectangle other)
    {
        return _center.Equals(other._center)
            && _extent.Equals(other._extent);
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(_center, _extent);

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format = null, IFormatProvider? formatProvider = null)
        => $"BoundingRectangle {{ Center = {Center.ToString(format, formatProvider)}, Extent = {Extent.ToString(format, formatProvider)} }}";

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"BoundingRectangle { Center = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        var partLength = "BoundingRectangle { Center = ".Length;

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
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"BoundingRectangle { Center = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        var partLength = "BoundingRectangle { Center = "u8.Length;

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
