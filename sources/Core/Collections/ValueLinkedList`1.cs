// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the LinkedList<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Collections;

/// <summary>Represents a linked list of items.</summary>
/// <typeparam name="T">The type of the items contained in the linked list.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(ValueLinkedList<>.DebugView))]
public partial struct ValueLinkedList<T>
    : IEnumerable<T>,
      IEquatable<ValueLinkedList<T>>
{
    internal ValueLinkedListNode<T>? _first;
    internal int _count;

    /// <summary>Initializes a new instance of the <see cref="ValueLinkedList{T}" /> struct.</summary>
    /// <param name="source">The enumerable that is used to populate the linked list.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    public ValueLinkedList(IEnumerable<T> source)
    {
        ThrowIfNull(source);

        foreach (var value in source)
        {
            _ = this.AddLast(value);
        }
    }

    /// <summary>Initializes a new instance of the <see cref="ValueLinkedList{T}" /> struct.</summary>
    /// <param name="span">The span that is used to populate the linked list.</param>
    public ValueLinkedList(ReadOnlySpan<T> span)
    {
        foreach (var value in span)
        {
            _ = this.AddLast(value);
        }
    }

    /// <summary>Gets the number of items contained in the linked list.</summary>
    public readonly int Count => _count;

    /// <summary>Gets the first node in the linked list or <c>null</c> if the linked list is empty.</summary>
    public readonly ValueLinkedListNode<T>? First => _first;

    /// <summary>Gets the last node in the linked list or <c>null</c> if the linked list is empty.</summary>
    public readonly ValueLinkedListNode<T>? Last => _first?._previous;

    /// <summary>Compares two <see cref="ValueLinkedList{T}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="ValueLinkedList{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="ValueLinkedList{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(ValueLinkedList<T> left, ValueLinkedList<T> right)
    {
        return (left._first == right._first)
            && (left._count == right._count);
    }

    /// <summary>Compares two <see cref="ValueLinkedList{T}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="ValueLinkedList{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="ValueLinkedList{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(ValueLinkedList<T> left, ValueLinkedList<T> right) => !(left == right);

    /// <inheritdoc />
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is ValueLinkedList<T> other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(ValueLinkedList<T> other) => this == other;

    /// <summary>Gets an enumerator that can iterate through the items in the linked list.</summary>
    /// <returns>An enumerator that can iterate through the items in the linked list.</returns>
    public readonly ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(_first, _count);

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
