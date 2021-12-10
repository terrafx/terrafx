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

    /// <summary>Gets the determinant of the matrix.</summary>
    public float Determinant
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var result = GetDeterminant(this);
            return result.ToScalar();
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

    /// <summary>Compares two matrices to determine if all elements are equal.</summary>
    /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
    /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if all elements of <paramref name="left" /> are equal to the corresponding element of <paramref name="right" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Matrix4x4 left, in Matrix4x4 right) => left == right;

    /// <summary>Compares two matrices to determine approximate equality.</summary>
    /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
    /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (exclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Matrix4x4 left, in Matrix4x4 right, in Matrix4x4 epsilon)
        => Vector4.CompareEqualAll(left._x, right._x, epsilon._x)
        && Vector4.CompareEqualAll(left._y, right._y, epsilon._y)
        && Vector4.CompareEqualAll(left._z, right._z, epsilon._z)
        && Vector4.CompareEqualAll(left._w, right._w, epsilon._w);

    /// <summary>Creates a matrix from an affine transform.</summary>
    /// <param name="affineTransform">The affine transform of the matrix.</param>
    /// <returns>A matrix that represents <paramref name="affineTransform" />.</returns>
    public static Matrix4x4 CreateFromAffineTransform(AffineTransform affineTransform)
    {
        var result = CreateFromScale(affineTransform.Scale) * CreateFromRotation(affineTransform.Rotation);
        result.W += new Vector4(affineTransform.Translation, 0.0f);
        return result;
    }

    /// <summary>Creates a matrix from an affine transform.</summary>
    /// <param name="affineTransform">The affine transform of the matrix.</param>
    /// <param name="rotationOrigin">The origin rotation specified by <paramref name="affineTransform" />.</param>
    /// <returns>A matrix that represents <paramref name="affineTransform" />.</returns>
    public static Matrix4x4 CreateFromAffineTransform(AffineTransform affineTransform, Vector3 rotationOrigin)
    {
        var scaleMatrix = CreateFromScale(affineTransform.Scale);
        var vRotationOrigin = new Vector4(rotationOrigin, 0.0f);

        scaleMatrix.W -= vRotationOrigin;

        var result = scaleMatrix * CreateFromRotation(affineTransform.Rotation);
        result.W += vRotationOrigin;
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
            
            v0 = CreateFromYZAB(r1, r2);
            v0 = CreateFromXZWY(v0);
            v1 = CreateFromXXCC(r1, r2);
            v1 = CreateFromXZXZ(v1);
            
            q1 = CreateFromXWAB(r0, v0);
            q1 = CreateFromXZWY(q1);

            Unsafe.SkipInit(out Matrix4x4 m);
            {
                m.X = new Vector4(q1);

                q1 = CreateFromYWCD(r0, v0);
                q1 = CreateFromZXWY(q1);
                m.Y = new Vector4(q1);

                q1 = CreateFromXYCD(v1, r0);
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

    /// <summary>Computes the inverse of a matrix.</summary>
    /// <param name="value">The matrix to invert.</param>
    /// <param name="determinant">On return, contains the dterminant of the matrix.</param>
    /// <returns>The inverse of <paramref name="value" /> or an <c>infinite</c> matrix if no inverse exists.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 Inverse(Matrix4x4 value, out float determinant)
    {
        var transposedValue = Transpose(value);

        var transposedX = transposedValue.X.AsVector128();
        var transposedY = transposedValue.Y.AsVector128();
        var transposedZ = transposedValue.Z.AsVector128();
        var transposedW = transposedValue.W.AsVector128();

        var d0 = Multiply(CreateFromXXYY(transposedZ), CreateFromZWZW(transposedW));
        var d1 = Multiply(CreateFromXXYY(transposedX), CreateFromZWZW(transposedY));
        var d2 = Multiply(CreateFromXZAC(transposedZ, transposedX), CreateFromYWBD(transposedW, transposedY));

        d0 = MultiplyAddNegated(d0, CreateFromZWZW(transposedZ), CreateFromXXYY(transposedW));
        d1 = MultiplyAddNegated(d1, CreateFromZWZW(transposedX), CreateFromXXYY(transposedY));
        d2 = MultiplyAddNegated(d2, CreateFromYWBD(transposedZ, transposedX), CreateFromXZAC(transposedW, transposedY));

        var tmp1 = CreateFromYWBB(d0, d2);
        var tmp2 = CreateFromYWDD(d1, d2);

        var c0 = Multiply(CreateFromYZXY(transposedY), CreateFromZXDA(tmp1, d0));
        var c2 = Multiply(CreateFromZXYX(transposedX), CreateFromYZBC(tmp1, d0));
        var c4 = Multiply(CreateFromYZXY(transposedW), CreateFromZXDA(tmp2, d1));
        var c6 = Multiply(CreateFromZXYX(transposedZ), CreateFromYZBC(tmp2, d1));

        var tmp3 = CreateFromXYAA(d0, d2);
        var tmp4 = CreateFromXYCC(d1, d2);

        c0 = MultiplyAddNegated(c0, CreateFromZWYZ(transposedY), CreateFromWXBC(d0, tmp3));
        c2 = MultiplyAddNegated(c2, CreateFromWZWY(transposedX), CreateFromZYCA(d0, tmp3));
        c4 = MultiplyAddNegated(c4, CreateFromZWYZ(transposedW), CreateFromWXBC(d1, tmp4));
        c6 = MultiplyAddNegated(c6, CreateFromWZWY(transposedZ), CreateFromZYCA(d1, tmp4));

        var v00 = Multiply(CreateFromWXWX(transposedY), CreateFromXWZX(CreateFromZZAB(d0, d2)));
        var v01 = Multiply(CreateFromYWXZ(transposedX), CreateFromWXYZ(CreateFromXWAB(d0, d2)));
        var v02 = Multiply(CreateFromWXWX(transposedW), CreateFromXWZX(CreateFromZZCD(d1, d2)));
        var v03 = Multiply(CreateFromYWXZ(transposedZ), CreateFromWXYZ(CreateFromXWCD(d1, d2)));

        var c1 = Subtract(c0, v00);
        c0 = Add(c0, v00);

        var c3 = Add(c2, v01);
        c2 = Subtract(c2, v01);

        var c5 = Subtract(c4, v02);
        c4 = Add(c4, v02);

        var c7 = Add(c6, v03);
        c6 = Subtract(c6, v03);

        c0 = CreateFromXZBD(c0, c1);
        c2 = CreateFromXZBD(c2, c3);
        c4 = CreateFromXZBD(c4, c5);
        c6 = CreateFromXZBD(c6, c7);
        c0 = CreateFromXZYW(c0);
        c2 = CreateFromXZYW(c2);
        c4 = CreateFromXZYW(c4);
        c6 = CreateFromXZYW(c6);

        var vDeterminant = DotProduct(c0, transposedX);
        determinant = vDeterminant.ToScalar();

        return new Matrix4x4(
            Divide(c0, vDeterminant),
            Divide(c2, vDeterminant),
            Divide(c4, vDeterminant),
            Divide(c6, vDeterminant)
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

    /// <summary>Determines if a matrix is equal to <see cref="Identity" />.</summary>
    /// <param name="value">The matrix to compare against <see cref="Identity" />.</param>
    /// <returns><c>true</c> if <paramref name="value" /> and <see cref="Identity" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsIdentity(Matrix4x4 value) => value == Identity;

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

    private static Vector128<float> GetDeterminant(Matrix4x4 value)
    {
        var vZ = value.Z.AsVector128();
        var vW = value.W.AsVector128();

        var vZ_YXXX = CreateFromYXXX(vZ);
        var vZ_ZZYY = CreateFromZZYY(vZ);
        var vZ_WWWZ = CreateFromWWWZ(vZ);

        var vW_YXXX = CreateFromYXXX(vW);
        var vW_ZZYY = CreateFromZZYY(vW);
        var vW_WWWZ = CreateFromWWWZ(vW);

        var p0 = Multiply(vZ_YXXX, vW_ZZYY);
        var p1 = Multiply(vZ_YXXX, vW_WWWZ);
        var p2 = Multiply(vZ_ZZYY, vW_WWWZ);

        p0 = MultiplyAddNegated(p0, vZ_ZZYY, vW_YXXX);
        p1 = MultiplyAddNegated(p1, vZ_WWWZ, vW_YXXX);
        p2 = MultiplyAddNegated(p2, vZ_WWWZ, vW_ZZYY);

        var vY = value.Y.AsVector128();

        var vY_YXXX = CreateFromYXXX(vY);
        var vY_ZZYY = CreateFromZZYY(vY);
        var vY_WWWZ = CreateFromWWWZ(vY);

        var s = Multiply(value.X.AsVector128(), Vector128.Create(1.0f, -1.0f, 1.0f, -1.0f));
        var r = Multiply(vY_WWWZ, p0);

        r = MultiplyAddNegated(r, vY_ZZYY, p1);
        r = MultiplyAdd(r, vY_YXXX, p2);

        return DotProduct(s, r);
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
