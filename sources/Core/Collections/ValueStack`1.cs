// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Stack<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Collections;

/// <summary>Represents a stack of items.</summary>
/// <typeparam name="T">The type of the items contained in the stack.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
[DebuggerTypeProxy(typeof(ValueStack<>.DebugView))]
public partial struct ValueStack<T>
    : IEnumerable<T>,
      IEquatable<ValueStack<T>>
{
    internal T[] _items;
    internal int _count;

    /// <summary>Initializes a new instance of the <see cref="ValueStack{T}" /> struct.</summary>
    public ValueStack()
    {
        _items = [];
    }

    /// <summary>Initializes a new instance of the <see cref="ValueStack{T}" /> struct.</summary>
    /// <param name="capacity">The initial capacity of the stack.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is <c>negative</c>.</exception>
    public ValueStack(int capacity)
    {
        ThrowIfNegative(capacity);
        _items = (capacity != 0) ? GC.AllocateUninitializedArray<T>(capacity) : [];
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
            _items = [];
        }

        _count = span.Length;
    }

    /// <summary>Initializes a new instance of the <see cref="ValueStack{T}" /> struct.</summary>
    /// <param name="array">The array that is used to populate the stack.</param>
    /// <param name="takeOwnership"><c>true</c> if the stack should take ownership of the array; otherwise, <c>false</c>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
    /// <remarks>By default ownership of <paramref name="array" /> is given to the value list.</remarks>
    public ValueStack(T[] array, bool takeOwnership = true)
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
            return items is not null ? items.Length : 0;
        }
    }

    /// <summary>Gets the number of items contained in the stack.</summary>
    public readonly int Count => _count;

    /// <summary><c>true</c> if the stack is <c>empty</c>; otherwise, <c>false</c>.</summary>
    public readonly bool IsEmpty => _count == 0;

    /// <summary>Compares two <see cref="ValueStack{T}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="ValueStack{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="ValueStack{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(ValueStack<T> left, ValueStack<T> right)
    {
        return (left._items == right._items)
            && (left._count == right._count);
    }

    /// <summary>Compares two <see cref="ValueStack{T}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="ValueStack{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="ValueStack{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(ValueStack<T> left, ValueStack<T> right) => !(left == right);

    /// <inheritdoc />
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is ValueStack<T> other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(ValueStack<T> other) => this == other;

    /// <summary>Gets an enumerator that can iterate through the items in the list.</summary>
    /// <returns>An enumerator that can iterate through the items in the list.</returns>
    public readonly ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(_items, _count);

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
