// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using static TerraFX.Utilities.HashUtilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a 3x3 row-major matrix.</summary>
    public readonly struct Matrix3x3 : IEquatable<Matrix3x3>, IFormattable
    {
        /// <summary>Defines the identity matrix.</summary>
        public static readonly Matrix3x3 Identity = new Matrix3x3(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

        private readonly Vector3 _x;
        private readonly Vector3 _y;
        private readonly Vector3 _z;

        /// <summary>Initializes a new instance of the <see cref="Matrix3x3" /> struct.</summary>
        /// <param name="x">The value of the x-dimension.</param>
        /// <param name="y">The value of the y-dimension.</param>
        /// <param name="z">The value of the z-dimension.</param>
        public Matrix3x3(Vector3 x, Vector3 y, Vector3 z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>Gets the value of the x-dimension.</summary>
        public Vector3 X => _x;

        /// <summary>Gets the value of the y-dimension.</summary>
        public Vector3 Y => _y;

        /// <summary>Gets the value of the z-dimension.</summary>
        public Vector3 Z => _z;

        /// <summary>Compares two <see cref="Matrix3x3" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Matrix3x3" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Matrix3x3" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Matrix3x3 left, Matrix3x3 right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y)
                && (left.Z == right.Z);
        }

        /// <summary>Compares two <see cref="Matrix3x3" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Matrix3x3" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Matrix3x3" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix3x3 left, Matrix3x3 right)
        {
            return (left.X != right.X)
                || (left.Y != right.Y)
                || (left.Z != right.Z);
        }

        /// <summary>Creates a new <see cref="Matrix3x3" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-dimension.</param>
        /// <returns>A new <see cref="Matrix3x3" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Matrix3x3 WithX(Vector3 value) => new Matrix3x3(value, Y, Z);

        /// <summary>Creates a new <see cref="Matrix3x3" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-dimension.</param>
        /// <returns>A new <see cref="Matrix3x3" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Matrix3x3 WithY(Vector3 value) => new Matrix3x3(X, value, Z);

        /// <summary>Creates a new <see cref="Matrix3x3" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="value">The new value of the z-dimension.</param>
        /// <returns>A new <see cref="Matrix3x3" /> instance with <see cref="Z" /> set to <paramref name="value" />.</returns>
        public Matrix3x3 WithZ(Vector3 value) => new Matrix3x3(X, Y, value);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Matrix3x3 other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Matrix3x3 other) => this == other;

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var combinedValue = 0;
            {
                combinedValue = CombineValue(X.GetHashCode(), combinedValue);
                combinedValue = CombineValue(Y.GetHashCode(), combinedValue);
                combinedValue = CombineValue(Z.GetHashCode(), combinedValue);
            }
            return FinalizeValue(combinedValue, SizeOf<Vector3>() * 3);
        }

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
    }
}
