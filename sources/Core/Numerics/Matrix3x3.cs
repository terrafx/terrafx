// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using static TerraFX.Utilities.InteropUtilities;
using static TerraFX.Utilities.HashUtilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a 3x3 row-major matrix.</summary>
    public readonly struct Matrix3x3 : IEquatable<Matrix3x3>, IFormattable
    {
        #region Defaults
        /// <summary>Defines the identity matrix.</summary>
        public static readonly Matrix3x3 Identity = new Matrix3x3(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);
        #endregion

        #region Fields
        private readonly Vector3 _x;
        private readonly Vector3 _y;
        private readonly Vector3 _z;
        #endregion

        #region Constructors
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
        #endregion

        #region Properties
        /// <summary>Gets the value of the x-dimension.</summary>
        public Vector3 X
        {
            get
            {
                return _x;
            }
        }

        /// <summary>Gets the value of the y-dimension.</summary>
        public Vector3 Y
        {
            get
            {
                return _y;
            }
        }

        /// <summary>Gets the value of the z-dimension.</summary>
        public Vector3 Z
        {
            get
            {
                return _z;
            }
        }
        #endregion

        #region Comparison Operators
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
        #endregion

        #region Methods
        /// <summary>Creates a new <see cref="Matrix3x3" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-dimension.</param>
        /// <returns>A new <see cref="Matrix3x3" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Matrix3x3 WithX(Vector3 value)
        {
            return new Matrix3x3(value, Y, Z);
        }

        /// <summary>Creates a new <see cref="Matrix3x3" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-dimension.</param>
        /// <returns>A new <see cref="Matrix3x3" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Matrix3x3 WithY(Vector3 value)
        {
            return new Matrix3x3(X, value, Z);
        }

        /// <summary>Creates a new <see cref="Matrix3x3" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="value">The new value of the z-dimension.</param>
        /// <returns>A new <see cref="Matrix3x3" /> instance with <see cref="Z" /> set to <paramref name="value" />.</returns>
        public Matrix3x3 WithZ(Vector3 value)
        {
            return new Matrix3x3(X, Y, value);
        }
        #endregion

        #region System.IEquatable<Matrix3x3> Methods
        /// <summary>Compares a <see cref="Matrix3x3" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Matrix3x3" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix3x3 other)
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

            var stringBuilder = new StringBuilder(7 + (separator.Length * 2));
            {
                stringBuilder.Append('<');
                stringBuilder.Append(X.ToString(format, formatProvider));
                stringBuilder.Append(separator);
                stringBuilder.Append(' ');
                stringBuilder.Append(Y.ToString(format, formatProvider));
                stringBuilder.Append(separator);
                stringBuilder.Append(' ');
                stringBuilder.Append(Z.ToString(format, formatProvider));
                stringBuilder.Append('>');
            }
            return stringBuilder.ToString();
        }
        #endregion

        #region System.Object Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Matrix3x3" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Matrix3x3 other)
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
            }
            return FinalizeValue(combinedValue, (SizeOf<Vector3>() * 3));
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
