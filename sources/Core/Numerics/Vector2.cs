// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using TerraFX.Utilities;
using static TerraFX.Utilities.VectorUtilities;
using SysVector2 = System.Numerics.Vector2;

namespace TerraFX.Numerics;

/// <summary>Defines a two-dimension Euclidean vector.</summary>
public readonly struct Vector2
    : IEquatable<Vector2>,
      IFormattable,
      ISpanFormattable,
      IUtf8SpanFormattable
{
    /// <summary>Defines a <see cref="Vector2" /> where all components are zero.</summary>
    public static Vector2 Zero => Create(0.0f, 0.0f);

    /// <summary>Defines a <see cref="Vector2" /> whose x-component is one and whose remaining components are zero.</summary>
    public static Vector2 UnitX => Create(1.0f, 0.0f);

    /// <summary>Defines a <see cref="Vector2" /> whose y-component is one and whose remaining components are zero.</summary>
    public static Vector2 UnitY => Create(0.0f, 1.0f);

    /// <summary>Defines a <see cref="Vector2" /> where all components are one.</summary>
    public static Vector2 One => Create(1.0f, 1.0f);

    private readonly SysVector2 _value;

    /// <summary>Creates a vector where each component is set a specified value.</summary>
    /// <param name="value">The value to set each component to.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Create(float value)
    {
        var vValue = new SysVector2(value);
        return new Vector2(vValue);
    }

    /// <summary>Creates a vector from a system vector.</summary>
    /// <param name="value">The value of the vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Create(SysVector2 value) => new Vector2(value);

    /// <summary>Creates a vector from a hardware vector.</summary>
    /// <param name="value">The value of the vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Create(Vector128<float> value) => new Vector2(value.AsVector2());

    /// <summary>Creates a vector from an X and Y component.</summary>
    /// <param name="x">The value of the x-component.</param>
    /// <param name="y">The value of the y-component.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Create(float x, float y)
    {
        var value = new SysVector2(x, y);
        return new Vector2(value);
    }

    private Vector2(SysVector2 value)
    {
        _value = value;
    }

    /// <summary>Gets the length of the vector.</summary>
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

    /// <summary>Compares two vectors to determine equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2 left, Vector2 right) => left._value == right._value;

    /// <summary>Compares two vectors to determine inequality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2 left, Vector2 right) => left._value != right._value;

    /// <summary>Computes the value of a vector.</summary>
    /// <param name="value">The vector.</param>
    /// <returns><paramref name="value" /></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(Vector2 value) => value;

    /// <summary>Computes the negation of a vector.</summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negation of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 value) => new Vector2(-value._value);

    /// <summary>Computes the sum of two vectors.</summary>
    /// <param name="left">The vector to which to add <paramref name="right" />.</param>
    /// <param name="right">The vector which is added to <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="right" /> added to <paramref name="left" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(Vector2 left, Vector2 right) => new Vector2(left._value + right._value);

    /// <summary>Computes the difference of two vectors.</summary>
    /// <param name="left">The vector from which to subtract <paramref name="right" />.</param>
    /// <param name="right">The vector which is subtracted from <paramref name="left" />.</param>
    /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 left, Vector2 right) => new Vector2(left._value - right._value);

    /// <summary>Computes the product of a vector and a float.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The float which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 left, float right) => new Vector2(left._value * right);

    /// <summary>Computes the product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 left, Vector2 right) => new Vector2(left._value * right._value);

    /// <summary>Computes the quotient of a vector and a float.</summary>
    /// <param name="left">The vector which is divided by <paramref name="right" />.</param>
    /// <param name="right">The float which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2 left, float right) => new Vector2(left._value / right);

    /// <summary>Computes the quotient of two vectors.</summary>
    /// <param name="left">The vector which is divided by <paramref name="right" />.</param>
    /// <param name="right">The vector which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2 left, Vector2 right) => new Vector2(left._value / right._value);

    /// <summary>Compares two vectors to determine element-wise equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>A vector that contains the element-wise comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 CompareEqual(Vector2 left, Vector2 right)
    {
        var result = VectorUtilities.CompareEqual(left._value.AsVector128(), right._value.AsVector128());
        return new Vector2(result.AsVector2());
    }

    /// <summary>Compares two vectors to determine approximate equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 CompareEqual(Vector2 left, Vector2 right, Vector2 epsilon)
    {
        var result = VectorUtilities.CompareEqual(left._value.AsVector128(), right._value.AsVector128(), epsilon._value.AsVector128());
        return new Vector2(result.AsVector2());
    }

    /// <summary>Compares two vectors to determine if all elements are equal.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if all elements of <paramref name="left" /> are equal to the corresponding element of <paramref name="right" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Vector2 left, Vector2 right) => left == right;

    /// <summary>Compares two vectors to determine if all elements are approximately equal.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">he maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Vector2 left, Vector2 right, Vector2 epsilon)
        => VectorUtilities.CompareEqualAll(left._value.AsVector128(), right._value.AsVector128(), epsilon._value.AsVector128());

    /// <summary>Computes the dot product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The quaternion which is used to multiply <paramref name="left" />.</param>
    /// <returns>The dot product of <paramref name="left" /> multiplied by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DotProduct(Vector2 left, Vector2 right) => SysVector2.Dot(left._value, right._value);

    /// <summary>Determines if any elements in a vector are either <see cref="float.PositiveInfinity" /> or <see cref="float.NegativeInfinity" />.</summary>
    /// <param name="value">The vector to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are either <see cref="float.PositiveInfinity" /> or <see cref="float.NegativeInfinity" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAnyInfinity(Vector2 value) => VectorUtilities.IsAnyInfinity(value._value.AsVector128());

    /// <summary>Determines if any elements in a quaternion are <see cref="float.NaN" />.</summary>
    /// <param name="value">The vector to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are <see cref="float.NaN" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAnyNaN(Vector2 value) => VectorUtilities.IsAnyNaN(value._value.AsVector128());

    /// <summary>Compares two vectors to determine the combined maximum.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>The combined maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Max(Vector2 left, Vector2 right)
    {
        var result = VectorUtilities.Max(left._value.AsVector128(), right._value.AsVector128());
        return new Vector2(result.AsVector2());
    }

    /// <summary>Compares two vectors to determine the combined minimum.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>The combined minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Min(Vector2 left, Vector2 right)
    {
        var result = VectorUtilities.Min(left._value.AsVector128(), right._value.AsVector128());
        return new Vector2(result.AsVector2());
    }

    /// <summary>Computes the normalized form of a vector.</summary>
    /// <param name="value">The vector to normalized.</param>
    /// <returns>The normalized form of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Normalize(Vector2 value)
    {
        var result = SysVector2.Normalize(value._value);
        return new Vector2(result);
    }

    /// <summary>Computes an estimate of the normalized form of a vector.</summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>An estimate of the normalized form of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 NormalizeEstimate(Vector2 value)
    {
        var result = VectorUtilities.NormalizeEstimate(value._value.AsVector128());
        return new Vector2(result.AsVector2());
    }

    /// <summary>Computes an estimate of the reciprocal of a vector.</summary>
    /// <param name="value">The vector for which to compute the reciprocal.</param>
    /// <returns>An estimate of the element-wise reciprocal of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ReciprocalEstimate(Vector2 value)
    {
        var result = VectorUtilities.ReciprocalEstimate(value._value.AsVector128());
        return new Vector2(result.AsVector2());
    }

    /// <summary>Computes an estimate of the reciprocal square-root of a vector.</summary>
    /// <param name="value">The vector for which to compute the reciprocal square-root.</param>
    /// <returns>An estimate of the element-wise reciprocal square-root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ReciprocalSqrtEstimate(Vector2 value)
    {
        var result = VectorUtilities.ReciprocalSqrtEstimate(value._value.AsVector128());
        return new Vector2(result.AsVector2());
    }

    /// <summary>Computes the square-root of a vector.</summary>
    /// <param name="value">The vector for which to compute the square-root.</param>
    /// <returns>The element-wise square-root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Sqrt(Vector2 value)
    {
        var result = VectorUtilities.Sqrt(value._value.AsVector128());
        return new Vector2(result.AsVector2());
    }

    /// <summary>Reinterprets the current instance as a new <see cref="SysVector2" />.</summary>
    /// <returns>The current instance reinterpreted as a new <see cref="SysVector2" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SysVector2 AsSystemVector2() => _value;

    /// <summary>Reinterprets the current instance as a new <see cref="Vector128{Single}" />.</summary>
    /// <returns>The current instance reinterpreted as a new <see cref="Vector128{Single}" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector128<float> AsVector128() => _value.AsVector128();

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Vector2 other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(Vector2 other)
    {
        return X.Equals(other.X)
            && Y.Equals(other.Y);
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format = null, IFormatProvider? formatProvider = null)
        => $"Vector2 {{ X = {X.ToString(format, formatProvider)}, Y = {Y.ToString(format, formatProvider)} }}";

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"Vector2 { X = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        var partLength = "Vector2 { X = ".Length;

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

        if (!"Vector2 { X = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        var partLength = "Vector2 { X = "u8.Length;

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
    public Vector2 WithX(float x)
    {
        var result = _value;
        result.X = x;
        return new Vector2(result);
    }

    /// <summary>Creates a new vector with <see cref="Y" /> set to the specified value.</summary>
    /// <param name="y">The new y-component of the vector.</param>
    /// <returns>A new vector with <see cref="Y" /> set to <paramref name="y" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 WithY(float y)
    {
        var result = _value;
        result.Y = y;
        return new Vector2(result);
    }
}
