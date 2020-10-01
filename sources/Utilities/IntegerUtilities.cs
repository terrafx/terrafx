// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

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
    }
}
