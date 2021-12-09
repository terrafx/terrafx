// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace TerraFX.Numerics;

/// <summary>Defines an affine transformation.</summary>
public struct AffineTransform : IEquatable<AffineTransform>, IFormattable
{
    /// <summary>Defines a transform where all components are zero.</summary>
    public static readonly AffineTransform Zero = new AffineTransform(Quaternion.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero);

    /// <summary>Defines the identity transform.</summary>
    public static readonly AffineTransform Identity = new AffineTransform(Quaternion.Identity, Vector3.Zero, Vector3.One, Vector3.Zero);

    private Quaternion _rotation;
    private Vector3 _rotationOrigin;
    private Vector3 _scale;
    private Vector3 _translation;

    /// <summary>Initializes a new instance of the <see cref="AffineTransform" /> struct.</summary>
    /// <param name="rotation">The rotation of the transform.</param>
    /// <param name="rotationOrigin">The center of rotation for the transform.</param>
    /// <param name="scale">The scale of the transform.</param>
    /// <param name="translation">The translation of the transform.</param>
    public AffineTransform(Quaternion rotation, Vector3 rotationOrigin, Vector3 scale, Vector3 translation)
    {
        _rotation = rotation;
        _rotationOrigin = rotationOrigin;
        _scale = scale;
        _translation = translation;
    }

    /// <summary>Gets the rotation of the transform.</summary>
    public Quaternion Rotation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _rotation;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _rotation = value;
        }
    }

    /// <summary>Gets the rotation of the transform.</summary>
    public Vector3 RotationOrigin
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _rotationOrigin;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _rotationOrigin = value;
        }
    }

    /// <summary>Gets the scale of the transform.</summary>
    public Vector3 Scale
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _scale;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _scale = value;
        }
    }

    /// <summary>Gets the translation of the transform.</summary>
    public Vector3 Translation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _translation;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _translation = value;
        }
    }

    /// <summary>Compares two transforms to determine equality.</summary>
    /// <param name="left">The transform to compare with <paramref name="right" />.</param>
    /// <param name="right">The transform to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(AffineTransform left, AffineTransform right)
        => (left.Rotation == right.Rotation)
        && (left.RotationOrigin == right.RotationOrigin)
        && (left.Scale == right.Scale)
        && (left.Translation == right.Translation);

    /// <summary>Compares two transforms to determine inequality.</summary>
    /// <param name="left">The transform to compare with <paramref name="right" />.</param>
    /// <param name="right">The transform to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(AffineTransform left, AffineTransform right)
        => (left.Rotation != right.Rotation)
        || (left.RotationOrigin != right.RotationOrigin)
        || (left.Scale != right.Scale)
        || (left.Translation != right.Translation);

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is AffineTransform other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(AffineTransform other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Scale, Rotation, Translation);

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return new StringBuilder(7 + (separator.Length * 2))
            .Append('<')
            .Append("Rotation: ")
            .Append(Rotation.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append("Rotation Origin: ")
            .Append(RotationOrigin.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append("Scale: ")
            .Append(Scale.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append("Translation: ")
            .Append(Translation.ToString(format, formatProvider))
            .Append('>')
            .ToString();
    }
}
