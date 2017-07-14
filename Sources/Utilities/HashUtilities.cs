// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.IntegerUtilities;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods related to computing hashcodes.</summary>
    unsafe public static class HashUtilities
    {
        #region Static Methods
        /// <summary>Combines a value with a seed to produce an unfinalized hashcode.</summary>
        /// <param name="value">The value that will be combined with <paramref name="seed" />.</param>
        /// <param name="seed">The seed that will be combined with <paramref name="value" />.</param>
        /// <returns>An unfinalized hashcode resulting from the combination of <paramref name="value" /> and <paramref name="seed" />.</returns>
        /// <remarks>The output returned by this method is meant to be used as the <c>seed</c> in subsequent calls to <see cref="CombineValue(int, int)" /> or as the <c>value</c> in a call to <see cref="FinalizeValue(int, int)" />.</remarks>
        [Pure]
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
        [Pure]
        public static int CombinePartialValue(int partialValue, int seed)
        {
            var combinedHashCode = partialValue;

            unchecked
            {
                combinedHashCode *= (-862048943);
                combinedHashCode = RotateLeft(combinedHashCode, 15);

                combinedHashCode *= 461845907;
                combinedHashCode ^= seed;
            }

            return combinedHashCode;
        }

        /// <summary>Computes the hashcode for a <see cref="byte" /> array.</summary>
        /// <param name="values">The <see cref="byte" /> array to be hashed.</param>
        /// <param name="offset">The offset into <paramref name="values" /> at which hashing should begin.</param>
        /// <param name="count">The number of elements in <paramref name="values" /> which should be hashed.</param>
        /// <param name="seed">The seed used as the basis for the computed hashcode.</param>
        /// <returns>The hashcode of all elements in <paramref name="values" />, starting with the initial <paramref name="seed" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset" /> is either negative or greater than the length of <paramref name="values" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> is either negative or would extend beyond the bounds of <paramref name="values" /> when starting from <paramref name="offset" />.</exception>
        public static int ComputeHashCode(byte[] values, int offset, int count, int seed)
        {
            if (values is null)
            {
                ThrowArgumentNullException(nameof(values));
            }
            else if ((offset < 0) || (offset > values.Length))
            {
                ThrowArgumentOutOfRangeException(nameof(offset), offset);
            }
            else if ((count < 0) || ((uint)(offset + count) > (uint)(values.Length)))
            {
                ThrowArgumentOutOfRangeException(nameof(count), count);
            }
            Contract.EndContractBlock();

            var combinedHashCode = seed;

            if (count != 0)
            {
                var blockCount = (count / sizeof(int));
                var remainingByteCount = (count % sizeof(int));

                fixed (byte* pValues = &values[0])
                {
                    var pBlocks = (int*)(pValues + offset);

                    for (var blockIndex = 0; blockIndex < blockCount; blockIndex++)
                    {
                        var value = pBlocks[blockIndex];
                        combinedHashCode = CombineValue(value, combinedHashCode);
                    }
                }

                var partialValue = 0;
                var index = (offset + (blockCount * sizeof(int)));

                switch (remainingByteCount)
                {
                    case 3:
                    {
                        partialValue ^= (values[index + 2] << 16);
                        goto case 2;
                    }

                    case 2:
                    {

                        partialValue ^= (values[index + 1] << 8);
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
                        Debug.Assert((remainingByteCount == 0), string.Format(Resources.ArgumentExceptionForInvalidTypeMessage, nameof(remainingByteCount), remainingByteCount));
                        break;
                    }
                }
            }

            return FinalizeValue(combinedHashCode, count);
        }

        /// <summary>Computes the hashcode for a <see cref="int" /> array.</summary>
        /// <param name="values">The <see cref="int" /> array to be hashed.</param>
        /// <param name="offset">The offset into <paramref name="values" /> at which hashing should begin.</param>
        /// <param name="count">The number of elements in <paramref name="values" /> which should be hashed.</param>
        /// <param name="seed">The seed used as the basis for the computed hashcode.</param>
        /// <returns>The hashcode of all elements in <paramref name="values" />, starting with the initial <paramref name="seed" />.</returns>
        /// <returns>The hashcode of all bytes in <paramref name="values" />, seeded with the initial value in <paramref name="seed" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset" /> is either negative or greater than the length of <paramref name="values" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> is either negative or would extend beyond the bounds of <paramref name="values" /> when starting from <paramref name="offset" />.</exception>
        public static int ComputeHashCode(int[] values, int offset, int count, int seed)
        {
            if (values is null)
            {
                ThrowArgumentNullException(nameof(values));
            }
            else if ((offset < 0) || (offset > values.Length))
            {
                ThrowArgumentOutOfRangeException(nameof(offset), offset);
            }
            else if ((count < 0) || ((uint)(offset + count) > (uint)(values.Length)))
            {
                ThrowArgumentOutOfRangeException(nameof(count), count);
            }
            Contract.EndContractBlock();

            var combinedHashCode = seed;

            for (var index = offset; index < count; index++)
            {
                var value = values[index];
                combinedHashCode = CombineValue(value, combinedHashCode);
            }

            return FinalizeValue(combinedHashCode, (count * sizeof(int)));
        }

        /// <summary>Finalizes a value to produce a hashcode.</summary>
        /// <param name="combinedValue">The combined value that will be finalized.</param>
        /// <param name="bytesCombined">The total number of bytes that were combined to produce <paramref name="combinedValue" />.</param>
        /// <returns>The finalized hashcode for <paramref name="combinedValue" /> and <paramref name="bytesCombined" />.</returns>
        [Pure]
        public static int FinalizeValue(int combinedValue, int bytesCombined)
        {
            var finalizedHashCode = combinedValue;

            unchecked
            {
                finalizedHashCode ^= bytesCombined;
                finalizedHashCode ^= (int)((uint)(finalizedHashCode) >> 16);
                finalizedHashCode *= (-2048144789);

                finalizedHashCode ^= (int)((uint)(finalizedHashCode) >> 13);
                finalizedHashCode *= (-1028477387);
                finalizedHashCode ^= (int)((uint)(finalizedHashCode) >> 16);
            }

            return finalizedHashCode;
        }
        #endregion
    }
}
