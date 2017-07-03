// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypesbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    /// <summary>A binary string that is used by COM.</summary>
    unsafe public struct BSTR : IEquatable<BSTR>
    {
        #region Constants
        /// <summary>A <see cref="BSTR" /> value that represents <c>null</c>.</summary>
        public static readonly BSTR NULL = null;
        #endregion

        #region Fields
        private WCHAR* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="BSTR" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public BSTR(WCHAR* value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="BSTR" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="BSTR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="BSTR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(BSTR left, BSTR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="BSTR" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="BSTR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="BSTR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(BSTR left, BSTR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Converts a <see cref="BSTR" /> to an equivalent <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="BSTR" /> to convert.</param>
        public static implicit operator WCHAR*(BSTR value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="void" />* to an equivalent <see cref="BSTR" /> value.</summary>
        /// <param name="value">The <see cref="void" />* to convert.</param>
        public static implicit operator BSTR(WCHAR* value)
        {
            return new BSTR(value);
        }
        #endregion

        #region System.IEquatable<BSTR>
        /// <summary>Compares a <see cref="BSTR" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="BSTR" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(BSTR other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="BSTR" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is BSTR other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return ((UIntPtr)(_value)).GetHashCode();
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString()
        {
            return ((UIntPtr)(_value)).ToString();
        }
        #endregion
    }
}
