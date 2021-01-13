// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.CompilerServices;
using static TerraFX.Interop.Mimalloc;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for simplifying memory management code.</summary>
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
            }
            return result;
        }

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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
            }
            return result;
        }

        /// <summary>Reallocates a chunk of unmanaged memory.</summary>
        /// <typeparam name="T">The type used to compute the new size, in bytes, of the allocation.</typeparam>
        /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
        /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
        /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* Reallocate<T>(void* address, bool zero = false)
            where T : unmanaged
        {
            var result = TryReallocate<T>(address, zero);

            if (result == null)
            {
                ThrowOutOfMemoryException();
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
        public static T* Reallocate<T>(void* address, uint alignment, bool zero = false)
            where T : unmanaged
        {
            var result = TryReallocate<T>(address, alignment, zero);

            if (result == null)
            {
                ThrowOutOfMemoryException();
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
        public static T* Reallocate<T>(void* address, nuint alignment, nuint offset, bool zero = false)
            where T : unmanaged
        {
            var result = TryReallocate<T>(address, alignment, offset, zero);

            if (result == null)
            {
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
                ThrowOutOfMemoryException();
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
        public static T* ReallocateArray<T>(void* address, nuint newCount, bool zero = false)
            where T : unmanaged
        {
            var result = TryReallocateArray<T>(address, newCount, zero);

            if (result == null)
            {
                ThrowOutOfMemoryException();
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
        public static T* ReallocateArray<T>(void* address, nuint newCount, nuint alignment, bool zero = false)
            where T : unmanaged
        {
            var result = TryReallocateArray<T>(address, newCount, alignment, zero);

            if (result == null)
            {
                ThrowOutOfMemoryException();
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
        public static T* ReallocateArray<T>(void* address, nuint newCount, nuint alignment, nuint offset, bool zero = false)
            where T : unmanaged
        {
            var result = TryReallocateArray<T>(address, newCount, alignment, offset, zero);

            if (result == null)
            {
                ThrowOutOfMemoryException();
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
        public static T* TryReallocate<T>(void* address, bool zero = false)
            where T : unmanaged => zero ? mi_rezalloc_tp<T>(address) : mi_realloc_tp<T>(address);

        /// <summary>Tries to reallocate a chunk of unmanaged memory that is aligned.</summary>
        /// <typeparam name="T">The type used to compute the new size, in bytes, of the allocation.</typeparam>
        /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
        /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
        /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
        /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and that is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the reallocation failed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* TryReallocate<T>(void* address, uint alignment, bool zero = false)
            where T : unmanaged => zero ? mi_rezalloc_aligned_tp<T>(address, alignment) : mi_realloc_aligned_tp<T>(address, alignment);

        /// <summary>Tries to reallocate a chunk of unmanaged memory that is aligned at a given offset.</summary>
        /// <typeparam name="T">The type used to compute the new size, in bytes, of the allocation.</typeparam>
        /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
        /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
        /// <param name="offset">The offset, in bytes, of the memory that is aligned.</param>
        /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
        /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and where <paramref name="offset" /> is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the reallocation failed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* TryReallocate<T>(void* address, nuint alignment, nuint offset, bool zero = false)
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
        public static T* TryReallocateArray<T>(void* address, nuint newCount, bool zero = false)
            where T : unmanaged => zero ? mi_recalloc_tp<T>(address, newCount) : mi_reallocn_tp<T>(address, newCount);

        /// <summary>Tries to reallocate a chunk of unmanaged memory that is aligned.</summary>
        /// <typeparam name="T">The type used to compute the new size, in bytes, of the elements in the allocation.</typeparam>
        /// <param name="address">The address of an allocated chunk of memory to reallocate</param>
        /// <param name="newCount">The new count of elements contained in the allocation.</param>
        /// <param name="alignment">The alignment, in bytes, of the allocation.</param>
        /// <param name="zero"><c>true</c> if the allocated memory should be zeroed; otherwise, <c>false</c>.</param>
        /// <returns>The address to an allocated chunk of memory that is at least <c>sizeof(<typeparamref name="T" />)</c> bytes in length and that is at least <paramref name="alignment" /> bytes aligned or <c>null</c> if the reallocation failed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* TryReallocateArray<T>(void* address, nuint newCount, nuint alignment, bool zero = false)
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
        public static T* TryReallocateArray<T>(void* address, nuint newCount, nuint alignment, nuint offset, bool zero = false)
            where T : unmanaged => zero ? mi_recalloc_aligned_at_tp<T>(address, newCount, alignment, offset) : mi_reallocn_aligned_at_tp<T>(address, newCount, alignment, offset);
    }
}
