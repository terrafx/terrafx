// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static TerraFX.Utilities.VectorUtilities;

namespace TerraFX.Numerics;

/// <summary>Defines an affine transformation.</summary>
public struct AffineTransform
    : IEquatable<AffineTransform>,
      IFormattable,
      ISpanFormattable,
      IUtf8SpanFormattable
{
    /// <summary>Gets a transform where all components are zero.</summary>
    public static AffineTransform Zero => Create(Quaternion.Zero, Vector3.Zero, Vector3.Zero);

    /// <summary>Gets the identity transform.</summary>
    public static AffineTransform Identity => Create(Quaternion.Identity, Vector3.One, Vector3.Zero);

    private Quaternion _rotation;
    private Vector3 _scale;
    private Vector3 _translation;

    /// <summary>Creates a new affine transform from a rotation, scale, and translation.</summary>
    /// <param name="rotation">The rotation of the affine transform.</param>
    /// <param name="scale">The scale of the affine transform.</param>
    /// <param name="translation">The translation of the affine transform.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AffineTransform Create(Quaternion rotation, Vector3 scale, Vector3 translation)
    {
        AffineTransform result;

        result._rotation = rotation;
        result._scale = scale;
        result._translation = translation;

        return result;
    }

    /// <summary>Tries to create an affine transform from a matrix.</summary>
    /// <param name="matrix">The matrix from which the affine transform should be created.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between the <c>(scaleX, scaleY, scaleZ, determinant)</c> and zero for which they should be considered equivalent.</param>
    /// <param name="result">The resulting affine transform or <see cref="Zero" /> if creation from <paramref name="matrix" /> failed.</param>
    /// <returns><c>true</c> if an affine transform was successfully created from <paramref name="matrix" />; otherwise, <c>false</c>.</returns>
    public static unsafe bool TryCreateFromMatrix(Matrix4x4 matrix, Vector4 epsilon, out AffineTransform result)
    {
        var translation = Vector3.Create(matrix.W.Value);

        var pMatrix = (Vector128<float>*)&matrix;
        var zeroW = Vector128.Create(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0x00000000).AsSingle();

        pMatrix[0] = BitwiseAnd(pMatrix[0], zeroW);
        pMatrix[1] = BitwiseAnd(pMatrix[1], zeroW);
        pMatrix[2] = BitwiseAnd(pMatrix[2], zeroW);
        pMatrix[3] = UnitW;

        var canonicalBasis = stackalloc Vector128<float>[3] {
            UnitX,
            UnitY,
            UnitZ,
        };

        var scale = stackalloc float[3] {
            Length(pMatrix[0]).ToScalar(),
            Length(pMatrix[1]).ToScalar(),
            Length(pMatrix[2]).ToScalar(),
        };

        var (a, b, c) = RankDecompose(scale[0], scale[1], scale[2]);

        if (scale[a] <= epsilon.X)
        {
            pMatrix[a] = canonicalBasis[a];
        }
        pMatrix[a] = Normalize(pMatrix[a]);

        if (scale[b] <= epsilon.Y)
        {
            var abs = Abs(pMatrix[a]);
            var (aa, bb, cc) = RankDecompose(abs.GetX(), abs.GetY(), abs.GetZ());
            pMatrix[b] = CrossProduct(pMatrix[a], canonicalBasis[cc]);
        }
        pMatrix[b] = Normalize(pMatrix[b]);

        if (scale[c] <= epsilon.Z)
        {
            pMatrix[c] = CrossProduct(pMatrix[a], pMatrix[b]);
        }
        pMatrix[c] = Normalize(pMatrix[c]);

        var determinant = matrix.Determinant;

        // use Kramer's rule to check for handedness of coordinate system
        if (determinant < 0.0f)
        {
            // switch coordinate system by negating the scale and inverting the basis vector on the x-axis
            scale[a] = -scale[a];
            pMatrix[a] = Negate(pMatrix[a]);

            determinant = -determinant;
        }

        determinant -= 1.0f;
        determinant *= determinant;

        if (epsilon.W < determinant)
        {
            // Non-SRT matrix encountered
            result = Zero;
            return false;
        }

        // generate the quaternion from the matrix

        result._rotation = Quaternion.CreateFromMatrix(matrix);
        result._scale = ((Vector3*)scale)[0];
        result._translation = translation;

        return true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (nuint a, nuint b, nuint c) RankDecompose(float x, float y, float z)
        {
            nuint a, b, c;

            if (x < y)
            {
                if (y < z)
                {
                    a = 2;
                    b = 1;
                    c = 0;
                }
                else
                {
                    a = 1;

                    if (x < z)
                    {
                        b = 2;
                        c = 0;
                    }
                    else
                    {
                        b = 0;
                        c = 2;
                    }
                }
            }
            else
            {
                if (x < z)
                {
                    a = 2;
                    b = 0;
                    c = 1;
                }
                else
                {
                    a = 0;

                    if (y < z)
                    {
                        b = 2;
                        c = 1;
                    }
                    else
                    {
                        b = 1;
                        c = 2;
                    }
                }
            }

            return (a, b, c);
        }
    }

    /// <summary>Gets or sets the rotation of the affine transform.</summary>
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

    /// <summary>Compares two affine transforms to determine equality.</summary>
    /// <param name="left">The affine transform to compare with <paramref name="right" />.</param>
    /// <param name="right">The affine transform to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(AffineTransform left, AffineTransform right)
    { 
        return (left._rotation == right._rotation)
            && (left._scale == right._scale)
            && (left._translation == right._translation);
    }

    /// <summary>Compares two affine transforms to determine inequality.</summary>
    /// <param name="left">The affine transform to compare with <paramref name="right" />.</param>
    /// <param name="right">The affine transform to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(AffineTransform left, AffineTransform right)
    { 
        return (left._rotation != right._rotation)
            || (left._scale != right._scale)
            || (left._translation != right._translation);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is AffineTransform other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(AffineTransform other)
    {
        return _rotation.Equals(other._rotation)
            && _scale.Equals(other._scale)
            && _translation.Equals(other._translation);
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(_rotation, _scale, _translation);

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format = null, IFormatProvider? formatProvider = null)
        => $"AffineTransform {{ Rotation = {Rotation.ToString(format, formatProvider)}, Scale = {Scale.ToString(format, formatProvider)}, Translation = {Translation.ToString(format, formatProvider)} }}";

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"AffineTransform { Rotation = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        var partLength = "AffineTransform { Rotation = ".Length;

        numWritten += partLength;
        destination = destination.Slice(numWritten);

        if (!Rotation.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!", Scale = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = ", Scale = ".Length;

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!Scale.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!", Translation = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = ", Translation = ".Length;

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!Translation.TryFormat(destination, out partLength, format, provider))
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

        if (!"AffineTransform { Rotation = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        var partLength = "AffineTransform { Rotation = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(numWritten);

        if (!Rotation.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!", Scale = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = ", Scale = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!Scale.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!", Translation = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = ", Translation = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!Translation.TryFormat(utf8Destination, out partLength, format, provider))
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
