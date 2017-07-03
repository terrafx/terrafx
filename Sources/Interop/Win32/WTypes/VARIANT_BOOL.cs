// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\wtypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>A boolean variable.</summary>
    public struct VARIANT_BOOL : IComparable, IComparable<VARIANT_BOOL>, IEquatable<VARIANT_BOOL>, IFormattable
    {
        #region Constants
        /// <summary>A <see cref="VARIANT_BOOL" /> value that represents <c>false</c>.</summary>
        public static readonly VARIANT_BOOL FALSE = false;

        /// <summary>A <see cref="VARIANT_BOOL" /> value that represents <c>true</c>.</summary>
        public static readonly VARIANT_BOOL TRUE = true;
        #endregion

        #region Fields
        private short _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="VARIANT_BOOL" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public VARIANT_BOOL(bool value) : this(value ? (short)(-1) : (short)(0))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="VARIANT_BOOL" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public VARIANT_BOOL(short value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="VARIANT_BOOL" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="VARIANT_BOOL" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="VARIANT_BOOL" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(VARIANT_BOOL left, VARIANT_BOOL right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="VARIANT_BOOL" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="VARIANT_BOOL" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="VARIANT_BOOL" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(VARIANT_BOOL left, VARIANT_BOOL right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="VARIANT_BOOL" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="VARIANT_BOOL" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="VARIANT_BOOL" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(VARIANT_BOOL left, VARIANT_BOOL right)
        {
            return (left._value > right._value);
        }

        /// <summary>Compares two <see cref="VARIANT_BOOL" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="VARIANT_BOOL" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="VARIANT_BOOL" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(VARIANT_BOOL left, VARIANT_BOOL right)
        {
            return (left._value >= right._value);
        }

        /// <summary>Compares two <see cref="VARIANT_BOOL" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="VARIANT_BOOL" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="VARIANT_BOOL" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(VARIANT_BOOL left, VARIANT_BOOL right)
        {
            return (left._value < right._value);
        }

        /// <summary>Compares two <see cref="VARIANT_BOOL" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="VARIANT_BOOL" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="VARIANT_BOOL" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(VARIANT_BOOL left, VARIANT_BOOL right)
        {
            return (left._value <= right._value);
        }

        /// <summary>Converts a <see cref="VARIANT_BOOL" /> to an equivalent <see cref="bool" /> value.</summary>
        /// <param name="value">The <see cref="VARIANT_BOOL" /> to convert.</param>
        public static implicit operator bool(VARIANT_BOOL value)
        {
            return (value._value != 0);
        }

        /// <summary>Converts a <see cref="VARIANT_BOOL" /> to an equivalent <see cref="short" /> value.</summary>
        /// <param name="value">The <see cref="VARIANT_BOOL" /> to convert.</param>
        public static implicit operator short(VARIANT_BOOL value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="bool" /> to an equivalent <see cref="VARIANT_BOOL" /> value.</summary>
        /// <param name="value">The <see cref="bool" /> to convert.</param>
        public static implicit operator VARIANT_BOOL(bool value)
        {
            return new VARIANT_BOOL(value);
        }

        /// <summary>Converts a <see cref="short" /> to an equivalent <see cref="VARIANT_BOOL" /> value.</summary>
        /// <param name="value">The <see cref="short" /> to convert.</param>
        public static implicit operator VARIANT_BOOL(short value)
        {
            return new VARIANT_BOOL(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="VARIANT_BOOL" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is VARIANT_BOOL other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<VARIANT_BOOL>
        /// <summary>Compares a <see cref="VARIANT_BOOL" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="VARIANT_BOOL" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(VARIANT_BOOL other)
        {
            // We have to actually compare because subtraction
            // causes wrapping for very large negative numbers.

            if (this < other)
            {
                return -1;
            }
            else if (this > other)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region System.IEquatable<VARIANT_BOOL>
        /// <summary>Compares a <see cref="VARIANT_BOOL" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="VARIANT_BOOL" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(VARIANT_BOOL other)
        {
            return (this == other);
        }
        #endregion

        #region System.IFormattable
        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            var stringBuilder = new StringBuilder(8);
            {
                stringBuilder.Append((bool)(this));
                stringBuilder.Append(' ');
                stringBuilder.Append('(');
                stringBuilder.Append(_value.ToString(format, formatProvider));
                stringBuilder.Append(')');
            }
            return stringBuilder.ToString();
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="VARIANT_BOOL" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is VARIANT_BOOL other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
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
