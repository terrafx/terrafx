// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\WTypesbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    /// <summary>A pointer to string of <see cref="OLECHAR" />.</summary>
    unsafe public struct LPOLESTR : IEquatable<LPOLESTR>
    {
        #region Constants
        /// <summary>A <see cref="LPOLESTR" /> value that represents <c>null</c>.</summary>
        public static readonly LPOLESTR NULL = null;
        #endregion

        #region Fields
        private OLECHAR* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="LPOLESTR" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public LPOLESTR(OLECHAR* value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="LPOLESTR" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="LPOLESTR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LPOLESTR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(LPOLESTR left, LPOLESTR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="LPOLESTR" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="LPOLESTR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="LPOLESTR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(LPOLESTR left, LPOLESTR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Converts a <see cref="LPOLESTR" /> to an equivalent <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="LPOLESTR" /> to convert.</param>
        public static implicit operator OLECHAR*(LPOLESTR value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="void" />* to an equivalent <see cref="LPOLESTR" /> value.</summary>
        /// <param name="value">The <see cref="void" />* to convert.</param>
        public static implicit operator LPOLESTR(OLECHAR* value)
        {
            return new LPOLESTR(value);
        }
        #endregion

        #region System.IEquatable<LPSTR>
        /// <summary>Compares a <see cref="LPOLESTR" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="LPOLESTR" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(LPOLESTR other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="LPOLESTR" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is LPOLESTR other)
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
