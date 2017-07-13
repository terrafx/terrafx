// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;

namespace TerraFX.Utilities
{
    /// <summary>Defines a native-sized unsigned integer.</summary>
    unsafe public /* blittable */ struct nuint : IComparable, IComparable<nuint>, IEquatable<nuint>, IFormattable
    {
        #region Fields
        internal void* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="nuint" /> struct.</summary>
        /// <param name="value">The <see cref="uint" /> used to initialize the instance.</param>
        public nuint(uint value)
        {
            _value = (void*)(value);
        }

        /// <summary>Initializes a new instance of the <see cref="nuint" /> struct.</summary>
        /// <param name="value">The <see cref="ulong" /> used to initialize the instance.</param>
        public nuint(ulong value)
        {
            Debug.Assert(IntPtr.Size == sizeof(ulong));
            _value = (void*)(value);
        }

        /// <summary>Initializes a new instance of the <see cref="nuint" /> struct.</summary>
        /// <param name="value">The <see cref="UIntPtr" /> used to initialize the instance.</param>
        public nuint(UIntPtr value)
        {
            _value = (void*)(value);
        }

        /// <summary>Initializes a new instance of the <see cref="nuint" /> struct.</summary>
        /// <param name="value">The <see cref="void" />* used to initialize the instance.</param>
        public nuint(void* value)
        {
            _value = value;
        }
        #endregion

        #region Unary Operators
        /// <summary>Computes the bitwise-complement of a <see cref="nuint" /> value.</summary>
        /// <param name="value">The <see cref="nuint" /> for which to compute the bitwise-complement.</param>
        /// <returns>The bitwise-complement of <paramref name="value" />.</returns>
        public static nuint operator ~(nuint value)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ~((uint)(value._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ~((ulong)(value._value));
                return (nuint)(result);
            }
        }

        /// <summary>Increments a <see cref="nuint" /> value.</summary>
        /// <param name="value">The <see cref="nuint" /> for which to increment.</param>
        /// <returns>The increment of <paramref name="value" /></returns>
        public static nuint operator ++(nuint value)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(value._value)) + 1;
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ((ulong)(value._value)) + 1;
                return (nuint)(result);
            }
        }

        /// <summary>Decrements a <see cref="nuint" /> value.</summary>
        /// <param name="value">The <see cref="nuint" /> for which to decrement.</param>
        /// <returns>The decrement of <paramref name="value" /></returns>
        public static nuint operator --(nuint value)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(value._value)) - 1;
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ((ulong)(value._value)) - 1;
                return (nuint)(result);
            }
        }
        #endregion

        #region Binary Operators
        /// <summary>Adds two <see cref="nuint" /> values to compute their sum.</summary>
        /// <param name="left">The <see cref="nuint" /> to add with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to add with <paramref name="left" />.</param>
        /// <returns>The sum of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static nuint operator +(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) + ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ((ulong)(left._value)) + ((ulong)(right._value));
                return (nuint)(result);
            }
        }

        /// <summary>Subtracts two <see cref="nuint" /> values to compute their difference.</summary>
        /// <param name="left">The <see cref="nuint" /> from which to subtract <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to subtract from <paramref name="left" />.</param>
        /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
        public static nuint operator -(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) - ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ((ulong)(left._value)) - ((ulong)(right._value));
                return (nuint)(result);
            }
        }

        /// <summary>Multiplies two <see cref="nuint" /> values to compute their product.</summary>
        /// <param name="left">The <see cref="nuint" /> to multiply with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to multiply with <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static nuint operator *(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) * ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ((ulong)(left._value)) * ((ulong)(right._value));
                return (nuint)(result);
            }
        }

        /// <summary>Divides two <see cref="nuint" /> values to compute their quotient.</summary>
        /// <param name="left">The <see cref="nuint" /> that will be divided by <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to divide <paramref name="left" /> by.</param>
        /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static nuint operator /(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) / ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ((ulong)(left._value)) / ((ulong)(right._value));
                return (nuint)(result);
            }
        }

        /// <summary>Divides two <see cref="nuint" /> values to compute their remainder.</summary>
        /// <param name="left">The <see cref="nuint" /> that will be divided by <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to divide <paramref name="left" /> by.</param>
        /// <returns>The remainder of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static nuint operator %(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) % ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ((ulong)(left._value)) % ((ulong)(right._value));
                return (nuint)(result);
            }
        }

        /// <summary>Computes the bitwise AND of two <see cref="nuint" /> values.</summary>
        /// <param name="left">The <see cref="nuint" /> to bitwise AND with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to bitwise AND with <paramref name="left" />.</param>
        /// <returns>The bitwise AND of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static nuint operator &(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) & ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ((ulong)(left._value)) & ((ulong)(right._value));
                return (nuint)(result);
            }
        }

        /// <summary>Computes the bitwise OR of two <see cref="nuint" /> values.</summary>
        /// <param name="left">The <see cref="nuint" /> to bitwise OR with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to bitwise OR with <paramref name="left" />.</param>
        /// <returns>The bitwise OR of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static nuint operator |(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) | ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ((ulong)(left._value)) | ((ulong)(right._value));
                return (nuint)(result);
            }
        }

        /// <summary>Computes the bitwise XOR of two <see cref="nuint" /> values.</summary>
        /// <param name="left">The <see cref="nuint" /> to bitwise XOR with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to bitwise XOR with <paramref name="left" />.</param>
        /// <returns>The bitwise XOR of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static nuint operator ^(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) ^ ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ((ulong)(left._value)) ^ ((ulong)(right._value));
                return (nuint)(result);
            }
        }

        /// <summary>Shifts a <see cref="nuint" /> value left.</summary>
        /// <param name="value">The <see cref="nuint" /> to shift left.</param>
        /// <param name="bits">The number of bits to shift <paramref name="value"/> left by.</param>
        /// <returns>The result of shifting <paramref name="value" /> left <paramref name="bits" /> times.</returns>
        public static nuint operator <<(nuint value, int bits)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(value._value)) << bits;
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ((ulong)(value._value)) << bits;
                return (nuint)(result);
            }
        }

        /// <summary>Shifts a <see cref="nuint" /> value right.</summary>
        /// <param name="value">The <see cref="nuint" /> to shift right.</param>
        /// <param name="bits">The number of bits to shift <paramref name="value"/> left by.</param>
        /// <returns>The result of shifting <paramref name="value" /> right <paramref name="bits" /> times.</returns>
        public static nuint operator >>(nuint value, int bits)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(value._value)) >> bits;
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                var result = ((ulong)(value._value)) >> bits;
                return (nuint)(result);
            }
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="nuint" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="nuint" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) == ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                return ((ulong)(left._value)) == ((ulong)(right._value));
            }
        }

        /// <summary>Compares two <see cref="nuint" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="nuint" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) != ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                return ((ulong)(left._value)) != ((ulong)(right._value));
            }
        }

        /// <summary>Compares two <see cref="nuint" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="nuint" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) < ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                return ((ulong)(left._value)) < ((ulong)(right._value));
            }
        }

        /// <summary>Compares two <see cref="nuint" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="nuint" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) > ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                return ((ulong)(left._value)) > ((ulong)(right._value));
            }
        }

        /// <summary>Compares two <see cref="nuint" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="nuint" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) <= ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                return ((ulong)(left._value)) <= ((ulong)(right._value));
            }
        }

        /// <summary>Compares two <see cref="nuint" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="nuint" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="nuint" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(nuint left, nuint right)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(left._value)) >= ((uint)(right._value));
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                return ((ulong)(left._value)) >= ((ulong)(right._value));
            }
        }
        #endregion

        #region Cast Operators
        /// <summary>Explicitly converts a <see cref="nuint" /> value to a <see cref="nint" /> value.</summary>
        /// <param name="value">The <see cref="nuint" /> value to convert.</param>
        public static explicit operator nint(nuint value)
        {
            return (nint)(value._value);
        }

        /// <summary>Explicitly converts a <see cref="nuint" /> value to a <see cref="uint" /> value.</summary>
        /// <param name="value">The <see cref="nuint" /> value to convert.</param>
        public static explicit operator uint(nuint value)
        {
            return (uint)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="nuint" /> value to a <see cref="ulong" /> value.</summary>
        /// <param name="value">The <see cref="nuint" /> value to convert.</param>
        public static implicit operator ulong(nuint value)
        {
            return (ulong)(value._value);
        }

        /// <summary>Explicitly converts a <see cref="nuint" /> value to a <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="nuint" /> value to convert.</param>
        public static explicit operator void* (nuint value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="uint" /> value to a <see cref="nuint" /> value.</summary>
        /// <param name="value">The <see cref="uint" /> value to convert.</param>
        public static implicit operator nuint(uint value)
        {
            return new nuint(value);
        }

        /// <summary>Explicitly converts a <see cref="ulong" /> value to a <see cref="nuint" /> value.</summary>
        /// <param name="value">The <see cref="ulong" /> value to convert.</param>
        public static explicit operator nuint(ulong value)
        {
            return new nuint(value);
        }

        /// <summary>Explicitly converts a <see cref="void" />* value to a <see cref="nuint" /> value.</summary>
        /// <param name="value">The <see cref="void" />* value to convert.</param>
        public static explicit operator nuint(void* value)
        {
            return new nuint(value);
        }
        #endregion

        #region System.IComparable Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="nuint" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is nuint other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<nuint> Methods
        /// <summary>Compares a <see cref="nuint" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="nuint" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(nuint other)
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

        #region System.IEquatable<nuint> Methods
        /// <summary>Compares a <see cref="nuint" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="nuint" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(nuint other)
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
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(_value)).ToString(format, formatProvider);
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                return ((ulong)(_value)).ToString(format, formatProvider);
            }
        }
        #endregion

        #region System.Object Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="nuint" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is nuint other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(_value)).GetHashCode();
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                return ((ulong)(_value)).GetHashCode();
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
