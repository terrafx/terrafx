// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Diagnostics.Contracts;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for manipulating integers.</summary>
    public static class IntegerUtilities
    {
        #region Static Methods
        /// <summary>Rotates an <see cref="int" /> value left the specified number of bits.</summary>
        /// <param name="value">The <see cref="int" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> left <paramref name="bits" /> times.</returns>
        [Pure]
        public static int RotateLeft(int value, byte bits)
        {
            var result = RotateLeft(unchecked((uint)value), bits);
            return unchecked((int)result);
        }

        /// <summary>Rotates an <see cref="long" /> value left the specified number of bits.</summary>
        /// <param name="value">The <see cref="long" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> left <paramref name="bits" /> times.</returns>
        [Pure]
        public static long RotateLeft(long value, byte bits)
        {
            var result = RotateLeft(unchecked((ulong)value), bits);
            return unchecked((long)result);
        }

        /// <summary>Rotates an <see cref="uint" /> value left the specified number of bits.</summary>
        /// <param name="value">The <see cref="uint" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> left <paramref name="bits" /> times.</returns>
        [Pure]
        public static uint RotateLeft(uint value, byte bits)
        {
            return unchecked((value << bits) | (value >> (32 - bits)));
        }

        /// <summary>Rotates an <see cref="ulong" /> value left the specified number of bits.</summary>
        /// <param name="value">The <see cref="ulong" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> left <paramref name="bits" /> times.</returns>
        [Pure]
        public static ulong RotateLeft(ulong value, byte bits)
        {
            return unchecked((value << bits) | (value >> (64 - bits)));
        }

        /// <summary>Rotates an <see cref="int" /> value right the specified number of bits.</summary>
        /// <param name="value">The <see cref="int" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> right <paramref name="bits" /> times.</returns>
        [Pure]
        public static int RotateRight(int value, byte bits)
        {
            var result = RotateRight(unchecked((uint)value), bits);
            return unchecked((int)result);
        }

        /// <summary>Rotates an <see cref="long" /> value right the specified number of bits.</summary>
        /// <param name="value">The <see cref="long" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> right <paramref name="bits" /> times.</returns>
        [Pure]
        public static long RotateRight(long value, byte bits)
        {
            var result = RotateRight(unchecked((ulong)value), bits);
            return unchecked((long)result);
        }

        /// <summary>Rotates an <see cref="uint" /> value right the specified number of bits.</summary>
        /// <param name="value">The <see cref="uint" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> right <paramref name="bits" /> times.</returns>
        [Pure]
        public static uint RotateRight(uint value, byte bits)
        {
            return unchecked((value >> bits) | (value << (32 - bits)));
        }

        /// <summary>Rotates an <see cref="ulong" /> value right the specified number of bits.</summary>
        /// <param name="value">The <see cref="ulong" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> right <paramref name="bits" /> times.</returns>
        [Pure]
        public static ulong RotateRight(ulong value, byte bits)
        {
            return unchecked((value >> bits) | (value << (64 - bits)));
        }
        #endregion
    }
}
