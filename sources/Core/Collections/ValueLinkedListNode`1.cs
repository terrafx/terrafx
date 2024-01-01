// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ICollectionDebugView<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

namespace TerraFX.Collections;

/// <summary>Represents a node in a linked list.</summary>
/// <typeparam name="T">The type of the items contained in the linked list.</typeparam>
/// <remarks>Initializes a new instance of the <see cref="ValueLinkedListNode{T}" /> class.</remarks>
/// <param name="value">The value held by the node.</param>
public sealed class ValueLinkedListNode<T>(T value)
{
    internal ValueLinkedListNode<T>? _next;
    internal ValueLinkedListNode<T>? _previous;
    internal T _value = value;
    internal bool _isFirstNode;

    /// <summary>Gets <c>true</c> if the node belongs to a linked list; otherwise, <c>false</c>.</summary>
    public bool HasParent => _next is not null;

    /// <summary>Gets <c>true</c> if the node is the first node in a linked list; otherwise, <c>false</c>.</summary>
    public bool IsFirstNode => _isFirstNode;

    /// <summary>Gets the next node in the linked list or <c>null</c> if none exists.</summary>
    public ValueLinkedListNode<T>? Next
    {
        get
        {
            var next = _next;
            return ((next is not null) && !next._isFirstNode) ? next : null;
        }
    }

    /// <summary>Gets the previous node in the linked list or <c>null</c> if none exists.</summary>
    public ValueLinkedListNode<T>? Previous
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
    public ref T ValueRef => ref _value;

    internal void Invalidate()
    {
        _next = null;
        _previous = null;
        _isFirstNode = false;
    }
}
