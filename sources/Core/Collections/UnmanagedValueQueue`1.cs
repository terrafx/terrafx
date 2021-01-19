// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the List<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Collections
{
    /// <summary>Represents a queue of unmanaged items that can be accessed by index.</summary>
    /// <typeparam name="T">The type of the unmanaged items contained in the queue.</typeparam>
    /// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
    [DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
    [DebuggerTypeProxy(typeof(UnmanagedValueQueue<>.DebugView))]
    public partial struct UnmanagedValueQueue<T> : IDisposable
        where  T : unmanaged
    {
        private UnmanagedArray<T> _items;
        private nuint _count;
        private nuint _head;
        private nuint _tail;
        private nuint _version;

        /// <summary>Initializes a new instance of the <see cref="UnmanagedValueQueue{T}" /> struct.</summary>
        /// <param name="capacity">The initial capacity of the queue.</param>
        /// <param name="alignment">The alignment, in bytes, of the items in the queue or <c>zero</c> to use the system default.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
        public UnmanagedValueQueue(nuint capacity, nuint alignment = 0)
        {
            if (capacity != 0)
            {
                _items = new UnmanagedArray<T>(capacity, alignment, zero: false);
            }
            else
            {
                if (alignment != 0)
                {
                    ThrowIfNotPow2(alignment, nameof(alignment));
                }
                _items = UnmanagedArray<T>.Empty;
            }

            _count = 0;
            _head = 0;
            _tail = 0;
            _version = 0;
        }

        /// <summary>Initializes a new instance of the <see cref="UnmanagedValueQueue{T}" /> struct.</summary>
        /// <param name="span">The span that is used to populate the queue.</param>
        /// <param name="alignment">The alignment, in bytes, of the items in the queue or <c>zero</c> to use the system default.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
        public unsafe UnmanagedValueQueue(UnmanagedReadOnlySpan<T> span, nuint alignment = 0)
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
                    ThrowIfNotPow2(alignment, nameof(alignment));
                }
                _items = UnmanagedArray<T>.Empty;
            }

            _count = span.Length;
            _head = 0;
            _tail = 0;
            _version = 0;
        }

        /// <summary>Initializes a new instance of the <see cref="UnmanagedValueQueue{T}" /> struct.</summary>
        /// <param name="array">The array that is used to populate the queue.</param>
        /// <param name="takeOwnership"><c>true</c> if the queue should take ownership of the array; otherwise, <c>false</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
        public unsafe UnmanagedValueQueue(UnmanagedArray<T> array, bool takeOwnership = false)
        {
            ThrowIfNull(array, nameof(array));

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
            _head = 0;
            _tail = 0;
            _version = 0;
        }

        /// <summary>Gets the number of items that can be contained by the queue without being resized.</summary>
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

        /// <summary>Gets the number of items contained in the queue.</summary>
        public readonly nuint Count => _count;

        /// <summary>Removes all items from the queue.</summary>
        public void Clear()
        {
            _version++;
            _count = 0;

            _head = 0;
            _tail = 0;
        }

        /// <summary>Checks whether the queue contains a specified item.</summary>
        /// <param name="item">The item to check for in the queue.</param>
        /// <returns><c>true</c> if <paramref name="item" /> was found in the queue; otherwise, <c>false</c>.</returns>
        public readonly unsafe bool Contains(T item)
        {
            var items = _items;

            if (!items.IsNull)
            {
                var head = _head;
                var tail = _tail;

                if ((head < tail) || (tail == 0))
                {
                    return TryGetIndexOfUnsafe(items.GetPointerUnsafe(head), Count, item, out _);
                }
                else
                {
                    return TryGetIndexOfUnsafe(items.GetPointerUnsafe(head), Count - head, item, out _)
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
        public readonly unsafe void CopyTo(UnmanagedSpan<T> destination)
        {
            var count = Count;

            if (count != 0)
            {
                ThrowIfNotInInsertBounds(count, destination.Length, nameof(Count), nameof(destination));

                var head = _head;
                var tail = _tail;

                if ((head < tail) || (tail == 0))
                {
                    CopyArrayUnsafe<T>(destination.GetPointerUnsafe(0), _items.GetPointerUnsafe(head), count);
                }
                else
                {
                    var headLength = count - head;
                    CopyArrayUnsafe<T>(destination.GetPointerUnsafe(0), _items.GetPointerUnsafe(head), headLength);
                    CopyArrayUnsafe<T>(destination.GetPointerUnsafe(headLength), _items.GetPointerUnsafe(0), tail);
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

                _head = 0;
                _tail = Count;
            }
        }

        /// <summary>Peeks the item at the head of the queue.</summary>
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

        /// <summary>Tries to dequeue an item from the head of the queue.</summary>
        /// <param name="item">When <c>true</c> is returned, this contains the item from the head of the queue.</param>
        /// <returns><c>true</c> if the queue was not empty; otherwise, <c>false</c>.</returns>
        public bool TryDequeue([MaybeNullWhen(false)] out T item)
        {
            var count = Count;
            var newCount = unchecked(count - 1);

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

        /// <summary>Tries to peek the item at the head of the queue.</summary>
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
    }
}
