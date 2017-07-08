// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\windef.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct HMONITOR : IEquatable<HMONITOR>, IFormattable
    {
        #region Fields
        internal HANDLE _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="HMONITOR" /> struct.</summary>
        /// <param name="value">The <see cref="HANDLE" /> used to initialize the instance.</param>
        public HMONITOR(HANDLE value)
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
            return left._value == right._value;
        }

        /// <summary>Compares two <see cref="HMONITOR" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="HMONITOR" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="HMONITOR" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(HMONITOR left, HMONITOR right)
        {
            return left._value != right._value;
        }

        /// <summary>Explicitly converts a <see cref="HMONITOR" /> value to a <see cref="IntPtr" />* value.</summary>
        /// <param name="value">The <see cref="HMONITOR" /> value to convert.</param>
        public static explicit operator IntPtr(HMONITOR value)
        {
            return (IntPtr)(value._value);
        }

        /// <summary>Explicitly converts a <see cref="IntPtr" /> value to a <see cref="HMONITOR" />* value.</summary>
        /// <param name="value">The <see cref="IntPtr" /> value to convert.</param>
        public static explicit operator HMONITOR(IntPtr value)
        {
            return new HMONITOR((HANDLE)(value));
        }

        /// <summary>Implicitly converts a <see cref="HMONITOR" /> value to a <see cref="UIntPtr" />* value.</summary>
        /// <param name="value">The <see cref="HMONITOR" /> value to convert.</param>
        public static implicit operator UIntPtr(HMONITOR value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="HMONITOR" /> value to a <see cref="HANDLE" /> value.</summary>
        /// <param name="value">The <see cref="HMONITOR" /> value to convert.</param>
        public static implicit operator HANDLE(HMONITOR value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="UIntPtr" /> value to a <see cref="HMONITOR" /> value.</summary>
        /// <param name="value">The <see cref="UIntPtr" /> value to convert.</param>
        public static implicit operator HMONITOR(UIntPtr value)
        {
            return new HMONITOR(value);
        }

        /// <summary>Implicitly converts a <see cref="HANDLE" /> value to a <see cref="HMONITOR" /> value.</summary>
        /// <param name="value">The <see cref="HANDLE" /> value to convert.</param>
        public static implicit operator HMONITOR(HANDLE value)
        {
            return new HMONITOR(value);
        }

        /// <summary>Implicitly converts a <see cref="void" />* value to a <see cref="HMONITOR" /> value.</summary>
        /// <param name="value">The <see cref="void" />* value to convert.</param>
        public static implicit operator HMONITOR(void* value)
        {
            return new HMONITOR(value);
        }

        /// <summary>Implicitly converts a <see cref="HMONITOR" /> value to a <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="HMONITOR" /> value to convert.</param>
        public static implicit operator void* (HMONITOR value)
        {
            return value._value;
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

        #region System.IFormattable
        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
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
            return _value.GetHashCode();
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString()
        {
            return _value.ToString();
        }
        #endregion
    }
}
