// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Queue<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.MemoryUtilities;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="UnmanagedValueQueue{T}" /> struct.</summary>
public static unsafe class UnmanagedValueQueue
{
    /// <summary>Gets an empty queue.</summary>
    public static UnmanagedValueQueue<T> Empty<T>()
        where T : unmanaged => new UnmanagedValueQueue<T>();

    /// <summary>Removes all items from the queue.</summary>
    /// <param name="queue">The queue which should be cleared.</param>
    public static void Clear<T>(this ref UnmanagedValueQueue<T> queue)
        where T : unmanaged
    {
        queue._count = 0;
        queue._head = 0;
        queue._tail = 0;
    }

    /// <summary>Checks whether the queue contains a specified item.</summary>
    /// <param name="queue">The queue which should be checked.</param>
    /// <param name="item">The item to check for in the queue.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the queue; otherwise, <c>false</c>.</returns>
    public static bool Contains<T>(this ref readonly UnmanagedValueQueue<T> queue, T item)
        where T : unmanaged
    {
        var items = queue._items;

        if (!items.IsNull)
        {
            var head = queue._head;
            var tail = queue._tail;

            if ((head < tail) || (tail == 0))
            {
                return TryGetIndexOfUnsafe(items.GetPointerUnsafe(head), queue._count, item, out _);
            }
            else
            {
                return TryGetIndexOfUnsafe(items.GetPointerUnsafe(head), queue._count - head, item, out _)
                    || TryGetIndexOfUnsafe(items.GetPointerUnsafe(0), tail, item, out _);
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>Copies the items of the queue to a span.</summary>
    /// <param name="queue">The queue which should be copied.</param>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedValueQueue{T}.Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public static void CopyTo<T>(this ref readonly UnmanagedValueQueue<T> queue, UnmanagedSpan<T> destination)
        where T : unmanaged
    {
        var count = queue._count;

        if (count != 0)
        {
            ThrowIfNotInInsertBounds(count, destination.Length);

            var head = queue._head;
            var tail = queue._tail;

            if ((head < tail) || (tail == 0))
            {
                CopyArrayUnsafe(destination.GetPointerUnsafe(0), queue._items.GetPointerUnsafe(head), count);
            }
            else
            {
                var items = queue._items;
                var headLength = count - head;

                CopyArrayUnsafe(destination.GetPointerUnsafe(0), items.GetPointerUnsafe(head), headLength);
                CopyArrayUnsafe(destination.GetPointerUnsafe(headLength), items.GetPointerUnsafe(0), tail);
            }
        }
    }

    /// <summary>Dequeues the item from the head of the queue.</summary>
    /// <param name="queue">The queue from which the item should be dequeued.</param>
    /// <returns>The item at the head of the queue.</returns>
    /// <exception cref="InvalidOperationException">The queue is empty.</exception>
    public static T Dequeue<T>(this ref UnmanagedValueQueue<T> queue)
        where T : unmanaged
    {
        if (!queue.TryDequeue(out var item))
        {
            ThrowForEmptyQueue();
        }
        return item;
    }

    /// <summary>Enqueues an item to the tail of the queue.</summary>
    /// <param name="queue">The queue from which the item should be enqueued.</param>
    /// <param name="item">The item to enqueue to the tail of the queue.</param>
    public static void Enqueue<T>(this ref UnmanagedValueQueue<T> queue, T item)
        where T : unmanaged
    {
        var count = queue._count;
        var newCount = count + 1;

        queue.EnsureCapacity(count + 1);

        var tail = queue._tail;
        var newTail = tail + 1;

        queue._count = newCount;
        queue._items[tail] = item;

        if (newTail == queue.Capacity)
        {
            newTail = 0;
        }
        queue._tail = newTail;
    }

    /// <summary>Ensures the capacity of the queue is at least the specified value.</summary>
    /// <param name="queue">The queue whose capacity should be ensured.</param>
    /// <param name="capacity">The minimum capacity the queue should support.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnsureCapacity<T>(this ref UnmanagedValueQueue<T> queue, nuint capacity)
        where T : unmanaged
    {
        var currentCapacity = queue.Capacity;

        if (capacity > currentCapacity)
        {
            queue.Resize(capacity, currentCapacity);
        }
    }

    /// <summary>Gets a pointer to the item at the specified index of the queue.</summary>
    /// <param name="queue">The queue from which to get a pointer.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the queue.</returns>
    /// <remarks>This method is because other operations may invalidate the backing data.</remarks>
    public static T* GetPointerUnsafe<T>(this ref readonly UnmanagedValueQueue<T> queue, nuint index)
        where T : unmanaged
    {
        T* item;
        var count = queue._count;

        if (index < count)
        {
            var head = queue._head;
            var headLength = count - head;

            var actualIndex = ((head < queue._tail) || (index < headLength)) ? (head + index) : (index - headLength);
            item = queue._items.GetPointerUnsafe(actualIndex);
        }
        else
        {
            item = null;
        }

        return item;
    }

    /// <summary>Gets a reference to the item at the specified index of the list.</summary>
    /// <param name="queue">The queue from which to get a reference.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the list.</returns>
    /// <remarks>
    ///     <para>This method is because other operations may invalidate the backing array.</para>
    ///     <para>This method is because it does not validate that <paramref name="index" /> is less than <see cref="UnmanagedValueQueue{T}.Count" />.</para>
    /// </remarks>
    public static ref T GetReferenceUnsafe<T>(this scoped ref readonly UnmanagedValueQueue<T> queue, nuint index)
        where T : unmanaged => ref *queue.GetPointerUnsafe(index);

    /// <summary>Peeks at item at the head of the queue.</summary>
    /// <param name="queue">The queue which should be peeked.</param>
    /// <returns>The item at the head of the queue.</returns>
    /// <exception cref="InvalidOperationException">The queue is empty.</exception>
    public static T Peek<T>(this ref readonly UnmanagedValueQueue<T> queue)
        where T : unmanaged
    {
        if (!queue.TryPeek(out var item))
        {
            ThrowForEmptyQueue();
        }
        return item;
    }

    /// <summary>Peeks at item at the specified index of the queue.</summary>
    /// <param name="queue">The queue which should be peeked.</param>
    /// <param name="index">The index from the head of the queue of the item at which to peek.</param>
    /// <returns>The item at the specified index of the queue.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="UnmanagedValueQueue{T}.Count" />.</exception>
    public static T Peek<T>(this ref readonly UnmanagedValueQueue<T> queue, nuint index)
        where T : unmanaged
    {
        if (!queue.TryPeek(index, out var item))
        {
            ThrowIfNotInBounds(index, queue._count);
        }
        return item!;
    }

    /// <summary>Removes the first occurrence of an item from the queue.</summary>
    /// <param name="queue">The queue from which the item should be removed.</param>
    /// <param name="item">The item to remove from the queue.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was removed from the queue; otherwise, <c>false</c>.</returns>
    public static bool Remove<T>(this ref UnmanagedValueQueue<T> queue, T item)
        where T : unmanaged
    {
        var items = queue._items;

        if (!items.IsNull)
        {
            var count = queue._count;
            var head = queue._head;
            var tail = queue._tail;

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

                queue._tail = newTail;
                queue._count = newCount;

                return true;
            }
        }

        return false;
    }

    /// <summary>Trims any excess capacity, up to a given threshold, from the queue.</summary>
    /// <param name="queue">The queue which should be trimmed.</param>
    /// <param name="threshold">A percentage, between <c>zero</c> and <c>one</c>, under which any excess will not be trimmed.</param>
    /// <remarks>This methods clamps <paramref name="threshold" /> to between <c>zero</c> and <c>one</c>, inclusive.</remarks>
    public static void TrimExcess<T>(this ref UnmanagedValueQueue<T> queue, float threshold = 1.0f)
        where T : unmanaged
    {
        var count = queue._count;
        var minCount = (nuint)(queue.Capacity * Clamp(threshold, 0.0f, 1.0f));

        if (count < minCount)
        {
            var items = queue._items;

            var alignment = !items.IsNull ? items.Alignment : 0;
            var newItems = new UnmanagedArray<T>(count, alignment);

            queue.CopyTo(newItems);
            items.Dispose();

            queue._items = newItems;

            queue._head = 0;
            queue._tail = count;
        }
    }

    /// <summary>Tries to dequeue an item from the head of the queue.</summary>
    /// <param name="queue">The queue from which the item should be dequeued.</param>
    /// <param name="item">When <c>true</c> is returned, this contains the item from the head of the queue.</param>
    /// <returns><c>true</c> if the queue was not empty; otherwise, <c>false</c>.</returns>
    public static bool TryDequeue<T>(this ref UnmanagedValueQueue<T> queue, out T item)
        where T : unmanaged
    {
        var count = queue._count;
        var newCount = unchecked(count - 1);

        if (count == 0)
        {
            item = default!;
            return false;
        }

        var head = queue._head;
        var newHead = head + 1;

        queue._count = newCount;

        if (newHead == queue.Capacity)
        {
            newHead = 0;
        }
        queue._head = newHead;

        item = queue._items[head];
        return true;
    }

    /// <summary>Tries to peek at the item at the head of the queue.</summary>
    /// <param name="queue">The queue which should be peeked.</param>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the head of the queue.</param>
    /// <returns><c>true</c> if the queue was not empty; otherwise, <c>false</c>.</returns>
    public static bool TryPeek<T>(this ref readonly UnmanagedValueQueue<T> queue, out T item)
        where T : unmanaged
    {
        if (queue._count != 0)
        {
            item = queue._items[queue._head];
            return true;
        }
        else
        {
            item = default!;
            return false;
        }
    }

    /// <summary>Tries to peek at the item at the head of the queue.</summary>
    /// <param name="queue">The queue which should be peeked.</param>
    /// <param name="index">The index from the head of the queue of the item at which to peek.</param>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the head of the queue.</param>
    /// <returns><c>true</c> if the queue was not empty and <paramref name="index" /> is less than <see cref="UnmanagedValueQueue{T}.Count" />; otherwise, <c>false</c>.</returns>
    public static bool TryPeek<T>(this ref readonly UnmanagedValueQueue<T> queue, nuint index, out T item)
        where T : unmanaged
    {
        var count = queue._count;

        if (index < count)
        {
            var head = queue._head;

            var actualIndex = ((head < queue._tail) || (index < (count - head))) ? (head + index) : index;
            item = queue._items[actualIndex];

            return true;
        }
        else
        {
            item = default!;
            return false;
        }
    }

    internal static void Resize<T>(this ref UnmanagedValueQueue<T> queue, nuint capacity, nuint currentCapacity)
        where T : unmanaged
    {
        var items = queue._items;

        var newCapacity = Max(capacity, currentCapacity * 2);
        var alignment = !items.IsNull ? items.Alignment : 0;

        var newItems = new UnmanagedArray<T>(newCapacity, alignment);

        queue.CopyTo(newItems);
        items.Dispose();

        queue._items = newItems;

        queue._head = 0;
        queue._tail = queue._count;
    }
}
