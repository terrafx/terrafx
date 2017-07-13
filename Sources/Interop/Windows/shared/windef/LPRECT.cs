// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\windef.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct LPRECT : IEquatable<LPRECT>, IFormattable
    {
        #region Fields
        internal RECT* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="LPRECT" /> struct.</summary>
        /// <param name="value">The <see cref="RECT" />* used to initialize the instance.</param>
        public LPRECT(RECT* value)
        {
            _value = value;
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="LPRECT" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="LPRECT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LPRECT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(LPRECT left, LPRECT right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="LPRECT" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="LPRECT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LPRECT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(LPRECT left, LPRECT right)
        {
            return (left._value != right._value);
        }
        #endregion

        #region Cast Operators
        /// <summary>Explicitly converts a <see cref="LPRECT" /> value to a <see cref="uint" /> value.</summary>
        /// <param name="value">The <see cref="LPRECT" /> value to convert.</param>
        public static explicit operator uint(LPRECT value)
        {
            return (uint)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="LPRECT" /> value to a <see cref="RECT" />* value.</summary>
        /// <param name="value">The <see cref="LPRECT" /> value to convert.</param>
        public static implicit operator RECT* (LPRECT value)
        {
            return value._value;
        }

        /// <summary>Explicitly converts a <see cref="uint" /> value to a <see cref="LPRECT" /> value.</summary>
        /// <param name="value">The <see cref="uint" /> value to convert.</param>
        public static explicit operator LPRECT(uint value)
        {
            return new LPRECT((RECT*)(value));
        }

        /// <summary>Implicitly converts a <see cref="RECT" />* value to a <see cref="LPRECT" /> value.</summary>
        /// <param name="value">The <see cref="RECT" />* value to convert.</param>
        public static implicit operator LPRECT(RECT* value)
        {
            return new LPRECT(value);
        }
        #endregion

        #region System.IEquatable<LPRECT> Methods
        /// <summary>Compares a <see cref="LPRECT" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="LPRECT" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(LPRECT other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="LPRECT" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is LPRECT other)
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
