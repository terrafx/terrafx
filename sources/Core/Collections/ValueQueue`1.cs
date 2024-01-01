// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Queue<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Represents a queue of items.</summary>
/// <typeparam name="T">The type of the items contained in the queue.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
[DebuggerTypeProxy(typeof(ValueQueue<>.DebugView))]
public partial struct ValueQueue<T>
    : IEnumerable<T>,
      IEquatable<ValueQueue<T>>
{
    private T[] _items;
    private int _count;
    private int _head;
    private int _tail;

    /// <summary>Initializes a new instance of the <see cref="ValueQueue{T}" /> struct.</summary>
    public ValueQueue()
    {
        _items = [];
    }

    /// <summary>Initializes a new instance of the <see cref="ValueQueue{T}" /> struct.</summary>
    /// <param name="capacity">The initial capacity of the queue.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is <c>negative</c>.</exception>
    public ValueQueue(int capacity)
    {
        ThrowIfNegative(capacity);
        _items = (capacity != 0) ? GC.AllocateUninitializedArray<T>(capacity) : [];
    }

    /// <summary>Initializes a new instance of the <see cref="ValueQueue{T}" /> struct.</summary>
    /// <param name="source">The enumerable that is used to populate the queue.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    public ValueQueue(IEnumerable<T> source)
    {
        // This is an extension method and throws ArgumentNullException if null
        var items = source.ToArray();

        _items = items;
        _count = items.Length;
    }

    /// <summary>Initializes a new instance of the <see cref="ValueQueue{T}" /> struct.</summary>
    /// <param name="span">The span that is used to populate the queue.</param>
    public ValueQueue(ReadOnlySpan<T> span)
    {
        if (span.Length != 0)
        {
            var items = GC.AllocateUninitializedArray<T>(span.Length);
            span.CopyTo(items);
            _items = items;
        }
        else
        {
            _items = [];
        }

        _count = span.Length;
    }

    /// <summary>Initializes a new instance of the <see cref="ValueQueue{T}" /> struct.</summary>
    /// <param name="array">The array that is used to populate the queue.</param>
    /// <param name="takeOwnership"><c>true</c> if the queue should take ownership of the array; otherwise, <c>false</c>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
    /// <remarks>By default ownership of <paramref name="array" /> is given to the value queue.</remarks>
    public ValueQueue(T[] array, bool takeOwnership = true)
    {
        ThrowIfNull(array);

        if (takeOwnership)
        {
            _items = array;
        }
        else
        {
            var items = GC.AllocateUninitializedArray<T>(array.Length);
            Array.Copy(array, items, array.Length);
            _items = items;
        }

        _count = array.Length;
    }

    /// <summary>Gets the number of items that can be contained by the queue without being resized.</summary>
    public readonly int Capacity
    {
        get
        {
            var items = _items;
            return items is not null ? items.Length : 0;
        }
    }

    /// <summary>Gets the number of items contained in the queue.</summary>
    public readonly int Count => _count;

    /// <summary>Compares two <see cref="ValueQueue{T}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="ValueQueue{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="ValueQueue{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(ValueQueue<T> left, ValueQueue<T> right)
    {
        return (left._items == right._items)
            && (left._count == right._count)
            && (left._head == right._head)
            && (left._tail == right._tail);
    }

    /// <summary>Compares two <see cref="ValueQueue{T}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="ValueQueue{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="ValueQueue{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(ValueQueue<T> left, ValueQueue<T> right) => !(left == right);

    /// <summary>Removes all items from the queue.</summary>
    public void Clear()
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>() && (_items is not null))
        {
            var head = _head;
            var tail = _tail;

            if ((head < tail) || (tail == 0))
            {
                Array.Clear(_items, head, _count);
            }
            else
            {
                var items = _items;

                Array.Clear(items, head, _count - head);
                Array.Clear(items, 0, tail);
            }
        }

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

        if (items is not null)
        {
            var head = _head;
            var tail = _tail;

            if ((head < tail) || (tail == 0))
            {
                return Array.IndexOf(items, item, head, _count) >= 0;
            }
            else
            {
                return (Array.IndexOf(items, item, head, _count - head) >= 0)
                    || (Array.IndexOf(items, item, 0, tail) >= 0);
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>Copies the items of the queue to a span.</summary>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentException"><see cref="Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public readonly void CopyTo(Span<T> destination)
    {
        var count = _count;
        var items = _items;

        var head = _head;
        var tail = _tail;

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

        EnsureCapacity(count + 1);

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
    /// <remarks>This method does not throw if <paramref name="capacity" /> is negative and instead does nothing.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void EnsureCapacity(int capacity)
    {
        var currentCapacity = Capacity;

        if (capacity > currentCapacity)
        {
            Resize(capacity, currentCapacity);
        }
    }

    /// <inheritdoc />
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is ValueQueue<T> other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(ValueQueue<T> other) => this == other;

    /// <summary>Gets an enumerator that can iterate through the items in the list.</summary>
    /// <returns>An enumerator that can iterate through the items in the list.</returns>
    public readonly ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(_items, _count, _head, _tail);

    /// <summary>Gets a reference to the item at the specified index of the list.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the list.</returns>
    /// <remarks>
    ///     <para>This method is because other operations may invalidate the backing array.</para>
    ///     <para>This method is because it does not validate that <paramref name="index" /> is less than <see cref="Capacity" />.</para>
    /// </remarks>
    public readonly ref T GetReferenceUnsafe(int index)
    {
        var count = _count;

        if (unchecked((uint)index < (uint)_count))
        {
            var head = _head;
            var headLength = count - head;

            var actualIndex = ((head < _tail) || (index < headLength)) ? (head + index) : (index - headLength);
            return ref _items.GetReferenceUnsafe(actualIndex);
        }
        else
        {
            return ref NullRef<T>();
        }
    }

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
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is <c>negative</c> or greater than or equal to <see cref="Count" />.</exception>
    public readonly T Peek(int index)
    {
        if (!TryPeek(index, out var item))
        {
            ThrowIfNotInBounds(index, _count);
        }
        return item!;
    }

    /// <summary>Removes the first occurrence of an item from the queue.</summary>
    /// <param name="item">The item to remove from the queue.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was removed from the queue; otherwise, <c>false</c>.</returns>
    public bool Remove(T item)
    {
        var items = _items;

        if (items is not null)
        {
            var count = _count;
            var head = _head;
            var tail = _tail;

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

                _tail = newTail;
                _count = newCount;

                return true;
            }
        }

        return false;
    }

    /// <summary>Trims any excess capacity, up to a given threshold, from the queue.</summary>
    /// <param name="threshold">A percentage, between <c>zero</c> and <c>one</c>, under which any excess will not be trimmed.</param>
    /// <remarks>This methods clamps <paramref name="threshold" /> to between <c>zero</c> and <c>one</c>, inclusive.</remarks>
    public void TrimExcess(float threshold = 1.0f)
    {
        var count = _count;
        var minCount = (int)(Capacity * Clamp(threshold, 0.0f, 1.0f));

        if (count < minCount)
        {
            var newItems = GC.AllocateUninitializedArray<T>(count);
            CopyTo(newItems);

            _items = newItems;
            _head = 0;
            _tail = count;
        }
    }

    /// <summary>Tries to dequeue an item from the head of the queue.</summary>
    /// <param name="item">When <c>true</c> is returned, this contains the item from the head of the queue.</param>
    /// <returns><c>true</c> if the queue was not empty; otherwise, <c>false</c>.</returns>
    public bool TryDequeue([MaybeNullWhen(false)] out T item)
    {
        var count = _count;
        var newCount = count - 1;

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

        var items = _items;
        item = items[head];

        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            items[head] = default!;
        }

        return true;
    }

    /// <summary>Tries to peek at the item at the head of the queue.</summary>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the head of the queue.</param>
    /// <returns><c>true</c> if the queue was not empty; otherwise, <c>false</c>.</returns>
    public readonly bool TryPeek([MaybeNullWhen(false)] out T item)
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
    public readonly bool TryPeek(int index, [MaybeNullWhen(false)] out T item)
    {
        var count = _count;

        if (unchecked((uint)index < (uint)count))
        {
            var head = _head;

            var actualIndex = ((head < _tail) || (index < (count - head))) ? (head + index) : index;
            item = _items[actualIndex];

            return true;
        }
        else
        {
            item = default!;
            return false;
        }
    }

    private void Resize(int capacity, int currentCapacity)
    {
        var newCapacity = Max(capacity, currentCapacity * 2);
        var newItems = GC.AllocateUninitializedArray<T>(newCapacity);

        CopyTo(newItems);
        _items = newItems;

        _head = 0;
        _tail = _count;
    }

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
