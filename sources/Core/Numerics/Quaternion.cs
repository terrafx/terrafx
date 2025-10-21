// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using TerraFX.Utilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VectorUtilities;
using SysQuaternion = System.Numerics.Quaternion;
using SysVector4 = System.Numerics.Vector4;

namespace TerraFX.Numerics;

/// <summary>Defines a quaternion which encodes a rotation as an axis-angle.</summary>
public readonly struct Quaternion
    : IEquatable<Quaternion>,
      IFormattable,
      ISpanFormattable,
      IUtf8SpanFormattable
{
    /// <summary>Defines a quaternion where all components are zero.</summary>
    public static Quaternion Zero => Create(0.0f, 0.0f, 0.0f, 0.0f);

    /// <summary>Defines the identity quaternion.</summary>
    public static Quaternion Identity => Create(0.0f, 0.0f, 0.0f, 1.0f);

    private readonly Vector128<float> _value;

    /// <summary>Creates a quaternion from a system quaternion.</summary>
    /// <param name="value">The value of the quaternion.</param>
    public static Quaternion Create(SysQuaternion value) => As<SysQuaternion, Quaternion>(ref value);

    /// <summary>Creates a quaternion from a system vector.</summary>
    /// <param name="value">The value of the quaternion.</param>
    public static Quaternion Create(SysVector4 value) => new Quaternion(value.AsVector128());

    /// <summary>Creates a quaternion from a vector.</summary>
    /// <param name="value">The value of the quaternion.</param>
    public static Quaternion Create(Vector4 value) => new Quaternion(value.Value);

    /// <summary>Creates a quaternion from a hardware vector.</summary>
    /// <param name="value">The value of the quaternion.</param>
    public static Quaternion Create(Vector128<float> value) => new Quaternion(value);

    /// <summary>Creates a quaternion from an X, Y, Z, and W component.</summary>
    /// <param name="x">The value of the x-component.</param>
    /// <param name="y">The value of the y-component.</param>
    /// <param name="z">The value of the z-component.</param>
    /// <param name="w">The value of the w-component.</param>
    public static Quaternion Create(float x, float y, float z, float w) => Create(Vector128.Create(x, y, z, w));

    private Quaternion(Vector128<float> value)
    {
        _value = value;
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
            return Vector3.Create(_value);
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

    /// <summary>Gets the value of the quaternion.</summary>
    public Vector128<float> Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value;
        }
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
            return _value.GetY();
        }
    }

    /// <summary>Gets the value of the z-component.</summary>
    public float Z
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.GetZ();
        }
    }

    /// <summary>Gets the value of the w-component.</summary>
    public float W
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.GetW();
        }
    }

    /// <summary>Compares two quaternions to determine equality.</summary>
    /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
    /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Quaternion left, Quaternion right) => Vector128.EqualsAll(left._value, right._value);

    /// <summary>Compares two quaternions to determine inequality.</summary>
    /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
    /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Quaternion left, Quaternion right) => !Vector128.EqualsAll(left._value, right._value);

    /// <summary>Computes the product of two quaternions.</summary>
    /// <param name="left">The quaternion to multiply by <paramref name="right" />.</param>
    /// <param name="right">The quaternion which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion operator *(Quaternion left, Quaternion right)
    {
        var result = MultiplyByW(right._value, left._value);
        result = MultiplyAdd(result, MultiplyByX(CreateFromWZYX(right._value), left._value), Vector128.Create(+1.0f, -1.0f, +1.0f, -1.0f));
        result = MultiplyAdd(result, MultiplyByY(CreateFromZWXY(right._value), left._value), Vector128.Create(+1.0f, +1.0f, -1.0f, -1.0f));
        result = MultiplyAdd(result, MultiplyByZ(CreateFromYXWZ(right._value), left._value), Vector128.Create(-1.0f, +1.0f, +1.0f, -1.0f));
        return new Quaternion(result);
    }

    /// <summary>Compares two quaternions to determine if all elements are equal.</summary>
    /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
    /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if all elements of <paramref name="left" /> are equal to the corresponding element of <paramref name="right" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Quaternion left, Quaternion right) => left == right;

    /// <summary>Compares two quaternions to determine if all elements are approximately equal.</summary>
    /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
    /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Quaternion left, Quaternion right, Quaternion epsilon) => VectorUtilities.CompareEqualAll(left._value, right._value, epsilon._value);

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
        Assert(axis != Vector3.Zero);
        Assert(!Vector3.IsAnyInfinity(axis));

        var normalizedAxis = Vector3.Normalize(axis);
        return CreateFromNormalizedAxisAngle(normalizedAxis, angle);
    }

    /// <summary>Creates a quaternion from a matrix.</summary>
    /// <param name="matrix">The matrix from which to create the quaternion.</param>
    /// <returns>A quaternion that represents <paramref name="matrix" />.</returns>
    public static Quaternion CreateFromMatrix(Matrix4x4 matrix)
    {
        var r0 = matrix.X.Value;
        var r1 = matrix.Y.Value;
        var r2 = matrix.Z.Value;

        var r00 = CreateFromX(r0);
        var r11 = CreateFromY(r1);
        var r22 = CreateFromZ(r2);

        // (4*x^2, 4*y^2, 4*z^2, 4*w^2)
        var x2y2z2w2 = Vector128.Create(1.0f, 1.0f, 1.0f, 1.0f);
        x2y2z2w2 = MultiplyAdd(x2y2z2w2, Vector128.Create(+1.0f, -1.0f, -1.0f, +1.0f), r00);
        x2y2z2w2 = MultiplyAdd(x2y2z2w2, Vector128.Create(-1.0f, +1.0f, -1.0f, +1.0f), r11);
        x2y2z2w2 = MultiplyAdd(x2y2z2w2, Vector128.Create(-1.0f, -1.0f, +1.0f, +1.0f), r22);

        // (4*x*y, 4*x*z, 4*y*z, unused)
        var xyxzyz = CreateFromYZCB(r0, r1) + CreateFromXZWY(CreateFromXXAB(r1, r2));

        // (4*x*w, 4*y*w, 4*z*w, unused)
        var xwywzw = CreateFromYXAA(r2, r1) - CreateFromXZWY(CreateFromZZCB(r1, r0));
        xwywzw *= Vector128.Create(-1.0f, +1.0f, -1.0f, +1.0f);

        // (4*x^2, 4*y^2, 4*x*y, unused)
        // (4*z^2, 4*w^2, 4*z*w, unused)
        // (4*x*z, 4*y*z, 4*x*w, 4*y*w)

        var t0 = CreateFromXYAA(x2y2z2w2, xyxzyz);
        var t1 = CreateFromZWCA(x2y2z2w2, xwywzw);
        var t2 = CreateFromYZAB(xyxzyz, xwywzw);

        // (4*x*x, 4*x*y, 4*x*z, 4*x*w)
        // (4*y*x, 4*y*y, 4*y*z, 4*y*w)
        // (4*z*x, 4*z*y, 4*z*z, 4*z*w)
        // (4*w*x, 4*w*y, 4*w*z, 4*w*w)
        var tensor0 = CreateFromXZAC(t0, t2);
        var tensor1 = CreateFromZYBD(t0, t2);
        var tensor2 = CreateFromXYAC(t2, t1);
        var tensor3 = CreateFromZWCB(t2, t1);

        // Select the row of the tensor-product matrix that has the largest magnitude.

        // x^2 >= y^2 equivalent to r11 - r00 <= 0
        var x2gey2 = Vector128.LessThanOrEqual(r11 - r00, Vector128<float>.Zero);
        t0 = ElementwiseSelect(x2gey2, tensor0, tensor1);

        // z^2 >= w^2 equivalent to r11 + r00 <= 0
        var z2gew2 = Vector128.LessThanOrEqual(r11 + r00, Vector128<float>.Zero);
        t1 = ElementwiseSelect(z2gew2, tensor2, tensor3);

        // x^2 + y^2 >= z^2 + w^2 equivalent to r22 <= 0
        var x2py2gez2pw2 = Vector128.LessThanOrEqual(r22, Vector128<float>.Zero);
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
        var result = normalizedAxis.AsVector128() * Vector128.Create(sin);

        result = result.WithW(cos);
        return new Quaternion(result);
    }

    /// <summary>Creates a quaternion from a specified pitch, yaw, and roll.</summary>
    /// <param name="pitchYawRoll">The pitch, yaw, and roll that should be used to create the quaternion.</param>
    /// <returns>A quaternion that represents <paramref name="pitchYawRoll" />.</returns>
    public static Quaternion CreateFromPitchYawRoll(Vector3 pitchYawRoll)
    {
        var (sin, cos) = SinCos(pitchYawRoll.AsVector128() * 0.5f);

        var p0 = CreateFromXAAA(sin, cos);
        var y0 = CreateFromBYBB(sin, cos);
        var r0 = CreateFromCCZC(sin, cos);

        var p1 = CreateFromXAAA(cos, sin);
        var y1 = CreateFromBYBB(cos, sin);
        var r1 = CreateFromCCZC(cos, sin);

        var q0 = p0 * y0;
        var q1 = p1 * Vector128.Create(1.0f, -1.0f, -1.0f, 1.0f);

        q0 *= r0;
        q1 *= y1;

        var result = MultiplyAdd(q0, q1, r1);
        return new Quaternion(result);
    }

    /// <summary>Computes the dot product of two quaternions.</summary>
    /// <param name="left">The quaternion to multiply by <paramref name="right" />.</param>
    /// <param name="right">The quaternion which is used to multiply <paramref name="left" />.</param>
    /// <returns>The dot product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DotProduct(Quaternion left, Quaternion right)
    {
        return Vector128.Dot(left._value, right._value);
    }

    /// <summary>Computes the inverse of a quaternion.</summary>
    /// <param name="value">The quaternion to invert.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between the <c>LengthSquared</c> and zero for which they should be considered equivalent.</param>
    /// <returns>The inverse of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion Inverse(Quaternion value, Vector4 epsilon)
    {
        var lengthSq = LengthSquared(value._value);
        var conjugate = QuaternionConjugate(value._value);
        var condition = Vector128.LessThanOrEqual(lengthSq, epsilon.Value);

        var result = conjugate / lengthSq;
        result = ElementwiseSelect(condition, Vector128<float>.Zero, result);
        return new Quaternion(result);
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
    /// <returns>The current instance reinterpreted as a new <see cref="SysQuaternion" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SysQuaternion AsSystemQuaternion() => AsReadonly<Vector128<float>, SysQuaternion>(in _value);

    /// <summary>Reinterprets the current instance as a new <see cref="SysVector4" />.</summary>
    /// <returns>The current instance reinterpreted as a new <see cref="SysVector4" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SysVector4 AsSystemVector4() => AsReadonly<Vector128<float>, SysVector4>(in _value);

    /// <summary>Reinterprets the current instance as a new <see cref="Vector4" />.</summary>
    /// <returns>The current instance reinterpreted as a new <see cref="Vector4" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SysVector4 AsVector4() => _value.AsVector4();

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Quaternion other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(Quaternion other)
    {
        return X.Equals(other.X)
            && Y.Equals(other.Y)
            && Z.Equals(other.Z)
            && W.Equals(other.W);
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format = null, IFormatProvider? formatProvider = null)
        => $"Quaternion {{ Axis = {Axis.ToString(format, formatProvider)}, Angle = {Angle.ToString(format, formatProvider)} }}";

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"Quaternion { Axis = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        var partLength = "Quaternion { Axis = ".Length;

        numWritten += partLength;
        destination = destination.Slice(numWritten);

        if (!Axis.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!", Angle = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = ", Angle = ".Length;

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!Angle.TryFormat(destination, out partLength, format, provider))
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

        if (!"Quaternion { Axis = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        var partLength = "Quaternion { Axis = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(numWritten);

        if (!Axis.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!", Angle = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = ", Angle = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!Angle.TryFormat(utf8Destination, out partLength, format, provider))
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

    /// <summary>Creates a new quaternion with <see cref="Angle" /> set to the specified value.</summary>
    /// <param name="angle">The new angle of the quaternion.</param>
    /// <returns>A new quaternion with <see cref="Angle" /> set to <paramref name="angle" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithAngle(float angle) => CreateFromAxisAngle(Axis, angle);

    /// <summary>Creates a new quaternion with <see cref="Axis" /> set to the specified value.</summary>
    /// <param name="axis">The new axis of the quaternion.</param>
    /// <returns>A new quaternion with <see cref="Axis" /> set to <paramref name="axis" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithAxis(Vector3 axis) => CreateFromAxisAngle(axis, Angle);

    /// <summary>Creates a new quaternion with <see cref="X" /> set to the specified value.</summary>
    /// <param name="x">The new x-component of the quaternion.</param>
    /// <returns>A new quaternion with <see cref="X" /> set to <paramref name="x" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithX(float x)
    {
        var result = _value.WithX(x);
        return new Quaternion(result);
    }

    /// <summary>Creates a new quaternion with <see cref="Y" /> set to the specified value.</summary>
    /// <param name="y">The new value of the y-component.</param>
    /// <returns>A new quaternion with <see cref="Y" /> set to <paramref name="y" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithY(float y)
    {
        var result = _value.WithY(y);
        return new Quaternion(result);
    }

    /// <summary>Creates a new quaternion with <see cref="Z" /> set to the specified value.</summary>
    /// <param name="z">The new value of the z-component.</param>
    /// <returns>A new quaternion with <see cref="Z" /> set to <paramref name="z" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithZ(float z)
    {
        var result = _value.WithZ(z);
        return new Quaternion(result);
    }

    /// <summary>Creates a new quaternion with <see cref="W" /> set to the specified value.</summary>
    /// <param name="w">The new value of the w-component.</param>
    /// <returns>A new quaternion with <see cref="W" /> set to <paramref name="w" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithW(float w)
    {
        var result = _value.WithW(w);
        return new Quaternion(result);
    }
}
