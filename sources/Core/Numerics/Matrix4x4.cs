// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using TerraFX.Utilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VectorUtilities;
using SysMatrix4x4 = System.Numerics.Matrix4x4;
using SysVector4 = System.Numerics.Vector4;

namespace TerraFX.Numerics;

/// <summary>Defines a 4x4 row-major matrix.</summary>
public struct Matrix4x4
    : IEquatable<Matrix4x4>,
      IFormattable,
      ISpanFormattable,
      IUtf8SpanFormattable
{
    /// <summary>Defines a matrix where all components are zero.</summary>
    public static Matrix4x4 Zero => Create(Vector128<float>.Zero, Vector128<float>.Zero, Vector128<float>.Zero, Vector128<float>.Zero);

    /// <summary>Defines the identity matrix.</summary>
    public static Matrix4x4 Identity => Create(UnitX, UnitY, UnitZ, UnitW);

    private Vector128<float> _x;
    private Vector128<float> _y;
    private Vector128<float> _z;
    private Vector128<float> _w;

    /// <summary>Creates a matrix from a system matrix.</summary>
    /// <param name="value">The value of the matrix.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 Create(SysMatrix4x4 value) => As<SysMatrix4x4, Matrix4x4>(ref value);

    /// <summary>Creates a matrix from system vectors representing the X, Y, Z, and W components.</summary>
    /// <param name="x">The value of the x-component.</param>
    /// <param name="y">The value of the y-component.</param>
    /// <param name="z">The value of the z-component.</param>
    /// <param name="w">The value of the w-component.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 Create(SysVector4 x, SysVector4 y, SysVector4 z, SysVector4 w)
    {
        return Create(
            x.AsVector128(),
            y.AsVector128(),
            z.AsVector128(),
            w.AsVector128()
        );
    }

    /// <summary>Creates a matrix from vectors representing the X, Y, Z, and W components.</summary>
    /// <param name="x">The value of the x-component.</param>
    /// <param name="y">The value of the y-component.</param>
    /// <param name="z">The value of the z-component.</param>
    /// <param name="w">The value of the w-component.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 Create(Vector4 x, Vector4 y, Vector4 z, Vector4 w)
    {
        return Create(
            x.Value,
            y.Value,
            z.Value,
            w.Value
        );
    }

    /// <summary>Creates a matrix from hardware vectors representing the X, Y, Z, and W components.</summary>
    /// <param name="x">The value of the x-component.</param>
    /// <param name="y">The value of the y-component.</param>
    /// <param name="z">The value of the z-component.</param>
    /// <param name="w">The value of the w-component.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 Create(Vector128<float> x, Vector128<float> y, Vector128<float> z, Vector128<float> w)
    {
        Matrix4x4 result;

        result._x = x;
        result._y = y;
        result._z = z;
        result._w = w;

        return result;
    }

    /// <summary>Creates a matrix from an affine transform.</summary>
    /// <param name="affineTransform">The affine transform of the matrix.</param>
    /// <returns>A matrix that represents <paramref name="affineTransform" />.</returns>
    public static Matrix4x4 CreateFromAffineTransform(AffineTransform affineTransform)
    {
        var result = CreateFromScale(affineTransform.Scale) * CreateFromQuaternion(affineTransform.Rotation);
        result._w = Add(result._w, affineTransform.Translation.AsVector128());
        return result;
    }

    /// <summary>Creates a matrix from an affine transform.</summary>
    /// <param name="affineTransform">The affine transform of the matrix.</param>
    /// <param name="rotationOrigin">The origin rotation specified by <paramref name="affineTransform" />.</param>
    /// <returns>A matrix that represents <paramref name="affineTransform" />.</returns>
    public static Matrix4x4 CreateFromAffineTransform(AffineTransform affineTransform, Vector3 rotationOrigin)
    {
        var scaleMatrix = CreateFromScale(affineTransform.Scale);
        var vRotationOrigin = rotationOrigin.AsVector128();

        scaleMatrix._w = Subtract(scaleMatrix._w, vRotationOrigin);

        var result = scaleMatrix * CreateFromQuaternion(affineTransform.Rotation);
        result._w = Add(result._w, vRotationOrigin);
        result._w = Add(result._w, affineTransform.Translation.AsVector128());
        return result;
    }

    /// <summary>Creates a matrix from a quaternion.</summary>
    /// <param name="quaternion">A quaternion representing the rotation of the matrix.</param>
    /// <returns>A matrix that represents <paramref name="quaternion" />.</returns>
    public static Matrix4x4 CreateFromQuaternion(Quaternion quaternion)
    {
        var vQuaternion = quaternion.Value;

        var q0 = Add(vQuaternion, vQuaternion);
        var q1 = Multiply(vQuaternion, q0);
        q1 = BitwiseAnd(q1, Vector128.Create(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0x00000000).AsSingle());

        var v0 = CreateFromYXXW(q1);
        var v1 = CreateFromZZYW(q1);
        var r0 = Subtract(Vector128.Create(1.0f, 1.0f, 1.0f, 0.0f), v0);
        r0 = Subtract(r0, v1);

        v0 = CreateFromXXYW(vQuaternion);
        v1 = CreateFromZYZW(q0);
        v0 = Multiply(v0, v1);

        v1 = CreateFromW(vQuaternion);
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

        Matrix4x4 m;
        {
            m._x = q1;

            q1 = CreateFromYWCD(r0, v0);
            q1 = CreateFromZXWY(q1);
            m._y = q1;

            q1 = CreateFromXYCD(v1, r0);
            m._z = q1;

            m._w = UnitW;
        }
        return m;
    }

    /// <summary>Creates a matrix from a the specified rotation around the x-axis.</summary>
    /// <param name="rotationX">A float representing the rotation around the x-axis for the matrix.</param>
    /// <returns>A matrix that represents <paramref name="rotationX" />.</returns>
    public static Matrix4x4 CreateFromRotationX(float rotationX)
    {
        var (sin, cos) = SinCos(rotationX);
        var tmp = CreateFromWXAD(Vector128.CreateScalar(cos), Vector128.CreateScalar(sin));

        return Create(
            UnitX,
            tmp,
            Multiply(CreateFromXZYW(tmp), Vector128.Create(1.0f, -1.0f, 1.0f, 1.0f)),
            UnitW
        );
    }

    /// <summary>Creates a matrix from a the specified rotation around the y-axis.</summary>
    /// <param name="rotationY">A float representing the rotation around the y-axis for the matrix.</param>
    /// <returns>A matrix that represents <paramref name="rotationY" />.</returns>
    public static Matrix4x4 CreateFromRotationY(float rotationY)
    {
        var (sin, cos) = SinCos(rotationY);
        var tmp = CreateFromXWAD(Vector128.CreateScalar(sin), Vector128.CreateScalar(cos));

        return Create(
            Multiply(CreateFromZYXW(tmp), Vector128.Create(1.0f, 1.0f, -1.0f, 1.0f)),
            UnitY,
            tmp,
            UnitW
        );
    }

    /// <summary>Creates a matrix from a the specified rotation around the z-axis.</summary>
    /// <param name="rotationZ">A float representing the rotation around the z-axis for the matrix.</param>
    /// <returns>A matrix that represents <paramref name="rotationZ" />.</returns>
    public static Matrix4x4 CreateFromRotationZ(float rotationZ)
    {
        var (sin, cos) = SinCos(rotationZ);
        var tmp = InterleaveLower(Vector128.CreateScalar(cos), Vector128.CreateScalar(sin));

        return Create(
            tmp,
            Multiply(CreateFromYXZW(tmp), Vector128.Create(-1.0f, 1.0f, 1.0f, 1.0f)),
            UnitZ,
            UnitW
        );
    }

    /// <summary>Creates a matrix from a scaling vector.</summary>
    /// <param name="scale">A vector representing the scale of the matrix.</param>
    /// <returns>A matrix that represents <paramref name="scale" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateFromScale(Vector3 scale)
    {
        var vScale = scale.AsVector128();

        return Create(
            BitwiseAnd(vScale, Vector128.Create(0xFFFFFFFF, 0x00000000, 0x00000000, 0x00000000).AsSingle()),
            BitwiseAnd(vScale, Vector128.Create(0x00000000, 0xFFFFFFFF, 0x00000000, 0x00000000).AsSingle()),
            BitwiseAnd(vScale, Vector128.Create(0x00000000, 0x00000000, 0xFFFFFFFF, 0x00000000).AsSingle()),
            UnitW
        );
    }

    /// <summary>Creates a matrix from a translation vector.</summary>
    /// <param name="translation">A vector representing the translation of the matrix.</param>
    /// <returns>A matrix that represents <paramref name="translation" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateFromTranslation(Vector3 translation)
    {
        return Create(
            UnitX,
            UnitY,
            UnitZ,
            translation.AsVector128().WithW(1.0f)
        );
    }

    /// <summary>Gets the value of the x-component.</summary>
    public Vector4 X
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return Vector4.Create(_x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _x = value.Value;
        }
    }

    /// <summary>Gets the value of the y-component.</summary>
    public Vector4 Y
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return Vector4.Create(_y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _y = value.Value;
        }
    }

    /// <summary>Gets the value of the z-component.</summary>
    public Vector4 Z
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return Vector4.Create(_z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _z = value.Value;
        }
    }

    /// <summary>Gets the value of the w-component.</summary>
    public Vector4 W
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            return Vector4.Create(_w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _w = value.Value;
        }
    }

    /// <summary>Gets the determinant of the matrix.</summary>
    public readonly float Determinant
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
    {
        return VectorUtilities.CompareEqualAll(left._x, right._x)
            && VectorUtilities.CompareEqualAll(left._y, right._y)
            && VectorUtilities.CompareEqualAll(left._z, right._z)
            && VectorUtilities.CompareEqualAll(left._w, right._w);
    }

    /// <summary>Compares two matrices to determine inequality.</summary>
    /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
    /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Matrix4x4 left, in Matrix4x4 right)
    {
        return CompareNotEqualAny(left._x, right._x)
            || CompareNotEqualAny(left._y, right._y)
            || CompareNotEqualAny(left._z, right._z)
            || CompareNotEqualAny(left._w, right._w);
    }

    /// <summary>Computes the product of two matrices.</summary>
    /// <param name="left">The matrix to multiply by <paramref name="right" />.</param>
    /// <param name="right">The matrix which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    public static Matrix4x4 operator *(Matrix4x4 left, in Matrix4x4 right)
    {
        return Create(
            ComputeRow(in right, left._x),
            ComputeRow(in right, left._y),
            ComputeRow(in right, left._z),
            ComputeRow(in right, left._w)
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Vector128<float> ComputeRow(in Matrix4x4 left, Vector128<float> right)
        {
            var sum1 = MultiplyByX(left._x, right);
            sum1 = MultiplyAddByZ(sum1, left._z, right);

            var sum2 = MultiplyByY(left._y, right);
            sum2 = MultiplyAddByW(sum2, left._w, right);

            return Add(sum1, sum2);
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
    /// <param name="epsilon">The maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Matrix4x4 left, in Matrix4x4 right, in Matrix4x4 epsilon)
    {
        return VectorUtilities.CompareEqualAll(left._x, right._x, epsilon._x)
            && VectorUtilities.CompareEqualAll(left._y, right._y, epsilon._y)
            && VectorUtilities.CompareEqualAll(left._z, right._z, epsilon._z)
            && VectorUtilities.CompareEqualAll(left._w, right._w, epsilon._w);
    }

    /// <summary>Creates a matrix that represents a camera looking at a focus in a left-handed coordinate system.</summary>
    /// <param name="position">The location of the camera.</param>
    /// <param name="focus">The focus of the camera.</param>
    /// <param name="up">The direction of up.</param>
    /// <returns>A matrix that represents a camera at <paramref name="position" /> looking at <paramref name="focus" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateLookAtLH(Vector3 position, Vector3 focus, Vector3 up)
    {
        var direction = focus - position;
        return CreateLookToLH(position, direction, up);
    }

    /// <summary>Creates a matrix that represents a camera looking at a focus in a right-handed coordinate system.</summary>
    /// <param name="position">The location of the camera.</param>
    /// <param name="focus">The focus of the camera.</param>
    /// <param name="up">The direction of up.</param>
    /// <returns>A matrix that represents a camera at <paramref name="position" /> looking at <paramref name="focus" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateLookAtRH(Vector3 position, Vector3 focus, Vector3 up)
    {
        var negativeDirection = position - focus;
        return CreateLookToLH(position, negativeDirection, up);
    }

    /// <summary>Creates a matrix that represents a camera looking to a direction in a left-handed coordinate system.</summary>
    /// <param name="position">The location of the camera.</param>
    /// <param name="direction">The direction of the camera.</param>
    /// <param name="up">The direction of up.</param>
    /// <returns>A matrix that represents a camera at <paramref name="position" /> looking to <paramref name="direction" />.</returns>
    public static Matrix4x4 CreateLookToLH(Vector3 position, Vector3 direction, Vector3 up)
    {
        Assert(direction != Vector3.Zero);
        Assert(!Vector3.IsAnyInfinity(direction));
        Assert(up != Vector3.Zero);
        Assert(!Vector3.IsAnyInfinity(up));

        var r2 = Normalize(direction.AsVector128());
        var r0 = Normalize(CrossProduct(up.AsVector128(), r2));
        var r1 = CrossProduct(r2, r0);

        var negativePosition = Negate(position.AsVector128());

        var result = Create(
            r0.WithW(DotProduct(r0, negativePosition).ToScalar()),
            r1.WithW(DotProduct(r1, negativePosition).ToScalar()),
            r2.WithW(DotProduct(r2, negativePosition).ToScalar()),
            UnitW
        );
        return Transpose(result);
    }

    /// <summary>Creates a matrix that represents a camera looking to a direction in a right-handed coordinate system.</summary>
    /// <param name="position">The location of the camera.</param>
    /// <param name="direction">The direction of the camera.</param>
    /// <param name="up">The direction of up.</param>
    /// <returns>A matrix that represents a camera at <paramref name="position" /> looking to <paramref name="direction" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateLookToRH(Vector3 position, Vector3 direction, Vector3 up)
    {
        var negativeDirection = -direction;
        return CreateLookToLH(position, negativeDirection, up);
    }

    /// <summary>Creates a matrix that represents an orthographic projection in a left-handed coordinate system.</summary>
    /// <param name="frustumWidth">The width of the frustum.</param>
    /// <param name="frustumHeight">The height of the frustum.</param>
    /// <param name="nearClippingDistance">The distance to the near clipping plane.</param>
    /// <param name="farClippingDistance">The distance to the far clipping plane.</param>
    /// <returns>A matrix that represents a orthographic projection.</returns>
    public static Matrix4x4 CreateOrthographicLH(float frustumWidth, float frustumHeight, float nearClippingDistance, float farClippingDistance)
    {
        Assert(!CompareEqual(frustumWidth, 0.0f, NearZeroEpsilon));
        Assert(!CompareEqual(frustumHeight, 0.0f, NearZeroEpsilon));
        Assert(!CompareEqual(farClippingDistance, nearClippingDistance, NearZeroEpsilon));

        var range = 1.0f / (farClippingDistance - nearClippingDistance);

        return Create(
            Vector128.CreateScalar(2.0f / frustumWidth),
            Vector128<float>.Zero.WithY(2.0f / frustumHeight),
            Vector128<float>.Zero.WithZ(range),
            UnitW.WithZ(-range * nearClippingDistance)
        );
    }

    /// <summary>Creates a matrix that represents an orthographic projection in a right-handed coordinate system.</summary>
    /// <param name="frustumWidth">The width of the frustum.</param>
    /// <param name="frustumHeight">The height of the frustum.</param>
    /// <param name="nearClippingDistance">The distance to the near clipping plane.</param>
    /// <param name="farClippingDistance">The distance to the far clipping plane.</param>
    /// <returns>A matrix that represents a orthographic projection.</returns>
    public static Matrix4x4 CreateOrthographicRH(float frustumWidth, float frustumHeight, float nearClippingDistance, float farClippingDistance)
    {
        Assert(!CompareEqual(frustumWidth, 0.0f, NearZeroEpsilon));
        Assert(!CompareEqual(frustumHeight, 0.0f, NearZeroEpsilon));
        Assert(!CompareEqual(farClippingDistance, nearClippingDistance, NearZeroEpsilon));

        var range = 1.0f / (nearClippingDistance - farClippingDistance);

        return Create(
            Vector128.CreateScalar(2.0f / frustumWidth),
            Vector128<float>.Zero.WithY(2.0f / frustumHeight),
            Vector128<float>.Zero.WithZ(range),
            UnitW.WithZ(range * nearClippingDistance)
        );
    }

    /// <summary>Creates a matrix that represents an off-center orthographic projection in a left-handed coordinate system.</summary>
    /// <param name="frustumLeft">The x-coordinate of the left side of the frustum.</param>
    /// <param name="frustumRight">The x-coordinate of the right side of the frustum.</param>
    /// <param name="frustumBottom">The y-coordinate of the bottom side of the frustum.</param>
    /// <param name="frustumTop">The y-coordinate of the top side of the frustum.</param>
    /// <param name="nearClippingDistance">The distance to the near clipping plane.</param>
    /// <param name="farClippingDistance">The distance to the far clipping plane.</param>
    /// <returns>A matrix that represents a orthographic projection.</returns>
    public static Matrix4x4 CreateOrthographicOffCenterLH(float frustumLeft, float frustumRight, float frustumBottom, float frustumTop, float nearClippingDistance, float farClippingDistance)
    {
        Assert(!CompareEqual(frustumLeft, frustumRight, NearZeroEpsilon * 2));
        Assert(!CompareEqual(frustumTop, frustumBottom, NearZeroEpsilon));
        Assert(!CompareEqual(farClippingDistance, nearClippingDistance, NearZeroEpsilon));

        var reciprocalWidth = 1.0f / (frustumRight - frustumLeft);
        var reciprocalHeight = 1.0f / (frustumTop - frustumBottom);
        var range = 1.0f / (farClippingDistance - nearClippingDistance);

        return Create(
            Vector128.CreateScalar(reciprocalWidth + reciprocalWidth),
            Vector128<float>.Zero.WithY(reciprocalHeight + reciprocalHeight),
            Vector128<float>.Zero.WithZ(range),
            Vector128.Create(-(frustumLeft + frustumRight) * reciprocalWidth, -(frustumTop + frustumBottom) * reciprocalHeight, -range * nearClippingDistance, 1.0f)
        );
    }

    /// <summary>Creates a matrix that represents an off-center orthographic projection in a right-handed coordinate system.</summary>
    /// <param name="frustumLeft">The x-coordinate of the left side of the frustum.</param>
    /// <param name="frustumRight">The x-coordinate of the right side of the frustum.</param>
    /// <param name="frustumBottom">The y-coordinate of the bottom side of the frustum.</param>
    /// <param name="frustumTop">The y-coordinate of the top side of the frustum.</param>
    /// <param name="nearClippingDistance">The distance to the near clipping plane.</param>
    /// <param name="farClippingDistance">The distance to the far clipping plane.</param>
    /// <returns>A matrix that represents a orthographic projection.</returns>
    public static Matrix4x4 CreateOrthographicOffCenterRH(float frustumLeft, float frustumRight, float frustumBottom, float frustumTop, float nearClippingDistance, float farClippingDistance)
    {
        Assert(!CompareEqual(frustumLeft, frustumRight, NearZeroEpsilon * 2));
        Assert(!CompareEqual(frustumTop, frustumBottom, NearZeroEpsilon));
        Assert(!CompareEqual(farClippingDistance, nearClippingDistance, NearZeroEpsilon));

        var reciprocalWidth = 1.0f / (frustumRight - frustumLeft);
        var reciprocalHeight = 1.0f / (frustumTop - frustumBottom);
        var range = 1.0f / (nearClippingDistance - farClippingDistance);

        return Create(
            Vector128.CreateScalar(reciprocalWidth + reciprocalWidth),
            Vector128<float>.Zero.WithY(reciprocalHeight + reciprocalHeight),
            Vector128<float>.Zero.WithZ(range),
            Vector128.Create(-(frustumLeft + frustumRight) * reciprocalWidth, -(frustumTop + frustumBottom) * reciprocalHeight, range * nearClippingDistance, 1.0f)
        );
    }

    /// <summary>Creates a matrix that represents a perspective projection in a left-handed coordinate system.</summary>
    /// <param name="frustumWidth">The width of the frustum.</param>
    /// <param name="frustumHeight">The height of the frustum.</param>
    /// <param name="nearClippingDistance">The distance to the near clipping plane.</param>
    /// <param name="farClippingDistance">The distance to the far clipping plane.</param>
    /// <returns>A matrix that represents a perspective projection.</returns>
    public static Matrix4x4 CreatePerspectiveLH(float frustumWidth, float frustumHeight, float nearClippingDistance, float farClippingDistance)
    {
        Assert((nearClippingDistance > 0.0f) && (0.0f < farClippingDistance));
        Assert(!CompareEqual(frustumWidth, 0.0f, NearZeroEpsilon));
        Assert(!CompareEqual(frustumHeight, 0.0f, NearZeroEpsilon));
        Assert(!CompareEqual(farClippingDistance, nearClippingDistance, NearZeroEpsilon));

        var doubleNearClippingDistance = nearClippingDistance + nearClippingDistance;
        var range = farClippingDistance / (farClippingDistance - nearClippingDistance);

        return Create(
            Vector128.CreateScalar(doubleNearClippingDistance / frustumWidth),
            Vector128<float>.Zero.WithY(doubleNearClippingDistance / frustumHeight),
            UnitW.WithZ(range),
            Vector128<float>.Zero.WithZ(-range * nearClippingDistance)
        );
    }

    /// <summary>Creates a matrix that represents a perspective projection in a right-handed coordinate system.</summary>
    /// <param name="frustumWidth">The width of the frustum.</param>
    /// <param name="frustumHeight">The height of the frustum.</param>
    /// <param name="nearClippingDistance">The distance to the near clipping plane.</param>
    /// <param name="farClippingDistance">The distance to the far clipping plane.</param>
    /// <returns>A matrix that represents a perspective projection.</returns>
    public static Matrix4x4 CreatePerspectiveRH(float frustumWidth, float frustumHeight, float nearClippingDistance, float farClippingDistance)
    {
        Assert((nearClippingDistance > 0.0f) && (0.0f < farClippingDistance));
        Assert(!CompareEqual(frustumWidth, 0.0f, NearZeroEpsilon));
        Assert(!CompareEqual(frustumHeight, 0.0f, NearZeroEpsilon));
        Assert(!CompareEqual(farClippingDistance, nearClippingDistance, NearZeroEpsilon));

        var doubleNearClippingDistance = nearClippingDistance + nearClippingDistance;
        var range = farClippingDistance / (nearClippingDistance - farClippingDistance);

        return Create(
            Vector128.CreateScalar(doubleNearClippingDistance / frustumWidth),
            Vector128<float>.Zero.WithY(doubleNearClippingDistance / frustumHeight),
            Vector128.Create(0.0f, 0.0f, 0.0f, -1.0f).WithZ(range),
            Vector128<float>.Zero.WithZ(range * nearClippingDistance)
        );
    }

    /// <summary>Creates a matrix that represents a perspective projection based on a field-of-view in a left-handed coordinate system.</summary>
    /// <param name="fieldOfView">The field-of-view angle, in radians.</param>
    /// <param name="aspectRatio">The aspect ratio, as X:Y.</param>
    /// <param name="nearClippingDistance">The distance to the near clipping plane.</param>
    /// <param name="farClippingDistance">The distance to the far clipping plane.</param>
    /// <returns>A matrix that represents a perspective projection based on <paramref name="fieldOfView" />.</returns>
    public static Matrix4x4 CreatePerspectiveFieldOfViewLH(float fieldOfView, float aspectRatio, float nearClippingDistance, float farClippingDistance)
    {
        Assert((nearClippingDistance > 0.0f) && (0.0f < farClippingDistance));
        Assert(!CompareEqual(fieldOfView, 0.0f, NearZeroEpsilon * 2));
        Assert(!CompareEqual(aspectRatio, 0.0f, NearZeroEpsilon));
        Assert(!CompareEqual(farClippingDistance, nearClippingDistance, NearZeroEpsilon));

        var (sin, cos) = SinCos(0.5f * fieldOfView);

        var height = cos / sin;
        var width = height / aspectRatio;
        var range = farClippingDistance / (farClippingDistance - nearClippingDistance);

        return Create(
            Vector128.CreateScalar(width),
            Vector128<float>.Zero.WithY(height),
            UnitW.WithZ(range),
            Vector128<float>.Zero.WithZ(-range * nearClippingDistance)
        );
    }

    /// <summary>Creates a matrix that represents a perspective projection based on a field-of-view in a right-handed coordinate system.</summary>
    /// <param name="fieldOfView">The field-of-view angle, in radians.</param>
    /// <param name="aspectRatio">The aspect ratio, as X:Y.</param>
    /// <param name="nearClippingDistance">The distance to the near clipping plane.</param>
    /// <param name="farClippingDistance">The distance to the far clipping plane.</param>
    /// <returns>A matrix that represents a perspective projection based on <paramref name="fieldOfView" />.</returns>
    public static Matrix4x4 CreatePerspectiveFieldOfViewRH(float fieldOfView, float aspectRatio, float nearClippingDistance, float farClippingDistance)
    {
        Assert((nearClippingDistance > 0.0f) && (0.0f < farClippingDistance));
        Assert(!CompareEqual(fieldOfView, 0.0f, NearZeroEpsilon * 2));
        Assert(!CompareEqual(aspectRatio, 0.0f, NearZeroEpsilon));
        Assert(!CompareEqual(farClippingDistance, nearClippingDistance, NearZeroEpsilon));

        var (sin, cos) = SinCos(0.5f * fieldOfView);

        var height = cos / sin;
        var width = height / aspectRatio;
        var range = farClippingDistance / (nearClippingDistance - farClippingDistance);

        return Create(
            Vector128.CreateScalar(width),
            Vector128<float>.Zero.WithY(height),
            Vector128.Create(0.0f, 0.0f, 0.0f, -1.0f).WithZ(range),
            Vector128<float>.Zero.WithZ(range * nearClippingDistance)
        );
    }

    /// <summary>Creates a matrix that represents an off-center perspective projection in a left-handed coordinate system.</summary>
    /// <param name="frustumLeft">The x-coordinate of the left side of the frustum.</param>
    /// <param name="frustumRight">The x-coordinate of the right side of the frustum.</param>
    /// <param name="frustumBottom">The y-coordinate of the bottom side of the frustum.</param>
    /// <param name="frustumTop">The y-coordinate of the top side of the frustum.</param>
    /// <param name="nearClippingDistance">The distance to the near clipping plane.</param>
    /// <param name="farClippingDistance">The distance to the far clipping plane.</param>
    /// <returns>A matrix that represents an off-center perspective projection.</returns>
    public static Matrix4x4 CreatePerspectiveOffCenterLH(float frustumLeft, float frustumRight, float frustumBottom, float frustumTop, float nearClippingDistance, float farClippingDistance)
    {
        Assert((nearClippingDistance > 0.0f) && (0.0f < farClippingDistance));
        Assert(!CompareEqual(frustumLeft, frustumRight, NearZeroEpsilon * 2));
        Assert(!CompareEqual(frustumTop, frustumBottom, NearZeroEpsilon));
        Assert(!CompareEqual(farClippingDistance, nearClippingDistance, NearZeroEpsilon));

        var doubleNearClippingDistance = nearClippingDistance + nearClippingDistance;
        var reciprocalWidth = 1.0f / (frustumRight - frustumLeft);
        var reciprocalHeight = 1.0f / (frustumTop - frustumBottom);
        var range = farClippingDistance / (farClippingDistance - nearClippingDistance);

        return Create(
            Vector128.CreateScalar(doubleNearClippingDistance * reciprocalWidth),
            Vector128<float>.Zero.WithY(doubleNearClippingDistance * reciprocalHeight),
            Vector128.Create(-(frustumLeft + frustumRight) * reciprocalWidth, -(frustumTop + frustumBottom) * reciprocalHeight, range, 1.0f),
            Vector128<float>.Zero.WithZ(-range * nearClippingDistance)
        );
    }

    /// <summary>Creates a matrix that represents an off-center perspective projection in a right-handed coordinate system.</summary>
    /// <param name="frustumLeft">The x-coordinate of the left side of the frustum.</param>
    /// <param name="frustumRight">The x-coordinate of the right side of the frustum.</param>
    /// <param name="frustumBottom">The y-coordinate of the bottom side of the frustum.</param>
    /// <param name="frustumTop">The y-coordinate of the top side of the frustum.</param>
    /// <param name="nearClippingDistance">The distance to the near clipping plane.</param>
    /// <param name="farClippingDistance">The distance to the far clipping plane.</param>
    /// <returns>A matrix that represents an off-center perspective projection.</returns>
    public static Matrix4x4 CreatePerspectiveOffCenterRH(float frustumLeft, float frustumRight, float frustumBottom, float frustumTop, float nearClippingDistance, float farClippingDistance)
    {
        Assert((nearClippingDistance > 0.0f) && (0.0f < farClippingDistance));
        Assert(!CompareEqual(frustumLeft, frustumRight, NearZeroEpsilon * 2));
        Assert(!CompareEqual(frustumTop, frustumBottom, NearZeroEpsilon));
        Assert(!CompareEqual(farClippingDistance, nearClippingDistance, NearZeroEpsilon));

        var doubleNearClippingDistance = nearClippingDistance + nearClippingDistance;
        var reciprocalWidth = 1.0f / (frustumRight - frustumLeft);
        var reciprocalHeight = 1.0f / (frustumTop - frustumBottom);
        var range = farClippingDistance / (nearClippingDistance - farClippingDistance);

        return Create(
            Vector128.CreateScalar(doubleNearClippingDistance * reciprocalWidth),
            Vector128<float>.Zero.WithY(doubleNearClippingDistance * reciprocalHeight),
            Vector128.Create((frustumLeft + frustumRight) * reciprocalWidth, (frustumTop + frustumBottom) * reciprocalHeight, range, -1.0f),
            Vector128<float>.Zero.WithZ(range * nearClippingDistance)
        );
    }

    /// <summary>Computes the inverse of a matrix.</summary>
    /// <param name="value">The matrix to invert.</param>
    /// <param name="determinant">On return, contains the determinant of the matrix.</param>
    /// <returns>The inverse of <paramref name="value" /> or an <c>infinite</c> matrix if no inverse exists.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 Inverse(Matrix4x4 value, out float determinant)
    {
        var transposedValue = Transpose(value);

        var transposedX = transposedValue._x;
        var transposedY = transposedValue._y;
        var transposedZ = transposedValue._z;
        var transposedW = transposedValue._w;

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

        return Create(
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
    {
        return VectorUtilities.IsAnyInfinity(value._x)
            || VectorUtilities.IsAnyInfinity(value._y)
            || VectorUtilities.IsAnyInfinity(value._z)
            || VectorUtilities.IsAnyInfinity(value._w);
    }

    /// <summary>Determines if any elements in a matrix are <see cref="float.NaN" />.</summary>
    /// <param name="value">The matrix to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are <see cref="float.NaN" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAnyNaN(Matrix4x4 value)
    {
        return VectorUtilities.IsAnyNaN(value._x)
            || VectorUtilities.IsAnyNaN(value._y)
            || VectorUtilities.IsAnyNaN(value._z)
            || VectorUtilities.IsAnyNaN(value._w);
    }

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
        // X.X, Z.X, X.Y, Z.Y
        // X.Z, Z.Z, X.W, Z.W

        var vX = value._x;
        var vZ = value._z;

        var interleaveLowerXZ = InterleaveLower(vX, vZ);
        var interleaveUpperXZ = InterleaveUpper(vX, vZ);

        // Y.X, W.X, Y.Y, W.Y
        // Y.Z, W.Z, Y.W, W.W

        var vY = value._y;
        var vW = value._w;

        var interleaveLowerYW = InterleaveLower(vY, vW);
        var interleaveUpperYW = InterleaveUpper(vY, vW);

        // X.X, Y.X, Z.X, W.X
        // X.Y, Y.Y, Z.Y, W.Y
        // X.Z, Y.Z, Z.Z, W.Z
        // X.W, Y.W, Z.W, W.W

        return Create(
            InterleaveLower(interleaveLowerXZ, interleaveLowerYW),
            InterleaveUpper(interleaveLowerXZ, interleaveLowerYW),
            InterleaveLower(interleaveUpperXZ, interleaveUpperYW),
            InterleaveUpper(interleaveUpperXZ, interleaveUpperYW)
        );
    }

    private static Vector128<float> GetDeterminant(Matrix4x4 value)
    {
        var vZ = value._z;
        var vW = value._w;

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

        var vY = value._y;

        var vY_YXXX = CreateFromYXXX(vY);
        var vY_ZZYY = CreateFromZZYY(vY);
        var vY_WWWZ = CreateFromWWWZ(vY);

        var s = Multiply(value._x, Vector128.Create(1.0f, -1.0f, 1.0f, -1.0f));
        var r = Multiply(vY_WWWZ, p0);

        r = MultiplyAddNegated(r, vY_ZZYY, p1);
        r = MultiplyAdd(r, vY_YXXX, p2);

        return DotProduct(s, r);
    }

    /// <summary>Reinterprets the current instance as a new <see cref="SysMatrix4x4" />.</summary>
    /// <returns>The current instance reinterpreted as a new <see cref="SysMatrix4x4" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SysMatrix4x4 AsSystemMatrix4x4() => As<Matrix4x4, SysMatrix4x4>(ref this);

    /// <inheritdoc />
    public override readonly bool Equals(object? obj) => (obj is Matrix4x4 other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(Matrix4x4 other)
    {
        return _x.Equals(other._x)
            && _y.Equals(other._y)
            && _z.Equals(other._z)
            && _w.Equals(other._w);
    }

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(_x, _y, _z, _w);

    /// <inheritdoc />
    public override readonly string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public readonly string ToString(string? format = null, IFormatProvider? formatProvider = null)
        => $"Matrix4x4 {{ X = {X.ToString(format, formatProvider)}, Y = {Y.ToString(format, formatProvider)}, Z = {Z.ToString(format, formatProvider)}, W = {W.ToString(format, formatProvider)} }}";

    /// <inheritdoc />
    public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"Matrix4x4 { X = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        var partLength = "Matrix4x4 { X = ".Length;

        numWritten += partLength;
        destination = destination.Slice(numWritten);

        if (!X.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!", Y = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = ", Y = ".Length;

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!Y.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!", Z = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = ", Z = ".Length;

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!Z.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!", W = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = ", W = ".Length;

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!W.TryFormat(destination, out partLength, format, provider))
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

        if (!"Matrix4x4 { X = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        var partLength = "Matrix4x4 { X = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(numWritten);

        if (!X.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!", Y = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = ", Y = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!Y.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!", Z = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = ", Z = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!Z.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!", W = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = ", W = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!W.TryFormat(utf8Destination, out partLength, format, provider))
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
