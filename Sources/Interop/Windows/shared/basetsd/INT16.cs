// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\basetsd.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct INT16 : IComparable, IComparable<INT16>, IEquatable<INT16>, IFormattable
    {
        #region Fields
        internal short _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="INT16" /> struct.</summary>
        /// <param name="value">The <see cref="short" /> used to initialize the instance.</param>
        public INT16(short value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Explicitly converts a <see cref="INT16" /> value to a <see cref="ushort" /> value.</summary>
        /// <param name="value">The <see cref="INT16" /> value to convert.</param>
        public static explicit operator ushort(INT16 value)
        {
            return (ushort)(value._value);
        }

        /// <summary>Explicitly converts a <see cref="INT16" /> value to a <see cref="ushort" /> value.</summary>
        /// <param name="value">The <see cref="INT16" /> value to convert.</param>
        public static explicit operator INT16(ushort value)
        {
            return new INT16((short)(value));
        }

        /// <summary>Implicitly converts a <see cref="INT16" /> value to a <see cref="short" /> value.</summary>
        /// <param name="value">The <see cref="INT16" /> value to convert.</param>
        public static implicit operator short(INT16 value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="short" /> value to a <see cref="INT16" /> value.</summary>
        /// <param name="value">The <see cref="short" /> value to convert.</param>
        public static implicit operator INT16(short value)
        {
            return new INT16(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="INT16" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is INT16 other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<INT16>
        /// <summary>Compares a <see cref="INT16" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="INT16" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(INT16 other)
        {
            return _value.CompareTo(other._value);
        }
        #endregion

        #region System.IEquatable<INT16>
        /// <summary>Compares a <see cref="INT16" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="INT16" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(INT16 other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="INT16" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is INT16 other)
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
