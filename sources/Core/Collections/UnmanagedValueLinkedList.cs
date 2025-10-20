// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the LinkedList<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="UnmanagedValueLinkedList{T}" /> struct.</summary>
public static unsafe class UnmanagedValueLinkedList
{
    /// <summary>Gets an empty linked list.</summary>
    public static UnmanagedValueLinkedList<T> Empty<T>()
        where T : unmanaged => new UnmanagedValueLinkedList<T>();

    /// <summary>Adds a new node containing a given value after the specified node.</summary>
    /// <param name="list">The list to which the node should be added.</param>
    /// <param name="node">The node after which the new node should be added.</param>
    /// <param name="value">The value which the new node contains.</param>
    /// <returns>The new node.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="node" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="node" /> does not have a parent.</exception>
    public static UnmanagedValueLinkedListNode<T>* AddAfter<T>(this ref UnmanagedValueLinkedList<T> list, UnmanagedValueLinkedListNode<T>* node, T value)
        where T : unmanaged
    {
        list.ValidateNode(node);

        var result = Allocate<UnmanagedValueLinkedListNode<T>>();
        result->_value = value;

        AssertNotNull(node->_next);
        list.InternalInsertNodeBefore(node->_next, result);

        Assert(result->HasParent);
        return result;
    }

    /// <summary>Adds a new node after the specified node.</summary>
    /// <param name="list">The list to which the node should be added.</param>
    /// <param name="node">The node after which <paramref name="newNode" /> should be added.</param>
    /// <param name="newNode">The new node.</param>
    /// <returns><paramref name="newNode" /></returns>
    /// <exception cref="ArgumentNullException"><paramref name="node" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="newNode" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="node" /> does not have a parent.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="newNode" /> already has a parent.</exception>
    public static UnmanagedValueLinkedListNode<T>* AddAfter<T>(this ref UnmanagedValueLinkedList<T> list, UnmanagedValueLinkedListNode<T>* node, UnmanagedValueLinkedListNode<T>* newNode)
        where T : unmanaged
    {
        list.ValidateNode(node);
        ValidateNewNode(newNode);

        AssertNotNull(node->_next);
        list.InternalInsertNodeBefore(node->_next, newNode);

        Assert(newNode->HasParent);
        return newNode;
    }

    /// <summary>Adds a new node containing a given value before the specified node.</summary>
    /// <param name="list">The list to which the node should be added.</param>
    /// <param name="node">The node before which the new node should be added.</param>
    /// <param name="value">The value which the new node contains.</param>
    /// <returns>The new node.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="node" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="node" /> does not have a parent.</exception>
    public static UnmanagedValueLinkedListNode<T>* AddBefore<T>(this ref UnmanagedValueLinkedList<T> list, UnmanagedValueLinkedListNode<T>* node, T value)
        where T : unmanaged
    {
        list.ValidateNode(node);

        var result = Allocate<UnmanagedValueLinkedListNode<T>>();
        result->_value = value;

        list.InternalInsertNodeBefore(node, result);

        if (node == list._first)
        {
            Assert(node->_isFirstNode);
            node->_isFirstNode = false;

            result->_isFirstNode = true;
            list._first = result;
        }

        Assert(result->HasParent);
        return result;
    }

    /// <summary>Adds a new node before the specified node.</summary>
    /// <param name="list">The list to which the node should be added.</param>
    /// <param name="node">The node before which <paramref name="newNode" /> should be added.</param>
    /// <param name="newNode">The new node.</param>
    /// <returns><paramref name="newNode" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="node" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="newNode" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="node" /> does not have a parent.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="newNode" /> already has a parent.</exception>
    public static UnmanagedValueLinkedListNode<T>* AddBefore<T>(this ref UnmanagedValueLinkedList<T> list, UnmanagedValueLinkedListNode<T>* node, UnmanagedValueLinkedListNode<T>* newNode)
        where T : unmanaged
    {
        list.ValidateNode(node);

        ValidateNewNode(newNode);
        list.InternalInsertNodeBefore(node, newNode);

        if (node == list._first)
        {
            Assert(node->_isFirstNode);
            node->_isFirstNode = false;

            newNode->_isFirstNode = true;
            list._first = newNode;
        }

        Assert(newNode->HasParent);
        return newNode;
    }

    /// <summary>Adds a new node containing a given value as the first node.</summary>
    /// <param name="list">The list to which the node should be added.</param>
    /// <param name="value">The value which the new node contains.</param>
    /// <returns>The new node.</returns>
    public static UnmanagedValueLinkedListNode<T>* AddFirst<T>(this ref UnmanagedValueLinkedList<T> list, T value)
        where T : unmanaged
    {
        var result = Allocate<UnmanagedValueLinkedListNode<T>>();
        result->_value = value;

        var first = list._first;

        if (first is null)
        {
            list.InternalInsertNodeToEmptyList(result);
        }
        else
        {
            list.InternalInsertNodeBefore(first, result);

            Assert(first->_isFirstNode);
            first->_isFirstNode = false;

            result->_isFirstNode = true;
            list._first = result;
        }

        Assert(result->HasParent);
        return result;
    }

    /// <summary>Adds a new node as the first node.</summary>
    /// <param name="list">The list to which the node should be added.</param>
    /// <param name="newNode">The new node.</param>
    /// <returns><paramref name="newNode" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="newNode" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="newNode" /> already has a parent.</exception>
    public static UnmanagedValueLinkedListNode<T>* AddFirst<T>(this ref UnmanagedValueLinkedList<T> list, UnmanagedValueLinkedListNode<T>* newNode)
        where T : unmanaged
    {
        ValidateNewNode(newNode);
        var first = list._first;

        if (first is null)
        {
            list.InternalInsertNodeToEmptyList(newNode);
        }
        else
        {
            list.InternalInsertNodeBefore(first, newNode);

            Assert(first->_isFirstNode);
            first->_isFirstNode = false;

            newNode->_isFirstNode = true;
            list._first = newNode;
        }

        Assert(newNode->HasParent);
        return newNode;
    }

    /// <summary>Adds a new node containing a given value as the last node.</summary>
    /// <param name="list">The list to which the node should be added.</param>
    /// <param name="value">The value which the new node contains.</param>
    /// <returns>The new node.</returns>
    public static UnmanagedValueLinkedListNode<T>* AddLast<T>(this ref UnmanagedValueLinkedList<T> list, T value)
        where T : unmanaged
    {
        var result = Allocate<UnmanagedValueLinkedListNode<T>>();
        result->_value = value;

        var first = list._first;

        if (first is null)
        {
            list.InternalInsertNodeToEmptyList(result);
        }
        else
        {
            list.InternalInsertNodeBefore(first, result);
        }

        Assert(result->HasParent);
        return result;
    }

    /// <summary>Adds a new node as the last node.</summary>
    /// <param name="list">The list to which the node should be added.</param>
    /// <param name="newNode">The new node.</param>
    /// <returns><paramref name="newNode" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="newNode" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="newNode" /> already has a parent.</exception>
    public static UnmanagedValueLinkedListNode<T>* AddLast<T>(this ref UnmanagedValueLinkedList<T> list, UnmanagedValueLinkedListNode<T>* newNode)
        where T : unmanaged
    {
        ValidateNewNode(newNode);

        var first = list._first;

        if (first is null)
        {
            list.InternalInsertNodeToEmptyList(newNode);
        }
        else
        {
            list.InternalInsertNodeBefore(first, newNode);
        }

        Assert(newNode->HasParent);
        return newNode;
    }

    /// <summary>Removes all items from the linked list.</summary>
    /// <param name="list">The list which should be cleared.</param>
    public static void Clear<T>(this ref UnmanagedValueLinkedList<T> list)
        where T : unmanaged
    {
        var current = list._first;

        while (current is not null)
        {
            var temp = current;
            current = current->_next;
            temp->Invalidate();
        }

        list._first = null;
        list._count = 0;
    }

    /// <summary>Checks whether the linked list contains a specified item.</summary>
    /// <param name="list">The list for which to check for the item.</param>
    /// <param name="value">The item to check for in the linked list.</param>
    /// <returns><c>true</c> if a node containing <paramref name="value" /> was found in the linked list; otherwise, <c>false</c>.</returns>
    public static bool Contains<T>(this ref readonly UnmanagedValueLinkedList<T> list, T value)
        where T : unmanaged => list.Contains(value, EqualityComparer<T>.Default);

    /// <summary>Checks whether the linked list contains a specified item.</summary>
    /// <param name="list">The list for which to check for the item.</param>
    /// <param name="value">The item to check for in the linked list.</param>
    /// <param name="comparer">The comparer to use when checking for equality.</param>
    /// <returns><c>true</c> if a node containing <paramref name="value" /> was found in the linked list; otherwise, <c>false</c>.</returns>
    public static bool Contains<T, TComparer>(this ref readonly UnmanagedValueLinkedList<T> list, T value, TComparer comparer)
        where T : unmanaged
        where TComparer : IEqualityComparer<T> => list.Find(value, comparer) is not null;

    /// <summary>Checks whether the linked list contains a specified node.</summary>
    /// <param name="list">The list for which to check for the item.</param>
    /// <param name="node">The node to check for in the linked list.</param>
    /// <returns><c>true</c> if <paramref name="node" /> was found in the linked list; otherwise, <c>false</c>.</returns>
    public static bool Contains<T>(this ref readonly UnmanagedValueLinkedList<T> list, UnmanagedValueLinkedListNode<T>* node)
        where T : unmanaged
    {
        if (node->HasParent)
        {
            var first = list._first;
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
    /// <param name="list">The list which should be copied.</param>
    /// <param name="destination">The destination span where the items should be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="UnmanagedValueLinkedList{T}.Count" /> is greater than <paramref name="destination" />.</exception>
    public static void CopyTo<T>(this ref readonly UnmanagedValueLinkedList<T> list, UnmanagedSpan<T> destination)
        where T : unmanaged
    {
        ThrowIfNotInInsertBounds(destination.Length, list._count);

        var first = list._first;
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

    /// <summary>Tries to find a node in the linked list that contains a specified value.</summary>
    /// <param name="list">The list which should be searched for the value.</param>
    /// <param name="value">The value to check for in the linked list.</param>
    /// <returns>The node that contains <paramref name="value" /> if it exists; otherwise, <c>null</c>.</returns>
    public static UnmanagedValueLinkedListNode<T>* Find<T>(this ref readonly UnmanagedValueLinkedList<T> list, T value)
        where T : unmanaged => list.Find(value, EqualityComparer<T>.Default);

    /// <summary>Tries to find a node in the linked list that contains a specified value.</summary>
    /// <param name="list">The list which should be searched for the value.</param>
    /// <param name="value">The value to check for in the linked list.</param>
    /// <param name="comparer">The comparer to use when checking for equality.</param>
    /// <returns>The node that contains <paramref name="value" /> if it exists; otherwise, <c>null</c>.</returns>
    public static UnmanagedValueLinkedListNode<T>* Find<T, TComparer>(this ref readonly UnmanagedValueLinkedList<T> list, T value, TComparer comparer)
        where T : unmanaged
        where TComparer : IEqualityComparer<T>
    {
        var first = list._first;
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
    /// <param name="list">The list which should be searched for the value.</param>
    /// <param name="value">The value to check for in the linked list.</param>
    /// <returns>The last node that contains <paramref name="value" /> if it exists; otherwise, <c>null</c>.</returns>
    public static UnmanagedValueLinkedListNode<T>* FindLast<T>(this ref readonly UnmanagedValueLinkedList<T> list, T value)
        where T : unmanaged => list.FindLast(value, EqualityComparer<T>.Default);

    /// <summary>Tries to find the last node in the linked list that contains a specified value.</summary>
    /// <param name="list">The list which should be searched for the value.</param>
    /// <param name="value">The value to check for in the linked list.</param>
    /// <param name="comparer">The comparer to use when checking for equality.</param>
    /// <returns>The last node that contains <paramref name="value" /> if it exists; otherwise, <c>null</c>.</returns>
    public static UnmanagedValueLinkedListNode<T>* FindLast<T, TComparer>(this ref readonly UnmanagedValueLinkedList<T> list, T value, TComparer comparer)
        where T : unmanaged
        where TComparer : IEqualityComparer<T>
    {
        var first = list._first;

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

    /// <summary>Tries to remove a node in the linked list that contains a specified value.</summary>
    /// <param name="list">The list from which the value should be removed.</param>
    /// <param name="value">The value to check for in the linked list.</param>
    /// <returns><c>true</c> if a node containing <paramref name="value" /> was removed from the linked list; otherwise, <c>false</c>.</returns>
    public static bool Remove<T>(this ref UnmanagedValueLinkedList<T> list, T value)
        where T : unmanaged => list.Remove(value, EqualityComparer<T>.Default);

    /// <summary>Tries to remove a node in the linked list that contains a specified value.</summary>
    /// <param name="list">The list from which the value should be removed.</param>
    /// <param name="value">The value to check for in the linked list.</param>
    /// <param name="comparer">The comparer to use when checking for equality.</param>
    /// <returns><c>true</c> if a node containing <paramref name="value" /> was removed from the linked list; otherwise, <c>false</c>.</returns>
    public static bool Remove<T, TComparer>(this ref UnmanagedValueLinkedList<T> list, T value, TComparer comparer)
        where T : unmanaged
        where TComparer : IEqualityComparer<T>
    {
        var node = list.Find(value, comparer);

        if (node is not null)
        {
            list.InternalRemoveNode(node);
            return true;
        }

        return false;
    }

    /// <summary>Removes a specified node from the linked list.</summary>
    /// <param name="list">The list from which the node should be removed.</param>
    /// <param name="node">The node to remove.</param>
    /// <exception cref="ArgumentNullException"><paramref name="node" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="node" /> does not have a parent.</exception>
    public static void Remove<T>(this ref UnmanagedValueLinkedList<T> list, UnmanagedValueLinkedListNode<T>* node)
        where T : unmanaged
    {
        list.ValidateNode(node);
        list.InternalRemoveNode(node);
    }

    /// <summary>Removes the first node from the linked list.</summary>
    /// <param name="list">The list from which the node should be removed.</param>
    /// <exception cref="ArgumentNullException"><see cref="UnmanagedValueLinkedList{T}.First" /> is <c>null</c>.</exception>
    public static void RemoveFirst<T>(this ref UnmanagedValueLinkedList<T> list)
        where T : unmanaged
    {
        var first = list._first;

        ThrowIfNull(first);
        list.InternalRemoveNode(first);
    }

    /// <summary>Removes the last node from the linked list.</summary>
    /// <param name="list">The list from which the node should be removed.</param>
    /// <exception cref="ArgumentNullException"><see cref="UnmanagedValueLinkedList{T}.Last" /> is <c>null</c>.</exception>
    public static void RemoveLast<T>(this ref UnmanagedValueLinkedList<T> list)
        where T : unmanaged
    {
        var last = list.Last;

        ThrowIfNull(last);
        list.InternalRemoveNode(last);
    }

    internal static void InternalInsertNodeBefore<T>(this ref UnmanagedValueLinkedList<T> list, UnmanagedValueLinkedListNode<T>* node, UnmanagedValueLinkedListNode<T>* newNode)
        where T : unmanaged
    {
        newNode->_next = node;

        var previousNode = node->_previous;
        newNode->_previous = previousNode;

        AssertNotNull(previousNode);

        previousNode->_next = newNode;
        node->_previous = newNode;

        list._count++;
    }

    internal static void InternalInsertNodeToEmptyList<T>(this ref UnmanagedValueLinkedList<T> list, UnmanagedValueLinkedListNode<T>* newNode)
        where T : unmanaged
    {
        Assert(list._first is null);
        Assert(list._count == 0);

        newNode->_next = newNode;
        newNode->_previous = newNode;
        newNode->_isFirstNode = true;

        list._first = newNode;
        list._count++;
    }

    internal static void InternalRemoveNode<T>(this ref UnmanagedValueLinkedList<T> list, UnmanagedValueLinkedListNode<T>* node)
        where T : unmanaged
    {
        Assert(node->HasParent);
        Assert(list.Contains(node));
        Assert(list._first is not null);

        var nextNode = node->_next;

        if (nextNode == node)
        {
            Assert(list._count == 1);
            Assert(list._first == node);

            list._first = null;
        }
        else
        {
            var previousNode = node->_previous;

            AssertNotNull(nextNode);
            nextNode->_previous = previousNode;

            AssertNotNull(previousNode);
            previousNode->_next = nextNode;

            if (node == list._first)
            {
                nextNode->_isFirstNode = true;
                list._first = nextNode;
            }
        }

        node->Invalidate();
        list._count--;
    }

    internal static void ValidateNewNode<T>(UnmanagedValueLinkedListNode<T>* node)
        where T : unmanaged
    {
        ThrowIfNull(node);

        if (node->HasParent)
        {
            ThrowForInvalidState(nameof(UnmanagedValueLinkedListNode<>.HasParent));
        }
    }

    internal static void ValidateNode<T>(this ref readonly UnmanagedValueLinkedList<T> list, UnmanagedValueLinkedListNode<T>* node)
        where T : unmanaged
    {
        ThrowIfNull(node);

        if (node->HasParent)
        {
            Assert(list.Contains(node));
        }
        else
        {
            ThrowForInvalidState(nameof(UnmanagedValueLinkedListNode<>.HasParent));
        }
    }
}
