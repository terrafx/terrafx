// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;
using static TerraFX.Utilities.VectorUtilities;

namespace TerraFX.Numerics;

/// <summary>Defines an affine transformation.</summary>
public struct AffineTransform : IEquatable<AffineTransform>, IFormattable
{
    /// <summary>Defines a transform where all components are zero.</summary>
    public static readonly AffineTransform Zero = new AffineTransform(Quaternion.Zero, Vector3.Zero, Vector3.Zero);

    /// <summary>Defines the identity transform.</summary>
    public static readonly AffineTransform Identity = new AffineTransform(Quaternion.Identity, Vector3.One, Vector3.Zero);

    private Quaternion _rotation;
    private Vector3 _scale;
    private Vector3 _translation;

    /// <summary>Initializes a new instance of the <see cref="AffineTransform" /> struct.</summary>
    /// <param name="rotation">The rotation of the transform.</param>
    /// <param name="scale">The scale of the transform.</param>
    /// <param name="translation">The translation of the transform.</param>
    public AffineTransform(Quaternion rotation, Vector3 scale, Vector3 translation)
    {
        _rotation = rotation;
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
        && (left.Scale == right.Scale)
        && (left.Translation == right.Translation);

    /// <summary>Compares two transforms to determine inequality.</summary>
    /// <param name="left">The transform to compare with <paramref name="right" />.</param>
    /// <param name="right">The transform to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(AffineTransform left, AffineTransform right)
        => (left.Rotation != right.Rotation)
        || (left.Scale != right.Scale)
        || (left.Translation != right.Translation);

    /// <summary>Tries to create an affine transform from a matrix.</summary>
    /// <param name="matrix">The matrix from which the affine transform should be created.</param>
    /// <param name="result">The resulting affine transform or <see cref="Zero" /> if decomposing <paramref name="matrix" /> failed.</param>
    /// <returns><c>true</c> if <paramref name="matrix" /> was succesfully decomposed; otherwise, <c>false</c>.</returns>
    public static unsafe bool TryCreateFromMatrix(Matrix4x4 matrix, out AffineTransform result)
    {
        var translation = new Vector3(matrix.W.AsVector128());

        var pMatrix = (Vector128<float>*)&matrix;
        var zeroW = Vector128.Create(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0x00000000).AsSingle();

        pMatrix[0] = BitwiseAnd(pMatrix[0], zeroW);
        pMatrix[1] = BitwiseAnd(pMatrix[1], zeroW);
        pMatrix[2] = BitwiseAnd(pMatrix[2], zeroW);
        pMatrix[3] = Vector4.UnitW.AsVector128();

        var canonicalBasis = stackalloc Vector128<float>[3] {
            Vector4.UnitX.AsVector128(),
            Vector4.UnitY.AsVector128(),
            Vector4.UnitZ.AsVector128(),
        };

        var scale = stackalloc float[3] {
             Length(pMatrix[0]).ToScalar(),
             Length(pMatrix[1]).ToScalar(),
             Length(pMatrix[2]).ToScalar(),
        };

        var (a, b, c) = RankDecompose(scale[0], scale[1], scale[2]);

        if (scale[a] < NearZeroEpsilon)
        {
            pMatrix[a] = canonicalBasis[a];
        }
        pMatrix[a] = Normalize(pMatrix[a]);

        if (scale[b] < NearZeroEpsilon)
        {
            var abs = Abs(pMatrix[a]);
            var (aa, bb, cc) = RankDecompose(abs.GetElement(0), abs.GetElement(1), abs.GetElement(2));
            pMatrix[b] = CrossProduct(pMatrix[a], canonicalBasis[cc]);
        }
        pMatrix[b] = Normalize(pMatrix[b]);

        if (scale[c] < NearZeroEpsilon)
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

        if (NearZeroEpsilon < determinant)
        {
            // Non-SRT matrix encountered
            result = Zero;
            return false;
        }

        // generate the quaternion from the matrix
        Unsafe.SkipInit(out result);

        result.Rotation = Quaternion.CreateFromMatrix(matrix);
        result.Scale = ((Vector3*)scale)[0];
        result.Translation = translation;

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
