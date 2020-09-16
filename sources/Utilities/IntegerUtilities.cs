// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for manipulating integers.</summary>
    public static class IntegerUtilities
    {
        /// <summary>Rounds a given address up to the nearest alignment.</summary>
        /// <param name="address">The address to be aligned.</param>
        /// <param name="alignment">The target alignment, which should be a power of two.</param>
        /// <returns><paramref name="address" /> rounded up to the specified <paramref name="alignment" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong AlignUp(ulong address, ulong alignment)
        {
            Assert(IsPow2(alignment));
            return (address + (alignment - 1)) & ~(alignment - 1);
        }

        /// <summary>Determines whether a given value is a power of two.</summary>
        /// <param name="value">The value to check.</param>
        /// <returns><c>true</c> if <paramref name="value" /> is a power of two; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPow2(ulong value)
        {
            if (Popcnt.X64.IsSupported)
            {
                return Popcnt.X64.PopCount(value) == 1;
            }
            else
            {
                return ((value & (value - 1)) == 0) && (value != 0);
            }
        }

        /// <summary>Rotates an <see cref="int" /> value left the specified number of bits.</summary>
        /// <param name="value">The <see cref="int" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> left <paramref name="bits" /> times.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RotateLeft(int value, byte bits)
        {
            var result = RotateLeft(unchecked((uint)value), bits);
            return unchecked((int)result);
        }

        /// <summary>Rotates an <see cref="long" /> value left the specified number of bits.</summary>
        /// <param name="value">The <see cref="long" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> left <paramref name="bits" /> times.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RotateLeft(long value, byte bits)
        {
            var result = RotateLeft(unchecked((ulong)value), bits);
            return unchecked((long)result);
        }

        /// <summary>Rotates an <see cref="uint" /> value left the specified number of bits.</summary>
        /// <param name="value">The <see cref="uint" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> left <paramref name="bits" /> times.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(uint value, byte bits) => BitOperations.RotateLeft(value, bits);

        /// <summary>Rotates an <see cref="ulong" /> value left the specified number of bits.</summary>
        /// <param name="value">The <see cref="ulong" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> left <paramref name="bits" /> times.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft(ulong value, byte bits) => BitOperations.RotateLeft(value, bits);

        /// <summary>Rotates an <see cref="int" /> value right the specified number of bits.</summary>
        /// <param name="value">The <see cref="int" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> right <paramref name="bits" /> times.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RotateRight(int value, byte bits)
        {
            var result = RotateRight(unchecked((uint)value), bits);
            return unchecked((int)result);
        }

        /// <summary>Rotates an <see cref="long" /> value right the specified number of bits.</summary>
        /// <param name="value">The <see cref="long" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> right <paramref name="bits" /> times.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RotateRight(long value, byte bits)
        {
            var result = RotateRight(unchecked((ulong)value), bits);
            return unchecked((long)result);
        }

        /// <summary>Rotates an <see cref="uint" /> value right the specified number of bits.</summary>
        /// <param name="value">The <see cref="uint" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> right <paramref name="bits" /> times.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateRight(uint value, byte bits) => BitOperations.RotateRight(value, bits);

        /// <summary>Rotates an <see cref="ulong" /> value right the specified number of bits.</summary>
        /// <param name="value">The <see cref="ulong" /> value to rotate.</param>
        /// <param name="bits">The number of bits to rotate <paramref name="value" />.</param>
        /// <returns>The result of rotating <paramref name="value" /> right <paramref name="bits" /> times.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateRight(ulong value, byte bits) => BitOperations.RotateRight(value, bits);
    }
}
