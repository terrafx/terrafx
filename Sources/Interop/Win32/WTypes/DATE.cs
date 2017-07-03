// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\wtypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    /// <summary>Date</summary>
    unsafe public struct DATE : IEquatable<DATE>
    {
        #region Fields
        private double _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="DATE" /> struct.</summary>
        /// <param name="value">The value of the instance.</param>
        public DATE(double value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="DATE" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="DATE" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="DATE" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(DATE left, DATE right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="DATE" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="DATE" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="DATE" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(DATE left, DATE right)
        {
            return (left._value == right._value);
        }

        /// <summary>Converts a <see cref="DATE" /> to an equivalent <see cref="IntPtr" /> value.</summary>
        /// <param name="value">The <see cref="DATE" /> to convert.</param>
        public static implicit operator IntPtr(DATE value)
        {
            return (IntPtr)(value._value);
        }

        /// <summary>Converts a <see cref="DATE" /> to an equivalent <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="DATE" /> to convert.</param>
        public static implicit operator double(DATE value)
        {
            return value._value;
        }

        /// <summary>Converts a <see cref="void" />* to an equivalent <see cref="DATE" /> value.</summary>
        /// <param name="value">The <see cref="void" />* to convert.</param>
        public static implicit operator DATE(double value)
        {
            return new DATE(value);
        }
        #endregion

        #region System.IEquatable<DATE>
        /// <summary>Compares a <see cref="DATE" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="DATE" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(DATE other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="DATE" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is DATE other)
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
