// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\wtypesbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DOUBLE : IComparable, IComparable<DOUBLE>, IEquatable<DOUBLE>, IFormattable
    {
        #region Fields
        internal double _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="DOUBLE" /> struct.</summary>
        /// <param name="value">The <see cref="double" /> used to initialize the instance.</param>
        public DOUBLE(double value)
        {
            _value = value;
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="DOUBLE" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="DOUBLE" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="DOUBLE" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(DOUBLE left, DOUBLE right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="DOUBLE" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="DOUBLE" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="DOUBLE" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(DOUBLE left, DOUBLE right)
        {
            return (left._value != right._value);
        }

        /// <summary>Compares two <see cref="DOUBLE" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="DOUBLE" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="DOUBLE" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(DOUBLE left, DOUBLE right)
        {
            return (left._value < right._value);
        }

        /// <summary>Compares two <see cref="DOUBLE" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="DOUBLE" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="DOUBLE" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(DOUBLE left, DOUBLE right)
        {
            return (left._value > right._value);
        }

        /// <summary>Compares two <see cref="DOUBLE" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="DOUBLE" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="DOUBLE" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(DOUBLE left, DOUBLE right)
        {
            return (left._value <= right._value);
        }

        /// <summary>Compares two <see cref="DOUBLE" /> instances to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="DOUBLE" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="DOUBLE" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(DOUBLE left, DOUBLE right)
        {
            return (left._value >= right._value);
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="DOUBLE" /> value to a <see cref="double" /> value.</summary>
        /// <param name="value">The <see cref="DOUBLE" /> value to convert.</param>
        public static implicit operator double(DOUBLE value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="double" /> value to a <see cref="DOUBLE" /> value.</summary>
        /// <param name="value">The <see cref="double" /> value to convert.</param>
        public static implicit operator DOUBLE(double value)
        {
            return new DOUBLE(value);
        }
        #endregion

        #region System.IComparable Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="DOUBLE" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is DOUBLE other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<DOUBLE> Methods
        /// <summary>Compares a <see cref="DOUBLE" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="DOUBLE" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(DOUBLE other)
        {
            var otherValue = other._value;
            return _value.CompareTo(otherValue);
        }
        #endregion

        #region System.IEquatable<DOUBLE> Methods
        /// <summary>Compares a <see cref="DOUBLE" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="DOUBLE" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(DOUBLE other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="DOUBLE" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is DOUBLE other)
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
