// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the List<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Collections;

/// <summary>Represents a list of unmanaged items that can be accessed by index.</summary>
/// <typeparam name="T">The type of the unmanaged items contained in the list.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
[DebuggerTypeProxy(typeof(UnmanagedValueList<>.DebugView))]
public partial struct UnmanagedValueList<T> : IDisposable
    where T : unmanaged
{
    private UnmanagedArray<T> _items;
    private nuint _count;
    private nuint _version;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueList{T}" /> struct.</summary>
    /// <param name="capacity">The initial capacity of the list.</param>
    /// <param name="alignment">The alignment, in bytes, of the items in the list or <c>zero</c> to use the system default.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
    public UnmanagedValueList(nuint capacity, nuint alignment = 0)
    {
        if (capacity != 0)
        {
            _items = new UnmanagedArray<T>(capacity, alignment, zero: false);
        }
        else
        {
            if (alignment != 0)
            {
                ThrowIfNotPow2(alignment);
            }
            _items = UnmanagedArray<T>.Empty;
        }

        _count = 0;
        _version = 0;
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueList{T}" /> struct.</summary>
    /// <param name="span">The span that is used to populate the list.</param>
    /// <param name="alignment">The alignment, in bytes, of the items in the list or <c>zero</c> to use the system default.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
    public unsafe UnmanagedValueList(UnmanagedReadOnlySpan<T> span, nuint alignment = 0)
    {
        if (span.Length != 0)
        {
            var items = new UnmanagedArray<T>(span.Length, alignment, zero: false);
            CopyArrayUnsafe<T>(items.GetPointerUnsafe(0), span.GetPointerUnsafe(0), span.Length);
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
        _version = 0;
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueList{T}" /> struct.</summary>
    /// <param name="array">The array that is used to populate the list.</param>
    /// <param name="takeOwnership"><c>true</c> if the list should take ownership of the array; otherwise, <c>false</c>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
    public unsafe UnmanagedValueList(UnmanagedArray<T> array, bool takeOwnership = false)
    {
        ThrowIfNull(array);

        if (takeOwnership)
        {
            _items = array;
        }
        else
        {
            var items = new UnmanagedArray<T>(array.Length, array.Alignment, zero: false);
            CopyArrayUnsafe<T>(items.GetPointerUnsafe(0), array.GetPointerUnsafe(0), array.Length);
            _items = items;
        }

        _count = array.Length;
        _version = 0;
    }

    /// <summary>Gets the number of items that can be contained by the list without being resized.</summary>
    public readonly nuint Capacity
    {
        get
        {
            var items = _items;

            if (!items.IsNull)
            {
                return _items.Length;
            }
            else
            {
                return 0;
            }
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
            ThrowIfNotInBounds(index, Count, nameof(index), nameof(Count));
            return _items[index];
        }

        set
        {
            ThrowIfNotInBounds(index, Count, nameof(index), nameof(Count));
            _version++;

            _items[index] = value;
        }
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="item">The item to add to the list.</param>
    public void Add(T item)
    {
        var count = Count;
        var newCount = count + 1;

        if (newCount <= Capacity)
        {
            _version++;
        }
        else
        {
            EnsureCapacity(count + 1);
        }

        _count = newCount;
        _items[count] = item;
    }

    /// <summary>Removes all items from the list.</summary>
    public void Clear()
    {
        _version++;
        _count = 0;
    }

    /// <summary>Checks whether the list contains a specified item.</summary>
    /// <param name="item">The item to check for in the list.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the list; otherwise, <c>false</c>.</returns>
    public readonly bool Contains(T item) => TryGetIndexOf(item, out _);

    /// <summary>Copies the items of the list to a span.</summary>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public readonly unsafe void CopyTo(UnmanagedSpan<T> destination)
    {
        var count = Count;

        if (count != 0)
        {
            ThrowIfNotInInsertBounds(count, destination.Length, nameof(Count), nameof(destination));
            CopyArrayUnsafe<T>(destination.GetPointerUnsafe(0), _items.GetPointerUnsafe(0), count);
        }
    }

    /// <inheritdoc />
    public void Dispose() => _items.Dispose();

    /// <summary>Ensures the capacity of the list is at least the specified value.</summary>
    /// <param name="capacity">The minimum capacity the list should support.</param>
    public unsafe void EnsureCapacity(nuint capacity)
    {
        var currentCapacity = Capacity;

        if (capacity > currentCapacity)
        {
            var items = _items;

            var newCapacity = Max(capacity, currentCapacity * 2);
            var alignment = !items.IsNull ? items.Alignment : 0;

            var newItems = new UnmanagedArray<T>(newCapacity, alignment, zero: false);

            CopyTo(newItems);
            items.Dispose();

            _version++;
            _items = newItems;
        }
    }

    /// <summary>Inserts an item into list at the specified index.</summary>
    /// <param name="index">The zero-based index at which <paramref name="item" /> is inserted.</param>
    /// <param name="item">The item to insert into the list.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is negative or greater than <see cref="Count" />.</exception>
    public void Insert(nuint index, T item)
    {
        var count = Count;
        ThrowIfNotInInsertBounds(index, count, nameof(index), nameof(Count));

        if (index != count)
        {
            _version++;
        }
        else
        {
            var newCount = count + 1;
            EnsureCapacity(newCount);
            _count = newCount;
        }

        _items[index] = item;
    }

    /// <summary>Removes the first occurence of an item from the list.</summary>
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

    /// <summary>Removes the item at the specified index from the list.</summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is negative or greater than or equal to <see cref="Count" />.</exception>
    public unsafe void RemoveAt(nuint index)
    {
        var count = Count;
        ThrowIfNotInBounds(index, count, nameof(index), nameof(Count));

        var newCount = count - 1;
        var items = _items;

        _version++;

        if (index < newCount)
        {
            CopyArrayUnsafe<T>(items.GetPointerUnsafe(index), items.GetPointerUnsafe(index + 1), newCount - index);
        }

        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            items[newCount] = default!;
        }

        _count = newCount;
    }

    /// <summary>Trims any excess capacity, up to a given threshold, from the list.</summary>
    /// <param name="threshold">A percentage, between <c>zero</c> and <c>one</c>, under which any exceess will not be trimmed.</param>
    /// <remarks>This methods clamps <paramref name="threshold" /> to between <c>zero</c> and <c>one</c>, inclusive.</remarks>
    public void TrimExcess(float threshold = 1.0f)
    {
        var count = Count;
        var minCount = (nuint)(Capacity * Clamp(threshold, 0.0f, 1.0f));

        if (count < minCount)
        {
            var items = _items;

            var alignment = !items.IsNull ? items.Alignment : 0;
            var newItems = new UnmanagedArray<T>(count, alignment, zero: false);

            CopyTo(newItems);
            items.Dispose();

            _version++;
            _items = newItems;
        }
    }

    /// <summary>Tries to get the index of a given item in the list.</summary>
    /// <param name="item">The item for which to get its index..</param>
    /// <param name="index">When <c>true</c> is returned, this contains the index of <paramref name="item" />.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the list; otherwise, <c>false</c>.</returns>
    public readonly unsafe bool TryGetIndexOf(T item, out nuint index)
    {
        var items = _items;

        if (!items.IsNull)
        {
            return TryGetIndexOfUnsafe(items.GetPointerUnsafe(0), Count, item, out index);
        }
        else
        {
            index = 0;
            return false;
        }
    }
}
