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
using TerraFX.Utilities;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VectorUtilities;
using SysQuaternion = System.Numerics.Quaternion;

namespace TerraFX.Numerics;

/// <summary>Defines a quaternion whch encodes a rotation as an axis-angle.</summary>
public readonly struct Quaternion : IEquatable<Quaternion>, IFormattable
{
    /// <summary>Defines a quaternion where all components are zero.</summary>
    public static Quaternion Zero => new Quaternion(Vector128<float>.Zero);

    /// <summary>Defines the identity quaternion.</summary>
    public static Quaternion Identity => new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

    private readonly Vector128<float> _value;

    /// <summary>Initializes a new instance of the <see cref="Quaternion" /> struct.</summary>
    /// <param name="x">The value of the x-component.</param>
    /// <param name="y">The value of the y-component.</param>
    /// <param name="z">The value of the z-component.</param>
    /// <param name="w">The value of the w-component.</param>
    public Quaternion(float x, float y, float z, float w)
    {
        _value = Vector128.Create(x, y, z, w);
    }

    /// <summary>Initializes a new instance of the <see cref="Quaternion" /> struct.</summary>
    /// <param name="value">The value of the quaternion.</param>
    public Quaternion(SysQuaternion value)
    {
        _value = As<SysQuaternion, Vector128<float>>(ref value);
    }

    /// <summary>Initializes a new instance of the <see cref="Quaternion" /> struct.</summary>
    /// <param name="value">The value of the quaternion.</param>
    public Quaternion(Vector128<float> value)
    {
        _value = value;
    }

    /// <summary>Gets the value of the x-component.</summary>
    public float X
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.ToScalar();
        }
    }

    /// <summary>Gets the value of the y-component.</summary>
    public float Y
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.GetElement(1);
        }
    }

    /// <summary>Gets the value of the z-component.</summary>
    public float Z
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.GetElement(2);
        }
    }

    /// <summary>Gets the value of the w-component.</summary>
    public float W
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.GetElement(3);
        }
    }

    /// <summary>Gets the angle of the quaternion.</summary>
    public float Angle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return 2.0f * MathUtilities.Acos(W);
        }
    }

    /// <summary>Gets the axis of the quaternion.</summary>
    public Vector3 Axis
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return new Vector3(_value);
        }
    }

    /// <summary>Gets the length of the quaternion.</summary>
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var result = Length(_value);
            return result.ToScalar();
        }
    }

    /// <summary>Gets the squared length of the quaternion.</summary>
    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var result = LengthSquared(_value);
            return result.ToScalar();
        }
    }

    /// <summary>Gets an estimate of the reciprocal length of the quaternion.</summary>
    public float ReciprocalLengthEstimate
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var result = ReciprocalLengthEstimate(_value);
            return result.ToScalar();
        }
    }

    /// <summary>Compares two quaternions to determine equality.</summary>
    /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
    /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Quaternion left, Quaternion right) => CompareEqualAll(left._value, right._value);

    /// <summary>Compares two quaternions to determine inequality.</summary>
    /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
    /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Quaternion left, Quaternion right) => CompareNotEqualAny(left._value, right._value);

    /// <summary>Computes the product of two quaternions.</summary>
    /// <param name="left">The quaternion to multiply by <paramref name="right" />.</param>
    /// <param name="right">The quaternion which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion operator *(Quaternion left, Quaternion right)
    {
        if (Sse.IsSupported || AdvSimd.IsSupported)
        {
            var result = MultiplyByW(right._value, left._value);
            result = MultiplyAdd(result, MultiplyByX(CreateFromWZYX(right._value), left._value), Vector128.Create(+1.0f, -1.0f, +1.0f, -1.0f));
            result = MultiplyAdd(result, MultiplyByY(CreateFromZWXY(right._value), left._value), Vector128.Create(+1.0f, +1.0f, -1.0f, -1.0f));
            result = MultiplyAdd(result, MultiplyByZ(CreateFromYXWZ(right._value), left._value), Vector128.Create(-1.0f, +1.0f, +1.0f, -1.0f));
            return new Quaternion(result);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Quaternion SoftwareFallback(Quaternion left, Quaternion right)
        {
            return new Quaternion(
                (left.W * right.X) + (left.X * right.W) + (left.Y * right.Z) - (left.Z * right.Y),
                (left.W * right.Y) - (left.X * right.Z) + (left.Y * right.W) + (left.Z * right.X),
                (left.W * right.Z) + (left.X * right.Y) - (left.Y * right.X) + (left.Z * right.W),
                (left.W * right.W) - (left.X * right.X) - (left.Y * right.Y) - (left.Z * right.Z)
            );
        }
    }

    /// <summary>Computes the concatenation of <paramref name="left" /> and <paramref name="right" />.</summary>
    /// <param name="left">The quaternion onto which <paramref name="right" /> is concatenated.</param>
    /// <param name="right">The quaternion which is concatenated onto <paramref name="left" />.</param>
    /// <returns>The concatenation of <paramref name="right" /> onto <paramref name="left" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion Concatenate(Quaternion left, Quaternion right) => right * left;

    /// <summary>Computes the conjugate of a quaternion.</summary>
    /// <param name="value">The quaternion for which to get its conjugate.</param>
    /// <returns>The conjugate of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion Conjugate(Quaternion value)
    {
        var result = QuaternionConjugate(value._value);
        return new Quaternion(result);
    }

    /// <summary>Creates a quaternion from an axis and angle.</summary>
    /// <param name="axis">The axis of the quaternion.</param>
    /// <param name="angle">The angle, in radians, of the quaternion.</param>
    /// <returns>A quaternion that represents <paramref name="axis" /> and <paramref name="angle" />.</returns>
    public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
    {
        Assert(AssertionsEnabled && (axis != Vector3.Zero));
        Assert(AssertionsEnabled && !Vector3.IsAnyInfinity(axis));

        var normalizedAxis = Vector3.Normalize(axis);
        return CreateFromNormalizedAxisAngle(normalizedAxis, angle);
    }

    /// <summary>Creates a quaternion from a matrix.</summary>
    /// <param name="matrix">The matrix from which to create the quaternion.</param>
    /// <returns>A quaternion that represents <paramref name="matrix" />.</returns>
    public static Quaternion CreateFromMatrix(Matrix4x4 matrix)
    {
        var r0 = matrix.X.AsVector128();
        var r1 = matrix.Y.AsVector128();
        var r2 = matrix.Z.AsVector128();

        var r00 = CreateFromX(r0);
        var r11 = CreateFromY(r1);
        var r22 = CreateFromZ(r2);

        // (4*x^2, 4*y^2, 4*z^2, 4*w^2)
        var x2y2z2w2 = Vector128.Create(1.0f, 1.0f, 1.0f, 1.0f);
        x2y2z2w2 = MultiplyAdd(x2y2z2w2, Vector128.Create(+1.0f, -1.0f, -1.0f, +1.0f), r00);
        x2y2z2w2 = MultiplyAdd(x2y2z2w2, Vector128.Create(-1.0f, +1.0f, -1.0f, +1.0f), r11);
        x2y2z2w2 = MultiplyAdd(x2y2z2w2, Vector128.Create(-1.0f, -1.0f, +1.0f, +1.0f), r22);

        // (4*x*y, 4*x*z, 4*y*z, unused)
        var xyxzyz = Add(CreateFromYZZY(r0, r1), CreateFromXZWY(CreateFromXXXY(r1, r2)));

        // (4*x*w, 4*y*w, 4*z*w, unused)
        var xwywzw = Subtract(CreateFromYXXX(r2, r1), CreateFromXZWY(CreateFromZZZY(r1, r0)));
        xwywzw = Multiply(xwywzw, Vector128.Create(-1.0f, +1.0f, -1.0f, +1.0f));

        // (4*x^2, 4*y^2, 4*x*y, unused)
        // (4*z^2, 4*w^2, 4*z*w, unused)
        // (4*x*z, 4*y*z, 4*x*w, 4*y*w)

        var t0 = CreateFromXYXX(x2y2z2w2, xyxzyz);
        var t1 = CreateFromZWZX(x2y2z2w2, xwywzw);
        var t2 = CreateFromYZXY(xyxzyz, xwywzw);

        // (4*x*x, 4*x*y, 4*x*z, 4*x*w)
        // (4*y*x, 4*y*y, 4*y*z, 4*y*w)
        // (4*z*x, 4*z*y, 4*z*z, 4*z*w)
        // (4*w*x, 4*w*y, 4*w*z, 4*w*w)
        var tensor0 = CreateFromXZXZ(t0, t2);
        var tensor1 = CreateFromZYYW(t0, t2);
        var tensor2 = CreateFromXYXZ(t2, t1);
        var tensor3 = CreateFromZWZY(t2, t1);

        // Select the row of the tensor-product matrix that has the largest magnitude.

        // x^2 >= y^2 equivalent to r11 - r00 <= 0
        var x2gey2 = CompareLessThanOrEqual(Subtract(r11, r00), Vector128<float>.Zero);
        t0 = ElementwiseSelect(x2gey2, tensor0, tensor1);

        // z^2 >= w^2 equivalent to r11 + r00 <= 0
        var z2gew2 = CompareLessThanOrEqual(Add(r11, r00), Vector128<float>.Zero);
        t1 = ElementwiseSelect(z2gew2, tensor2, tensor3);

        // x^2 + y^2 >= z^2 + w^2 equivalent to r22 <= 0
        var x2py2gez2pw2 = CompareLessThanOrEqual(r22, Vector128<float>.Zero);
        t2 = ElementwiseSelect(x2py2gez2pw2, t0, t1);

        var result = VectorUtilities.Normalize(t2);
        return new Quaternion(result);
    }

    /// <summary>Creates a quaternion from an axis and angle.</summary>
    /// <param name="normalizedAxis">The normalized axis of the quaternion.</param>
    /// <param name="angle">The angle, in radians, of the quaternion.</param>
    /// <returns>A quaternion that represents <paramref name="normalizedAxis" /> and <paramref name="angle" />.</returns>
    public static Quaternion CreateFromNormalizedAxisAngle(Vector3 normalizedAxis, float angle)
    {
        (var sin, var cos) = MathUtilities.SinCos(angle * 0.5f);

        if (Sse41.IsSupported || AdvSimd.IsSupported)
        {
            var result = Multiply(normalizedAxis.AsVector128(), Vector128.Create(sin));
            result = result.WithElement(3, cos);
            return new Quaternion(result);
        }
        else
        {
            return new Quaternion(
                sin * normalizedAxis.X,
                sin * normalizedAxis.Y,
                sin * normalizedAxis.Z,
                cos
            );
        }
    }

    /// <summary>Computes the dot product of two quaternions.</summary>
    /// <param name="left">The quaternion to multiply by <paramref name="right" />.</param>
    /// <param name="right">The quatnerion which is used to multiply <paramref name="left" />.</param>
    /// <returns>The dot product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DotProduct(Quaternion left, Quaternion right)
    {
        var result = VectorUtilities.DotProduct(left._value, right._value);
        return result.ToScalar();
    }

    /// <summary>Compares two quaternions to determine if all elements are equal.</summary>
    /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
    /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if all elements of <paramref name="left" /> are equal to the corresponding element of <paramref name="right" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(Quaternion left, Quaternion right) => left == right;

    /// <summary>Compares two quaternions to determine if all elements are approximately equal.</summary>
    /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
    /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(Quaternion left, Quaternion right, Quaternion epsilon) => CompareEqualAll(left._value, right._value, epsilon._value);

    /// <summary>Computes the inverse of a quaternion.</summary>
    /// <param name="value">The quaternion to invert.</param>
    /// <returns>The inverse of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion Inverse(Quaternion value)
    {
        if (Sse41.IsSupported || AdvSimd.IsSupported)
        {
            var lengthSq = LengthSquared(value._value);
            var conjugate = QuaternionConjugate(value._value);
            var condition = CompareLessThanOrEqual(lengthSq, Vector128.Create(NearZeroEpsilon));

            var result = Divide(conjugate, lengthSq);
            result = ElementwiseSelect(condition, Vector128<float>.Zero, result);
            return new Quaternion(result);
        }
        else
        {
            var lengthSq = value.LengthSquared;

            if (lengthSq <= NearZeroEpsilon)
            {
                return Zero;
            }

            return new Quaternion(
                value.X / lengthSq,
                value.Y / lengthSq,
                value.Z / lengthSq,
                value.W / lengthSq
            );
        }
    }

    /// <summary>Determines if any elements in a quaternion are either <see cref="float.PositiveInfinity" /> or <see cref="float.NegativeInfinity" />.</summary>
    /// <param name="value">The quaternion to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are either <see cref="float.PositiveInfinity" /> or <see cref="float.NegativeInfinity" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAnyInfinity(Quaternion value) => VectorUtilities.IsAnyInfinity(value._value);

    /// <summary>Determines if any elements in a quaternion are <see cref="float.NaN" />.</summary>
    /// <param name="value">The quaternion to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are <see cref="float.NaN" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAnyNaN(Quaternion value) => VectorUtilities.IsAnyNaN(value._value);

    /// <summary>Determines if a quaternion is equal to <see cref="Identity" />.</summary>
    /// <param name="value">The quaternion to compare against <see cref="Identity" />.</param>
    /// <returns><c>true</c> if <paramref name="value" /> and <see cref="Identity" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsIdentity(Quaternion value) => value == Identity;

    /// <summary>Computes the normalized form of a quaternion.</summary>
    /// <param name="value">The quaternion to normalize.</param>
    /// <returns>The normalized form of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion Normalize(Quaternion value)
    {
        var result = VectorUtilities.Normalize(value._value);
        return new Quaternion(result);
    }

    /// <summary>Computes an estimate of the normalized form of a quaternion.</summary>
    /// <param name="value">The quaternion to normalize.</param>
    /// <returns>An estimate of the normalized form of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion NormalizeEstimate(Quaternion value)
    {
        var result = VectorUtilities.NormalizeEstimate(value._value);
        return new Quaternion(result);
    }

    /// <summary>Reinterprets the current instance as a new <see cref="SysQuaternion" />.</summary>
    /// <returns>The current instance reintepreted as a new <see cref="SysQuaternion" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SysQuaternion AsQuaternion() => AsReadonly<Vector128<float>, SysQuaternion>(in _value);

    /// <summary>Reinterprets the current instance as a new <see cref="Vector128{Single}" />.</summary>
    /// <returns>The current instance reintepreted as a new <see cref="Vector128{Single}" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector128<float> AsVector128() => _value;

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Quaternion other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(Quaternion other) => _value.Equals(other._value);

    /// <inheritdoc />
    public override int GetHashCode() => _value.GetHashCode();

    /// <inheritdoc />
    public override string ToString() => _value.ToString();

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

    /// <summary>Creates a new <see cref="Quaternion" /> instance with <see cref="X" /> set to the specified value.</summary>
    /// <param name="x">The new value of the x-component.</param>
    /// <returns>A new <see cref="Quaternion" /> instance with <see cref="X" /> set to <paramref name="x" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithX(float x)
    {
        var result = _value.WithElement(0, x);
        return new Quaternion(result);
    }

    /// <summary>Creates a new <see cref="Quaternion" /> instance with <see cref="Y" /> set to the specified value.</summary>
    /// <param name="y">The new value of the y-component.</param>
    /// <returns>A new <see cref="Quaternion" /> instance with <see cref="Y" /> set to <paramref name="y" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithY(float y)
    {
        var result = _value.WithElement(1, y);
        return new Quaternion(result);
    }

    /// <summary>Creates a new <see cref="Quaternion" /> instance with <see cref="Z" /> set to the specified value.</summary>
    /// <param name="z">The new value of the z-component.</param>
    /// <returns>A new <see cref="Quaternion" /> instance with <see cref="Z" /> set to <paramref name="z" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithZ(float z)
    {
        var result = _value.WithElement(2, z);
        return new Quaternion(result);
    }

    /// <summary>Creates a new <see cref="Quaternion" /> instance with <see cref="W" /> set to the specified value.</summary>
    /// <param name="w">The new value of the w-component.</param>
    /// <returns>A new <see cref="Quaternion" /> instance with <see cref="W" /> set to <paramref name="w" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithW(float w)
    {
        var result = _value.WithElement(3, w);
        return new Quaternion(result);
    }
}
