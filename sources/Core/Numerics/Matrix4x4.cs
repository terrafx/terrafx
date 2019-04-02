// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using static TerraFX.Utilities.HashUtilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a 4x4 row-major matrix.</summary>
    public readonly struct Matrix4x4 : IEquatable<Matrix4x4>, IFormattable
    {
        #region Defaults
        /// <summary>Defines the identity matrix.</summary>
        public static readonly Matrix4x4 Identity = new Matrix4x4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);
        #endregion

        #region Fields
        private readonly Vector4 _x;
        private readonly Vector4 _y;
        private readonly Vector4 _z;
        private readonly Vector4 _w;
        #endregion

        #region Constructors
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
        #endregion

        #region Properties
        /// <summary>Gets the value of the x-dimension.</summary>
        public Vector4 X
        {
            get
            {
                return _x;
            }
        }

        /// <summary>Gets the value of the y-dimension.</summary>
        public Vector4 Y
        {
            get
            {
                return _y;
            }
        }

        /// <summary>Gets the value of the z-dimension.</summary>
        public Vector4 Z
        {
            get
            {
                return _z;
            }
        }

        /// <summary>Gets the value of the w-dimension.</summary>
        public Vector4 W
        {
            get
            {
                return _w;
            }
        }
        #endregion

        #region Comparison Operators
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
        #endregion

        #region Methods
        /// <summary>Creates a new <see cref="Matrix4x4" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-dimension.</param>
        /// <returns>A new <see cref="Matrix4x4" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Matrix4x4 WithX(Vector4 value)
        {
            return new Matrix4x4(value, Y, Z, W);
        }

        /// <summary>Creates a new <see cref="Matrix4x4" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-dimension.</param>
        /// <returns>A new <see cref="Matrix4x4" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Matrix4x4 WithY(Vector4 value)
        {
            return new Matrix4x4(X, value, Z, W);
        }

        /// <summary>Creates a new <see cref="Matrix4x4" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="value">The new value of the z-dimension.</param>
        /// <returns>A new <see cref="Matrix4x4" /> instance with <see cref="Z" /> set to <paramref name="value" />.</returns>
        public Matrix4x4 WithZ(Vector4 value)
        {
            return new Matrix4x4(X, Y, value, W);
        }

        /// <summary>Creates a new <see cref="Matrix4x4" /> instance with <see cref="W" /> set to the specified value.</summary>
        /// <param name="value">The new value of the w-dimension.</param>
        /// <returns>A new <see cref="Matrix4x4" /> instance with <see cref="W" /> set to <paramref name="value" />.</returns>
        public Matrix4x4 WithW(Vector4 value)
        {
            return new Matrix4x4(X, Y, Z, value);
        }
        #endregion

        #region System.IEquatable<Matrix4x4> Methods
        /// <summary>Compares a <see cref="Matrix4x4" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Matrix4x4" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix4x4 other)
        {
            return (this == other);
        }
        #endregion

        #region System.IFormattable Methods
        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
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
        #endregion

        #region System.Object Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Matrix4x4" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Matrix4x4 other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            var combinedValue = 0;
            {
                combinedValue = CombineValue(X.GetHashCode(), combinedValue);
                combinedValue = CombineValue(Y.GetHashCode(), combinedValue);
                combinedValue = CombineValue(Z.GetHashCode(), combinedValue);
                combinedValue = CombineValue(W.GetHashCode(), combinedValue);
            }
            return FinalizeValue(combinedValue, (SizeOf<Vector4>() * 4));
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString()
        {
            return ToString(format: null, formatProvider: null);
        }
        #endregion
    }
}
