// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Text;

namespace TerraFX.Numerics
{
    /// <summary>Defines a 3x3 row-major matrix.</summary>
    public readonly struct Matrix3x3 : IEquatable<Matrix3x3>, IFormattable
    {
        /// <summary>Defines the identity matrix.</summary>
        public static readonly Matrix3x3 Identity = new Matrix3x3(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

        /// <summary>Defines the all zeros matrix.</summary>
        public static readonly Matrix3x3 Zero = new Matrix3x3(Vector3.Zero, Vector3.Zero, Vector3.Zero);

        /// <summary>Defines the all ones matrix.</summary>
        public static readonly Matrix3x3 One = new Matrix3x3(Vector3.One, Vector3.One, Vector3.One);

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

        /// <summary>Compares two matrices instances to determine equality.</summary>
        /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
        /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Matrix3x3 left, Matrix3x3 right)
            => (left.X == right.X)
            && (left.Y == right.Y)
            && (left.Z == right.Z);

        /// <summary>Compares two matrices instances to determine inequality.</summary>
        /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
        /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix3x3 left, Matrix3x3 right)
            => (left.X != right.X)
            || (left.Y != right.Y)
            || (left.Z != right.Z);

        /// <summary>Computes the product of a matrix and a float.</summary>
        /// <param name="left">The matrix to multiply by <paramref name="right" />.</param>
        /// <param name="right">The float which is used to multiply <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static Matrix3x3 operator *(Matrix3x3 left, float right) => new Matrix3x3(
            left.X * right,
            left.Y * right,
            left.Z * right
        );

        /// <summary>Computes the product of two matrices.</summary>
        /// <param name="left">The matrix to multiply by <paramref name="right" />.</param>
        /// <param name="right">The matrix which is used to multiply <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static Matrix3x3 operator *(Matrix3x3 left, Matrix3x3 right)
        {
            var transposed = Transpose(right);

            return new Matrix3x3(
                DotRows(left, transposed.X),
                DotRows(left, transposed.Y),
                DotRows(left, transposed.Z)
            );

            static Vector3 DotRows(Matrix3x3 left, Vector3 right)
            {
                return new Vector3(
                    Vector3.Dot(left.X, right),
                    Vector3.Dot(left.Y, right),
                    Vector3.Dot(left.Z, right)
                );
            }
        }

        /// <summary>Creates a matrix from a rotation.</summary>
        /// <param name="rotation">The rotation of the matrix.</param>
        /// <returns>A matrix that represents <paramref name="rotation" />.</returns>
        public static Matrix3x3 CreateFromRotation(Quaternion rotation)
        {
            var w2 = rotation.W * rotation.W;
            var x2 = rotation.X * rotation.X;
            var y2 = rotation.Y * rotation.Y;
            var z2 = rotation.Z * rotation.Z;

            var wz = 2 * rotation.W * rotation.Z;
            var xz = 2 * rotation.X * rotation.Z;
            var xy = 2 * rotation.X * rotation.Y;
            var wx = 2 * rotation.W * rotation.X;
            var wy = 2 * rotation.W * rotation.Y;
            var yz = 2 * rotation.Y * rotation.Z;

            return new Matrix3x3(
                new Vector3(w2 + x2 - y2 - z2, wz + xy, xz - wy),
                new Vector3(xy - wz, w2 - x2 + y2 - z2, wx + yz),
                new Vector3(wy + xz, yz - wx, w2 - x2 - y2 + z2)
            );
        }

        /// <summary>Compares two matrices to determine approximate equality.</summary>
        /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
        /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
        /// <param name="epsilon">The maximum (exclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
        public static bool EqualsEstimate(Matrix3x3 left, Matrix3x3 right, Matrix3x3 epsilon)
            => Vector3.EqualsEstimate(left.X, right.X, epsilon.X)
            && Vector3.EqualsEstimate(left.Y, right.Y, epsilon.Y)
            && Vector3.EqualsEstimate(left.Z, right.Z, epsilon.Z);

        /// <summary>Transposes a matrix.</summary>
        /// <param name="value">The matrix to transpose.</param>
        /// <returns>The transposition of <paramref name="value" />.</returns>
        public static Matrix3x3 Transpose(Matrix3x3 value) => new Matrix3x3(
            new Vector3(value.X.X, value.Y.X, value.Z.X),
            new Vector3(value.X.Y, value.Y.Y, value.Z.Y),
            new Vector3(value.X.Z, value.Y.Z, value.Z.Z)
        );

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Matrix3x3 other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Matrix3x3 other) => this == other;

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

        /// <summary>Creates a new <see cref="Matrix3x3" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="x">The new value of the x-dimension.</param>
        /// <returns>A new <see cref="Matrix3x3" /> instance with <see cref="X" /> set to <paramref name="x" />.</returns>
        public Matrix3x3 WithX(Vector3 x) => new Matrix3x3(x, Y, Z);

        /// <summary>Creates a new <see cref="Matrix3x3" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="y">The new value of the y-dimension.</param>
        /// <returns>A new <see cref="Matrix3x3" /> instance with <see cref="Y" /> set to <paramref name="y" />.</returns>
        public Matrix3x3 WithY(Vector3 y) => new Matrix3x3(X, y, Z);

        /// <summary>Creates a new <see cref="Matrix3x3" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="z">The new value of the z-dimension.</param>
        /// <returns>A new <see cref="Matrix3x3" /> instance with <see cref="Z" /> set to <paramref name="z" />.</returns>
        public Matrix3x3 WithZ(Vector3 z) => new Matrix3x3(X, Y, z);
    }
}
