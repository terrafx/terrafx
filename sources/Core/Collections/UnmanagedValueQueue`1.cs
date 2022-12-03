// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Queue<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Collections;

/// <summary>Represents a queue of unmanaged items.</summary>
/// <typeparam name="T">The type of the unmanaged items contained in the queue.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
[DebuggerTypeProxy(typeof(UnmanagedValueQueue<>.DebugView))]
public unsafe partial struct UnmanagedValueQueue<T> : IDisposable, IEnumerable<T>
    where T : unmanaged
{
    /// <summary>Gets an empty queue.</summary>
    public static UnmanagedValueQueue<T> Empty => new UnmanagedValueQueue<T>();

    private UnmanagedArray<T> _items;
    private nuint _count;
    private nuint _head;
    private nuint _tail;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueQueue{T}" /> struct.</summary>
    public UnmanagedValueQueue()
    {
        _items = UnmanagedArray<T>.Empty;
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueQueue{T}" /> struct.</summary>
    /// <param name="capacity">The initial capacity of the queue.</param>
    /// <param name="alignment">The alignment, in bytes, of the items in the queue or <c>zero</c> to use the system default.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
    public UnmanagedValueQueue(nuint capacity, nuint alignment = 0)
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

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueQueue{T}" /> struct.</summary>
    /// <param name="span">The span that is used to populate the queue.</param>
    /// <param name="alignment">The alignment, in bytes, of the items in the queue or <c>zero</c> to use the system default.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
    public UnmanagedValueQueue(UnmanagedReadOnlySpan<T> span, nuint alignment = 0)
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

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueQueue{T}" /> struct.</summary>
    /// <param name="array">The array that is used to populate the queue.</param>
    /// <param name="takeOwnership"><c>true</c> if the queue should take ownership of the array; otherwise, <c>false</c>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
    /// <remarks>By default ownership of <paramref name="array" /> is given to the value queue.</remarks>
    public UnmanagedValueQueue(UnmanagedArray<T> array, bool takeOwnership = true)
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

    /// <summary>Gets the number of items that can be contained by the queue without being resized.</summary>
    public readonly nuint Capacity
    {
        get
        {
            var items = _items;
            return !items.IsNull ? items.Length : 0;
        }
    }

    /// <summary>Gets the number of items contained in the queue.</summary>
    public readonly nuint Count => _count;

    /// <summary>Removes all items from the queue.</summary>
    public void Clear()
    {
        _count = 0;
        _head = 0;
        _tail = 0;
    }

    /// <summary>Checks whether the queue contains a specified item.</summary>
    /// <param name="item">The item to check for in the queue.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the queue; otherwise, <c>false</c>.</returns>
    public readonly bool Contains(T item)
    {
        var items = _items;

        if (!items.IsNull)
        {
            var head = _head;
            var tail = _tail;

            if ((head < tail) || (tail == 0))
            {
                return TryGetIndexOfUnsafe(items.GetPointerUnsafe(head), _count, item, out _);
            }
            else
            {
                return TryGetIndexOfUnsafe(items.GetPointerUnsafe(head), _count - head, item, out _)
                    || TryGetIndexOfUnsafe(items.GetPointerUnsafe(0), tail, item, out _);
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>Copies the items of the queue to a span.</summary>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public readonly void CopyTo(UnmanagedSpan<T> destination)
    {
        var count = _count;

        if (count != 0)
        {
            ThrowIfNotInInsertBounds(count, destination.Length);

            var head = _head;
            var tail = _tail;

            if ((head < tail) || (tail == 0))
            {
                CopyArrayUnsafe(destination.GetPointerUnsafe(0), _items.GetPointerUnsafe(head), count);
            }
            else
            {
                var items = _items;
                var headLength = count - head;

                CopyArrayUnsafe(destination.GetPointerUnsafe(0), items.GetPointerUnsafe(head), headLength);
                CopyArrayUnsafe(destination.GetPointerUnsafe(headLength), items.GetPointerUnsafe(0), tail);
            }
        }
    }

    /// <inheritdoc />
    public void Dispose() => _items.Dispose();

    /// <summary>Dequeues the item from the head of the queue.</summary>
    /// <returns>The item at the head of the queue.</returns>
    /// <exception cref="InvalidOperationException">The queue is empty.</exception>
    public T Dequeue()
    {
        if (!TryDequeue(out var item))
        {
            ThrowForEmptyQueue();
        }
        return item;
    }

    /// <summary>Enqueues an item to the tail of the queue.</summary>
    /// <param name="item">The item to enqueue to the tail of the queue.</param>
    public void Enqueue(T item)
    {
        var count = _count;
        var newCount = count + 1;

        if (newCount > Capacity)
        {
            EnsureCapacity(count + 1);
        }

        var tail = _tail;
        var newTail = tail + 1;

        _count = newCount;
        _items[tail] = item;

        if (newTail == Capacity)
        {
            newTail = 0;
        }
        _tail = newTail;
    }

    /// <summary>Ensures the capacity of the queue is at least the specified value.</summary>
    /// <param name="capacity">The minimum capacity the queue should support.</param>
    public void EnsureCapacity(nuint capacity)
    {
        var currentCapacity = Capacity;

        if (capacity > currentCapacity)
        {
            var items = _items;

            var newCapacity = Max(capacity, currentCapacity * 2);
            var alignment = !items.IsNull ? items.Alignment : 0;

            var newItems = new UnmanagedArray<T>(newCapacity, alignment);

            CopyTo(newItems);
            items.Dispose();

            _items = newItems;

            _head = 0;
            _tail = _count;
        }
    }

    /// <summary>Gets an enumerator that can iterate through the items in the list.</summary>
    /// <returns>An enumerator that can iterate through the items in the list.</returns>
    public ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <summary>Gets a pointer to the item at the specified index of the queue.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the queue.</returns>
    /// <remarks>This method is because other operations may invalidate the backing data.</remarks>
    public T* GetPointerUnsafe(nuint index)
    {
        T* item;
        var count = _count;

        if (index < count)
        {
            var head = _head;
            var headLength = count - head;

            if ((head < _tail) || (index < headLength))
            {
                item = _items.GetPointerUnsafe(head + index);
            }
            else
            {
                item = _items.GetPointerUnsafe(index - headLength);
            }
        }
        else
        {
            item = null;
        }

        return item;
    }

    /// <summary>Gets a reference to the item at the specified index of the list.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the list.</returns>
    /// <remarks>
    ///     <para>This method is because other operations may invalidate the backing array.</para>
    ///     <para>This method is because it does not validate that <paramref name="index" /> is less than <see cref="Count" />.</para>
    /// </remarks>
    public ref T GetReferenceUnsafe(nuint index) => ref *GetPointerUnsafe(index);

    /// <summary>Peeks at item at the head of the queue.</summary>
    /// <returns>The item at the head of the queue.</returns>
    /// <exception cref="InvalidOperationException">The queue is empty.</exception>
    public readonly T Peek()
    {
        if (!TryPeek(out var item))
        {
            ThrowForEmptyQueue();
        }
        return item;
    }

    /// <summary>Peeks at item at the specified index of the queue.</summary>
    /// <param name="index">The index from the head of the queue of the item at which to peek.</param>
    /// <returns>The item at the specified index of the queue.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Count" />.</exception>
    public readonly T Peek(nuint index)
    {
        if (!TryPeek(index, out var item))
        {
            ThrowIfNotInBounds(index, _count);
        }
        return item!;
    }

    /// <summary>Removes the first occurence of an item from the queue.</summary>
    /// <param name="item">The item to remove from the queue.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was removed from the queue; otherwise, <c>false</c>.</returns>
    public bool Remove(T item)
    {
        var items = _items;

        if (!items.IsNull)
        {
            var count = _count;
            var head = _head;
            var tail = _tail;

            if (TryGetIndexOfUnsafe(items.GetPointerUnsafe(head), count - head, item, out var index) || TryGetIndexOfUnsafe(items.GetPointerUnsafe(0), tail, item, out index))
            {
                var newTail = unchecked(tail - 1);
                var newCount = count - 1;

                CopyArrayUnsafe(items.GetPointerUnsafe(index), items.GetPointerUnsafe(index + 1), newCount - index);

                if (tail == 0)
                {
                    newTail = newCount;
                }
                else if (head >= tail)
                {
                    items[newCount] = items[0];
                    CopyArrayUnsafe(items.GetPointerUnsafe(0), items.GetPointerUnsafe(1), newTail);
                }

                _tail = newTail;
                _count = newCount;

                return true;
            }
        }

        return false;
    }

    /// <summary>Trims any excess capacity, up to a given threshold, from the queue.</summary>
    /// <param name="threshold">A percentage, between <c>zero</c> and <c>one</c>, under which any exceess will not be trimmed.</param>
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

            _head = 0;
            _tail = count;
        }
    }

    /// <summary>Tries to dequeue an item from the head of the queue.</summary>
    /// <param name="item">When <c>true</c> is returned, this contains the item from the head of the queue.</param>
    /// <returns><c>true</c> if the queue was not empty; otherwise, <c>false</c>.</returns>
    public bool TryDequeue(out T item)
    {
        var count = _count;
        var newCount = unchecked(count - 1);

        if (count == 0)
        {
            item = default!;
            return false;
        }

        var head = _head;
        var newHead = head + 1;

        _count = newCount;

        if (newHead == Capacity)
        {
            newHead = 0;
        }
        _head = newHead;

        item = _items[head];
        return true;
    }

    /// <summary>Tries to peek at the item at the head of the queue.</summary>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the head of the queue.</param>
    /// <returns><c>true</c> if the queue was not empty; otherwise, <c>false</c>.</returns>
    public readonly bool TryPeek(out T item)
    {
        if (_count != 0)
        {
            item = _items[_head];
            return true;
        }
        else
        {
            item = default!;
            return false;
        }
    }

    /// <summary>Tries to peek at the item at the head of the queue.</summary>
    /// <param name="index">The index from the head of the queue of the item at which to peek.</param>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the head of the queue.</param>
    /// <returns><c>true</c> if the queue was not empty and <paramref name="index" /> is less than <see cref="Count" />; otherwise, <c>false</c>.</returns>
    public readonly bool TryPeek(nuint index, out T item)
    {
        var count = _count;

        if (index < count)
        {
            var head = _head;

            if ((head < _tail) || (index < (count - head)))
            {
                item = _items[head + index];
            }
            else
            {
                item = _items[index];
            }
            return true;
        }
        else
        {
            item = default!;
            return false;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
