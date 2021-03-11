// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a three-dimensional Euclidean vector.</summary>
    public readonly struct Vector3 : IEquatable<Vector3>, IEqualEstimate<Vector3>, IFormattable
    {
        /// <summary>Defines a <see cref="Vector3" /> where all components are zero.</summary>
        public static readonly Vector3 Zero = new Vector3(0.0f, 0.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector3" /> whose x-component is one and whose remaining components are zero.</summary>
        public static readonly Vector3 UnitX = new Vector3(1.0f, 0.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector3" /> whose y-component is one and whose remaining components are zero.</summary>
        public static readonly Vector3 UnitY = new Vector3(0.0f, 1.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector3" /> whose z-component is one and whose remaining components are zero.</summary>
        public static readonly Vector3 UnitZ = new Vector3(0.0f, 0.0f, 1.0f);

        /// <summary>Defines a <see cref="Vector3" /> where all components are one.</summary>
        public static readonly Vector3 One = new Vector3(1.0f, 1.0f, 1.0f);

        private readonly float _x;
        private readonly float _y;
        private readonly float _z;

        /// <summary>Initializes a new instance of the <see cref="Vector3" /> struct.</summary>
        /// <param name="x">The value of the x-dimension.</param>
        /// <param name="y">The value of the y-dimension.</param>
        /// <param name="z">The value of the z-dimension.</param>
        public Vector3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>Initializes a new instance of the <see cref="Vector3" /> struct with each component set to <paramref name="value" />.</summary>
        /// <param name="value">The value to set each component to.</param>
        public Vector3(float value)
        {
            _x = value;
            _y = value;
            _z = value;
        }

        /// <summary>Gets the value of the x-dimension.</summary>
        public float X => _x;

        /// <summary>Gets the value of the y-dimension.</summary>
        public float Y => _y;

        /// <summary>Gets the value of the z-dimension.</summary>
        public float Z => _z;

        /// <summary>Gets the square-rooted length of the vector.</summary>
        public float Length => MathUtilities.Sqrt(LengthSquared);

        /// <summary>Gets the squared length of the vector.</summary>
        public float LengthSquared => Dot(this, this);

        /// <summary>Compares two vectors to determine equality.</summary>
        /// <param name="left">The vector to compare with <paramref name="right" />.</param>
        /// <param name="right">The vector to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector3 left, Vector3 right)
            => (left.X == right.X)
            && (left.Y == right.Y)
            && (left.Z == right.Z);

        /// <summary>Compares two vectors to determine inequality.</summary>
        /// <param name="left">The vector to compare with <paramref name="right" />.</param>
        /// <param name="right">The vector to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector3 left, Vector3 right)
            => (left.X != right.X)
            || (left.Y != right.Y)
            || (left.Z != right.Z);

        /// <summary>Computes the value of a vector.</summary>
        /// <param name="value">The vector.</param>
        /// <returns><paramref name="value" /></returns>
        public static Vector3 operator +(Vector3 value) => value;

        /// <summary>Computes the negation of a vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negation of <paramref name="value" />.</returns>
        public static Vector3 operator -(Vector3 value) => value * -1;

        /// <summary>Computes the sum of two vectors.</summary>
        /// <param name="left">The vector to which to add <paramref name="right" />.</param>
        /// <param name="right">The vector which is added to <paramref name="left" />.</param>
        /// <returns>The sum of <paramref name="right" /> added to <paramref name="left" />.</returns>
        public static Vector3 operator +(Vector3 left, Vector3 right) => new Vector3(
            left.X + right.X,
            left.Y + right.Y,
            left.Z + right.Z
        );

        /// <summary>Computes the difference of two vectors.</summary>
        /// <param name="left">The vector from which to subtract <paramref name="right" />.</param>
        /// <param name="right">The vector which is subtracted from <paramref name="left" />.</param>
        /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
        public static Vector3 operator -(Vector3 left, Vector3 right) => new Vector3(
            left.X - right.X,
            left.Y - right.Y,
            left.Z - right.Z
        );

        /// <summary>Computes the product of a vector and a float.</summary>
        /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
        /// <param name="right">The float which is used to multiply <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static Vector3 operator *(Vector3 left, float right) => new Vector3(
            left.X * right,
            left.Y * right,
            left.Z * right
        );

        /// <summary>Computes the product of two vectors.</summary>
        /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
        /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static Vector3 operator *(Vector3 left, Vector3 right) => new Vector3(
            left.X * right.X,
            left.Y * right.Y,
            left.Z * right.Z
        );

        /// <summary>Computes the product of a vector and matrix.</summary>
        /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
        /// <param name="right">The matrix which is used to multiply <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static Vector3 operator *(Vector3 left, Matrix3x3 right) => Transform(left, right);

        /// <summary>Computes the quotient of a vector and a float.</summary>
        /// <param name="left">The vector which is divied by <paramref name="right" />.</param>
        /// <param name="right">The float which divides <paramref name="left" />.</param>
        /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static Vector3 operator /(Vector3 left, float right) => new Vector3(
            left.X / right,
            left.Y / right,
            left.Z / right
        );

        /// <summary>Computes the quotient of two vectors.</summary>
        /// <param name="left">The vector which is divied by <paramref name="right" />.</param>
        /// <param name="right">The vector which divides <paramref name="left" />.</param>
        /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static Vector3 operator /(Vector3 left, Vector3 right) => new Vector3(
            left.X / right.X,
            left.Y / right.Y,
            left.Z / right.Z
        );

        /// <summary>Computes the cross product of two vectors.</summary>
        /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
        /// <param name="right">The quatnerion which is used to multiply <paramref name="left" />.</param>
        /// <returns>The cross product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static Vector3 Cross(Vector3 left, Vector3 right) => new Vector3(
            (left.Y * right.Z) - (left.Z * right.Y),
            (left.Z * right.X) - (left.X * right.Z),
            (left.X * right.Y) - (left.Y * right.X)
        );

        /// <summary>Computes the dot product of two vectors.</summary>
        /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
        /// <param name="right">The quatnerion which is used to multiply <paramref name="left" />.</param>
        /// <returns>The dot product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static float Dot(Vector3 left, Vector3 right)
            => (left.X * right.X)
             + (left.Y * right.Y)
             + (left.Z * right.Z);

        /// <summary>Compares two vectors to determine approximate equality.</summary>
        /// <param name="left">The vector to compare with <paramref name="right" />.</param>
        /// <param name="right">The vector to compare with <paramref name="left" />.</param>
        /// <param name="epsilon">The maximum (exclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
        public static bool EqualsEstimate(Vector3 left, Vector3 right, Vector3 epsilon)
            => MathUtilities.EqualsEstimate(left.X, right.X, epsilon.X)
            && MathUtilities.EqualsEstimate(left.Y, right.Y, epsilon.Y)
            && MathUtilities.EqualsEstimate(left.Z, right.Z, epsilon.Z);

        /// <summary>Compares two vectors to determine the combined maximum.</summary>
        /// <param name="left">The vector to compare with <paramref name="right" />.</param>
        /// <param name="right">The vector to compare with <paramref name="left" />.</param>
        /// <returns>The combined maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Vector3 Max(Vector3 left, Vector3 right) => new Vector3(
            MathUtilities.Max(left.X, right.X),
            MathUtilities.Max(left.Y, right.Y),
            MathUtilities.Max(left.Z, right.Z)
        );

        /// <summary>Tests if two <see cref="Vector3" /> instances (this and right) have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="right">The right instance to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns><c>True</c> if similar, <c>False</c> otherwise.</returns>
        public bool EqualEstimate(Vector3 right, Vector3 epsilon)
        {
            return FloatUtilities.EqualEstimate(X, right.X, epsilon.X)
                && FloatUtilities.EqualEstimate(Y, right.Y, epsilon.Y)
                && FloatUtilities.EqualEstimate(Z, right.Z, epsilon.Z);
        }

        /// <summary>Computes the normalized form of a vector.</summary>
        /// <param name="value">The vector to normalized.</param>
        /// <returns>The normalized form of <paramref name="value" />.</returns>
        public static Vector3 Normalize(Vector3 value) => value / value.Length;

        /// <summary>Rotates a vector using a quaternion.</summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns><paramref name="value" /> rotated by <paramref name="rotation" />.</returns>
        public static Vector3 Rotate(Vector3 value, Quaternion rotation) => value * Matrix3x3.CreateFromRotation(rotation);

        /// <summary>Rotates a vector using the inverse of a quaternion.</summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns><paramref name="value" /> rotated by the inverse of <paramref name="rotation" />.</returns>
        public static Vector3 RotateInverse(Vector3 value, Quaternion rotation) => value * Matrix3x3.CreateFromRotation(Quaternion.Invert(rotation));

        /// <summary>Transforms a vector using a matrix.</summary>
        /// <param name="value">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns><paramref name="value" /> transformed by <paramref name="matrix" />.</returns>
        public static Vector3 Transform(Vector3 value, Matrix3x3 matrix) => new Vector3(
            (value.X * matrix.X.X) + (value.Y * matrix.Y.X) + (value.Z * matrix.Z.X),
            (value.X * matrix.X.Y) + (value.Y * matrix.Y.Y) + (value.Z * matrix.Z.Y),
            (value.X * matrix.X.Z) + (value.Y * matrix.Y.Z) + (value.Z * matrix.Z.Z)
        );

        /// <summary>Transforms a vector using a matrix.</summary>
        /// <param name="value">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns><paramref name="value" /> transformed by <paramref name="matrix" />.</returns>
        public static Vector3 Transform(Vector3 value, Matrix4x4 matrix) => new Vector3(
            (value.X * matrix.X.X) + (value.Y * matrix.Y.X) + (value.Z * matrix.Z.X) + matrix.W.X,
            (value.X * matrix.X.Y) + (value.Y * matrix.Y.Y) + (value.Z * matrix.Z.Y) + matrix.W.Y,
            (value.X * matrix.X.Z) + (value.Y * matrix.Y.Z) + (value.Z * matrix.Z.Z) + matrix.W.Z
        );

        /// <summary>Transforms a vector using a normalized matrix.</summary>
        /// <param name="value">The vector to transform.</param>
        /// <param name="matrix">The normalized transformation matrix.</param>
        /// <returns><paramref name="value" /> transformed by <paramref name="matrix" />.</returns>
        public static Vector3 TransformNormal(Vector3 value, Matrix4x4 matrix) => new Vector3(
            (value.X * matrix.X.X) + (value.Y * matrix.Y.X) + (value.Z * matrix.Z.X),
            (value.X * matrix.X.Y) + (value.Y * matrix.Y.Y) + (value.Z * matrix.Z.Y),
            (value.X * matrix.X.Z) + (value.Y * matrix.Y.Z) + (value.Z * matrix.Z.Z)
        );

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Vector3 other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Vector3 other) => this == other;

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
        public Vector3 WithX(float x) => new Vector3(x, Y, Z);

        /// <summary>Creates a new <see cref="Vector3" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="y">The new value of the y-dimension.</param>
        /// <returns>A new <see cref="Vector3" /> instance with <see cref="Y" /> set to <paramref name="y" />.</returns>
        public Vector3 WithY(float y) => new Vector3(X, y, Z);

        /// <summary>Creates a new <see cref="Vector3" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="z">The new value of the z-dimension.</param>
        /// <returns>A new <see cref="Vector3" /> instance with <see cref="Z" /> set to <paramref name="z" />.</returns>
        public Vector3 WithZ(float z) => new Vector3(X, Y, z);
    }
}
