// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for simplifying interop code.</summary>
    public static unsafe class InteropUtilities
    {
        /// <summary>Gets a reference of <typeparamref name="T" /> from a given <see cref="int" />.</summary>
        /// <typeparam name="T">The type of the reference to retrieve.</typeparam>
        /// <param name="source">The <see cref="int" /> for which to get the reference from.</param>
        /// <returns>A reference of <typeparamref name="T" /> from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(int source) => ref Unsafe.AsRef<T>((void*)source);

        /// <summary>Gets a reference of <typeparamref name="T" /> from a given <see cref="IntPtr" />.</summary>
        /// <typeparam name="T">The type of the reference to retrieve.</typeparam>
        /// <param name="source">The <see cref="IntPtr" /> for which to get the reference from.</param>
        /// <returns>A reference of <typeparamref name="T" /> from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(IntPtr source) => ref Unsafe.AsRef<T>((void*)source);

        /// <summary>Gets a reference of <typeparamref name="T" /> from a given <see cref="UIntPtr" />.</summary>
        /// <typeparam name="T">The type of the reference to retrieve.</typeparam>
        /// <param name="source">The <see cref="UIntPtr" /> for which to get the reference from.</param>
        /// <returns>A reference of <typeparamref name="T" /> from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(UIntPtr source) => ref Unsafe.AsRef<T>((void*)source);

        /// <summary>Gets a reference of <typeparamref name="T" /> from a given pointer.</summary>
        /// <typeparam name="T">The type of the reference to retrieve.</typeparam>
        /// <param name="source">The pointer for which to get the reference from.</param>
        /// <returns>A reference of <typeparamref name="T" /> from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(void* source) => ref Unsafe.AsRef<T>(source);

        /// <summary>Marshals a function pointer to get a corresponding managed delegate.</summary>
        /// <typeparam name="TDelegate">The type of the managed delegate to get.</typeparam>
        /// <param name="function">The function pointer to marshal.</param>
        /// <returns>A managed delegate that invokes <paramref name="function" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TDelegate MarshalFunction<TDelegate>(IntPtr function) => Marshal.GetDelegateForFunctionPointer<TDelegate>(function);

        /// <summary>Gets a null reference of <typeparamref name="T"/>.</summary>
        /// <typeparam name="T">The type of the null reference to retrieve.</typeparam>
        /// <returns>A null reference of <typeparamref name="T"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T NullRef<T>() => ref Unsafe.AsRef<T>(null);

        /// <summary>Gets the size of <typeparamref name="T" />.</summary>
        /// <typeparam name="T">The type for which to get the size.</typeparam>
        /// <returns>The size of <typeparamref name="T" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint SizeOf<T>() => unchecked((uint)Marshal.SizeOf<T>());

        /// <summary>Bypasses definite assignment rules by taking advantage of <c>out</c> semantics.</summary>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="value">The value for which to skip initialization.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SkipInit<T>([MaybeNull] out T value)
        {
            value = default!;
        }
    }
}
