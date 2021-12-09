// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;
using TerraFX.Utilities;
using static TerraFX.Utilities.VectorUtilities;
using SysVector4 = System.Numerics.Vector4;

namespace TerraFX.Numerics;

/// <summary>Defines a four-dimensional Euclidean vector.</summary>
public readonly struct Vector4 : IEquatable<Vector4>, IFormattable
{
    /// <summary>Defines a <see cref="Vector4" /> where all components are zero.</summary>
    public static Vector4 Zero => new Vector4(Vector128<float>.Zero);

    /// <summary>Defines a <see cref="Vector4" /> whose x-component is one and whose remaining components are zero.</summary>
    public static Vector4 UnitX => new Vector4(SysVector4.UnitX);

    /// <summary>Defines a <see cref="Vector4" /> whose y-component is one and whose remaining components are zero.</summary>
    public static Vector4 UnitY => new Vector4(SysVector4.UnitY);

    /// <summary>Defines a <see cref="Vector4" /> whose z-component is one and whose remaining components are zero.</summary>
    public static Vector4 UnitZ => new Vector4(SysVector4.UnitZ);

    /// <summary>Defines a <see cref="Vector4" /> whose w-component is one and whose remaining components are zero.</summary>
    public static Vector4 UnitW => new Vector4(SysVector4.UnitW);

    /// <summary>Defines a <see cref="Vector4" /> where all components are one.</summary>
    public static Vector4 One => new Vector4(SysVector4.One);

    private readonly Vector128<float> _value;

    /// <summary>Initializes a new instance of the <see cref="Vector4" /> struct.</summary>
    /// <param name="x">The value of the x-dimension.</param>
    /// <param name="y">The value of the y-dimension.</param>
    /// <param name="z">The value of the z-dimension.</param>
    /// <param name="w">The value of the w-dimension.</param>
    public Vector4(float x, float y, float z, float w)
    {
        _value = Vector128.Create(x, y, z, w);
    }

    /// <summary>Initializes a new instance of the <see cref="Vector4" /> struct with each component set to <paramref name="value" />.</summary>
    /// <param name="value">The value to set each component to.</param>
    public Vector4(float value)
    {
        _value = Vector128.Create(value);
    }

    /// <summary>Initializes a new instance of the <see cref="Vector4" /> struct.</summary>
    /// <param name="vector">The value of the x and y dimensions.</param>
    /// <param name="z">The value of the z-dimension.</param>
    /// <param name="w">The value of the w-dimension.</param>
    public Vector4(Vector2 vector, float z, float w)
    {
        var value = new SysVector4(vector.AsVector2(), z, w);
        _value = value.AsVector128();
    }

    /// <summary>Initializes a new instance of the <see cref="Vector4" /> struct.</summary>
    /// <param name="vector">The value of the x, y and z-dimensions.</param>
    /// <param name="w">The value of the w-dimension.</param>
    public Vector4(Vector3 vector, float w)
    {
        var value = new SysVector4(vector.AsVector3(), w);
        _value = value.AsVector128();
    }

    /// <summary>Initializes a new instance of the <see cref="Vector4" /> struct.</summary>
    /// <param name="value">The value of the vector.</param>
    public Vector4(SysVector4 value)
    {
        _value = value.AsVector128();
    }

    /// <summary>Initializes a new instance of the <see cref="Vector4" /> struct.</summary>
    /// <param name="value">The value of the vector.</param>
    public Vector4(Vector128<float> value)
    {
        _value = value;
    }

    /// <summary>Gets the value of the x-dimension.</summary>
    public float X
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.ToScalar();
        }
    }

    /// <summary>Gets the value of the y-dimension.</summary>
    public float Y
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.GetElement(1);
        }
    }

    /// <summary>Gets the value of the z-dimension.</summary>
    public float Z
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.GetElement(2);
        }
    }

    /// <summary>Gets the value of the w-dimension.</summary>
    public float W
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.GetElement(3);
        }
    }

    /// <summary>Gets the length of the vector.</summary>
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var result = Length(_value);
            return result.ToScalar();
        }
    }

    /// <summary>Gets the squared length of the vector.</summary>
    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var result = LengthSquared(_value);
            return result.ToScalar();
        }
    }

    /// <summary>Gets an estimate of the reciprocal length of the vector.</summary>
    public float ReciprocalLengthEstimate
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var result = ReciprocalLengthEstimate(_value);
            return result.ToScalar();
        }
    }

    /// <summary>Compares two vectors to determine equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector4 left, Vector4 right) => CompareEqualAll(left._value, right._value);

    /// <summary>Compares two vectors to determine inequality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector4 left, Vector4 right) => CompareNotEqualAny(left._value, right._value);

    /// <summary>Computes the value of a vector.</summary>
    /// <param name="value">The vector.</param>
    /// <returns><paramref name="value" /></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 value) => value;

    /// <summary>Computes the negation of a vector.</summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negation of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 value)
    {
        var result = Negate(value._value);
        return new Vector4(result);
    }

    /// <summary>Computes the sum of two vectors.</summary>
    /// <param name="left">The vector to which to add <paramref name="right" />.</param>
    /// <param name="right">The vector which is added to <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="right" /> added to <paramref name="left" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 left, Vector4 right)
    {
        var result = Add(left._value, right._value);
        return new Vector4(result);
    }

    /// <summary>Computes the difference of two vectors.</summary>
    /// <param name="left">The vector from which to subtract <paramref name="right" />.</param>
    /// <param name="right">The vector which is subtracted from <paramref name="left" />.</param>
    /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 left, Vector4 right)
    {
        var result = Subtract(left._value, right._value);
        return new Vector4(result);
    }

    /// <summary>Computes the product of a vector and a float.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The float which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 left, float right)
    {
        var result = Multiply(left._value, right);
        return new Vector4(result);
    }

    /// <summary>Computes the product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 left, Vector4 right)
    {
        var result = Multiply(left._value, right._value);
        return new Vector4(result);
    }

    /// <summary>Computes the product of a vector and matrix.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The matrix which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 left, Matrix4x4 right) => Transform(left, right);

    /// <summary>Computes the quotient of a vector and a float.</summary>
    /// <param name="left">The vector which is divied by <paramref name="right" />.</param>
    /// <param name="right">The float which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 left, float right)
    {
        var result = Divide(left._value, right);
        return new Vector4(result);
    }

    /// <summary>Computes the quotient of two vectors.</summary>
    /// <param name="left">The vector which is divied by <paramref name="right" />.</param>
    /// <param name="right">The vector which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 left, Vector4 right)
    {
        var result = Divide(left._value, right._value);
        return new Vector4(result);
    }

    /// <summary>Computes the dot product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The quatnerion which is used to multiply <paramref name="left" />.</param>
    /// <returns>The dot product of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DotProduct(Vector4 left, Vector4 right)
    {
        var result = VectorUtilities.DotProduct(left._value, right._value);
        return result.ToScalar();
    }

    /// <summary>Compares two vectors to determine element-wise equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>A vector that contains the element-wise comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Equals(Vector4 left, Vector4 right)
    {
        var result = CompareEqual(left._value, right._value);
        return new Vector4(result);
    }

    /// <summary>Compares two vectors to determine approximate equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns>A vector that contains the element-wise approximate comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Equals(Vector4 left, Vector4 right, Vector4 epsilon)
    {
        var result = CompareEqual(left._value, right._value, epsilon._value);
        return new Vector4(result);
    }

    /// <summary>Compares two vectors to determine if all elements are equal.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if all elements of <paramref name="left" /> are equal to the corresponding element of <paramref name="right" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(Vector4 left, Vector4 right) => left == right;

    /// <summary>Compares two vectors to determine if all elements are approximately equal.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(Vector4 left, Vector4 right, Vector4 epsilon) => CompareEqualAll(left._value, right._value, epsilon._value);

    /// <summary>Determines if any elements in a vector are either <see cref="float.PositiveInfinity" /> or <see cref="float.NegativeInfinity" />.</summary>
    /// <param name="value">The vector to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are either <see cref="float.PositiveInfinity" /> or <see cref="float.NegativeInfinity" />; otherwise, <c>false</c>.</returns>
    public static bool IsAnyInfinity(Vector4 value) => VectorUtilities.IsAnyInfinity(value._value);

    /// <summary>Determines if any elements in a quaternion are <see cref="float.NaN" />.</summary>
    /// <param name="value">The vector to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are <see cref="float.NaN" />; otherwise, <c>false</c>.</returns>
    public static bool IsAnyNaN(Vector4 value) => VectorUtilities.IsAnyNaN(value._value);

    /// <summary>Compares two vectors to determine the element-wise maximum.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>The element-wise maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Max(Vector4 left, Vector4 right)
    {
        var result = VectorUtilities.Max(left._value, right._value);
        return new Vector4(result);
    }

    /// <summary>Compares two vectors to determine the element-wise minimum.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>The element-wise minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Min(Vector4 left, Vector4 right)
    {
        var result = VectorUtilities.Min(left._value, right._value);
        return new Vector4(result);
    }

    /// <summary>Computes the normalized form of a vector.</summary>
    /// <param name="value">The vector to normalized.</param>
    /// <returns>The normalized form of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Normalize(Vector4 value)
    {
        var result = VectorUtilities.Normalize(value._value);
        return new Vector4(result);
    }

    /// <summary>Computes an estimate of the normalized form of a vector.</summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>An estimate of the normalized form of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 NormalizeEstimate(Vector4 value)
    {
        var result = VectorUtilities.NormalizeEstimate(value._value);
        return new Vector4(result);
    }

    /// <summary>Computes an estimate of the reciprocal of a vector.</summary>
    /// <param name="value">The vector for which to compute the reciprocal.</param>
    /// <returns>An estimate of the element-wise reciprocal of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ReciprocalEstimate(Vector4 value)
    {
        var result = VectorUtilities.ReciprocalEstimate(value._value);
        return new Vector4(result);
    }

    /// <summary>Computes an estimate of the reciprocal square-root of a vector.</summary>
    /// <param name="value">The vector for which to compute the reciprocal square-root.</param>
    /// <returns>An estimate of the element-wise reciprocal square-root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ReciprocalSqrtEstimate(Vector4 value)
    {
        var result = VectorUtilities.ReciprocalSqrtEstimate(value._value);
        return new Vector4(result);
    }

    /// <summary>Computes the square-root of a vector.</summary>
    /// <param name="value">The vector for which to compute the square-root.</param>
    /// <returns>The element-wise square-root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Sqrt(Vector4 value)
    {
        var result = VectorUtilities.Sqrt(value._value);
        return new Vector4(result);
    }

    /// <summary>Transforms a vector using a matrix.</summary>
    /// <param name="value">The vector to transform.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns><paramref name="value" /> transformed by <paramref name="matrix" />.</returns>
    public static Vector4 Transform(Vector4 value, Matrix4x4 matrix)
    {
        var result = MultiplyByX(matrix.X._value, value._value);
        result = MultiplyAddByY(result, matrix.Y._value, value._value);
        result = MultiplyAddByZ(result, matrix.Z._value, value._value);
        result = MultiplyAddByW(result, matrix.W._value, value._value);
        return new Vector4(result);
    }

    /// <summary>Reinterprets the current instance as a new <see cref="SysVector4" />.</summary>
    /// <returns>The current instance reintepreted as a new <see cref="SysVector4" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SysVector4 AsVector4() => _value.AsVector4();

    /// <summary>Reinterprets the current instance as a new <see cref="Vector128{Single}" />.</summary>
    /// <returns>The current instance reintepreted as a new <see cref="Vector128{Single}" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector128<float> AsVector128() => _value;

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Vector4 other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(Vector4 other)
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

    /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="X" /> set to the specified value.</summary>
    /// <param name="x">The new value of the x-dimension.</param>
    /// <returns>A new <see cref="Vector4" /> instance with <see cref="X" /> set to <paramref name="x" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithX(float x)
    {
        var result = _value.WithElement(0, x);
        return new Vector4(result);
    }

    /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="Y" /> set to the specified value.</summary>
    /// <param name="y">The new value of the y-dimension.</param>
    /// <returns>A new <see cref="Vector4" /> instance with <see cref="Y" /> set to <paramref name="y" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithY(float y)
    {
        var result = _value.WithElement(1, y);
        return new Vector4(result);
    }

    /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="Z" /> set to the specified value.</summary>
    /// <param name="z">The new value of the z-dimension.</param>
    /// <returns>A new <see cref="Vector4" /> instance with <see cref="Z" /> set to <paramref name="z" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithZ(float z)
    {
        var result = _value.WithElement(2, z);
        return new Vector4(result);
    }

    /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="W" /> set to the specified value.</summary>
    /// <param name="w">The new value of the w-dimension.</param>
    /// <returns>A new <see cref="Vector4" /> instance with <see cref="W" /> set to <paramref name="w" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithW(float w)
    {
        var result = _value.WithElement(3, w);
        return new Vector4(result);
    }
}
