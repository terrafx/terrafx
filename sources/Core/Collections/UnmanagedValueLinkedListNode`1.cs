// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ICollectionDebugView<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TerraFX.Collections;

/// <summary>Represents a node in a linked list.</summary>
/// <typeparam name="T">The type of the items contained in the linked list.</typeparam>
/// <remarks>Initializes a new instance of the <see cref="UnmanagedValueLinkedListNode{T}" /> class.</remarks>
/// <param name="value">The value held by the node.</param>
public unsafe struct UnmanagedValueLinkedListNode<T>(T value) : IEquatable<UnmanagedValueLinkedListNode<T>>
    where T : unmanaged
{
    internal UnmanagedValueLinkedListNode<T>* _next;
    internal UnmanagedValueLinkedListNode<T>* _previous;
    internal T _value = value;
    internal bool _isFirstNode;

    /// <summary>Gets <c>true</c> if the node belongs to a linked list; otherwise, <c>false</c>.</summary>
    public bool HasParent => _next is not null;

    /// <summary>Gets <c>true</c> if the node is the first node in a linked list; otherwise, <c>false</c>.</summary>
    public bool IsFirstNode => _isFirstNode;

    /// <summary>Gets the next node in the linked list or <c>null</c> if none exists.</summary>
    public UnmanagedValueLinkedListNode<T>* Next
    {
        get
        {
            var next = _next;
            return ((next is not null) && !next->_isFirstNode) ? next : null;
        }
    }

    /// <summary>Gets the previous node in the linked list or <c>null</c> if none exists.</summary>
    public UnmanagedValueLinkedListNode<T>* Previous
    {
        get
        {
            var previous = _previous;
            return ((previous is not null) && !_isFirstNode) ? previous : null;
        }
    }

    /// <summary>Gets or sets the value held by the node.</summary>
    public T Value
    {
        get
        {
            return _value;
        }

        set
        {
            _value = value;
        }
    }

    /// <summary>Gets a reference to the value held by the node.</summary>
    [UnscopedRef]
    public ref T ValueRef => ref _value;

    /// <summary>Compares two <see cref="UnmanagedValueLinkedListNode{T}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="UnmanagedValueLinkedListNode{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedValueLinkedListNode{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(UnmanagedValueLinkedListNode<T> left, UnmanagedValueLinkedListNode<T> right)
    {
        return (left._next == right._next)
            && (left._previous == right._previous)
            && EqualityComparer<T>.Default.Equals(left._value, right._value)
            && (left._isFirstNode == right._isFirstNode);
    }

    /// <summary>Compares two <see cref="UnmanagedValueLinkedListNode{T}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="UnmanagedValueLinkedListNode{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedValueLinkedListNode{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(UnmanagedValueLinkedListNode<T> left, UnmanagedValueLinkedListNode<T> right)
    {
        return (left._next != right._next)
            || (left._previous != right._previous)
            || !EqualityComparer<T>.Default.Equals(left._value, right._value)
            || (left._isFirstNode != right._isFirstNode);
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => (obj is UnmanagedValueLinkedListNode<T> other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(UnmanagedValueLinkedListNode<T> other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine((nuint)_next, (nuint)_previous, _value, _isFirstNode);

    internal void Invalidate()
    {
        _next = null;
        _previous = null;
        _isFirstNode = false;
    }
}
