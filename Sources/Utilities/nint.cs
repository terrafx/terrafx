// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;

namespace TerraFX.Utilities
{
    /// <summary>Defines a native-sized signed integer.</summary>
    unsafe public /* blittable */ struct nint : IComparable, IComparable<nint>, IEquatable<nint>, IFormattable
    {
        #region Fields
        internal void* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="nint" /> struct.</summary>
        /// <param name="value">The <see cref="int" /> used to initialize the instance.</param>
        public nint(int value)
        {
            _value = (void*)(value);
        }

        /// <summary>Initializes a new instance of the <see cref="nint" /> struct.</summary>
        /// <param name="value">The <see cref="long" /> used to initialize the instance.</param>
        public nint(long value)
        {
            Debug.Assert(IntPtr.Size == sizeof(long));
            _value = (void*)(value);
        }
        #endregion

        #region Operators
        /// <summary>Negates a <see cref="nint" /> value to determine its inverse.</summary>
        /// <param name="value">The <see cref="nint" /> to negate.</param>
        /// <returns>The inverse of <paramref name="value" />.</returns>
        public static nint operator -(nint value)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return -((int)(value._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = -((long)(value._value));
                return (nint)(result);
            }
        }

        /// <summary>Computes the bitwise-complement of a <see cref="nint" /> value.</summary>
        /// <param name="value">The <see cref="nint" /> for which to compute the bitwise-complement.</param>
        /// <returns>The bitwise-complement of <paramref name="value" />.</returns>
        public static nint operator ~(nint value)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ~((int)(value._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ~((long)(value._value));
                return (nint)(result);
            }
        }

        /// <summary>Increments a <see cref="nint" /> value.</summary>
        /// <param name="value">The <see cref="nint" /> for which to increment.</param>
        /// <returns>The increment of <paramref name="value" /></returns>
        public static nint operator ++(nint value)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(value._value)) + 1;
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ((long)(value._value)) + 1;
                return (nint)(result);
            }
        }

        /// <summary>Decrements a <see cref="nint" /> value.</summary>
        /// <param name="value">The <see cref="nint" /> for which to decrement.</param>
        /// <returns>The decrement of <paramref name="value" /></returns>
        public static nint operator --(nint value)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(value._value)) - 1;
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ((long)(value._value)) - 1;
                return (nint)(result);
            }
        }

        /// <summary>Adds two <see cref="nint" /> values to compute their sum.</summary>
        /// <param name="left">The <see cref="nint" /> to add with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to add with <paramref name="left" />.</param>
        /// <returns>The sum of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static nint operator +(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) + ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ((long)(left._value)) + ((long)(right._value));
                return (nint)(result);
            }
        }

        /// <summary>Subtracts two <see cref="nint" /> values to compute their difference.</summary>
        /// <param name="left">The <see cref="nint" /> from which to subtract <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to subtract from <paramref name="left" />.</param>
        /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
        public static nint operator -(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) - ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ((long)(left._value)) - ((long)(right._value));
                return (nint)(result);
            }
        }

        /// <summary>Multiplies two <see cref="nint" /> values to compute their product.</summary>
        /// <param name="left">The <see cref="nint" /> to multiply with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to multiply with <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static nint operator *(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) * ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ((long)(left._value)) * ((long)(right._value));
                return (nint)(result);
            }
        }

        /// <summary>Divides two <see cref="nint" /> values to compute their quotient.</summary>
        /// <param name="left">The <see cref="nint" /> that will be divided by <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to divide <paramref name="left" /> by.</param>
        /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static nint operator /(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) / ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ((long)(left._value)) / ((long)(right._value));
                return (nint)(result);
            }
        }

        /// <summary>Divides two <see cref="nint" /> values to compute their remainder.</summary>
        /// <param name="left">The <see cref="nint" /> that will be divided by <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to divide <paramref name="left" /> by.</param>
        /// <returns>The remainder of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static nint operator %(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) % ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ((long)(left._value)) % ((long)(right._value));
                return (nint)(result);
            }
        }

        /// <summary>Computes the bitwise AND of two <see cref="nint" /> values.</summary>
        /// <param name="left">The <see cref="nint" /> to bitwise AND with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to bitwise AND with <paramref name="left" />.</param>
        /// <returns>The bitwise AND of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static nint operator &(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) & ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ((long)(left._value)) & ((long)(right._value));
                return (nint)(result);
            }
        }

        /// <summary>Computes the bitwise OR of two <see cref="nint" /> values.</summary>
        /// <param name="left">The <see cref="nint" /> to bitwise OR with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to bitwise OR with <paramref name="left" />.</param>
        /// <returns>The bitwise OR of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static nint operator |(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) | ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ((long)(left._value)) | ((long)(right._value));
                return (nint)(result);
            }
        }

        /// <summary>Computes the bitwise XOR of two <see cref="nint" /> values.</summary>
        /// <param name="left">The <see cref="nint" /> to bitwise XOR with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to bitwise XOR with <paramref name="left" />.</param>
        /// <returns>The bitwise XOR of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static nint operator ^(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) ^ ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ((long)(left._value)) ^ ((long)(right._value));
                return (nint)(result);
            }
        }

        /// <summary>Shifts a <see cref="nint" /> value left.</summary>
        /// <param name="value">The <see cref="nint" /> to shift left.</param>
        /// <param name="bits">The number of bits to shift <paramref name="value"/> left by.</param>
        /// <returns>The result of shifting <paramref name="value" /> left <paramref name="bits" /> times.</returns>
        public static nint operator <<(nint value, int bits)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(value._value)) << bits;
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ((long)(value._value)) << bits;
                return (nint)(result);
            }
        }

        /// <summary>Shifts a <see cref="nint" /> value right.</summary>
        /// <param name="value">The <see cref="nint" /> to shift right.</param>
        /// <param name="bits">The number of bits to shift <paramref name="value"/> left by.</param>
        /// <returns>The result of shifting <paramref name="value" /> right <paramref name="bits" /> times.</returns>
        public static nint operator >>(nint value, int bits)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(value._value)) >> bits;
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                var result = ((long)(value._value)) >> bits;
                return (nint)(result);
            }
        }

        /// <summary>Compares two <see cref="nint" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="nint" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) == ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                return ((long)(left._value)) == ((long)(right._value));
            }
        }

        /// <summary>Compares two <see cref="nint" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="nint" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) != ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                return ((long)(left._value)) != ((long)(right._value));
            }
        }

        /// <summary>Compares two <see cref="nint" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="nint" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) < ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                return ((long)(left._value)) < ((long)(right._value));
            }
        }

        /// <summary>Compares two <see cref="nint" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="nint" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) > ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                return ((long)(left._value)) > ((long)(right._value));
            }
        }

        /// <summary>Compares two <see cref="nint" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="nint" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) <= ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                return ((long)(left._value)) <= ((long)(right._value));
            }
        }

        /// <summary>Compares two <see cref="nint" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="nint" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nint" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(nint left, nint right)
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(left._value)) >= ((int)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                return ((long)(left._value)) >= ((long)(right._value));
            }
        }

        /// <summary>Explicitly converts a <see cref="nint" /> value to a <see cref="int" /> value.</summary>
        /// <param name="value">The <see cref="nint" /> value to convert.</param>
        public static explicit operator int(nint value)
        {
            return (int)(value._value);
        }

        /// <summary>Explicitly converts a <see cref="long" /> value to a <see cref="nint" /> value.</summary>
        /// <param name="value">The <see cref="long" /> value to convert.</param>
        public static explicit operator nint(long value)
        {
            return new nint(value);
        }

        /// <summary>Implicitly converts a <see cref="nint" /> value to a <see cref="long" /> value.</summary>
        /// <param name="value">The <see cref="nint" /> value to convert.</param>
        public static implicit operator long(nint value)
        {
            return (long)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="int" /> value to a <see cref="nint" /> value.</summary>
        /// <param name="value">The <see cref="int" /> value to convert.</param>
        public static implicit operator nint(int value)
        {
            return new nint(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="nint" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is nint other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<nint>
        /// <summary>Compares a <see cref="nint" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="nint" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(nint other)
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

        #region System.IEquatable<nint>
        /// <summary>Compares a <see cref="nint" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="nint" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(nint other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="nint" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is nint other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            if (IntPtr.Size == sizeof(int))
            {
                return ((int)(_value)).GetHashCode();
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(long));
                return ((long)(_value)).GetHashCode();
            }
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
