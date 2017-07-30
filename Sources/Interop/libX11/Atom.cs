// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    /// <summary>A unique ID corresponding to a string name.</summary>
    unsafe public /* blittable */ struct Atom : IComparable, IComparable<Atom>, IEquatable<Atom>, IFormattable
    {
        #region Fields
        internal nuint _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Atom" /> struct.</summary>
        /// <param name="value">The <see cref="nuint" /> used to initialize the instance.</param>
        public Atom(nuint value)
        {
            _value = value;
        }
        #endregion

        #region Unary Operators
        /// <summary>Computes the bitwise-complement of a <see cref="Atom" /> value.</summary>
        /// <param name="value">The <see cref="Atom" /> for which to compute the bitwise-complement.</param>
        /// <returns>The bitwise-complement of <paramref name="value" />.</returns>
        public static Atom operator ~(Atom value)
        {
            return ~value._value;
        }

        /// <summary>Increments a <see cref="Atom" /> value.</summary>
        /// <param name="value">The <see cref="Atom" /> for which to increment.</param>
        /// <returns>The increment of <paramref name="value" /></returns>
        public static Atom operator ++(Atom value)
        {
            return (value._value + 1);
        }

        /// <summary>Decrements a <see cref="Atom" /> value.</summary>
        /// <param name="value">The <see cref="Atom" /> for which to decrement.</param>
        /// <returns>The decrement of <paramref name="value" /></returns>
        public static Atom operator --(Atom value)
        {
            return (value._value - 1);
        }
        #endregion

        #region Binary Operators
        /// <summary>Adds two <see cref="Atom" /> values to compute their sum.</summary>
        /// <param name="left">The <see cref="Atom" /> to add with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to add with <paramref name="left" />.</param>
        /// <returns>The sum of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Atom operator +(Atom left, Atom right)
        {
            return (left._value + right._value);
        }

        /// <summary>Subtracts two <see cref="Atom" /> values to compute their difference.</summary>
        /// <param name="left">The <see cref="Atom" /> from which to subtract <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to subtract from <paramref name="left" />.</param>
        /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
        public static Atom operator -(Atom left, Atom right)
        {
            return (left._value - right._value);
        }

        /// <summary>Multiplies two <see cref="Atom" /> values to compute their product.</summary>
        /// <param name="left">The <see cref="Atom" /> to multiply with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to multiply with <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Atom operator *(Atom left, Atom right)
        {
            return (left._value * right._value);
        }

        /// <summary>Divides two <see cref="Atom" /> values to compute their quotient.</summary>
        /// <param name="left">The <see cref="Atom" /> that will be divided by <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to divide <paramref name="left" /> by.</param>
        /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static Atom operator /(Atom left, Atom right)
        {
            return (left._value / right._value);
        }

        /// <summary>Divides two <see cref="Atom" /> values to compute their remainder.</summary>
        /// <param name="left">The <see cref="Atom" /> that will be divided by <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to divide <paramref name="left" /> by.</param>
        /// <returns>The remainder of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static Atom operator %(Atom left, Atom right)
        {
            return (left._value % right._value);
        }

        /// <summary>Computes the bitwise AND of two <see cref="Atom" /> values.</summary>
        /// <param name="left">The <see cref="Atom" /> to bitwise AND with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to bitwise AND with <paramref name="left" />.</param>
        /// <returns>The bitwise AND of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Atom operator &(Atom left, Atom right)
        {
            return (left._value & right._value);
        }

        /// <summary>Computes the bitwise OR of two <see cref="Atom" /> values.</summary>
        /// <param name="left">The <see cref="Atom" /> to bitwise OR with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to bitwise OR with <paramref name="left" />.</param>
        /// <returns>The bitwise OR of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Atom operator |(Atom left, Atom right)
        {
            return (left._value | right._value);
        }

        /// <summary>Computes the bitwise XOR of two <see cref="Atom" /> values.</summary>
        /// <param name="left">The <see cref="Atom" /> to bitwise XOR with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to bitwise XOR with <paramref name="left" />.</param>
        /// <returns>The bitwise XOR of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Atom operator ^(Atom left, Atom right)
        {
            return (left._value ^ right._value);
        }

        /// <summary>Shifts a <see cref="Atom" /> value left.</summary>
        /// <param name="value">The <see cref="Atom" /> to shift left.</param>
        /// <param name="bits">The number of bits to shift <paramref name="value"/> left by.</param>
        /// <returns>The result of shifting <paramref name="value" /> left <paramref name="bits" /> times.</returns>
        public static Atom operator <<(Atom value, int bits)
        {
            return (value._value << bits);
        }

        /// <summary>Shifts a <see cref="Atom" /> value right.</summary>
        /// <param name="value">The <see cref="Atom" /> to shift right.</param>
        /// <param name="bits">The number of bits to shift <paramref name="value"/> left by.</param>
        /// <returns>The result of shifting <paramref name="value" /> right <paramref name="bits" /> times.</returns>
        public static Atom operator >>(Atom value, int bits)
        {
            return (value._value >> bits);
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="Atom" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Atom" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Atom left, Atom right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="Atom" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Atom" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Atom left, Atom right)
        {
            return (left._value != right._value);
        }

        /// <summary>Compares two <see cref="Atom" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="Atom" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(Atom left, Atom right)
        {
            return (left._value < right._value);
        }

        /// <summary>Compares two <see cref="Atom" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="Atom" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(Atom left, Atom right)
        {
            return (left._value > right._value);
        }

        /// <summary>Compares two <see cref="Atom" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="Atom" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Atom left, Atom right)
        {
            return (left._value <= right._value);
        }

        /// <summary>Compares two <see cref="Atom" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="Atom" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Atom" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Atom left, Atom right)
        {
            return (left._value >= right._value);
        }
        #endregion

        #region Cast Operators
        /// <summary>Explicitly converts a <see cref="Atom" /> value to a <see cref="nint" /> value.</summary>
        /// <param name="value">The <see cref="Atom" /> value to convert.</param>
        public static explicit operator nint(Atom value)
        {
            return (nint)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="Atom" /> value to a <see cref="nuint" /> value.</summary>
        /// <param name="value">The <see cref="Atom" /> value to convert.</param>
        public static implicit operator nuint(Atom value)
        {
            return value._value;
        }

        /// <summary>Explicitly converts a <see cref="Atom" /> value to a <see cref="uint" /> value.</summary>
        /// <param name="value">The <see cref="Atom" /> value to convert.</param>
        public static explicit operator uint(Atom value)
        {
            return (uint)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="Atom" /> value to a <see cref="ulong" /> value.</summary>
        /// <param name="value">The <see cref="Atom" /> value to convert.</param>
        public static implicit operator ulong(Atom value)
        {
            return value._value;
        }

        /// <summary>Explicitly converts a <see cref="Atom" /> value to a <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="Atom" /> value to convert.</param>
        public static explicit operator void* (Atom value)
        {
            return (void*)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="uint" /> value to a <see cref="Atom" /> value.</summary>
        /// <param name="value">The <see cref="uint" /> value to convert.</param>
        public static implicit operator Atom(uint value)
        {
            return new Atom(value);
        }

        /// <summary>Explicitly converts a <see cref="ulong" /> value to a <see cref="Atom" /> value.</summary>
        /// <param name="value">The <see cref="ulong" /> value to convert.</param>
        public static explicit operator Atom(ulong value)
        {
            return new Atom((nuint)(value));
        }

        /// <summary>Implicitly converts a <see cref="nuint" /> value to a <see cref="Atom" /> value.</summary>
        /// <param name="value">The <see cref="nuint" /> value to convert.</param>
        public static implicit operator Atom(nuint value)
        {
            return new Atom(value);
        }

        /// <summary>Explicitly converts a <see cref="void" />* value to a <see cref="Atom" /> value.</summary>
        /// <param name="value">The <see cref="void" />* value to convert.</param>
        public static explicit operator Atom(void* value)
        {
            return new Atom((nuint)(value));
        }
        #endregion

        #region System.IComparable Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="Atom" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is Atom other)
            {
                return CompareTo(other);
            }
            else
            {
                throw NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<Atom> Methods
        /// <summary>Compares a <see cref="Atom" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="Atom" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(Atom other)
        {
            var otherValue = other._value;
            return _value.CompareTo(otherValue);
        }
        #endregion

        #region System.IEquatable<Atom> Methods
        /// <summary>Compares a <see cref="Atom" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Atom" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Atom other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Atom" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Atom other)
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
