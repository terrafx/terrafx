// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Stack<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Collections;

/// <summary>Represents a stack of items that can be accessed by index.</summary>
/// <typeparam name="T">The type of the items contained in the stack.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
[DebuggerTypeProxy(typeof(ValueStack<>.DebugView))]
public partial struct ValueStack<T> : IEnumerable<T>
{
    private T[] _items;
    private int _count;

    /// <summary>Initializes a new instance of the <see cref="ValueStack{T}" /> struct.</summary>
    /// <param name="capacity">The initial capacity of the stack.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is <c>negative</c>.</exception>
    public ValueStack(int capacity)
    {
        ThrowIfNegative(capacity);

        if (capacity != 0)
        {
            _items = GC.AllocateUninitializedArray<T>(capacity);
        }
        else
        {
            _items = Array.Empty<T>();
        }

        _count = 0;
    }

    /// <summary>Initializes a new instance of the <see cref="ValueStack{T}" /> struct.</summary>
    /// <param name="source">The enumerable that is used to populate the stack.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    public ValueStack(IEnumerable<T> source)
    {
        // This is an extension method and throws ArgumentNullException if null
        _items = source.ToArray();
        _count = _items.Length;
    }

    /// <summary>Initializes a new instance of the <see cref="ValueStack{T}" /> struct.</summary>
    /// <param name="span">The span that is used to populate the stack.</param>
    public ValueStack(ReadOnlySpan<T> span)
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
    }

    /// <summary>Initializes a new instance of the <see cref="ValueStack{T}" /> struct.</summary>
    /// <param name="array">The array that is used to populate the stack.</param>
    /// <param name="takeOwnership"><c>true</c> if the stack should take ownership of the array; otherwise, <c>false</c>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
    public ValueStack(T[] array, bool takeOwnership = false)
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

    /// <summary>Gets the number of items that can be contained by the stack without being resized.</summary>
    public readonly int Capacity
    {
        get
        {
            var items = _items;
            return items is not null ? _items.Length : 0;
        }
    }

    /// <summary>Gets the number of items contained in the stack.</summary>
    public readonly int Count => _count;

    /// <summary>Removes all items from the stack.</summary>
    public void Clear()
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            Array.Clear(_items, 0, Count);
        }

        _count = 0;
    }

    /// <summary>Checks whether the stack contains a specified item.</summary>
    /// <param name="item">The item to check for in the stack.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the stack; otherwise, <c>false</c>.</returns>
    public readonly bool Contains(T item)
    {
        var items = _items;

        if (items is not null)
        {
            var count = Count;
            return (count != 0) && Array.LastIndexOf(items, item, count - 1) != -1;
        }
        else
        {
            return false;
        }
    }

    /// <summary>Copies the items of the stack to a span.</summary>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentException"><see cref="Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public readonly void CopyTo(Span<T> destination) => _items.AsSpan(0, Count).CopyTo(destination);

    /// <summary>Ensures the capacity of the stack is at least the specified value.</summary>
    /// <param name="capacity">The minimum capacity the stack should support.</param>
    /// <remarks>This method does not throw if <paramref name="capacity" /> is negative and is instead does nothing.</remarks>
    public void EnsureCapacity(int capacity)
    {
        var currentCapacity = Capacity;

        if (capacity > currentCapacity)
        {
            var newCapacity = Max(capacity, currentCapacity * 2);
            var newItems = GC.AllocateUninitializedArray<T>(newCapacity);

            CopyTo(newItems);
            _items = newItems;
        }
    }

    /// <summary>Gets an enumerator that can iterate through the items in the list.</summary>
    /// <returns>An enumerator that can iterate through the items in the list.</returns>
    public Enumerator GetEnumerator() => new Enumerator(this);

    /// <summary>Gets a reference to the item at the specified index of the list.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the list.</returns>
    /// <remarks>
    ///     <para>This method is because other operations may invalidate the backing array.</para>
    ///     <para>This method is because it does not validate that <paramref name="index" /> is less than <see cref="Capacity" />.</para>
    /// </remarks>
    public ref T GetReferenceUnsafe(int index)
    {
        var count = Count;

        if (unchecked((uint)index < (uint)count))
        {
            return ref _items.GetReference(count - (index + 1));
        }
        else
        {
            return ref NullRef<T>();
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
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is <c>negative</c> or greater than or equal to <see cref="Count" />.</exception>
    public readonly T Peek(int index)
    {
        if (!TryPeek(index, out var item))
        {
            ThrowIfNotInBounds(index, Count);
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

        if (newCount > Capacity)
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
        var minCount = (int)(Capacity * Clamp(threshold, 0.0f, 1.0f));

        if (count < minCount)
        {
            var newItems = GC.AllocateUninitializedArray<T>(count);
            CopyTo(newItems);
            _items = newItems;
        }
    }

    /// <summary>Tries to peek the item at the top of the stack.</summary>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the top of the stack.</param>
    /// <returns><c>true</c> if the stack was not empty; otherwise, <c>false</c>.</returns>
    public readonly bool TryPeek([MaybeNullWhen(false)] out T item)
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

    /// <summary>Tries to peek the item at the top of the stack.</summary>
    /// <param name="index">The index from the top of the stack of the item at which to peek.</param>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the top of the stack.</param>
    /// <returns><c>true</c> if the stack was not empty and <paramref name="index" /> is less than <see cref="Count" />; otherwise, <c>false</c>.</returns>
    public readonly bool TryPeek(int index, [MaybeNullWhen(false)] out T item)
    {
        var count = Count;

        if (unchecked((uint)index < (uint)count))
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
    public bool TryPop([MaybeNullWhen(false)] out T item)
    {
        var count = Count;
        var newCount = count - 1;

        if (count == 0)
        {
            item = default!;
            return false;
        }

        _count = newCount;

        var items = _items;
        item = items[newCount];

        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            items[newCount] = default!;
        }

        return true;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
