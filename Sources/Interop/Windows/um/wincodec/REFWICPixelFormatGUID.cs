// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct REFWICPixelFormatGUID : IEquatable<REFWICPixelFormatGUID>, IFormattable
    {
        #region Fields
        internal REFGUID _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="REFWICPixelFormatGUID" /> struct.</summary>
        /// <param name="value">The <see cref="REFGUID" /> used to initialize the instance.</param>
        public REFWICPixelFormatGUID(REFGUID value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="REFWICPixelFormatGUID" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="REFWICPixelFormatGUID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="REFWICPixelFormatGUID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(REFWICPixelFormatGUID left, REFWICPixelFormatGUID right)
        {
            return left._value == right._value;
        }

        /// <summary>Compares two <see cref="REFWICPixelFormatGUID" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="REFWICPixelFormatGUID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="REFWICPixelFormatGUID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(REFWICPixelFormatGUID left, REFWICPixelFormatGUID right)
        {
            return left._value != right._value;
        }

        /// <summary>Implicitly converts a <see cref="REFWICPixelFormatGUID" /> value to a <see cref="REFGUID" /> value.</summary>
        /// <param name="value">The <see cref="REFWICPixelFormatGUID" /> value to convert.</param>
        public static implicit operator REFGUID(REFWICPixelFormatGUID value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="REFGUID" /> value to a <see cref="REFWICPixelFormatGUID" /> value.</summary>
        /// <param name="value">The <see cref="REFGUID" /> value to convert.</param>
        public static implicit operator REFWICPixelFormatGUID(REFGUID value)
        {
            return new REFWICPixelFormatGUID(value);
        }

        /// <summary>Implicitly converts a <see cref="REFWICPixelFormatGUID" /> value to a <see cref="GUID" />* value.</summary>
        /// <param name="value">The <see cref="REFWICPixelFormatGUID" /> value to convert.</param>
        public static implicit operator GUID* (REFWICPixelFormatGUID value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="GUID" />* value to a <see cref="REFWICPixelFormatGUID" /> value.</summary>
        /// <param name="value">The <see cref="GUID" />* value to convert.</param>
        public static implicit operator REFWICPixelFormatGUID(GUID* value)
        {
            return new REFWICPixelFormatGUID(value);
        }
        #endregion

        #region System.IEquatable<REFWICPixelFormatGUID>
        /// <summary>Compares a <see cref="REFWICPixelFormatGUID" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="REFWICPixelFormatGUID" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(REFWICPixelFormatGUID other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="REFWICPixelFormatGUID" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is REFWICPixelFormatGUID other)
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
