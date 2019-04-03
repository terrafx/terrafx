// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using static TerraFX.Utilities.HashUtilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a four-dimensional Euclidean vector.</summary>
    public readonly struct Vector4 : IEquatable<Vector4>, IFormattable
    {
        #region Defaults
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
        #endregion

        #region Fields
        private readonly float _x;
        private readonly float _y;
        private readonly float _z;
        private readonly float _w;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Vector4" /> struct.</summary>
        /// <param name="x">The value of the x-component.</param>
        /// <param name="y">The value of the y-component.</param>
        /// <param name="z">The value of the z-component.</param>
        /// <param name="w">The value of the w-component.</param>
        public Vector4(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }
        #endregion

        #region Properties
        /// <summary>Gets the value of the x-component.</summary>
        public float X
        {
            get
            {
                return _x;
            }
        }

        /// <summary>Gets the value of the y-component.</summary>
        public float Y
        {
            get
            {
                return _y;
            }
        }

        /// <summary>Gets the value of the z-component.</summary>
        public float Z
        {
            get
            {
                return _z;
            }
        }

        /// <summary>Gets the value of the w-component.</summary>
        public float W
        {
            get
            {
                return _w;
            }
        }
        #endregion

        #region Comparison Operators
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
        #endregion

        #region Methods
        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-component.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Vector4 WithX(float value)
        {
            return new Vector4(value, Y, Z, W);
        }

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-component.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Vector4 WithY(float value)
        {
            return new Vector4(X, value, Z, W);
        }

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="value">The new value of the z-component.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="Z" /> set to <paramref name="value" />.</returns>
        public Vector4 WithZ(float value)
        {
            return new Vector4(X, Y, value, W);
        }

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="W" /> set to the specified value.</summary>
        /// <param name="value">The new value of the w-component.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="W" /> set to <paramref name="value" />.</returns>
        public Vector4 WithW(float value)
        {
            return new Vector4(X, Y, Z, value);
        }
        #endregion

        #region System.IEquatable<Vector4> Methods
        /// <summary>Compares a <see cref="Vector4" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Vector4" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Vector4 other)
        {
            return this == other;
        }
        #endregion

        #region System.IFormattable Methods
        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
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
        #endregion

        #region System.Object Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Vector4" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            return (obj is Vector4 other)
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
            return FinalizeValue(combinedValue, sizeof(float) * 4);
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
