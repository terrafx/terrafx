// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX;

/// <summary>Represents a type and memory safe way to read and write a contiguous region of unmanaged memory.</summary>
/// <typeparam name="T">The type of items contained in the span.</typeparam>
[DebuggerDisplay("IsEmpty = {IsEmpty}; Length = {Length}")]
[DebuggerTypeProxy(typeof(UnmanagedSpan<>.DebugView))]
public readonly unsafe partial struct UnmanagedSpan<T>
    : IEnumerable<T>,
      IEquatable<UnmanagedSpan<T>>
    where T : unmanaged
{
    internal readonly nuint _length;
    internal readonly T* _items;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedSpan{T}" /> struct.</summary>
    /// <param name="item">A pointer to the item the span will contain.</param>
    /// <exception cref="ArgumentOutOfRangeException"><c>1</c> is greater than the remaining amount of address space.</exception>
    public UnmanagedSpan(T* item)
    {
        if (item is not null)
        {
            ThrowIfNotInInsertBounds(SizeOf<T>(), nuint.MaxValue - (nuint)item + 1);

            _length = 1;
            _items = item;
        }
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedSpan{T}" /> struct.</summary>
    /// <param name="firstItem">A pointer to the first item the span will contain.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="firstItem" /> is null and <paramref name="length" /> is not <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length" /> is greater than the remaining amount of address space.</exception>
    public UnmanagedSpan(T* firstItem, nuint length)
    {
        if (firstItem is null)
        {
            ThrowIfNotZero(length);
        }
        ThrowIfNotInInsertBounds(length * SizeOf<T>(), nuint.MaxValue - (nuint)firstItem + 1);

        _length = length;
        _items = firstItem;
    }

    /// <summary><c>true</c> if the span is empty; otherwise, <c>false</c>.</summary>
    public bool IsEmpty => _length == 0;

    /// <summary><c>true</c> if the span is <c>null</c>; otherwise, <c>false</c>.</summary>
    public bool IsNull => _items is null;

    /// <summary>Gets the length, in items, of the span.</summary>
    public nuint Length => _length;

    /// <summary>Gets or sets the item at the specified index in the span.</summary>
    /// <param name="index">The index of the item to get or set.</param>
    /// <returns>The item that exists at <paramref name="index" /> in the span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Length" />.</exception>
    public ref T this[nuint index] => ref *this.GetPointer(index);

    /// <summary>Compares two <see cref="UnmanagedSpan{T}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="UnmanagedSpan{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedSpan{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(UnmanagedSpan<T> left, UnmanagedSpan<T> right)
        => (left._length == right._length)
        && (left._items == right._items);

    /// <summary>Compares two <see cref="UnmanagedSpan{T}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="UnmanagedSpan{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedSpan{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(UnmanagedSpan<T> left, UnmanagedSpan<T> right)
        => (left._length != right._length)
        || (left._items != right._items);

    /// <summary>Implicitly converts the span to a readonly span.</summary>
    /// <param name="span">The span to convert.</param>
    /// <returns>A readonly span that covers the same memory as <paramref name="span" />.</returns>
    public static implicit operator UnmanagedReadOnlySpan<T>(UnmanagedSpan<T> span) => new UnmanagedReadOnlySpan<T>(span);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => (obj is UnmanagedSpan<T> other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(UnmanagedSpan<T> other) => this == other;

    /// <summary>Gets an enumerator that can iterate through the items in the span.</summary>
    /// <returns>An enumerator that can iterate through the items in the span.</returns>
    public Enumerator GetEnumerator() => new Enumerator(this);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(_length, (nuint)_items);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
