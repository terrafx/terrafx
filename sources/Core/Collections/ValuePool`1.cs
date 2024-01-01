// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Threading;
using static TerraFX.Utilities.ExceptionUtilities;

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
    private ValueQueue<T> _availableItems;
    private ValueList<T> _items;

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

    /// <summary>Removes all items from the pool.</summary>
    public void Clear()
    {
        _items.Clear();
        _availableItems.Clear();
    }

    /// <inheritdoc />
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is ValuePool<T> other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(ValuePool<T> other) => this == other;

    /// <summary>Gets an enumerator that can iterate through the items in the pool.</summary>
    /// <returns>An enumerator that can iterate through the items in the pool.</returns>
    public readonly ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(_availableItems, _items);

    /// <summary>Removes the first occurrence of an item from the pool.</summary>
    /// <param name="item">The item to remove from the pool.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was removed from the pool; otherwise, <c>false</c>.</returns>
    public bool Remove(T item)
    {
        var result = _items.Remove(item);

        if (result)
        {
            _ = _availableItems.Remove(item);
        }

        return result;
    }

    /// <summary>Removes the first occurrence of an item from the pool.</summary>
    /// <param name="item">The item to remove from the pool.</param>
    /// <param name="mutex">The mutex to use when removing an item from the pool.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was removed from the pool; otherwise, <c>false</c>.</returns>
    public bool Remove(T item, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return Remove(item);
    }

    /// <summary>Rents an item from the pool, creating a new item if none are available.</summary>
    /// <param name="createItem">A pointer to the function to invoke if a new item needs to be created.</param>
    /// <param name="arg">The argument passed to <paramref name="createItem" />.</param>
    /// <returns>A rented item.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="createItem" /> is <c>null</c>.</exception>
    public T Rent<TArg>(delegate*<TArg, T> createItem, TArg arg)
    {
        ThrowIfNull(createItem);

        if (!_availableItems.TryDequeue(out var item))
        {
            item = createItem(arg);
            _items.Add(item);
        }

        return item;
    }

    /// <summary>Rents an item from the pool, creating a new item if none are available.</summary>
    /// <param name="createItem">A pointer to the function to invoke if a new item needs to be created.</param>
    /// <param name="arg">The argument passed to <paramref name="createItem" />.</param>
    /// <param name="mutex">The mutex to use when renting an item from the pool.</param>
    /// <returns>A rented item.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="createItem" /> is <c>null</c>.</exception>
    public T Rent<TArg>(delegate*<TArg, T> createItem, TArg arg, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return Rent(createItem, arg);
    }

    /// <summary>Returns an item to the pool.</summary>
    /// <param name="item">The item that should be returned to the pool.</param>
    /// <exception cref="ArgumentNullException"><paramref name="item" /> is <c>null</c>.</exception>
    public void Return(T item)
    {
        if (item is null)
        {
            ThrowArgumentNullException(nameof(item));
        }
        _availableItems.Enqueue(item);
    }

    /// <summary>Returns an item to the pool.</summary>
    /// <param name="item">The item that should be returned to the pool.</param>
    /// <param name="mutex">The mutex to use when returning an item to the pool.</param>
    /// <exception cref="ArgumentNullException"><paramref name="item" /> is <c>null</c>.</exception>
    public void Return(T item, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        Return(item);
    }

    internal readonly ref T GetReferenceUnsafe(int index) => ref _items.GetReferenceUnsafe(index);

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
