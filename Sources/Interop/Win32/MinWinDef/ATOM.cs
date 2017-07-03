// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\minwindef.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>An atom.</summary>
    unsafe public struct ATOM : IComparable, IComparable<ATOM>, IEquatable<ATOM>, IFormattable
    {
        #region Fields
        private ushort Value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="ATOM" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public ATOM(ushort value)
        {
            Value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="ATOM" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="ATOM" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="ATOM" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ATOM left, ATOM right)
        {
            return (left.Value == right.Value);
        }

        /// <summary>Compares two <see cref="ATOM" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="ATOM" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="ATOM" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ATOM left, ATOM right)
        {
            return (left.Value == right.Value);
        }

        /// <summary>Compares two <see cref="ATOM" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="ATOM" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="ATOM" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(ATOM left, ATOM right)
        {
            return (left.Value > right.Value);
        }

        /// <summary>Compares two <see cref="ATOM" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="ATOM" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="ATOM" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(ATOM left, ATOM right)
        {
            return (left.Value >= right.Value);
        }

        /// <summary>Compares two <see cref="ATOM" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="ATOM" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="ATOM" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(ATOM left, ATOM right)
        {
            return (left.Value < right.Value);
        }

        /// <summary>Compares two <see cref="ATOM" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="ATOM" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="ATOM" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(ATOM left, ATOM right)
        {
            return (left.Value <= right.Value);
        }

        /// <summary>Converts a <see cref="ATOM" /> to an equivalent <see cref="LPWSTR" /> value.</summary>
        /// <param name="value">The <see cref="ATOM" /> to convert.</param>
        public static explicit operator LPWSTR(ATOM value)
        {
            return new LPWSTR((WCHAR*)(value.Value));
        }

        /// <summary>Converts a <see cref="ATOM" /> to an equivalent <see cref="ushort" /> value.</summary>
        /// <param name="value">The <see cref="ATOM" /> to convert.</param>
        public static implicit operator ushort(ATOM value)
        {
            return value.Value;
        }
        /// <summary>Converts a <see cref="ushort" /> to an equivalent <see cref="ATOM" /> value.</summary>
        /// <param name="value">The <see cref="ushort" /> to convert.</param>
        public static implicit operator ATOM(ushort value)
        {
            return new ATOM(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="ATOM" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is ATOM other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<ATOM>
        /// <summary>Compares a <see cref="ATOM" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="ATOM" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(ATOM other)
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

        #region System.IEquatable<ATOM>
        /// <summary>Compares a <see cref="ATOM" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="ATOM" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(ATOM other)
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
            return Value.ToString(format, formatProvider);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="ATOM" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is ATOM other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
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
