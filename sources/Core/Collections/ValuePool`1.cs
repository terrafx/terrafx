// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace TerraFX.Collections;

/// <summary>Represents a pool of items.</summary>
/// <typeparam name="T">The type of the items contained in the pool.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
[DebuggerTypeProxy(typeof(ValuePool<>.DebugView))]
public unsafe partial struct ValuePool<T>
    : IEnumerable<T>,
      IEquatable<ValuePool<T>>
{
    internal ValueQueue<T> _availableItems;
    internal ValueList<T> _items;

    /// <summary>Initializes a new instance of the <see cref="ValuePool{T}" /> struct.</summary>
    public ValuePool()
    {
        _availableItems = [];
        _items = [];
    }

    /// <summary>Initializes a new instance of the <see cref="ValuePool{T}" /> struct.</summary>
    /// <param name="capacity">The initial capacity of the pool.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is <c>negative</c>.</exception>
    public ValuePool(int capacity)
    {
        _availableItems = new ValueQueue<T>(capacity);
        _items = new ValueList<T>(capacity);
    }

    /// <summary>Gets the number of items available in the pool.</summary>
    public readonly int AvailableItemCount => _availableItems.Count;

    /// <summary>Gets an enumerator that can iterate through the available items in the pool.</summary>
    public readonly AvailableItemsEnumerator AvailableItems => new AvailableItemsEnumerator(this);

    /// <summary>Gets the number of items that can be contained by the pool without being resized.</summary>
    public readonly int Capacity => _items.Capacity;

    /// <summary>Gets the number of items contained in the pool.</summary>
    public readonly int Count => _items.Count;

    /// <summary><c>true</c> if the pool  is <c>empty</c>; otherwise, <c>false</c>.</summary>
    public readonly bool IsEmpty => Count == 0;

    /// <summary>Compares two <see cref="ValuePool{T}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="ValuePool{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="ValuePool{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(ValuePool<T> left, ValuePool<T> right)
    {
        return (left._availableItems == right._availableItems)
            && (left._items == right._items);
    }

    /// <summary>Compares two <see cref="ValuePool{T}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="ValuePool{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="ValuePool{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(ValuePool<T> left, ValuePool<T> right) => !(left == right);

    /// <inheritdoc />
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is ValuePool<T> other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(ValuePool<T> other) => this == other;

    /// <summary>Gets an enumerator that can iterate through the items in the pool.</summary>
    /// <returns>An enumerator that can iterate through the items in the pool.</returns>
    public readonly ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(_availableItems, _items);

    internal readonly ref T GetReferenceUnsafe(int index) => ref _items.GetReferenceUnsafe(index);

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
