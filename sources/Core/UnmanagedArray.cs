// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX;

/// <summary>Provides functionality for the <see cref="UnmanagedArray{T}" /> struct.</summary>
public static unsafe class UnmanagedArray
{
    /// <summary>Gets an empty array.</summary>
    public static UnmanagedArray<T> Empty<T>()
        where T : unmanaged => UnmanagedArray<T>.s_empty;

    /// <summary>Gets an empty array.</summary>
    public static UnmanagedArray<T, TData> Empty<T, TData>()
        where T : unmanaged
        where TData : unmanaged => UnmanagedArray<T, TData>.s_empty;

    /// <summary>Converts the array to a span.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array to convert to a span.</param>
    /// <returns>A span that covers the array.</returns>
    public static Span<T> AsSpan<T>(this UnmanagedArray<T> array)
        where T : unmanaged => array.AsUnmanagedSpan().AsSpan();

    /// <summary>Converts the array to a span.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array to convert to a span.</param>
    /// <returns>A span that covers the array.</returns>
    public static Span<T> AsSpan<T, TData>(this UnmanagedArray<T, TData> array)
        where T : unmanaged
        where TData : unmanaged => array.AsUnmanagedSpan().AsSpan();

    /// <summary>Converts the array to a span starting at the specified index.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array to convert to a span.</param>
    /// <param name="start">The index in the array at which the span should start.</param>
    /// <returns>A span that covers the array beginning at <paramref name="start" />.</returns>
    public static Span<T> AsSpan<T>(this UnmanagedArray<T> array, nuint start)
        where T : unmanaged => array.AsUnmanagedSpan(start).AsSpan();

    /// <summary>Converts the array to a span starting at the specified index.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array to convert to a span.</param>
    /// <param name="start">The index in the array at which the span should start.</param>
    /// <returns>A span that covers the array beginning at <paramref name="start" />.</returns>
    public static Span<T> AsSpan<T, TData>(this UnmanagedArray<T, TData> array, nuint start)
        where T : unmanaged
        where TData : unmanaged => array.AsUnmanagedSpan(start).AsSpan();

    /// <summary>Converts the array to a span starting at the specified index and continuing for the specified number of items.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array to convert to a span.</param>
    /// <param name="start">The index in the array at which the span should start.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <returns>A span that covers the array beginning at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    public static Span<T> AsSpan<T>(this UnmanagedArray<T> array, nuint start, nuint length)
        where T : unmanaged => array.AsUnmanagedSpan(start, length).AsSpan();

    /// <summary>Converts the array to a span starting at the specified index and continuing for the specified number of items.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array to convert to a span.</param>
    /// <param name="start">The index in the array at which the span should start.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <returns>A span that covers the array beginning at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    public static Span<T> AsSpan<T, TData>(this UnmanagedArray<T, TData> array, nuint start, nuint length)
        where T : unmanaged
        where TData : unmanaged => array.AsUnmanagedSpan(start, length).AsSpan();

    /// <summary>Converts the array to an unmanaged span.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array to convert to an unmanaged span.</param>
    /// <returns>An unmanaged span that covers the array.</returns>
    public static UnmanagedSpan<T> AsUnmanagedSpan<T>(this UnmanagedArray<T> array)
        where T : unmanaged => array;

    /// <summary>Converts the array to an unmanaged span.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array to convert to an unmanaged span.</param>
    /// <returns>An unmanaged span that covers the array.</returns>
    public static UnmanagedSpan<T> AsUnmanagedSpan<T, TData>(this UnmanagedArray<T, TData> array)
        where T : unmanaged
        where TData : unmanaged => array;

    /// <summary>Converts the array to an unmanaged span starting at the specified index.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array to convert to an unmanaged span.</param>
    /// <param name="start">The index in the array at which the span should start.</param>
    /// <returns>An unmanaged span that covers the array beginning at <paramref name="start" />.</returns>
    public static UnmanagedSpan<T> AsUnmanagedSpan<T>(this UnmanagedArray<T> array, nuint start)
        where T : unmanaged
    {
        if (!array.IsNull)
        {
            var length = array.Length;
            ThrowIfNotInBounds(start, length);
            return new UnmanagedSpan<T>(array.GetPointerUnsafe(start), length - start);
        }
        else
        {
            ThrowIfNotZero(start);
            return UnmanagedSpan.Empty<T>();
        }
    }

    /// <summary>Converts the array to an unmanaged span starting at the specified index.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array to convert to an unmanaged span.</param>
    /// <param name="start">The index in the array at which the span should start.</param>
    /// <returns>An unmanaged span that covers the array beginning at <paramref name="start" />.</returns>
    public static UnmanagedSpan<T> AsUnmanagedSpan<T, TData>(this UnmanagedArray<T, TData> array, nuint start)
        where T : unmanaged
        where TData : unmanaged
    {
        if (!array.IsNull)
        {
            var length = array.Length;
            ThrowIfNotInBounds(start, length);
            return new UnmanagedSpan<T>(array.GetPointerUnsafe(start), length - start);
        }
        else
        {
            ThrowIfNotZero(start);
            return UnmanagedSpan.Empty<T>();
        }
    }

    /// <summary>Converts the array to an unmanaged span starting at the specified index and continuing for the specified number of items.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array to convert to an unmanaged span.</param>
    /// <param name="start">The index in the array at which the span should start.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <returns>An unmanaged span that covers the array beginning at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    public static UnmanagedSpan<T> AsUnmanagedSpan<T>(this UnmanagedArray<T> array, nuint start, nuint length)
        where T : unmanaged
    {
        if (!array.IsNull)
        {
            var arrayLength = array.Length;

            ThrowIfNotInBounds(start, arrayLength);
            ThrowIfNotInInsertBounds(length, arrayLength - start);

            return new UnmanagedSpan<T>(array.GetPointerUnsafe(start), length);
        }
        else
        {
            ThrowIfNotZero(start);
            ThrowIfNotZero(length);

            return UnmanagedSpan.Empty<T>();
        }
    }

    /// <summary>Converts the array to an unmanaged span starting at the specified index and continuing for the specified number of items.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array to convert to an unmanaged span.</param>
    /// <param name="start">The index in the array at which the span should start.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <returns>An unmanaged span that covers the array beginning at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    public static UnmanagedSpan<T> AsUnmanagedSpan<T, TData>(this UnmanagedArray<T, TData> array, nuint start, nuint length)
        where T : unmanaged
        where TData : unmanaged
    {
        if (!array.IsNull)
        {
            var arrayLength = array.Length;

            ThrowIfNotInBounds(start, arrayLength);
            ThrowIfNotInInsertBounds(length, arrayLength - start);

            return new UnmanagedSpan<T>(array.GetPointerUnsafe(start), length);
        }
        else
        {
            ThrowIfNotZero(start);
            ThrowIfNotZero(length);

            return UnmanagedSpan.Empty<T>();
        }
    }

    /// <summary>Clears all items in the array to <c>zero</c>.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array to clear.</param>
    public static void Clear<T>(this UnmanagedArray<T> array)
        where T : unmanaged
    {
        AssertNotNull(array);
        ClearArrayUnsafe(array.GetPointerUnsafe(), array.Length);
    }

    /// <summary>Clears all items in the array to <c>zero</c>.</summary>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array to clear.</param>
    public static void Clear<T, TData>(this UnmanagedArray<T, TData> array)
        where T : unmanaged
        where TData : unmanaged
    {
        AssertNotNull(array);
        ClearArrayUnsafe(array.GetPointerUnsafe(), array.Length);
    }

    /// <summary>Copies the items in the array to a given destination.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array to copy.</param>
    /// <param name="destination">The destination array where the items should be copied.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedArray{T}.Length" /> is greater than <paramref name="destination" />.</exception>
    public static void CopyTo<T>(this UnmanagedArray<T> array, UnmanagedArray<T> destination)
        where T : unmanaged
    {
        AssertNotNull(array);

        var metadataLength = array.Length;

        ThrowIfNull(destination);
        ThrowIfNotInInsertBounds(metadataLength, destination.Length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(), array.GetPointerUnsafe(), metadataLength);
    }

    /// <summary>Copies the items in the array to a given destination.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array to copy.</param>
    /// <param name="destination">The destination array where the items should be copied.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedArray{T}.Length" /> is greater than <paramref name="destination" />.</exception>
    public static void CopyTo<T, TData>(this UnmanagedArray<T> array, UnmanagedArray<T, TData> destination)
        where T : unmanaged
        where TData : unmanaged
    {
        AssertNotNull(array);

        var metadataLength = array.Length;

        ThrowIfNull(destination);
        ThrowIfNotInInsertBounds(metadataLength, destination.Length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(), array.GetPointerUnsafe(), metadataLength);
    }

    /// <summary>Copies the items in the array to a given destination.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array to copy.</param>
    /// <param name="destination">The destination array where the items should be copied.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedArray{T, TData}.Length" /> is greater than <paramref name="destination" />.</exception>
    public static void CopyTo<T, TData>(this UnmanagedArray<T, TData> array, UnmanagedArray<T> destination)
        where T : unmanaged
        where TData : unmanaged
    {
        AssertNotNull(array);

        var metadataLength = array.Length;

        ThrowIfNull(destination);
        ThrowIfNotInInsertBounds(metadataLength, destination.Length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(), array.GetPointerUnsafe(), metadataLength);
    }

    /// <summary>Copies the items in the array to a given destination.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array to copy.</param>
    /// <param name="destination">The destination array where the items should be copied.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedArray{T, TData}.Length" /> is greater than <paramref name="destination" />.</exception>
    public static void CopyTo<T, TData>(this UnmanagedArray<T, TData> array, UnmanagedArray<T, TData> destination)
        where T : unmanaged
        where TData : unmanaged
    {
        AssertNotNull(array);

        var metadataLength = array.Length;

        ThrowIfNull(destination);
        ThrowIfNotInInsertBounds(metadataLength, destination.Length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(), array.GetPointerUnsafe(), metadataLength);
    }

    /// <summary>Copies the items in the array to a given destination.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array to copy.</param>
    /// <param name="destination">The destination span where the items should be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedArray{T}.Length" /> is greater than <paramref name="destination" />.</exception>
    public static void CopyTo<T>(this UnmanagedArray<T> array, UnmanagedSpan<T> destination)
        where T : unmanaged
    {
        AssertNotNull(array);

        var metadataLength = array.Length;

        ThrowIfNotInInsertBounds(metadataLength, destination.Length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(), array.GetPointerUnsafe(), metadataLength);
    }

    /// <summary>Copies the items in the array to a given destination.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array to copy.</param>
    /// <param name="destination">The destination span where the items should be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedArray{T, TData}.Length" /> is greater than <paramref name="destination" />.</exception>
    public static void CopyTo<T, TData>(this UnmanagedArray<T, TData> array, UnmanagedSpan<T> destination)
        where T : unmanaged
        where TData : unmanaged
    {
        AssertNotNull(array);

        var metadataLength = array.Length;

        ThrowIfNotInInsertBounds(metadataLength, destination.Length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(), array.GetPointerUnsafe(), metadataLength);
    }

    /// <summary>Gets a pointer to the item at the specified index in the array.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array for which to get a pointer.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the array.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="UnmanagedArray{T}.Length" />.</exception>
    public static T* GetPointer<T>(this UnmanagedArray<T> array, nuint index = 0)
        where T : unmanaged
    {
        ThrowIfNotInBounds(index, array.Length);
        return array.GetPointerUnsafe(index);
    }

    /// <summary>Gets a pointer to the item at the specified index in the array.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array for which to get a pointer.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the array.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="UnmanagedArray{T, TData}.Length" />.</exception>
    public static T* GetPointer<T, TData>(this UnmanagedArray<T, TData> array, nuint index = 0)
        where T : unmanaged
        where TData : unmanaged
    {
        ThrowIfNotInBounds(index, array.Length);
        return array.GetPointerUnsafe(index);
    }

    /// <summary>Gets a pointer to the item at the specified index in the array.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array for which to get a pointer.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the array.</returns>
    /// <remarks>This method is unsafe because it does not validate that <paramref name="index" /> is less than <see cref="UnmanagedArray{T}.Length" />.</remarks>
    public static T* GetPointerUnsafe<T>(this UnmanagedArray<T> array, nuint index = 0)
        where T : unmanaged
    {
        Assert(index <= array.Length);
        return &array._metadata->Item + index;
    }

    /// <summary>Gets a pointer to the item at the specified index in the array.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array for which to get a pointer.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the array.</returns>
    /// <remarks>This method is unsafe because it does not validate that <paramref name="index" /> is less than <see cref="UnmanagedArray{T, TData}.Length" />.</remarks>
    public static T* GetPointerUnsafe<T, TData>(this UnmanagedArray<T, TData> array, nuint index = 0)
        where T : unmanaged
        where TData : unmanaged
    {
        Assert(index <= array.Length);
        return &array._metadata->Item + index;
    }

    /// <summary>Gets a reference to the item at the specified index in the array.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array for which to get a reference.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the array.</returns>
    public static ref T GetReference<T>(this UnmanagedArray<T> array, nuint index = 0)
        where T : unmanaged => ref *array.GetPointer(index);

    /// <summary>Gets a reference to the item at the specified index in the array.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array for which to get a reference.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the array.</returns>
    public static ref T GetReference<T, TData>(this UnmanagedArray<T, TData> array, nuint index = 0)
        where T : unmanaged
        where TData : unmanaged => ref *array.GetPointer(index);

    /// <summary>Gets a reference to the item at the specified index in the array.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <param name="array">The array for which to get a reference.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the array.</returns>
    /// <remarks>This method is unsafe because it does not validate that <paramref name="index" /> is less than <see cref="UnmanagedArray{T}.Length" />.</remarks>
    public static ref T GetReferenceUnsafe<T>(this UnmanagedArray<T> array, nuint index = 0)
        where T : unmanaged => ref *array.GetPointerUnsafe(index);

    /// <summary>Gets a reference to the item at the specified index in the array.</summary>
    /// <typeparam name="T">The type of the items contained by the array.</typeparam>
    /// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
    /// <param name="array">The array for which to get a reference.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the array.</returns>
    /// <remarks>This method is unsafe because it does not validate that <paramref name="index" /> is less than <see cref="UnmanagedArray{T, TData}.Length" />.</remarks>
    public static ref T GetReferenceUnsafe<T, TData>(this UnmanagedArray<T, TData> array, nuint index = 0)
        where T : unmanaged
        where TData : unmanaged => ref *array.GetPointerUnsafe(index);
}
