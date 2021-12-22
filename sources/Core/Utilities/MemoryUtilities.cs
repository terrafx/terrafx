// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static TerraFX.Interop.Mimalloc.Mimalloc;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Utilities;

/// <summary>Provides a set of methods for interacting with unmanaged memory.</summary>
public static unsafe class MemoryUtilities
{
    /// <summary>Allocates a chunk of unmanaged memory.</summary>
    /// <param name="size">The size, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="size" /> bytes in length.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* Allocate(nuint size, bool zero = false)
    {
        var result = TryAllocate(size, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(size);
        }
        return result;
    }

    /// <summary>Allocates a chunk of unmanaged memory that is aligned.</summary>
    /// <param name="size">The size, in bytes, of the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="size" /> bytes in length and that is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* Allocate(nuint size, nuint alignment, bool zero = false)
    {
        var result = TryAllocate(size, alignment, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(size);
        }
        return result;
    }

    /// <summary>Allocates a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <param name="size">The size, in bytes, of the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of <paramref name="offset" />.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="size" /> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* Allocate(nuint size, nuint alignment, nuint offset, bool zero = false)
    {
        var result = TryAllocate(size, alignment, offset, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(size);
        }
        return result;
    }

    /// <summary>Allocates a chunk of unmanaged memory.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the allocation.</typeparam>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* Allocate<T>(bool zero = false)
        where T : unmanaged
    {
        var result = TryAllocate<T>(zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(SizeOf<T>());
        }
        return result;
    }

    /// <summary>Allocates a chunk of unmanaged memory that is aligned.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the allocation.</typeparam>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and that is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* Allocate<T>(nuint alignment, bool zero = false)
        where T : unmanaged
    {
        var result = TryAllocate<T>(alignment, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(SizeOf<T>());
        }
        return result;
    }

    /// <summary>Allocates a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the allocation.</typeparam>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* Allocate<T>(nuint alignment, nuint offset, bool zero = false)
        where T : unmanaged
    {
        var result = TryAllocate<T>(alignment, offset, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(SizeOf<T>());
        }
        return result;
    }

    /// <summary>Allocates a chunk of unmanaged memory.</summary>
    /// <param name="count">The count of elements contained in the allocation.</param>
    /// <param name="size">The size, in bytes, of the elements in the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="size" /> bytes in length.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* AllocateArray(nuint count, nuint size, bool zero = false)
    {
        var result = TryAllocateArray(count, size, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(count, size);
        }
        return result;
    }

    /// <summary>Allocates a chunk of unmanaged memory that is aligned.</summary>
    /// <param name="count">The count of elements contained in the allocation.</param>
    /// <param name="size">The size, in bytes, of the elements in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="size" /> bytes in length and that is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* AllocateArray(nuint count, nuint size, nuint alignment, bool zero = false)
    {
        var result = TryAllocateArray(count, size, alignment, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(count, size);
        }
        return result;
    }

    /// <summary>Allocates a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <param name="count">The count of elements contained in the allocation.</param>
    /// <param name="size">The size, in bytes, of the elements in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of <paramref name="offset" />.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="size" /> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* AllocateArray(nuint count, nuint size, nuint alignment, nuint offset, bool zero = false)
    {
        var result = TryAllocateArray(count, size, alignment, offset, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(count, size);
        }
        return result;
    }

    /// <summary>Allocates a chunk of unmanaged memory.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in the allocation.</typeparam>
    /// <param name="count">The count of elements contained in the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* AllocateArray<T>(nuint count, bool zero = false)
        where T : unmanaged
    {
        var result = TryAllocateArray<T>(count, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(count, SizeOf<T>());
        }
        return result;
    }

    /// <summary>Allocates a chunk of unmanaged memory that is aligned.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in the allocation.</typeparam>
    /// <param name="count">The count of elements contained in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and that is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* AllocateArray<T>(nuint count, nuint alignment, bool zero = false)
        where T : unmanaged
    {
        var result = TryAllocateArray<T>(count, alignment, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(count, SizeOf<T>());
        }
        return result;
    }

    /// <summary>Allocates a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in the allocation.</typeparam>
    /// <param name="count">The count of elements contained in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* AllocateArray<T>(nuint count, nuint alignment, nuint offset, bool zero = false)
        where T : unmanaged
    {
        var result = TryAllocateArray<T>(count, alignment, offset, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(count, SizeOf<T>());
        }
        return result;
    }

    /// <summary>Clears a destination buffer to zero.</summary>
    /// <param name="destination">The destination buffer to clear.</param>
    /// <param name="size">The size, in bytes, of <paramref name="destination" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c> and <paramref name="size" /> is not <c>zero</c>.</exception>
    public static void Clear(void* destination, nuint size)
    {
        if (size == 0)
        {
            return;
        }
        ThrowIfNull(destination);

        ClearUnsafe(destination, size);
    }

    /// <summary>Clears a destination buffer to zero.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of <paramref name="destination" />.</typeparam>
    /// <param name="destination">The destination buffer to clear.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear<T>(T* destination)
        where T : unmanaged => Clear(destination, SizeOf<T>());

    /// <summary>Clears a destination buffer to zero.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in <paramref name="destination" />.</typeparam>
    /// <param name="destination">The destination buffer to clear.</param>
    /// <param name="count">The count of elements in <paramref name="destination" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c> and <paramref name="count" /> is not <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearArray<T>(T* destination, nuint count)
        where T : unmanaged => Clear(destination, count * SizeOf<T>());

    /// <summary>Clears a destination buffer to zero.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in <paramref name="destination" />.</typeparam>
    /// <param name="destination">The destination buffer to clear.</param>
    /// <param name="count">The count of elements in <paramref name="destination" />.</param>
    /// <remarks>This method is unsafe because it does not validate <paramref name="destination" /> is not <c>null</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearArrayUnsafe<T>(T* destination, nuint count)
        where T : unmanaged => ClearUnsafe(destination, count * SizeOf<T>());

    /// <summary>Clears a destination buffer to zero.</summary>
    /// <param name="destination">The destination buffer to clear.</param>
    /// <param name="size">The size, in bytes, of <paramref name="destination" />.</param>
    /// <remarks>This method is unsafe because it does not validate <paramref name="destination" /> is not <c>null</c> if <paramref name="size" /> is not <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearUnsafe(void* destination, nuint size)
    {
        Assert(AssertionsEnabled && ((destination != null) || (size == 0)));

        if (size <= 32)
        {
            SmallClear(destination, size);
        }
        else
        {
            LargeClear(destination, size);
        }

        static void LargeClear(void* destination, nuint length)
        {
            Assert(AssertionsEnabled && (length > 32));
            var blocks = length / 128;

            length -= blocks * 128;
            Assert(AssertionsEnabled && (length < 128));

            for (nuint block = 0; block < blocks; block++)
            {
                // Bytes 0-31

                WriteUnaligned<Vector128<byte>>(destination, default);
                WriteUnaligned<Vector128<byte>>(destination, 16, default);

                // Bytes 32-63

                WriteUnaligned<Vector128<byte>>(destination, 32, default);
                WriteUnaligned<Vector128<byte>>(destination, 48, default);

                // Bytes 64-95

                WriteUnaligned<Vector128<byte>>(destination, 64, default);
                WriteUnaligned<Vector128<byte>>(destination, 80, default);

                // Bytes 96-127

                WriteUnaligned<Vector128<byte>>(destination, 96, default);
                WriteUnaligned<Vector128<byte>>(destination, 112, default);

                destination = (void*)((nuint)destination + 128);
            }

            blocks = length / 32;
            Assert(AssertionsEnabled && (blocks <= 3));

            length -= blocks * 32;
            Assert(AssertionsEnabled && (length < 32));

            switch (blocks)
            {
                case 3:
                {
                    WriteUnaligned<Vector128<byte>>(destination, 64, default);
                    WriteUnaligned<Vector128<byte>>(destination, 80, default);

                    goto case 2;
                }

                case 2:
                {
                    WriteUnaligned<Vector128<byte>>(destination, 32, default);
                    WriteUnaligned<Vector128<byte>>(destination, 48, default);

                    goto case 1;
                }

                case 1:
                {
                    WriteUnaligned<Vector128<byte>>(destination, default);
                    WriteUnaligned<Vector128<byte>>(destination, 16, default);

                    goto case 0;
                }

                case 0:
                {
                    SmallClear(destination, length);
                    break;
                }
            }
        }

        static void SmallClear(void* destination, nuint length)
        {
            Assert(AssertionsEnabled && (length <= 32));

            switch (length)
            {
                case 32:
                case 31:
                case 30:
                case 29:
                case 28:
                case 27:
                case 26:
                case 25:
                case 24:
                case 23:
                case 22:
                case 21:
                case 20:
                case 19:
                case 18:
                case 17:
                {
                    WriteUnaligned<Vector128<byte>>(destination, default);
                    WriteUnaligned<Vector128<byte>>(destination, length - 16, default);
                    break;
                }

                case 16:
                {
                    WriteUnaligned<Vector128<byte>>(destination, default);
                    break;
                }

                case 15:
                case 14:
                case 13:
                case 12:
                case 11:
                case 10:
                case 09:
                {
                    WriteUnaligned<ulong>(destination, default);
                    WriteUnaligned<ulong>(destination, length - 8, default);
                    break;
                }

                case 8:
                {
                    WriteUnaligned<ulong>(destination, default);
                    break;
                }

                case 7:
                case 6:
                case 5:
                {
                    WriteUnaligned<uint>(destination, default);
                    WriteUnaligned<uint>(destination, length - 4, default);
                    break;
                }

                case 4:
                {
                    WriteUnaligned<uint>(destination, default);
                    break;
                }

                case 3:
                {
                    WriteUnaligned<ushort>(destination, default);
                    WriteUnaligned<ushort>(destination, length - 2, default);
                    break;
                }

                case 2:
                {
                    WriteUnaligned<ushort>(destination, default);
                    break;
                }

                case 1:
                {
                    WriteUnaligned<byte>(destination, default);
                    break;
                }
            }
        }
    }

    /// <summary>Clears a destination buffer to zero.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in <paramref name="destination" />.</typeparam>
    /// <param name="destination">The destination buffer to clear.</param>
    /// <remarks>This method is unsafe because it does not validate <paramref name="destination" /> is not <c>null</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearUnsafe<T>(T* destination)
        where T : unmanaged => ClearUnsafe(destination, SizeOf<T>());

    /// <summary>Copies memory from a source buffer to a destination buffer.</summary>
    /// <param name="destination">The destination buffer to which memory should be copied.</param>
    /// <param name="source">The source buffer from which memory is copied.</param>
    /// <param name="size">The size, in bytes, to be copied.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c> and <paramref name="size" /> is not <c>zero</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c> and <paramref name="size" /> is not <c>zero</c>.</exception>
    public static void Copy(void* destination, void* source, nuint size)
    {
        if (size == 0)
        {
            return;
        }

        ThrowIfNull(source);
        ThrowIfNull(destination);

        CopyUnsafe(destination, source, size);
    }

    /// <summary>Copies memory from a source buffer to a destination buffer.</summary>
    /// <param name="destination">The destination buffer to which memory should be copied.</param>
    /// <param name="destinationLength">The length, in bytes, of <paramref name="destination" />.</param>
    /// <param name="source">The source buffer from which memory is copied.</param>
    /// <param name="sourceLength">The length, in bytes, of <paramref name="source" />.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="sourceLength" /> is greater than <paramref name="destinationLength" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c> and <paramref name="sourceLength" /> is not <c>zero</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c> and <paramref name="destinationLength" /> is not <c>zero</c>.</exception>
    public static void Copy(void* destination, nuint destinationLength, void* source, nuint sourceLength)
    {
        ThrowIfNotInInsertBounds(sourceLength, destinationLength);
        Copy(destination, source, sourceLength);
    }

    /// <summary>Copies memory from a source buffer to a destination buffer.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of <paramref name="source" />.</typeparam>
    /// <param name="destination">The destination buffer to which memory should be copied.</param>
    /// <param name="source">The source buffer from which memory is copied.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy<T>(T* destination, T* source)
        where T : unmanaged => Copy(destination, source, SizeOf<T>());

    /// <summary>Copies memory from a source buffer to a destination buffer.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in <paramref name="source" />.</typeparam>
    /// <param name="destination">The destination buffer to which memory should be copied.</param>
    /// <param name="source">The source buffer from which memory is copied.</param>
    /// <param name="count">The count of elements in <paramref name="source" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c> and <paramref name="count" /> is not <c>zero</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c> and <paramref name="count" /> is not <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyArray<T>(T* destination, T* source, nuint count)
        where T : unmanaged => Copy(destination, source, count * SizeOf<T>());

    /// <summary>Copies memory from a source buffer to a destination buffer.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in <paramref name="source" /> and <paramref name="destination" />.</typeparam>
    /// <param name="destination">The destination buffer to which memory should be copied.</param>
    /// <param name="destinationCount">The count of elements in <paramref name="destination" />.</param>
    /// <param name="source">The source buffer from which memory is copied.</param>
    /// <param name="sourceCount">The count of elements in <paramref name="source" />.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="sourceCount" /> is greater than <paramref name="destinationCount" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c> and <paramref name="sourceCount" /> is not <c>zero</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c> and <paramref name="destinationCount" /> is not <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyArray<T>(T* destination, nuint destinationCount, T* source, nuint sourceCount)
        where T : unmanaged => Copy(destination, destinationCount * SizeOf<T>(), source, sourceCount * SizeOf<T>());

    /// <summary>Copies memory from a source buffer to a destination buffer.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in <paramref name="source" />.</typeparam>
    /// <param name="destination">The destination buffer to which memory should be copied.</param>
    /// <param name="source">The source buffer from which memory is copied.</param>
    /// <param name="count">The count of elements in <paramref name="source" />.</param>
    /// <remarks>This method is unsafe because it does not validate <paramref name="destination" /> and <paramref name="source" /> are not <c>null</c> if <paramref name="count" /> is not <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyArrayUnsafe<T>(T* destination, T* source, nuint count)
        where T : unmanaged => CopyUnsafe(destination, source, count * SizeOf<T>());

    /// <summary>Copies memory from a source buffer to a destination buffer.</summary>
    /// <param name="destination">The destination buffer to which memory should be copied.</param>
    /// <param name="source">The source buffer from which memory is copied.</param>
    /// <param name="length">The length, in bytes, to be copied.</param>
    /// <remarks>This method is unsafe because it does not validate <paramref name="destination" /> and <paramref name="source" /> are not <c>null</c> if <paramref name="length" /> is not <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyUnsafe(void* destination, void* source, nuint length)
    {
        Assert(AssertionsEnabled && ((destination != null) || (length == 0)));
        Assert(AssertionsEnabled && ((source != null) || (length == 0)));
        Assert(AssertionsEnabled && ((destination != source) || (length == 0)));

        if (length <= 32)
        {
            SmallCopy(destination, source, length);
        }
        else if ((source >= destination) || (((nuint)source + length) <= (nuint)destination))
        {
            NonOverlappingCopy(destination, source, length);
        }
        else
        {
            OverlappingCopy(destination, source, length);
        }

        static void NonOverlappingCopy(void* destination, void* source, nuint length)
        {
            Assert(AssertionsEnabled && (length > 32));
            var blocks = length / 128;

            length -= blocks * 128;
            Assert(AssertionsEnabled && (length < 128));

            for (nuint block = 0; block < blocks; block++)
            {
                // Bytes 0-31

                var lower = ReadUnaligned<Vector128<byte>>(source);
                var upper = ReadUnaligned<Vector128<byte>>(source, 16);

                WriteUnaligned(destination, lower);
                WriteUnaligned(destination, 16, upper);

                // Bytes 32-63

                lower = ReadUnaligned<Vector128<byte>>(source, 32);
                upper = ReadUnaligned<Vector128<byte>>(source, 48);

                WriteUnaligned(destination, 32, lower);
                WriteUnaligned(destination, 48, upper);

                // Bytes 64-95

                lower = ReadUnaligned<Vector128<byte>>(source, 64);
                upper = ReadUnaligned<Vector128<byte>>(source, 80);

                WriteUnaligned(destination, 64, lower);
                WriteUnaligned(destination, 80, upper);

                // Bytes 96-127

                lower = ReadUnaligned<Vector128<byte>>(source, 96);
                upper = ReadUnaligned<Vector128<byte>>(source, 112);

                WriteUnaligned(destination, 96, lower);
                WriteUnaligned(destination, 112, upper);

                source = (void*)((nuint)source + 128);
                destination = (void*)((nuint)destination + 128);
            }

            TrailingCopy(destination, source, length);
        }

        static void OverlappingCopy(void* destination, void* source, nuint length)
        {
            Assert(AssertionsEnabled && (source < destination));
            Assert(AssertionsEnabled && (((nuint)source + length) > (nuint)destination));

            source = (void*)((nuint)source + length);
            destination = (void*)((nuint)destination + length);

            // This is basically the same as the non-overlapping copy but does
            // everything in reverse, from back to front. Since we know source
            // is less than destination and that there is some overlap this ensures
            // we are only overwriting bytes that have already been read.

            Assert(AssertionsEnabled && (length > 32));
            var blocks = length / 128;

            length -= blocks * 128;
            Assert(AssertionsEnabled && (length < 128));

            for (nuint block = 0; block < blocks; block++)
            {
                // Bytes 127-96

                var upper = ReadUnaligned<Vector128<byte>>(source, 112);
                var lower = ReadUnaligned<Vector128<byte>>(source, 96);

                WriteUnaligned(destination, 112, upper);
                WriteUnaligned(destination, 96, lower);

                // Bytes 95-64

                upper = ReadUnaligned<Vector128<byte>>(source, 80);
                lower = ReadUnaligned<Vector128<byte>>(source, 64);

                WriteUnaligned(destination, 80, upper);
                WriteUnaligned(destination, 64, lower);

                // Bytes 63-32

                upper = ReadUnaligned<Vector128<byte>>(source, 48);
                lower = ReadUnaligned<Vector128<byte>>(source, 32);

                WriteUnaligned(destination, 48, upper);
                WriteUnaligned(destination, 32, lower);

                // Bytes 31-0

                upper = ReadUnaligned<Vector128<byte>>(source, 16);
                lower = ReadUnaligned<Vector128<byte>>(source);

                WriteUnaligned(destination, 16, upper);
                WriteUnaligned(destination, lower);

                source = (void*)((nuint)source - 128);
                destination = (void*)((nuint)destination - 128);
            }

            TrailingCopy(destination, source, length);
        }

        static void SmallCopy(void* destination, void* source, nuint length)
        {
            Assert(AssertionsEnabled && (length <= 32));

            switch (length)
            {
                case 32:
                case 31:
                case 30:
                case 29:
                case 28:
                case 27:
                case 26:
                case 25:
                case 24:
                case 23:
                case 22:
                case 21:
                case 20:
                case 19:
                case 18:
                case 17:
                {
                    var lower = ReadUnaligned<Vector128<byte>>(source);
                    var upper = ReadUnaligned<Vector128<byte>>(source, length - 16);

                    WriteUnaligned(destination, lower);
                    WriteUnaligned(destination, length - 16, upper);
                    break;
                }

                case 16:
                {
                    var value = ReadUnaligned<Vector128<byte>>(source);
                    WriteUnaligned(destination, value);
                    break;
                }

                case 15:
                case 14:
                case 13:
                case 12:
                case 11:
                case 10:
                case 09:
                {
                    var lower = ReadUnaligned<ulong>(source);
                    var upper = ReadUnaligned<ulong>(source, length - 8);

                    WriteUnaligned(destination, lower);
                    WriteUnaligned(destination, length - 8, upper);
                    break;
                }

                case 8:
                {
                    var value = ReadUnaligned<ulong>(source);
                    WriteUnaligned(destination, value);
                    break;
                }

                case 7:
                case 6:
                case 5:
                {
                    var lower = ReadUnaligned<uint>(source);
                    var upper = ReadUnaligned<uint>(source, length - 4);

                    WriteUnaligned(destination, lower);
                    WriteUnaligned(destination, length - 4, upper);
                    break;
                }

                case 4:
                {
                    var value = ReadUnaligned<uint>(source);
                    WriteUnaligned(destination, value);
                    break;
                }

                case 3:
                {
                    var lower = ReadUnaligned<ushort>(source);
                    var upper = ReadUnaligned<ushort>(source, length - 2);

                    WriteUnaligned(destination, lower);
                    WriteUnaligned(destination, length - 2, upper);
                    break;
                }

                case 2:
                {
                    var value = ReadUnaligned<ushort>(source);
                    WriteUnaligned(destination, value);
                    break;
                }

                case 1:
                {
                    var value = ReadUnaligned<byte>(source);
                    WriteUnaligned(destination, value);
                    break;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void TrailingCopy(void* destination, void* source, nuint length)
        {
            var blocks = length / 32;
            Assert(AssertionsEnabled && (blocks <= 3));

            length -= blocks * 32;
            Assert(AssertionsEnabled && (length < 32));

            switch (blocks)
            {
                case 3:
                {
                    var lower = ReadUnaligned<Vector128<byte>>(source, 64);
                    var upper = ReadUnaligned<Vector128<byte>>(source, 80);

                    WriteUnaligned(destination, 64, lower);
                    WriteUnaligned(destination, 80, upper);

                    goto case 2;
                }

                case 2:
                {
                    var lower = ReadUnaligned<Vector128<byte>>(source, 32);
                    var upper = ReadUnaligned<Vector128<byte>>(source, 48);

                    WriteUnaligned(destination, 32, lower);
                    WriteUnaligned(destination, 48, upper);

                    goto case 1;
                }

                case 1:
                {
                    var lower = ReadUnaligned<Vector128<byte>>(source);
                    var upper = ReadUnaligned<Vector128<byte>>(source, 16);

                    WriteUnaligned(destination, lower);
                    WriteUnaligned(destination, 16, upper);

                    goto case 0;
                }

                case 0:
                {
                    SmallCopy(destination, source, length);
                    break;
                }
            }
        }
    }

    /// <summary>Copies memory from a source buffer to a destination buffer.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of <paramref name="source" />.</typeparam>
    /// <param name="destination">The destination buffer to which memory should be copied.</param>
    /// <param name="source">The source buffer from which memory is copied.</param>
    /// <remarks>This method is unsafe because it does not validate <paramref name="destination" /> and <paramref name="source" /> are not <c>null</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyUnsafe<T>(T* destination, T* source)
        where T : unmanaged => CopyUnsafe(destination, source, SizeOf<T>());

    /// <summary>Frees an allocated chunk of unmanaged memory.</summary>
    /// <param name="address">The address to an allocated chunk of memory to free</param>
    public static void Free(void* address) => mi_free(address);

    /// <summary>Reallocates a chunk of unmanaged memory.</summary>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newSize">The new size, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="newSize" /> bytes in length.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* Reallocate(void* address, nuint newSize, bool zero = false)
    {
        var result = TryReallocate(address, newSize, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(newSize);
        }
        return result;
    }

    /// <summary>Reallocates a chunk of unmanaged memory that is aligned.</summary>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newSize">The new size, in bytes, of the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="newSize" /> bytes in length and that is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* Reallocate(void* address, nuint newSize, nuint alignment, bool zero = false)
    {
        var result = TryReallocate(address, newSize, alignment, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(newSize);
        }
        return result;
    }

    /// <summary>Reallocates a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newSize">The new size, in bytes, of the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of <paramref name="offset" />.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="newSize" /> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* Reallocate(void* address, nuint newSize, nuint alignment, nuint offset, bool zero = false)
    {
        var result = TryReallocate(address, newSize, alignment, offset, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(newSize);
        }
        return result;
    }

    /// <summary>Reallocates a chunk of unmanaged memory.</summary>
    /// <typeparam name="T">The type used to compute the new size, in bytes, of the allocation.</typeparam>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* Reallocate<T>(T* address, bool zero = false)
        where T : unmanaged
    {
        var result = TryReallocate(address, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(SizeOf<T>());
        }
        return result;
    }

    /// <summary>Reallocates a chunk of unmanaged memory that is aligned.</summary>
    /// <typeparam name="T">The type used to compute the new size, in bytes, of the allocation.</typeparam>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and that is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* Reallocate<T>(T* address, uint alignment, bool zero = false)
        where T : unmanaged
    {
        var result = TryReallocate(address, alignment, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(SizeOf<T>());
        }
        return result;
    }

    /// <summary>Reallocates a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <typeparam name="T">The type used to compute the new size, in bytes, of the allocation.</typeparam>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* Reallocate<T>(T* address, nuint alignment, nuint offset, bool zero = false)
        where T : unmanaged
    {
        var result = TryReallocate(address, alignment, offset, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(SizeOf<T>());
        }
        return result;
    }

    /// <summary>Reallocates a chunk of unmanaged memory.</summary>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newCount">The new count of elements contained in the allocation.</param>
    /// <param name="newSize">The new size, in bytes, of the elements in the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="newSize" /> bytes in length.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* ReallocateArray(void* address, nuint newCount, nuint newSize, bool zero = false)
    {
        var result = TryReallocateArray(address, newCount, newSize, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(newCount, newSize);
        }
        return result;
    }

    /// <summary>Reallocates a chunk of unmanaged memory that is aligned.</summary>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newCount">The new count of elements contained in the allocation.</param>
    /// <param name="newSize">The new size, in bytes, of the elements in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="newSize" /> bytes in length and that is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* ReallocateArray(void* address, nuint newCount, nuint newSize, nuint alignment, bool zero = false)
    {
        var result = TryReallocateArray(address, newCount, newSize, alignment, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(newCount, newSize);
        }
        return result;
    }

    /// <summary>Reallocates a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newCount">The new count of elements contained in the allocation.</param>
    /// <param name="newSize">The new size, in bytes, of the elements in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of <paramref name="offset" />.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="newSize" /> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* ReallocateArray(void* address, nuint newCount, nuint newSize, nuint alignment, nuint offset, bool zero = false)
    {
        var result = TryReallocateArray(address, newCount, newSize, alignment, offset, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(newCount, newSize);
        }
        return result;
    }

    /// <summary>Reallocates a chunk of unmanaged memory.</summary>
    /// <typeparam name="T">The type used to compute the new size, in bytes, of the elements in the allocation.</typeparam>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newCount">The new count of elements contained in the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* ReallocateArray<T>(T* address, nuint newCount, bool zero = false)
        where T : unmanaged
    {
        var result = TryReallocateArray(address, newCount, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(newCount, SizeOf<T>());
        }
        return result;
    }

    /// <summary>Reallocates a chunk of unmanaged memory that is aligned.</summary>
    /// <typeparam name="T">The type used to compute the new size, in bytes, of the elements in the allocation.</typeparam>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newCount">The new count of elements contained in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and that is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* ReallocateArray<T>(T* address, nuint newCount, nuint alignment, bool zero = false)
        where T : unmanaged
    {
        var result = TryReallocateArray<T>(address, newCount, alignment, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(newCount, SizeOf<T>());
        }
        return result;
    }

    /// <summary>Reallocates a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <typeparam name="T">The type used to compute the new size, in bytes, of the elements in the allocation.</typeparam>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newCount">The new count of elements contained in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* ReallocateArray<T>(T* address, nuint newCount, nuint alignment, nuint offset, bool zero = false)
        where T : unmanaged
    {
        var result = TryReallocateArray(address, newCount, alignment, offset, zero);

        if (result == null)
        {
            ThrowOutOfMemoryException(newCount, SizeOf<T>());
        }
        return result;
    }

    /// <summary>Tries to allocate a chunk of unmanaged memory.</summary>
    /// <param name="size">The size, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="size" /> bytes in length or <c>null</c> if the allocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* TryAllocate(nuint size, bool zero = false)
        => zero ? mi_zalloc(size) : mi_malloc(size);

    /// <summary>Tries to allocate a chunk of unmanaged memory that is aligned.</summary>
    /// <param name="size">The size, in bytes, of the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="size" /> bytes in length and that is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the allocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* TryAllocate(nuint size, nuint alignment, bool zero = false)
        => zero ? mi_zalloc_aligned(size, alignment) : mi_malloc_aligned(size, alignment);

    /// <summary>Tries to allocate a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <param name="size">The size, in bytes, of the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of <paramref name="offset" />.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="size" /> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the allocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* TryAllocate(nuint size, nuint alignment, nuint offset, bool zero = false)
        => zero ? mi_zalloc_aligned_at(size, alignment, offset) : mi_malloc_aligned_at(size, alignment, offset);

    /// <summary>Tries to allocate a chunk of unmanaged memory.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the allocation.</typeparam>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length or <c>null</c> if the allocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* TryAllocate<T>(bool zero = false)
        where T : unmanaged => zero ? mi_zalloc_tp<T>() : mi_malloc_tp<T>();

    /// <summary>Tries to allocate a chunk of unmanaged memory that is aligned.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the allocation.</typeparam>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and that is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the allocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* TryAllocate<T>(nuint alignment, bool zero = false)
        where T : unmanaged => zero ? mi_zalloc_aligned_tp<T>(alignment) : mi_malloc_aligned_tp<T>(alignment);

    /// <summary>Tries to allocate a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the allocation.</typeparam>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the allocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* TryAllocate<T>(nuint alignment, nuint offset, bool zero = false)
        where T : unmanaged => zero ? mi_zalloc_aligned_at_tp<T>(alignment, offset) : mi_malloc_aligned_at_tp<T>(alignment, offset);

    /// <summary>Tries to allocate a chunk of unmanaged memory.</summary>
    /// <param name="count">The count of elements contained in the allocation.</param>
    /// <param name="size">The size, in bytes, of the elements in the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="size" /> bytes in length or <c>null</c> if the allocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* TryAllocateArray(nuint count, nuint size, bool zero = false)
        => zero ? mi_calloc(count, size) : mi_mallocn(count, size);

    /// <summary>Tries to allocate a chunk of unmanaged memory that is aligned.</summary>
    /// <param name="count">The count of elements contained in the allocation.</param>
    /// <param name="size">The size, in bytes, of the elements in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="size" /> bytes in length and that is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the allocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* TryAllocateArray(nuint count, nuint size, nuint alignment, bool zero = false)
        => zero ? mi_calloc_aligned(count, size, alignment) : mi_mallocn_aligned(count, size, alignment);

    /// <summary>Tries to allocate a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <param name="count">The count of elements contained in the allocation.</param>
    /// <param name="size">The size, in bytes, of the elements in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of <paramref name="offset" />.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="size" /> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the allocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* TryAllocateArray(nuint count, nuint size, nuint alignment, nuint offset, bool zero = false)
        => zero ? mi_calloc_aligned_at(count, size, alignment, offset) : mi_mallocn_aligned_at(count, size, alignment, offset);

    /// <summary>Tries to allocate a chunk of unmanaged memory.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in the allocation.</typeparam>
    /// <param name="count">The count of elements contained in the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length or <c>null</c> if the allocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* TryAllocateArray<T>(nuint count, bool zero = false)
        where T : unmanaged => zero ? mi_calloc_tp<T>(count) : mi_mallocn_tp<T>(count);

    /// <summary>Tries to allocate a chunk of unmanaged memory that is aligned.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in the allocation.</typeparam>
    /// <param name="count">The count of elements contained in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and that is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the allocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* TryAllocateArray<T>(nuint count, nuint alignment, bool zero = false)
        where T : unmanaged => zero ? mi_calloc_aligned_tp<T>(count, alignment) : mi_mallocn_aligned_tp<T>(count, alignment);

    /// <summary>Tries to allocate a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in the allocation.</typeparam>
    /// <param name="count">The count of elements contained in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the allocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* TryAllocateArray<T>(nuint count, nuint alignment, nuint offset, bool zero = false)
        where T : unmanaged => zero ? mi_calloc_aligned_at_tp<T>(count, alignment, offset) : mi_mallocn_aligned_at_tp<T>(count, alignment, offset);

    /// <summary>Tries to get the index of a given item in the specified buffer.</summary>
    /// <typeparam name="T">The type of the item to try and get the index for.</typeparam>
    /// <param name="items">The items to search for <paramref name="item" />.</param>
    /// <param name="length">The length, in items, of <paramref name="items" />.</param>
    /// <param name="item">The item to try and get the index for.</param>
    /// <param name="index">When <c>true</c> is returned, this contains the index of <paramref name="item" />.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in <paramref name="items" />; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="items" /> is <c>null</c> and <paramref name="length" /> is not <c>zero</c>.</exception>
    public static bool TryGetIndexOf<T>(T* items, nuint length, T item, out nuint index)
        where T : unmanaged
    {
        if (length == 0)
        {
            index = 0;
            return false;
        }
        ThrowIfNull(items);

        return TryGetIndexOfUnsafe(items, length, item, out index);
    }

    /// <summary>Tries to get the index of a given item in the specified buffer.</summary>
    /// <typeparam name="T">The type of the item to try and get the index for.</typeparam>
    /// <param name="items">The items to search for <paramref name="item" />.</param>
    /// <param name="length">The length, in items, of <paramref name="items" />.</param>
    /// <param name="item">The item to try and get the index for.</param>
    /// <param name="index">When <c>true</c> is returned, this contains the index of <paramref name="item" />.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in <paramref name="items" />; otherwise, <c>false</c>.</returns>
    /// <remarks>This method is unsafe because it does not validate <paramref name="items" /> is not <c>null</c> if <paramref name="length" /> is not <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetIndexOfUnsafe<T>(T* items, nuint length, T item, out nuint index)
        where T : unmanaged
    {
        return SoftwareFallback(items, length, item, out index);

        static bool SoftwareFallback(T* items, nuint length, T item, out nuint index)
        {
            var equalityComparer = EqualityComparer<T>.Default;

            for (nuint i = 0; i < length; i++)
            {
                if (equalityComparer.Equals(items[i], item))
                {
                    index = i;
                    return true;
                }
            }

            index = 0;
            return false;
        }
    }

    /// <summary>Tries to get the last index of a given item in the specified buffer.</summary>
    /// <typeparam name="T">The type of the item to try and get the index for.</typeparam>
    /// <param name="items">The items to search for <paramref name="item" />.</param>
    /// <param name="length">The length, in items, of <paramref name="items" />.</param>
    /// <param name="item">The item to try and get the last index for.</param>
    /// <param name="index">When <c>true</c> is returned, this contains the last index of <paramref name="item" />.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in <paramref name="items" />; otherwise, <c>false</c>.</returns>
    /// <remarks>This method is unsafe because it does not validate <paramref name="items" /> is not <c>null</c> if <paramref name="length" /> is not <c>zero</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetLastIndexOfUnsafe<T>(T* items, nuint length, T item, out nuint index)
        where T : unmanaged
    {
        return SoftwareFallback(items, length, item, out index);

        static bool SoftwareFallback(T* items, nuint length, T item, out nuint index)
        {
            var equalityComparer = EqualityComparer<T>.Default;

            if (length != 0)
            {
                for (var i = length - 1; i > 0; i--)
                {
                    if (equalityComparer.Equals(items[i], item))
                    {
                        index = i;
                        return true;
                    }
                }

                if (equalityComparer.Equals(items[0], item))
                {
                    index = 0;
                    return true;
                }
            }

            index = 0;
            return false;
        }
    }

    /// <summary>Tries to reallocate a chunk of unmanaged memory.</summary>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newSize">The new size, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="newSize" /> bytes in length or <c>null</c> if the reallocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* TryReallocate(void* address, nuint newSize, bool zero = false)
        => zero ? mi_rezalloc(address, newSize) : mi_realloc(address, newSize);

    /// <summary>Tries to reallocate a chunk of unmanaged memory that is aligned.</summary>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newSize">The new size, in bytes, of the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="newSize" /> bytes in length and that is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the reallocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* TryReallocate(void* address, nuint newSize, nuint alignment, bool zero = false)
        => zero ? mi_rezalloc_aligned(address, newSize, alignment) : mi_realloc_aligned(address, newSize, alignment);

    /// <summary>Tries to reallocate a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newSize">The new size, in bytes, of the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of <paramref name="offset" />.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="newSize" /> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the reallocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* TryReallocate(void* address, nuint newSize, nuint alignment, nuint offset, bool zero = false)
        => zero ? mi_rezalloc_aligned_at(address, newSize, alignment, offset) : mi_realloc_aligned_at(address, newSize, alignment, offset);

    /// <summary>Tries to reallocate a chunk of unmanaged memory.</summary>
    /// <typeparam name="T">The type used to compute the new size, in bytes, of the allocation.</typeparam>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length or <c>null</c> if the reallocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* TryReallocate<T>(T* address, bool zero = false)
        where T : unmanaged => zero ? mi_rezalloc_tp<T>(address) : mi_realloc_tp<T>(address);

    /// <summary>Tries to reallocate a chunk of unmanaged memory that is aligned.</summary>
    /// <typeparam name="T">The type used to compute the new size, in bytes, of the allocation.</typeparam>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and that is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the reallocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* TryReallocate<T>(T* address, uint alignment, bool zero = false)
        where T : unmanaged => zero ? mi_rezalloc_aligned_tp<T>(address, alignment) : mi_realloc_aligned_tp<T>(address, alignment);

    /// <summary>Tries to reallocate a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <typeparam name="T">The type used to compute the new size, in bytes, of the allocation.</typeparam>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the reallocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* TryReallocate<T>(T* address, nuint alignment, nuint offset, bool zero = false)
        where T : unmanaged => zero ? mi_rezalloc_aligned_at_tp<T>(address, alignment, offset) : mi_realloc_aligned_at_tp<T>(address, alignment, offset);

    /// <summary>Tries to reallocate a chunk of unmanaged memory.</summary>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newCount">The new count of elements contained in the allocation.</param>
    /// <param name="newSize">The new size, in bytes, of the elements in the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="newSize" /> bytes in length or <c>null</c> if the reallocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* TryReallocateArray(void* address, nuint newCount, nuint newSize, bool zero = false)
        => zero ? mi_recalloc(address, newCount, newSize) : mi_reallocn(address, newCount, newSize);

    /// <summary>Tries to reallocate a chunk of unmanaged memory that is aligned.</summary>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newCount">The new count of elements contained in the allocation.</param>
    /// <param name="newSize">The new size, in bytes, of the elements in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="newSize" /> bytes in length and that is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the reallocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* TryReallocateArray(void* address, nuint newCount, nuint newSize, nuint alignment, bool zero = false)
        => zero ? mi_recalloc_aligned(address, newCount, newSize, alignment) : mi_reallocn_aligned(address, newCount, newSize, alignment);

    /// <summary>Tries to reallocate a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newCount">The new count of elements contained in the allocation.</param>
    /// <param name="newSize">The new size, in bytes, of the elements in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of <paramref name="offset" />.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <paramref name="newSize" /> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the reallocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* TryReallocateArray(void* address, nuint newCount, nuint newSize, nuint alignment, nuint offset, bool zero = false)
        => zero ? mi_recalloc_aligned_at(address, newCount, newSize, alignment, offset) : mi_reallocn_aligned_at(address, newCount, newSize, alignment, offset);

    /// <summary>Tries to reallocate a chunk of unmanaged memory.</summary>
    /// <typeparam name="T">The type used to compute the new size, in bytes, of the elements in the allocation.</typeparam>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newCount">The new count of elements contained in the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length or <c>null</c> if the reallocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* TryReallocateArray<T>(T* address, nuint newCount, bool zero = false)
        where T : unmanaged => zero ? mi_recalloc_tp<T>(address, newCount) : mi_reallocn_tp<T>(address, newCount);

    /// <summary>Tries to reallocate a chunk of unmanaged memory that is aligned.</summary>
    /// <typeparam name="T">The type used to compute the new size, in bytes, of the elements in the allocation.</typeparam>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newCount">The new count of elements contained in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and that is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the reallocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* TryReallocateArray<T>(T* address, nuint newCount, nuint alignment, bool zero = false)
        where T : unmanaged => zero ? mi_recalloc_aligned_tp<T>(address, newCount, alignment) : mi_reallocn_aligned_tp<T>(address, newCount, alignment);

    /// <summary>Tries to reallocate a chunk of unmanaged memory that is aligned at a given offset.</summary>
    /// <typeparam name="T">The type used to compute the new size, in bytes, of the elements in the allocation.</typeparam>
    /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
    /// <param name="newCount">The new count of elements contained in the allocation.</param>
    /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
    /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
    /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
    /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the reallocation failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* TryReallocateArray<T>(T* address, nuint newCount, nuint alignment, nuint offset, bool zero = false)
        where T : unmanaged => zero ? mi_recalloc_aligned_at_tp<T>(address, newCount, alignment, offset) : mi_reallocn_aligned_at_tp<T>(address, newCount, alignment, offset);
}
