// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;

namespace TerraFX.Numerics;

/// <summary>Defines a bounding sphere.</summary>
public struct BoundingSphere
    : IEquatable<BoundingSphere>,
      IFormattable,
      ISpanFormattable,
      IUtf8SpanFormattable
{
    /// <summary>Gets a bounding sphere with zero radius.</summary>
    public static BoundingSphere Zero => CreateFromRadius(Vector3.Zero, 0.0f);

    private Vector3 _center;
    private float _radius;

    /// <summary>Creates a bounding sphere from a center and radius.</summary>
    /// <param name="center">The center of the bounding sphere.</param>
    /// <param name="radius">The radius of the bounding sphere.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingSphere CreateFromRadius(Vector3 center, float radius)
    {
        BoundingSphere result;

        result._center = center;
        result._radius = radius;

        return result;
    }

    /// <summary>Gets or sets the center of the bounding sphere.</summary>
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

    /// <summary>Gets or sets the radius of the bounding sphere.</summary>
    public float Radius
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return _radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _radius = value;
        }
    }

    /// <summary>Compares two bounding spheres to determine equality.</summary>
    /// <param name="left">The bounding sphere to compare with <paramref name="right" />.</param>
    /// <param name="right">The bounding sphere to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BoundingSphere left, BoundingSphere right)
    {
        return (left._center == right._center)
            && (left._radius == right._radius);
    }

    /// <summary>Compares two bounding boxes instances to determine inequality.</summary>
    /// <param name="left">The bounding box to compare with <paramref name="right" />.</param>
    /// <param name="right">The bounding box to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BoundingSphere left, BoundingSphere right)
    {
        return (left._center != right._center)
            || (left._radius != right._radius);
    }

    /// <inheritdoc />
    public override readonly bool Equals(object? obj) => (obj is BoundingSphere other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(BoundingSphere other)
    {
        return _center.Equals(other._center)
            && _radius.Equals(other._radius);
    }

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(_center, _radius);

    /// <inheritdoc />
    public override readonly string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public readonly string ToString(string? format = null, IFormatProvider? formatProvider = null)
        => $"BoundingSphere {{ Center = {Center.ToString(format, formatProvider)}, Radius = {Radius.ToString(format, formatProvider)} }}";

    /// <inheritdoc />
    public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"BoundingSphere { Center = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        var partLength = "BoundingSphere { Center = ".Length;

        numWritten += partLength;
        destination = destination.Slice(numWritten);

        if (!Center.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!", Radius = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = ", Radius = ".Length;

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!Radius.TryFormat(destination, out partLength, format, provider))
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

        if (!"BoundingSphere { Center = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        var partLength = "BoundingSphere { Center = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(numWritten);

        if (!Center.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!", Radius = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = ", Radius = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!Radius.TryFormat(utf8Destination, out partLength, format, provider))
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
