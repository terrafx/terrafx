// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using TerraFX.Utilities;
using static TerraFX.Utilities.VectorUtilities;
using SysVector3 = System.Numerics.Vector3;

namespace TerraFX.Numerics;

/// <summary>Defines a three-dimensional Euclidean vector.</summary>
public readonly struct Vector3 : IEquatable<Vector3>, IFormattable
{
    /// <summary>Defines a <see cref="Vector3" /> where all components are zero.</summary>
    public static Vector3 Zero => Create(0.0f, 0.0f, 0.0f);

    /// <summary>Defines a <see cref="Vector3" /> whose x-component is one and whose remaining components are zero.</summary>
    public static Vector3 UnitX => Create(1.0f, 0.0f, 0.0f);

    /// <summary>Defines a <see cref="Vector3" /> whose y-component is one and whose remaining components are zero.</summary>
    public static Vector3 UnitY => Create(0.0f, 1.0f, 0.0f);

    /// <summary>Defines a <see cref="Vector3" /> whose z-component is one and whose remaining components are zero.</summary>
    public static Vector3 UnitZ => Create(0.0f, 0.0f, 1.0f);

    /// <summary>Defines a <see cref="Vector3" /> where all components are one.</summary>
    public static Vector3 One => Create(1.0f, 1.0f, 1.0f);

    private readonly SysVector3 _value;

    /// <summary>Creates a vector where each component is set a specified value.</summary>
    /// <param name="value">The value to set each component to.</param>
    public static Vector3 Create(float value)
    {
        var vector = new SysVector3(value);
        return new Vector3(vector);
    }

    /// <summary>Creates a vector from a system vector.</summary>
    /// <param name="value">The value of the vector.</param>
    public static Vector3 Create(SysVector3 value) => new Vector3(value);

    /// <summary>Creates a vector from a hardware vector.</summary>
    /// <param name="value">The value of the vector.</param>
    public static Vector3 Create(Vector128<float> value) => new Vector3(value.AsVector3());

    /// <summary>Creates a vector from a two-dimensional vector and a Z-component</summary>
    /// <param name="vector">The value of the x and y dimensions.</param>
    /// <param name="z">The value of the z-component.</param>
    public static Vector3 Create(Vector2 vector, float z)
    {
        var value = new SysVector3(vector.AsSystemVector2(), z);
        return new Vector3(value);
    }

    /// <summary>Creates a vector from an X, Y, and Z component.</summary>
    /// <param name="x">The value of the x-component.</param>
    /// <param name="y">The value of the y-component.</param>
    /// <param name="z">The value of the z-component.</param>
    public static Vector3 Create(float x, float y, float z)
    {
        var value = new SysVector3(x, y, z);
        return new Vector3(value);
    }

    private Vector3(SysVector3 value)
    {
        _value = value;
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

    /// <summary>Gets the value of the x-component.</summary>
    public float X
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.X;
        }
    }

    /// <summary>Gets the value of the y-component.</summary>
    public float Y
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.Y;
        }
    }

    /// <summary>Gets the value of the z-component.</summary>
    public float Z
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.Z;
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
    /// <returns>The product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 left, float right) => new Vector3(left._value * right);

    /// <summary>Computes the product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 left, Vector3 right) => new Vector3(left._value * right._value);

    /// <summary>Computes the product of a vector and matrix.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The matrix which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 left, Matrix4x4 right) => Transform(left, right);

    /// <summary>Computes the quotient of a vector and a float.</summary>
    /// <param name="left">The vector which is divided by <paramref name="right" />.</param>
    /// <param name="right">The float which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 left, float right) => new Vector3(left._value / right);

    /// <summary>Computes the quotient of two vectors.</summary>
    /// <param name="left">The vector which is divided by <paramref name="right" />.</param>
    /// <param name="right">The vector which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 left, Vector3 right) => new Vector3(left._value / right._value);

    /// <summary>Compares two vectors to determine element-wise equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>A vector that contains the element-wise comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 CompareEqual(Vector3 left, Vector3 right)
    {
        var result = VectorUtilities.CompareEqual(left._value.AsVector128(), right._value.AsVector128());
        return new Vector3(result.AsVector3());
    }

    /// <summary>Compares two vectors to determine approximate equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 CompareEqual(Vector3 left, Vector3 right, Vector3 epsilon)
    {
        var result = VectorUtilities.CompareEqual(left._value.AsVector128(), right._value.AsVector128(), epsilon._value.AsVector128());
        return new Vector3(result.AsVector3());
    }

    /// <summary>Compares two vectors to determine if all elements are equal.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if all elements of <paramref name="left" /> are equal to the corresponding element of <paramref name="right" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Vector3 left, Vector3 right) => left == right;

    /// <summary>Compares two vectors to determine if all elements are approximately equal.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">he maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Vector3 left, Vector3 right, Vector3 epsilon)
        => VectorUtilities.CompareEqualAll(left._value.AsVector128(), right._value.AsVector128(), epsilon._value.AsVector128());

    /// <summary>Computes the cross product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The quaternion which is used to multiply <paramref name="left" />.</param>
    /// <returns>The cross product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 CrossProduct(Vector3 left, Vector3 right)
    {
        var result = SysVector3.Cross(left._value, right._value);
        return new Vector3(result);
    }

    /// <summary>Computes the dot product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The quaternion which is used to multiply <paramref name="left" />.</param>
    /// <returns>The dot product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DotProduct(Vector3 left, Vector3 right) => SysVector3.Dot(left._value, right._value);

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
        var result = VectorUtilities.Max(left._value.AsVector128(), right._value.AsVector128());
        return new Vector3(result.AsVector3());
    }

    /// <summary>Compares two vectors to determine the combined minimum.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>The combined minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Min(Vector3 left, Vector3 right)
    {
        var result = VectorUtilities.Min(left._value.AsVector128(), right._value.AsVector128());
        return new Vector3(result.AsVector3());
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
        return new Vector3(result.AsVector3());
    }

    /// <summary>Computes an estimate of the reciprocal of a vector.</summary>
    /// <param name="value">The vector for which to compute the reciprocal.</param>
    /// <returns>An estimate of the element-wise reciprocal of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ReciprocalEstimate(Vector3 value)
    {
        var result = VectorUtilities.ReciprocalEstimate(value._value.AsVector128());
        return new Vector3(result.AsVector3());
    }

    /// <summary>Computes an estimate of the reciprocal square-root of a vector.</summary>
    /// <param name="value">The vector for which to compute the reciprocal square-root.</param>
    /// <returns>An estimate of the element-wise reciprocal square-root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ReciprocalSqrtEstimate(Vector3 value)
    {
        var result = VectorUtilities.ReciprocalSqrtEstimate(value._value.AsVector128());
        return new Vector3(result.AsVector3());
    }

    /// <summary>Rotates a vector using a quaternion.</summary>
    /// <param name="value">The vector to rotate.</param>
    /// <param name="rotation">The rotation.</param>
    /// <returns><paramref name="value" /> rotated by <paramref name="rotation" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Rotate(Vector3 value, Quaternion rotation)
    {
        var vValue = Quaternion.Create(value._value.AsVector128());
        var conjugate = Quaternion.Conjugate(rotation);

        var result = Quaternion.Concatenate(conjugate, vValue);
        result = Quaternion.Concatenate(result, rotation);

        return Create(result.Value);
    }

    /// <summary>Rotates a vector using the inverse of a quaternion.</summary>
    /// <param name="value">The vector to rotate.</param>
    /// <param name="rotation">The rotation.</param>
    /// <returns><paramref name="value" /> rotated by the inverse of <paramref name="rotation" />.</returns>
    public static Vector3 RotateInverse(Vector3 value, Quaternion rotation)
    {
        var vValue = Quaternion.Create(value._value.AsVector128());
        var conjugate = Quaternion.Conjugate(rotation);

        var result = Quaternion.Concatenate(rotation, vValue);
        result = Quaternion.Concatenate(result, conjugate);

        return Create(result.Value);
    }

    /// <summary>Computes the square-root of a vector.</summary>
    /// <param name="value">The vector for which to compute the square-root.</param>
    /// <returns>The element-wise square-root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Sqrt(Vector3 value)
    {
        var result = VectorUtilities.Sqrt(value._value.AsVector128());
        return new Vector3(result.AsVector3());
    }

    /// <summary>Transforms a vector using a matrix.</summary>
    /// <param name="value">The vector to transform.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns><paramref name="value" /> transformed by <paramref name="matrix" />.</returns>
    public static Vector3 Transform(Vector3 value, Matrix4x4 matrix)
    {
        var vValue = value._value.AsVector128();

        var result = matrix.W.Value;
        result = MultiplyAddByX(result, matrix.X.Value, vValue);
        result = MultiplyAddByY(result, matrix.Y.Value, vValue);
        result = MultiplyAddByZ(result, matrix.Z.Value, vValue);
        return Create(result);
    }

    /// <summary>Transforms a vector using a normalized matrix.</summary>
    /// <param name="value">The vector to transform.</param>
    /// <param name="matrix">The normalized transformation matrix.</param>
    /// <returns><paramref name="value" /> transformed by <paramref name="matrix" />.</returns>
    public static Vector3 TransformNormal(Vector3 value, Matrix4x4 matrix)
    {
        var vValue = value._value.AsVector128();

        var result = MultiplyByX(matrix.X.Value, vValue);
        result = MultiplyAddByY(result, matrix.Y.Value, vValue);
        result = MultiplyAddByZ(result, matrix.Z.Value, vValue);
        return Create(result);
    }

    /// <summary>Reinterprets the current instance as a new <see cref="SysVector3" />.</summary>
    /// <returns>The current instance reinterpreted as a new <see cref="SysVector3" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SysVector3 AsSystemVector3() => _value;

    /// <summary>Reinterprets the current instance as a new <see cref="Vector128{Single}" />.</summary>
    /// <returns>The current instance reinterpreted as a new <see cref="Vector128{Single}" />.</returns>
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
        => $"{nameof(Vector3)} {{ {nameof(X)} = {X.ToString(format, formatProvider)}, {nameof(Y)} = {Y.ToString(format, formatProvider)}, {nameof(Z)} = {Z.ToString(format, formatProvider)} }}";

    /// <summary>Creates a new vector with <see cref="X" /> set to the specified value.</summary>
    /// <param name="x">The new x-component of the vector.</param>
    /// <returns>A new vector with <see cref="X" /> set to <paramref name="x" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithX(float x)
    {
        var result = _value;
        result.X = x;
        return new Vector3(result);
    }

    /// <summary>Creates a new vector with <see cref="Y" /> set to the specified value.</summary>
    /// <param name="y">The new y-component of the vector.</param>
    /// <returns>A new vector with <see cref="Y" /> set to <paramref name="y" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithY(float y)
    {
        var result = _value;
        result.Y = y;
        return new Vector3(result);
    }

    /// <summary>Creates a new vector with <see cref="Z" /> set to the specified value.</summary>
    /// <param name="z">The new z-component of the vector.</param>
    /// <returns>A new vector with <see cref="Z" /> set to <paramref name="z" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithZ(float z)
    {
        var result = _value;
        result.Z = z;
        return new Vector3(result);
    }
}
