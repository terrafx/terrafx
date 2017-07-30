// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;

namespace TerraFX.Interop
{
    /// <summary>A window resource ID.</summary>
    unsafe public /* blittable */ struct Window : IEquatable<Window>, IFormattable
    {
        #region Fields
        internal XID _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Window" /> struct.</summary>
        /// <param name="value">The <see cref="XID" /> used to initialize the instance.</param>
        public Window(XID value)
        {
            _value = value;
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="Window" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Window" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Window" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Window left, Window right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="Window" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Window" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Window" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Window left, Window right)
        {
            return (left._value != right._value);
        }
        #endregion

        #region Cast Operators
        /// <summary>Explicitly converts a <see cref="Window" /> value to a <see cref="nint" /> value.</summary>
        /// <param name="value">The <see cref="Window" /> value to convert.</param>
        public static explicit operator nint(Window value)
        {
            return (nint)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="Window" /> value to a <see cref="nuint" /> value.</summary>
        /// <param name="value">The <see cref="Window" /> value to convert.</param>
        public static implicit operator nuint(Window value)
        {
            return value._value;
        }

        /// <summary>Explicitly converts a <see cref="Window" /> value to a <see cref="uint" /> value.</summary>
        /// <param name="value">The <see cref="Window" /> value to convert.</param>
        public static explicit operator uint(Window value)
        {
            return (uint)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="Window" /> value to a <see cref="ulong" /> value.</summary>
        /// <param name="value">The <see cref="Window" /> value to convert.</param>
        public static implicit operator ulong(Window value)
        {
            return value._value;
        }

        /// <summary>Explicitly converts a <see cref="Window" /> value to a <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="Window" /> value to convert.</param>
        public static explicit operator void* (Window value)
        {
            return (void*)(value._value);
        }

        /// <summary>Implicitly converts a <see cref="Window" /> value to a <see cref="XID" /> value.</summary>
        /// <param name="value">The <see cref="Window" /> value to convert.</param>
        public static implicit operator XID(Window value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="uint" /> value to a <see cref="Window" /> value.</summary>
        /// <param name="value">The <see cref="uint" /> value to convert.</param>
        public static implicit operator Window(uint value)
        {
            return new Window(value);
        }

        /// <summary>Explicitly converts a <see cref="ulong" /> value to a <see cref="Window" /> value.</summary>
        /// <param name="value">The <see cref="ulong" /> value to convert.</param>
        public static explicit operator Window(ulong value)
        {
            return new Window((nuint)(value));
        }

        /// <summary>Implicitly converts a <see cref="nuint" /> value to a <see cref="Window" /> value.</summary>
        /// <param name="value">The <see cref="nuint" /> value to convert.</param>
        public static implicit operator Window(nuint value)
        {
            return new Window(value);
        }

        /// <summary>Explicitly converts a <see cref="void" />* value to a <see cref="Window" /> value.</summary>
        /// <param name="value">The <see cref="void" />* value to convert.</param>
        public static explicit operator Window(void* value)
        {
            return new Window((nuint)(value));
        }

        /// <summary>Implicitly converts a <see cref="XID" /> value to a <see cref="Window" /> value.</summary>
        /// <param name="value">The <see cref="XID" /> value to convert.</param>
        public static implicit operator Window(XID value)
        {
            return new Window(value);
        }
        #endregion

        #region System.IEquatable<Window> Methods
        /// <summary>Compares a <see cref="Window" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Window" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Window other)
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
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Window" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Window other)
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
