// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    public /* blittable */ struct WICPixelFormatGUID : IEquatable<WICPixelFormatGUID>, IFormattable
    {
        #region Fields
        internal GUID _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="WICPixelFormatGUID" /> struct.</summary>
        /// <param name="value">The <see cref="GUID" /> used to initialize the instance.</param>
        public WICPixelFormatGUID(GUID value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Implicitly converts a <see cref="WICPixelFormatGUID" /> value to a <see cref="Guid" /> value.</summary>
        /// <param name="value">The <see cref="WICPixelFormatGUID" /> value to convert.</param>
        public static implicit operator Guid(WICPixelFormatGUID value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="WICPixelFormatGUID" /> value to a <see cref="GUID" /> value.</summary>
        /// <param name="value">The <see cref="WICPixelFormatGUID" /> value to convert.</param>
        public static implicit operator GUID(WICPixelFormatGUID value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="Guid" /> value to a <see cref="WICPixelFormatGUID" /> value.</summary>
        /// <param name="value">The <see cref="Guid" /> value to convert.</param>
        public static implicit operator WICPixelFormatGUID(Guid value)
        {
            return new WICPixelFormatGUID(value);
        }

        /// <summary>Implicitly converts a <see cref="GUID" /> value to a <see cref="WICPixelFormatGUID" /> value.</summary>
        /// <param name="value">The <see cref="GUID" /> value to convert.</param>
        public static implicit operator WICPixelFormatGUID(GUID value)
        {
            return new WICPixelFormatGUID(value);
        }
        #endregion

        #region System.IEquatable<WICPixelFormatGUID>
        /// <summary>Compares a <see cref="WICPixelFormatGUID" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="WICPixelFormatGUID" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(WICPixelFormatGUID other)
        {
            return _value.Equals(other._value);
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="WICPixelFormatGUID" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is WICPixelFormatGUID other)
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
