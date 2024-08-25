// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the List<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;
using TerraFX.Threading;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="UnmanagedValueList{T}" /> struct.</summary>
public static unsafe class UnmanagedValueList
{
    /// <summary>Gets an empty list.</summary>
    public static UnmanagedValueList<T> Empty<T>()
        where T : unmanaged => [];

    /// <summary>Adds an item to the list.</summary>
    /// <param name="list">The list to which the item should be added.</param>
    /// <param name="item">The item to add to the list.</param>
    public static void Add<T>(this scoped ref UnmanagedValueList<T> list, T item)
        where T : unmanaged
    {
        var count = list._count;
        var newCount = count + 1;

        list.EnsureCapacity(count + 1);

        list._count = newCount;
        list._items[count] = item;
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="list">The list to which the item should be added.</param>
    /// <param name="item">The item to add to the list.</param>
    /// <param name="mutex">The mutex to use when adding <paramref name="item" /> to the list.</param>
    public static void Add<T>(this ref UnmanagedValueList<T> list, T item, ValueMutex mutex)
        where T : unmanaged
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        list.Add(item);
    }

    /// <summary>Converts the backing array for the list to a span.</summary>
    /// <param name="list">The list which should be converted.</param>
    /// <returns>A span that covers the backing array for the list.</returns>
    /// <remarks>This method is because other operations may invalidate the backing array.</remarks>
    public static Span<T> AsSpanUnsafe<T>(this ref UnmanagedValueList<T> list)
        where T : unmanaged => list.AsUnmanagedSpanUnsafe().AsSpan();

    /// <summary>Converts the backing array for the list to a span starting at the specified index and continuing for the specified number of items.</summary>
    /// <param name="list">The list which should be converted.</param>
    /// <param name="start">The index of the first item to include in the span.</param>
    /// <param name="length">The number of items to include in the span.</param>
    /// <returns>A span that covers the backing array for the list starting at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    /// <remarks>
    ///     <para>This method is unsafe because other operations may invalidate the backing array.</para>
    ///     <para>This method is unsafe because it can give access to uninitialized memory in the backing array when <see cref="ValueList{T}.Count" /> is less than <see cref="ValueList{T}.Capacity" />.</para>
    /// </remarks>
    public static Span<T> AsSpanUnsafe<T>(this ref readonly UnmanagedValueList<T> list, nuint start, nuint length)
        where T : unmanaged => list._items.AsUnmanagedSpan().AsSpan(start, length);

    /// <summary>Converts the backing array for the list to an unmanaged span.</summary>
    /// <param name="list">The list which should be converted.</param>
    /// <returns>An unmanaged span that covers the backing array for the list.</returns>
    /// <remarks>This method is because other operations may invalidate the backing array.</remarks>
    public static UnmanagedSpan<T> AsUnmanagedSpanUnsafe<T>(this ref readonly UnmanagedValueList<T> list)
        where T : unmanaged => list._items.AsUnmanagedSpan(start: 0, length: list._count);

    /// <summary>Converts the backing array for the list to a span starting at the specified index and continuing for the specified number of items.</summary>
    /// <param name="list">The list which should be converted.</param>
    /// <param name="start">The index of the first item to include in the span.</param>
    /// <param name="length">The number of items to include in the span.</param>
    /// <returns>A span that covers the backing array for the list starting at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    /// <remarks>
    ///     <para>This method is unsafe because other operations may invalidate the backing array.</para>
    ///     <para>This method is unsafe because it can give access to uninitialized memory in the backing array when <see cref="ValueList{T}.Count" /> is less than <see cref="ValueList{T}.Capacity" />.</para>
    /// </remarks>
    public static UnmanagedSpan<T> AsUnmanagedSpanUnsafe<T>(this ref readonly UnmanagedValueList<T> list, nuint start, nuint length)
        where T : unmanaged => list._items.AsUnmanagedSpan(start, length);

    /// <summary>Removes all items from the list.</summary>
    /// <param name="list">The list which should be cleared.</param>
    public static void Clear<T>(this ref UnmanagedValueList<T> list)
        where T : unmanaged => list._count = 0;

    /// <summary>Checks whether the list contains a specified item.</summary>
    /// <param name="list">The list which should be checked.</param>
    /// <param name="item">The item to check for in the list.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the list; otherwise, <c>false</c>.</returns>
    public static bool Contains<T>(this ref readonly UnmanagedValueList<T> list, T item)
        where T : unmanaged => list.TryGetIndexOf(item, out _);

    /// <summary>Copies the items of the list to a span.</summary>
    /// <param name="list">The list which should be copied.</param>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedValueList{T}.Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public static void CopyTo<T>(this ref readonly UnmanagedValueList<T> list, UnmanagedSpan<T> destination)
        where T : unmanaged
    {
        var count = list._count;

        if (count != 0)
        {
            ThrowIfNotInInsertBounds(count, destination.Length);
            CopyArrayUnsafe(destination.GetPointerUnsafe(0), list._items.GetPointerUnsafe(0), count);
        }
    }

    /// <summary>Ensures the capacity of the list is at least the specified value.</summary>
    /// <param name="list">The list whose capacity should be ensured.</param>
    /// <param name="capacity">The minimum capacity the list should support.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnsureCapacity<T>(this ref UnmanagedValueList<T> list, nuint capacity)
        where T : unmanaged
    {
        var currentCapacity = list.Capacity;

        if (capacity > currentCapacity)
        {
            list.Resize(capacity, currentCapacity);
        }
    }

    /// <summary>Gets a pointer to the item at the specified index of the list.</summary>
    /// <param name="list">The list for which to get a pointer.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the list.</returns>
    /// <remarks>
    ///     <para>This method is because other operations may invalidate the backing array.</para>
    ///     <para>This method is because it does not validate that <paramref name="index" /> is less than <see cref="UnmanagedValueList{T}.Capacity" />.</para>
    /// </remarks>
    public static T* GetPointerUnsafe<T>(this ref readonly UnmanagedValueList<T> list, nuint index = 0)
        where T : unmanaged
    {
        AssertNotNull(list._items);
        Assert(index <= list.Capacity);
        return list._items.GetPointerUnsafe(index);
    }

    /// <summary>Gets a reference to the item at the specified index of the list.</summary>
    /// <param name="list">The list for which to get a reference.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the list.</returns>
    /// <remarks>
    ///     <para>This method is because other operations may invalidate the backing array.</para>
    ///     <para>This method is because it does not validate that <paramref name="index" /> is less than <see cref="UnmanagedValueList{T}.Capacity" />.</para>
    /// </remarks>
    public static ref T GetReferenceUnsafe<T>(this scoped ref readonly UnmanagedValueList<T> list, nuint index = 0)
        where T : unmanaged => ref *list.GetPointerUnsafe(index);

    /// <summary>Inserts an item into list at the specified index.</summary>
    /// <param name="list">The list to which the item should be inserted.</param>
    /// <param name="index">The zero-based index at which <paramref name="item" /> is inserted.</param>
    /// <param name="item">The item to insert into the list.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is negative or greater than <see cref="UnmanagedValueList{T}.Count" />.</exception>
    public static void Insert<T>(this ref UnmanagedValueList<T> list, nuint index, T item)
        where T : unmanaged
    {
        var count = list._count;
        ThrowIfNotInInsertBounds(index, count);

        var newCount = count + 1;
        list.EnsureCapacity(newCount);

        var items = list._items;

        if (index != newCount)
        {
            CopyArrayUnsafe(items.GetPointerUnsafe(index), items.GetPointerUnsafe(index + 1), count - index);
        }

        list._count = newCount;
        items[index] = item;
    }

    /// <summary>Removes the first occurrence of an item from the list.</summary>
    /// <param name="list">The list from which the item should be removed.</param>
    /// <param name="item">The item to remove from the list.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was removed from the list; otherwise, <c>false</c>.</returns>
    public static bool Remove<T>(this ref UnmanagedValueList<T> list, T item)
        where T : unmanaged
    {
        if (list.TryGetIndexOf(item, out var index))
        {
            list.RemoveAt(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>Removes the first occurrence of an item from the list.</summary>
    /// <param name="list">The list from which the item should be removed.</param>
    /// <param name="item">The item to remove from the list.</param>
    /// <param name="mutex">The mutex to use when removing <paramref name="item" /> from the list.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was removed from the list; otherwise, <c>false</c>.</returns>
    public static bool Remove<T>(this ref UnmanagedValueList<T> list, T item, ValueMutex mutex)
        where T : unmanaged
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return list.Remove(item);
    }

    /// <summary>Removes the item at the specified index from the list.</summary>
    /// <param name="list">The list from which the item should be removed.</param>
    /// <param name="index">The zero-based index of the item to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is negative or greater than or equal to <see cref="UnmanagedValueList{T}.Count" />.</exception>
    public static void RemoveAt<T>(this ref UnmanagedValueList<T> list, nuint index)
        where T : unmanaged
    {
        var count = list._count;
        ThrowIfNotInBounds(index, count);

        var newCount = count - 1;
        var items = list._items;

        if (index < newCount)
        {
            CopyArrayUnsafe(items.GetPointerUnsafe(index), items.GetPointerUnsafe(index + 1), newCount - index);
        }

        list._count = newCount;
    }

    /// <summary>Sets the number of items contained in the list.</summary>
    /// <param name="list">The list whose count should be set.</param>
    /// <param name="count">The new number of items contained in the list.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> is greater than <see cref="UnmanagedValueList{T}.Capacity" />.</exception>
    /// <remarks>
    ///     <para>This method allows you to explicitly shrink the list down to zero or grow it up to <see cref="UnmanagedValueList{T}.Capacity" />.</para>
    ///     <para>This method is because growing the count may leak uninitialized memory.</para>
    /// </remarks>
    public static void SetCountUnsafe<T>(this ref UnmanagedValueList<T> list, nuint count)
        where T : unmanaged
    {
        ThrowIfNotInInsertBounds(count, list.Capacity);
        list._count = count;
    }

    /// <summary>Trims any excess capacity, up to a given threshold, from the list.</summary>
    /// <param name="list">The list which should be trimmed.</param>
    /// <param name="threshold">A percentage, between <c>zero</c> and <c>one</c>, under which any excess will not be trimmed.</param>
    /// <remarks>This methods clamps <paramref name="threshold" /> to between <c>zero</c> and <c>one</c>, inclusive.</remarks>
    public static void TrimExcess<T>(this ref UnmanagedValueList<T> list, float threshold = 1.0f)
        where T : unmanaged
    {
        var count = list._count;
        var minCount = (nuint)(list.Capacity * Clamp(threshold, 0.0f, 1.0f));

        if (count < minCount)
        {
            var items = list._items;

            var alignment = !items.IsNull ? items.Alignment : 0;
            var newItems = new UnmanagedArray<T>(count, alignment);

            list.CopyTo(newItems);
            items.Dispose();

            list._items = newItems;
        }
    }

    /// <summary>Tries to get the index of a given item in the list.</summary>
    /// <param name="list">The list from which the index should be retrieved.</param>
    /// <param name="item">The item for which to get its index..</param>
    /// <param name="index">When <c>true</c> is returned, this contains the index of <paramref name="item" />.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the list; otherwise, <c>false</c>.</returns>
    public static bool TryGetIndexOf<T>(this ref readonly UnmanagedValueList<T> list, T item, out nuint index)
        where T : unmanaged
    {
        var items = list._items;

        if (!items.IsNull)
        {
            return TryGetIndexOfUnsafe(items.GetPointerUnsafe(0), list._count, item, out index);
        }
        else
        {
            index = 0;
            return false;
        }
    }

    internal static void Resize<T>(this ref UnmanagedValueList<T> list, nuint capacity, nuint currentCapacity)
        where T : unmanaged
    {
        var items = list._items;

        var newCapacity = Max(capacity, currentCapacity * 2);
        var alignment = !items.IsNull ? items.Alignment : 0;

        var newItems = new UnmanagedArray<T>(newCapacity, alignment);

        list.CopyTo(newItems);
        items.Dispose();

        list._items = newItems;
    }
}
