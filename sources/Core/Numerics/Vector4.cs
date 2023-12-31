// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using TerraFX.Utilities;
using static TerraFX.Utilities.VectorUtilities;
using SysVector4 = System.Numerics.Vector4;

namespace TerraFX.Numerics;

/// <summary>Defines a four-dimensional Euclidean vector.</summary>
public readonly struct Vector4
    : IEquatable<Vector4>,
      IFormattable,
      ISpanFormattable,
      IUtf8SpanFormattable
{
    /// <summary>Defines a <see cref="Vector4" /> where all components are zero.</summary>
    public static Vector4 Zero => Create(0.0f, 0.0f, 0.0f, 0.0f);

    /// <summary>Defines a <see cref="Vector4" /> whose x-component is one and whose remaining components are zero.</summary>
    public static Vector4 UnitX => Create(1.0f, 0.0f, 0.0f, 0.0f);

    /// <summary>Defines a <see cref="Vector4" /> whose y-component is one and whose remaining components are zero.</summary>
    public static Vector4 UnitY => Create(0.0f, 1.0f, 0.0f, 0.0f);

    /// <summary>Defines a <see cref="Vector4" /> whose z-component is one and whose remaining components are zero.</summary>
    public static Vector4 UnitZ => Create(0.0f, 0.0f, 1.0f, 0.0f);

    /// <summary>Defines a <see cref="Vector4" /> whose w-component is one and whose remaining components are zero.</summary>
    public static Vector4 UnitW => Create(0.0f, 0.0f, 0.0f, 1.0f);

    /// <summary>Defines a <see cref="Vector4" /> where all components are one.</summary>
    public static Vector4 One => Create(1.0f, 1.0f, 1.0f, 1.0f);

    private readonly Vector128<float> _value;

    /// <summary>Creates a vector where each component is set a specified value.</summary>
    /// <param name="value">The value to set each component to.</param>
    public static Vector4 Create(float value) => new Vector4(Vector128.Create(value));

    /// <summary>Creates a vector from a system vector.</summary>
    /// <param name="value">The value of the vector.</param>
    public static Vector4 Create(SysVector4 value) => new Vector4(value.AsVector128());

    /// <summary>Creates a vector from a hardware vector.</summary>
    /// <param name="value">The value of the vector.</param>
    public static Vector4 Create(Vector128<float> value) => new Vector4(value);

    /// <summary>Creates a vector from a three-dimensional vector and a W-component</summary>
    /// <param name="vector">The value of the x, y and z-dimensions.</param>
    /// <param name="w">The value of the w-component.</param>
    public static Vector4 Create(Vector3 vector, float w)
    {
        var value = new SysVector4(vector.AsSystemVector3(), w);
        return new Vector4(value.AsVector128());
    }

    /// <summary>Creates a vector from a two-dimensional vector, a Y-component, and a Z-component</summary>
    /// <param name="vector">The value of the x and y dimensions.</param>
    /// <param name="z">The value of the z-component.</param>
    /// <param name="w">The value of the w-component.</param>
    public static Vector4 Create(Vector2 vector, float z, float w)
    {
        var value = new SysVector4(vector.AsSystemVector2(), z, w);
        return new Vector4(value.AsVector128());
    }

    /// <summary>Creates a vector from an X, Y, Z, and W-component.</summary>
    /// <param name="x">The value of the x-component.</param>
    /// <param name="y">The value of the y-component.</param>
    /// <param name="z">The value of the z-component.</param>
    /// <param name="w">The value of the w-component.</param>
    public static Vector4 Create(float x, float y, float z, float w) => new Vector4(Vector128.Create(x, y, z, w));

    private Vector4(Vector128<float> value)
    {
        _value = value;
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

    /// <summary>Gets the value of the vector.</summary>
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

    /// <summary>Compares two vectors to determine equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector4 left, Vector4 right) => VectorUtilities.CompareEqualAll(left._value, right._value);

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
    /// <returns>The product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 left, float right)
    {
        var result = Multiply(left._value, right);
        return new Vector4(result);
    }

    /// <summary>Computes the product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 left, Vector4 right)
    {
        var result = Multiply(left._value, right._value);
        return new Vector4(result);
    }

    /// <summary>Computes the product of a vector and matrix.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The matrix which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 left, Matrix4x4 right) => Transform(left, right);

    /// <summary>Computes the quotient of a vector and a float.</summary>
    /// <param name="left">The vector which is divided by <paramref name="right" />.</param>
    /// <param name="right">The float which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 left, float right)
    {
        var result = Divide(left._value, right);
        return new Vector4(result);
    }

    /// <summary>Computes the quotient of two vectors.</summary>
    /// <param name="left">The vector which is divided by <paramref name="right" />.</param>
    /// <param name="right">The vector which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 left, Vector4 right)
    {
        var result = Divide(left._value, right._value);
        return new Vector4(result);
    }

    /// <summary>Compares two vectors to determine element-wise equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>A vector that contains the element-wise comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 CompareEqual(Vector4 left, Vector4 right)
    {
        var result = VectorUtilities.CompareEqual(left._value, right._value);
        return new Vector4(result);
    }

    /// <summary>Compares two vectors to determine approximate equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns>A vector that contains the element-wise approximate comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 CompareEqual(Vector4 left, Vector4 right, Vector4 epsilon)
    {
        var result = VectorUtilities.CompareEqual(left._value, right._value, epsilon._value);
        return new Vector4(result);
    }

    /// <summary>Compares two vectors to determine if all elements are equal.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if all elements of <paramref name="left" /> are equal to the corresponding element of <paramref name="right" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Vector4 left, Vector4 right) => left == right;

    /// <summary>Compares two vectors to determine if all elements are approximately equal.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Vector4 left, Vector4 right, Vector4 epsilon) => VectorUtilities.CompareEqualAll(left._value, right._value, epsilon._value);

    /// <summary>Computes the dot product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The quaternion which is used to multiply <paramref name="left" />.</param>
    /// <returns>The dot product of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DotProduct(Vector4 left, Vector4 right)
    {
        var result = VectorUtilities.DotProduct(left._value, right._value);
        return result.ToScalar();
    }

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
        var result = MultiplyByX(matrix.X.Value, value._value);
        result = MultiplyAddByY(result, matrix.Y.Value, value._value);
        result = MultiplyAddByZ(result, matrix.Z.Value, value._value);
        result = MultiplyAddByW(result, matrix.W.Value, value._value);
        return new Vector4(result);
    }

    /// <summary>Reinterprets the current instance as a new <see cref="SysVector4" />.</summary>
    /// <returns>The current instance reinterpreted as a new <see cref="SysVector4" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SysVector4 AsSystemVector4() => _value.AsVector4();

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
    public string ToString(string? format = null, IFormatProvider? formatProvider = null)
        => $"Vector4 {{ X = {X.ToString(format, formatProvider)}, Y = {Y.ToString(format, formatProvider)}, Z = {Z.ToString(format, formatProvider)}, W = {W.ToString(format, formatProvider)} }}";

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"Vector4 { X = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        var partLength = "Vector4 { X = ".Length;

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
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"Vector4 { X = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        var partLength = "Vector4 { X = "u8.Length;

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

    /// <summary>Creates a new vector with <see cref="X" /> set to the specified value.</summary>
    /// <param name="x">The new x-component of the vector.</param>
    /// <returns>A new vector with <see cref="X" /> set to <paramref name="x" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithX(float x)
    {
        var result = _value.WithX(x);
        return new Vector4(result);
    }

    /// <summary>Creates a new vector with <see cref="Y" /> set to the specified value.</summary>
    /// <param name="y">The new y-component of the vector.</param>
    /// <returns>A new vector with <see cref="Y" /> set to <paramref name="y" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithY(float y)
    {
        var result = _value.WithY(y);
        return new Vector4(result);
    }

    /// <summary>Creates a new vector with <see cref="Z" /> set to the specified value.</summary>
    /// <param name="z">The new z-component of the vector.</param>
    /// <returns>A new vector with <see cref="Z" /> set to <paramref name="z" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithZ(float z)
    {
        var result = _value.WithZ(z);
        return new Vector4(result);
    }

    /// <summary>Creates a new vector with <see cref="W" /> set to the specified value.</summary>
    /// <param name="w">The new z-component of the vector.</param>
    /// <returns>A new vector with <see cref="W" /> set to <paramref name="w" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithW(float w)
    {
        var result = _value.WithW(w);
        return new Vector4(result);
    }
}
