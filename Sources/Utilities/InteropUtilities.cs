// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for simplifying interop code.</summary>
    public static unsafe class InteropUtilities
    {
        #region Static Methods
        /// <summary>Gets a reference of <typeparamref name="T" /> from a given <see cref="int" />.</summary>
        /// <typeparam name="T">The type of the reference to retrieve.</typeparam>
        /// <param name="source">The <see cref="int" /> for which to get the reference from.</param>
        /// <returns>A reference of <typeparamref name="T" /> from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(int source)
        {
            return ref Unsafe.AsRef<T>((void*)(source));
        }

        /// <summary>Gets a reference of <typeparamref name="T" /> from a given pointer.</summary>
        /// <typeparam name="T">The type of the reference to retrieve.</typeparam>
        /// <param name="source">The pointer for which to get the reference from.</param>
        /// <returns>A reference of <typeparamref name="T" /> from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(void* source)
        {
            return ref Unsafe.AsRef<T>(source);
        }

        /// <summary>Gets a null reference of <typeparamref name="T"/>.</summary>
        /// <typeparam name="T">The type of the null reference to retrieve.</typeparam>
        /// <returns>A null reference of <typeparamref name="T"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T NullRef<T>()
        {
            return ref Unsafe.AsRef<T>(null);
        }

        /// <summary>Gets the size of <typeparamref name="T" />.</summary>
        /// <typeparam name="T">The type for which to get the size.</typeparam>
        /// <returns>The size of <typeparamref name="T" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint SizeOf<T>()
        {
            return unchecked((uint)(Marshal.SizeOf<T>()));
        }
        #endregion
    }
}
