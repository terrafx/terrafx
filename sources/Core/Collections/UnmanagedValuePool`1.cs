// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace TerraFX.Collections;

/// <summary>Represents a pool of unmanaged items.</summary>
/// <typeparam name="T">The type of the unmanaged items contained in the pool.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
[DebuggerTypeProxy(typeof(UnmanagedValuePool<>.DebugView))]
public unsafe partial struct UnmanagedValuePool<T>
    : IDisposable,
      IEnumerable<T>,
      IEquatable<UnmanagedValuePool<T>>
    where T : unmanaged
{
    internal UnmanagedValueQueue<T> _availableItems;
    internal UnmanagedValueList<T> _items;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValuePool{T}" /> struct.</summary>
    public UnmanagedValuePool()
    {
        _availableItems = [];
        _items = [];
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValuePool{T}" /> struct.</summary>
    /// <param name="capacity">The initial capacity of the pool.</param>
    public UnmanagedValuePool(nuint capacity)
    {
        _availableItems = new UnmanagedValueQueue<T>(capacity);
        _items = new UnmanagedValueList<T>(capacity);
    }

    /// <summary>Gets the number of items available in the pool.</summary>
    public readonly nuint AvailableCount => _availableItems.Count;

    /// <summary>Gets an enumerator that can iterate through the available items in the pool.</summary>
    public readonly AvailableItemsEnumerator AvailableItems => new AvailableItemsEnumerator(this);

    /// <summary>Gets the number of items that can be contained by the pool without being resized.</summary>
    public readonly nuint Capacity => _items.Capacity;

    /// <summary>Gets the number of items contained in the pool.</summary>
    public readonly nuint Count => _items.Count;

    /// <summary>Compares two <see cref="UnmanagedValuePool{T}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="UnmanagedValuePool{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedValuePool{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(UnmanagedValuePool<T> left, UnmanagedValuePool<T> right)
    {
        return (left._availableItems == right._availableItems)
            && (left._items == right._items);
    }

    /// <summary>Compares two <see cref="UnmanagedValuePool{T}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="UnmanagedValuePool{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedValuePool{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(UnmanagedValuePool<T> left, UnmanagedValuePool<T> right) => !(left == right);

    /// <inheritdoc />
    public readonly void Dispose()
    {
        _availableItems.Dispose();
        _items.Dispose();
    }

    /// <inheritdoc />
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is UnmanagedValuePool<T> other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(UnmanagedValuePool<T> other) => this == other;

    /// <summary>Gets an enumerator that can iterate through the items in the pool.</summary>
    /// <returns>An enumerator that can iterate through the items in the pool.</returns>
    public readonly ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(_availableItems, _items);

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
