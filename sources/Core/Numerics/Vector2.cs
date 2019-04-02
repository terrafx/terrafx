// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using static TerraFX.Utilities.HashUtilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a two-dimensional Euclidean vector.</summary>
    public readonly struct Vector2 : IEquatable<Vector2>, IFormattable
    {
        #region Defaults
        /// <summary>Defines a <see cref="Vector2" /> where all components are zero.</summary>
        public static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector2" /> whose x-component is one and whose remaining components are zero.</summary>
        public static readonly Vector2 UnitX = new Vector2(1.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector2" /> whose y-component is one and whose remaining components are zero.</summary>
        public static readonly Vector2 UnitY = new Vector2(0.0f, 1.0f);

        /// <summary>Defines a <see cref="Vector2" /> where all components are one.</summary>
        public static readonly Vector2 One = new Vector2(1.0f, 1.0f);
        #endregion

        #region Fields
        private readonly float _x;
        private readonly float _y;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Vector2" /> struct.</summary>
        /// <param name="x">The value of the x-dimension.</param>
        /// <param name="y">The value of the y-dimension.</param>
        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }
        #endregion

        #region Properties
        /// <summary>Gets the value of the x-dimension.</summary>
        public float X
        {
            get
            {
                return _x;
            }
        }

        /// <summary>Gets the value of the y-dimension.</summary>
        public float Y
        {
            get
            {
                return _y;
            }
        }
        #endregion

        #region Comparison Operators
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
        #endregion

        #region Methods
        /// <summary>Creates a new <see cref="Vector2" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-dimension.</param>
        /// <returns>A new <see cref="Vector2" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Vector2 WithX(float value)
        {
            return new Vector2(value, Y);
        }

        /// <summary>Creates a new <see cref="Vector2" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-dimension.</param>
        /// <returns>A new <see cref="Vector2" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Vector2 WithY(float value)
        {
            return new Vector2(X, value);
        }
        #endregion

        #region System.IEquatable<Vector2> Methods
        /// <summary>Compares a <see cref="Vector2" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Vector2" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Vector2 other)
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

            return new StringBuilder(5 + separator.Length)
                .Append('<')
                .Append(X.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(Y.ToString(format, formatProvider))
                .Append('>')
                .ToString();
        }
        #endregion

        #region System.Object Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Vector2" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Vector2 other)
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
            }
            return FinalizeValue(combinedValue, (sizeof(float) * 2));
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
