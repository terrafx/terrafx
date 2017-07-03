// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winnt.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>A locale id.</summary>
    public struct LCID : IComparable, IComparable<LCID>, IEquatable<LCID>
    {
        #region Fields
        private uint _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="LCID" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is greater than <see cref="uint.MaxValue" />.</exception>
        public LCID(char value)
        {
            if (value > uint.MaxValue)
            {
                ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(value), value);
            }

            _value = (uint)(value);
        }

        /// <summary>Initializes a new instance of the <see cref="LCID" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public LCID(uint value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="LCID" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="LCID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LCID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(LCID left, LCID right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="LCID" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="LCID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LCID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(LCID left, LCID right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="LCID" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LCID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LCID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(LCID left, LCID right)
        {
            return (left._value > right._value);
        }

        /// <summary>Compares two <see cref="LCID" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LCID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LCID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(LCID left, LCID right)
        {
            return (left._value >= right._value);
        }

        /// <summary>Compares two <see cref="LCID" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LCID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LCID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(LCID left, LCID right)
        {
            return (left._value < right._value);
        }

        /// <summary>Compares two <see cref="LCID" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="LCID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LCID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(LCID left, LCID right)
        {
            return (left._value <= right._value);
        }

        /// <summary>Converts a <see cref="LCID" /> to an equivalent <see cref="char" /> value.</summary>
        /// <param name="value">The <see cref="LCID" /> to convert.</param>
        public static implicit operator char(LCID value)
        {
            return (char)(value._value);
        }

        /// <summary>Converts a <see cref="LCID" /> to an equivalent <see cref="uint" /> value.</summary>
        /// <param name="value">The <see cref="LCID" /> to convert.</param>
        public static implicit operator uint(LCID value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="char" /> to an equivalent <see cref="LCID" /> value.</summary>
        /// <param name="value">The <see cref="char" /> to convert.</param>
        public static explicit operator LCID(char value)
        {
            return new LCID(value);
        }

        /// <summary>Converts a <see cref="uint" /> to an equivalent <see cref="LCID" /> value.</summary>
        /// <param name="value">The <see cref="uint" /> to convert.</param>
        public static implicit operator LCID(uint value)
        {
            return new LCID(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="LCID" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is LCID other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<LCID>
        /// <summary>Compares a <see cref="LCID" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="LCID" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(LCID other)
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

        #region System.IEquatable<LCID>
        /// <summary>Compares a <see cref="LCID" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="LCID" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(LCID other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="LCID" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is LCID other)
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
