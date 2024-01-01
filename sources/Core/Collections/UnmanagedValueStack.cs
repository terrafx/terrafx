// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Stack<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.MemoryUtilities;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="UnmanagedValueStack{T}" /> struct.</summary>
public static unsafe class UnmanagedValueStack
{
    /// <summary>Gets an empty stack.</summary>
    public static UnmanagedValueStack<T> Empty<T>()
        where T : unmanaged => new UnmanagedValueStack<T>();

    /// <summary>Removes all items from the stack.</summary>
    public static void Clear<T>(this ref UnmanagedValueStack<T> stack)
        where T : unmanaged => stack._count = 0;

    /// <summary>Checks whether the stack contains a specified item.</summary>
    /// <param name="stack">The stack which should be checked.</param>
    /// <param name="item">The item to check for in the stack.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the stack; otherwise, <c>false</c>.</returns>
    public static bool Contains<T>(this ref readonly UnmanagedValueStack<T> stack, T item)
        where T : unmanaged
    {
        var items = stack._items;
        return !items.IsNull && TryGetLastIndexOfUnsafe(items.GetPointerUnsafe(0), stack._count, item, out _);
    }

    /// <summary>Copies the items of the stack to a span.</summary>
    /// <param name="stack">The stack which should be copied.</param>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedValueStack{T}.Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public static void CopyTo<T>(this ref readonly UnmanagedValueStack<T> stack, UnmanagedSpan<T> destination)
        where T : unmanaged
    {
        var count = stack._count;

        if (count != 0)
        {
            ThrowIfNotInInsertBounds(count, destination.Length);
            CopyArrayUnsafe(destination.GetPointerUnsafe(0), stack._items.GetPointerUnsafe(0), count);
        }
    }

    /// <summary>Ensures the capacity of the stack is at least the specified value.</summary>
    /// <param name="stack">The stack whose capacity should be ensured.</param>
    /// <param name="capacity">The minimum capacity the stack should support.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnsureCapacity<T>(this ref UnmanagedValueStack<T> stack, nuint capacity)
        where T : unmanaged
    {
        var currentCapacity = stack.Capacity;

        if (capacity > currentCapacity)
        {
            stack.Resize(capacity, currentCapacity);
        }
    }

    /// <summary>Gets a pointer to the item at the specified index of the stack.</summary>
    /// <param name="stack">The stack for which to get a pointer.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the stack.</returns>
    /// <remarks>This method is because other operations may invalidate the backing array.</remarks>
    public static T* GetPointerUnsafe<T>(this ref readonly UnmanagedValueStack<T> stack, nuint index)
        where T : unmanaged
    {
        T* item;
        var count = stack._count;

        item = (index < count) ? stack._items.GetPointerUnsafe(count - (index + 1)) : null;
        return item;
    }

    /// <summary>Gets a reference to the item at the specified index of the list.</summary>
    /// <param name="stack">The stack for which to get a reference.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the list.</returns>
    /// <remarks>
    ///     <para>This method is because other operations may invalidate the backing array.</para>
    ///     <para>This method is because it does not validate that <paramref name="index" /> is less than <see cref="UnmanagedValueStack{T}.Count" />.</para>
    /// </remarks>
    public static ref T GetReferenceUnsafe<T>(this scoped ref readonly UnmanagedValueStack<T> stack, nuint index)
        where T : unmanaged => ref *stack.GetPointerUnsafe(index);

    /// <summary>Peeks at the item at the top of the stack.</summary>
    /// <param name="stack">The stack which should be peeked.</param>
    /// <returns>The item at the top of the stack.</returns>
    /// <exception cref="InvalidOperationException">The stack is empty.</exception>
    public static T Peek<T>(this ref readonly UnmanagedValueStack<T> stack)
        where T : unmanaged
    {
        if (!stack.TryPeek(out var item))
        {
            ThrowForEmptyStack();
        }
        return item;
    }

    /// <summary>Peeks at item at the specified index of the stack.</summary>
    /// <param name="stack">The stack which should be peeked.</param>
    /// <param name="index">The index from the top of the stack of the item at which to peek.</param>
    /// <returns>The item at the specified index of the stack.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="UnmanagedValueStack{T}.Count" />.</exception>
    public static T Peek<T>(this ref readonly UnmanagedValueStack<T> stack, nuint index)
        where T : unmanaged
    {
        if (!stack.TryPeek(index, out var item))
        {
            ThrowIfNotInBounds(index, stack._count);
        }
        return item!;
    }

    /// <summary>Pops the item from the top of the stack.</summary>
    /// <param name="stack">The stack from which the item should be popped.</param>
    /// <returns>The item at the top of the stack.</returns>
    /// <exception cref="InvalidOperationException">The stack is empty.</exception>
    public static T Pop<T>(this ref UnmanagedValueStack<T> stack)
        where T : unmanaged
    {
        if (!stack.TryPop(out var item))
        {
            ThrowForEmptyStack();
        }
        return item;
    }

    /// <summary>Pushes an item to the top of the stack.</summary>
    /// <param name="stack">The stack to which the item should be pushed.</param>
    /// <param name="item">The item to push to the top of the stack.</param>
    public static void Push<T>(this ref UnmanagedValueStack<T> stack, T item)
        where T : unmanaged
    {
        var count = stack._count;
        var newCount = count + 1;

        stack.EnsureCapacity(count + 1);

        stack._count = newCount;
        stack._items[count] = item;
    }

    /// <summary>Trims any excess capacity, up to a given threshold, from the stack.</summary>
    /// <param name="stack">The stack which should be trimmed.</param>
    /// <param name="threshold">A percentage, between <c>zero</c> and <c>one</c>, under which any excess will not be trimmed.</param>
    /// <remarks>This methods clamps <paramref name="threshold" /> to between <c>zero</c> and <c>one</c>, inclusive.</remarks>
    public static void TrimExcess<T>(this ref UnmanagedValueStack<T> stack, float threshold = 1.0f)
        where T : unmanaged
    {
        var count = stack._count;
        var minCount = (nuint)(stack.Capacity * Clamp(threshold, 0.0f, 1.0f));

        if (count < minCount)
        {
            var items = stack._items;

            var alignment = !items.IsNull ? items.Alignment : 0;
            var newItems = new UnmanagedArray<T>(count, alignment);

            stack.CopyTo(newItems);
            items.Dispose();

            stack._items = newItems;
        }
    }

    /// <summary>Tries to peek the item at the head of the stack.</summary>
    /// <param name="stack">The stack which should be peeked.</param>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the head of the stack.</param>
    /// <returns><c>true</c> if the stack was not empty; otherwise, <c>false</c>.</returns>
    public static bool TryPeek<T>(this ref readonly UnmanagedValueStack<T> stack, out T item)
        where T : unmanaged
    {
        var count = stack._count;

        if (count != 0)
        {
            item = stack._items[count - 1];
            return true;
        }
        else
        {
            item = default!;
            return false;
        }
    }

    /// <summary>Tries to peek the item at the head of the stack.</summary>
    /// <param name="stack">The stack which should be peeked.</param>
    /// <param name="index">The index from the top of the stack of the item at which to peek.</param>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the head of the stack.</param>
    /// <returns><c>true</c> if the stack was not empty and <paramref name="index" /> is less than <see cref="UnmanagedValueStack{T}.Count" />; otherwise, <c>false</c>.</returns>
    public static bool TryPeek<T>(this ref readonly UnmanagedValueStack<T> stack, nuint index, out T item)
        where T : unmanaged
    {
        var count = stack._count;

        if (index < count)
        {
            item = stack._items[count - (index + 1)];
            return true;
        }
        else
        {
            item = default!;
            return false;
        }
    }

    /// <summary>Tries to pop an item from the top of the stack.</summary>
    /// <param name="stack">The stack from which the item should be popped.</param>
    /// <param name="item">When <c>true</c> is returned, this contains the item from the top of the stack.</param>
    /// <returns><c>true</c> if the stack was not empty; otherwise, <c>false</c>.</returns>
    public static bool TryPop<T>(this ref UnmanagedValueStack<T> stack, out T item)
        where T : unmanaged
    {
        var count = stack._count;
        var newCount = unchecked(count - 1);

        if (count == 0)
        {
            item = default!;
            return false;
        }

        stack._count = newCount;

        var items = stack._items;
        item = items[newCount];

        return true;
    }

    internal static void Resize<T>(this ref UnmanagedValueStack<T> stack, nuint capacity, nuint currentCapacity)
        where T : unmanaged
    {
        var items = stack._items;

        var newCapacity = Max(capacity, currentCapacity * 2);
        var alignment = !items.IsNull ? items.Alignment : 0;

        var newItems = new UnmanagedArray<T>(newCapacity, alignment);

        stack.CopyTo(newItems);
        items.Dispose();

        stack._items = newItems;
    }
}
