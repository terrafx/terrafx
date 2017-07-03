// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>A member id.</summary>
    public struct MEMBERID : IComparable, IComparable<MEMBERID>, IEquatable<MEMBERID>
    {
        #region Fields
        private int _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="MEMBERID" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public MEMBERID(int value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="MEMBERID" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="MEMBERID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="MEMBERID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(MEMBERID left, MEMBERID right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="MEMBERID" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="MEMBERID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="MEMBERID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(MEMBERID left, MEMBERID right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="MEMBERID" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="MEMBERID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="MEMBERID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(MEMBERID left, MEMBERID right)
        {
            return (left._value > right._value);
        }

        /// <summary>Compares two <see cref="MEMBERID" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="MEMBERID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="MEMBERID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(MEMBERID left, MEMBERID right)
        {
            return (left._value >= right._value);
        }

        /// <summary>Compares two <see cref="MEMBERID" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="MEMBERID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="MEMBERID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(MEMBERID left, MEMBERID right)
        {
            return (left._value < right._value);
        }

        /// <summary>Compares two <see cref="MEMBERID" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="MEMBERID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="MEMBERID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(MEMBERID left, MEMBERID right)
        {
            return (left._value <= right._value);
        }

        /// <summary>Converts a <see cref="MEMBERID" /> to an equivalent <see cref="int" /> value.</summary>
        /// <param name="value">The <see cref="MEMBERID" /> to convert.</param>
        public static implicit operator int(MEMBERID value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="int" /> to an equivalent <see cref="MEMBERID" /> value.</summary>
        /// <param name="value">The <see cref="int" /> to convert.</param>
        public static implicit operator MEMBERID(int value)
        {
            return new MEMBERID(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="MEMBERID" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is MEMBERID other)
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
        /// <summary>Compares a <see cref="MEMBERID" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="MEMBERID" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(MEMBERID other)
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

        #region System.IEquatable<CHAR>
        /// <summary>Compares a <see cref="MEMBERID" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="MEMBERID" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(MEMBERID other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="MEMBERID" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is MEMBERID other)
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
