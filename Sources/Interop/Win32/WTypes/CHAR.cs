// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>An 8-bit ANSI character.</summary>
    public struct CHAR : IComparable, IComparable<CHAR>, IEquatable<CHAR>
    {
        #region Fields
        private byte _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="CHAR" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is greater than <see cref="byte.MaxValue" />.</exception>
        public CHAR(char value)
        {
            if (value > byte.MaxValue)
            {
                ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(value), value);
            }

            _value = (byte)(value);
        }

        /// <summary>Initializes a new instance of the <see cref="CHAR" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public CHAR(byte value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="CHAR" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="CHAR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="CHAR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(CHAR left, CHAR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="CHAR" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="CHAR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="CHAR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(CHAR left, CHAR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="CHAR" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="CHAR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="CHAR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(CHAR left, CHAR right)
        {
            return (left._value > right._value);
        }

        /// <summary>Compares two <see cref="CHAR" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="CHAR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="CHAR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(CHAR left, CHAR right)
        {
            return (left._value >= right._value);
        }

        /// <summary>Compares two <see cref="CHAR" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="CHAR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="CHAR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(CHAR left, CHAR right)
        {
            return (left._value < right._value);
        }

        /// <summary>Compares two <see cref="CHAR" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="CHAR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="CHAR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(CHAR left, CHAR right)
        {
            return (left._value <= right._value);
        }

        /// <summary>Converts a <see cref="CHAR" /> to an equivalent <see cref="char" /> value.</summary>
        /// <param name="value">The <see cref="CHAR" /> to convert.</param>
        public static implicit operator char(CHAR value)
        {
            return (char)(value._value);
        }

        /// <summary>Converts a <see cref="CHAR" /> to an equivalent <see cref="byte" /> value.</summary>
        /// <param name="value">The <see cref="CHAR" /> to convert.</param>
        public static implicit operator byte(CHAR value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="char" /> to an equivalent <see cref="CHAR" /> value.</summary>
        /// <param name="value">The <see cref="char" /> to convert.</param>
        public static explicit operator CHAR(char value)
        {
            return new CHAR(value);
        }

        /// <summary>Converts a <see cref="byte" /> to an equivalent <see cref="CHAR" /> value.</summary>
        /// <param name="value">The <see cref="byte" /> to convert.</param>
        public static implicit operator CHAR(byte value)
        {
            return new CHAR(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="CHAR" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is CHAR other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<CHAR>
        /// <summary>Compares a <see cref="CHAR" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="CHAR" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(CHAR other)
        {
            return (_value - other._value);
        }
        #endregion

        #region System.IEquatable<CHAR>
        /// <summary>Compares a <see cref="CHAR" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="CHAR" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(CHAR other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="CHAR" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is CHAR other)
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
            return ((char)(_value)).ToString();
        }
        #endregion
    }
}
