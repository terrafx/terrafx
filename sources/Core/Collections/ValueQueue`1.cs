// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Queue<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static TerraFX.Utilities.ExceptionUtilities;

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
    internal T[] _items;
    internal int _count;
    internal int _head;
    internal int _tail;

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

    /// <summary><c>true</c> if the queue is <c>empty</c>; otherwise, <c>false</c>.</summary>
    public readonly bool IsEmpty => _count == 0;

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

    /// <inheritdoc />
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is ValueQueue<T> other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(ValueQueue<T> other) => this == other;

    /// <summary>Gets an enumerator that can iterate through the items in the list.</summary>
    /// <returns>An enumerator that can iterate through the items in the list.</returns>
    public readonly ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(_items, _count, _head, _tail);

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
