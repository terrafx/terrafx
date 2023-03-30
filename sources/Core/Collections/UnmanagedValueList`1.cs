// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the List<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TerraFX.Threading;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Collections;

/// <summary>Represents a list of unmanaged items that can be accessed by index.</summary>
/// <typeparam name="T">The type of the unmanaged items contained in the list.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
[DebuggerTypeProxy(typeof(UnmanagedValueList<>.DebugView))]
public unsafe partial struct UnmanagedValueList<T> : IDisposable, IEnumerable<T>
    where T : unmanaged
{
    /// <summary>Gets an empty list.</summary>
    public static UnmanagedValueList<T> Empty => new UnmanagedValueList<T>();

    private UnmanagedArray<T> _items;
    private nuint _count;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueList{T}" /> struct.</summary>
    public UnmanagedValueList()
    {
        _items = UnmanagedArray<T>.Empty;
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueList{T}" /> struct.</summary>
    /// <param name="capacity">The initial capacity of the list.</param>
    /// <param name="alignment">The alignment, in bytes, of the items in the list or <c>zero</c> to use the system default.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
    public UnmanagedValueList(nuint capacity, nuint alignment = 0)
    {
        if (capacity != 0)
        {
            _items = new UnmanagedArray<T>(capacity, alignment);
        }
        else
        {
            if (alignment != 0)
            {
                ThrowIfNotPow2(alignment);
            }
            _items = UnmanagedArray<T>.Empty;
        }
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueList{T}" /> struct.</summary>
    /// <param name="span">The span that is used to populate the list.</param>
    /// <param name="alignment">The alignment, in bytes, of the items in the list or <c>zero</c> to use the system default.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
    public UnmanagedValueList(UnmanagedReadOnlySpan<T> span, nuint alignment = 0)
    {
        if (span.Length != 0)
        {
            var items = new UnmanagedArray<T>(span.Length, alignment);
            CopyArrayUnsafe(items.GetPointerUnsafe(0), span.GetPointerUnsafe(0), span.Length);
            _items = items;
        }
        else
        {
            if (alignment != 0)
            {
                ThrowIfNotPow2(alignment);
            }
            _items = UnmanagedArray<T>.Empty;
        }

        _count = span.Length;
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueList{T}" /> struct.</summary>
    /// <param name="array">The array that is used to populate the list.</param>
    /// <param name="takeOwnership"><c>true</c> if the list should take ownership of the array; otherwise, <c>false</c>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
    /// <remarks>By default ownership of <paramref name="array" /> is given to the value list.</remarks>
    public UnmanagedValueList(UnmanagedArray<T> array, bool takeOwnership = true)
    {
        ThrowIfNull(array);

        if (takeOwnership)
        {
            _items = array;
        }
        else
        {
            var items = new UnmanagedArray<T>(array.Length, array.Alignment);
            CopyArrayUnsafe(items.GetPointerUnsafe(0), array.GetPointerUnsafe(0), array.Length);
            _items = items;
        }

        _count = array.Length;
    }

    /// <summary>Gets the number of items that can be contained by the list without being resized.</summary>
    public readonly nuint Capacity
    {
        get
        {
            var items = _items;
            return !items.IsNull ? items.Length : 0;
        }
    }

    /// <summary>Gets the number of items contained in the list.</summary>
    public readonly nuint Count => _count;

    /// <summary>Gets or sets the item at the specified index in the list.</summary>
    /// <param name="index">The index of the item to get or set.</param>
    /// <returns>The item that exists at <paramref name="index" /> in the list.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Count" />.</exception>
    public T this[nuint index]
    {
        readonly get
        {
            ThrowIfNotInBounds(index, _count);
            return _items[index];
        }

        set
        {
            ThrowIfNotInBounds(index, _count);
            _items[index] = value;
        }
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="item">The item to add to the list.</param>
    public void Add(T item)
    {
        var count = _count;
        var newCount = count + 1;

        EnsureCapacity(count + 1);

        _count = newCount;
        _items[count] = item;
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="item">The item to add to the list.</param>
    /// <param name="mutex">The mutex to use when adding <paramref name="item" /> to the list.</param>
    public void Add(T item, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        Add(item);
    }

    /// <summary>Converts the backing array for the list to a span.</summary>
    /// <returns>A span that covers the backing array for the list.</returns>
    public Span<T> AsSpanUnsafe() => AsUnmanagedSpanUnsafe().AsSpan();

    /// <summary>Converts the backing array for the list to an unmanaged span.</summary>
    /// <returns>An unmanaged span that covers the backing array for the list.</returns>
    /// <remarks>
    ///     <para>This method is because other operations may invalidate the backing array.</para>
    ///     <para>This method is because it gives access to uninitialized memory in the backing array when <see cref="Count" /> is less than <see cref="Capacity" />.</para>
    /// </remarks>
    public UnmanagedSpan<T> AsUnmanagedSpanUnsafe() => new UnmanagedSpan<T>(_items);

    /// <summary>Removes all items from the list.</summary>
    public void Clear() => _count = 0;

    /// <summary>Checks whether the list contains a specified item.</summary>
    /// <param name="item">The item to check for in the list.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the list; otherwise, <c>false</c>.</returns>
    public readonly bool Contains(T item) => TryGetIndexOf(item, out _);

    /// <summary>Copies the items of the list to a span.</summary>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public readonly void CopyTo(UnmanagedSpan<T> destination)
    {
        var count = _count;

        if (count != 0)
        {
            ThrowIfNotInInsertBounds(count, destination.Length);
            CopyArrayUnsafe(destination.GetPointerUnsafe(0), _items.GetPointerUnsafe(0), count);
        }
    }

    /// <inheritdoc />
    public void Dispose() => _items.Dispose();

    /// <summary>Ensures the capacity of the list is at least the specified value.</summary>
    /// <param name="capacity">The minimum capacity the list should support.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void EnsureCapacity(nuint capacity)
    {
        var currentCapacity = Capacity;

        if (capacity > currentCapacity)
        {
            Resize(capacity, currentCapacity);
        }
    }

    /// <summary>Gets an enumerator that can iterate through the items in the list.</summary>
    /// <returns>An enumerator that can iterate through the items in the list.</returns>
    public ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <summary>Gets a pointer to the item at the specified index of the list.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the list.</returns>
    /// <remarks>
    ///     <para>This method is because other operations may invalidate the backing array.</para>
    ///     <para>This method is because it does not validate that <paramref name="index" /> is less than <see cref="Capacity" />.</para>
    /// </remarks>
    public T* GetPointerUnsafe(nuint index)
    {
        AssertNotNull(_items);
        Assert(index <= Capacity);
        return _items.GetPointerUnsafe(index);
    }

    /// <summary>Gets a reference to the item at the specified index of the list.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the list.</returns>
    /// <remarks>
    ///     <para>This method is because other operations may invalidate the backing array.</para>
    ///     <para>This method is because it does not validate that <paramref name="index" /> is less than <see cref="Capacity" />.</para>
    /// </remarks>
    public ref T GetReferenceUnsafe(nuint index) => ref *GetPointerUnsafe(index);

    /// <summary>Inserts an item into list at the specified index.</summary>
    /// <param name="index">The zero-based index at which <paramref name="item" /> is inserted.</param>
    /// <param name="item">The item to insert into the list.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is negative or greater than <see cref="Count" />.</exception>
    public void Insert(nuint index, T item)
    {
        var count = _count;
        ThrowIfNotInInsertBounds(index, count);

        var newCount = count + 1;
        EnsureCapacity(newCount);

        var items = _items;

        if (index != newCount)
        {
            CopyArrayUnsafe(items.GetPointerUnsafe(index), items.GetPointerUnsafe(index + 1), count - index);
        }

        _count = newCount;
        items[index] = item;
    }

    /// <summary>Removes the first occurrence of an item from the list.</summary>
    /// <param name="item">The item to remove from the list.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was removed from the list; otherwise, <c>false</c>.</returns>
    public bool Remove(T item)
    {
        if (TryGetIndexOf(item, out var index))
        {
            RemoveAt(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>Removes the first occurrence of an item from the list.</summary>
    /// <param name="item">The item to remove from the list.</param>
    /// <param name="mutex">The mutex to use when removing <paramref name="item" /> from the list.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was removed from the list; otherwise, <c>false</c>.</returns>
    public bool Remove(T item, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return Remove(item);
    }

    /// <summary>Removes the item at the specified index from the list.</summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is negative or greater than or equal to <see cref="Count" />.</exception>
    public void RemoveAt(nuint index)
    {
        var count = _count;
        ThrowIfNotInBounds(index, count);

        var newCount = count - 1;
        var items = _items;

        if (index < newCount)
        {
            CopyArrayUnsafe(items.GetPointerUnsafe(index), items.GetPointerUnsafe(index + 1), newCount - index);
        }

        _count = newCount;
    }

    /// <summary>Sets the number of items contained in the list.</summary>
    /// <param name="count">The new number of items contained in the list.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> is greater than <see cref="Capacity" />.</exception>
    /// <remarks>
    ///     <para>This method allows you to explicitly shrink the list down to zero or grow it up to <see cref="Capacity" />.</para>
    ///     <para>This method is because growing the count may leak uninitialized memory.</para>
    /// </remarks>
    public void SetCountUnsafe(nuint count)
    {
        ThrowIfNotInInsertBounds(count, Capacity);
        _count = count;
    }

    /// <summary>Trims any excess capacity, up to a given threshold, from the list.</summary>
    /// <param name="threshold">A percentage, between <c>zero</c> and <c>one</c>, under which any excess will not be trimmed.</param>
    /// <remarks>This methods clamps <paramref name="threshold" /> to between <c>zero</c> and <c>one</c>, inclusive.</remarks>
    public void TrimExcess(float threshold = 1.0f)
    {
        var count = _count;
        var minCount = (nuint)(Capacity * Clamp(threshold, 0.0f, 1.0f));

        if (count < minCount)
        {
            var items = _items;

            var alignment = !items.IsNull ? items.Alignment : 0;
            var newItems = new UnmanagedArray<T>(count, alignment);

            CopyTo(newItems);
            items.Dispose();

            _items = newItems;
        }
    }

    /// <summary>Tries to get the index of a given item in the list.</summary>
    /// <param name="item">The item for which to get its index..</param>
    /// <param name="index">When <c>true</c> is returned, this contains the index of <paramref name="item" />.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the list; otherwise, <c>false</c>.</returns>
    public readonly bool TryGetIndexOf(T item, out nuint index)
    {
        var items = _items;

        if (!items.IsNull)
        {
            return TryGetIndexOfUnsafe(items.GetPointerUnsafe(0), _count, item, out index);
        }
        else
        {
            index = 0;
            return false;
        }
    }

    private void Resize(nuint capacity, nuint currentCapacity)
    {
        var items = _items;

        var newCapacity = Max(capacity, currentCapacity * 2);
        var alignment = !items.IsNull ? items.Alignment : 0;

        var newItems = new UnmanagedArray<T>(newCapacity, alignment);

        CopyTo(newItems);
        items.Dispose();

        _items = newItems;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
