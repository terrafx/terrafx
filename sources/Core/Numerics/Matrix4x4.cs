// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VectorUtilities;
using SysMatrix4x4 = System.Numerics.Matrix4x4;

namespace TerraFX.Numerics;

/// <summary>Defines a 4x4 row-major matrix.</summary>
public struct Matrix4x4 : IEquatable<Matrix4x4>, IFormattable
{
    /// <summary>Defines a matrix where all components are zero.</summary>
    public static readonly Matrix4x4 Zero = new Matrix4x4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero);

    /// <summary>Defines the identity matrix.</summary>
    public static readonly Matrix4x4 Identity = new Matrix4x4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

    private Vector4 _x;
    private Vector4 _y;
    private Vector4 _z;
    private Vector4 _w;

    /// <summary>Initializes a new instance of the <see cref="Matrix4x4" /> struct.</summary>
    /// <param name="x">The value of the x-dimension.</param>
    /// <param name="y">The value of the y-dimension.</param>
    /// <param name="z">The value of the z-dimension.</param>
    /// <param name="w">The value of the w-dimension.</param>
    public Matrix4x4(Vector4 x, Vector4 y, Vector4 z, Vector4 w)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
    }

    /// <summary>Initializes a new instance of the <see cref="Matrix4x4" /> struct.</summary>
    /// <param name="x">The value of the x-dimension.</param>
    /// <param name="y">The value of the y-dimension.</param>
    /// <param name="z">The value of the z-dimension.</param>
    /// <param name="w">The value of the w-dimension.</param>
    public Matrix4x4(Vector128<float> x, Vector128<float> y, Vector128<float> z, Vector128<float> w)
    {
        _x = new Vector4(x);
        _y = new Vector4(y);
        _z = new Vector4(z);
        _w = new Vector4(w);
    }

    /// <summary>Initializes a new instance of the <see cref="Matrix4x4" /> struct.</summary>
    /// <param name="value">The value of the matrix.</param>
    public Matrix4x4(SysMatrix4x4 value)
    {
        this = As<SysMatrix4x4, Matrix4x4>(ref value);
    }

    /// <summary>Gets the value of the x-dimension.</summary>
    public Vector4 X
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _x = value;
        }
    }

    /// <summary>Gets the value of the y-dimension.</summary>
    public Vector4 Y
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _y = value;
        }
    }

    /// <summary>Gets the value of the z-dimension.</summary>
    public Vector4 Z
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _z = value;
        }
    }

    /// <summary>Gets the value of the w-dimension.</summary>
    public Vector4 W
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _w = value;
        }
    }

    /// <summary>Compares two matrices to determine equality.</summary>
    /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
    /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Matrix4x4 left, in Matrix4x4 right)
        => (left._x == right._x)
        && (left._y == right._y)
        && (left._z == right._z)
        && (left._w == right._w);

    /// <summary>Compares two matrices to determine inequality.</summary>
    /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
    /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Matrix4x4 left, in Matrix4x4 right)
        => (left._x != right._x)
        || (left._y != right._y)
        || (left._z != right._z)
        || (left._w != right._w);

    /// <summary>Computes the product of two matrices.</summary>
    /// <param name="left">The matrix to multiply by <paramref name="right" />.</param>
    /// <param name="right">The matrix which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    public static Matrix4x4 operator *(Matrix4x4 left, in Matrix4x4 right)
    {
        return new Matrix4x4(
            ComputeRow(in right, left.X),
            ComputeRow(in right, left.Y),
            ComputeRow(in right, left.Z),
            ComputeRow(in right, left.W)
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Vector4 ComputeRow(in Matrix4x4 left, Vector4 right)
        {
            if (Sse.IsSupported || AdvSimd.IsSupported)
            {
                var vRight = right.AsVector128();

                var sum1 = MultiplyByX(left._x.AsVector128(), vRight);
                sum1 = MultiplyAddByZ(sum1, left._z.AsVector128(), vRight);

                var sum2 = MultiplyByY(left._y.AsVector128(), vRight);
                sum2 = MultiplyAddByW(sum2, left._w.AsVector128(), vRight);

                return new Vector4(Add(sum1, sum2));
            }
            else
            {
                return new Vector4(
                    (left.X.X * right.X) + (left.Y.X * right.Y) + (left.Z.X * right.Z) + (left.W.X * right.W),
                    (left.X.Y * right.X) + (left.Y.Y * right.Y) + (left.Z.Y * right.Z) + (left.W.Y * right.W),
                    (left.X.Z * right.X) + (left.Y.Z * right.Y) + (left.Z.Z * right.Z) + (left.W.Z * right.W),
                    (left.X.W * right.X) + (left.Y.W * right.Y) + (left.Z.W * right.Z) + (left.W.W * right.W)
                );
            }
        }
    }

    /// <summary>Creates a matrix from an affine transform.</summary>
    /// <param name="affineTransform">The affine transform of the matrix.</param>
    /// <returns>A matrix that represents <paramref name="affineTransform" />.</returns>
    public static Matrix4x4 CreateFromAffineTransform(AffineTransform affineTransform)
    {
        var scaleMatrix = CreateFromScale(affineTransform.Scale);
        var rotationOrigin = new Vector4(affineTransform.RotationOrigin, 0.0f);

        scaleMatrix.W -= rotationOrigin;

        var result = scaleMatrix * CreateFromRotation(affineTransform.Rotation);
        result.W += rotationOrigin;
        result.W += new Vector4(affineTransform.Translation, 0.0f);
        return result;
    }

    /// <summary>Creates a matrix from a rotation quaternion.</summary>
    /// <param name="rotation">A quaternion representing the rotation of the matrix.</param>
    /// <returns>A matrix that represents <paramref name="rotation" />.</returns>
    public static Matrix4x4 CreateFromRotation(Quaternion rotation)
    {
        if (Sse.IsSupported || AdvSimd.Arm64.IsSupported)
        {
            var quaternion = rotation.AsVector128();

            var q0 = Add(quaternion, quaternion);
            var q1 = Multiply(quaternion, q0);
            q1 = BitwiseAnd(q1, Vector128.Create(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0x00000000).AsSingle());

            var v0 = CreateFromYXXW(q1);
            var v1 = CreateFromZZYW(q1);
            var r0 = Subtract(Vector128.Create(1.0f, 1.0f, 1.0f, 0.0f), v0);
            r0 = Subtract(r0, v1);

            v0 = CreateFromXXYW(quaternion);
            v1 = CreateFromZYZW(q0);
            v0 = Multiply(v0, v1);
            
            v1 = CreateFromW(quaternion);
            var v2 = CreateFromYZXW(q0);
            v1 = Multiply(v1, v2);
            
            var r1 = Add(v0, v1);
            var r2 = Subtract(v0, v1);
            
            v0 = CreateFromYZXY(r1, r2);
            v0 = CreateFromXZWY(v0);
            v1 = CreateFromXXZZ(r1, r2);
            v1 = CreateFromXZXZ(v1);
            
            q1 = CreateFromXWXY(r0, v0);
            q1 = CreateFromXZWY(q1);

            Unsafe.SkipInit(out Matrix4x4 m);
            {
                m.X = new Vector4(q1);

                q1 = CreateFromYWZW(r0, v0);
                q1 = CreateFromZXWY(q1);
                m.Y = new Vector4(q1);

                q1 = CreateFromXYZW(v1, r0);
                m.Z = new Vector4(q1);

                m.W = Vector4.UnitW;
            }
            return m;
        }
        else
        {
            var xSq = rotation.X * rotation.X;
            var ySq = rotation.Y * rotation.Y;
            var zSq = rotation.Z * rotation.Z;
            var wSq = rotation.W * rotation.W;

            var x2 = rotation.X * 2;
            var y2 = rotation.Y * 2;
            var w2 = rotation.W * 2;

            var x2y = x2 * rotation.Y;
            var x2z = x2 * rotation.Z;

            var y2z = y2 * rotation.Z;

            var w2x = w2 * rotation.X;
            var w2y = w2 * rotation.Y;
            var w2z = w2 * rotation.Z;

            return new Matrix4x4(
                new Vector4(wSq + xSq - ySq - zSq, w2z + x2y, x2z - w2y, 0),
                new Vector4(x2y - w2z, wSq - xSq + ySq - zSq, w2x + y2z, 0),
                new Vector4(w2y + x2z, y2z - w2x, wSq - xSq - ySq + zSq, 0),
                Vector4.UnitW
            );
        }
    }

    /// <summary>Creates a matrix from a scaling vector.</summary>
    /// <param name="scale">A vector representing the scale of the matrix.</param>
    /// <returns>A matrix that represents <paramref name="scale" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateFromScale(Vector3 scale)
    {
        if (Sse.IsSupported || AdvSimd.IsSupported)
        {
            var vScale = scale.AsVector128();

            return new Matrix4x4(
                BitwiseAnd(vScale, Vector128.Create(0xFFFFFFFF, 0x00000000, 0x00000000, 0x00000000).AsSingle()),
                BitwiseAnd(vScale, Vector128.Create(0x00000000, 0xFFFFFFFF, 0x00000000, 0x00000000).AsSingle()),
                BitwiseAnd(vScale, Vector128.Create(0x00000000, 0x00000000, 0xFFFFFFFF, 0x00000000).AsSingle()),
                Vector4.UnitW.AsVector128()
            );
        }
        else
        {
            return SoftwareFallback(scale);
        }

        static Matrix4x4 SoftwareFallback(Vector3 scale)
        {
            return new Matrix4x4(
                new Vector4(scale.X, 0.0f, 0.0f, 0.0f),
                new Vector4(0.0f, scale.Y, 0.0f, 0.0f),
                new Vector4(0.0f, 0.0f, scale.Z, 0.0f),
                Vector4.UnitW
            );
        }
    }

    /// <summary>Creates a matrix from a translation vector.</summary>
    /// <param name="translation">A vector representing the translation of the matrix.</param>
    /// <returns>A matrix that represents <paramref name="translation" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateFromTranslation(Vector3 translation)
    {
        return new Matrix4x4(
            Vector4.UnitX,
            Vector4.UnitY,
            Vector4.UnitZ,
            new Vector4(translation, 1.0f)
        );
    }

    /// <summary>Determines if any elements in a matrix are either <see cref="float.PositiveInfinity" /> or <see cref="float.NegativeInfinity" />.</summary>
    /// <param name="value">The matrix to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are either <see cref="float.PositiveInfinity" /> or <see cref="float.NegativeInfinity" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAnyInfinity(Matrix4x4 value)
        => Vector4.IsAnyInfinity(value._x)
        || Vector4.IsAnyInfinity(value._y)
        || Vector4.IsAnyInfinity(value._z)
        || Vector4.IsAnyInfinity(value._w);

    /// <summary>Determines if any elements in a matrix are <see cref="float.NaN" />.</summary>
    /// <param name="value">The matrix to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are <see cref="float.NaN" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAnyNaN(Matrix4x4 value)
        => Vector4.IsAnyNaN(value._x)
        || Vector4.IsAnyNaN(value._y)
        || Vector4.IsAnyNaN(value._z)
        || Vector4.IsAnyNaN(value._w);

    /// <summary>Compares two matrices to determine if all elements are equal.</summary>
    /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
    /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if all elements of <paramref name="left" /> are equal to the corresponding element of <paramref name="right" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(Matrix4x4 left, in Matrix4x4 right) => left == right;

    /// <summary>Compares two matrices to determine approximate equality.</summary>
    /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
    /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (exclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(Matrix4x4 left, in Matrix4x4 right, in Matrix4x4 epsilon)
        => Vector4.EqualsAll(left._x, right._x, epsilon._x)
        && Vector4.EqualsAll(left._y, right._y, epsilon._y)
        && Vector4.EqualsAll(left._z, right._z, epsilon._z)
        && Vector4.EqualsAll(left._w, right._w, epsilon._w);

    /// <summary>Transposes a matrix.</summary>
    /// <param name="value">The matrix to transpose.</param>
    /// <returns>The transposition of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 Transpose(Matrix4x4 value)
    {
        if (Sse.IsSupported || AdvSimd.Arm64.IsSupported)
        {
            // X.X, Z.X, X.Y, Z.Y
            // X.Z, Z.Z, X.W, Z.W

            var vX = value._x.AsVector128();
            var vZ = value._z.AsVector128();

            var interleaveLowerXZ = InterleaveLower(vX, vZ);
            var interleaveUpperXZ = InterleaveUpper(vX, vZ);

            // Y.X, W.X, Y.Y, W.Y
            // Y.Z, W.Z, Y.W, W.W

            var vY = value._y.AsVector128();
            var vW = value._w.AsVector128();

            var interleaveLowerYW = InterleaveLower(vY, vW);
            var interleaveUpperYW = InterleaveUpper(vY, vW);

            // X.X, Y.X, Z.X, W.X
            // X.Y, Y.Y, Z.Y, W.Y
            // X.Z, Y.Z, Z.Z, W.Z
            // X.W, Y.W, Z.W, W.W

            return new Matrix4x4(
                InterleaveLower(interleaveLowerXZ, interleaveLowerYW),
                InterleaveUpper(interleaveLowerXZ, interleaveLowerYW),
                InterleaveLower(interleaveUpperXZ, interleaveUpperYW),
                InterleaveUpper(interleaveUpperXZ, interleaveUpperYW)
            );
        }
        else
        {
            return SoftwareFallback(value);
        }

        static Matrix4x4 SoftwareFallback(Matrix4x4 value)
        {
            return new Matrix4x4(
                new Vector4(value._x.X, value._y.X, value._z.X, value._w.X),
                new Vector4(value._x.Y, value._y.Y, value._z.Y, value._w.Y),
                new Vector4(value._x.Z, value._y.Z, value._z.Z, value._w.Z),
                new Vector4(value._x.W, value._y.W, value._z.W, value._w.W)
            );
        }
    }

    /// <summary>Reinterprets the current instance as a new <see cref="SysMatrix4x4" />.</summary>
    /// <returns>The current instance reintepreted as a new <see cref="SysMatrix4x4" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SysMatrix4x4 AsMatrix4x4() => As<Matrix4x4, SysMatrix4x4>(ref this);

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Matrix4x4 other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(Matrix4x4 other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return new StringBuilder(9 + (separator.Length * 3))
            .Append('<')
            .Append(X.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append(Y.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append(Z.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append(W.ToString(format, formatProvider))
            .Append('>')
            .ToString();
    }
}
