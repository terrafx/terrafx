// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winnt.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Diagnostics;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct PCSTR : IEquatable<PCSTR>, IFormattable
    {
        #region Fields
        internal /* readonly */ CHAR* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="PCSTR" /> struct.</summary>
        /// <param name="value">The <see cref="CHAR" />* used to initialize the instance.</param>
        public PCSTR(CHAR* value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="PCSTR" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="PCSTR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="PCSTR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(PCSTR left, PCSTR right)
        {
            return left._value == right._value;
        }

        /// <summary>Compares two <see cref="PCSTR" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="PCSTR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="PCSTR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(PCSTR left, PCSTR right)
        {
            return left._value != right._value;
        }

        /// <summary>Implicitly converts a <see cref="PCSTR" /> value to a <see cref="CHAR" />* value.</summary>
        /// <param name="value">The <see cref="PCSTR" /> value to convert.</param>
        public static implicit operator CHAR*(PCSTR value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="CHAR" />* value to a <see cref="PCSTR" /> value.</summary>
        /// <param name="value">The <see cref="CHAR" />* value to convert.</param>
        public static implicit operator PCSTR(CHAR* value)
        {
            return new PCSTR(value);
        }
        #endregion

        #region System.IEquatable<PCSTR>
        /// <summary>Compares a <see cref="PCSTR" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="PCSTR" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(PCSTR other)
        {
            return (this == other);
        }
        #endregion

        #region System.IFormattable
        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(_value)).ToString(format, formatProvider);
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                return ((ulong)(_value)).ToString(format, formatProvider);
            }
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="PCSTR" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is PCSTR other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(_value)).GetHashCode();
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                return ((ulong)(_value)).GetHashCode();
            }
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString()
        {
            if (IntPtr.Size == sizeof(uint))
            {
                return ((uint)(_value)).ToString();
            }
            else
            {
                Debug.Assert(IntPtr.Size == sizeof(ulong));
                return ((ulong)(_value)).ToString();
            }
        }
        #endregion
    }
}
