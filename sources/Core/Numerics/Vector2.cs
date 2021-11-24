// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX.Numerics;

/// <summary>Defines a two-dimensional Euclidean vector.</summary>
public readonly struct Vector2 : IEquatable<Vector2>, IFormattable
{
    /// <summary>Defines a <see cref="Vector2" /> where all components are zero.</summary>
    public static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);

    /// <summary>Defines a <see cref="Vector2" /> whose x-component is one and whose remaining components are zero.</summary>
    public static readonly Vector2 UnitX = new Vector2(1.0f, 0.0f);

    /// <summary>Defines a <see cref="Vector2" /> whose y-component is one and whose remaining components are zero.</summary>
    public static readonly Vector2 UnitY = new Vector2(0.0f, 1.0f);

    /// <summary>Defines a <see cref="Vector2" /> where all components are one.</summary>
    public static readonly Vector2 One = new Vector2(1.0f, 1.0f);

    private readonly float _x;
    private readonly float _y;

    /// <summary>Initializes a new instance of the <see cref="Vector2" /> struct.</summary>
    /// <param name="x">The value of the x-dimension.</param>
    /// <param name="y">The value of the y-dimension.</param>
    public Vector2(float x, float y)
    {
        _x = x;
        _y = y;
    }

    /// <summary>Initializes a new instance of the <see cref="Vector2" /> struct with each component set to <paramref name="value" />.</summary>
    /// <param name="value">The value to set each component to.</param>
    public Vector2(float value)
    {
        _x = value;
        _y = value;
    }

    /// <summary>Gets the value of the x-dimension.</summary>
    public float X => _x;

    /// <summary>Gets the value of the y-dimension.</summary>
    public float Y => _y;


    /// <summary>Gets the length of the vector.</summary>
    public float Length => MathUtilities.Sqrt(LengthSquared);

    /// <summary>Gets the squared length of the vector.</summary>
    public float LengthSquared => Dot(this, this);


    /// <summary>Compares two vectors to determine equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Vector2 left, Vector2 right)
        => (left.X == right.X)
        && (left.Y == right.Y);

    /// <summary>Compares two vectors to determine inequality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Vector2 left, Vector2 right)
        => (left.X != right.X)
        || (left.Y != right.Y);

    /// <summary>Computes the value of a vector.</summary>
    /// <param name="value">The vector.</param>
    /// <returns><paramref name="value" /></returns>
    public static Vector2 operator +(Vector2 value) => value;

    /// <summary>Computes the negation of a vector.</summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negation of <paramref name="value" />.</returns>
    public static Vector2 operator -(Vector2 value) => value * -1;

    /// <summary>Computes the sum of two vectors.</summary>
    /// <param name="left">The vector to which to add <paramref name="right" />.</param>
    /// <param name="right">The vector which is added to <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="right" /> added to <paramref name="left" />.</returns>
    public static Vector2 operator +(Vector2 left, Vector2 right) => new Vector2(
        left.X + right.X,
        left.Y + right.Y
    );

    /// <summary>Computes the difference of two vectors.</summary>
    /// <param name="left">The vector from which to subtract <paramref name="right" />.</param>
    /// <param name="right">The vector which is subtracted from <paramref name="left" />.</param>
    /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
    public static Vector2 operator -(Vector2 left, Vector2 right) => new Vector2(
        left.X - right.X,
        left.Y - right.Y
    );

    /// <summary>Computes the product of a vector and a float.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The float which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    public static Vector2 operator *(Vector2 left, float right) => new Vector2(
        left.X * right,
        left.Y * right
    );

    /// <summary>Computes the product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    public static Vector2 operator *(Vector2 left, Vector2 right) => new Vector2(
        left.X * right.X,
        left.Y * right.Y
    );

    /// <summary>Computes the quotient of a vector and a float.</summary>
    /// <param name="left">The vector which is divied by <paramref name="right" />.</param>
    /// <param name="right">The float which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    public static Vector2 operator /(Vector2 left, float right) => new Vector2(
        left.X / right,
        left.Y / right
    );

    /// <summary>Computes the quotient of two vectors.</summary>
    /// <param name="left">The vector which is divied by <paramref name="right" />.</param>
    /// <param name="right">The vector which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    public static Vector2 operator /(Vector2 left, Vector2 right) => new Vector2(
        left.X / right.X,
        left.Y / right.Y
    );

    /// <summary>Computes the dot product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The quatnerion which is used to multiply <paramref name="left" />.</param>
    /// <returns>The dot product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    public static float Dot(Vector2 left, Vector2 right)
        => (left.X * right.X)
         + (left.Y * right.Y);

    /// <summary>Compares two vectors to determine approximate equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (exclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    public static bool EqualsEstimate(Vector2 left, Vector2 right, Vector2 epsilon)
        => MathUtilities.EqualsEstimate(left.X, right.X, epsilon.X)
        && MathUtilities.EqualsEstimate(left.Y, right.Y, epsilon.Y);

    /// <summary>Compares two vectors to determine the combined maximum.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>The combined maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    public static Vector2 Max(Vector2 left, Vector2 right) => new Vector2(
        MathUtilities.Max(left.X, right.X),
        MathUtilities.Max(left.Y, right.Y)
    );

    /// <summary>Compares two vectors to determine the combined minimum.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>The combined minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    public static Vector2 Min(Vector2 left, Vector2 right) => new Vector2(
        MathUtilities.Min(left.X, right.X),
        MathUtilities.Min(left.Y, right.Y)
    );

    /// <summary>Computes the normalized form of a vector.</summary>
    /// <param name="value">The vector to normalized.</param>
    /// <returns>The normalized form of <paramref name="value" />.</returns>
    public static Vector2 Normalize(Vector2 value) => value / value.Length;

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Vector2 other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(Vector2 other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return new StringBuilder(5 + separator.Length)
            .Append('<')
            .Append(X.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append(Y.ToString(format, formatProvider))
            .Append('>')
            .ToString();
    }

    /// <summary>Creates a new <see cref="Vector2" /> instance with <see cref="X" /> set to the specified value.</summary>
    /// <param name="x">The new value of the x-dimension.</param>
    /// <returns>A new <see cref="Vector2" /> instance with <see cref="X" /> set to <paramref name="x" />.</returns>
    public Vector2 WithX(float x) => new Vector2(x, Y);

    /// <summary>Creates a new <see cref="Vector2" /> instance with <see cref="Y" /> set to the specified value.</summary>
    /// <param name="y">The new value of the y-dimension.</param>
    /// <returns>A new <see cref="Vector2" /> instance with <see cref="Y" /> set to <paramref name="y" />.</returns>
    public Vector2 WithY(float y) => new Vector2(X, y);
}
