// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\windef.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    /// <summary>A handle to a display monitor.</summary>
    unsafe public struct HMONITOR : IEquatable<HMONITOR>
    {
        #region Constants
        /// <summary>A <see cref="HMONITOR" /> value that represents <c>null</c>.</summary>
        public static readonly HMONITOR NULL = null;
        #endregion

        #region Fields
        private void* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="HMONITOR" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public HMONITOR(IntPtr value) : this(value.ToPointer())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="HMONITOR" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public HMONITOR(void* value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="HMONITOR" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="HMONITOR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="HMONITOR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(HMONITOR left, HMONITOR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="HMONITOR" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="HMONITOR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="HMONITOR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(HMONITOR left, HMONITOR right)
        {
            return (left._value == right._value);
        }

        /// <summary>Converts a <see cref="HMONITOR" /> to an equivalent <see cref="IntPtr" /> value.</summary>
        /// <param name="value">The <see cref="HMONITOR" /> to convert.</param>
        public static implicit operator IntPtr(HMONITOR value)
        {
            return (IntPtr)(value._value);
        }

        /// <summary>Converts a <see cref="HMONITOR" /> to an equivalent <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="HMONITOR" /> to convert.</param>
        public static implicit operator void* (HMONITOR value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="IntPtr" /> to an equivalent <see cref="HMONITOR" /> value.</summary>
        /// <param name="value">The <see cref="IntPtr" /> to convert.</param>
        public static implicit operator HMONITOR(IntPtr value)
        {
            return new HMONITOR(value);
        }

        /// <summary>Converts a <see cref="void" />* to an equivalent <see cref="HMONITOR" /> value.</summary>
        /// <param name="value">The <see cref="void" />* to convert.</param>
        public static implicit operator HMONITOR(void* value)
        {
            return new HMONITOR(value);
        }
        #endregion

        #region System.IEquatable<HMONITOR>
        /// <summary>Compares a <see cref="HMONITOR" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="HMONITOR" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(HMONITOR other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="HMONITOR" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is HMONITOR other)
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
