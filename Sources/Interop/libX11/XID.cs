// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>A generic resource ID.</summary>
    unsafe public struct XID : IEquatable<XID>, IFormattable
    {
        #region Constants
        /// <summary>A null generic resource ID.</summary>
        public static readonly XID None = new XID(0);
        #endregion

        #region Fields
        internal nint _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="XID" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public XID(nint value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="XID" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="XID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="XID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(XID left, XID right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="XID" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="XID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="XID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(XID left, XID right)
        {
            return (left._value == right._value);
        }

        /// <summary>Converts a <see cref="XID" /> to a <see cref="nint" /> value.</summary>
        /// <param name="value">The <see cref="XID" /> to convert.</param>
        public static implicit operator nint(XID value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="nint" /> to a <see cref="XID" /> value.</summary>
        /// <param name="value">The <see cref="nint" /> to convert.</param>
        public static implicit operator XID(nint value)
        {
            return new XID(value);
        }
        #endregion

        #region System.IEquatable<XID>
        /// <summary>Compares a <see cref="XID" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="XID" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(XID other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="XID" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is XID other)
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
            return ToString(format: null, formatProvider: null);
        }
        #endregion
    }
}