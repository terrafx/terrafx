// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\guiddef.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    public /* blittable */ struct CLSID : IEquatable<CLSID>, IFormattable
    {
        #region Fields
        internal GUID _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="CLSID" /> struct.</summary>
        /// <param name="value">The <see cref="GUID" /> used to initialize the instance.</param>
        public CLSID(GUID value)
        {
            _value = value;
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="CLSID" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="CLSID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="CLSID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(CLSID left, CLSID right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="CLSID" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="CLSID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="CLSID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(CLSID left, CLSID right)
        {
            return (left._value != right._value);
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="CLSID" /> value to a <see cref="Guid" /> value.</summary>
        /// <param name="value">The <see cref="CLSID" /> value to convert.</param>
        public static implicit operator Guid(CLSID value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="CLSID" /> value to a <see cref="GUID" /> value.</summary>
        /// <param name="value">The <see cref="CLSID" /> value to convert.</param>
        public static implicit operator GUID(CLSID value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="Guid" /> value to a <see cref="CLSID" /> value.</summary>
        /// <param name="value">The <see cref="Guid" /> value to convert.</param>
        public static implicit operator CLSID(Guid value)
        {
            return new CLSID(value);
        }

        /// <summary>Implicitly converts a <see cref="GUID" /> value to a <see cref="CLSID" /> value.</summary>
        /// <param name="value">The <see cref="GUID" /> value to convert.</param>
        public static implicit operator CLSID(GUID value)
        {
            return new CLSID(value);
        }
        #endregion

        #region System.IEquatable<CLSID> Methods
        /// <summary>Compares a <see cref="CLSID" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="CLSID" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(CLSID other)
        {
            var otherValue = other._value;
            return _value.Equals(otherValue);
        }
        #endregion

        #region System.IFormattable Methods
        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }
        #endregion

        #region System.Object Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="CLSID" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is CLSID other)
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
