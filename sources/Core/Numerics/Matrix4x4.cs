// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;

namespace TerraFX.Numerics
{
    /// <summary>Defines a 4x4 row-major matrix.</summary>
    public readonly struct Matrix4x4 : IEquatable<Matrix4x4>, IFormattable
    {
        /// <summary>Defines the identity matrix.</summary>
        public static readonly Matrix4x4 Identity = new Matrix4x4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

        /// <summary>Defines the all zeros matrix.</summary>
        public static readonly Matrix4x4 Zero = new Matrix4x4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero);

        /// <summary>Defines the all ones matrix.</summary>
        public static readonly Matrix4x4 One = new Matrix4x4(Vector4.One, Vector4.One, Vector4.One, Vector4.One);

        private readonly Vector4 _x;
        private readonly Vector4 _y;
        private readonly Vector4 _z;
        private readonly Vector4 _w;

        /// <summary>Initializes a new instance of the <see cref="Matrix4x4" /> struct.</summary>
        /// <param name="x">The value of the x-dimension.</param>
        /// <param name="y">The value of the y-dimension.</param>
        /// <param name="z">The value of the z-dimension.</param>
        /// <param name="w">The value of the w-dimension.</param>
        public Matrix4x4(Vector4 x, Vector4 y, Vector4 z, Vector4 w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        /// <summary>Gets the value of the x-dimension.</summary>
        public Vector4 X => _x;

        /// <summary>Gets the value of the y-dimension.</summary>
        public Vector4 Y => _y;

        /// <summary>Gets the value of the z-dimension.</summary>
        public Vector4 Z => _z;

        /// <summary>Gets the value of the w-dimension.</summary>
        public Vector4 W => _w;

        /// <summary>Compares two <see cref="Matrix4x4" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Matrix4x4" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Matrix4x4" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Matrix4x4 left, Matrix4x4 right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y)
                && (left.Z == right.Z)
                && (left.W == right.W);
        }

        /// <summary>Compares two <see cref="Matrix4x4" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Matrix4x4" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Matrix4x4" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix4x4 left, Matrix4x4 right)
        {
            return (left.X != right.X)
                || (left.Y != right.Y)
                || (left.Z != right.Z)
                || (left.W != right.W);
        }

        /// <summary>Multiplies a <see cref="Matrix4x4" /> with a <see cref="float" />.</summary>
        /// <param name="left">The <see cref="Matrix4x4" /> to multiply with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="float" /> to multiply with <paramref name="left" />.</param>
        /// <returns>The matrix multiplication result.</returns>
        public static Matrix4x4 operator *(Matrix4x4 left, float right)
        {
            return new Matrix4x4(
                left.X * right,
                left.Y * right,
                left.Z * right,
                left.W * right
            );
        }

        /// <summary>Multiplies two <see cref="Matrix4x4" /> instances.</summary>
        /// <param name="left">The <see cref="Matrix4x4" /> to multiply with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Matrix4x4" /> to multiply with <paramref name="left" />.</param>
        /// <returns>The matrix multiplication result.</returns>
        public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
        {
            var transposed = Transpose(right);

            return new Matrix4x4(
                DotRows(left, transposed.X),
                DotRows(left, transposed.Y),
                DotRows(left, transposed.Z),
                DotRows(left, transposed.W)
            );

            static Vector4 DotRows(Matrix4x4 left, Vector4 right)
            {
                return new Vector4(
                    Vector4.Dot(left.X, right),
                    Vector4.Dot(left.Y, right),
                    Vector4.Dot(left.Z, right),
                    Vector4.Dot(left.W, right)
                );
            }
        }

        /// <summary>Creates a new <see cref="Matrix4x4" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-dimension.</param>
        /// <returns>A new <see cref="Matrix4x4" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Matrix4x4 WithX(Vector4 value) => new Matrix4x4(value, Y, Z, W);

        /// <summary>Creates a new <see cref="Matrix4x4" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-dimension.</param>
        /// <returns>A new <see cref="Matrix4x4" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Matrix4x4 WithY(Vector4 value) => new Matrix4x4(X, value, Z, W);

        /// <summary>Creates a new <see cref="Matrix4x4" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="value">The new value of the z-dimension.</param>
        /// <returns>A new <see cref="Matrix4x4" /> instance with <see cref="Z" /> set to <paramref name="value" />.</returns>
        public Matrix4x4 WithZ(Vector4 value) => new Matrix4x4(X, Y, value, W);

        /// <summary>Creates a new <see cref="Matrix4x4" /> instance with <see cref="W" /> set to the specified value.</summary>
        /// <param name="value">The new value of the w-dimension.</param>
        /// <returns>A new <see cref="Matrix4x4" /> instance with <see cref="W" /> set to <paramref name="value" />.</returns>
        public Matrix4x4 WithW(Vector4 value) => new Matrix4x4(X, Y, Z, value);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Matrix4x4 other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Matrix4x4 other) => this == other;

        /// <summary>Tests if two <see cref="Matrix4x4" /> instances have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="left">The left instance to compare.</param>
        /// <param name="right">The right instance to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns><c>True</c> if similar, <c>False</c> otherwise.</returns>
        public static bool EqualEstimate(Matrix4x4 left, Matrix4x4 right, Matrix4x4 epsilon)
        {
            return Vector4.EqualEstimate(left.X, right.X, epsilon.X)
                && Vector4.EqualEstimate(left.Y, right.Y, epsilon.Y)
                && Vector4.EqualEstimate(left.Z, right.Z, epsilon.Z)
                && Vector4.EqualEstimate(left.W, right.W, epsilon.W);
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

        /// <summary>Creates a transposed version of this <see cref="Matrix4x4" />.</summary>
        /// <returns>The transposed <see cref="Matrix4x4" />.</returns>
        public static Matrix4x4 Transpose(Matrix4x4 value)
        {
            return new Matrix4x4(
                new Vector4(value.X.X, value.Y.X, value.Z.X, value.W.X),
                new Vector4(value.X.Y, value.Y.Y, value.Z.Y, value.W.Y),
                new Vector4(value.X.Z, value.Y.Z, value.Z.Z, value.W.Z),
                new Vector4(value.X.W, value.Y.W, value.Z.W, value.W.W)
            );
        }
    }
}
