// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winnt.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct LPCWSTR : IEquatable<LPCWSTR>, IFormattable
    {
        #region Fields
        internal /* readonly */ WCHAR* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="LPCWSTR" /> struct.</summary>
        /// <param name="value">The <see cref="WCHAR" />* used to initialize the instance.</param>
        public LPCWSTR(WCHAR* value)
        {
            _value = value;
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="LPCWSTR" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="LPCWSTR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LPCWSTR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(LPCWSTR left, LPCWSTR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="LPCWSTR" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="LPCWSTR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LPCWSTR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(LPCWSTR left, LPCWSTR right)
        {
            return (left._value != right._value);
        }
        #endregion

        #region Cast Operators
        /// <summary>Explicitly converts a <see cref="LPCWSTR" /> value to a <see cref="uint" /> value.</summary>
        /// <param name="value">The <see cref="LPCWSTR" /> value to convert.</param>
        public static explicit operator uint(LPCWSTR value)
        {
            return (uint)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="LPCWSTR" /> value to a <see cref="ushort" />* value.</summary>
        /// <param name="value">The <see cref="LPCWSTR" /> value to convert.</param>
        public static implicit operator ushort* (LPCWSTR value)
        {
            return (ushort*)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="LPCWSTR" /> value to a <see cref="WCHAR" />* value.</summary>
        /// <param name="value">The <see cref="LPCWSTR" /> value to convert.</param>
        public static implicit operator WCHAR* (LPCWSTR value)
        {
            return value._value;
        }

        /// <summary>Explicitly converts a <see cref="uint" /> value to a <see cref="LPCWSTR" /> value.</summary>
        /// <param name="value">The <see cref="uint" /> value to convert.</param>
        public static explicit operator LPCWSTR(uint value)
        {
            return new LPCWSTR((WCHAR*)(value));
        }

        /// <summary>Implicitly converts a <see cref="ushort" />* value to a <see cref="LPCWSTR" /> value.</summary>
        /// <param name="value">The <see cref="ushort" />* value to convert.</param>
        public static implicit operator LPCWSTR(ushort* value)
        {
            return new LPCWSTR((WCHAR*)(value));
        }

        /// <summary>Implicitly converts a <see cref="WCHAR" />* value to a <see cref="LPCWSTR" /> value.</summary>
        /// <param name="value">The <see cref="WCHAR" />* value to convert.</param>
        public static implicit operator LPCWSTR(WCHAR* value)
        {
            return new LPCWSTR(value);
        }

        /// <summary>Explicitly converts a <see cref="NativeStringUni" /> value to a <see cref="LPCWSTR" /> value.</summary>
        /// <param name="value">The <see cref="NativeStringUni" /> value to convert.</param>
        public static explicit operator LPCWSTR(NativeStringUni value)
        {
            return new LPCWSTR((WCHAR*)((IntPtr)(value)));
        }
        #endregion

        #region System.IEquatable<LPCWSTR> Methods
        /// <summary>Compares a <see cref="LPCWSTR" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="LPCWSTR" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(LPCWSTR other)
        {
            var otherValue = (nuint)(other._value);
            return ((nuint)(_value)).Equals(otherValue);
        }
        #endregion

        #region System.IFormattable Methods
        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((nuint)(_value)).ToString(format, formatProvider);
        }
        #endregion

        #region System.Object Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="LPCWSTR" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is LPCWSTR other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return ((nuint)(_value)).GetHashCode();
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString()
        {
            return ((nuint)(_value)).ToString();
        }
        #endregion
    }
}
