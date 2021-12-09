// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace TerraFX.Numerics;

/// <summary>Defines a bounding box.</summary>
public struct BoundingBox : IEquatable<BoundingBox>, IFormattable
{
    private const uint CornerCount = 8;

    private Vector3 _center;
    private Vector3 _extents;

    /// <summary>Initializes a new instance of the <see cref="BoundingBox" /> struct.</summary>
    public BoundingBox()
    {
        _center = Vector3.Zero;
        _extents = Vector3.One;
    }

    /// <summary>Initializes a new instance of the <see cref="BoundingBox" /> struct.</summary>
    /// <param name="center">The center of the bounding box.</param>
    /// <param name="extents">The distance from the center to each side of the bounding box.</param>
    public BoundingBox(Vector3 center, Vector3 extents)
    {
        _center = center;
        _extents = extents;
    }

    /// <summary>Gets the center of the bounding box.</summary>
    public Vector3 Center
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

    /// <summary>Gets the distance from the center to each side of the bounding box.</summary>
    public Vector3 Extents
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _extents;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _extents = value;
        }
    }

    /// <summary>Compares two bounding boxes to determine equality.</summary>
    /// <param name="left">The bounding box to compare with <paramref name="right" />.</param>
    /// <param name="right">The bounding box to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BoundingBox left, BoundingBox right)
        => (left._center == right._center)
        && (left._extents == right._extents);

    /// <summary>Compares two bounding boxes instances to determine inequality.</summary>
    /// <param name="left">The bounding box to compare with <paramref name="right" />.</param>
    /// <param name="right">The bounding box to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BoundingBox left, BoundingBox right)
        => (left._center != right._center)
        || (left._extents != right._extents);

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is BoundingBox other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(BoundingBox other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(_center, _extents);

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return new StringBuilder(5 + separator.Length)
            .Append('<')
            .Append("Center: ")
            .Append(Center.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append("Extents: ")
            .Append(Extents.ToString(format, formatProvider))
            .Append('>')
            .ToString();
    }
}
