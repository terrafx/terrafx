// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX.Numerics
{
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


        /// <summary>Gets the square-rooted length of the vector.</summary>
        public float Length => MathF.Sqrt(LengthSquared);

        /// <summary>Gets the squared length of the vector.</summary>
        public float LengthSquared => Dot(this, this);


        /// <summary>Compares two <see cref="Vector2" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Vector2" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Vector2" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y);
        }

        /// <summary>Compares two <see cref="Vector2" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Vector2" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Vector2" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return (left.X != right.X)
                || (left.Y != right.Y);
        }

        /// <summary>Returns the value of the <see cref="Vector2" /> operand (the sign of the operand is unchanged).</summary>
        /// <param name="value">The operand to return</param>
        /// <returns>The value of the operand, <paramref name="value" />.</returns>
        public static Vector2 operator +(Vector2 value) => value;

        /// <summary>Negates the value of the specified <see cref="Vector2" /> operand.</summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The result of <paramref name="value" /> multiplied by negative one (-1).</returns>
        public static Vector2 operator -(Vector2 value) => value * -1;

        /// <summary>Adds two specified <see cref="Vector2" /> values.</summary>
        /// <param name="left">The first value to add.</param>
        /// <param name="right">The second value to add.</param>
        /// <returns>The result of adding <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Vector2 operator +(Vector2 left, Vector2 right) => new Vector2(left.X + right.X, left.Y + right.Y);

        /// <summary>Subtracts two specified <see cref="Vector2" /> values.</summary>
        /// <param name="left">The minuend.</param>
        /// <param name="right">The subtrahend.</param>
        /// <returns>The result of subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        public static Vector2 operator -(Vector2 left, Vector2 right) => new Vector2(left.X - right.X, left.Y - right.Y);

        /// <summary>Multiplies two specified <see cref="Vector2" /> values.</summary>
        /// <param name="left">The first value to multiply.</param>
        /// <param name="right">The second value to multiply.</param>
        /// <returns>The result of multiplying <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Vector2 operator *(Vector2 left, Vector2 right) => new Vector2(left.X * right.X, left.Y * right.Y);

        /// <summary>Divides two specified <see cref="Vector2" /> values.</summary>
        /// <param name="left">The dividend.</param>
        /// <param name="right">The divisor.</param>
        /// <returns>The result of dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Vector2 operator /(Vector2 left, Vector2 right) => new Vector2(left.X / right.X, left.Y / right.Y);

        /// <summary>Multiplies each component of a <see cref="Vector2" /> value by a given <see cref="float" /> value.</summary>
        /// <param name="left">The vector to multiply.</param>
        /// <param name="right">The value to multiply each component by.</param>
        /// <returns>The result of multiplying each component of <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Vector2 operator *(Vector2 left, float right) => new Vector2(left.X * right, left.Y * right);

        /// <summary>Divides each component of a <see cref="Vector2" /> value by a given <see cref="float" /> value.</summary>
        /// <param name="left">The dividend.</param>
        /// <param name="right">The divisor to divide each component by.</param>
        /// <returns>The result of multiplying each component of <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Vector2 operator /(Vector2 left, float right) => new Vector2(left.X / right, left.Y / right);

        /// <summary>Calculates the dot product of two <see cref="Vector2" /> values.</summary>
        /// <param name="left">The first value to dot.</param>
        /// <param name="right">The second value to dot.</param>
        /// <returns>The result of adding the multiplication of each component of <paramref name="left" /> by each component of <paramref name="right" />.</returns>
        public static float Dot(Vector2 left, Vector2 right) => (left.X * right.X) + (left.Y * right.Y);

        /// <summary>Computes the normalized value of the given <see cref="Vector2" /> value.</summary>
        /// <param name="value">The value to normalize.</param>
        /// <returns>The unit vector of <paramref name="value" />.</returns>
        public static Vector2 Normalize(Vector2 value) => value / value.Length;

        /// <summary>Computes the <see cref="Vector2" /> that for each component has the maximum value out of this and v.</summary>
        /// <param name="v">The <see cref="Vector2" /> for this operation.</param>
        /// <param name="other">The other <see cref="Vector2" /> to compute the max with.</param>
        /// <returns>The resulting new instance.</returns>
        public static Vector2 Max(Vector2 v, Vector2 other) => new Vector2(MathF.Max(v.X, other.X), MathF.Max(v.Y, other.Y));

        /// <summary>Computes the <see cref="Vector2" /> that for each component has the minimum value out of this and v.</summary>
        /// <param name="v">The <see cref="Vector2" /> for this operation.</param>
        /// <param name="other">The other <see cref="Vector2" /> to compute the min with.</param>
        /// <returns>The resulting new instance.</returns>
        public static Vector2 Min(Vector2 v, Vector2 other) => new Vector2(MathF.Min(v.X, other.X), MathF.Min(v.Y, other.Y));

        /// <summary>Creates a new <see cref="Vector2" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-dimension.</param>
        /// <returns>A new <see cref="Vector2" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Vector2 WithX(float value) => new Vector2(value, Y);

        /// <summary>Creates a new <see cref="Vector2" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-dimension.</param>
        /// <returns>A new <see cref="Vector2" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Vector2 WithY(float value) => new Vector2(X, value);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Vector2 other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Vector2 other) => this == other;

        /// <summary>Tests if two <see cref="Vector2" /> instances have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="left">The left instance to compare.</param>
        /// <param name="right">The right instance to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns><c>True</c> if similar, <c>False</c> otherwise.</returns>
        public static bool EqualEstimate(Vector2 left, Vector2 right, Vector2 epsilon)
        {
            return FloatUtilities.EqualEstimate(left.X, right.X, epsilon.X)
                && FloatUtilities.EqualEstimate(left.Y, right.Y, epsilon.Y);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(X);
                hashCode.Add(Y);
            }
            return hashCode.ToHashCode();
        }

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
    }
}
