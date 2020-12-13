// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

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
        /// <param name="v3">The value of the x,y and z-dimensions.</param>
        /// <param name="w">The value of the w-dimension.</param>
        public Vector4(Vector3 v3, float w)
        {
            _x = v3.X;
            _y = v3.Y;
            _z = v3.Z;
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
        /// <param name="vector">The <see cref="Vector4" /> for this operation.</param>
        public static float Length(Vector4 vector) => MathF.Sqrt(Vector4.LengthSquared(vector));

        /// <summary>Gets the squared length of the vector.</summary>
        /// <param name="vector">The <see cref="Vector4" /> for this operation.</param>
        public static float LengthSquared(Vector4 vector) => Dot(vector, vector);

        /// <summary>Compares two <see cref="Vector4" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Vector4" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Vector4" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector4 left, Vector4 right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y)
                && (left.Z == right.Z)
                && (left.W == right.W);
        }

        /// <summary>Compares two <see cref="Vector4" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Vector4" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Vector4" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector4 left, Vector4 right)
        {
            return (left.X != right.X)
                || (left.Y != right.Y)
                || (left.Z != right.Z)
                || (left.W != right.W);
        }

        /// <summary>Returns the value of the <see cref="Vector4" /> operand (the sign of the operand is unchanged).</summary>
        /// <param name="value">The operand to return</param>
        /// <returns>The value of the operand, <paramref name="value" />.</returns>
        public static Vector4 operator +(Vector4 value) => value;

        /// <summary>Negates the value of the specified <see cref="Vector4" /> operand.</summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The result of <paramref name="value" /> multiplied by negative one (-1).</returns>
        public static Vector4 operator -(Vector4 value) => value * -1;

        /// <summary>Adds two specified <see cref="Vector4" /> values.</summary>
        /// <param name="left">The first value to add.</param>
        /// <param name="right">The second value to add.</param>
        /// <returns>The result of adding <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Vector4 operator +(Vector4 left, Vector4 right) => new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);

        /// <summary>Subtracts two specified <see cref="Vector4" /> values.</summary>
        /// <param name="left">The minuend.</param>
        /// <param name="right">The subtrahend.</param>
        /// <returns>The result of subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        public static Vector4 operator -(Vector4 left, Vector4 right) => new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);

        /// <summary>Multiplies two specified <see cref="Vector4" /> values.</summary>
        /// <param name="left">The first value to multiply.</param>
        /// <param name="right">The second value to multiply.</param>
        /// <returns>The result of multiplying <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Vector4 operator *(Vector4 left, Vector4 right) => new Vector4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);

        /// <summary>Divides two specified <see cref="Vector4" /> values.</summary>
        /// <param name="left">The dividend.</param>
        /// <param name="right">The divisor.</param>
        /// <returns>The result of dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Vector4 operator /(Vector4 left, Vector4 right) => new Vector4(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);

        /// <summary>Multiplies each component of a <see cref="Vector4" /> value by a given <see cref="float" /> value.</summary>
        /// <param name="left">The vector to multiply.</param>
        /// <param name="right">The value to multiply each component by.</param>
        /// <returns>The result of multiplying each component of <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Vector4 operator *(Vector4 left, float right) => new Vector4(left.X * right, left.Y * right, left.Z * right, left.W * right);

        /// <summary>Divides each component of a <see cref="Vector4" /> value by a given <see cref="float" /> value.</summary>
        /// <param name="left">The dividend.</param>
        /// <param name="right">The divisor to divide each component by.</param>
        /// <returns>The result of multiplying each component of <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Vector4 operator /(Vector4 left, float right) => new Vector4(left.X / right, left.Y / right, left.Z / right, left.W / right);

        /// <summary>Multiplies each component of a <see cref="Vector4" /> value by a given <see cref="float" /> value.</summary>
        /// <param name="v">The vector to multiply.</param>
        /// <param name="m">The value to multiply each component by.</param>
        /// <returns>The result of multiplying each component of <paramref name="v" /> by <paramref name="m" />.</returns>
        public static Vector4 operator *(Vector4 v, Matrix4x4 m) => MapPosition(v, m);

        /// <summary>Calculates the dot product of two <see cref="Vector4" /> values.</summary>
        /// <param name="left">The first value to dot.</param>
        /// <param name="right">The second value to dot.</param>
        /// <returns>The result of adding the multiplication of each component of <paramref name="left" /> by each component of <paramref name="right" />.</returns>
        public static float Dot(Vector4 left, Vector4 right) => (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);

        /// <summary>Computes the normalized value of the given <see cref="Vector4" /> value.</summary>
        /// <param name="value">The value to normalize.</param>
        /// <returns>The unit vector of <paramref name="value" />.</returns>
        public static Vector4 Normalize(Vector4 value) => value / Vector4.Length(value);

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-dimension.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Vector4 WithX(float value) => new Vector4(value, Y, Z, W);

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-dimension.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Vector4 WithY(float value) => new Vector4(X, value, Z, W);

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="value">The new value of the z-dimension.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="Z" /> set to <paramref name="value" />.</returns>
        public Vector4 WithZ(float value) => new Vector4(X, Y, value, W);

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="W" /> set to the specified value.</summary>
        /// <param name="value">The new value of the w-dimension.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="W" /> set to <paramref name="value" />.</returns>
        public Vector4 WithW(float value) => new Vector4(X, Y, Z, value);

        /// <summary>Matrix-Vector multiplication between the left <see cref="Matrix4x4" /> and right <see cref="Vector4" />.</summary>
        /// <param name="v">The <see cref="Vector4" /> for this operation.</param>
        /// <param name="m">The <see cref="Matrix4x4" /> to multiply.</param>
        /// <returns>The resulting transformed <see cref="Vector4" />.</returns>
        public static Vector4 MapPosition(Vector4 v, Matrix4x4 m) => new Vector4(
            (v.X * m.X.X) + (v.Y * m.Y.X) + (v.Z * m.Z.X) + (v.W * m.W.X),
            (v.X * m.X.Y) + (v.Y * m.Y.Y) + (v.Z * m.Z.Y) + (v.W * m.W.Y),
            (v.X * m.X.Z) + (v.Y * m.Y.Z) + (v.Z * m.Z.Z) + (v.W * m.W.Z),
            (v.X * m.X.W) + (v.Y * m.Y.W) + (v.Z * m.Z.W) + (v.W * m.W.W)
        );

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Vector4 other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Vector4 other) => this == other;

        /// <summary>Tests if two <see cref="Vector4" /> instances have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="other">The other instance to compare.</param>
        /// <param name="errorTolerance">The threshold below which they are sufficiently similar.</param>
        /// <returns><c>True</c> if similar, <c>False</c> otherwise.</returns>
        public bool IsSimilarTo(Vector4 other, float errorTolerance = FloatUtilities.ErrorTolerance)
        {
            return X.IsSimilarTo(other.X, errorTolerance)
                && Y.IsSimilarTo(other.Y, errorTolerance)
                && Z.IsSimilarTo(other.Z, errorTolerance)
                && W.IsSimilarTo(other.W, errorTolerance);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(X);
                hashCode.Add(Y);
                hashCode.Add(Z);
                hashCode.Add(W);
            }
            return hashCode.ToHashCode();
        }

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
    }
}
