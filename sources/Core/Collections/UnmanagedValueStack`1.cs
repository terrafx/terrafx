// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Stack<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Diagnostics;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Collections
{
    /// <summary>Represents a stack of unmanaged items that can be accessed by index.</summary>
    /// <typeparam name="T">The type of the unmanaged items contained in the stack.</typeparam>
    /// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
    [DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
    [DebuggerTypeProxy(typeof(UnmanagedValueStack<>.DebugView))]
    public partial struct UnmanagedValueStack<T> : IDisposable
        where  T : unmanaged
    {
        private UnmanagedArray<T> _items;
        private nuint _count;
        private nuint _version;

        /// <summary>Initializes a new instance of the <see cref="UnmanagedValueStack{T}" /> struct.</summary>
        /// <param name="capacity">The initial capacity of the stack.</param>
        /// <param name="alignment">The alignment, in bytes, of the items in the stack or <c>zero</c> to use the system default.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
        public UnmanagedValueStack(nuint capacity, nuint alignment = 0)
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

        /// <summary>Initializes a new instance of the <see cref="UnmanagedValueStack{T}" /> struct.</summary>
        /// <param name="span">The span that is used to populate the stack.</param>
        /// <param name="alignment">The alignment, in bytes, of the items in the stack or <c>zero</c> to use the system default.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
        public unsafe UnmanagedValueStack(UnmanagedReadOnlySpan<T> span, nuint alignment = 0)
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

        /// <summary>Initializes a new instance of the <see cref="UnmanagedValueStack{T}" /> struct.</summary>
        /// <param name="array">The array that is used to populate the stack.</param>
        /// <param name="takeOwnership"><c>true</c> if the stack should take ownership of the array; otherwise, <c>false</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
        public unsafe UnmanagedValueStack(UnmanagedArray<T> array, bool takeOwnership = false)
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

        /// <summary>Gets the number of items that can be contained by the stack without being resized.</summary>
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

        /// <summary>Gets the number of items contained in the stack.</summary>
        public readonly nuint Count => _count;

        /// <summary>Removes all items from the stack.</summary>
        public void Clear()
        {
            _version++;
            _count = 0;
        }

        /// <summary>Checks whether the stack contains a specified item.</summary>
        /// <param name="item">The item to check for in the stack.</param>
        /// <returns><c>true</c> if <paramref name="item" /> was found in the stack; otherwise, <c>false</c>.</returns>
        public readonly unsafe bool Contains(T item)
        {
            var items = _items;

            if (!items.IsNull)
            {
                return TryGetLastIndexOfUnsafe(items.GetPointerUnsafe(0), Count, item, out _);
            }
            else
            {
                return false;
            }
        }

        /// <summary>Copies the items of the stack to a span.</summary>
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

        /// <summary>Ensures the capacity of the stack is at least the specified value.</summary>
        /// <param name="capacity">The minimum capacity the stack should support.</param>
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

        /// <summary>Peeks at the item at the top of the stack.</summary>
        /// <returns>The item at the top of the stack.</returns>
        /// <exception cref="InvalidOperationException">The stack is empty.</exception>
        public readonly T Peek()
        {
            if (!TryPeek(out var item))
            {
                ThrowForEmptyStack();
            }
            return item;
        }

        /// <summary>Peeks at item at the specified index of the stack.</summary>
        /// <param name="index">The index from the top of the stack of the item at which to peek.</param>
        /// <returns>The item at the specified index of the stack.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Count" />.</exception>
        public readonly T Peek(nuint index)
        {
            if (!TryPeek(index, out var item))
            {
                ThrowIfNotInBounds(index, Count, nameof(index), nameof(Count));
                Fail();
            }
            return item;
        }

        /// <summary>Pops the item from the top of the stack.</summary>
        /// <returns>The item at the top of the stack.</returns>
        /// <exception cref="InvalidOperationException">The stack is empty.</exception>
        public T Pop()
        {
            if (!TryPop(out var item))
            {
                ThrowForEmptyStack();
            }
            return item;
        }

        /// <summary>Pushes an item to the top of the stack.</summary>
        /// <param name="item">The item to push to the top of the stack.</param>
        public void Push(T item)
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

        /// <summary>Trims any excess capacity, up to a given threshold, from the stack.</summary>
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

        /// <summary>Tries to peek the item at the head of the stack.</summary>
        /// <param name="item">When <c>true</c> is returned, this contains the item at the head of the stack.</param>
        /// <returns><c>true</c> if the stack was not empty; otherwise, <c>false</c>.</returns>
        public readonly bool TryPeek(out T item)
        {
            var count = Count;

            if (count != 0)
            {
                item = _items[count - 1];
                return true;
            }
            else
            {
                item = default!;
                return false;
            }
        }

        /// <summary>Tries to peek the item at the head of the stack.</summary>
        /// <param name="index">The index from the top of the stack of the item at which to peek.</param>
        /// <param name="item">When <c>true</c> is returned, this contains the item at the head of the stack.</param>
        /// <returns><c>true</c> if the stack was not empty and <paramref name="index" /> is less than <see cref="Count" />; otherwise, <c>false</c>.</returns>
        public readonly bool TryPeek(nuint index, out T item)
        {
            var count = Count;

            if (index < count)
            {
                item = _items[count - (index + 1)];
                return true;
            }
            else
            {
                item = default!;
                return false;
            }
        }

        /// <summary>Tries to pop an item from the top of the stack.</summary>
        /// <param name="item">When <c>true</c> is returned, this contains the item from the top of the stack.</param>
        /// <returns><c>true</c> if the stack was not empty; otherwise, <c>false</c>.</returns>
        public bool TryPop(out T item)
        {
            var count = Count;
            var newCount = unchecked(count - 1);

            if (count == 0)
            {
                item = default!;
                return false;
            }

            _version++;
            _count = newCount;

            var items = _items;
            item = items[newCount];

            return true;
        }
    }
}
