// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\guiddef.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    public /* blittable */ struct IID : IEquatable<IID>, IFormattable
    {
        #region Fields
        internal GUID _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="IID" /> struct.</summary>
        /// <param name="value">The <see cref="GUID" /> used to initialize the instance.</param>
        public IID(GUID value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Explicitly converts a <see cref="IID" /> value to a <see cref="Guid" /> value.</summary>
        /// <param name="value">The <see cref="IID" /> value to convert.</param>
        public static implicit operator Guid(IID value)
        {
            return value._value;
        }

        /// <summary>Explicitly converts a <see cref="IID" /> value to a <see cref="GUID" /> value.</summary>
        /// <param name="value">The <see cref="IID" /> value to convert.</param>
        public static implicit operator GUID(IID value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="Guid" /> value to a <see cref="IID" /> value.</summary>
        /// <param name="value">The <see cref="Guid" /> value to convert.</param>
        public static implicit operator IID(Guid value)
        {
            return new IID(value);
        }

        /// <summary>Implicitly converts a <see cref="GUID" /> value to a <see cref="IID" /> value.</summary>
        /// <param name="value">The <see cref="GUID" /> value to convert.</param>
        public static implicit operator IID(GUID value)
        {
            return new IID(value);
        }
        #endregion

        #region System.IEquatable<IID>
        /// <summary>Compares a <see cref="IID" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="IID" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(IID other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="IID" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is IID other)
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
