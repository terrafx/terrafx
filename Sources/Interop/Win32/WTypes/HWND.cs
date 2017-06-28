// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    /// <summary>A handle to a window.</summary>
    unsafe public struct HWND : IEquatable<HWND>
    {
        #region Constants
        /// <summary>A <see cref="HWND" /> value that represents <c>null</c>.</summary>
        public static readonly HWND NULL = null;
        #endregion

        #region Fields
        private void* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="HWND" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public HWND(IntPtr value) : this(value.ToPointer())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="HWND" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public HWND(void* value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="HWND" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="HWND" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="HWND" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(HWND left, HWND right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="HWND" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="HWND" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="HWND" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(HWND left, HWND right)
        {
            return (left._value == right._value);
        }

        /// <summary>Converts a <see cref="HWND" /> to an equivalent <see cref="IntPtr" /> value.</summary>
        /// <param name="value">The <see cref="HWND" /> to convert.</param>
        public static implicit operator IntPtr(HWND value)
        {
            return (IntPtr)(value._value);
        }

        /// <summary>Converts a <see cref="HWND" /> to an equivalent <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="HWND" /> to convert.</param>
        public static implicit operator void* (HWND value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="IntPtr" /> to an equivalent <see cref="HWND" /> value.</summary>
        /// <param name="value">The <see cref="IntPtr" /> to convert.</param>
        public static implicit operator HWND(IntPtr value)
        {
            return new HWND(value);
        }

        /// <summary>Converts a <see cref="void" />* to an equivalent <see cref="HWND" /> value.</summary>
        /// <param name="value">The <see cref="void" />* to convert.</param>
        public static implicit operator HWND(void* value)
        {
            return new HWND(value);
        }
        #endregion

        #region System.IEquatable<HWND>
        /// <summary>Compares a <see cref="HWND" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="HWND" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(HWND other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="HWND" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is HWND other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return ((IntPtr)(_value)).GetHashCode();
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString()
        {
            return ((IntPtr)(_value)).ToString();
        }
        #endregion
    }
}
