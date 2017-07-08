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

        #region Operators
        /// <summary>Negates a <see cref="LRESULT" /> value to determine its inverse.</summary>
        /// <param name="value">The <see cref="LRESULT" /> to negate.</param>
        /// <returns>The inverse of <paramref name="value" />.</returns>
        public static LRESULT operator -(LRESULT value)
        {
            return -value._value;
        }

        /// <summary>Computes the bitwise-complement of a <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> for which to compute the bitwise-complement.</param>
        /// <returns>The bitwise-complement of <paramref name="value" />.</returns>
        public static LRESULT operator ~(LRESULT value)
        {
            return ~value._value;
        }

        /// <summary>Increments a <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> for which to increment.</param>
        /// <returns>The increment of <paramref name="value" /></returns>
        public static LRESULT operator ++(LRESULT value)
        {
            return value._value + 1;
        }

        /// <summary>Decrements a <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> for which to decrement.</param>
        /// <returns>The decrement of <paramref name="value" /></returns>
        public static LRESULT operator --(LRESULT value)
        {
            return value._value - 1;
        }

        /// <summary>Adds two <see cref="LRESULT" /> values to compute their sum.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to add with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to add with <paramref name="left" />.</param>
        /// <returns>The sum of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static LRESULT operator +(LRESULT left, LRESULT right)
        {
            return left._value + right._value;
        }

        /// <summary>Subtracts two <see cref="LRESULT" /> values to compute their difference.</summary>
        /// <param name="left">The <see cref="LRESULT" /> from which to subtract <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to subtract from <paramref name="left" />.</param>
        /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
        public static LRESULT operator -(LRESULT left, LRESULT right)
        {
            return left._value - right._value;
        }

        /// <summary>Multiplies two <see cref="LRESULT" /> values to compute their product.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to multiply with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to multiply with <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static LRESULT operator *(LRESULT left, LRESULT right)
        {
            return left._value * right._value;
        }

        /// <summary>Divides two <see cref="LRESULT" /> values to compute their quotient.</summary>
        /// <param name="left">The <see cref="LRESULT" /> that will be divided by <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to divide <paramref name="left" /> by.</param>
        /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static LRESULT operator /(LRESULT left, LRESULT right)
        {
            return left._value / right._value;
        }

        /// <summary>Divides two <see cref="LRESULT" /> values to compute their remainder.</summary>
        /// <param name="left">The <see cref="LRESULT" /> that will be divided by <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to divide <paramref name="left" /> by.</param>
        /// <returns>The remainder of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static LRESULT operator %(LRESULT left, LRESULT right)
        {
            return left._value % right._value;
        }

        /// <summary>Computes the bitwise AND of two <see cref="LRESULT" /> values.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to bitwise AND with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to bitwise AND with <paramref name="left" />.</param>
        /// <returns>The bitwise AND of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static LRESULT operator &(LRESULT left, LRESULT right)
        {
            return left._value & right._value;
        }

        /// <summary>Computes the bitwise OR of two <see cref="LRESULT" /> values.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to bitwise OR with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to bitwise OR with <paramref name="left" />.</param>
        /// <returns>The bitwise OR of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static LRESULT operator |(LRESULT left, LRESULT right)
        {
            return left._value | right._value;
        }

        /// <summary>Computes the bitwise XOR of two <see cref="LRESULT" /> values.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to bitwise XOR with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to bitwise XOR with <paramref name="left" />.</param>
        /// <returns>The bitwise XOR of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static LRESULT operator ^(LRESULT left, LRESULT right)
        {
            return left._value ^ right._value;
        }

        /// <summary>Shifts a <see cref="LRESULT" /> value left.</summary>
        /// <param name="value">The <see cref="LRESULT" /> to shift left.</param>
        /// <param name="bits">The number of bits to shift <paramref name="value"/> left by.</param>
        /// <returns>The result of shifting <paramref name="value" /> left <paramref name="bits" /> times.</returns>
        public static LRESULT operator <<(LRESULT value, int bits)
        {
            return value._value << bits;
        }

        /// <summary>Shifts a <see cref="LRESULT" /> value right.</summary>
        /// <param name="value">The <see cref="LRESULT" /> to shift right.</param>
        /// <param name="bits">The number of bits to shift <paramref name="value"/> left by.</param>
        /// <returns>The result of shifting <paramref name="value" /> right <paramref name="bits" /> times.</returns>
        public static LRESULT operator >>(LRESULT value, int bits)
        {
            return value._value >> bits;
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(LRESULT left, LRESULT right)
        {
            return left._value == right._value;
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(LRESULT left, LRESULT right)
        {
            return left._value != right._value;
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(LRESULT left, LRESULT right)
        {
            return left._value < right._value;
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(LRESULT left, LRESULT right)
        {
            return left._value > right._value;
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(LRESULT left, LRESULT right)
        {
            return left._value <= right._value;
        }

        /// <summary>Compares two <see cref="LRESULT" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LRESULT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LRESULT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(LRESULT left, LRESULT right)
        {
            return left._value >= right._value;
        }

        /// <summary>Explicitly converts a <see cref="LRESULT" /> value to a <see cref="int" /> value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> value to convert.</param>
        public static explicit operator int(LRESULT value)
        {
            return (int)(value._value);
        }

        /// <summary>Explicitly converts a <see cref="LRESULT" /> value to a <see cref="nuint" /> value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> value to convert.</param>
        public static explicit operator nuint(LRESULT value)
        {
            return (nuint)(value._value);
        }

        /// <summary>Explicitly converts a <see cref="long" /> value to a <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="long" /> value to convert.</param>
        public static explicit operator LRESULT(long value)
        {
            return new LRESULT((nint)(value));
        }

        /// <summary>Explicitly converts a <see cref="void" />* value to a <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="void" />* value to convert.</param>
        public static explicit operator LRESULT(void* value)
        {
            return new LRESULT((nint)(value));
        }

        /// <summary>Explicitly converts a <see cref="LRESULT" /> value to a <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="LRESULT" /> value to convert.</param>
        public static explicit operator void* (LRESULT value)
        {
            return (void*)(value._value);
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

        /// <summary>Implicitly converts a <see cref="nint" /> value to a <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="nint" /> value to convert.</param>
        public static implicit operator LRESULT(nint value)
        {
            return new LRESULT(value);
        }

        /// <summary>Implicitly converts a <see cref="LONG_PTR" /> value to a <see cref="LRESULT" /> value.</summary>
        /// <param name="value">The <see cref="LONG_PTR" /> value to convert.</param>
        public static implicit operator LRESULT(LONG_PTR value)
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
            return _value.CompareTo(other._value);
        }
        #endregion

        #region System.IEquatable<LRESULT>
        /// <summary>Compares a <see cref="LRESULT" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="LRESULT" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(LRESULT other)
        {
            return _value.Equals(other._value);
        }
        #endregion

        #region System.IFormattable
        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
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
            return _value.ToString();
        }
        #endregion
    }
}
