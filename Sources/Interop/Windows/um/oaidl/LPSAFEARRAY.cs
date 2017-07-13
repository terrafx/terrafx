// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct LPSAFEARRAY : IEquatable<LPSAFEARRAY>, IFormattable
    {
        #region Fields
        internal SAFEARRAY* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="LPSAFEARRAY" /> struct.</summary>
        /// <param name="value">The <see cref="SAFEARRAY" />* used to initialize the instance.</param>
        public LPSAFEARRAY(SAFEARRAY* value)
        {
            _value = value;
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="LPSAFEARRAY" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="LPSAFEARRAY" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LPSAFEARRAY" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(LPSAFEARRAY left, LPSAFEARRAY right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="LPSAFEARRAY" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="LPSAFEARRAY" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LPSAFEARRAY" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(LPSAFEARRAY left, LPSAFEARRAY right)
        {
            return (left._value != right._value);
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="LPSAFEARRAY" /> value to a <see cref="SAFEARRAY" />* value.</summary>
        /// <param name="value">The <see cref="LPSAFEARRAY" /> value to convert.</param>
        public static implicit operator SAFEARRAY* (LPSAFEARRAY value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="SAFEARRAY" />* value to a <see cref="LPSAFEARRAY" /> value.</summary>
        /// <param name="value">The <see cref="SAFEARRAY" />* value to convert.</param>
        public static implicit operator LPSAFEARRAY(SAFEARRAY* value)
        {
            return new LPSAFEARRAY(value);
        }
        #endregion

        #region System.IEquatable<LPSAFEARRAY> Methods
        /// <summary>Compares a <see cref="LPSAFEARRAY" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="LPSAFEARRAY" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(LPSAFEARRAY other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="LPSAFEARRAY" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is LPSAFEARRAY other)
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
