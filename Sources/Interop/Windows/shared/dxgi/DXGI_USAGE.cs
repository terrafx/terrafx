// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_USAGE : IComparable, IComparable<DXGI_USAGE>, IEquatable<DXGI_USAGE>, IFormattable
    {
        #region Fields
        internal DWORD _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="DXGI_USAGE" /> struct.</summary>
        /// <param name="value">The <see cref="DWORD" /> used to initialize the instance.</param>
        public DXGI_USAGE(DWORD value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Explicitly converts a <see cref="DXGI_USAGE" /> value to a <see cref="int" /> value.</summary>
        /// <param name="value">The <see cref="DXGI_USAGE" /> value to convert.</param>
        public static explicit operator int(DXGI_USAGE value)
        {
            return (int)(value._value);
        }

        /// <summary>Explicitly converts a <see cref="int" /> value to a <see cref="DXGI_USAGE" /> value.</summary>
        /// <param name="value">The <see cref="int" /> value to convert.</param>
        public static explicit operator DXGI_USAGE(int value)
        {
            return new DXGI_USAGE((uint)(value));
        }

        /// <summary>Implicitly converts a <see cref="DXGI_USAGE" /> value to a <see cref="uint" /> value.</summary>
        /// <param name="value">The <see cref="DXGI_USAGE" /> value to convert.</param>
        public static implicit operator uint(DXGI_USAGE value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="DXGI_USAGE" /> value to a <see cref="DWORD" /> value.</summary>
        /// <param name="value">The <see cref="DXGI_USAGE" /> value to convert.</param>
        public static implicit operator DWORD(DXGI_USAGE value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="uint" /> value to a <see cref="DXGI_USAGE" /> value.</summary>
        /// <param name="value">The <see cref="uint" /> value to convert.</param>
        public static implicit operator DXGI_USAGE(uint value)
        {
            return new DXGI_USAGE(value);
        }

        /// <summary>Implicitly converts a <see cref="DWORD" /> value to a <see cref="DXGI_USAGE" /> value.</summary>
        /// <param name="value">The <see cref="DWORD" /> value to convert.</param>
        public static implicit operator DXGI_USAGE(DWORD value)
        {
            return new DXGI_USAGE(value);
        }
        #endregion

        #region System.IComparable
        /// <summary>Compares a <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="DXGI_USAGE" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is DXGI_USAGE other)
            {
                return CompareTo(other);
            }
            else
            {
                throw ExceptionUtilities.NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<DXGI_USAGE>
        /// <summary>Compares a <see cref="DXGI_USAGE" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="DXGI_USAGE" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(DXGI_USAGE other)
        {
            return _value.CompareTo(other._value);
        }
        #endregion

        #region System.IEquatable<DXGI_USAGE>
        /// <summary>Compares a <see cref="DXGI_USAGE" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="DXGI_USAGE" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(DXGI_USAGE other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="DXGI_USAGE" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is DXGI_USAGE other)
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
