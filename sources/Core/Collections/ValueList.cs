// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;
using TerraFX.Threading;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="ValueList{T}" /> struct.</summary>
public static class ValueList
{
    /// <summary>Gets an empty list.</summary>
    public static ValueList<T> Empty<T>() => new ValueList<T>();

    /// <summary>Adds an item to the list.</summary>
    /// <param name="list">The list to which the item should be added.</param>
    /// <param name="item">The item to add to the list.</param>
    public static void Add<T>(this ref ValueList<T> list, T item)
    {
        var count = list._count;
        var newCount = count + 1;

        list.EnsureCapacity(newCount);

        list._count = newCount;
        list._items[count] = item;
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="list">The list to which the item should be added.</param>
    /// <param name="item">The item to add to the list.</param>
    /// <param name="mutex">The mutex to use when adding <paramref name="item" /> to the list.</param>
    public static void Add<T>(this ref ValueList<T> list, T item, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        list.Add(item);
    }

    /// <summary>Converts the backing array for the list to a span.</summary>
    /// <param name="list">The list which should be converted.</param>
    /// <returns>A span that covers the backing array for the list.</returns>
    /// <remarks>This method is unsafe because other operations may invalidate the backing array.</remarks>
    public static Span<T> AsSpanUnsafe<T>(this ref readonly ValueList<T> list) => list._items.AsSpan(start: 0, length: list._count);

    /// <summary>Converts the backing array for the list to a span starting at the specified index and continuing for the specified number of items.</summary>
    /// <param name="list">The list which should be converted.</param>
    /// <param name="start">The index of the first item to include in the span.</param>
    /// <param name="length">The number of items to include in the span.</param>
    /// <returns>A span that covers the backing array for the list starting at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    /// <remarks>
    ///     <para>This method is unsafe because other operations may invalidate the backing array.</para>
    ///     <para>This method is unsafe because it can give access to uninitialized memory in the backing array when <see cref="ValueList{T}.Count" /> is less than <see cref="ValueList{T}.Capacity" />.</para>
    /// </remarks>
    public static Span<T> AsSpanUnsafe<T>(this ref readonly ValueList<T> list, int start, int length) => list._items.AsSpan(start, length);

    /// <summary>Removes all items from the list.</summary>
    /// <param name="list">The list which should be cleared.</param>
    public static void Clear<T>(this ref ValueList<T> list)
    {
        var items = list._items;

        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>() && (items is not null))
        {
            Array.Clear(items, 0, list._count);
        }

        list._count = 0;
    }

    /// <summary>Checks whether the list contains a specified item.</summary>
    /// <param name="list">The list which should be checked.</param>
    /// <param name="item">The item to check for in the list.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the list; otherwise, <c>false</c>.</returns>
    public static bool Contains<T>(this ref readonly ValueList<T> list, T item) => list.IndexOf(item) >= 0;

    /// <summary>Copies the items of the list to a span.</summary>
    /// <param name="list">The list which should be copied.</param>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentException"><see cref="ValueList{T}.Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public static void CopyTo<T>(this ref readonly ValueList<T> list, Span<T> destination) => list._items.AsSpan(0, list._count).CopyTo(destination);

    /// <summary>Ensures the capacity of the list is at least the specified value.</summary>
    /// <param name="list">The list whose capacity should be ensured.</param>
    /// <param name="capacity">The minimum capacity the list should support.</param>
    /// <remarks>This method does not throw if <paramref name="capacity" /> is negative and instead does nothing.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnsureCapacity<T>(this ref ValueList<T> list, int capacity)
    {
        var currentCapacity = list.Capacity;

        if (capacity > currentCapacity)
        {
            list.Resize(capacity, currentCapacity);
        }
    }

    /// <summary>Gets a reference to the item at the specified index of the list.</summary>
    /// <param name="list">The list from which to get the reference.</param>
    /// <param name="index">The index of the item to get a reference to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the list.</returns>
    /// <remarks>
    ///     <para>This method is unsafe because other operations may invalidate the backing array.</para>
    ///     <para>This method is unsafe because it does not validate that <paramref name="index" /> is less than <see cref="ValueList{T}.Capacity" />.</para>
    /// </remarks>
    public static ref T GetReferenceUnsafe<T>(this scoped ref readonly ValueList<T> list, int index)
    {
        AssertNotNull(list._items);
        Assert(unchecked((uint)index <= (uint)list.Capacity));
        return ref list._items.GetReferenceUnsafe(index);
    }

    /// <summary>Gets the index of the first occurrence of an item in the list.</summary>
    /// <param name="list">The list from which the index is to be retrieved.</param>
    /// <param name="item">The item to find in the list.</param>
    /// <returns>The index of <paramref name="item" /> if it was found in the list; otherwise, <c>-1</c>.</returns>
    public static int IndexOf<T>(this ref readonly ValueList<T> list, T item)
    {
        var items = list._items;
        return items is not null ? Array.IndexOf(items, item, 0, list._count) : -1;
    }

    /// <summary>Inserts an item into list at the specified index.</summary>
    /// <param name="list">The list to which the item should be inserted.</param>
    /// <param name="index">The zero-based index at which <paramref name="item" /> is inserted.</param>
    /// <param name="item">The item to insert into the list.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is negative or greater than <see cref="ValueList{T}.Count" />.</exception>
    public static void Insert<T>(this ref ValueList<T> list, int index, T item)
    {
        var count = list._count;
        ThrowIfNotInInsertBounds(index, count);

        var newCount = count + 1;
        list.EnsureCapacity(newCount);

        var items = list._items;

        if (index != newCount)
        {
            Array.Copy(items, index, items, index + 1, count - index);
        }

        list._count = newCount;
        items[index] = item;
    }

    /// <summary>Removes the first occurrence of an item from the list.</summary>
    /// <param name="list">The list from which the item should be removed.</param>
    /// <param name="item">The item to remove from the list.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was removed from the list; otherwise, <c>false</c>.</returns>
    public static bool Remove<T>(this ref ValueList<T> list, T item)
    {
        var index = list.IndexOf(item);

        if (index >= 0)
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
    public static bool Remove<T>(this ref ValueList<T> list, T item, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return list.Remove(item);
    }

    /// <summary>Removes the item at the specified index from the list.</summary>
    /// <param name="list">The list from which the item should be removed.</param>
    /// <param name="index">The zero-based index of the item to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is negative or greater than or equal to <see cref="ValueList{T}.Count" />.</exception>
    public static void RemoveAt<T>(this ref ValueList<T> list, int index)
    {
        var count = list._count;
        ThrowIfNotInBounds(index, count);

        var newCount = count - 1;
        var items = list._items;

        if (index < newCount)
        {
            Array.Copy(items, index + 1, items, index, newCount - index);
        }

        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            items[newCount] = default!;
        }

        list._count = newCount;
    }

    /// <summary>Sets the number of items contained in the list.</summary>
    /// <param name="list">The list whose count should be set.</param>
    /// <param name="count">The new number of items contained in the list.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> is <c>negative</c> or greater than <see cref="ValueList{T}.Capacity" />.</exception>
    /// <remarks>
    ///     <para>This method allows you to explicitly shrink the list down to zero or grow it up to <see cref="ValueList{T}.Capacity" />.</para>
    ///     <para>This method is unsafe because growing the count may leak uninitialized memory.</para>
    /// </remarks>
    public static void SetCountUnsafe<T>(this ref ValueList<T> list, int count)
    {
        ThrowIfNotInInsertBounds(count, list.Capacity);
        list._count = count;
    }

    /// <summary>Trims any excess capacity, up to a given threshold, from the list.</summary>
    /// <param name="list">The list which should be trimmed.</param>
    /// <param name="threshold">A percentage, between <c>zero</c> and <c>one</c>, under which any excess will not be trimmed.</param>
    /// <remarks>This methods clamps <paramref name="threshold" /> to between <c>zero</c> and <c>one</c>, inclusive.</remarks>
    public static void TrimExcess<T>(this ref ValueList<T> list, float threshold = 1.0f)
    {
        var count = list._count;
        var minCount = (int)(list.Capacity * Clamp(threshold, 0.0f, 1.0f));

        if (count < minCount)
        {
            var newItems = GC.AllocateUninitializedArray<T>(count);
            list.CopyTo(newItems);
            list._items = newItems;
        }
    }

    internal static void Resize<T>(this ref ValueList<T> list, int capacity, int currentCapacity)
    {
        var newCapacity = Max(capacity, currentCapacity * 2);
        var newItems = GC.AllocateUninitializedArray<T>(newCapacity);

        list.CopyTo(newItems);
        list._items = newItems;
    }
}
