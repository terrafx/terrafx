// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Queue<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Collections
{
    /// <summary>Represents a queue of items that can be accessed by index.</summary>
    /// <typeparam name="T">The type of the items contained in the queue.</typeparam>
    /// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
    [DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
    [DebuggerTypeProxy(typeof(ValueQueue<>.DebugView))]
    public partial struct ValueQueue<T>
    {
        private T[] _items;
        private int _count;
        private int _head;
        private int _tail;
        private int _version;

        /// <summary>Initializes a new instance of the <see cref="ValueQueue{T}" /> struct.</summary>
        /// <param name="capacity">The initial capacity of the queue.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is <c>negative</c>.</exception>
        public ValueQueue(int capacity)
        {
            ThrowIfNegative(capacity, nameof(capacity));

            if (capacity != 0)
            {
                _items = GC.AllocateUninitializedArray<T>(capacity);
            }
            else
            {
                _items = Array.Empty<T>();
            }

            _count = 0;
            _head = 0;
            _tail = 0;
            _version = 0;
        }

        /// <summary>Initializes a new instance of the <see cref="ValueQueue{T}" /> struct.</summary>
        /// <param name="source">The enumerable that is used to populate the queue.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
        public ValueQueue(IEnumerable<T> source)
        {
            // This is an extension method and throws ArgumentNullException if null
            _items = source.ToArray();

            _count = _items.Length;
            _head = 0;
            _tail = 0;
            _version = 0;
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
                _items = Array.Empty<T>();
            }

            _count = span.Length;
            _head = 0;
            _tail = 0;
            _version = 0;
        }

        /// <summary>Initializes a new instance of the <see cref="ValueQueue{T}" /> struct.</summary>
        /// <param name="array">The array that is used to populate the queue.</param>
        /// <param name="takeOwnership"><c>true</c> if the queue should take ownership of the array; otherwise, <c>false</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
        public ValueQueue(T[] array, bool takeOwnership = false)
        {
            ThrowIfNull(array, nameof(array));

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
            _head = 0;
            _tail = 0;
            _version = 0;
        }

        /// <summary>Gets the number of items that can be contained by the queue without being resized.</summary>
        public readonly int Capacity
        {
            get
            {
                var items = _items;

                if (items is not null)
                {
                    return _items.Length;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>Gets the number of items contained in the queue.</summary>
        public readonly int Count => _count;

        /// <summary>Removes all items from the queue.</summary>
        public void Clear()
        {
            _version++;

            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                var head = _head;
                var tail = _tail;

                if ((head < tail) || (tail == 0))
                {
                    Array.Clear(_items, head, Count);
                }
                else
                {
                    Array.Clear(_items, head, Count - head);
                    Array.Clear(_items, 0, tail);
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
                    return Array.IndexOf(items, item, head, Count) != -1;
                }
                else
                {
                    return (Array.IndexOf(items, item, head, Count - head) != -1)
                        || (Array.IndexOf(items, item, 0, tail) != -1);
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
        /// <remarks>This method does not throw if <paramref name="capacity" /> is negative and is instead does nothing.</remarks>
        public void EnsureCapacity(int capacity)
        {
            var currentCapacity = Capacity;

            if (capacity > currentCapacity)
            {
                var newCapacity = Max(capacity, currentCapacity * 2);
                var newItems = GC.AllocateUninitializedArray<T>(newCapacity);

                CopyTo(newItems);

                _version++;
                _items = newItems;

                _head = 0;
                _tail = Count;
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
                ThrowIfNotInBounds(index, Count, nameof(index), nameof(Count));
                Fail();
            }
            return item;
        }

        /// <summary>Tries to dequeue an item from the head of the queue.</summary>
        /// <param name="item">When <c>true</c> is returned, this contains the item from the head of the queue.</param>
        /// <returns><c>true</c> if the queue was not empty; otherwise, <c>false</c>.</returns>
        public bool TryDequeue([MaybeNullWhen(false)] out T item)
        {
            var count = Count;
            var newCount = count - 1;

            if (count == 0)
            {
                item = default!;
                return false;
            }

            _version++;

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
            if (Count != 0)
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
            var count = Count;

            if (unchecked((uint)index < count))
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
    }
}
