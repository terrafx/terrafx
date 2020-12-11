// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a 4x4 row-major matrix.</summary>
    public readonly struct Matrix4x4 : IEquatable<Matrix4x4>, IFormattable
    {
        /// <summary>Defines the identity matrix.</summary>
        public static readonly Matrix4x4 Identity = new Matrix4x4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

        /// <summary>Defines the all zeros matrix.</summary>
        public static readonly Matrix4x4 Zero = new Matrix4x4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero);

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

        /// <summary>Multiplies two <see cref="Matrix4x4" /> instances.</summary>
        /// <param name="left">The <see cref="Matrix4x4" /> to multiply with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Matrix4x4" /> to multiply with <paramref name="left" />.</param>
        /// <returns>The matrix multiplication result.</returns>
        public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
        {
            return new Matrix4x4(
                new Vector4(left.X.X * right.X.X, left.Y.X * right.X.Y, left.Z.X * right.X.Z, left.W.X * right.X.W),
                new Vector4(left.X.X * right.Y.X, left.Y.X * right.Y.Y, left.Z.X * right.Y.Z, left.W.X * right.Y.W),
                new Vector4(left.X.X * right.Z.X, left.Y.X * right.Z.Y, left.Z.X * right.Z.Z, left.W.X * right.Z.W),
                new Vector4(left.X.X * right.W.X, left.Y.X * right.W.Y, left.Z.X * right.W.Z, left.W.X * right.W.W)
              );
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
        /// <param name="other">The other instance to compare.</param>
        /// <param name="errorTolerance">The threshold below which they are sufficiently similar.</param>
        /// <returns><c>True</c> if similar, <c>False</c> otherwise.</returns>
        public bool IsSimilarTo(Matrix4x4 other, float errorTolerance = FloatUtilities.ErrorTolerance)
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

        /// <summary>A rounded version of this <see cref="Matrix4x4" />.</summary>
        /// <param name="m">The <see cref="Matrix4x4" /> for this operation.</param>
        /// <param name="numDigits">The number of fractional digits to round to. By default 4.</param>
        /// <returns>The resulting rounded <see cref="Matrix4x4" />.</returns>
        public static Matrix4x4 Round(Matrix4x4 m, int numDigits = 4) => new Matrix4x4(Vector4.Round(m.X, numDigits), Vector4.Round(m.Y, numDigits), Vector4.Round(m.Z ,numDigits), Vector4.Round(m.W ,numDigits));

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
        public static Matrix4x4 Transpose(Matrix4x4 m)
        {
            return new Matrix4x4(
                new Vector4(m.X.X, m.Y.X, m.Z.X, m.W.X),
                new Vector4(m.X.Y, m.Y.Y, m.Z.Y, m.W.Y),
                new Vector4(m.X.Z, m.Y.Z, m.Z.Z, m.W.Z),
                new Vector4(m.X.W, m.Y.W, m.Z.W, m.W.W));
        }

    }
}
