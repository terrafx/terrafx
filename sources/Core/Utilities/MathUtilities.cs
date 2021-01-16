// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods to supplement or replace <see cref="Math" /> and <see cref="MathF" />.</summary>
    public static class MathUtilities
    {
        /// <inheritdoc cref="MathF.Abs(float)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Abs(float value) => MathF.Abs(value);

        /// <summary>Rounds a given address up to the nearest alignment.</summary>
        /// <param name="address">The address to be aligned.</param>
        /// <param name="alignment">The target alignment, which should be a power of two.</param>
        /// <returns><paramref name="address" /> rounded up to the specified <paramref name="alignment" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint AlignUp(uint address, uint alignment)
        {
            Assert(AssertionsEnabled && IsPow2(alignment));
            return (address + (alignment - 1)) & ~(alignment - 1);
        }

        /// <summary>Rounds a given address up to the nearest alignment.</summary>
        /// <param name="address">The address to be aligned.</param>
        /// <param name="alignment">The target alignment, which should be a power of two.</param>
        /// <returns><paramref name="address" /> rounded up to the specified <paramref name="alignment" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong AlignUp(ulong address, ulong alignment)
        {
            Assert(AssertionsEnabled && IsPow2(alignment));
            return (address + (alignment - 1)) & ~(alignment - 1);
        }

        /// <summary>Rounds a given address up to the nearest alignment.</summary>
        /// <param name="address">The address to be aligned.</param>
        /// <param name="alignment">The target alignment, which should be a power of two.</param>
        /// <returns><paramref name="address" /> rounded up to the specified <paramref name="alignment" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint AlignUp(nuint address, nuint alignment)
        {
            Assert(AssertionsEnabled && IsPow2(alignment));
            return (address + (alignment - 1)) & ~(alignment - 1);
        }

        /// <summary>Clamps a <see cref="float" /> value to be between a minimum and maximum value.</summary>
        /// <param name="value">The value to restrict.</param>
        /// <param name="min">The minimum value (inclusive).</param>
        /// <param name="max">The maximum value (inclusive).</param>
        /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max)
        {
            // The compare order here is important.
            // It ensures we match HLSL behavior for the scenario where min is larger than max.

            var result = value;

            result = (result > max) ? max : result;
            result = (result < min) ? min : result;

            return result;
        }

        /// <inheritdoc cref="MathF.Cos(float)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cos(float value) => MathF.Cos(value);

        /// <summary>Computes <paramref name="dividend" /> / <paramref name="divisor" /> but rounds the result up, rather than down.</summary>
        /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
        /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
        /// <returns>The quotient of <paramref name="dividend" /> / <paramref name="divisor" />, rounded up.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint DivideRoundingUp(uint dividend, uint divisor) => (dividend + divisor - 1) / divisor;

        /// <summary>Computes the quotient and remainder of <paramref name="dividend" /> / <paramref name="divisor" />.</summary>
        /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
        /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
        /// <returns>The quotient and remainder of <paramref name="dividend" /> / <paramref name="divisor" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (nuint quotient, nuint remainder) DivRem(nuint dividend, nuint divisor)
        {
            var quotient = dividend / divisor;
            var remainder = dividend - (quotient * divisor);
            return (quotient, remainder);
        }

        /// <summary>Compares two floats to determine if they are approximately equal.</summary>
        /// <param name="left">The float to compare with <paramref name="right" />.</param>
        /// <param name="right">The float to compare with <paramref name="left" />.</param>
        /// <param name="epsilon">The maximum (exclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsEstimate(this float left, float right, float epsilon) => Abs(right - left) < epsilon;

        /// <inheritdoc cref="Math.Max(int, int)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int left, int right) => Math.Max(left, right);

        /// <inheritdoc cref="MathF.Max(float, float)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float left, float right) => MathF.Max(left, right);

        /// <inheritdoc cref="Math.Min(int, int)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int left, int right) => Math.Min(left, right);

        /// <inheritdoc cref="MathF.Max(float, float)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float left, float right) => MathF.Min(left, right);

        /// <summary>Determines whether a given value is a power of two.</summary>
        /// <param name="value">The value to check.</param>
        /// <returns><c>true</c> if <paramref name="value" /> is a power of two; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPow2(uint value)
        {
            if (Popcnt.IsSupported)
            {
                return Popcnt.PopCount(value) == 1;
            }
            else
            {
                return unchecked((value & (value - 1)) == 0) && (value != 0);
            }
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
                return unchecked((value & (value - 1)) == 0) && (value != 0);
            }
        }

        /// <summary>Determines whether a given value is a power of two.</summary>
        /// <param name="value">The value to check.</param>
        /// <returns><c>true</c> if <paramref name="value" /> is a power of two; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPow2(nuint value)
        {
            if (Is64BitProcess)
            {
                return IsPow2((ulong)value);
            }
            else
            {
                return IsPow2((uint)value);
            }
        }

        /// <inheritdoc cref="MathF.Sin(float)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sin(float value) => MathF.Sin(value);

        /// <summary>Returns the sine and cosine of the specified angle.</summary>
        /// <param name="value">The angle for which to get the sine and cosine.</param>
        /// <returns>The sine and cosine of <paramref name="value" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float Sin, float Cos) SinCos(float value) => (MathF.Sin(value), MathF.Cos(value));

        /// <inheritdoc cref="MathF.Sqrt(float)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sqrt(float value) => MathF.Sqrt(value);

        /// <inheritdoc cref="MathF.Tan(float)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Tan(float value) => MathF.Tan(value);
    }
}
