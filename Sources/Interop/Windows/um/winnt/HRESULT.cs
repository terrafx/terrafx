// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winnt.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct HRESULT : IComparable, IComparable<HRESULT>, IEquatable<HRESULT>, IFormattable
    {
        #region Fields
        internal int _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="HRESULT" /> struct.</summary>
        /// <param name="value">The <see cref="int" /> used to initialize the instance.</param>
        public HRESULT(int value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Explicitly converts a <see cref="HRESULT" /> value to a <see cref="uint" /> value.</summary>
        /// <param name="value">The <see cref="HRESULT" /> value to convert.</param>
        public static explicit operator uint(HRESULT value)
        {
            return (uint)(value._value);
        }

        /// <summary>Explicitly converts a <see cref="uint" /> value to a <see cref="HRESULT" /> value.</summary>
        /// <param name="value">The <see cref="uint" /> value to convert.</param>
        public static explicit operator HRESULT(uint value)
        {
            return new HRESULT((int)(value));
        }

        /// <summary>Implicitly converts a <see cref="HRESULT" /> value to a <see cref="int" /> value.</summary>
        /// <param name="value">The <see cref="HRESULT" /> value to convert.</param>
        public static implicit operator int(HRESULT value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="int" /> value to a <see cref="HRESULT" /> value.</summary>
        /// <param name="value">The <see cref="int" /> value to convert.</param>
        public static implicit operator HRESULT(int value)
        {
            return new HRESULT(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="HRESULT" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is HRESULT other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<HRESULT>
        /// <summary>Compares a <see cref="HRESULT" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="HRESULT" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(HRESULT other)
        {
            return _value.CompareTo(other._value);
        }
        #endregion

        #region System.IEquatable<HRESULT>
        /// <summary>Compares a <see cref="HRESULT" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="HRESULT" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(HRESULT other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="HRESULT" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is HRESULT other)
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
