// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    /// <summary>A handle to an instance.</summary>
    unsafe public struct HINSTANCE : IEquatable<HINSTANCE>
    {
        #region Constants
        /// <summary>A <see cref="HINSTANCE" /> value that represents <c>null</c>.</summary>
        public static readonly HINSTANCE NULL = null;
        #endregion

        #region Fields
        private void* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="HINSTANCE" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public HINSTANCE(UIntPtr value) : this(value.ToPointer())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="HINSTANCE" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public HINSTANCE(void* value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="HINSTANCE" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="HINSTANCE" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="HINSTANCE" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(HINSTANCE left, HINSTANCE right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="HINSTANCE" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="HINSTANCE" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="HINSTANCE" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(HINSTANCE left, HINSTANCE right)
        {
            return (left._value == right._value);
        }

        /// <summary>Converts a <see cref="HINSTANCE" /> to an equivalent <see cref="HMODULE" /> value.</summary>
        /// <param name="value">The <see cref="HINSTANCE" /> to convert.</param>
        public static implicit operator HMODULE(HINSTANCE value)
        {
            return new HMODULE(value._value);
        }

        /// <summary>Converts a <see cref="HINSTANCE" /> to an equivalent <see cref="UIntPtr" /> value.</summary>
        /// <param name="value">The <see cref="HINSTANCE" /> to convert.</param>
        public static implicit operator UIntPtr(HINSTANCE value)
        {
            return (UIntPtr)(value._value);
        }

        /// <summary>Converts a <see cref="HINSTANCE" /> to an equivalent <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="HINSTANCE" /> to convert.</param>
        public static implicit operator void* (HINSTANCE value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="UIntPtr" /> to an equivalent <see cref="HINSTANCE" /> value.</summary>
        /// <param name="value">The <see cref="UIntPtr" /> to convert.</param>
        public static implicit operator HINSTANCE(UIntPtr value)
        {
            return new HINSTANCE(value);
        }

        /// <summary>Converts a <see cref="void" />* to an equivalent <see cref="HINSTANCE" /> value.</summary>
        /// <param name="value">The <see cref="void" />* to convert.</param>
        public static implicit operator HINSTANCE(void* value)
        {
            return new HINSTANCE(value);
        }
        #endregion

        #region System.IEquatable<HINSTANCE>
        /// <summary>Compares a <see cref="HINSTANCE" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="HINSTANCE" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(HINSTANCE other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="HINSTANCE" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is HINSTANCE other)
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
