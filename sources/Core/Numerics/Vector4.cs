// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a four-dimensional Euclidean vector.</summary>
    public readonly struct Vector4 : IEquatable<Vector4>, IFormattable
    {
        /// <summary>Defines a <see cref="Vector4" /> where all components are zero.</summary>
        public static readonly Vector4 Zero = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector4" /> whose x-component is one and whose remaining components are zero.</summary>
        public static readonly Vector4 UnitX = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector4" /> whose y-component is one and whose remaining components are zero.</summary>
        public static readonly Vector4 UnitY = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector4" /> whose z-component is one and whose remaining components are zero.</summary>
        public static readonly Vector4 UnitZ = new Vector4(0.0f, 0.0f, 1.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector4" /> whose w-component is one and whose remaining components are zero.</summary>
        public static readonly Vector4 UnitW = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>Defines a <see cref="Vector4" /> where all components are one.</summary>
        public static readonly Vector4 One = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

        private readonly float _x;
        private readonly float _y;
        private readonly float _z;
        private readonly float _w;

        /// <summary>Initializes a new instance of the <see cref="Vector4" /> struct.</summary>
        /// <param name="x">The value of the x-dimension.</param>
        /// <param name="y">The value of the y-dimension.</param>
        /// <param name="z">The value of the z-dimension.</param>
        /// <param name="w">The value of the w-dimension.</param>
        public Vector4(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        /// <summary>Initializes a new instance of the <see cref="Vector4" /> struct with each component set to <paramref name="value" />.</summary>
        /// <param name="value">The value to set each component to.</param>
        public Vector4(float value)
        {
            _x = value;
            _y = value;
            _z = value;
            _w = value;
        }

        /// <summary>Initializes a new instance of the <see cref="Vector4" /> struct.</summary>
        /// <param name="vector">The value of the x,y and z-dimensions.</param>
        /// <param name="w">The value of the w-dimension.</param>
        public Vector4(Vector3 vector, float w)
        {
            _x = vector.X;
            _y = vector.Y;
            _z = vector.Z;
            _w = w;
        }

        /// <summary>Gets the value of the x-dimension.</summary>
        public float X => _x;

        /// <summary>Gets the value of the y-dimension.</summary>
        public float Y => _y;

        /// <summary>Gets the value of the z-dimension.</summary>
        public float Z => _z;

        /// <summary>Gets the value of the w-dimension.</summary>
        public float W => _w;

        /// <summary>Gets the square-rooted length of the vector.</summary>
        public float Length => MathUtilities.Sqrt(LengthSquared);

        /// <summary>Gets the squared length of the vector.</summary>
        public float LengthSquared => Dot(this, this);

        /// <summary>Compares two vectors to determine equality.</summary>
        /// <param name="left">The vector to compare with <paramref name="right" />.</param>
        /// <param name="right">The vector to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector4 left, Vector4 right)
            => (left.X == right.X)
            && (left.Y == right.Y)
            && (left.Z == right.Z)
            && (left.W == right.W);

        /// <summary>Compares two vectors to determine inequality.</summary>
        /// <param name="left">The vector to compare with <paramref name="right" />.</param>
        /// <param name="right">The vector to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector4 left, Vector4 right)
            => (left.X != right.X)
            || (left.Y != right.Y)
            || (left.Z != right.Z)
            || (left.W != right.W);

        /// <summary>Computes the value of a vector.</summary>
        /// <param name="value">The vector.</param>
        /// <returns><paramref name="value" /></returns>
        public static Vector4 operator +(Vector4 value) => value;

        /// <summary>Computes the negation of a vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negation of <paramref name="value" />.</returns>
        public static Vector4 operator -(Vector4 value) => value * -1;

        /// <summary>Computes the sum of two vectors.</summary>
        /// <param name="left">The vector to which to add <paramref name="right" />.</param>
        /// <param name="right">The vector which is added to <paramref name="left" />.</param>
        /// <returns>The sum of <paramref name="right" /> added to <paramref name="left" />.</returns>
        public static Vector4 operator +(Vector4 left, Vector4 right) => new Vector4(
            left.X + right.X,
            left.Y + right.Y,
            left.Z + right.Z,
            left.W + right.W
        );

        /// <summary>Computes the difference of two vectors.</summary>
        /// <param name="left">The vector from which to subtract <paramref name="right" />.</param>
        /// <param name="right">The vector which is subtracted from <paramref name="left" />.</param>
        /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
        public static Vector4 operator -(Vector4 left, Vector4 right) => new Vector4(
            left.X - right.X,
            left.Y - right.Y,
            left.Z - right.Z,
            left.W - right.W
        );

        /// <summary>Computes the product of a vector and a float.</summary>
        /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
        /// <param name="right">The float which is used to multiply <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static Vector4 operator *(Vector4 left, float right) => new Vector4(
            left.X * right,
            left.Y * right,
            left.Z * right,
            left.W * right
        );

        /// <summary>Computes the product of two vectors.</summary>
        /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
        /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static Vector4 operator *(Vector4 left, Vector4 right) => new Vector4(
            left.X * right.X,
            left.Y * right.Y,
            left.Z * right.Z,
            left.W * right.W
        );

        /// <summary>Computes the product of a vector and matrix.</summary>
        /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
        /// <param name="right">The matrix which is used to multiply <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static Vector4 operator *(Vector4 left, Matrix4x4 right) => Transform(left, right);

        /// <summary>Computes the quotient of a vector and a float.</summary>
        /// <param name="left">The vector which is divied by <paramref name="right" />.</param>
        /// <param name="right">The float which divides <paramref name="left" />.</param>
        /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static Vector4 operator /(Vector4 left, float right) => new Vector4(
            left.X / right,
            left.Y / right,
            left.Z / right,
            left.W / right
        );

        /// <summary>Computes the quotient of two vectors.</summary>
        /// <param name="left">The vector which is divied by <paramref name="right" />.</param>
        /// <param name="right">The vector which divides <paramref name="left" />.</param>
        /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static Vector4 operator /(Vector4 left, Vector4 right) => new Vector4(
            left.X / right.X,
            left.Y / right.Y,
            left.Z / right.Z,
            left.W / right.W
        );

        /// <summary>Computes the dot product of two vectors.</summary>
        /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
        /// <param name="right">The quatnerion which is used to multiply <paramref name="left" />.</param>
        /// <returns>The dot product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static float Dot(Vector4 left, Vector4 right)
            => (left.X * right.X)
             + (left.Y * right.Y)
             + (left.Z * right.Z)
             + (left.W * right.W);

        /// <summary>Compares two vectors to determine approximate equality.</summary>
        /// <param name="left">The vector to compare with <paramref name="right" />.</param>
        /// <param name="right">The vector to compare with <paramref name="left" />.</param>
        /// <param name="epsilon">The maximum (exclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
        public static bool EqualsEstimate(Vector4 left, Vector4 right, Vector4 epsilon)
            => MathUtilities.EqualsEstimate(left.X, right.X, epsilon.X)
            && MathUtilities.EqualsEstimate(left.Y, right.Y, epsilon.Y)
            && MathUtilities.EqualsEstimate(left.Z, right.Z, epsilon.Z)
            && MathUtilities.EqualsEstimate(left.W, right.W, epsilon.W);

        /// <summary>Compares two vectors to determine the combined maximum.</summary>
        /// <param name="left">The vector to compare with <paramref name="right" />.</param>
        /// <param name="right">The vector to compare with <paramref name="left" />.</param>
        /// <returns>The combined maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Vector4 Max(Vector4 left, Vector4 right) => new Vector4(
            MathUtilities.Max(left.X, right.X),
            MathUtilities.Max(left.Y, right.Y),
            MathUtilities.Max(left.Z, right.Z),
            MathUtilities.Min(left.W, right.W)
        );

        /// <summary>Compares two vectors to determine the combined minimum.</summary>
        /// <param name="left">The vector to compare with <paramref name="right" />.</param>
        /// <param name="right">The vector to compare with <paramref name="left" />.</param>
        /// <returns>The combined minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Vector4 Min(Vector4 left, Vector4 right) => new Vector4(
            MathUtilities.Min(left.X, right.X),
            MathUtilities.Min(left.Y, right.Y),
            MathUtilities.Min(left.Z, right.Z),
            MathUtilities.Min(left.W, right.W)
        );

        /// <summary>Computes the normalized form of a vector.</summary>
        /// <param name="value">The vector to normalized.</param>
        /// <returns>The normalized form of <paramref name="value" />.</returns>
        public static Vector4 Normalize(Vector4 value) => value / value.Length;

        /// <summary>Transforms a vector using a matrix.</summary>
        /// <param name="value">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns><paramref name="value" /> transformed by <paramref name="matrix" />.</returns>
        public static Vector4 Transform(Vector4 value, Matrix4x4 matrix) => new Vector4(
            (value.X * matrix.X.X) + (value.Y * matrix.Y.X) + (value.Z * matrix.Z.X) + (value.W * matrix.W.X),
            (value.X * matrix.X.Y) + (value.Y * matrix.Y.Y) + (value.Z * matrix.Z.Y) + (value.W * matrix.W.Y),
            (value.X * matrix.X.Z) + (value.Y * matrix.Y.Z) + (value.Z * matrix.Z.Z) + (value.W * matrix.W.Z),
            (value.X * matrix.X.W) + (value.Y * matrix.Y.W) + (value.Z * matrix.Z.W) + (value.W * matrix.W.W)
        );

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Vector4 other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Vector4 other) => this == other;

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
        public Vector4 WithX(float x) => new Vector4(x, Y, Z, W);

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="y">The new value of the y-dimension.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="Y" /> set to <paramref name="y" />.</returns>
        public Vector4 WithY(float y) => new Vector4(X, y, Z, W);

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="z">The new value of the z-dimension.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="Z" /> set to <paramref name="z" />.</returns>
        public Vector4 WithZ(float z) => new Vector4(X, Y, z, W);

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="W" /> set to the specified value.</summary>
        /// <param name="w">The new value of the w-dimension.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="W" /> set to <paramref name="w" />.</returns>
        public Vector4 WithW(float w) => new Vector4(X, Y, Z, w);
    }
}
