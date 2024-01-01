// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Threading;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="UnmanagedValuePool{T}" /> struct.</summary>
public static unsafe class UnmanagedValuePool
{
    /// <summary>Gets an empty pool.</summary>
    public static UnmanagedValuePool<T> Empty<T>()
        where T : unmanaged => new UnmanagedValuePool<T>();

    /// <summary>Removes all items from the pool.</summary>
    /// <param name="pool">The pool which should be cleared.</param>
    public static void Clear<T>(this ref UnmanagedValuePool<T> pool)
        where T : unmanaged
    {
        pool._items.Clear();
        pool._availableItems.Clear();
    }

    /// <summary>Removes the first occurrence of an item from the pool.</summary>
    /// <param name="pool">The pool from which the item should be removed.</param>
    /// <param name="item">The item to remove from the pool.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was removed from the pool; otherwise, <c>false</c>.</returns>
    public static bool Remove<T>(this ref UnmanagedValuePool<T> pool, T item)
        where T : unmanaged
    {
        var result = pool._items.Remove(item);

        if (result)
        {
            _ = pool._availableItems.Remove(item);
        }

        return result;
    }

    /// <summary>Removes the first occurrence of an item from the pool.</summary>
    /// <param name="pool">The pool from which the item should be removed.</param>
    /// <param name="item">The item to remove from the pool.</param>
    /// <param name="mutex">The mutex to use when removing an item from the pool.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was removed from the pool; otherwise, <c>false</c>.</returns>
    public static bool Remove<T>(this ref UnmanagedValuePool<T> pool, T item, ValueMutex mutex)
        where T : unmanaged
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return pool.Remove(item);
    }

    /// <summary>Rents an item from the pool, creating a new item if none are available.</summary>
    /// <param name="pool">The pool from which the item should be rented.</param>
    /// <param name="createItem">A pointer to the function to invoke if a new item needs to be created.</param>
    /// <returns>A rented item.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="createItem" /> is <c>null</c>.</exception>
    public static T Rent<T>(this ref UnmanagedValuePool<T> pool, delegate*<T> createItem)
        where T : unmanaged
    {
        ThrowIfNull(createItem);

        if (!pool._availableItems.TryDequeue(out var item))
        {
            item = createItem();
            pool._items.Add(item);
        }

        return item;
    }

    /// <summary>Rents an item from the pool, creating a new item if none are available.</summary>
    /// <param name="pool">The pool from which the item should be rented.</param>
    /// <param name="createItem">A pointer to the function to invoke if a new item needs to be created.</param>
    /// <param name="mutex">The mutex to use when renting an item from the pool.</param>
    /// <returns>A rented item.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="createItem" /> is <c>null</c>.</exception>
    public static T Rent<T>(this ref UnmanagedValuePool<T> pool, delegate*<T> createItem, ValueMutex mutex)
        where T : unmanaged
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return pool.Rent(createItem);
    }

    /// <summary>Rents an item from the pool, creating a new item if none are available.</summary>
    /// <param name="pool">The pool from which the item should be rented.</param>
    /// <param name="createItem">A pointer to the function to invoke if a new item needs to be created.</param>
    /// <param name="arg">The argument passed to <paramref name="createItem" />.</param>
    /// <returns>A rented item.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="createItem" /> is <c>null</c>.</exception>
    public static T Rent<T, TArg>(this ref UnmanagedValuePool<T> pool, delegate*<TArg, T> createItem, TArg arg)
        where T : unmanaged
    {
        ThrowIfNull(createItem);

        if (!pool._availableItems.TryDequeue(out var item))
        {
            item = createItem(arg);
            pool._items.Add(item);
        }

        return item;
    }
    /// <summary>Rents an item from the pool, creating a new item if none are available.</summary>
    /// <param name="pool">The pool from which the item should be rented.</param>
    /// <param name="createItem">A pointer to the function to invoke if a new item needs to be created.</param>
    /// <param name="arg">The argument passed to <paramref name="createItem" />.</param>
    /// <param name="mutex">The mutex to use when renting an item from the pool.</param>
    /// <returns>A rented item.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="createItem" /> is <c>null</c>.</exception>
    public static T Rent<T, TArg>(this ref UnmanagedValuePool<T> pool, delegate*<TArg, T> createItem, TArg arg, ValueMutex mutex)
        where T : unmanaged
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return pool.Rent(createItem, arg);
    }

    /// <summary>Returns an item to the pool.</summary>
    /// <param name="pool">The pool to which the item should be returned.</param>
    /// <param name="item">The item that should be returned to the pool.</param>
    public static void Return<T>(this ref UnmanagedValuePool<T> pool, T item)
        where T : unmanaged => pool._availableItems.Enqueue(item);

    /// <summary>Returns an item to the pool.</summary>
    /// <param name="pool">The pool to which the item should be returned.</param>
    /// <param name="item">The item that should be returned to the pool.</param>
    /// <param name="mutex">The mutex to use when renting an item from the pool.</param>
    /// <exception cref="ArgumentNullException"><paramref name="item" /> is <c>null</c>.</exception>
    public static void Return<T>(this ref UnmanagedValuePool<T> pool, T item, ValueMutex mutex)
        where T : unmanaged
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        pool.Return(item);
    }

    internal static T* GetPointerUnsafe<T>(this ref readonly UnmanagedValuePool<T> pool, nuint index)
        where T : unmanaged => pool._items.GetPointerUnsafe(index);

    internal static ref T GetReferenceUnsafe<T>(this scoped ref readonly UnmanagedValuePool<T> pool, nuint index)
        where T : unmanaged => ref *pool.GetPointerUnsafe(index);
}
