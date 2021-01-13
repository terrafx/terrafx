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

        /// <summary>Rounds a given address up to the nearest alignment.</summary>
        /// <param name="address">The address to be aligned.</param>
        /// <param name="alignment">The target alignment, which should be a power of two.</param>
        /// <returns><paramref name="address" /> rounded up to the specified <paramref name="alignment" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint AlignUp(nuint address, nuint alignment)
        {
            Assert(IsPow2(alignment));
            return (address + (alignment - 1)) & ~(alignment - 1);
        }

        /// <summary>Computes <paramref name="dividend" /> / <paramref name="divisor" /> but rounds the result up, rather than down.</summary>
        /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
        /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
        /// <returns>The quotient of <paramref name="dividend" /> / <paramref name="divisor" />, rounded up.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong DivideRoundingUp(uint dividend, uint divisor) => (dividend + divisor - 1) / divisor;

        /// <summary>Computes the quotient and remainder of <paramref name="dividend" /> / <paramref name="divisor" />.</summary>
        /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
        /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
        /// <returns>The quotient and remainder of <paramref name="dividend" /> / <paramref name="divisor" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (nuint quotient, nuint remainder) DivRem(nuint dividend, nuint divisor)
        {
            var quotient = dividend / divisor;
            var remainder = divisor - (quotient * divisor);
            return (quotient, remainder);
        }

        /// <summary>Determines whether a given value is a power of two.</summary>
        /// <param name="value">The value to check.</param>
        /// <returns><c>true</c> if <paramref name="value" /> is a power of two; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPow2(ulong value)
        {
            return Popcnt.X64.IsSupported
                ? Popcnt.X64.PopCount(value) == 1
                : ((value & (value - 1)) == 0) && (value != 0);
        }
    }
}
