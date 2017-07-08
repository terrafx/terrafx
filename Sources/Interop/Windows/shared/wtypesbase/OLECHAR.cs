// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\wtypesbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct OLECHAR : IComparable, IComparable<OLECHAR>, IEquatable<OLECHAR>, IFormattable
    {
        #region Fields
        internal WCHAR _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="OLECHAR" /> struct.</summary>
        /// <param name="value">The <see cref="WCHAR" /> used to initialize the instance.</param>
        public OLECHAR(WCHAR value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Explicitly converts a <see cref="OLECHAR" /> value to a <see cref="short" /> value.</summary>
        /// <param name="value">The <see cref="OLECHAR" /> value to convert.</param>
        public static explicit operator short(OLECHAR value)
        {
            return (short)(value._value);
        }

        /// <summary>Explicitly converts a <see cref="short" /> value to a <see cref="OLECHAR" /> value.</summary>
        /// <param name="value">The <see cref="short" /> value to convert.</param>
        public static implicit operator OLECHAR(short value)
        {
            return new OLECHAR((WCHAR)(value));
        }

        /// <summary>Implicitly converts a <see cref="OLECHAR" /> value to a <see cref="ushort" /> value.</summary>
        /// <param name="value">The <see cref="OLECHAR" /> value to convert.</param>
        public static implicit operator ushort(OLECHAR value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="OLECHAR" /> value to a <see cref="WCHAR" /> value.</summary>
        /// <param name="value">The <see cref="OLECHAR" /> value to convert.</param>
        public static implicit operator WCHAR(OLECHAR value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="ushort" /> value to a <see cref="OLECHAR" /> value.</summary>
        /// <param name="value">The <see cref="ushort" /> value to convert.</param>
        public static implicit operator OLECHAR(ushort value)
        {
            return new OLECHAR(value);
        }

        /// <summary>Implicitly converts a <see cref="WCHAR" /> value to a <see cref="OLECHAR" /> value.</summary>
        /// <param name="value">The <see cref="WCHAR" /> value to convert.</param>
        public static implicit operator OLECHAR(WCHAR value)
        {
            return new OLECHAR(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="OLECHAR" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is OLECHAR other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<OLECHAR>
        /// <summary>Compares a <see cref="OLECHAR" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="OLECHAR" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(OLECHAR other)
        {
            return _value.CompareTo(other._value);
        }
        #endregion

        #region System.IEquatable<OLECHAR>
        /// <summary>Compares a <see cref="OLECHAR" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="OLECHAR" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(OLECHAR other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="OLECHAR" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is OLECHAR other)
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