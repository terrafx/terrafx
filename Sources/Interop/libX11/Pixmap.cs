// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;

namespace TerraFX.Interop
{
    /// <summary>A pixmap resource ID.</summary>
    public /* blittable */ struct Pixmap : IEquatable<Pixmap>, IFormattable
    {
        #region Fields
        internal XID _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Pixmap" /> struct.</summary>
        /// <param name="value">The <see cref="XID" /> used to initialize the instance.</param>
        public Pixmap(XID value)
        {
            _value = value;
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="Pixmap" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Pixmap" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Pixmap" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Pixmap left, Pixmap right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="Pixmap" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Pixmap" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Pixmap" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Pixmap left, Pixmap right)
        {
            return (left._value != right._value);
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="Pixmap" /> value to a <see cref="XID" /> value.</summary>
        /// <param name="value">The <see cref="Pixmap" /> value to convert.</param>
        public static implicit operator XID(Pixmap value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="XID" /> value to a <see cref="Pixmap" /> value.</summary>
        /// <param name="value">The <see cref="XID" /> value to convert.</param>
        public static implicit operator Pixmap(XID value)
        {
            return new Pixmap(value);
        }
        #endregion

        #region System.IEquatable<Pixmap> Methods
        /// <summary>Compares a <see cref="Pixmap" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Pixmap" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Pixmap other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Pixmap" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Pixmap other)
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
