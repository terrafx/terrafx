// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Utilities;

/// <summary>Provides a set of methods to supplement or replace <see cref="Math" /> and <see cref="MathF" />.</summary>
public static class MathUtilities
{
    /// <summary>Computes the absolute value of a given 16-bit signed integer.</summary>
    /// <param name="value">The integer for which to compute its absolute.</param>
    /// <returns>The absolute value of <paramref name="value" />.</returns>
    /// <remarks>This method does not account for <see cref="short.MinValue" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short Abs(short value)
    {
        Assert(AssertionsEnabled && (value != short.MinValue));
        var mask = value >> ((sizeof(short) * 8) - 1);
        return (short)((value + mask) ^ mask);
    }

    /// <summary>Computes the absolute value of a given 32-bit signed integer.</summary>
    /// <param name="value">The integer for which to compute its absolute.</param>
    /// <returns>The absolute value of <paramref name="value" />.</returns>
    /// <remarks>This method does not account for <see cref="int.MinValue" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Abs(int value)
    {
        Assert(AssertionsEnabled && (value != int.MinValue));
        var mask = value >> ((sizeof(int) * 8) - 1);
        return (value + mask) ^ mask;
    }

    /// <summary>Computes the absolute value of a given signed native integer.</summary>
    /// <param name="value">The integer for which to compute its absolute.</param>
    /// <returns>The absolute value of <paramref name="value" />.</returns>
    /// <remarks>This method does not account for <see cref="nint.MinValue" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe nint Abs(nint value)
    {
        Assert(AssertionsEnabled && (value != nint.MinValue));
        var mask = value >> ((sizeof(nint) * 8) - 1);
        return (value + mask) ^ mask;
    }

    /// <summary>Computes the absolute value of a given 64-bit float.</summary>
    /// <param name="value">The float for which to compute its absolute.</param>
    /// <returns>The absolute value of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Abs(double value) => Math.Abs(value);

    /// <summary>Computes the absolute value of a given 8-bit signed integer.</summary>
    /// <param name="value">The integer for which to compute its absolute.</param>
    /// <returns>The absolute value of <paramref name="value" />.</returns>
    /// <remarks>This method does not account for <see cref="sbyte.MinValue" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte Abs(sbyte value)
    {
        Assert(AssertionsEnabled && (value != sbyte.MinValue));
        var mask = value >> ((sizeof(int) * 8) - 1);
        return (sbyte)((value + mask) ^ mask);
    }

    /// <summary>Computes the absolute value of a given 32-bit float.</summary>
    /// <param name="value">The float for which to compute its absolute.</param>
    /// <returns>The absolute value of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Abs(float value) => MathF.Abs(value);

    /// <summary>Computes the arc-cosine for a given 64-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the arc-cosine.</param>
    /// <returns>The arc-cosine of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Acos(double value) => Math.Acos(value);

    /// <summary>Computes the arc-cosine for a given 32-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the arc-cosine.</param>
    /// <returns>The arc-cosine of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Acos(float value) => MathF.Acos(value);

    /// <summary>Rounds a given address up to the nearest alignment.</summary>
    /// <param name="address">The address to be aligned.</param>
    /// <param name="alignment">The target alignment, which should be a power of two.</param>
    /// <returns><paramref name="address" /> rounded up to the specified <paramref name="alignment" />.</returns>
    /// <remarks>This method does not account for an <paramref name="alignment" /> which is not a <c>power of two</c>.</remarks>
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
    /// <remarks>This method does not account for an <paramref name="alignment" /> which is not a <c>power of two</c>.</remarks>
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
    /// <remarks>This method does not account for an <paramref name="alignment" /> which is not a <c>power of two</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint AlignUp(nuint address, nuint alignment)
    {
        Assert(AssertionsEnabled && IsPow2(alignment));
        return (address + (alignment - 1)) & ~(alignment - 1);
    }

    /// <summary>Computes the arc-sine for a given 64-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the arc-sine.</param>
    /// <returns>The arc-sine of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Asin(double value) => Math.Asin(value);

    /// <summary>Computes the arc-sine for a given 32-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the arc-sine.</param>
    /// <returns>The arc-sine of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Asin(float value) => MathF.Asin(value);

    /// <summary>Computes the arc-tangent for a given 64-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the arc-tangent.</param>
    /// <returns>The arc-tangent of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Atan(double value) => Math.Atan(value);

    /// <summary>Computes the arc-tangent for a given 32-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the arc-tangent.</param>
    /// <returns>The arc-tangent of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Atan(float value) => MathF.Atan(value);

    /// <summary>Clamps an 8-bit unsigned integer to be between a minimum and maximum value.</summary>
    /// <param name="value">The value to restrict.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <remarks>This method does not account for <paramref name="min" /> being greater than <paramref name="max" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Clamp(byte value, byte min, byte max)
    {
        Assert(AssertionsEnabled && (min <= max));

        // The compare order here is important.
        // It ensures we match HLSL behavior for the scenario where min is larger than max.

        var result = value;

        result = Max(result, min);
        result = Min(result, max);

        return result;
    }

    /// <summary>Clamps a 64-bit float to be between a minimum and maximum value.</summary>
    /// <param name="value">The value to restrict.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <remarks>This method does not account for <paramref name="min" /> being greater than <paramref name="max" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Clamp(double value, double min, double max)
    {
        Assert(AssertionsEnabled && (min <= max));

        // The compare order here is important.
        // It ensures we match HLSL behavior for the scenario where min is larger than max.

        var result = value;

        result = Max(result, min);
        result = Min(result, max);

        return result;
    }

    /// <summary>Clamps a 16-bit signed integer to be between a minimum and maximum value.</summary>
    /// <param name="value">The value to restrict.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <remarks>This method does not account for <paramref name="min" /> being greater than <paramref name="max" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short Clamp(short value, short min, short max)
    {
        Assert(AssertionsEnabled && (min <= max));

        // The compare order here is important.
        // It ensures we match HLSL behavior for the scenario where min is larger than max.

        var result = value;

        result = Max(result, min);
        result = Min(result, max);

        return result;
    }

    /// <summary>Clamps a 32-bit signed integer to be between a minimum and maximum value.</summary>
    /// <param name="value">The value to restrict.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <remarks>This method does not account for <paramref name="min" /> being greater than <paramref name="max" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(int value, int min, int max)
    {
        Assert(AssertionsEnabled && (min <= max));

        // The compare order here is important.
        // It ensures we match HLSL behavior for the scenario where min is larger than max.

        var result = value;

        result = Max(result, min);
        result = Min(result, max);

        return result;
    }

    /// <summary>Clamps a 64-bit signed integer to be between a minimum and maximum value.</summary>
    /// <param name="value">The value to restrict.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <remarks>This method does not account for <paramref name="min" /> being greater than <paramref name="max" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Clamp(long value, long min, long max)
    {
        Assert(AssertionsEnabled && (min <= max));

        // The compare order here is important.
        // It ensures we match HLSL behavior for the scenario where min is larger than max.

        var result = value;

        result = Max(result, min);
        result = Min(result, max);

        return result;
    }

    /// <summary>Clamps a signed native integer to be between a minimum and maximum value.</summary>
    /// <param name="value">The value to restrict.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <remarks>This method does not account for <paramref name="min" /> being greater than <paramref name="max" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nint Clamp(nint value, nint min, nint max)
    {
        Assert(AssertionsEnabled && (min <= max));

        // The compare order here is important.
        // It ensures we match HLSL behavior for the scenario where min is larger than max.

        var result = value;

        result = Max(result, min);
        result = Min(result, max);

        return result;
    }

    /// <summary>Clamps an 8-bit signed integer to be between a minimum and maximum value.</summary>
    /// <param name="value">The value to restrict.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <remarks>This method does not account for <paramref name="min" /> being greater than <paramref name="max" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte Clamp(sbyte value, sbyte min, sbyte max)
    {
        Assert(AssertionsEnabled && (min <= max));

        // The compare order here is important.
        // It ensures we match HLSL behavior for the scenario where min is larger than max.

        var result = value;

        result = Max(result, min);
        result = Min(result, max);

        return result;
    }

    /// <summary>Clamps a 32-bit float to be between a minimum and maximum value.</summary>
    /// <param name="value">The value to restrict.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <remarks>This method does not account for <paramref name="min" /> being greater than <paramref name="max" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp(float value, float min, float max)
    {
        Assert(AssertionsEnabled && (min <= max));

        // The compare order here is important.
        // It ensures we match HLSL behavior for the scenario where min is larger than max.

        var result = value;

        result = Max(result, min);
        result = Min(result, max);

        return result;
    }

    /// <summary>Clamps a 16-bit unsigned integer to be between a minimum and maximum value.</summary>
    /// <param name="value">The value to restrict.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <remarks>This method does not account for <paramref name="min" /> being greater than <paramref name="max" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort Clamp(ushort value, ushort min, ushort max)
    {
        Assert(AssertionsEnabled && (min <= max));

        // The compare order here is important.
        // It ensures we match HLSL behavior for the scenario where min is larger than max.

        var result = value;

        result = Max(result, min);
        result = Min(result, max);

        return result;
    }

    /// <summary>Clamps a 32-bit unsigned integer to be between a minimum and maximum value.</summary>
    /// <param name="value">The value to restrict.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <remarks>This method does not account for <paramref name="min" /> being greater than <paramref name="max" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Clamp(uint value, uint min, uint max)
    {
        Assert(AssertionsEnabled && (min <= max));

        // The compare order here is important.
        // It ensures we match HLSL behavior for the scenario where min is larger than max.

        var result = value;

        result = Max(result, min);
        result = Min(result, max);

        return result;
    }

    /// <summary>Clamps a 64-bit unsigned integer to be between a minimum and maximum value.</summary>
    /// <param name="value">The value to restrict.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <remarks>This method does not account for <paramref name="min" /> being greater than <paramref name="max" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Clamp(ulong value, ulong min, ulong max)
    {
        Assert(AssertionsEnabled && (min <= max));

        // The compare order here is important.
        // It ensures we match HLSL behavior for the scenario where min is larger than max.

        var result = value;

        result = Max(result, min);
        result = Min(result, max);

        return result;
    }

    /// <summary>Clamps an unsigned native integer to be between a minimum and maximum value.</summary>
    /// <param name="value">The value to restrict.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns><paramref name="value" /> clamped to be between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <remarks>This method does not throw if <paramref name="min" /> is greater than <paramref name="max" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint Clamp(nuint value, nuint min, nuint max)
    {
        Assert(AssertionsEnabled && (min <= max));

        // The compare order here is important.
        // It ensures we match HLSL behavior for the scenario where min is larger than max.

        var result = value;

        result = Max(result, min);
        result = Min(result, max);

        return result;
    }

    /// <summary>Computes the cosine for a given 64-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the cosine.</param>
    /// <returns>The cosine of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Cos(double value) => Math.Cos(value);

    /// <summary>Computes the cosine for a given 32-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the cosine.</param>
    /// <returns>The cosine of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Cos(float value) => MathF.Cos(value);

    /// <summary>Computes the quotient of two 32-bit unsigned integers but rounds the result up, rather than down.</summary>
    /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
    /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
    /// <returns>The quotient of <paramref name="dividend" /> / <paramref name="divisor" />, rounded up.</returns>
    /// <remarks>This method does not account for <paramref name="divisor" /> being <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint DivideRoundingUp(uint dividend, uint divisor)
    {
        Assert(AssertionsEnabled && (divisor != 0));
        return (dividend + divisor - 1) / divisor;
    }

    /// <summary>Computes the quotient of two 64-bit unsigned integers but rounds the result up, rather than down.</summary>
    /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
    /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
    /// <returns>The quotient of <paramref name="dividend" /> / <paramref name="divisor" />, rounded up.</returns>
    /// <remarks>This method does not account for <paramref name="divisor" /> being <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong DivideRoundingUp(ulong dividend, ulong divisor)
    {
        Assert(AssertionsEnabled && (divisor != 0));
        return (dividend + divisor - 1) / divisor;
    }

    /// <summary>Computes the quotient of two unsigned native integers but rounds the result up, rather than down.</summary>
    /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
    /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
    /// <returns>The quotient of <paramref name="dividend" /> / <paramref name="divisor" />, rounded up.</returns>
    /// <remarks>This method does not account for <paramref name="divisor" /> being <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint DivideRoundingUp(nuint dividend, nuint divisor)
    {
        Assert(AssertionsEnabled && (divisor != 0));
        return (dividend + divisor - 1) / divisor;
    }

    /// <summary>Computes the quotient and remainder of two 32-bit signed integers.</summary>
    /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
    /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
    /// <returns>The quotient and remainder of <paramref name="dividend" /> / <paramref name="divisor" />.</returns>
    /// <remarks>This method does not account for <paramref name="divisor" /> being <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int quotient, int remainder) DivRem(int dividend, int divisor) => Math.DivRem(dividend, divisor);

    /// <summary>Computes the quotient and remainder of two 64-bit signed  integers.</summary>
    /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
    /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
    /// <returns>The quotient and remainder of <paramref name="dividend" /> / <paramref name="divisor" />.</returns>
    /// <remarks>This method does not account for <paramref name="divisor" /> being <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (long quotient, long remainder) DivRem(long dividend, long divisor) => Math.DivRem(dividend, divisor);

    /// <summary>Computes the quotient and remainder of two signed native integers.</summary>
    /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
    /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
    /// <returns>The quotient and remainder of <paramref name="dividend" /> / <paramref name="divisor" />.</returns>
    /// <remarks>This method does not account for <paramref name="divisor" /> being <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (nint quotient, nint remainder) DivRem(nint dividend, nint divisor) => Math.DivRem(dividend, divisor);

    /// <summary>Computes the quotient and remainder of two 32-bit unsigned integers.</summary>
    /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
    /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
    /// <returns>The quotient and remainder of <paramref name="dividend" /> / <paramref name="divisor" />.</returns>
    /// <remarks>This method does not account for <paramref name="divisor" /> being <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (uint quotient, uint remainder) DivRem(uint dividend, uint divisor) => Math.DivRem(dividend, divisor);

    /// <summary>Computes the quotient and remainder of two 64-bit unsigned  integers.</summary>
    /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
    /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
    /// <returns>The quotient and remainder of <paramref name="dividend" /> / <paramref name="divisor" />.</returns>
    /// <remarks>This method does not account for <paramref name="divisor" /> being <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (ulong quotient, ulong remainder) DivRem(ulong dividend, ulong divisor) => Math.DivRem(dividend, divisor);

    /// <summary>Computes the quotient and remainder of two unsigned native integers.</summary>
    /// <param name="dividend">The value being divided by <paramref name="divisor" />.</param>
    /// <param name="divisor">The value that is used to divide <paramref name="dividend" />.</param>
    /// <returns>The quotient and remainder of <paramref name="dividend" /> / <paramref name="divisor" />.</returns>
    /// <remarks>This method does not account for <paramref name="divisor" /> being <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (nuint quotient, nuint remainder) DivRem(nuint dividend, nuint divisor) => Math.DivRem(dividend, divisor);

    /// <summary>Compares two 64-bit floats to determine approximate equality.</summary>
    /// <param name="left">The float to compare with <paramref name="right" />.</param>
    /// <param name="right">The float to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(double left, double right, double epsilon) => Abs(left - right) <= epsilon;

    /// <summary>Compares two 32-bit floats to determine approximate equality.</summary>
    /// <param name="left">The float to compare with <paramref name="right" />.</param>
    /// <param name="right">The float to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(float left, float right, float epsilon) => Abs(left - right) <= epsilon;

    /// <summary>Computes the maximum of two 32-bit signed integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Max(byte left, byte right) => (left > right) ? left : right;

    /// <summary>Computes the maximum of two 64-bit floats.</summary>
    /// <param name="left">The float to compare with <paramref name="right" />.</param>
    /// <param name="right">The float to compare with <paramref name="left" />.</param>
    /// <returns>The maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Max(double left, double right)
    {
        if (Sse41.IsSupported)
        {
            // TODO: This isn't correctly taking +0.0 vs -0.0 into account

            var vLeft = Vector128.CreateScalarUnsafe(left);
            var vRight = Vector128.CreateScalarUnsafe(right);

            var tmp = Sse2.Max(vLeft, vRight);
            var msk = Sse2.CompareUnordered(vLeft, vLeft);

            return Sse41.BlendVariable(tmp, vLeft, msk).ToScalar();
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.MaxScalar(
                Vector64.CreateScalar(left),
                Vector64.CreateScalar(right)
            ).ToScalar();
        }
        else
        {
            return Math.Max(left, right);
        }
    }

    /// <summary>Computes the maximum of two 16-bit signed integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short Max(short left, short right) => (left > right) ? left : right;

    /// <summary>Computes the maximum of two 32-bit signed integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Max(int left, int right) => (left > right) ? left : right;

    /// <summary>Computes the maximum of two 64-bit signed integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Max(long left, long right) => (left > right) ? left : right;

    /// <summary>Computes the maximum of two signed native integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nint Max(nint left, nint right) => (left > right) ? left : right;

    /// <summary>Computes the maximum of two 8-bit signed integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte Max(sbyte left, sbyte right) => (left > right) ? left : right;

    /// <summary>Computes the maximum of two 32-bit floats.</summary>
    /// <param name="left">The float to compare with <paramref name="right" />.</param>
    /// <param name="right">The float to compare with <paramref name="left" />.</param>
    /// <returns>The maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Max(float left, float right)
    {
        if (Sse41.IsSupported)
        {
            // TODO: This isn't correctly taking +0.0 vs -0.0 into account

            var vLeft = Vector128.CreateScalarUnsafe(left);
            var vRight = Vector128.CreateScalarUnsafe(right);

            var tmp = Sse.Max(vLeft, vRight);
            var msk = Sse.CompareUnordered(vLeft, vLeft);

            return Sse41.BlendVariable(tmp, vLeft, msk).ToScalar();
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.MaxScalar(
                Vector64.CreateScalar(left),
                Vector64.CreateScalar(right)
            ).ToScalar();
        }
        else
        {
            return MathF.Max(left, right);
        }
    }

    /// <summary>Computes the maximum of two 16-bit unsigned integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort Max(ushort left, ushort right) => (left > right) ? left : right;

    /// <summary>Computes the maximum of two 32-bit unsigned integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Max(uint left, uint right) => (left > right) ? left : right;

    /// <summary>Computes the maximum of two 64-bit unsigned integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Max(ulong left, ulong right) => (left > right) ? left : right;

    /// <summary>Computes the maximum of two unsigned native integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint Max(nuint left, nuint right) => (left > right) ? left : right;

    /// <summary>Computes the minimum of two 8-bit unsigned integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Min(byte left, byte right) => (left < right) ? left : right;

    /// <summary>Computes the minimum of two 64-bit floats.</summary>
    /// <param name="left">The float to compare with <paramref name="right" />.</param>
    /// <param name="right">The float to compare with <paramref name="left" />.</param>
    /// <returns>The minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    /// <remarks>This method does not account for <c>negative zero</c> and returns the other parameter if one is <see cref="double.NaN" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Min(double left, double right)
    {
        if (Sse41.IsSupported)
        {
            // TODO: This isn't correctly taking +0.0 vs -0.0 into account

            var vLeft = Vector128.CreateScalarUnsafe(left);
            var vRight = Vector128.CreateScalarUnsafe(right);

            var tmp = Sse2.Min(vLeft, vRight);
            var msk = Sse2.CompareUnordered(vLeft, vLeft);

            return Sse41.BlendVariable(tmp, vLeft, msk).ToScalar();
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.MinScalar(
                Vector64.CreateScalar(left),
                Vector64.CreateScalar(right)
            ).ToScalar();
        }
        else
        {
            return Math.Min(left, right);
        }
    }

    /// <summary>Computes the minimum of two 16-bit signed integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short Min(short left, short right) => (left < right) ? left : right;

    /// <summary>Computes the minimum of two 32-bit signed integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Min(int left, int right) => (left < right) ? left : right;

    /// <summary>Computes the minimum of two 64-bit signed integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Min(long left, long right) => (left < right) ? left : right;

    /// <summary>Computes the minimum of two signed native integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nint Min(nint left, nint right) => (left < right) ? left : right;

    /// <summary>Computes the minimum of two 8-bit signed integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte Min(sbyte left, sbyte right) => (left < right) ? left : right;

    /// <summary>Computes the minimum of two 32-bit floats.</summary>
    /// <param name="left">The float to compare with <paramref name="right" />.</param>
    /// <param name="right">The float to compare with <paramref name="left" />.</param>
    /// <returns>The minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    /// <remarks>This method does not account for <c>negative zero</c> and returns the other parameter if one is <see cref="double.NaN" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Min(float left, float right)
    {
        if (Sse41.IsSupported)
        {
            // TODO: This isn't correctly taking +0.0 vs -0.0 into account

            var vLeft = Vector128.CreateScalarUnsafe(left);
            var vRight = Vector128.CreateScalarUnsafe(right);

            var tmp = Sse.Min(vLeft, vRight);
            var msk = Sse.CompareUnordered(vLeft, vLeft);

            return Sse41.BlendVariable(tmp, vLeft, msk).ToScalar();
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.MinScalar(
                Vector64.CreateScalar(left),
                Vector64.CreateScalar(right)
            ).ToScalar();
        }
        else
        {
            return MathF.Min(left, right);
        }
    }

    /// <summary>Computes the minimum of two 16-bit unsigned integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort Min(ushort left, ushort right) => (left < right) ? left : right;

    /// <summary>Computes the minimum of two 32-bit unsigned integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Min(uint left, uint right) => (left < right) ? left : right;

    /// <summary>Computes the minimum of two 64-bit unsigned integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Min(ulong left, ulong right) => (left < right) ? left : right;

    /// <summary>Computes the minimum of two unsigned native integers.</summary>
    /// <param name="left">The integer to compare with <paramref name="right" />.</param>
    /// <param name="right">The integer to compare with <paramref name="left" />.</param>
    /// <returns>The minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint Min(nuint left, nuint right) => (left < right) ? left : right;

    /// <summary>Determines whether a given value is a power of two.</summary>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if <paramref name="value" /> is a power of two; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(uint value) => BitOperations.IsPow2(value);

    /// <summary>Determines whether a given value is a power of two.</summary>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if <paramref name="value" /> is a power of two; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(ulong value) => BitOperations.IsPow2(value);

    /// <summary>Determines whether a given value is a power of two.</summary>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if <paramref name="value" /> is a power of two; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(nuint value)
        => Is64BitProcess ? BitOperations.IsPow2(value) : BitOperations.IsPow2((uint)value);

    /// <summary>Computes an estimate of the reciprocal of a given 64-bit float.</summary>
    /// <param name="value">The float for which to compute the reciprocal.</param>
    /// <returns>An estimate of the reciprocal of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReciprocalEstimate(double value) => Math.ReciprocalEstimate(value);

    /// <summary>Computes an estimate of the reciprocal of a given 32-bit float.</summary>
    /// <param name="value">The float for which to compute the reciprocal.</param>
    /// <returns>An estimate of the reciprocal of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReciprocalEstimate(float value) => MathF.ReciprocalEstimate(value);

    /// <summary>Computes an estimate of the reciprocal square-root of a given 64-bit float.</summary>
    /// <param name="value">The float for which to compute the reciprocal square-root.</param>
    /// <returns>An estimate of the reciprocal square-root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReciprocalSqrtEstimate(double value) => Math.ReciprocalSqrtEstimate(value);

    /// <summary>Computes an estimate of the reciprocal square-root of a given 32-bit float.</summary>
    /// <param name="value">The float for which to compute the reciprocal square-root.</param>
    /// <returns>An estimate of the reciprocal square-root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReciprocalSqrtEstimate(float value) => MathF.ReciprocalSqrtEstimate(value);

    /// <summary>Computes the sine for a given 64-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the sine.</param>
    /// <returns>The sine of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Sin(double value) => Math.Sin(value);

    /// <summary>Computes the sine for a given 32-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the sine.</param>
    /// <returns>The sine of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sin(float value) => MathF.Sin(value);

    /// <summary>Computes the sine and cosine for a given 64-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the sine and cosine.</param>
    /// <returns>The sine and cosine of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (double Sin, double Cos) SinCos(double value) => (Sin(value), Cos(value));

    /// <summary>Computes the sine and cosine for a given 32-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the sine and cosine.</param>
    /// <returns>The sine and cosine of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (float Sin, float Cos) SinCos(float value) => (Sin(value), Cos(value));

    /// <summary>Computes the square root of a given 64-bit float.</summary>
    /// <param name="value">The float for which to compute the square root.</param>
    /// <returns>The square root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Sqrt(double value) => Math.Sqrt(value);

    /// <summary>Computes the square root of a given 32-bit float.</summary>
    /// <param name="value">The float for which to compute the square root.</param>
    /// <returns>The square root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sqrt(float value) => MathF.Sqrt(value);

    /// <summary>Computes the tangent for a given 64-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the tangent.</param>
    /// <returns>The tangent of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Tan(double value) => Math.Tan(value);

    /// <summary>Computes the tangent for a given 32-bit float.</summary>
    /// <param name="value">The float, in radians, for which to compute the tangent.</param>
    /// <returns>The tangent of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Tan(float value) => MathF.Tan(value);
}
