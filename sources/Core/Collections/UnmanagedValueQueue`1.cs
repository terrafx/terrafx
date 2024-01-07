// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Queue<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Represents a queue of unmanaged items.</summary>
/// <typeparam name="T">The type of the unmanaged items contained in the queue.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
[DebuggerTypeProxy(typeof(UnmanagedValueQueue<>.DebugView))]
public unsafe partial struct UnmanagedValueQueue<T>
    : IDisposable,
      IEnumerable<T>,
      IEquatable<UnmanagedValueQueue<T>>
    where T : unmanaged
{
    internal UnmanagedArray<T> _items;
    internal nuint _count;
    internal nuint _head;
    internal nuint _tail;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueQueue{T}" /> struct.</summary>
    public UnmanagedValueQueue()
    {
        _items = [];
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
            _items = [];
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
            _items = [];
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

    /// <summary><c>true</c> if the queue is <c>empty</c>; otherwise, <c>false</c>.</summary>
    public readonly bool IsEmpty => _count == 0;

    /// <summary>Compares two <see cref="UnmanagedValueQueue{T}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="UnmanagedValueQueue{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedValueQueue{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(UnmanagedValueQueue<T> left, UnmanagedValueQueue<T> right)
    {
        return (left._items == right._items)
            && (left._count == right._count)
            && (left._head == right._head)
            && (left._tail == right._tail);
    }

    /// <summary>Compares two <see cref="UnmanagedValueQueue{T}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="UnmanagedValueQueue{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedValueQueue{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(UnmanagedValueQueue<T> left, UnmanagedValueQueue<T> right) => !(left == right);

    /// <inheritdoc />
    public readonly void Dispose() => _items.Dispose();

    /// <inheritdoc />
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is UnmanagedValueQueue<T> other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(UnmanagedValueQueue<T> other) => this == other;

    /// <summary>Gets an enumerator that can iterate through the items in the list.</summary>
    /// <returns>An enumerator that can iterate through the items in the list.</returns>
    public readonly ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(_items, _count, _head, _tail);

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
