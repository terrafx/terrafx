// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct VkQueue : IEquatable<VkQueue>, IFormattable
    {
        #region Fields
        internal void* _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="VkQueue" /> struct.</summary>
        /// <param name="value">The <see cref="void" />* used to initialize the instance.</param>
        public VkQueue(void* value)
        {
            _value = value;
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="VkQueue" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="VkQueue" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="VkQueue" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(VkQueue left, VkQueue right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="VkQueue" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="VkQueue" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="VkQueue" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(VkQueue left, VkQueue right)
        {
            return (left._value != right._value);
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="VkQueue" /> value to a <see cref="void" />* value.</summary>
        /// <param name="value">The <see cref="VkQueue" /> value to convert.</param>
        public static implicit operator void* (VkQueue value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="void" />* value to a <see cref="VkQueue" /> value.</summary>
        /// <param name="value">The <see cref="void" />* value to convert.</param>
        public static implicit operator VkQueue(void* value)
        {
            return new VkQueue(value);
        }
        #endregion

        #region System.IEquatable<VkQueue> Methods
        /// <summary>Compares a <see cref="VkQueue" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="VkQueue" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(VkQueue other)
        {
            var otherValue = (nuint)(other._value);
            return ((nuint)(_value)).Equals(otherValue);
        }
        #endregion

        #region System.IFormattable Methods
        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((nuint)(_value)).ToString(format, formatProvider);
        }
        #endregion

        #region System.Object Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="VkQueue" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is VkQueue other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return ((nuint)(_value)).GetHashCode();
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString()
        {
            return ((nuint)(_value)).ToString();
        }
        #endregion
    }
}
