// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Stack<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Collections;

/// <summary>Represents a stack of unmanaged items.</summary>
/// <typeparam name="T">The type of the unmanaged items contained in the stack.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
[DebuggerTypeProxy(typeof(UnmanagedValueStack<>.DebugView))]
public unsafe partial struct UnmanagedValueStack<T> : IDisposable, IEnumerable<T>
    where T : unmanaged
{
    /// <summary>Gets an empty stack.</summary>
    public static UnmanagedValueStack<T> Empty => new UnmanagedValueStack<T>();

    private UnmanagedArray<T> _items;
    private nuint _count;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueStack{T}" /> struct.</summary>
    public UnmanagedValueStack()
    {
        _items = UnmanagedArray<T>.Empty;
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueStack{T}" /> struct.</summary>
    /// <param name="capacity">The initial capacity of the stack.</param>
    /// <param name="alignment">The alignment, in bytes, of the items in the stack or <c>zero</c> to use the system default.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
    public UnmanagedValueStack(nuint capacity, nuint alignment = 0)
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

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueStack{T}" /> struct.</summary>
    /// <param name="span">The span that is used to populate the stack.</param>
    /// <param name="alignment">The alignment, in bytes, of the items in the stack or <c>zero</c> to use the system default.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
    public UnmanagedValueStack(UnmanagedReadOnlySpan<T> span, nuint alignment = 0)
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

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueStack{T}" /> struct.</summary>
    /// <param name="array">The array that is used to populate the stack.</param>
    /// <param name="takeOwnership"><c>true</c> if the stack should take ownership of the array; otherwise, <c>false</c>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
    /// <remarks>By default ownership of <paramref name="array" /> is given to the value stack.</remarks>
    public UnmanagedValueStack(UnmanagedArray<T> array, bool takeOwnership = true)
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

    /// <summary>Gets the number of items that can be contained by the stack without being resized.</summary>
    public readonly nuint Capacity
    {
        get
        {
            var items = _items;
            return !items.IsNull ? items.Length : 0;
        }
    }

    /// <summary>Gets the number of items contained in the stack.</summary>
    public readonly nuint Count => _count;

    /// <summary>Removes all items from the stack.</summary>
    public void Clear() => _count = 0;

    /// <summary>Checks whether the stack contains a specified item.</summary>
    /// <param name="item">The item to check for in the stack.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the stack; otherwise, <c>false</c>.</returns>
    public readonly bool Contains(T item)
    {
        var items = _items;
        return !items.IsNull && TryGetLastIndexOfUnsafe(items.GetPointerUnsafe(0), _count, item, out _);
    }

    /// <summary>Copies the items of the stack to a span.</summary>
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

    /// <summary>Ensures the capacity of the stack is at least the specified value.</summary>
    /// <param name="capacity">The minimum capacity the stack should support.</param>
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

    /// <summary>Gets a pointer to the item at the specified index of the stack.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the stack.</returns>
    /// <remarks>This method is because other operations may invalidate the backing array.</remarks>
    public T* GetPointerUnsafe(nuint index)
    {
        T* item;
        var count = _count;

        item = (index < count) ? _items.GetPointerUnsafe(count - (index + 1)) : null;
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
            ThrowIfNotInBounds(index, _count);
        }
        return item!;
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
        var count = _count;
        var newCount = count + 1;

        EnsureCapacity(count + 1);

        _count = newCount;
        _items[count] = item;
    }

    /// <summary>Trims any excess capacity, up to a given threshold, from the stack.</summary>
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

    /// <summary>Tries to peek the item at the head of the stack.</summary>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the head of the stack.</param>
    /// <returns><c>true</c> if the stack was not empty; otherwise, <c>false</c>.</returns>
    public readonly bool TryPeek(out T item)
    {
        var count = _count;

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
        var count = _count;

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
        var count = _count;
        var newCount = unchecked(count - 1);

        if (count == 0)
        {
            item = default!;
            return false;
        }

        _count = newCount;

        var items = _items;
        item = items[newCount];

        return true;
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
