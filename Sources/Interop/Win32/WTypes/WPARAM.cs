// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Diagnostics;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>A message parameter.</summary>
    unsafe public struct WPARAM : IComparable, IComparable<WPARAM>, IEquatable<WPARAM>, IFormattable
    {
        #region Fields
        private UIntPtr _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="WPARAM" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public WPARAM(uint value) : this((UIntPtr)(value))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="WPARAM" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public WPARAM(ulong value) : this((UIntPtr)(value))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="WPARAM" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public WPARAM(UIntPtr value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="WPARAM" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="WPARAM" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="WPARAM" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(WPARAM left, WPARAM right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="WPARAM" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="WPARAM" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="WPARAM" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(WPARAM left, WPARAM right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="WPARAM" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="WPARAM" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="WPARAM" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(WPARAM left, WPARAM right)
        {
            return ((void*)(left._value) > (void*)(right._value));
        }

        /// <summary>Compares two <see cref="WPARAM" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="WPARAM" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="WPARAM" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(WPARAM left, WPARAM right)
        {
            return ((void*)(left._value) >= (void*)(right._value));
        }

        /// <summary>Compares two <see cref="WPARAM" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="WPARAM" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="WPARAM" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(WPARAM left, WPARAM right)
        {
            return ((void*)(left._value) < (void*)(right._value));
        }

        /// <summary>Compares two <see cref="WPARAM" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="WPARAM" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="WPARAM" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(WPARAM left, WPARAM right)
        {
            return ((void*)(left._value) <= (void*)(right._value));
        }

        /// <summary>Converts a <see cref="WPARAM" /> to an equivalent <see cref="uint" /> value.</summary>
        /// <param name="value">The <see cref="WPARAM" /> to convert.</param>
        public static explicit operator uint(WPARAM value)
        {
            return (uint)(value._value);
        }

        /// <summary>Converts a <see cref="WPARAM" /> to an equivalent <see cref="ulong" /> value.</summary>
        /// <param name="value">The <see cref="WPARAM" /> to convert.</param>
        public static implicit operator ulong(WPARAM value)
        {
            return (ulong)(value._value);
        }

        /// <summary>Converts a <see cref="WPARAM" /> to an equivalent <see cref="UIntPtr" /> value.</summary>
        /// <param name="value">The <see cref="WPARAM" /> to convert.</param>
        public static implicit operator UIntPtr(WPARAM value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="ulong" /> to an equivalent <see cref="WPARAM" /> value.</summary>
        /// <param name="value">The <see cref="ulong" /> to convert.</param>
        public static explicit operator WPARAM(ulong value)
        {
            return new WPARAM(value);
        }

        /// <summary>Converts a <see cref="uint" /> to an equivalent <see cref="WPARAM" /> value.</summary>
        /// <param name="value">The <see cref="uint" /> to convert.</param>
        public static implicit operator WPARAM(uint value)
        {
            return new WPARAM(value);
        }

        /// <summary>Converts a <see cref="UIntPtr" /> to an equivalent <see cref="WPARAM" /> value.</summary>
        /// <param name="value">The <see cref="UIntPtr" /> to convert.</param>
        public static implicit operator WPARAM(UIntPtr value)
        {
            return new WPARAM(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="WPARAM" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is WPARAM other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<WPARAM>
        /// <summary>Compares a <see cref="WPARAM" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="WPARAM" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(WPARAM other)
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

        #region System.IEquatable<WPARAM>
        /// <summary>Compares a <see cref="WPARAM" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="WPARAM" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(WPARAM other)
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
            if (UIntPtr.Size == sizeof(int))
            {
                return ((int)(_value)).ToString(format, formatProvider);
            }
            else
            {
                Debug.Assert(UIntPtr.Size == sizeof(long));
                return ((long)(_value)).ToString(format, formatProvider);
            }
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="WPARAM" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is WPARAM other)
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
