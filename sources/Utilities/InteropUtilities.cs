// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for simplifying interop code.</summary>
    public static unsafe class InteropUtilities
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

        /// <summary>Gets a string for a given <see cref="ReadOnlySpan{SByte}" />.</summary>
        /// <param name="source">The <see cref="ReadOnlySpan{SByte}" /> for which to create the string.</param>
        /// <returns>A string created from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? AsString(this ReadOnlySpan<sbyte> source)
        {
            string? result = null;

            if (source.AsPointer() != null)
            {
                var bytes = MemoryMarshal.Cast<sbyte, byte>(source);
                result = Encoding.UTF8.GetString(bytes);
            }

            return result;
        }

        /// <summary>Gets a string for a given <see cref="ReadOnlySpan{UInt16}" />.</summary>
        /// <param name="source">The <see cref="ReadOnlySpan{UInt16}" /> for which to create the string.</param>
        /// <returns>A string created from <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? AsString(this ReadOnlySpan<ushort> source)
        {
            string? result = null;

            if (source.AsPointer() != null)
            {
                var chars = MemoryMarshal.Cast<ushort, char>(source);
                result = new string(chars);
            }

            return result;
        }

        /// <summary>Marshals a managed delegate to get a function pointer which will invoke it.</summary>
        /// <typeparam name="TDelegate">The type of the managed delegate.</typeparam>
        /// <param name="function">The managed delegate for which to marshal.</param>
        /// <returns>A function pointer that invokes <paramref name="function" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr MarshalDelegate<TDelegate>(TDelegate function)
            where TDelegate : notnull => Marshal.GetFunctionPointerForDelegate(function);

        /// <summary>Marshals a function pointer to get a managed delegate which will invoke it.</summary>
        /// <typeparam name="TDelegate">The type of the managed delegate.</typeparam>
        /// <param name="function">The function pointer for which to marshal.</param>
        /// <returns>A managed delegate that invokes <paramref name="function" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TDelegate MarshalFunctionPointer<TDelegate>(IntPtr function) => Marshal.GetDelegateForFunctionPointer<TDelegate>(function);

        /// <summary>Marshals a string to a null-terminated UTF8 string.</summary>
        /// <param name="source">The string for which to marshal.</param>
        /// <returns>A null-terminated UTF8 string that is equivalent to <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<sbyte> MarshalStringToUtf8(string? source)
        {
            ReadOnlySpan<sbyte> result;

            if (source is null)
            {
                result = null;
            }
            else
            {
                var maxLength = Encoding.UTF8.GetMaxByteCount(source.Length);
                var bytes = new sbyte[maxLength + 1];

                var length = Encoding.UTF8.GetBytes(source, MemoryMarshal.Cast<sbyte, byte>(bytes));
                result = bytes.AsSpan(0, length);
            }

            return result;
        }

        /// <summary>Marshals a string to a null-terminated UTF16 string.</summary>
        /// <param name="source">The string for which to marshal.</param>
        /// <returns>A null-terminated UTF16 string that is equivalent to <paramref name="source" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<ushort> MarshalStringToUtf16(string? source) => MemoryMarshal.Cast<char, ushort>(source.AsSpan());

        /// <summary>Marshals a null-terminated UTF8 string to a <see cref="ReadOnlySpan{SByte}" />.</summary>
        /// <param name="source">The pointer to the null-terminated UTF8 string.</param>
        /// <param name="maxLength">The maxmimum length of <paramref name="source" /> or <c>-1</c> if the maximum length is unknown.</param>
        /// <returns>A <see cref="ReadOnlySpan{SByte}" /> that starts at <paramref name="source" /> and extends to <paramref name="maxLength" /> or the first null character, whichever comes first.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<sbyte> MarshalUtf8ToReadOnlySpan(sbyte* source, int maxLength = -1) => (source != null) ? MarshalUtf8ToReadOnlySpan(in *source, maxLength) : default;

        /// <summary>Marshals a null-terminated UTF8 string to a <see cref="ReadOnlySpan{SByte}" />.</summary>
        /// <param name="source">The pointer to the null-terminated UTF8 string.</param>
        /// <param name="maxLength">The maxmimum length of <paramref name="source" /> or <c>-1</c> if the maximum length is unknown.</param>
        /// <returns>A <see cref="ReadOnlySpan{SByte}" /> that starts at <paramref name="source" /> and extends to <paramref name="maxLength" /> or the first null character, whichever comes first.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<sbyte> MarshalUtf8ToReadOnlySpan(in sbyte source, int maxLength = -1)
        {
            if (maxLength < 0)
            {
                maxLength = int.MaxValue;
            }

            var span = MemoryMarshal.CreateReadOnlySpan(ref AsRef(in source), maxLength);
            var length = span.IndexOf((sbyte)'\0');

            if (length != -1)
            {
                span = span.Slice(0, length);
            }

            return span;
        }

        /// <summary>Marshals a null-terminated UTF16 string to a <see cref="ReadOnlySpan{UInt16}" />.</summary>
        /// <param name="source">The pointer to the null-terminated UTF16 string.</param>
        /// <param name="maxLength">The maxmimum length of <paramref name="source" /> or <c>-1</c> if the maximum length is unknown.</param>
        /// <returns>A <see cref="ReadOnlySpan{UInt16}" /> that starts at <paramref name="source" /> and extends to <paramref name="maxLength" /> or the first null character, whichever comes first.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<ushort> MarshalUtf16ToReadOnlySpan(ushort* source, int maxLength = -1) => (source != null) ? MarshalNullTerminatedStringUtf16(in *source, maxLength) : null;

        /// <summary>Marshals a null-terminated UTF16 string to a <see cref="ReadOnlySpan{UInt16}" />.</summary>
        /// <param name="source">The pointer to the null-terminated UTF16 string.</param>
        /// <param name="maxLength">The maxmimum length of <paramref name="source" /> or <c>-1</c> if the maximum length is unknown.</param>
        /// <returns>A <see cref="ReadOnlySpan{UInt16}" /> that starts at <paramref name="source" /> and extends to <paramref name="maxLength" /> or the first null character, whichever comes first.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<ushort> MarshalNullTerminatedStringUtf16(in ushort source, int maxLength = -1)
        {
            if (maxLength < 0)
            {
                maxLength = int.MaxValue;
            }

            var span = MemoryMarshal.CreateReadOnlySpan(ref AsRef(in source), maxLength);
            var length = span.IndexOf('\0');

            if (length != -1)
            {
                span = span.Slice(0, length);
            }

            return span;
        }

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
