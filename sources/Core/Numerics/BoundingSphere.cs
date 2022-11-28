// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;

namespace TerraFX.Numerics;

/// <summary>Defines a bounding sphere.</summary>
public struct BoundingSphere : IEquatable<BoundingSphere>, IFormattable
{
    /// <summary>Gets a bounding sphere with zero radius.</summary>
    public static readonly BoundingSphere Zero = CreateFromRadius(Vector3.Zero, 0.0f);

    private Vector3 _center;
    private float _radius;

    /// <summary>Creates a bounding sphere from a center and radius.</summary>
    /// <param name="center">The center of the bounding sphere.</param>
    /// <param name="radius">The radius of the bounding sphere.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingSphere CreateFromRadius(Vector3 center, float radius)
    {
        Unsafe.SkipInit(out BoundingSphere result);

        result._center = center;
        result._radius = radius;

        return result;
    }

    /// <summary>Gets or sets the center of the bounding sphere.</summary>
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

    /// <summary>Gets or sets the radius of the bounding sphere.</summary>
    public float Radius
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
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
    public override bool Equals(object? obj) => (obj is BoundingSphere other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(BoundingSphere other)
    {
        return _center.Equals(other._center)
            && _radius.Equals(other._radius);
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(_center, _radius);

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"{nameof(BoundingSphere)} {{ {nameof(Center)} = {_center.ToString(format, formatProvider)}, {nameof(Radius)} = {_radius.ToString(format, formatProvider)} }}";
}
