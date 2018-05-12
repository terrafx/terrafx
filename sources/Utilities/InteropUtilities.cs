// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for simplifying interop code.</summary>
    public static unsafe class InteropUtilities
    {
        #region Static Fields
        /// <summary>Holds a mapping of all of the managed delegates created for the various function pointers.</summary>
        private static readonly Dictionary<IntPtr, Delegate> _marshalledFunctions = new Dictionary<IntPtr, Delegate>();
        #endregion

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

        /// <summary>Gets a reference of <typeparamref name="T" /> from a given <see cref="nint" />.</summary>
        /// <typeparam name="T">The type of the reference to retrieve.</typeparam>
        /// <param name="source">The <see cref="nint" /> for which to get the reference from.</param>
        /// <returns>A reference of <typeparamref name="T" /> from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(nint source)
        {
            return ref Unsafe.AsRef<T>((void*)(source));
        }

        /// <summary>Gets a reference of <typeparamref name="T" /> from a given <see cref="nuint" />.</summary>
        /// <typeparam name="T">The type of the reference to retrieve.</typeparam>
        /// <param name="source">The <see cref="nuint" /> for which to get the reference from.</param>
        /// <returns>A reference of <typeparamref name="T" /> from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(nuint source)
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

        /// <summary>Marshals a function pointer to get a corresponding managed delegate.</summary>
        /// <typeparam name="TDelegate">The type of the managed delegate to get.</typeparam>
        /// <param name="function">The function pointer to marshal.</param>
        /// <returns>A managed delegate that invokes <paramref name="function" />.</returns>
        public static TDelegate MarshalFunction<TDelegate>(IntPtr function)
        {
            if (_marshalledFunctions.TryGetValue(function, out var value) == false)
            {
                // In the case where two threads marshal the same function simultaneously,
                // we will end up with an existing entry. We will just overwrite since both
                // the delegates will do the same thing. The GC will just end up having some
                // extra work.
                value = Marshal.GetDelegateForFunctionPointer(function, typeof(TDelegate));
                _marshalledFunctions[function] = value;
            }

            return (TDelegate)((object)(value));
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
