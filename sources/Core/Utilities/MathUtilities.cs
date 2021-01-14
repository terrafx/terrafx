// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods to supplement or replace <see cref="Math" /> and <see cref="MathF" />.</summary>
    public static class MathUtilities
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

        /// <summary>Clamps a <see cref="float" /> value to be between a minimum and maximum value.</summary>
        /// <param name="value">The value to restrict.</param>
        /// <param name="min">The minimum value (inclusive).</param>
        /// <param name="max">The maximum value (inclusive).</param>
        /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
        public static float Clamp(float value, float min, float max)
        {
            // The compare order here is important.
            // It ensures we match HLSL behavior for the scenario where min is larger than max.

            var result = value;

            result = (result > max) ? max : result;
            result = (result < min) ? min : result;

            return result;
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

        /// <summary>Tests if two <see cref="float"/> instances have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="left">The first insance to compare.</param>
        /// <param name="right">The other instance to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns><c>true</c> if similar, <c>false</c> otherwise.</returns>
        public static bool EqualEstimate(this float left, float right, float epsilon) => MathF.Abs(right - left) < epsilon;

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
