// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\basetsd.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Diagnostics;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>The maximum number of bytes to which a pointer can point.</summary>
    unsafe public struct SIZE_T : IComparable, IComparable<SIZE_T>, IEquatable<SIZE_T>, IFormattable
    {
        #region Fields
        private UIntPtr _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="SIZE_T" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public SIZE_T(int value) : this((UIntPtr)(value))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SIZE_T" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public SIZE_T(long value) : this((UIntPtr)(value))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SIZE_T" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public SIZE_T(UIntPtr value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="SIZE_T" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="SIZE_T" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="SIZE_T" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(SIZE_T left, SIZE_T right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="SIZE_T" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="SIZE_T" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="SIZE_T" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(SIZE_T left, SIZE_T right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="SIZE_T" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="SIZE_T" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="SIZE_T" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(SIZE_T left, SIZE_T right)
        {
            return ((void*)(left._value) > (void*)(right._value));
        }

        /// <summary>Compares two <see cref="SIZE_T" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="SIZE_T" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="SIZE_T" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(SIZE_T left, SIZE_T right)
        {
            return ((void*)(left._value) >= (void*)(right._value));
        }

        /// <summary>Compares two <see cref="SIZE_T" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="SIZE_T" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="SIZE_T" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(SIZE_T left, SIZE_T right)
        {
            return ((void*)(left._value) < (void*)(right._value));
        }

        /// <summary>Compares two <see cref="SIZE_T" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="SIZE_T" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="SIZE_T" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(SIZE_T left, SIZE_T right)
        {
            return ((void*)(left._value) <= (void*)(right._value));
        }

        /// <summary>Converts a <see cref="SIZE_T" /> to an equivalent <see cref="int" /> value.</summary>
        /// <param name="value">The <see cref="SIZE_T" /> to convert.</param>
        public static explicit operator int(SIZE_T value)
        {
            return (int)(value._value);
        }

        /// <summary>Converts a <see cref="SIZE_T" /> to an equivalent <see cref="long" /> value.</summary>
        /// <param name="value">The <see cref="SIZE_T" /> to convert.</param>
        public static implicit operator long(SIZE_T value)
        {
            return (long)(value._value);
        }

        /// <summary>Converts a <see cref="SIZE_T" /> to an equivalent <see cref="UIntPtr" /> value.</summary>
        /// <param name="value">The <see cref="SIZE_T" /> to convert.</param>
        public static implicit operator UIntPtr(SIZE_T value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="long" /> to an equivalent <see cref="SIZE_T" /> value.</summary>
        /// <param name="value">The <see cref="long" /> to convert.</param>
        public static explicit operator SIZE_T(long value)
        {
            return new SIZE_T(value);
        }

        /// <summary>Converts a <see cref="int" /> to an equivalent <see cref="SIZE_T" /> value.</summary>
        /// <param name="value">The <see cref="int" /> to convert.</param>
        public static implicit operator SIZE_T(int value)
        {
            return new SIZE_T(value);
        }

        /// <summary>Converts a <see cref="UIntPtr" /> to an equivalent <see cref="SIZE_T" /> value.</summary>
        /// <param name="value">The <see cref="UIntPtr" /> to convert.</param>
        public static implicit operator SIZE_T(UIntPtr value)
        {
            return new SIZE_T(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="SIZE_T" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is SIZE_T other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<SIZE_T>
        /// <summary>Compares a <see cref="SIZE_T" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="SIZE_T" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(SIZE_T other)
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

        #region System.IEquatable<SIZE_T>
        /// <summary>Compares a <see cref="SIZE_T" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="SIZE_T" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(SIZE_T other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="SIZE_T" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is SIZE_T other)
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
