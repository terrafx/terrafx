// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    /// <summary>A pointer to a null-terminated string of 8-bit ANSI characters.</summary>
    unsafe public struct LPSTR : IEquatable<LPSTR>
    {
        #region Constants
        /// <summary>A <see cref="LPSTR" /> value that represents <c>null</c>.</summary>
        public static readonly LPSTR NULL = null;
        #endregion

        #region Fields
        private CHAR* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="LPSTR" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public LPSTR(CHAR* value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="LPSTR" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="LPSTR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LPSTR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(LPSTR left, LPSTR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="LPSTR" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="LPSTR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LPSTR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(LPSTR left, LPSTR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Converts a <see cref="LPSTR" /> to an equivalent <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="LPSTR" /> to convert.</param>
        public static implicit operator CHAR*(LPSTR value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="void" />* to an equivalent <see cref="LPSTR" /> value.</summary>
        /// <param name="value">The <see cref="void" />* to convert.</param>
        public static implicit operator LPSTR(CHAR* value)
        {
            return new LPSTR(value);
        }
        #endregion

        #region System.IEquatable<LPSTR>
        /// <summary>Compares a <see cref="LPSTR" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="LPSTR" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(LPSTR other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="LPSTR" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is LPSTR other)
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
