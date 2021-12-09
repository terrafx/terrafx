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
using static TerraFX.Utilities.VectorUtilities;
using SysVector3 = System.Numerics.Vector3;

namespace TerraFX.Numerics;

/// <summary>Defines a three-dimensional Euclidean vector.</summary>
public readonly struct Vector3 : IEquatable<Vector3>, IFormattable
{
    /// <summary>Defines a <see cref="Vector3" /> where all components are zero.</summary>
    public static Vector3 Zero => new Vector3(SysVector3.Zero);

    /// <summary>Defines a <see cref="Vector3" /> whose x-component is one and whose remaining components are zero.</summary>
    public static Vector3 UnitX => new Vector3(SysVector3.UnitX);

    /// <summary>Defines a <see cref="Vector3" /> whose y-component is one and whose remaining components are zero.</summary>
    public static Vector3 UnitY = new Vector3(SysVector3.UnitY);

    /// <summary>Defines a <see cref="Vector3" /> whose z-component is one and whose remaining components are zero.</summary>
    public static Vector3 UnitZ = new Vector3(SysVector3.UnitZ);

    /// <summary>Defines a <see cref="Vector3" /> where all components are one.</summary>
    public static Vector3 One = new Vector3(SysVector3.One);

    private readonly SysVector3 _value;

    /// <summary>Initializes a new instance of the <see cref="Vector3" /> struct.</summary>
    /// <param name="x">The value of the x-dimension.</param>
    /// <param name="y">The value of the y-dimension.</param>
    /// <param name="z">The value of the z-dimension.</param>
    public Vector3(float x, float y, float z)
    {
        _value = new SysVector3(x, y, z);
    }

    /// <summary>Initializes a new instance of the <see cref="Vector3" /> struct with each component set to <paramref name="value" />.</summary>
    /// <param name="value">The value to set each component to.</param>
    public Vector3(float value)
    {
        _value = new SysVector3(value);
    }

    /// <summary>Initializes a new instance of the <see cref="Vector3" /> struct.</summary>
    /// <param name="vector">The value of the x and y dimensions.</param>
    /// <param name="z">The value of the z-dimension.</param>
    public Vector3(Vector2 vector, float z)
    {
        _value = new SysVector3(vector.AsVector2(), z);
    }

    /// <summary>Initializes a new instance of the <see cref="Vector3" /> struct.</summary>
    /// <param name="value">The value of the vector.</param>
    public Vector3(SysVector3 value)
    {
        _value = value;
    }

    /// <summary>Initializes a new instance of the <see cref="Vector3" /> struct.</summary>
    /// <param name="value">The value of the vector.</param>
    public Vector3(Vector128<float> value)
    {
        _value = value.AsVector3();
    }

    /// <summary>Gets the value of the x-dimension.</summary>
    public float X
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.X;
        }
    }

    /// <summary>Gets the value of the y-dimension.</summary>
    public float Y
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.Y;
        }
    }

    /// <summary>Gets the value of the z-dimension.</summary>
    public float Z
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.Z;
        }
    }

    /// <summary>Gets the square-rooted length of the vector.</summary>
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.Length();
        }
    }

    /// <summary>Gets the squared length of the vector.</summary>
    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.LengthSquared();
        }
    }

    /// <summary>Gets an estimate of the reciprocal length of the vector.</summary>
    public float ReciprocalLengthEstimate
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var result = ReciprocalLengthEstimate(_value.AsVector128());
            return result.ToScalar();
        }
    }

    /// <summary>Compares two vectors to determine equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector3 left, Vector3 right) => left._value == right._value;

    /// <summary>Compares two vectors to determine inequality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector3 left, Vector3 right) => left._value != right._value;

    /// <summary>Computes the value of a vector.</summary>
    /// <param name="value">The vector.</param>
    /// <returns><paramref name="value" /></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 value) => value;

    /// <summary>Computes the negation of a vector.</summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negation of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 value) => new Vector3(-value._value);

    /// <summary>Computes the sum of two vectors.</summary>
    /// <param name="left">The vector to which to add <paramref name="right" />.</param>
    /// <param name="right">The vector which is added to <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="right" /> added to <paramref name="left" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 left, Vector3 right) => new Vector3(left._value + right._value);

    /// <summary>Computes the difference of two vectors.</summary>
    /// <param name="left">The vector from which to subtract <paramref name="right" />.</param>
    /// <param name="right">The vector which is subtracted from <paramref name="left" />.</param>
    /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 left, Vector3 right) => new Vector3(left._value - right._value);

    /// <summary>Computes the product of a vector and a float.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The float which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 left, float right) => new Vector3(left._value * right);

    /// <summary>Computes the product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 left, Vector3 right) => new Vector3(left._value * right._value);

    /// <summary>Computes the product of a vector and matrix.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The matrix which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 left, Matrix4x4 right) => Transform(left, right);

    /// <summary>Computes the quotient of a vector and a float.</summary>
    /// <param name="left">The vector which is divied by <paramref name="right" />.</param>
    /// <param name="right">The float which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 left, float right) => new Vector3(left._value / right);

    /// <summary>Computes the quotient of two vectors.</summary>
    /// <param name="left">The vector which is divied by <paramref name="right" />.</param>
    /// <param name="right">The vector which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 left, Vector3 right) => new Vector3(left._value / right._value);

    /// <summary>Computes the cross product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The quatnerion which is used to multiply <paramref name="left" />.</param>
    /// <returns>The cross product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 CrossProduct(Vector3 left, Vector3 right)
    {
        var result = SysVector3.Cross(left._value, right._value);
        return new Vector3(result);
    }

    /// <summary>Computes the dot product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The quatnerion which is used to multiply <paramref name="left" />.</param>
    /// <returns>The dot product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DotProduct(Vector3 left, Vector3 right) => SysVector3.Dot(left._value, right._value);

    /// <summary>Compares two vectors to determine element-wise equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>A vector that contains the element-wise comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Equals(Vector3 left, Vector3 right)
    {
        if (Sse.IsSupported || AdvSimd.IsSupported)
        {
            var result = CompareEqual(left._value.AsVector128(), right._value.AsVector128());
            return new Vector3(result);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector3 SoftwareFallback(Vector3 left, Vector3 right)
        {
            return new Vector3(
                (left.X == right.X) ? AllBitsSet : 0.0f,
                (left.Y == right.Y) ? AllBitsSet : 0.0f,
                (left.Z == right.Z) ? AllBitsSet : 0.0f
            );
        }
    }

    /// <summary>Compares two vectors to determine approximate equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (exclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Equals(Vector3 left, Vector3 right, Vector3 epsilon)
    {
        if (Sse.IsSupported || AdvSimd.IsSupported)
        {
            var result = CompareEqual(left._value.AsVector128(), right._value.AsVector128(), epsilon._value.AsVector128());
            return new Vector3(result);
        }
        else
        {
            return SoftwareFallback(left, right, epsilon);
        }

        static Vector3 SoftwareFallback(Vector3 left, Vector3 right, Vector3 epsilon)
        {
            return new Vector3(
                MathUtilities.Equals(left.X, right.X, epsilon.X) ? AllBitsSet : 0.0f,
                MathUtilities.Equals(left.Y, right.Y, epsilon.Y) ? AllBitsSet : 0.0f,
                MathUtilities.Equals(left.Z, right.Z, epsilon.Z) ? AllBitsSet : 0.0f
            );
        }
    }

    /// <summary>Compares two vectors to determine if all elements are equal.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if all elements of <paramref name="left" /> are equal to the corresponding element of <paramref name="right" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(Vector3 left, Vector3 right) => left == right;

    /// <summary>Compares two vectors to determine if all elements are approximately equal.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">he maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(Vector3 left, Vector3 right, Vector3 epsilon)
    {
        if (Sse.IsSupported || AdvSimd.Arm64.IsSupported)
        {
            return CompareEqualAll(left._value.AsVector128(), right._value.AsVector128(), epsilon._value.AsVector128());
        }
        else
        {
            return SoftwareFallback(left, right, epsilon);
        }

        static bool SoftwareFallback(Vector3 left, Vector3 right, Vector3 epsilon)
        {
            return MathUtilities.Equals(left.X, right.X, epsilon.X)
                && MathUtilities.Equals(left.Y, right.Y, epsilon.Y)
                && MathUtilities.Equals(left.Z, right.Z, epsilon.Z);
        }
    }

    /// <summary>Determines if any elements in a vector are either <see cref="float.PositiveInfinity" /> or <see cref="float.NegativeInfinity" />.</summary>
    /// <param name="value">The vector to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are either <see cref="float.PositiveInfinity" /> or <see cref="float.NegativeInfinity" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAnyInfinity(Vector3 value) => VectorUtilities.IsAnyInfinity(value._value.AsVector128());

    /// <summary>Determines if any elements in a quaternion are <see cref="float.NaN" />.</summary>
    /// <param name="value">The vector to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are <see cref="float.NaN" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAnyNaN(Vector3 value) => VectorUtilities.IsAnyNaN(value._value.AsVector128());

    /// <summary>Compares two vectors to determine the combined maximum.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>The combined maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Max(Vector3 left, Vector3 right)
    {
        if (Sse41.IsSupported || AdvSimd.IsSupported)
        {
            var result = VectorUtilities.Max(left._value.AsVector128(), right._value.AsVector128());
            return new Vector3(result);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector3 SoftwareFallback(Vector3 left, Vector3 right)
        {
            return new Vector3(
                MathUtilities.Max(left.X, right.X),
                MathUtilities.Max(left.Y, right.Y),
                MathUtilities.Max(left.Z, right.Z)
            );
        }
    }

    /// <summary>Compares two vectors to determine the combined minimum.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>The combined minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Min(Vector3 left, Vector3 right)
    {
        if (Sse41.IsSupported || AdvSimd.IsSupported)
        {
            var result = VectorUtilities.Min(left._value.AsVector128(), right._value.AsVector128());
            return new Vector3(result);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector3 SoftwareFallback(Vector3 left, Vector3 right)
        {
            return new Vector3(
                MathUtilities.Min(left.X, right.X),
                MathUtilities.Min(left.Y, right.Y),
                MathUtilities.Min(left.Z, right.Z)
            );
        }
    }

    /// <summary>Computes the normalized form of a vector.</summary>
    /// <param name="value">The vector to normalized.</param>
    /// <returns>The normalized form of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Normalize(Vector3 value)
    {
        var result = SysVector3.Normalize(value._value);
        return new Vector3(result);
    }

    /// <summary>Computes an estimate of the normalized form of a vector.</summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>An estimate of the normalized form of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 NormalizeEstimate(Vector3 value)
    {
        var result = VectorUtilities.NormalizeEstimate(value._value.AsVector128());
        return new Vector3(result);
    }

    /// <summary>Computes an estimate of the reciprocal of a vector.</summary>
    /// <param name="value">The vector for which to compute the reciprocal.</param>
    /// <returns>An estimate of the element-wise reciprocal of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ReciprocalEstimate(Vector3 value)
    {
        var result = VectorUtilities.ReciprocalEstimate(value._value.AsVector128());
        return new Vector3(result);
    }

    /// <summary>Computes an estimate of the reciprocal square-root of a vector.</summary>
    /// <param name="value">The vector for which to compute the reciprocal square-root.</param>
    /// <returns>An estimate of the element-wise reciprocal square-root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ReciprocalSqrtEstimate(Vector3 value)
    {
        var result = VectorUtilities.ReciprocalSqrtEstimate(value._value.AsVector128());
        return new Vector3(result);
    }

    /// <summary>Rotates a vector using a quaternion.</summary>
    /// <param name="value">The vector to rotate.</param>
    /// <param name="rotation">The rotation.</param>
    /// <returns><paramref name="value" /> rotated by <paramref name="rotation" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Rotate(Vector3 value, Quaternion rotation)
    {
        var vValue = new Quaternion(value._value.AsVector128());
        var conjugate = Quaternion.Conjugate(rotation);

        var result = Quaternion.Concatenate(conjugate, vValue);
        result = Quaternion.Concatenate(result, rotation);

        return new Vector3(result.AsVector128());
    }

    /// <summary>Rotates a vector using the inverse of a quaternion.</summary>
    /// <param name="value">The vector to rotate.</param>
    /// <param name="rotation">The rotation.</param>
    /// <returns><paramref name="value" /> rotated by the inverse of <paramref name="rotation" />.</returns>
    public static Vector3 RotateInverse(Vector3 value, Quaternion rotation)
    {
        var vValue = new Quaternion(value._value.AsVector128());
        var conjugate = Quaternion.Conjugate(rotation);

        var result = Quaternion.Concatenate(rotation, vValue);
        result = Quaternion.Concatenate(result, conjugate);

        return new Vector3(result.AsVector128());
    }

    /// <summary>Transforms a vector using a matrix.</summary>
    /// <param name="value">The vector to transform.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns><paramref name="value" /> transformed by <paramref name="matrix" />.</returns>
    public static Vector3 Transform(Vector3 value, Matrix4x4 matrix)
    {
        var vValue = value._value.AsVector128();

        var result = matrix.W.AsVector128();
        result = MultiplyAddByX(result, matrix.X.AsVector128(), vValue);
        result = MultiplyAddByY(result, matrix.Y.AsVector128(), vValue);
        result = MultiplyAddByZ(result, matrix.Z.AsVector128(), vValue);
        return new Vector3(result);
    }

    /// <summary>Transforms a vector using a normalized matrix.</summary>
    /// <param name="value">The vector to transform.</param>
    /// <param name="matrix">The normalized transformation matrix.</param>
    /// <returns><paramref name="value" /> transformed by <paramref name="matrix" />.</returns>
    public static Vector3 TransformNormal(Vector3 value, Matrix4x4 matrix)
    {
        var vValue = value._value.AsVector128();

        var result = MultiplyByX(matrix.X.AsVector128(), vValue);
        result = MultiplyAddByY(result, matrix.Y.AsVector128(), vValue);
        result = MultiplyAddByZ(result, matrix.Z.AsVector128(), vValue);
        return new Vector3(result);
    }

    /// <summary>Reinterprets the current instance as a new <see cref="SysVector3" />.</summary>
    /// <returns>The current instance reintepreted as a new <see cref="SysVector3" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SysVector3 AsVector3() => _value;

    /// <summary>Reinterprets the current instance as a new <see cref="Vector128{Single}" />.</summary>
    /// <returns>The current instance reintepreted as a new <see cref="Vector128{Single}" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector128<float> AsVector128() => _value.AsVector128();

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Vector3 other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(Vector3 other)
    {
        return X.Equals(other.X)
            && Y.Equals(other.Y)
            && Z.Equals(other.Z);
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return new StringBuilder(7 + (separator.Length * 2))
            .Append('<')
            .Append(X.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append(Y.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append(Z.ToString(format, formatProvider))
            .Append('>')
            .ToString();
    }

    /// <summary>Creates a new <see cref="Vector3" /> instance with <see cref="X" /> set to the specified value.</summary>
    /// <param name="x">The new value of the x-dimension.</param>
    /// <returns>A new <see cref="Vector3" /> instance with <see cref="X" /> set to <paramref name="x" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithX(float x)
    {
        var result = _value;
        result.X = x;
        return new Vector3(result);
    }

    /// <summary>Creates a new <see cref="Vector3" /> instance with <see cref="Y" /> set to the specified value.</summary>
    /// <param name="y">The new value of the y-dimension.</param>
    /// <returns>A new <see cref="Vector3" /> instance with <see cref="Y" /> set to <paramref name="y" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithY(float y)
    {
        var result = _value;
        result.Y = y;
        return new Vector3(result);
    }

    /// <summary>Creates a new <see cref="Vector3" /> instance with <see cref="Z" /> set to the specified value.</summary>
    /// <param name="z">The new value of the z-dimension.</param>
    /// <returns>A new <see cref="Vector3" /> instance with <see cref="Z" /> set to <paramref name="z" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithZ(float z)
    {
        var result = _value;
        result.Z = z;
        return new Vector3(result);
    }
}
