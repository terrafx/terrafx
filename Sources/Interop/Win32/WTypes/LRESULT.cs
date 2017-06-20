// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Diagnostics;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>Signed result of message processing.</summary>
    unsafe public struct LRESULT : IComparable, IComparable<LRESULT>, IEquatable<LRESULT>, IFormattable
    {
        #region Fields
        private IntPtr _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="LRESULT" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public LRESULT(int value) : this((IntPtr)(value))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LRESULT" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public LRESULT(long value) : this((IntPtr)(value))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LRESULT" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public LRESULT(IntPtr value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="LRESULT" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(LRESULT left, LRESULT right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(LRESULT left, LRESULT right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(LRESULT left, LRESULT right)
        {
            return ((void*)(left._value) > (void*)(right._value));
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(LRESULT left, LRESULT right)
        {
            return ((void*)(left._value) >= (void*)(right._value));
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(LRESULT left, LRESULT right)
        {
            return ((void*)(left._value) < (void*)(right._value));
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(LRESULT left, LRESULT right)
        {
            return ((void*)(left._value) <= (void*)(right._value));
        }

        /// <summary>Converts a <see cref="LRESULT" /> to an equivalent <see cref="int" /> value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> to convert.</param>
        public static explicit operator int(LRESULT value)
        {
            return (int)(value._value);
        }

        /// <summary>Converts a <see cref="LRESULT" /> to an equivalent <see cref="long" /> value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> to convert.</param>
        public static implicit operator long(LRESULT value)
        {
            return (long)(value._value);
        }

        /// <summary>Converts a <see cref="LRESULT" /> to an equivalent <see cref="IntPtr" /> value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> to convert.</param>
        public static implicit operator IntPtr(LRESULT value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="long" /> to an equivalent <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="long" /> to convert.</param>
        public static explicit operator LRESULT(long value)
        {
            return new LRESULT(value);
        }

        /// <summary>Converts a <see cref="int" /> to an equivalent <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="int" /> to convert.</param>
        public static implicit operator LRESULT(int value)
        {
            return new LRESULT(value);
        }

        /// <summary>Converts a <see cref="IntPtr" /> to an equivalent <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="IntPtr" /> to convert.</param>
        public static implicit operator LRESULT(IntPtr value)
        {
            return new LRESULT(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="LRESULT" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is LRESULT other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<LRESULT>
        /// <summary>Compares a <see cref="LRESULT" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="LRESULT" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(LRESULT other)
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

        #region System.IEquatable<LRESULT>
        /// <summary>Compares a <see cref="LRESULT" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="LRESULT" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(LRESULT other)
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
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(_value)).ToString(format, formatProvider);
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                return ((long)(_value)).ToString(format, formatProvider);
            }
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="LRESULT" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is LRESULT other)
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
