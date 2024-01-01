// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="ValueStack{T}" /> struct.</summary>
public static class ValueStack
{
    /// <summary>Gets an empty stack.</summary>
    public static ValueStack<T> Empty<T>() => new ValueStack<T>();

    /// <summary>Removes all items from the stack.</summary>
    /// <param name="stack">The stack which should be cleared.</param>
    public static void Clear<T>(this ref ValueStack<T> stack)
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>() && (stack._items is not null))
        {
            Array.Clear(stack._items, 0, stack._count);
        }

        stack._count = 0;
    }

    /// <summary>Checks whether the stack contains a specified item.</summary>
    /// <param name="stack">The stack which should be checked.</param>
    /// <param name="item">The item to check for in the stack.</param>
    /// <returns><c>true</c> if <paramref name="item" /> was found in the stack; otherwise, <c>false</c>.</returns>
    public static bool Contains<T>(this ref readonly ValueStack<T> stack, T item)
    {
        var items = stack._items;

        if (items is not null)
        {
            var count = stack._count;
            return (count != 0) && Array.LastIndexOf(items, item, count - 1) >= 0;
        }
        else
        {
            return false;
        }
    }

    /// <summary>Copies the items of the stack to a span.</summary>
    /// <param name="stack">The stack which should be copied.</param>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentException"><see cref="ValueStack{T}.Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public static void CopyTo<T>(this ref readonly ValueStack<T> stack, Span<T> destination) => stack._items.AsSpan(0, stack._count).CopyTo(destination);

    /// <summary>Ensures the capacity of the stack is at least the specified value.</summary>
    /// <param name="stack">The stack whose capacity should be ensured.</param>
    /// <param name="capacity">The minimum capacity the stack should support.</param>
    /// <remarks>This method does not throw if <paramref name="capacity" /> is negative and instead does nothing.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnsureCapacity<T>(this ref ValueStack<T> stack, int capacity)
    {
        var currentCapacity = stack.Capacity;

        if (capacity > currentCapacity)
        {
            stack.Resize(capacity, currentCapacity);
        }
    }

    /// <summary>Gets a reference to the item at the specified index of the list.</summary>
    /// <param name="stack">The stack from which to get the reference.</param>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the list.</returns>
    /// <remarks>
    ///     <para>This method is because other operations may invalidate the backing array.</para>
    ///     <para>This method is because it does not validate that <paramref name="index" /> is less than <see cref="ValueStack{T}.Capacity" />.</para>
    /// </remarks>
    public static ref T GetReferenceUnsafe<T>(this scoped ref readonly ValueStack<T> stack, int index)
    {
        var count = stack._count;
        return ref (((uint)index < (uint)count) ? ref stack._items.GetReferenceUnsafe(count - (index + 1)) : ref NullRef<T>());
    }

    /// <summary>Peeks at the item at the top of the stack.</summary>
    /// <param name="stack">The stack which should be peeked.</param>
    /// <returns>The item at the top of the stack.</returns>
    /// <exception cref="InvalidOperationException">The stack is empty.</exception>
    public static T Peek<T>(this ref readonly ValueStack<T> stack)
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
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is <c>negative</c> or greater than or equal to <see cref="ValueStack{T}.Count" />.</exception>
    public static T Peek<T>(this ref readonly ValueStack<T> stack, int index)
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
    public static T Pop<T>(this ref ValueStack<T> stack)
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
    public static void Push<T>(this ref ValueStack<T> stack, T item)
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
    public static void TrimExcess<T>(this ref ValueStack<T> stack, float threshold = 1.0f)
    {
        var count = stack._count;
        var minCount = (int)(stack.Capacity * Clamp(threshold, 0.0f, 1.0f));

        if (count < minCount)
        {
            var newItems = GC.AllocateUninitializedArray<T>(count);
            stack.CopyTo(newItems);
            stack._items = newItems;
        }
    }

    /// <summary>Tries to peek the item at the top of the stack.</summary>
    /// <param name="stack">The stack which should be peeked.</param>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the top of the stack.</param>
    /// <returns><c>true</c> if the stack was not empty; otherwise, <c>false</c>.</returns>
    public static bool TryPeek<T>(this ref readonly ValueStack<T> stack, [MaybeNullWhen(false)] out T item)
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

    /// <summary>Tries to peek the item at the top of the stack.</summary>
    /// <param name="stack">The stack which should be peeked.</param>
    /// <param name="index">The index from the top of the stack of the item at which to peek.</param>
    /// <param name="item">When <c>true</c> is returned, this contains the item at the top of the stack.</param>
    /// <returns><c>true</c> if the stack was not empty and <paramref name="index" /> is less than <see cref="ValueStack{T}.Count" />; otherwise, <c>false</c>.</returns>
    public static bool TryPeek<T>(this ref readonly ValueStack<T> stack, int index, [MaybeNullWhen(false)] out T item)
    {
        var count = stack._count;

        if (unchecked((uint)index < (uint)count))
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
    public static bool TryPop<T>(this ref ValueStack<T> stack, [MaybeNullWhen(false)] out T item)
    {
        var count = stack._count;
        var newCount = count - 1;

        if (count == 0)
        {
            item = default!;
            return false;
        }

        stack._count = newCount;

        var items = stack._items;
        item = stack._items[newCount];

        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            items[newCount] = default!;
        }

        return true;
    }

    internal static void Resize<T>(this ref ValueStack<T> stack, int capacity, int currentCapacity)
    {
        var newCapacity = Max(capacity, currentCapacity * 2);
        var newItems = GC.AllocateUninitializedArray<T>(newCapacity);

        stack.CopyTo(newItems);
        stack._items = newItems;
    }
}
