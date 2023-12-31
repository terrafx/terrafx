// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX;

/// <summary>Provides functionality for the <see cref="UnmanagedSpan{T}" /> struct.</summary>
public static unsafe class UnmanagedSpan
{
    /// <summary>Gets an empty span.</summary>
    public static UnmanagedSpan<T> Empty<T>()
        where T : unmanaged => new UnmanagedSpan<T>();

    /// <summary>Converts the unmanaged span to a span.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <param name="span">The unmanaged span to convert.</param>
    /// <returns>A span that covers the unmanaged span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedSpan{T}.Length" /> is greater than <see cref="int.MaxValue" />.</exception>
    public static Span<T> AsSpan<T>(this UnmanagedSpan<T> span)
        where T : unmanaged
    {
        var length = span.Length;
        ThrowIfNotInInsertBounds(length, int.MaxValue);
        return new Span<T>(span.GetPointerUnsafe(), (int)length);
    }

    /// <summary>Converts the array to a span starting at the specified index.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <param name="span">The unmanaged span to convert.</param>
    /// <param name="start">The index of the array at which the span should start.</param>
    /// <returns>A span that covers the array beginning at <paramref name="start" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedSpan{T}.Length" /> is greater than <see cref="int.MaxValue" />.</exception>
    public static Span<T> AsSpan<T>(this UnmanagedSpan<T> span, nuint start)
        where T : unmanaged
    {
        var length = span.Length;
        ThrowIfNotInBounds(start, length);

        length -= start;
        ThrowIfNotInInsertBounds(length, int.MaxValue);

        return new Span<T>(span.GetPointerUnsafe(start), (int)length);
    }

    /// <summary>Converts the array to a span starting at the specified index and continuing for the specified number of items.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <param name="span">The unmanaged span to convert.</param>
    /// <param name="start">The index of the array at which the span should start.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <returns>A span that covers the array beginning at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    public static Span<T> AsSpan<T>(this UnmanagedSpan<T> span, nuint start, nuint length)
        where T : unmanaged
    {
        var spanLength = span.Length;

        ThrowIfNotInBounds(start, spanLength);
        ThrowIfNotInInsertBounds(length, spanLength - start);
        ThrowIfNotInInsertBounds(length, int.MaxValue);

        return new Span<T>(span.GetPointerUnsafe(start), (int)length);
    }

    /// <summary>Clears all items in the span to <c>zero</c>.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <param name="span">The unmanaged span to clear.</param>
    public static void Clear<T>(this UnmanagedSpan<T> span)
        where T : unmanaged => ClearArrayUnsafe(span.GetPointerUnsafe(), span.Length);

    /// <summary>Copies the items in the span to a given destination.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <param name="span">The unmanaged span to copy.</param>
    /// <param name="destination">The destination array where the items should be copied.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedSpan{T}.Length" /> is greater than <paramref name="destination" />.</exception>
    public static void CopyTo<T>(this UnmanagedSpan<T> span, UnmanagedArray<T> destination)
        where T : unmanaged
    {
        var length = span.Length;

        ThrowIfNull(destination);
        ThrowIfNotInInsertBounds(length, destination.Length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(), span.GetPointerUnsafe(), length);
    }

    /// <summary>Copies the items in the span to a given destination.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="span">The unmanaged span to copy.</param>
    /// <param name="destination">The destination array where the items should be copied.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedSpan{T}.Length" /> is greater than <paramref name="destination" />.</exception>
    public static void CopyTo<T, TData>(this UnmanagedSpan<T> span, UnmanagedArray<T, TData> destination)
        where T : unmanaged
        where TData : unmanaged
    {
        var length = span.Length;

        ThrowIfNull(destination);
        ThrowIfNotInInsertBounds(length, destination.Length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(), span.GetPointerUnsafe(), length);
    }

    /// <summary>Copies the items in the array to a given destination.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <param name="span">The unmanaged span to copy.</param>
    /// <param name="destination">The destination span where the items should be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedSpan{T}.Length" /> is greater than <paramref name="destination" />.</exception>
    public static void CopyTo<T>(this UnmanagedSpan<T> span, UnmanagedSpan<T> destination)
        where T : unmanaged
    {
        var length = span.Length;

        ThrowIfNotInInsertBounds(length, destination.Length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(), span.GetPointerUnsafe(), length);
    }

    /// <summary>Gets a pointer to the item at the specified index in the span.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <param name="span">The unmanaged span for which to get a pointer.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="UnmanagedSpan{T}.Length" />.</exception>
    public static T* GetPointer<T>(this UnmanagedSpan<T> span, nuint index = 0)
        where T : unmanaged
    {
        ThrowIfNotInBounds(index, span.Length);
        return span.GetPointerUnsafe(index);
    }

    /// <summary>Gets a pointer to the item at the specified index in the span.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <param name="span">The unmanaged span for which to get a pointer.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the span.</returns>
    /// <remarks>This method is unsafe because it does not validate that <paramref name="index" /> is less than <see cref="UnmanagedSpan{T}.Length" />.</remarks>
    public static T* GetPointerUnsafe<T>(this UnmanagedSpan<T> span, nuint index = 0)
        where T : unmanaged
    {
        Assert(index < span.Length);
        return span._items + index;
    }

    /// <summary>Gets a reference to the item at the specified index in the span.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <param name="span">The unmanaged span for which to get a reference.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the span.</returns>
    public static ref T GetReference<T>(this UnmanagedSpan<T> span, nuint index = 0)
        where T : unmanaged => ref *span.GetPointer(index);

    /// <summary>Gets a reference to the item at the specified index in the span.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <param name="span">The unmanaged span for which to get a reference.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the span.</returns>
    /// <remarks>This method is unsafe because it does not validate that <paramref name="index" /> is less than <see cref="UnmanagedSpan{T}.Length" />.</remarks>
    public static ref T GetReferenceUnsafe<T>(this UnmanagedSpan<T> span, nuint index = 0)
        where T : unmanaged => ref *span.GetPointerUnsafe(index);

    /// <summary>Slices the span so that it begins at the specified index.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <param name="span">The unmanaged span to slice.</param>
    /// <param name="start">The index at which the span should start.</param>
    /// <returns>A span that covers the same memory as the current span beginning at <paramref name="start" />.</returns>
    public static UnmanagedSpan<T> Slice<T>(this UnmanagedSpan<T> span, nuint start)
        where T : unmanaged
    {
        var length = span.Length;
        ThrowIfNotInBounds(start, length);
        return new UnmanagedSpan<T>(span.GetPointerUnsafe(start), length - start);
    }

    /// <summary>Slices the span so that it begins at the specified index and continuing for the specified number of items.</summary>
    /// <typeparam name="T">The type of items contained in the span.</typeparam>
    /// <param name="span">The unmanaged span to slice.</param>
    /// <param name="start">The index at which the span should start.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <returns>A span that covers the same memory as the current span beginning at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    public static UnmanagedSpan<T> Slice<T>(this UnmanagedSpan<T> span, nuint start, nuint length)
        where T : unmanaged
    {
        var spanLength = span.Length;
        ThrowIfNotInBounds(start, spanLength);

        ThrowIfNotInInsertBounds(length, spanLength - start);
        return new UnmanagedSpan<T>(span.GetPointerUnsafe(start), length);
    }
}
