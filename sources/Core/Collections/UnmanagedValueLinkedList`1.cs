// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the LinkedList<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Collections;

/// <summary>Represents a linked list of unmanaged items.</summary>
/// <typeparam name="T">The type of the items contained in the linked list.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(UnmanagedValueLinkedList<>.DebugView))]
public unsafe partial struct UnmanagedValueLinkedList<T>
    : IDisposable,
      IEnumerable<T>,
      IEquatable<UnmanagedValueLinkedList<T>>
    where T : unmanaged
{
    private UnmanagedValueLinkedListNode<T>* _first;
    private nuint _count;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueLinkedList{T}" /> struct.</summary>
    /// <param name="source">The enumerable that is used to populate the linked list.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    public UnmanagedValueLinkedList(IEnumerable<T> source)
    {
        ThrowIfNull(source);

        foreach (var value in source)
        {
            _ = AddLast(value);
        }
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedValueLinkedList{T}" /> struct.</summary>
    /// <param name="span">The span that is used to populate the linked list.</param>
    public UnmanagedValueLinkedList(UnmanagedReadOnlySpan<T> span)
    {
        foreach (var value in span)
        {
            _ = AddLast(value);
        }
    }

    /// <summary>Gets the number of items contained in the linked list.</summary>
    public readonly nuint Count => _count;

    /// <summary>Gets the first node in the linked list or <c>null</c> if the linked list is empty.</summary>
    public readonly UnmanagedValueLinkedListNode<T>* First => _first;

    /// <summary>Gets the last node in the linked list or <c>null</c> if the linked list is empty.</summary>
    public readonly UnmanagedValueLinkedListNode<T>* Last
    {
        get
        {
            var first = _first;
            return (first is not null) ? first->_previous : null;
        }
    }

    /// <summary>Compares two <see cref="UnmanagedValueLinkedList{T}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="UnmanagedValueLinkedList{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedValueLinkedList{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(UnmanagedValueLinkedList<T> left, UnmanagedValueLinkedList<T> right)
    {
        return (left._first == right._first)
            && (left._count == right._count);
    }

    /// <summary>Compares two <see cref="UnmanagedValueLinkedList{T}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="UnmanagedValueLinkedList{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedValueLinkedList{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(UnmanagedValueLinkedList<T> left, UnmanagedValueLinkedList<T> right) => !(left == right);

    /// <summary>Adds a new node containing a given value after the specified node.</summary>
    /// <param name="node">The node after which the new node should be added.</param>
    /// <param name="value">The value which the new node contains.</param>
    /// <returns>The new node.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="node" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="node" /> does not have a parent.</exception>
    public UnmanagedValueLinkedListNode<T>* AddAfter(UnmanagedValueLinkedListNode<T>* node, T value)
    {
        ValidateNode(node);

        var result = Allocate<UnmanagedValueLinkedListNode<T>>();
        result->_value = value;

        AssertNotNull(node->_next);
        InternalInsertNodeBefore(node->_next, result);

        Assert(result->HasParent);
        return result;
    }

    /// <summary>Adds a new node after the specified node.</summary>
    /// <param name="node">The node after which <paramref name="newNode" /> should be added.</param>
    /// <param name="newNode">The new node.</param>
    /// <returns><paramref name="newNode" /></returns>
    /// <exception cref="ArgumentNullException"><paramref name="node" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="newNode" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="node" /> does not have a parent.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="newNode" /> already has a parent.</exception>
    public UnmanagedValueLinkedListNode<T>* AddAfter(UnmanagedValueLinkedListNode<T>* node, UnmanagedValueLinkedListNode<T>* newNode)
    {
        ValidateNode(node);
        ValidateNewNode(newNode);

        AssertNotNull(node->_next);
        InternalInsertNodeBefore(node->_next, newNode);

        Assert(newNode->HasParent);
        return newNode;
    }

    /// <summary>Adds a new node containing a given value before the specified node.</summary>
    /// <param name="node">The node before which the new node should be added.</param>
    /// <param name="value">The value which the new node contains.</param>
    /// <returns>The new node.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="node" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="node" /> does not have a parent.</exception>
    public UnmanagedValueLinkedListNode<T>* AddBefore(UnmanagedValueLinkedListNode<T>* node, T value)
    {
        ValidateNode(node);

        var result = Allocate<UnmanagedValueLinkedListNode<T>>();
        result->_value = value;

        InternalInsertNodeBefore(node, result);

        if (node == _first)
        {
            Assert(node->_isFirstNode);
            node->_isFirstNode = false;

            result->_isFirstNode = true;
            _first = result;
        }

        Assert(result->HasParent);
        return result;
    }

    /// <summary>Adds a new node before the specified node.</summary>
    /// <param name="node">The node before which <paramref name="newNode" /> should be added.</param>
    /// <param name="newNode">The new node.</param>
    /// <returns><paramref name="newNode" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="node" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="newNode" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="node" /> does not have a parent.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="newNode" /> already has a parent.</exception>
    public UnmanagedValueLinkedListNode<T>* AddBefore(UnmanagedValueLinkedListNode<T>* node, UnmanagedValueLinkedListNode<T>* newNode)
    {
        ValidateNode(node);

        ValidateNewNode(newNode);
        InternalInsertNodeBefore(node, newNode);

        if (node == _first)
        {
            Assert(node->_isFirstNode);
            node->_isFirstNode = false;

            newNode->_isFirstNode = true;
            _first = newNode;
        }

        Assert(newNode->HasParent);
        return newNode;
    }

    /// <summary>Adds a new node containing a given value as the first node.</summary>
    /// <param name="value">The value which the new node contains.</param>
    /// <returns>The new node.</returns>
    public UnmanagedValueLinkedListNode<T>* AddFirst(T value)
    {
        var result = Allocate<UnmanagedValueLinkedListNode<T>>();
        result->_value = value;

        var first = _first;

        if (first is null)
        {
            InternalInsertNodeToEmptyList(result);
        }
        else
        {
            InternalInsertNodeBefore(first, result);

            Assert(first->_isFirstNode);
            first->_isFirstNode = false;

            result->_isFirstNode = true;
            _first = result;
        }

        Assert(result->HasParent);
        return result;
    }

    /// <summary>Adds a new node as the first node.</summary>
    /// <param name="newNode">The new node.</param>
    /// <returns><paramref name="newNode" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="newNode" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="newNode" /> already has a parent.</exception>
    public UnmanagedValueLinkedListNode<T>* AddFirst(UnmanagedValueLinkedListNode<T>* newNode)
    {
        ValidateNewNode(newNode);
        var first = _first;

        if (first is null)
        {
            InternalInsertNodeToEmptyList(newNode);
        }
        else
        {
            InternalInsertNodeBefore(first, newNode);

            Assert(first->_isFirstNode);
            first->_isFirstNode = false;

            newNode->_isFirstNode = true;
            _first = newNode;
        }

        Assert(newNode->HasParent);
        return newNode;
    }

    /// <summary>Adds a new node containing a given value as the last node.</summary>
    /// <param name="value">The value which the new node contains.</param>
    /// <returns>The new node.</returns>
    public UnmanagedValueLinkedListNode<T>* AddLast(T value)
    {
        var result = Allocate<UnmanagedValueLinkedListNode<T>>();
        result->_value = value;

        var first = _first;

        if (first is null)
        {
            InternalInsertNodeToEmptyList(result);
        }
        else
        {
            InternalInsertNodeBefore(first, result);
        }

        Assert(result->HasParent);
        return result;
    }

    /// <summary>Adds a new node as the last node.</summary>
    /// <param name="newNode">The new node.</param>
    /// <returns><paramref name="newNode" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="newNode" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="newNode" /> already has a parent.</exception>
    public UnmanagedValueLinkedListNode<T>* AddLast(UnmanagedValueLinkedListNode<T>* newNode)
    {
        ValidateNewNode(newNode);

        var first = _first;

        if (first is null)
        {
            InternalInsertNodeToEmptyList(newNode);
        }
        else
        {
            InternalInsertNodeBefore(first, newNode);
        }

        Assert(newNode->HasParent);
        return newNode;
    }

    /// <summary>Removes all items from the linked list.</summary>
    public void Clear()
    {
        var current = _first;

        while (current is not null)
        {
            var temp = current;
            current = current->_next;
            temp->Invalidate();
        }

        _first = null;
        _count = 0;
    }

    /// <summary>Checks whether the linked list contains a specified item.</summary>
    /// <param name="value">The item to check for in the linked list.</param>
    /// <returns><c>true</c> if a node containing <paramref name="value" /> was found in the linked list; otherwise, <c>false</c>.</returns>
    public readonly bool Contains(T value) => Contains(value, EqualityComparer<T>.Default);

    /// <summary>Checks whether the linked list contains a specified item.</summary>
    /// <param name="value">The item to check for in the linked list.</param>
    /// <param name="comparer">The comparer to use when checking for equality.</param>
    /// <returns><c>true</c> if a node containing <paramref name="value" /> was found in the linked list; otherwise, <c>false</c>.</returns>
    public readonly bool Contains<TComparer>(T value, TComparer comparer)
        where TComparer : IEqualityComparer<T> => Find(value, comparer) is not null;

    /// <summary>Checks whether the linked list contains a specified node.</summary>
    /// <param name="node">The node to check for in the linked list.</param>
    /// <returns><c>true</c> if <paramref name="node" /> was found in the linked list; otherwise, <c>false</c>.</returns>
    public readonly bool Contains(UnmanagedValueLinkedListNode<T>* node)
    {
        if (node->HasParent)
        {
            var first = _first;
            var current = first;

            if (current is not null)
            {
                do
                {
                    AssertNotNull(current);

                    if (current == node)
                    {
                        return true;
                    }

                    current = current->_next;
                }
                while (current != first);
            }
        }
        return false;
    }

    /// <summary>Copies the values contained by the nodes in the linked list to a given destination.</summary>
    /// <param name="destination">The destination span where the items should be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Count" /> is greater than <paramref name="destination" />.</exception>
    public readonly void CopyTo(UnmanagedSpan<T> destination)
    {
        ThrowIfNotInInsertBounds(destination.Length, _count);

        var first = _first;
        var current = first;

        if (current is not null)
        {
            nuint index = 0;

            do
            {
                AssertNotNull(current);
                destination[index] = current->_value;

                current = current->_next;
                index++;
            }
            while (current != first);
        }
    }

    /// <inheritdoc />
    public readonly void Dispose()
    {
        var first = _first;
        var current = first;

        if (current is not null)
        {
            do
            {
                AssertNotNull(current);
                var next = current->_next;

                Free(current);
                current = next;
            }
            while (current != first);
        }
    }

    /// <inheritdoc />
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is UnmanagedValueLinkedList<T> other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(UnmanagedValueLinkedList<T> other) => this == other;

    /// <summary>Tries to find a node in the linked list that contains a specified value.</summary>
    /// <param name="value">The value to check for in the linked list.</param>
    /// <returns>The node that contains <paramref name="value" /> if it exists; otherwise, <c>null</c>.</returns>
    public readonly UnmanagedValueLinkedListNode<T>* Find(T value) => Find(value, EqualityComparer<T>.Default);

    /// <summary>Tries to find a node in the linked list that contains a specified value.</summary>
    /// <param name="value">The value to check for in the linked list.</param>
    /// <param name="comparer">The comparer to use when checking for equality.</param>
    /// <returns>The node that contains <paramref name="value" /> if it exists; otherwise, <c>null</c>.</returns>
    public readonly UnmanagedValueLinkedListNode<T>* Find<TComparer>(T value, TComparer comparer)
        where TComparer : IEqualityComparer<T>
    {
        var first = _first;
        var current = first;

        if (current is not null)
        {
            do
            {
                AssertNotNull(current);

                if (comparer.Equals(current->_value, value))
                {
                    return current;
                }

                current = current->_next;
            }
            while (current != first);
        }

        return null;
    }

    /// <summary>Tries to find the last node in the linked list that contains a specified value.</summary>
    /// <param name="value">The value to check for in the linked list.</param>
    /// <returns>The last node that contains <paramref name="value" /> if it exists; otherwise, <c>null</c>.</returns>
    public readonly UnmanagedValueLinkedListNode<T>* FindLast(T value) => FindLast(value, EqualityComparer<T>.Default);

    /// <summary>Tries to find the last node in the linked list that contains a specified value.</summary>
    /// <param name="value">The value to check for in the linked list.</param>
    /// <param name="comparer">The comparer to use when checking for equality.</param>
    /// <returns>The last node that contains <paramref name="value" /> if it exists; otherwise, <c>null</c>.</returns>
    public readonly UnmanagedValueLinkedListNode<T>* FindLast<TComparer>(T value, TComparer comparer)
        where TComparer : IEqualityComparer<T>
    {
        var first = _first;

        if (first is not null)
        {
            var last = first->_previous;
            var current = last;

            if (current is not null)
            {   
                do
                {
                    AssertNotNull(current);

                    if (comparer.Equals(current->_value, value))
                    {
                        return current;
                    }

                    current = current->_previous;
                }
                while (current != last);
            }
        }
        return null;
    }

    /// <summary>Gets an enumerator that can iterate through the items in the linked list.</summary>
    /// <returns>An enumerator that can iterate through the items in the linked list.</returns>
    public readonly ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine((nuint)_first, _count);

    /// <summary>Tries to remove a node in the linked list that contains a specified value.</summary>
    /// <param name="value">The value to check for in the linked list.</param>
    /// <returns><c>true</c> if a node containing <paramref name="value" /> was removed from the linked list; otherwise, <c>false</c>.</returns>
    public bool Remove(T value) => Remove(value, EqualityComparer<T>.Default);

    /// <summary>Tries to remove a node in the linked list that contains a specified value.</summary>
    /// <param name="value">The value to check for in the linked list.</param>
    /// <param name="comparer">The comparer to use when checking for equality.</param>
    /// <returns><c>true</c> if a node containing <paramref name="value" /> was removed from the linked list; otherwise, <c>false</c>.</returns>
    public bool Remove<TComparer>(T value, TComparer comparer)
        where TComparer : IEqualityComparer<T>
    {
        var node = Find(value, comparer);

        if (node is not null)
        {
            InternalRemoveNode(node);
            return true;
        }

        return false;
    }

    /// <summary>Removes a specified node from the linked list.</summary>
    /// <param name="node">The node to remove.</param>
    /// <exception cref="ArgumentNullException"><paramref name="node" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="node" /> does not have a parent.</exception>
    public void Remove(UnmanagedValueLinkedListNode<T>* node)
    {
        ValidateNode(node);
        InternalRemoveNode(node);
    }

    /// <summary>Removes the first node from the linked list.</summary>
    /// <exception cref="ArgumentNullException"><see cref="First" /> is <c>null</c>.</exception>
    public void RemoveFirst()
    {
        var first = _first;

        ThrowIfNull(first);
        InternalRemoveNode(first);
    }

    /// <summary>Removes the last node from the linked list.</summary>
    /// <exception cref="ArgumentNullException"><see cref="Last" /> is <c>null</c>.</exception>
    public void RemoveLast()
    {
        var last = Last;

        ThrowIfNull(last);
        InternalRemoveNode(last);
    }

    private void InternalInsertNodeBefore(UnmanagedValueLinkedListNode<T>* node, UnmanagedValueLinkedListNode<T>* newNode)
    {
        newNode->_next = node;

        var previousNode = node->_previous;
        newNode->_previous = previousNode;

        AssertNotNull(previousNode);

        previousNode->_next = newNode;
        node->_previous = newNode;

        _count++;
    }

    private void InternalInsertNodeToEmptyList(UnmanagedValueLinkedListNode<T>* newNode)
    {
        Assert(_first is null);
        Assert(_count == 0);

        newNode->_next = newNode;
        newNode->_previous = newNode;
        newNode->_isFirstNode = true;

        _first = newNode;
        _count++;
    }

    private void InternalRemoveNode(UnmanagedValueLinkedListNode<T>* node)
    {
        Assert(node->HasParent);
        Assert(Contains(node));
        Assert(_first is not null);

        var nextNode = node->_next;

        if (nextNode == node)
        {
            Assert(_count == 1);
            Assert(_first == node);

            _first = null;
        }
        else
        {
            var previousNode = node->_previous;

            AssertNotNull(nextNode);
            nextNode->_previous = previousNode;

            AssertNotNull(previousNode);
            previousNode->_next = nextNode;

            if (node == _first)
            {
                nextNode->_isFirstNode = true;
                _first = nextNode;
            }
        }

        node->Invalidate();
        _count--;
    }

    private readonly void ValidateNewNode(UnmanagedValueLinkedListNode<T>* node)
    {
        ThrowIfNull(node);

        if (node->HasParent)
        {
            ThrowForInvalidState(nameof(UnmanagedValueLinkedListNode<T>.HasParent));
        }
    }

    private readonly void ValidateNode(UnmanagedValueLinkedListNode<T>* node)
    {
        ThrowIfNull(node);

        if (node->HasParent)
        {
            Assert(Contains(node));
        }
        else
        {
            ThrowForInvalidState(nameof(UnmanagedValueLinkedListNode<T>.HasParent));
        }
    }

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
