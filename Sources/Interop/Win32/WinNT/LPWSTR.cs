// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winnt.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    /// <summary>A pointer to a null-terminated string of 16-bit Unicode characters.</summary>
    unsafe public struct LPWSTR : IEquatable<LPWSTR>
    {
        #region Constants
        /// <summary>A <see cref="LPWSTR" /> value that represents <c>null</c>.</summary>
        public static readonly LPWSTR NULL = null;
        #endregion

        #region Fields
        private WCHAR* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="LPWSTR" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public LPWSTR(WCHAR* value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="LPWSTR" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="LPWSTR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LPWSTR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(LPWSTR left, LPWSTR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="LPWSTR" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="LPWSTR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LPWSTR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(LPWSTR left, LPWSTR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Converts a <see cref="LPWSTR" /> to an equivalent <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="LPWSTR" /> to convert.</param>
        public static implicit operator WCHAR*(LPWSTR value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="void" />* to an equivalent <see cref="LPWSTR" /> value.</summary>
        /// <param name="value">The <see cref="void" />* to convert.</param>
        public static implicit operator LPWSTR(WCHAR* value)
        {
            return new LPWSTR(value);
        }
        #endregion

        #region System.IEquatable<LPWSTR>
        /// <summary>Compares a <see cref="LPWSTR" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="LPWSTR" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(LPWSTR other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="LPWSTR" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is LPWSTR other)
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
