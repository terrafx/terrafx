// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace TerraFX.Utilities;

/// <summary>Provides a set of methods to supplement or replace <see cref="Vector128" /> and <see cref="Vector128{T}" />.</summary>
public static class VectorUtilities
{
    /// <summary>Gets a value used to represent all bits set.</summary>
    public static float AllBitsSet
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return BitConverter.UInt32BitsToSingle(0xFFFFFFFF);
        }
    }

    /// <summary>Gets a value used to determine if a value is near zero.</summary>
    public static float NearZeroEpsilon
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return 4.7683716E-07f; // 2^-21: 0x35000000
        }
    }

    /// <summary>Computes the absolute value of a vector.</summary>
    /// <param name="value">The vector for which to get its absolute value.</param>
    /// <returns>The absolute value of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Abs(Vector128<float> value)
    {
        if (Sse41.IsSupported)
        {
            return Sse.And(value, Vector128.Create(0x7FFFFFFF).AsSingle());
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Abs(value);
        }
        else
        {
            return SoftwareFallback(value);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> value)
        {
            return Vector128.Create(
                MathF.Abs(value.GetElement(0)),
                MathF.Abs(value.GetElement(1)),
                MathF.Abs(value.GetElement(2)),
                MathF.Abs(value.GetElement(3))
            );
        }
    }

    /// <summary>Computes the sum of two vectors.</summary>
    /// <param name="left">The vector to which to add <paramref name="right" />.</param>
    /// <param name="right">The vector which is added to <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="right" /> added to <paramref name="left" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Add(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Add(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Add(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                left.GetElement(0) + right.GetElement(0),
                left.GetElement(1) + right.GetElement(1),
                left.GetElement(2) + right.GetElement(2),
                left.GetElement(3) + right.GetElement(3)
            );
        }
    }

    /// <summary>Computes the bitwise-and two vectors.</summary>
    /// <param name="left">The vector to bitwise-and with <paramref name="right" />.</param>
    /// <param name="right">The vector to bitwise-and with <paramref name="left" />.</param>
    /// <returns>The bitwise-and of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> BitwiseAnd(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.And(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.And(left, right);
        }
        else
        {
            var result = SoftwareFallback(left.AsUInt32(), right.AsUInt32());
            return result.AsSingle();
        }

        static Vector128<uint> SoftwareFallback(Vector128<uint> left, Vector128<uint> right)
        {
            return Vector128.Create(
                left.GetElement(0) & right.GetElement(0),
                left.GetElement(1) & right.GetElement(1),
                left.GetElement(2) & right.GetElement(2),
                left.GetElement(3) & right.GetElement(3)
            );
        }
    }

    /// <summary>Computes the bitwise-and two vectors.</summary>
    /// <param name="left">The vector to bitwise-and with <paramref name="right" />.</param>
    /// <param name="right">The vector to bitwise-and with <paramref name="left" />.</param>
    /// <returns>The bitwise-and of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> BitwiseAndNot(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.AndNot(right, left);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.BitwiseClear(left, right);
        }
        else
        {
            var result = SoftwareFallback(left.AsUInt32(), right.AsUInt32());
            return result.AsSingle();
        }

        static Vector128<uint> SoftwareFallback(Vector128<uint> left, Vector128<uint> right)
        {
            return Vector128.Create(
                left.GetElement(0) & ~right.GetElement(0),
                left.GetElement(1) & ~right.GetElement(1),
                left.GetElement(2) & ~right.GetElement(2),
                left.GetElement(3) & ~right.GetElement(3)
            );
        }
    }

    /// <summary>Computes the bitwise-or two vectors.</summary>
    /// <param name="left">The vector to bitwise-or with <paramref name="right" />.</param>
    /// <param name="right">The vector to bitwise-or with <paramref name="left" />.</param>
    /// <returns>The bitwise-or of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> BitwiseOr(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Or(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Or(left, right);
        }
        else
        {
            var result = SoftwareFallback(left.AsUInt32(), right.AsUInt32());
            return result.AsSingle();
        }

        static Vector128<uint> SoftwareFallback(Vector128<uint> left, Vector128<uint> right)
        {
            return Vector128.Create(
                left.GetElement(0) | right.GetElement(0),
                left.GetElement(1) | right.GetElement(1),
                left.GetElement(2) | right.GetElement(2),
                left.GetElement(3) | right.GetElement(3)
            );
        }
    }

    /// <summary>Computes the bitwise-xor two vectors.</summary>
    /// <param name="left">The vector to bitwise-xor with <paramref name="right" />.</param>
    /// <param name="right">The vector to bitwise-xor with <paramref name="left" />.</param>
    /// <returns>The bitwise-xor of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> BitwiseXor(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Xor(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Xor(left, right);
        }
        else
        {
            var result = SoftwareFallback(left.AsUInt32(), right.AsUInt32());
            return result.AsSingle();
        }

        static Vector128<uint> SoftwareFallback(Vector128<uint> left, Vector128<uint> right)
        {
            return Vector128.Create(
                left.GetElement(0) ^ right.GetElement(0),
                left.GetElement(1) ^ right.GetElement(1),
                left.GetElement(2) ^ right.GetElement(2),
                left.GetElement(3) ^ right.GetElement(3)
            );
        }
    }

    /// <summary>Compares two vectors to determine which elements equivalent.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>A vector that contains the element-wise comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CompareEqual(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.CompareEqual(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.CompareEqual(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                (left.GetElement(0) == right.GetElement(0)) ? AllBitsSet : 0.0f,
                (left.GetElement(1) == right.GetElement(1)) ? AllBitsSet : 0.0f,
                (left.GetElement(2) == right.GetElement(2)) ? AllBitsSet : 0.0f,
                (left.GetElement(3) == right.GetElement(3)) ? AllBitsSet : 0.0f
            );
        }
    }

    /// <summary>Compares two vectors to determine approximate equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns>A vector that contains the element-wise approximate comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CompareEqual(Vector128<float> left, Vector128<float> right, Vector128<float> epsilon)
    {
        if (Sse41.IsSupported)
        {
            var result = Sse.Subtract(left, right);
            result = Sse.And(result, Vector128.Create(0x7FFFFFFF).AsSingle());
            return Sse.CompareLessThanOrEqual(result, epsilon);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.Subtract(left, right);
            result = AdvSimd.Abs(result);
            return AdvSimd.CompareLessThanOrEqual(result, epsilon);
        }
        else
        {
            return SoftwareFallback(left, right, epsilon);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right, Vector128<float> epsilon)
        {
            return Vector128.Create(
                (MathUtilities.Abs(left.GetElement(0) - right.GetElement(0)) <= epsilon.GetElement(0)) ? AllBitsSet : 0.0f,
                (MathUtilities.Abs(left.GetElement(1) - right.GetElement(1)) <= epsilon.GetElement(1)) ? AllBitsSet : 0.0f,
                (MathUtilities.Abs(left.GetElement(2) - right.GetElement(2)) <= epsilon.GetElement(2)) ? AllBitsSet : 0.0f,
                (MathUtilities.Abs(left.GetElement(3) - right.GetElement(3)) <= epsilon.GetElement(3)) ? AllBitsSet : 0.0f
            );
        }
    }

    /// <summary>Compares two vectors to determine equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            var result = Sse.CompareNotEqual(left, right);
            return Sse.MoveMask(result) == 0x00;
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.CompareEqual(left, right);
            return AdvSimd.Arm64.MinAcross(result).ToScalar() != 0;
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static bool SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return (left.GetElement(0) == right.GetElement(0))
                && (left.GetElement(1) == right.GetElement(1))
                && (left.GetElement(2) == right.GetElement(2))
                && (left.GetElement(3) == right.GetElement(3));
        }
    }

    /// <summary>Compares two vectors to determine if all elements are approximately equal.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">he maximum (inclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareEqualAll(Vector128<float> left, Vector128<float> right, Vector128<float> epsilon)
    {
        if (Sse41.IsSupported)
        {
            var result = Sse.Subtract(left, right);
            result = Sse.And(result, Vector128.Create(0x7FFFFFFF).AsSingle());
            result = Sse.CompareNotLessThanOrEqual(result, epsilon);
            return Sse.MoveMask(result) == 0x00;
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.Subtract(left, right);
            result = AdvSimd.Abs(result);
            result = AdvSimd.CompareLessThanOrEqual(result, epsilon);
            return AdvSimd.Arm64.MinAcross(result).ToScalar() != 0;
        }
        else
        {
            return SoftwareFallback(left, right, epsilon);
        }

        static bool SoftwareFallback(Vector128<float> left, Vector128<float> right, Vector128<float> epsilon)
        {
            return (MathUtilities.Abs(left.GetElement(0) - right.GetElement(0)) <= epsilon.GetElement(0))
                && (MathUtilities.Abs(left.GetElement(1) - right.GetElement(1)) <= epsilon.GetElement(1))
                && (MathUtilities.Abs(left.GetElement(2) - right.GetElement(2)) <= epsilon.GetElement(2))
                && (MathUtilities.Abs(left.GetElement(3) - right.GetElement(3)) <= epsilon.GetElement(3));
        }
    }

    /// <summary>Compares two vectors to determine which elements are greater or equivalent.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>A vector that contains the element-wise comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CompareGreaterThan(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.CompareGreaterThan(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.CompareGreaterThan(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                (left.GetElement(0) > right.GetElement(0)) ? AllBitsSet : 0.0f,
                (left.GetElement(1) > right.GetElement(1)) ? AllBitsSet : 0.0f,
                (left.GetElement(2) > right.GetElement(2)) ? AllBitsSet : 0.0f,
                (left.GetElement(3) > right.GetElement(3)) ? AllBitsSet : 0.0f
            );
        }
    }

    /// <summary>Compares two vectors to determine which elements are greater.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>A vector that contains the element-wise comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CompareGreaterThanOrEqual(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.CompareGreaterThanOrEqual(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.CompareGreaterThanOrEqual(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                (left.GetElement(0) >= right.GetElement(0)) ? AllBitsSet : 0.0f,
                (left.GetElement(1) >= right.GetElement(1)) ? AllBitsSet : 0.0f,
                (left.GetElement(2) >= right.GetElement(2)) ? AllBitsSet : 0.0f,
                (left.GetElement(3) >= right.GetElement(3)) ? AllBitsSet : 0.0f
            );
        }
    }

    /// <summary>Compares two vectors to determine which elements are lesser.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>A vector that contains the element-wise comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CompareLessThan(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.CompareLessThan(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.CompareLessThan(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                (left.GetElement(0) < right.GetElement(0)) ? AllBitsSet : 0.0f,
                (left.GetElement(1) < right.GetElement(1)) ? AllBitsSet : 0.0f,
                (left.GetElement(2) < right.GetElement(2)) ? AllBitsSet : 0.0f,
                (left.GetElement(3) < right.GetElement(3)) ? AllBitsSet : 0.0f
            );
        }
    }

    /// <summary>Compares two vectors to determine which elements are lesser or equivalent.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>A vector that contains the element-wise comparison of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CompareLessThanOrEqual(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.CompareLessThanOrEqual(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.CompareLessThanOrEqual(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                (left.GetElement(0) <= right.GetElement(0)) ? AllBitsSet : 0.0f,
                (left.GetElement(1) <= right.GetElement(1)) ? AllBitsSet : 0.0f,
                (left.GetElement(2) <= right.GetElement(2)) ? AllBitsSet : 0.0f,
                (left.GetElement(3) <= right.GetElement(3)) ? AllBitsSet : 0.0f
            );
        }
    }

    /// <summary>Compares two vectors to determine equality.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareNotEqualAny(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            var result = Sse.CompareNotEqual(left, right);
            return Sse.MoveMask(result) != 0x00;
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.CompareEqual(left, right);
            return AdvSimd.Arm64.MaxAcross(result).ToScalar() == 0;
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static bool SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return (left.GetElement(0) != right.GetElement(0))
                || (left.GetElement(1) != right.GetElement(1))
                || (left.GetElement(2) != right.GetElement(2))
                || (left.GetElement(3) != right.GetElement(3));
        }
    }

    /// <summary>Checks a vector to determine if all elements represent <c>true</c>.</summary>
    /// <param name="value">The vector to check.</param>
    /// <returns><c>true</c> if all elements in <paramref name="value" /> represent <c>true</c>; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareTrueAll(Vector128<float> value)
    {
        if (Sse41.IsSupported)
        {
            return Sse.MoveMask(value) == 0x0F;
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.MinAcross(value).ToScalar() != 0;
        }
        else
        {
            return SoftwareFallback(value.AsUInt32());
        }

        static bool SoftwareFallback(Vector128<uint> value)
        {
            return (value.GetElement(0) != 0)
                && (value.GetElement(1) != 0)
                && (value.GetElement(2) != 0)
                && (value.GetElement(3) != 0);
        }
    }

    /// <summary>Checks a vector to determine if any elements represent <c>true</c>.</summary>
    /// <param name="value">The vector to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> represent <c>true</c>; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareTrueAny(Vector128<float> value)
    {
        if (Sse41.IsSupported)
        {
            return Sse.MoveMask(value) != 0x00;
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.MaxAcross(value).ToScalar() != 0;
        }
        else
        {
            return SoftwareFallback(value.AsUInt32());
        }

        static bool SoftwareFallback(Vector128<uint> value)
        {
            return (value.GetElement(0) != 0)
                || (value.GetElement(1) != 0)
                || (value.GetElement(2) != 0)
                || (value.GetElement(3) != 0);
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with all elements initialized to the <c>X</c> component of the input vector.</summary>
    /// <param name="value">The vector whose component is used to initialize all elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with all elements initialized to the component of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromX(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b00_00_00_00);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b00_00_00_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.DuplicateSelectedScalarToVector128(value, 0);
        }
        else
        {
            var x = value.GetElement(0);
            return Vector128.Create(x);
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with all elements initialized to the <c>Y</c> component of the input vector.</summary>
    /// <param name="value">The vector whose component is used to initialize all elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with all elements initialized to the component of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromY(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b01_01_01_01);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b01_01_01_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.DuplicateSelectedScalarToVector128(value, 1);
        }
        else
        {
            var y = value.GetElement(1);
            return Vector128.Create(y);
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with all elements initialized to the <c>Z</c> component of the input vector.</summary>
    /// <param name="value">The vector whose component is used to initialize all elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with all elements initialized to the component of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZ(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b10_10_10_10);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b10_10_10_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.DuplicateSelectedScalarToVector128(value, 2);
        }
        else
        {
            var z = value.GetElement(2);
            return Vector128.Create(z);
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with all elements initialized to the <c>W</c> component of the input vector.</summary>
    /// <param name="value">The vector whose component is used to initialize all elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with all elements initialized to the component of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromW(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b11_11_11_11);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b11_11_11_11);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.DuplicateSelectedScalarToVector128(value, 3);
        }
        else
        {
            var w = value.GetElement(3);
            return Vector128.Create(w);
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>X</c>, <c>Y</c>, and <c>W</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXXYW(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b11_01_00_00);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b11_01_00_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.Arm64.InsertSelectedScalar(value, 1, value, 0);
            return AdvSimd.Arm64.InsertSelectedScalar(result, 2, value, 1);
        }
        else
        {
            return Vector128.Create(
                value.GetElement(0),
                value.GetElement(0),
                value.GetElement(1),
                value.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>X</c>, <c>Y</c>, and <c>Y</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXXYY(Vector128<float> value)
    {
        if (Sse41.IsSupported)
        {
            return Sse.UnpackLow(value, value);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.ZipLow(value, value);
        }
        else
        {
            return Vector128.Create(
                value.GetElement(0),
                value.GetElement(0),
                value.GetElement(1),
                value.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>Z</c>, <c>X</c>, and <c>Z</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXZXZ(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b10_00_10_00);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b10_00_10_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.UnzipEven(value, value);
        }
        else
        {
            return Vector128.Create(
                value.GetElement(0),
                value.GetElement(2),
                value.GetElement(0),
                value.GetElement(2)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>Z</c>, <c>Y</c>, and <c>W</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXZYW(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b11_01_10_00);
        }
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b11_01_10_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.Arm64.UnzipEven(value, value).GetLower(),
                AdvSimd.Arm64.UnzipOdd(value, value).GetUpper()
            );
        }
        else
        {
            return Vector128.Create(
                value.GetElement(0),
                value.GetElement(2),
                value.GetElement(1),
                value.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>Z</c>, <c>W</c>, and <c>Y</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXZWY(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b01_11_10_00);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b01_11_10_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var indices = Vector128.Create(
                0x0, 0x1, 0x2, 0x3, // X
                0x8, 0x9, 0xA, 0xB, // Z
                0xC, 0xD, 0xE, 0xF, // W
                0x4, 0x5, 0x6, 0x7  // Y
            );
            return AdvSimd.Arm64.VectorTableLookup(value.AsSByte(), indices).AsSingle();
        }
        else
        {
            return Vector128.Create(
                value.GetElement(0),
                value.GetElement(2),
                value.GetElement(3),
                value.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>W</c>, <c>Z</c>, and <c>X</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXWZX(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b00_10_11_00);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b00_10_11_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var indices = Vector128.Create(
                0x0, 0x1, 0x2, 0x3, // X
                0xC, 0xD, 0xE, 0xF, // W
                0x8, 0x9, 0xA, 0xB, // Z
                0x0, 0x1, 0x2, 0x3  // X
            );
            return AdvSimd.Arm64.VectorTableLookup(value.AsSByte(), indices).AsSingle();
        }
        else
        {
            return Vector128.Create(
                value.GetElement(0),
                value.GetElement(3),
                value.GetElement(2),
                value.GetElement(0)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>X</c>, <c>X</c>, and <c>X</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYXXX(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b00_00_00_01);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b00_00_00_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.DuplicateSelectedScalarToVector128(value, 0);
            return AdvSimd.Arm64.InsertSelectedScalar(result, 0, value, 1);
        }
        else
        {
            return Vector128.Create(
                value.GetElement(1),
                value.GetElement(0),
                value.GetElement(0),
                value.GetElement(0)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>X</c>, <c>X</c>, and <c>W</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYXXW(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b11_00_00_01);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b11_00_00_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var lower = value.GetLower().AsUInt64();
            lower = AdvSimd.ReverseElement32(lower);

            var result = Vector128.Create(lower.AsSingle(), value.GetUpper());
            return AdvSimd.Arm64.InsertSelectedScalar(result, 2, value, 0);
        }
        else
        {
            return Vector128.Create(
                value.GetElement(1),
                value.GetElement(0),
                value.GetElement(0),
                value.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>X</c>, <c>Z</c>, and <c>W</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYXZW(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b11_10_00_01);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b11_10_00_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.ReverseElement32(value.GetLower().AsInt64()).AsSingle(),
                value.GetUpper()
            );
        }
        else
        {
            return Vector128.Create(
                value.GetElement(1),
                value.GetElement(0),
                value.GetElement(2),
                value.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>X</c>, <c>W</c>, and <c>Z</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYXWZ(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b10_11_00_01);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b10_11_00_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = value.AsUInt64();
            result = AdvSimd.ReverseElement32(result);
            return result.AsSingle();
        }
        else
        {
            return Vector128.Create(
                value.GetElement(1),
                value.GetElement(0),
                value.GetElement(3),
                value.GetElement(2)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>Z</c>, <c>X</c>, and <c>Y</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYZXY(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b01_00_10_01);
        }
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b01_00_10_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var lower = value.GetLower();
            return Vector128.Create(
                AdvSimd.ExtractVector64(value.GetUpper(), lower, 1),
                lower
            );
        }
        else
        {
            return Vector128.Create(
                value.GetElement(1),
                value.GetElement(2),
                value.GetElement(0),
                value.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>Z</c>, <c>X</c>, and <c>W</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYZXW(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b11_00_10_01);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b11_00_10_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var indices = Vector128.Create(
                0x4, 0x5, 0x6, 0x7, // Y
                0x8, 0x9, 0xA, 0xB, // Z
                0x0, 0x1, 0x2, 0x3, // X
                0xC, 0xD, 0xE, 0xF  // W
            );
            return AdvSimd.Arm64.VectorTableLookup(value.AsSByte(), indices).AsSingle();
        }
        else
        {
            return Vector128.Create(
                value.GetElement(1),
                value.GetElement(2),
                value.GetElement(0),
                value.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>W</c>, <c>X</c>, and <c>Z</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYWXZ(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b10_00_11_01);
        }
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b10_00_11_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var indices = Vector128.Create(
                0x4, 0x5, 0x6, 0x7, // Y
                0xC, 0xD, 0xE, 0xF, // W
                0x0, 0x1, 0x2, 0x3, // X
                0x8, 0x9, 0xA, 0xB  // Z
            );
            return AdvSimd.Arm64.VectorTableLookup(value.AsSByte(), indices).AsSingle();
        }
        else
        {
            return Vector128.Create(
                value.GetElement(1),
                value.GetElement(3),
                value.GetElement(0),
                value.GetElement(2)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>X</c>, <c>Y</c>, and <c>X</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZXYX(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b00_01_00_10);
        }
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b00_01_00_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var indices = Vector128.Create(
                0x8, 0x9, 0xA, 0xB, // Z
                0x0, 0x1, 0x2, 0x3, // X
                0x4, 0x5, 0x6, 0x7, // Y
                0x0, 0x1, 0x2, 0x3  // X
            );
            return AdvSimd.Arm64.VectorTableLookup(value.AsSByte(), indices).AsSingle();
        }
        else
        {
            return Vector128.Create(
                value.GetElement(2),
                value.GetElement(0),
                value.GetElement(1),
                value.GetElement(0)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>X</c>, <c>Y</c>, and <c>W</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZXYW(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b11_01_00_10);
        }
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b11_01_00_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var indices = Vector128.Create(
                0x8, 0x9, 0xA, 0xB, // Z
                0x0, 0x1, 0x2, 0x3, // X
                0x4, 0x5, 0x6, 0x7, // Y
                0xC, 0xD, 0xE, 0xF  // W
            );
            return AdvSimd.Arm64.VectorTableLookup(value.AsSByte(), indices).AsSingle();
        }
        else
        {
            return Vector128.Create(
                value.GetElement(2),
                value.GetElement(0),
                value.GetElement(1),
                value.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>X</c>, <c>W</c>, and <c>Y</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZXWY(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b01_11_00_10);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b01_11_00_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var indices = Vector128.Create(
                0x8, 0x9, 0xA, 0xB, // Z
                0x0, 0x1, 0x2, 0x3, // X
                0xC, 0xD, 0xE, 0xF, // W
                0x4, 0x5, 0x6, 0x7  // Y
            );
            return AdvSimd.Arm64.VectorTableLookup(value.AsSByte(), indices).AsSingle();
        }
        else
        {
            return Vector128.Create(
                value.GetElement(2),
                value.GetElement(0),
                value.GetElement(3),
                value.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>Y</c>, <c>X</c>, and <c>W</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZYXW(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b11_00_01_10);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b11_00_01_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var indices = Vector128.Create(
                0x8, 0x9, 0xA, 0xB, // Z
                0x4, 0x5, 0x6, 0x7, // Y
                0x0, 0x1, 0x2, 0x3, // X
                0xC, 0xD, 0xE, 0xF  // W
            );
            return AdvSimd.Arm64.VectorTableLookup(value.AsSByte(), indices).AsSingle();
        }
        else
        {
            return Vector128.Create(
                value.GetElement(2),
                value.GetElement(1),
                value.GetElement(0),
                value.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>Y</c>, <c>Z</c>, and <c>W</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZYZW(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b11_10_01_10);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b11_10_01_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.InsertSelectedScalar(value, 0, value, 2);
        }
        else
        {
            return Vector128.Create(
                value.GetElement(2),
                value.GetElement(1),
                value.GetElement(2),
                value.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>Z</c>, <c>Y</c>, and <c>Y</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZZYY(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b01_01_10_10);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b01_01_10_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.DuplicateSelectedScalarToVector64(value, 2),
                AdvSimd.DuplicateSelectedScalarToVector64(value, 1)
            );
        }
        else
        {
            return Vector128.Create(
                value.GetElement(2),
                value.GetElement(2),
                value.GetElement(1),
                value.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>Z</c>, <c>Y</c>, and <c>W</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZZYW(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b11_01_10_10);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b11_01_10_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var indices = Vector128.Create(
                0x8, 0x9, 0xA, 0xB, // Z
                0x8, 0x9, 0xA, 0xB, // Z
                0x4, 0x5, 0x6, 0x7, // Y
                0xC, 0xD, 0xE, 0xF  // W
            );
            return AdvSimd.Arm64.VectorTableLookup(value.AsSByte(), indices).AsSingle();
        }
        else
        {
            return Vector128.Create(
                value.GetElement(2),
                value.GetElement(2),
                value.GetElement(1),
                value.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>W</c>, <c>X</c>, and <c>Y</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZWXY(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b01_00_11_10);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b01_00_11_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(value.GetUpper(), value.GetLower());
        }
        else
        {
            return Vector128.Create(
                value.GetElement(2),
                value.GetElement(3),
                value.GetElement(0),
                value.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>W</c>, <c>Y</c>, and <c>Z</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZWYZ(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b10_01_11_10);
        }
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b10_01_11_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var upper = value.GetUpper();
            return Vector128.Create(
                upper,
                AdvSimd.ExtractVector64(upper, value.GetLower(), 1)
            );
        }
        else
        {
            return Vector128.Create(
                value.GetElement(2),
                value.GetElement(3),
                value.GetElement(1),
                value.GetElement(2)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>W</c>, <c>Z</c>, and <c>W</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZWZW(Vector128<float> value)
    {
        if (Sse41.IsSupported)
        {
            return Sse.MoveHighToLow(value, value);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var upper = value.GetUpper();
            return Vector128.Create(upper, upper);
        }
        else
        {
            return Vector128.Create(
                value.GetElement(2),
                value.GetElement(3),
                value.GetElement(2),
                value.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>W</c>, <c>X</c>, <c>Y</c>, and <c>Z</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromWXYZ(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b10_01_00_11);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b10_01_00_11);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.ExtractVector128(value, value, 3);
        }
        else
        {
            return Vector128.Create(
                value.GetElement(3),
                value.GetElement(0),
                value.GetElement(1),
                value.GetElement(2)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>W</c>, <c>X</c>, <c>W</c>, and <c>X</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromWXWX(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b00_11_00_11);
        }
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b00_11_00_11);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var indices = Vector128.Create(
                0xC, 0xD, 0xE, 0xF, // W
                0x0, 0x1, 0x2, 0x3, // X
                0xC, 0xD, 0xE, 0xF, // W
                0x0, 0x1, 0x2, 0x3  // X
            );
            return AdvSimd.Arm64.VectorTableLookup(value.AsSByte(), indices).AsSingle();
        }
        else
        {
            return Vector128.Create(
                value.GetElement(3),
                value.GetElement(0),
                value.GetElement(3),
                value.GetElement(0)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>W</c>, <c>Z</c>, <c>Y</c>, and <c>X</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromWZYX(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b00_01_10_11);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b00_01_10_11);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.ReverseElement32(value.AsUInt64()).AsSingle();
            return Vector128.Create(result.GetUpper(), result.GetLower());
        }
        else
        {
            return Vector128.Create(
                value.GetElement(3),
                value.GetElement(2),
                value.GetElement(1),
                value.GetElement(0)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>W</c>, <c>Z</c>, <c>W</c>, and <c>Y</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromWZWY(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b01_11_10_11);
        }
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b01_11_10_11);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var indices = Vector128.Create(
                0xC, 0xD, 0xE, 0xF, // W
                0x8, 0x9, 0xA, 0xB, // Z
                0xC, 0xD, 0xE, 0xF, // W
                0x4, 0x5, 0x6, 0x7  // Y
            );
            return AdvSimd.Arm64.VectorTableLookup(value.AsSByte(), indices).AsSingle();
        }
        else
        {
            return Vector128.Create(
                value.GetElement(3),
                value.GetElement(2),
                value.GetElement(3),
                value.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>W</c>, <c>W</c>, <c>W</c>, and <c>Z</c> components of the input vector.</summary>
    /// <param name="value">The vector whose components is used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromWWWZ(Vector128<float> value)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute(value, 0b10_11_11_11);
        }
        else if (Sse41.IsSupported)
        {
            return Sse.Shuffle(value, value, 0b10_11_11_11);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.DuplicateSelectedScalarToVector128(value, 3);
            return AdvSimd.Arm64.InsertSelectedScalar(result, 3, value, 2);
        }
        else
        {
            return Vector128.Create(
                value.GetElement(3),
                value.GetElement(3),
                value.GetElement(3),
                value.GetElement(2)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>B</c>, <c>Y</c>, <c>B</c>, and <c>B</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromBYBB(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse41.BlendVariable(
                CreateFromY(upper),
                CreateFromY(lower),
                Vector128.Create(0x00000000, 0xFFFFFFFF, 0x00000000, 0x00000000).AsSingle()
            );
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.DuplicateSelectedScalarToVector128(upper, 1);
            return AdvSimd.Arm64.InsertSelectedScalar(result, 1, lower, 1);
        }
        else
        {
            return Vector128.Create(
                upper.GetElement(1),
                lower.GetElement(1),
                upper.GetElement(1),
                upper.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>C</c>, <c>C</c>, <c>C</c>, and <c>C</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromCCZC(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse41.BlendVariable(
                CreateFromZ(upper),
                CreateFromZ(lower),
                Vector128.Create(0x00000000, 0x00000000, 0xFFFFFFFF, 0x00000000).AsSingle()
            );
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.DuplicateSelectedScalarToVector128(upper, 2);
            return AdvSimd.Arm64.InsertSelectedScalar(result, 2, lower, 2);
        }
        else
        {
            return Vector128.Create(
                upper.GetElement(2),
                upper.GetElement(2),
                lower.GetElement(2),
                upper.GetElement(2)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>A</c>, <c>A</c>, and <c>A</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXAAA(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse41.BlendVariable(
                CreateFromX(upper),
                CreateFromX(lower),
                Vector128.Create(0xFFFFFFFF, 0x00000000, 0x00000000, 0x00000000).AsSingle()
            );
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.DuplicateSelectedScalarToVector128(upper, 0);
            return AdvSimd.Arm64.InsertSelectedScalar(result, 0, lower, 0);
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(0),
                upper.GetElement(0),
                upper.GetElement(0),
                upper.GetElement(0)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>X</c>, <c>A</c>, and <c>B</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXXAB(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b01_00_00_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.DuplicateSelectedScalarToVector64(lower, 0),
                upper.GetLower()
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(0),
                lower.GetElement(0),
                upper.GetElement(0),
                upper.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>X</c>, <c>C</c>, and <c>C</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXXCC(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b10_10_00_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.DuplicateSelectedScalarToVector64(lower, 0),
                AdvSimd.DuplicateSelectedScalarToVector64(upper, 2)
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(0),
                lower.GetElement(0),
                upper.GetElement(2),
                upper.GetElement(2)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>Y</c>, <c>A</c>, and <c>A</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXYAA(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b00_00_01_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                lower.GetLower(),
                AdvSimd.DuplicateSelectedScalarToVector64(upper, 0) 
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(0),
                lower.GetElement(1),
                upper.GetElement(0),
                upper.GetElement(0)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>Y</c>, <c>A</c>, and <c>C</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXYAC(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b10_00_01_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                lower.GetLower(),
                AdvSimd.Arm64.InsertSelectedScalar(upper.GetLower(), 1, upper, 2)
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(0),
                lower.GetElement(1),
                upper.GetElement(0),
                upper.GetElement(2)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>Y</c>, <c>C</c>, and <c>C</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXYCC(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b10_10_01_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                lower.GetLower(),
                AdvSimd.DuplicateSelectedScalarToVector64(upper, 2)
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(0),
                lower.GetElement(1),
                upper.GetElement(2),
                upper.GetElement(2)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>Y</c>, <c>C</c>, and <c>D</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXYCD(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b11_10_01_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(lower.GetLower(), upper.GetUpper());
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(0),
                lower.GetElement(1),
                upper.GetElement(2),
                upper.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>Z</c>, <c>A</c>, and <c>C</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXZAC(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b10_00_10_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.UnzipEven(lower, upper);
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(0),
                lower.GetElement(2),
                upper.GetElement(0),
                upper.GetElement(2)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>Z</c>, <c>B</c>, and <c>D</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXZBD(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b11_01_10_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.Arm64.UnzipEven(lower, lower).GetLower(),
                AdvSimd.Arm64.UnzipOdd(upper, upper).GetUpper()
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(0),
                lower.GetElement(2),
                upper.GetElement(1),
                upper.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>W</c>, <c>A</c>, and <c>B</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXWAB(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b01_00_11_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = Vector128.Create(
                lower.GetLower(),
                upper.GetLower()
            );
            return AdvSimd.Arm64.InsertSelectedScalar(result, 1, lower, 3);
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(0),
                lower.GetElement(3),
                upper.GetElement(0),
                upper.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>W</c>, <c>A</c>, and <c>D</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXWAD(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b11_00_11_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.Arm64.InsertSelectedScalar(lower.GetLower(), 1, lower, 3),
                AdvSimd.Arm64.InsertSelectedScalar(upper.GetLower(), 1, upper, 3)
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(0),
                lower.GetElement(3),
                upper.GetElement(0),
                upper.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>X</c>, <c>W</c>, <c>C</c>, and <c>D</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromXWCD(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b11_10_11_00);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = Vector128.Create(
                lower.GetLower(),
                upper.GetUpper()
            );
            return AdvSimd.Arm64.InsertSelectedScalar(result, 1, lower, 3);
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(0),
                lower.GetElement(3),
                upper.GetElement(2),
                upper.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>X</c>, <c>A</c>, and <c>A</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYXAA(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b00_00_00_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.ReverseElement32(lower.GetLower().AsInt64()).AsSingle(),
                AdvSimd.DuplicateSelectedScalarToVector64(upper, 0)
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(1),
                lower.GetElement(0),
                upper.GetElement(0),
                upper.GetElement(0)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>Z</c>, <c>A</c>, and <c>B</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYZAB(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b01_00_10_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.ExtractVector64(lower.GetUpper(), lower.GetLower(), 1),
                upper.GetLower()
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(1),
                lower.GetElement(2),
                upper.GetElement(0),
                upper.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>Z</c>, <c>B</c>, and <c>C</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYZBC(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b10_01_10_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.ExtractVector64(lower.GetUpper(), lower.GetLower(), 1),
                AdvSimd.ExtractVector64(upper.GetUpper(), upper.GetLower(), 1)
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(1),
                lower.GetElement(2),
                upper.GetElement(1),
                upper.GetElement(2)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>Z</c>, <c>C</c>, and <c>B</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYZCB(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b01_10_10_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.ExtractVector64(lower.GetUpper(), lower.GetLower(), 1),
                AdvSimd.ReverseElement32(AdvSimd.ExtractVector64(upper.GetUpper(), upper.GetLower(), 1).AsInt64()).AsSingle()
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(1),
                lower.GetElement(2),
                upper.GetElement(2),
                upper.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>W</c>, <c>B</c>, and <c>D</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYWBB(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b01_01_11_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.Arm64.UnzipOdd(lower, upper);
            return AdvSimd.Arm64.InsertSelectedScalar(result, 3, upper, 1);
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(1),
                lower.GetElement(3),
                upper.GetElement(1),
                upper.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>W</c>, <c>B</c>, and <c>D</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYWBD(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b11_01_11_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.UnzipOdd(lower, upper);
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(1),
                lower.GetElement(3),
                upper.GetElement(1),
                upper.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>W</c>, <c>C</c>, and <c>D</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYWCD(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b11_10_11_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.Arm64.UnzipOdd(lower, lower).GetLower(),
                upper.GetUpper()
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(1),
                lower.GetElement(3),
                upper.GetElement(2),
                upper.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Y</c>, <c>W</c>, <c>D</c>, and <c>D</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromYWDD(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b11_11_11_01);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.Arm64.UnzipOdd(lower, upper);
            return AdvSimd.Arm64.InsertSelectedScalar(result, 2, upper, 3);
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(1),
                lower.GetElement(3),
                upper.GetElement(3),
                upper.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>X</c>, <c>D</c>, and <c>A</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZXDA(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b00_11_00_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.Arm64.UnzipEven(lower, upper);
            result = AdvSimd.ReverseElement32(result.AsInt64()).AsSingle();
            return AdvSimd.Arm64.InsertSelectedScalar(result, 2, upper, 3);
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(2),
                lower.GetElement(0),
                upper.GetElement(3),
                upper.GetElement(0)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>Y</c>, <c>B</c>, and <c>D</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZYBD(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b11_01_01_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.Arm64.InsertSelectedScalar(lower.GetUpper(), 1, lower, 1),
                AdvSimd.Arm64.InsertSelectedScalar(upper.GetUpper(), 0, upper, 1)
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(2),
                lower.GetElement(1),
                upper.GetElement(1),
                upper.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>Y</c>, <c>C</c>, and <c>A</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZYCA(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b00_10_01_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.Arm64.UnzipEven(lower, upper);
            result = AdvSimd.ReverseElement32(result.AsInt64()).AsSingle();
            return AdvSimd.Arm64.InsertSelectedScalar(result, 1, lower, 1);
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(2),
                lower.GetElement(1),
                upper.GetElement(2),
                upper.GetElement(0)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>Z</c>, <c>A</c>, and <c>B</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZZAB(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b01_00_10_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.DuplicateSelectedScalarToVector64(lower, 2),
                upper.GetLower()
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(2),
                lower.GetElement(2),
                upper.GetElement(0),
                upper.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>Z</c>, <c>C</c>, and <c>B</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZZCB(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b01_10_10_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.DuplicateSelectedScalarToVector64(lower, 2),
                AdvSimd.ReverseElement32(AdvSimd.ExtractVector64(upper.GetUpper(), upper.GetLower(), 1).AsInt64()).AsSingle()
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(2),
                lower.GetElement(2),
                upper.GetElement(2),
                upper.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>Z</c>, <c>C</c>, and <c>D</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZZCD(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b11_10_10_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.DuplicateSelectedScalarToVector64(lower, 2),
                upper.GetUpper()
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(2),
                lower.GetElement(2),
                upper.GetElement(2),
                upper.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>W</c>, <c>C</c>, and <c>A</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZWCA(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b00_10_11_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                lower.GetUpper(),
                AdvSimd.Arm64.InsertSelectedScalar(upper.GetUpper(), 1, upper, 0)
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(2),
                lower.GetElement(3),
                upper.GetElement(2),
                upper.GetElement(0)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>Z</c>, <c>W</c>, <c>C</c>, and <c>B</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromZWCB(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b01_10_11_10);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                lower.GetUpper(),
                AdvSimd.Arm64.InsertSelectedScalar(upper.GetUpper(), 1, upper, 1)
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(2),
                lower.GetElement(3),
                upper.GetElement(2),
                upper.GetElement(1)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>W</c>, <c>X</c>, <c>A</c>, and <c>D</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromWXAD(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b11_00_00_11);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.ExtractVector64(lower.GetLower(), lower.GetUpper(), 1),
                AdvSimd.Arm64.InsertSelectedScalar(upper.GetLower(), 1, upper, 3)
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(3),
                lower.GetElement(0),
                upper.GetElement(0),
                upper.GetElement(3)
            );
        }
    }

    /// <summary>Creates a new <see cref="Vector128{Single}" /> instance with elements initialized, in order, to the respective <c>W</c>, <c>X</c>, <c>B</c>, and <c>C</c> components of the input vectors.</summary>
    /// <param name="lower">The vector whose components represent the lower indices used to initialize elements.</param>
    /// <param name="upper">The vector whose components represent the upper indices used to initialize elements.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> with elements initialized to the components of <paramref name="lower" /> and <paramref name="upper" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CreateFromWXBC(Vector128<float> lower, Vector128<float> upper)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Shuffle(lower, upper, 0b10_01_00_11);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return Vector128.Create(
                AdvSimd.ExtractVector64(lower.GetLower(), lower.GetUpper(), 1),
                AdvSimd.ExtractVector64(upper.GetUpper(), upper.GetLower(), 1)
            );
        }
        else
        {
            return Vector128.Create(
                lower.GetElement(3),
                lower.GetElement(0),
                upper.GetElement(1),
                upper.GetElement(2)
            );
        }
    }

    /// <summary>Computes the cross product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
    /// <returns>The cross product of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> CrossProduct(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported || AdvSimd.Arm64.IsSupported)
        {
            var result = Multiply(CreateFromYZXW(left), CreateFromZXYW(right));
            result = MultiplyAddNegated(result, CreateFromZXYW(left), CreateFromYZXW(right));
            return BitwiseAnd(result, Vector128.Create(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0x00000000).AsSingle());
        }
        else
        {
            return Vector128.Create(
                (left.GetElement(1) * right.GetElement(2)) - (left.GetElement(2) * right.GetElement(1)),
                (left.GetElement(2) * right.GetElement(0)) - (left.GetElement(0) * right.GetElement(2)),
                (left.GetElement(0) * right.GetElement(1)) - (left.GetElement(1) * right.GetElement(0)),
                0
            );
        }
    }

    /// <summary>Computes the quotient of a vector and a float.</summary>
    /// <param name="left">The vector which is divied by <paramref name="right" />.</param>
    /// <param name="right">The float which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Divide(Vector128<float> left, float right)
    {
        if (Sse41.IsSupported)
        {
            var scalar = Vector128.Create(right);
            return Sse.Divide(left, scalar);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var scalar = Vector128.Create(right);
            return AdvSimd.Arm64.Divide(left, scalar);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, float right)
        {
            return Vector128.Create(
                left.GetElement(0) / right,
                left.GetElement(1) / right,
                left.GetElement(2) / right,
                left.GetElement(3) / right
            );
        }
    }

    /// <summary>Computes the quotient of two vectors.</summary>
    /// <param name="left">The vector which is divied by <paramref name="right" />.</param>
    /// <param name="right">The vector which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Divide(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Divide(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.Divide(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                left.GetElement(0) / right.GetElement(0),
                left.GetElement(1) / right.GetElement(1),
                left.GetElement(2) / right.GetElement(2),
                left.GetElement(3) / right.GetElement(3)
            );
        }
    }

    /// <summary>Computes the dot product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
    /// <returns>The dot product of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> DotProduct(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse41.DotProduct(left, right, 0xFF);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var tmp = AdvSimd.Multiply(left, right);
            tmp = AdvSimd.Arm64.AddPairwise(tmp, tmp);
            return AdvSimd.Arm64.AddPairwise(tmp, tmp);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            var result = (left.GetElement(0) * right.GetElement(0))
                       + (left.GetElement(1) * right.GetElement(1))
                       + (left.GetElement(2) * right.GetElement(2))
                       + (left.GetElement(3) * right.GetElement(3));
            return Vector128.Create(result);
        }
    }

    /// <summary>Conditionally selects, on an element-wise basis, from two vectors.</summary>
    /// <param name="condition">The vector that specifies the element-wise conditions used to select <paramref name="left" /> or <paramref name="right" />.</param>
    /// <param name="left">The vector to select the element from when the condition represents <c>true</c>.</param>
    /// <param name="right">The vector to select the element from when the condition represents <c>false</c>.</param>
    /// <returns>A new <see cref="Vector128{Single}" /> made up of elements from <paramref name="left" /> or <paramref name="right" /> based on <paramref name="condition" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> ElementwiseSelect(Vector128<float> condition, Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse41.BlendVariable(right, left, condition);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.BitwiseSelect(condition, left, right);
        }
        else
        {
            var result = SoftwareFallback(condition.AsUInt32(), left.AsUInt32(), right.AsUInt32());
            return result.AsSingle();
        }

        static Vector128<uint> SoftwareFallback(Vector128<uint> condition, Vector128<uint> left, Vector128<uint> right)
        {
            return Vector128.Create(
                (condition.GetElement(0) != 0) ? left.GetElement(0) : right.GetElement(0),
                (condition.GetElement(1) != 0) ? left.GetElement(1) : right.GetElement(1),
                (condition.GetElement(2) != 0) ? left.GetElement(2) : right.GetElement(2),
                (condition.GetElement(3) != 0) ? left.GetElement(3) : right.GetElement(3)
            );
        }
    }

    /// <summary>Interleaves the lower elements of two vectors.</summary>
    /// <param name="left">The vector to interleave with <paramref name="right" />.</param>
    /// <param name="right">The vector to interleave with <paramref name="left" />.</param>
    /// <returns>A vector that is made up of the interleaved lower elements of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> InterleaveLower(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.UnpackLow(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.ZipLow(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                left.GetElement(0),
                right.GetElement(0),
                left.GetElement(1),
                right.GetElement(1)
            );
        }
    }

    /// <summary>Interleaves the upper elements of two vectors.</summary>
    /// <param name="left">The vector to interleave with <paramref name="right" />.</param>
    /// <param name="right">The vector to interleave with <paramref name="left" />.</param>
    /// <returns>A vector that is made up of the interleaved upper elements of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> InterleaveUpper(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.UnpackHigh(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.ZipHigh(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                left.GetElement(2),
                right.GetElement(2),
                left.GetElement(3),
                right.GetElement(3)
            );
        }
    }

    /// <summary>Determines if any elements in a vector are either <see cref="float.PositiveInfinity" /> or <see cref="float.NegativeInfinity" />.</summary>
    /// <param name="value">The vector to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are either <see cref="float.PositiveInfinity" /> or <see cref="float.NegativeInfinity" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAnyInfinity(Vector128<float> value)
    {
        if (Sse41.IsSupported)
        {
            var result = Sse.And(value, Vector128.Create(0x7FFFFFFF).AsSingle());
            result = Sse.CompareEqual(result, Vector128.Create(float.PositiveInfinity));
            return Sse.MoveMask(result) != 0;
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.Abs(value);
            result = AdvSimd.CompareEqual(result, Vector128.Create(float.PositiveInfinity));
            return AdvSimd.Arm64.MaxAcross(result).ToScalar() != 0;
        }
        else
        {
            return SoftwareFallback(value.AsUInt32());
        }

        static bool SoftwareFallback(Vector128<uint> value)
        {
            return float.IsInfinity(value.GetElement(0))
                || float.IsInfinity(value.GetElement(1))
                || float.IsInfinity(value.GetElement(2))
                || float.IsInfinity(value.GetElement(3));
        }
    }

    /// <summary>Determines if any elements in a vector are <see cref="float.NaN" />.</summary>
    /// <param name="value">The vector to check.</param>
    /// <returns><c>true</c> if any elements in <paramref name="value" /> are <see cref="float.NaN" />; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAnyNaN(Vector128<float> value) => CompareNotEqualAny(value, value);

    /// <summary>Computes the length of a vector.</summary>
    /// <param name="value">The vector whose length is to be computed.</param>
    /// <returns>The length of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Length(Vector128<float> value)
    {
        var result = LengthSquared(value);
        return Sqrt(result);
    }

    /// <summary>Computes the squared length of a vector.</summary>
    /// <param name="value">The vector whose squared length is to be computed.</param>
    /// <returns>The squared length of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> LengthSquared(Vector128<float> value) => DotProduct(value, value);

    /// <summary>Compares two vectors to determine the element-wise maximum.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>The element-wise maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Max(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            // TODO: This isn't correctly taking +0.0 vs -0.0 into account
            var tmp = Sse.Max(left, right);
            var msk = Sse.CompareUnordered(left, right);
            return Sse41.BlendVariable(tmp, left, msk);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Max(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                MathUtilities.Max(left.GetElement(0), right.GetElement(0)),
                MathUtilities.Max(left.GetElement(1), right.GetElement(1)),
                MathUtilities.Max(left.GetElement(2), right.GetElement(2)),
                MathUtilities.Max(left.GetElement(3), right.GetElement(3))
            );
        }
    }

    /// <summary>Compares two vectors to determine the element-wise minimum.</summary>
    /// <param name="left">The vector to compare with <paramref name="right" />.</param>
    /// <param name="right">The vector to compare with <paramref name="left" />.</param>
    /// <returns>The element-wise minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Min(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            // TODO: This isn't correctly taking +0.0 vs -0.0 into account
            var tmp = Sse.Min(left, right);
            var msk = Sse.CompareUnordered(left, left);
            return Sse41.BlendVariable(tmp, left, msk);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Min(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                MathUtilities.Min(left.GetElement(0), right.GetElement(0)),
                MathUtilities.Min(left.GetElement(1), right.GetElement(1)),
                MathUtilities.Min(left.GetElement(2), right.GetElement(2)),
                MathUtilities.Min(left.GetElement(3), right.GetElement(3))
            );
        }
    }

    /// <summary>Computes the product of a vector and a float.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The float which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Multiply(Vector128<float> left, float right)
    {
        if (Sse41.IsSupported)
        {
            var scalar = Vector128.Create(right);
            return Sse.Multiply(left, scalar);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var scalar = Vector64.CreateScalar(right);
            return AdvSimd.MultiplyBySelectedScalar(left, scalar, 0);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, float right)
        {
            return Vector128.Create(
                left.GetElement(0) * right,
                left.GetElement(1) * right,
                left.GetElement(2) * right,
                left.GetElement(3) * right
            );
        }
    }

    /// <summary>Computes the product of two vectors.</summary>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Multiply(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Multiply(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Multiply(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                left.GetElement(0) * right.GetElement(0),
                left.GetElement(1) * right.GetElement(1),
                left.GetElement(2) * right.GetElement(2),
                left.GetElement(3) * right.GetElement(3)
            );
        }
    }

    /// <summary>Computes the product of two vectors and then adds a third.</summary>
    /// <param name="addend">The vector which is added to the product of <paramref name="left" /> and <paramref name="right" />.</param>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="addend" /> and the product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> MultiplyAdd(Vector128<float> addend, Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            var result = Sse.Multiply(left, right);
            return Sse.Add(addend, result);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.Multiply(left, right);
            return AdvSimd.Add(addend, result);
        }
        else
        {
            return SoftwareFallback(addend, left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> addend, Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                addend.GetElement(0) + (left.GetElement(0) * right.GetElement(0)),
                addend.GetElement(1) + (left.GetElement(1) * right.GetElement(1)),
                addend.GetElement(2) + (left.GetElement(2) * right.GetElement(2)),
                addend.GetElement(3) + (left.GetElement(3) * right.GetElement(3))
            );
        }
    }

    /// <summary>Computes the product of a vector and the x-component of another vector and then adds a third.</summary>
    /// <param name="addend">The vector which is added to the product of <paramref name="left" /> and <paramref name="right" />.</param>
    /// <param name="left">The vector to multiply by the component of <paramref name="right" />.</param>
    /// <param name="right">The vector whose component is used to multiply <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="addend" /> and the product of <paramref name="left" /> multipled by the component of <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> MultiplyAddByX(Vector128<float> addend, Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            var scalar = CreateFromX(right);
            var result = Sse.Multiply(left, scalar);
            return Sse.Add(result, addend);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.MultiplyBySelectedScalar(left, right, 0);
            return AdvSimd.Add(result, addend);
        }
        else
        {
            var scalar = right.GetElement(0);
            return SoftwareFallback(addend, left, scalar);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> addend, Vector128<float> left, float right)
        {
            return Vector128.Create(
                addend.GetElement(0) + (left.GetElement(0) * right),
                addend.GetElement(1) + (left.GetElement(1) * right),
                addend.GetElement(2) + (left.GetElement(2) * right),
                addend.GetElement(3) + (left.GetElement(3) * right)
            );
        }
    }

    /// <summary>Computes the product of a vector and the y-component of another vector and then adds a third.</summary>
    /// <param name="addend">The vector which is added to the product of <paramref name="left" /> and <paramref name="right" />.</param>
    /// <param name="left">The vector to multiply by the component of <paramref name="right" />.</param>
    /// <param name="right">The vector whose component is used to multiply <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="addend" /> and the product of <paramref name="left" /> multipled by the component of <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> MultiplyAddByY(Vector128<float> addend, Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            var scalar = CreateFromY(right);
            var result = Sse.Multiply(left, scalar);
            return Sse.Add(addend, result);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.MultiplyBySelectedScalar(left, right, 1);
            return AdvSimd.Add(addend, result);
        }
        else
        {
            var scalar = right.GetElement(1);
            return SoftwareFallback(addend, left, scalar);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> addend, Vector128<float> left, float right)
        {
            return Vector128.Create(
                addend.GetElement(0) + (left.GetElement(0) * right),
                addend.GetElement(1) + (left.GetElement(1) * right),
                addend.GetElement(2) + (left.GetElement(2) * right),
                addend.GetElement(3) + (left.GetElement(3) * right)
            );
        }
    }

    /// <summary>Computes the product of a vector and the z-component of another vector and then adds a third.</summary>
    /// <param name="addend">The vector which is added to the product of <paramref name="left" /> and <paramref name="right" />.</param>
    /// <param name="left">The vector to multiply by the component of <paramref name="right" />.</param>
    /// <param name="right">The vector whose component is used to multiply <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="addend" /> and the product of <paramref name="left" /> multipled by the component of <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> MultiplyAddByZ(Vector128<float> addend, Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            var scalar = CreateFromZ(right);
            var result = Sse.Multiply(left, scalar);
            return Sse.Add(result, addend);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.MultiplyBySelectedScalar(left, right, 2);
            return AdvSimd.Add(result, addend);
        }
        else
        {
            var scalar = right.GetElement(2);
            return SoftwareFallback(addend, left, scalar);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> addend, Vector128<float> left, float right)
        {
            return Vector128.Create(
                addend.GetElement(0) + (left.GetElement(0) * right),
                addend.GetElement(1) + (left.GetElement(1) * right),
                addend.GetElement(2) + (left.GetElement(2) * right),
                addend.GetElement(3) + (left.GetElement(3) * right)
            );
        }
    }

    /// <summary>Computes the product of a vector and the w-component of another vector and then adds a third.</summary>
    /// <param name="addend">The vector which is added to the product of <paramref name="left" /> and <paramref name="right" />.</param>
    /// <param name="left">The vector to multiply by the component of <paramref name="right" />.</param>
    /// <param name="right">The vector whose component is used to multiply <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="addend" /> and the product of <paramref name="left" /> multipled by the component of <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> MultiplyAddByW(Vector128<float> addend, Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            var scalar = CreateFromW(right);
            var result = Sse.Multiply(left, scalar);
            return Sse.Add(addend, result);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.MultiplyBySelectedScalar(left, right, 3);
            return AdvSimd.Add(addend, result);
        }
        else
        {
            var scalar = right.GetElement(3);
            return SoftwareFallback(addend, left, scalar);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> addend, Vector128< float> left, float right)
        {
            return Vector128.Create(
                addend.GetElement(0) + (left.GetElement(0) * right),
                addend.GetElement(1) + (left.GetElement(1) * right),
                addend.GetElement(2) + (left.GetElement(2) * right),
                addend.GetElement(3) + (left.GetElement(3) * right)
            );
        }
    }

    /// <summary>Computes the negated product of two vectors and then adds a third.</summary>
    /// <param name="addend">The vector which is added to the negated product of <paramref name="left" /> and <paramref name="right" />.</param>
    /// <param name="left">The vector to multiply by <paramref name="right" />.</param>
    /// <param name="right">The vector which is used to multiply <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="addend" /> and the product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> MultiplyAddNegated(Vector128<float> addend, Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            var result = Sse.Multiply(left, right);
            return Sse.Subtract(addend, result);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            var result = AdvSimd.Multiply(left, right);
            return AdvSimd.Subtract(addend, result);
        }
        else
        {
            return SoftwareFallback(addend, left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> minuend, Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                minuend.GetElement(0) - (left.GetElement(0) * right.GetElement(0)),
                minuend.GetElement(1) - (left.GetElement(1) * right.GetElement(1)),
                minuend.GetElement(2) - (left.GetElement(2) * right.GetElement(2)),
                minuend.GetElement(3) - (left.GetElement(3) * right.GetElement(3))
            );
        }
    }

    /// <summary>Computes the product of a vector and the x-component of another vector.</summary>
    /// <param name="left">The vector to multiply by the component of <paramref name="right" />.</param>
    /// <param name="right">The vector whose component is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multiplied by the component of <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> MultiplyByX(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            var scalar = CreateFromX(right);
            return Sse.Multiply(left, scalar);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.MultiplyBySelectedScalar(left, right, 0);
        }
        else
        {
            var scalar = right.GetElement(0);
            return SoftwareFallback(left, scalar);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, float right)
        {
            return Vector128.Create(
                left.GetElement(0) * right,
                left.GetElement(1) * right,
                left.GetElement(2) * right,
                left.GetElement(3) * right
            );
        }
    }

    /// <summary>Computes the product of a vector and the y-component of another vector.</summary>
    /// <param name="left">The vector to multiply by the component of <paramref name="right" />.</param>
    /// <param name="right">The vector whose component is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multiplied by the component of <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> MultiplyByY(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            var scalar = CreateFromY(right);
            return Sse.Multiply(left, scalar);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.MultiplyBySelectedScalar(left, right, 1);
        }
        else
        {
            var scalar = right.GetElement(1);
            return SoftwareFallback(left, scalar);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, float right)
        {
            return Vector128.Create(
                left.GetElement(0) * right,
                left.GetElement(1) * right,
                left.GetElement(2) * right,
                left.GetElement(3) * right
            );
        }
    }

    /// <summary>Computes the product of a vector and the z-component of another vector.</summary>
    /// <param name="left">The vector to multiply by the component of <paramref name="right" />.</param>
    /// <param name="right">The vector whose component is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multiplied by the component of <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> MultiplyByZ(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            var scalar = CreateFromZ(right);
            return Sse.Multiply(left, scalar);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.MultiplyBySelectedScalar(left, right, 2);
        }
        else
        {
            var scalar = right.GetElement(2);
            return SoftwareFallback(left, scalar);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, float right)
        {
            return Vector128.Create(
                left.GetElement(0) * right,
                left.GetElement(1) * right,
                left.GetElement(2) * right,
                left.GetElement(3) * right
            );
        }
    }

    /// <summary>Computes the product of a vector and the w-component of another vector.</summary>
    /// <param name="left">The vector to multiply by the component of <paramref name="right" />.</param>
    /// <param name="right">The vector whose component is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multiplied by the component of <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> MultiplyByW(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            var scalar = CreateFromW(right);
            return Sse.Multiply(left, scalar);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.MultiplyBySelectedScalar(left, right, 3);
        }
        else
        {
            var scalar = right.GetElement(3);
            return SoftwareFallback(left, scalar);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, float right)
        {
            return Vector128.Create(
                left.GetElement(0) * right,
                left.GetElement(1) * right,
                left.GetElement(2) * right,
                left.GetElement(3) * right
            );
        }
    }

    /// <summary>Computes the negation of a vector.</summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negation of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Negate(Vector128<float> value)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Subtract(Vector128<float>.Zero, value);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Negate(value);
        }
        else
        {
            return SoftwareFallback(value);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> value)
        {
            return Vector128.Create(
                -value.GetElement(0),
                -value.GetElement(1),
                -value.GetElement(2),
                -value.GetElement(3)
            );
        }
    }

    /// <summary>Computes the normalized form of a vector.</summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>The normalized form of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Normalize(Vector128<float> value)
    {
        var length = Length(value);
        var mask = CompareEqual(length, Vector128<float>.Zero);

        var result = Divide(value, length);
        return ElementwiseSelect(mask, Vector128<float>.Zero, result);
    }

    /// <summary>Computes an estimate of the normalized form of a vector.</summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>An estimate of the normalized form of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> NormalizeEstimate(Vector128<float> value)
    {
        var result = ReciprocalLengthEstimate(value);
        return Multiply(value, result);
    }

    /// <summary>Computes the conjugate of a quaternion.</summary>
    /// <param name="value">The quaternion for which to get its conjugate.</param>
    /// <returns>The conjugate of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> QuaternionConjugate(Vector128<float> value)
    {
        if (Sse41.IsSupported || AdvSimd.Arm64.IsSupported)
        {
            var multiplier = Vector128.Create(-1.0f, -1.0f, -1.0f, 1.0f);
            return Multiply(value, multiplier);
        }
        else
        {
            return Vector128.Create(
                -value.GetElement(0),
                -value.GetElement(1),
                -value.GetElement(2),
                +value.GetElement(3)
            );
        }
    }

    /// <summary>Computes an estimate of the reciprocal of a vector.</summary>
    /// <param name="value">The vector for which to compute the reciprocal.</param>
    /// <returns>An estimate of the reciprocal of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> ReciprocalEstimate(Vector128<float> value)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Reciprocal(value);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.ReciprocalEstimate(value);
        }
        else
        {
            return SoftwareFallback(value);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> value)
        {
            return Vector128.Create(
                MathUtilities.ReciprocalEstimate(value.GetElement(0)),
                MathUtilities.ReciprocalEstimate(value.GetElement(1)),
                MathUtilities.ReciprocalEstimate(value.GetElement(2)),
                MathUtilities.ReciprocalEstimate(value.GetElement(3))
            );
        }
    }

    /// <summary>Computes an estimate of the reciprocal square-root of a vector.</summary>
    /// <param name="value">The vector for which to compute the square-root.</param>
    /// <returns>An estimate of the element-wise reciprocal square-root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> ReciprocalSqrtEstimate(Vector128<float> value)
    {
        if (Sse41.IsSupported)
        {
            return Sse.ReciprocalSqrt(value);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.ReciprocalSquareRootEstimate(value);
        }
        else
        {
            return SoftwareFallback(value);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> value)
        {
            return Vector128.Create(
                MathUtilities.ReciprocalSqrtEstimate(value.GetElement(0)),
                MathUtilities.ReciprocalSqrtEstimate(value.GetElement(1)),
                MathUtilities.ReciprocalSqrtEstimate(value.GetElement(2)),
                MathUtilities.ReciprocalSqrtEstimate(value.GetElement(3))
            );
        }
    }

    /// <summary>Computes an estimate of the reciprocal length of a vector.</summary>
    /// <param name="value">The vector whose reciprocal length is to be computed.</param>
    /// <returns>An estimate of the reciprocal length of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> ReciprocalLengthEstimate(Vector128<float> value)
    {
        var result = LengthSquared(value);
        return ReciprocalSqrtEstimate(result);
    }

    /// <summary>Computes the sine and cosine of a vector.</summary>
    /// <param name="value">The vector for which to compute the sine and cosine.</param>
    /// <returns>The element-wise sine and cosine of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (Vector128<float> Sin, Vector128<float> Cos) SinCos(Vector128<float> value)
    {
        return SoftwareFallback(value);

        static (Vector128<float>, Vector128<float>) SoftwareFallback(Vector128<float> value)
        {
            var (sinX, cosX) = MathUtilities.SinCos(value.GetElement(0));
            var (sinY, cosY) = MathUtilities.SinCos(value.GetElement(0));
            var (sinZ, cosZ) = MathUtilities.SinCos(value.GetElement(0));
            var (sinW, cosW) = MathUtilities.SinCos(value.GetElement(0));
            return (Vector128.Create(sinX, sinY, sinZ, sinW), Vector128.Create(cosX, cosY, cosZ, cosW));
        }
    }

    /// <summary>Computes the square-root of a vector.</summary>
    /// <param name="value">The vector for which to compute the square-root.</param>
    /// <returns>The element-wise square-root of <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Sqrt(Vector128<float> value)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Sqrt(value);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.Sqrt(value);
        }
        else
        {
            return SoftwareFallback(value);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> value)
        {
            return Vector128.Create(
                MathUtilities.Sqrt(value.GetElement(0)),
                MathUtilities.Sqrt(value.GetElement(1)),
                MathUtilities.Sqrt(value.GetElement(2)),
                MathUtilities.Sqrt(value.GetElement(3))
            );
        }
    }

    /// <summary>Computes the difference of two vectors.</summary>
    /// <param name="left">The vector from which to subtract <paramref name="right" />.</param>
    /// <param name="right">The vector which is subtracted from <paramref name="left" />.</param>
    /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<float> Subtract(Vector128<float> left, Vector128<float> right)
    {
        if (Sse41.IsSupported)
        {
            return Sse.Subtract(left, right);
        }
        else if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Subtract(left, right);
        }
        else
        {
            return SoftwareFallback(left, right);
        }

        static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                left.GetElement(0) - right.GetElement(0),
                left.GetElement(1) - right.GetElement(1),
                left.GetElement(2) - right.GetElement(2),
                left.GetElement(3) - right.GetElement(3)
            );
        }
    }
}
