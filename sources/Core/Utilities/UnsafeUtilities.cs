// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods to supplement or replace <see cref="Unsafe" /> and <see cref="MemoryMarshal" />.</summary>
    public static unsafe class UnsafeUtilities
    {
        /// <summary>Gets the underlying pointer for a <typeparamref name="T" /> reference.</summary>
        /// <typeparam name="T">The type of <paramref name="source" />.</typeparam>
        /// <param name="source">The reference for which to get the underlying pointer.</param>
        /// <returns>The underlying pointer for <paramref name="source" />.</returns>
        public static T* AsPointer<T>(ref T source)
            where T : unmanaged => (T*)Unsafe.AsPointer(ref source);

        /// <summary>Gets the underlying pointer for a <see cref="Span{T}" />.</summary>
        /// <typeparam name="T">The type of elements contained in <paramref name="source" />.</typeparam>
        /// <param name="source">The span for which to get the underlying pointer.</param>
        /// <returns>The underlying pointer for <paramref name="source" />.</returns>
        public static T* AsPointer<T>(this Span<T> source)
            where T : unmanaged => AsPointer(ref source.AsRef());

        /// <summary>Gets the underlying pointer for a <see cref="ReadOnlySpan{T}" />.</summary>
        /// <typeparam name="T">The type of elements contained in <paramref name="source" />.</typeparam>
        /// <param name="source">The span for which to get the underlying pointer.</param>
        /// <returns>The underlying pointer for <paramref name="source" />.</returns>
        public static T* AsPointer<T>(this ReadOnlySpan<T> source)
            where T : unmanaged => AsPointer(ref source.AsRef());

        /// <summary>Gets a reference of <typeparamref name="T" /> from a given <see cref="IntPtr" />.</summary>
        /// <typeparam name="T">The type of the reference.</typeparam>
        /// <param name="source">The <see cref="IntPtr" /> for which to get the reference.</param>
        /// <returns>A reference of <typeparamref name="T" /> from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(IntPtr source) => ref Unsafe.AsRef<T>((void*)source);

        /// <summary>Gets a reference of <typeparamref name="T" /> from a given <see cref="UIntPtr" />.</summary>
        /// <typeparam name="T">The type of the reference.</typeparam>
        /// <param name="source">The <see cref="UIntPtr" /> for which to get the reference.</param>
        /// <returns>A reference of <typeparamref name="T" /> from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(UIntPtr source) => ref Unsafe.AsRef<T>((void*)source);

        /// <summary>Gets a reference of <typeparamref name="T" /> from a given pointer.</summary>
        /// <typeparam name="T">The type of the reference.</typeparam>
        /// <param name="source">The pointer for which to get the reference.</param>
        /// <returns>A reference of <typeparamref name="T" /> from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(void* source) => ref Unsafe.AsRef<T>(source);

        /// <summary>Gets a reference of <typeparamref name="T" /> from a given readonly reference.</summary>
        /// <typeparam name="T">The type of <paramref name="source" />.</typeparam>
        /// <param name="source">The readonly reference for which to get the reference.</param>
        /// <returns>A reference of <typeparamref name="T" /> from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(in T source) => ref Unsafe.AsRef(in source);

        /// <summary>Gets the underlying reference for a <see cref="Span{T}" />.</summary>
        /// <typeparam name="T">The type of elements contained in <paramref name="source" />.</typeparam>
        /// <param name="source">The span for which to get the underlying reference.</param>
        /// <returns>The underlying reference for <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(this Span<T> source) => ref MemoryMarshal.GetReference(source);

        /// <summary>Gets the underlying reference for a <see cref="ReadOnlySpan{T}" />.</summary>
        /// <typeparam name="T">The type of elements contained in <paramref name="source" />.</typeparam>
        /// <param name="source">The span for which to get the underlying reference.</param>
        /// <returns>The underlying reference for <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(this ReadOnlySpan<T> source) => ref MemoryMarshal.GetReference(source);

        /// <summary>Gets the size of <typeparamref name="T" />.</summary>
        /// <typeparam name="T">The type for which to get the size.</typeparam>
        /// <returns>The size of <typeparamref name="T" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint SizeOf<T>() => unchecked((uint)Unsafe.SizeOf<T>());
    }
}
