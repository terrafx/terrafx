// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="ValueQueue{T}" /> struct.</summary>
public static class ValueQueue
{
    /// <summary>Gets an empty queue.</summary>
    public static ValueQueue<T> Empty<T>() => new ValueQueue<T>();

    /// <summary>Removes all items from the queue.</summary>
    /// <param name="queue">The queue which should be cleared.</param>
    public static void Clear<T>(this ref ValueQueue<T> queue)
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>() && (queue._items is not null))
        {
            var head = queue._head;
            var tail = queue._tail;

            if ((head < tail) || (tail == 0))
            {
                Array.Clear(queue._items, head, queue._count);
            }
            else
            {
                var items = queue._items;

                Array.Clear(items, head, queue._count - head);
                Array.Clear(items, 0, tail);
            }
        }

        queue._count = 0;
        queue._head = 0;
        queue._tail = 0;
    }

    /// <summary>Checks whether the queue contains a specified item.</summary>
    /// <param name="queue">The queue which should be checked.</param>
    /// <param name="item">The item to check for in the queue.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the queue; otherwise, <c>false</c>.</returns>
    public static bool Contains<T>(this ref readonly ValueQueue<T> queue, T item)
    {
        var items = queue._items;

        if (items is not null)
        {
            var head = queue._head;
            var tail = queue._tail;

            if ((head < tail) || (tail == 0))
            {
                return Array.IndexOf(items, item, head, queue._count) >= 0;
            }
            else
            {
                return (Array.IndexOf(items, item, head, queue._count - head) >= 0)
                    || (Array.IndexOf(items, item, 0, tail) >= 0);
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
    /// <exception cref="ArgumentException"><see cref="ValueQueue{T}.Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public static void CopyTo<T>(this ref readonly ValueQueue<T> queue, Span<T> destination)
    {
        var count = queue._count;
        var items = queue._items;

        var head = queue._head;
        var tail = queue._tail;

        if ((head < tail) || (tail == 0))
        {
            items.AsSpan(head, count).CopyTo(destination);
        }
        else
        {
            var headLength = count - head;
            items.AsSpan(head, headLength).CopyTo(destination);
            items.AsSpan(0, tail).CopyTo(destination[headLength..]);
        }
    }

    /// <summary>Dequeues the item from the head of the queue.</summary>
    /// <param name="queue">The queue from which the item should be dequeued.</param>
    /// <returns>The item at the head of the queue.</returns>
    /// <exception cref="InvalidOperationException">The queue is empty.</exception>
    public static T Dequeue<T>(this ref ValueQueue<T> queue)
    {
        if (!queue.TryDequeue(out var item))
        {
            ThrowForEmptyQueue();
        }
        return item;
    }

    /// <summary>Enqueues an item to the tail of the queue.</summary>
    /// <param name="queue">The queue to which the item should be enqueued.</param>
    /// <param name="item">The item to enqueue to the tail of the queue.</param>
    public static void Enqueue<T>(this ref ValueQueue<T> queue, T item)
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
    /// <remarks>This method does not throw if <paramref name="capacity" /> is negative and instead does nothing.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnsureCapacity<T>(this ref ValueQueue<T> queue, int capacity)
    {
        var currentCapacity = queue.Capacity;

        if (capacity > currentCapacity)
        {
            queue.Resize(capacity, currentCapacity);
        }
    }

    /// <summary>Gets a reference to the item at the specified index of the list.</summary>
    /// <param name="queue">The queue from which to get the reference.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the list.</returns>
    /// <remarks>
    ///     <para>This method is because other operations may invalidate the backing array.</para>
    ///     <para>This method is because it does not validate that <paramref name="index" /> is less than <see cref="ValueQueue{T}.Capacity" />.</para>
    /// </remarks>
    public static ref T GetReferenceUnsafe<T>(this scoped ref readonly ValueQueue<T> queue, int index)
    {
        var count = queue._count;

        if (unchecked((uint)index < (uint)queue._count))
        {
            var head = queue._head;
            var headLength = count - head;

            var actualIndex = ((head < queue._tail) || (index < headLength)) ? (head + index) : (index - headLength);
            return ref queue._items.GetReferenceUnsafe(actualIndex);
        }
        else
        {
            return ref NullRef<T>();
        }
    }

    /// <summary>Peeks at item at the head of the queue.</summary>
    /// <param name="queue">The queue which should be peeked.</param>
    /// <returns>The item at the head of the queue.</returns>
    /// <exception cref="InvalidOperationException">The queue is empty.</exception>
    public static T Peek<T>(this ref readonly ValueQueue<T> queue)
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
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is <c>negative</c> or greater than or equal to <see cref="ValueQueue{T}.Count" />.</exception>
    public static T Peek<T>(this ref readonly ValueQueue<T> queue, int index)
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
    public static bool Remove<T>(this ref ValueQueue<T> queue, T item)
    {
        var items = queue._items;

        if (items is not null)
        {
            var count = queue._count;
            var head = queue._head;
            var tail = queue._tail;

            var index = Array.IndexOf(items, item, head, count - head);

            if (index < 0)
            {
                index = Array.IndexOf(items, item, 0, tail);
            }

            if (index >= 0)
            {
                var newTail = tail - 1;
                var newCount = count - 1;

                Array.Copy(items, index + 1, items, index, newCount - index);

                if (tail == 0)
                {
                    newTail = newCount;
                }
                else if (head >= tail)
                {
                    items[newCount] = items[0];
                    Array.Copy(items, 1, items, 0, newTail);

                    if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                    {
                        items[newTail] = default!;
                    }
                }

                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                {
                    items[newTail] = default!;
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
    public static void TrimExcess<T>(this ref ValueQueue<T> queue, float threshold = 1.0f)
    {
        var count = queue._count;
        var minCount = (int)(queue.Capacity * Clamp(threshold, 0.0f, 1.0f));

        if (count < minCount)
        {
            var newItems = GC.AllocateUninitializedArray<T>(count);
            queue.CopyTo(newItems);

            queue._items = newItems;
            queue._head = 0;
            queue._tail = count;
        }
    }

    /// <summary>Tries to dequeue an item from the head of the queue.</summary>
    /// <param name="queue">The queue from which the item should be dequeued.</param>
    /// <param name="item">When <c>true</c> is returned, this contains the item from the head of the queue.</param>
    /// <returns><c>true</c> if the queue was not empty; otherwise, <c>false</c>.</returns>
    public static bool TryDequeue<T>(this ref ValueQueue<T> queue, [MaybeNullWhen(false)] out T item)
    {
        var count = queue._count;
        var newCount = count - 1;

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

        var items = queue._items;
        item = items[head];

        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            items[head] = default!;
        }

        return true;
    }

    /// <summary>Tries to peek at the item at the head of the queue.</summary>
    /// <param name="queue">The queue which should be peeked.</param>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the head of the queue.</param>
    /// <returns><c>true</c> if the queue was not empty; otherwise, <c>false</c>.</returns>
    public static bool TryPeek<T>(this ref readonly ValueQueue<T> queue, [MaybeNullWhen(false)] out T item)
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
    /// <returns><c>true</c> if the queue was not empty and <paramref name="index" /> is less than <see cref="ValueQueue{T}.Count" />; otherwise, <c>false</c>.</returns>
    public static bool TryPeek<T>(this ref readonly ValueQueue<T> queue, int index, [MaybeNullWhen(false)] out T item)
    {
        var count = queue._count;

        if (unchecked((uint)index < (uint)count))
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

    internal static void Resize<T>(this ref ValueQueue<T> queue, int capacity, int currentCapacity)
    {
        var newCapacity = Max(capacity, currentCapacity * 2);
        var newItems = GC.AllocateUninitializedArray<T>(newCapacity);

        queue.CopyTo(newItems);
        queue._items = newItems;

        queue._head = 0;
        queue._tail = queue._count;
    }
}
