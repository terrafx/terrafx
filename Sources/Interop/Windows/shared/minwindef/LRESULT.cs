// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\minwindef.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct LRESULT : IComparable, IComparable<LRESULT>, IEquatable<LRESULT>, IFormattable
    {
        #region Fields
        internal LONG_PTR _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="LRESULT" /> struct.</summary>
        /// <param name="value">The <see cref="LONG_PTR" /> used to initialize the instance.</param>
        public LRESULT(LONG_PTR value)
        {
            _value = value;
        }
        #endregion

        #region Comparison Operators
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
            return (left._value != right._value);
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(LRESULT left, LRESULT right)
        {
            return (left._value < right._value);
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(LRESULT left, LRESULT right)
        {
            return (left._value > right._value);
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(LRESULT left, LRESULT right)
        {
            return (left._value <= right._value);
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(LRESULT left, LRESULT right)
        {
            return (left._value >= right._value);
        }
        #endregion

        #region Cast Operators
        /// <summary>Explicitly converts a <see cref="LRESULT" /> value to a <see cref="int" /> value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> value to convert.</param>
        public static explicit operator int(LRESULT value)
        {
            return (int)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="LRESULT" /> value to a <see cref="long" /> value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> value to convert.</param>
        public static implicit operator long(LRESULT value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="LRESULT" /> value to a <see cref="nint" /> value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> value to convert.</param>
        public static implicit operator nint(LRESULT value)
        {
            return value._value;
        }

        /// <summary>Explicitly converts a <see cref="LRESULT" /> value to a <see cref="nuint" /> value.</summary>
        /// <param name="value">The <see cref="nuint" /> value to convert.</param>
        public static explicit operator nuint(LRESULT value)
        {
            return (nuint)(value._value);
        }

        /// <summary>Explicitly converts a <see cref="LRESULT" /> value to a <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> value to convert.</param>
        public static explicit operator void* (LRESULT value)
        {
            return (void*)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="LRESULT" /> value to a <see cref="LONG_PTR" /> value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> value to convert.</param>
        public static implicit operator LONG_PTR(LRESULT value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="int" /> value to a <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="int" /> value to convert.</param>
        public static implicit operator LRESULT(int value)
        {
            return new LRESULT(value);
        }

        /// <summary>Explicitly converts a <see cref="long" /> value to a <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="long" /> value to convert.</param>
        public static explicit operator LRESULT(long value)
        {
            return new LRESULT((nint)(value));
        }

        /// <summary>Implicitly converts a <see cref="nint" /> value to a <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="nint" /> value to convert.</param>
        public static implicit operator LRESULT(nint value)
        {
            return new LRESULT(value);
        }

        /// <summary>Explicitly converts a <see cref="void" />* value to a <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="void" />* value to convert.</param>
        public static explicit operator LRESULT(void* value)
        {
            return new LRESULT((nint)(value));
        }

        /// <summary>Implicitly converts a <see cref="LONG_PTR" /> value to a <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="LONG_PTR" /> value to convert.</param>
        public static implicit operator LRESULT(LONG_PTR value)
        {
            return new LRESULT(value);
        }
        #endregion

        #region System.IComparable Methods
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

        #region System.IComparable<LRESULT> Methods
        /// <summary>Compares a <see cref="LRESULT" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="LRESULT" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(LRESULT other)
        {
            var otherValue = other._value;
            return _value.CompareTo(otherValue);
        }
        #endregion

        #region System.IEquatable<LRESULT> Methods
        /// <summary>Compares a <see cref="LRESULT" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="LRESULT" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(LRESULT other)
        {
            var otherValue = other._value;
            return _value.Equals(otherValue);
        }
        #endregion

        #region System.IFormattable Methods
        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }
        #endregion

        #region System.Object Methods
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
            return _value.ToString();
        }
        #endregion
    }
}
