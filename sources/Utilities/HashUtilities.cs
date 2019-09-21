// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.IntegerUtilities;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods related to computing hashcodes.</summary>
    public static unsafe class HashUtilities
    {
        /// <summary>Combines a value with a seed to produce an unfinalized hashcode.</summary>
        /// <param name="value">The value that will be combined with <paramref name="seed" />.</param>
        /// <param name="seed">The seed that will be combined with <paramref name="value" />.</param>
        /// <returns>An unfinalized hashcode resulting from the combination of <paramref name="value" /> and <paramref name="seed" />.</returns>
        /// <remarks>The output returned by this method is meant to be used as the <c>seed</c> in subsequent calls to <see cref="CombineValue(int, int)" /> or as the <c>value</c> in a call to <see cref="FinalizeValue(int, int)" />.</remarks>
        public static int CombineValue(int value, int seed)
        {
            var combinedHashCode = CombinePartialValue(value, seed);

            unchecked
            {
                combinedHashCode = RotateLeft(combinedHashCode, 13);
                combinedHashCode *= 5;
                combinedHashCode -= 430675100;
            }

            return combinedHashCode;
        }

        /// <summary>Combines a partial value with a seed to produce an unfinalized hashcode.</summary>
        /// <param name="partialValue">The partial value that will be combined with <paramref name="seed" />.</param>
        /// <param name="seed">The seed that will be combined with <paramref name="partialValue" />.</param>
        /// <returns>An unfinalized hashcode resulting from the combination of <paramref name="partialValue" /> and <paramref name="seed" />.</returns>
        /// <remarks>
        ///   <para>The output returned by this method is meant to be used as a value in the call to <see cref="FinalizeValue(int, int)" />.</para>
        ///   <para>This method is only meant to be used when there are less than four-bytes remaining in the value to be combined; otherwise, a call to <see cref="CombineValue(int, int)" /> should be made instead.</para>
        /// </remarks>
        public static int CombinePartialValue(int partialValue, int seed)
        {
            var combinedHashCode = partialValue;

            unchecked
            {
                combinedHashCode *= -862048943;
                combinedHashCode = RotateLeft(combinedHashCode, 15);

                combinedHashCode *= 461845907;
                combinedHashCode ^= seed;
            }

            return combinedHashCode;
        }

        /// <summary>Computes the hashcode for a <see cref="byte" /> array.</summary>
        /// <param name="values">The <see cref="byte" />s to be hashed.</param>
        /// <param name="seed">The seed used as the basis for the computed hashcode.</param>
        /// <returns>The hashcode of all elements in <paramref name="values" />, starting with the initial <paramref name="seed" />.</returns>
        public static int ComputeHashCode(ReadOnlySpan<byte> values, int seed)
        {
            var combinedHashCode = seed;
            var count = values.Length;

            if (count != 0)
            {
                var blocks = MemoryMarshal.Cast<byte, int>(values);

                for (var blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
                {
                    var value = blocks[blockIndex];
                    combinedHashCode = CombineValue(value, combinedHashCode);
                }

                var partialValue = 0;
                var index = blocks.Length * sizeof(int);
                var remainingByteCount = count - index;

                switch (remainingByteCount)
                {
                    case 3:
                    {
                        partialValue ^= values[index + 2] << 16;
                        goto case 2;
                    }

                    case 2:
                    {

                        partialValue ^= values[index + 1] << 8;
                        goto case 1;
                    }

                    case 1:
                    {
                        partialValue ^= values[index];
                        combinedHashCode = CombinePartialValue(partialValue, combinedHashCode);
                        break;
                    }

                    default:
                    {
                        Assert(remainingByteCount == 0, Resources.ArgumentExceptionForInvalidTypeMessage, nameof(remainingByteCount), remainingByteCount);
                        break;
                    }
                }
            }

            return FinalizeValue(combinedHashCode, count);
        }

        /// <summary>Computes the hashcode for a <see cref="int" /> array.</summary>
        /// <param name="values">The <see cref="int" />s to be hashed.</param>
        /// <param name="seed">The seed used as the basis for the computed hashcode.</param>
        /// <returns>The hashcode of all elements in <paramref name="values" />, starting with the initial <paramref name="seed" />.</returns>
        /// <returns>The hashcode of all bytes in <paramref name="values" />, seeded with the initial value in <paramref name="seed" />.</returns>
        public static int ComputeHashCode(ReadOnlySpan<int> values, int seed)
        {
            var combinedHashCode = seed;
            var count = values.Length;

            for (var index = 0; index < count; index++)
            {
                var value = values[index];
                combinedHashCode = CombineValue(value, combinedHashCode);
            }

            return FinalizeValue(combinedHashCode, count * sizeof(int));
        }

        /// <summary>Finalizes a value to produce a hashcode.</summary>
        /// <param name="combinedValue">The combined value that will be finalized.</param>
        /// <param name="bytesCombined">The total number of bytes that were combined to produce <paramref name="combinedValue" />.</param>
        /// <returns>The finalized hashcode for <paramref name="combinedValue" /> and <paramref name="bytesCombined" />.</returns>
        public static int FinalizeValue(int combinedValue, int bytesCombined)
        {
            var finalizedHashCode = combinedValue;

            unchecked
            {
                finalizedHashCode ^= bytesCombined;
                finalizedHashCode ^= (int)((uint)finalizedHashCode >> 16);
                finalizedHashCode *= -2048144789;

                finalizedHashCode ^= (int)((uint)finalizedHashCode >> 13);
                finalizedHashCode *= -1028477387;
                finalizedHashCode ^= (int)((uint)finalizedHashCode >> 16);
            }

            return finalizedHashCode;
        }

        /// <summary>Finalizes a value to produce a hashcode.</summary>
        /// <param name="combinedValue">The combined value that will be finalized.</param>
        /// <param name="bytesCombined">The total number of bytes that were combined to produce <paramref name="combinedValue" />.</param>
        /// <returns>The finalized hashcode for <paramref name="combinedValue" /> and <paramref name="bytesCombined" />.</returns>
        public static int FinalizeValue(int combinedValue, uint bytesCombined) => FinalizeValue(combinedValue, unchecked((int)bytesCombined));
    }
}
