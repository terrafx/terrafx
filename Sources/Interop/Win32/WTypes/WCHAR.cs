// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypesbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>A 16-bit Unicode character.</summary>
    public struct WCHAR : IComparable, IComparable<WCHAR>, IEquatable<WCHAR>
    {
        #region Fields
        private ushort _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="WCHAR" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is greater than <see cref="ushort.MaxValue" />.</exception>
        public WCHAR(char value) : this((ushort)(value))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="WCHAR" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public WCHAR(ushort value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="WCHAR" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="WCHAR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="WCHAR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(WCHAR left, WCHAR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="WCHAR" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="WCHAR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="WCHAR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(WCHAR left, WCHAR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="WCHAR" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="WCHAR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="WCHAR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(WCHAR left, WCHAR right)
        {
            return (left._value > right._value);
        }

        /// <summary>Compares two <see cref="WCHAR" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="WCHAR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="WCHAR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(WCHAR left, WCHAR right)
        {
            return (left._value >= right._value);
        }

        /// <summary>Compares two <see cref="WCHAR" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="WCHAR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="WCHAR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(WCHAR left, WCHAR right)
        {
            return (left._value < right._value);
        }

        /// <summary>Compares two <see cref="WCHAR" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="WCHAR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="WCHAR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(WCHAR left, WCHAR right)
        {
            return (left._value <= right._value);
        }

        /// <summary>Converts a <see cref="WCHAR" /> to an equivalent <see cref="char" /> value.</summary>
        /// <param name="value">The <see cref="WCHAR" /> to convert.</param>
        public static implicit operator char(WCHAR value)
        {
            return (char)(value._value);
        }

        /// <summary>Converts a <see cref="WCHAR" /> to an equivalent <see cref="ushort" /> value.</summary>
        /// <param name="value">The <see cref="WCHAR" /> to convert.</param>
        public static implicit operator ushort(WCHAR value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="char" /> to an equivalent <see cref="WCHAR" /> value.</summary>
        /// <param name="value">The <see cref="char" /> to convert.</param>
        public static implicit operator WCHAR(char value)
        {
            return new WCHAR(value);
        }

        /// <summary>Converts a <see cref="ushort" /> to an equivalent <see cref="WCHAR" /> value.</summary>
        /// <param name="value">The <see cref="ushort" /> to convert.</param>
        public static implicit operator WCHAR(ushort value)
        {
            return new WCHAR(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="WCHAR" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is WCHAR other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<WCHAR>
        /// <summary>Compares a <see cref="WCHAR" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="WCHAR" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(WCHAR other)
        {
            return (_value - other._value);
        }
        #endregion

        #region System.IEquatable<WCHAR>
        /// <summary>Compares a <see cref="WCHAR" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="WCHAR" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(WCHAR other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="WCHAR" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is WCHAR other)
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
