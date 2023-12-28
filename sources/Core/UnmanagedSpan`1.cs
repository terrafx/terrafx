// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;
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
    private readonly nuint _length;
    private readonly T* _items;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedSpan{T}" /> struct.</summary>
    /// <param name="array">The array to create the span for.</param>
    public UnmanagedSpan(UnmanagedArray<T> array)
    {
        if (!array.IsNull)
        {
            _length = array.Length;
            _items = array.GetPointerUnsafe(0);
        }
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedSpan{T}" /> struct.</summary>
    /// <param name="array">The array to create the span for.</param>
    /// <param name="start">The index of <paramref name="array" /> at which the span should start.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than or equal to the length of <paramref name="array" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="array" /> is null and <paramref name="start" /> is not <c>zero</c>.</exception>
    public UnmanagedSpan(UnmanagedArray<T> array, nuint start)
    {
        if (!array.IsNull)
        {
            ThrowIfNotInBounds(start, array.Length);

            _length = array.Length - start;
            _items = array.GetPointerUnsafe(start);
        }
        else
        {
            ThrowIfNotZero(start);
        }
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedSpan{T}" /> struct.</summary>
    /// <param name="array">The array to create the span for.</param>
    /// <param name="start">The index of <paramref name="array" /> at which the span should start.</param>
    /// <param name="length">The length, in items, of the span beginning at <paramref name="start" />.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than or equal to the length of <paramref name="array" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> plus <paramref name="length" /> is greater than or equal to the length of <paramref name="array" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="array" /> is null and <paramref name="start" /> is not <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="array" /> is null and <paramref name="length" /> is not <c>zero</c>.</exception>
    public UnmanagedSpan(UnmanagedArray<T> array, nuint start, nuint length)
    {
        if (!array.IsNull)
        {
            ThrowIfNotInBounds(start, array.Length);
            ThrowIfNotInInsertBounds(length, array.Length - start);

            _length = length;
            _items = array.GetPointerUnsafe(start);
        }
        else
        {
            ThrowIfNotZero(start);
            ThrowIfNotZero(length);
        }
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedSpan{T}" /> struct.</summary>
    /// <param name="firstItem">A pointer to the first item the span will contain.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="firstItem" /> is null and <paramref name="length" /> is not <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length" /> is greater than the remaining amount of address space.</exception>
    public UnmanagedSpan(T* firstItem, nuint length)
    {
        if (firstItem == null)
        {
            ThrowIfNotZero(length);
        }
        ThrowIfNotInInsertBounds(length * SizeOf<T>(), nuint.MaxValue - (nuint)firstItem + 1);

        _length = length;
        _items = firstItem;
    }

    /// <summary><c>true</c> if the span is empty; otherwise, <c>false</c>.</summary>
    public bool IsEmpty => _length == 0;

    /// <summary>Gets the length, in items, of the span.</summary>
    public nuint Length => _length;

    /// <summary>Gets or sets the item at the specified index of the span.</summary>
    /// <param name="index">The index of the item to get or set.</param>
    /// <returns>The item that exists at <paramref name="index" /> in the span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Length" />.</exception>
    public ref T this[nuint index] => ref *GetPointer(index);

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

    /// <summary>Converts the unmanaged span to a span.</summary>
    /// <returns>A span that covers the unmanaged span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Length" /> is greater than <see cref="int.MaxValue" />.</exception>
    public Span<T> AsSpan()
    {
        var length = _length;
        ThrowIfNotInInsertBounds(length, int.MaxValue);
        return new Span<T>(_items, (int)length);
    }

    /// <summary>Converts the array to a span starting at the specified index.</summary>
    /// <param name="start">The index of the array at which the span should start.</param>
    /// <returns>A span that covers the array beginning at <paramref name="start" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Length" /> is greater than <see cref="int.MaxValue" />.</exception>
    public Span<T> AsSpan(nuint start)
    {
        var length = _length;
        ThrowIfNotInBounds(start, length);

        length -= start;
        ThrowIfNotInInsertBounds(length, int.MaxValue);

        return new Span<T>(_items + start, (int)length);
    }

    /// <summary>Converts the array to a span starting at the specified index and continuing for the specified number of items.</summary>
    /// <param name="start">The index of the array at which the span should start.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <returns>A span that covers the array beginning at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    public Span<T> AsSpan(nuint start, nuint length)
    {
        var thisLength = _length;

        ThrowIfNotInBounds(start, thisLength);
        ThrowIfNotInInsertBounds(length, thisLength - start);
        ThrowIfNotInInsertBounds(length, int.MaxValue);

        return new Span<T>(_items + start, (int)length);
    }

    /// <summary>Clears all items in the span to <c>zero</c>.</summary>
    public void Clear() => ClearArrayUnsafe(_items, _length);

    /// <summary>Copies the items in the span to a given destination.</summary>
    /// <param name="destination">The destination array where the items should be copied.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Length" /> is greater than <paramref name="destination" />.</exception>
    public void CopyTo(UnmanagedArray<T> destination)
    {
        var items = _items;
        var length = _length;

        ThrowIfNull(destination);
        ThrowIfNotInInsertBounds(length, destination.Length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(0), items, length);
    }

    /// <summary>Copies the items in the array to a given destination.</summary>
    /// <param name="destination">The destination span where the items should be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Length" /> is greater than <paramref name="destination" />.</exception>
    public void CopyTo(UnmanagedSpan<T> destination)
    {
        var items = _items;
        var length = _length;

        ThrowIfNotInInsertBounds(length, destination._length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(0), items, length);
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => (obj is UnmanagedSpan<T> other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(UnmanagedSpan<T> other) => this == other;

    /// <summary>Gets an enumerator that can iterate through the items in the span.</summary>
    /// <returns>An enumerator that can iterate through the items in the span.</returns>
    public Enumerator GetEnumerator() => new Enumerator(this);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(_length, (nuint)_items);

    /// <summary>Gets a pointer to the item at the specified index of the span.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Length" />.</exception>
    public T* GetPointer(nuint index)
    {
        ThrowIfNotInBounds(index, _length);
        return GetPointerUnsafe(index);
    }

    /// <summary>Gets a pointer to the item at the specified index of the span.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the span.</returns>
    /// <remarks>This method is unsafe because it does not validate that <paramref name="index" /> is less than <see cref="Length" />.</remarks>
    public T* GetPointerUnsafe(nuint index)
    {
        Assert(index < _length);
        return _items + index;
    }

    /// <summary>Gets a reference to the item at the specified index of the span.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the span.</returns>
    public ref T GetReference(nuint index) => ref *GetPointer(index);

    /// <summary>Gets a reference to the item at the specified index of the span.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the span.</returns>
    /// <remarks>This method is unsafe because it does not validate that <paramref name="index" /> is less than <see cref="Length" />.</remarks>
    public ref T GetReferenceUnsafe(nuint index) => ref *GetPointerUnsafe(index);

    /// <summary>Slices the span so that it begins at the specified index.</summary>
    /// <param name="start">The index at which the span should start.</param>
    /// <returns>A span that covers the same memory as the current span beginning at <paramref name="start" />.</returns>
    public UnmanagedSpan<T> Slice(nuint start)
    {
        var length = _length;
        ThrowIfNotInBounds(start, length);
        return new UnmanagedSpan<T>(_items + start, length - start);
    }

    /// <summary>Slices the span so that it begins at the specified index and continuing for the specified number of items.</summary>
    /// <param name="start">The index at which the span should start.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <returns>A span that covers the same memory as the current span beginning at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    public UnmanagedSpan<T> Slice(nuint start, nuint length)
    {
        var thisLength = _length;
        ThrowIfNotInBounds(start, thisLength);

        ThrowIfNotInInsertBounds(length, thisLength - start);
        return new UnmanagedSpan<T>(_items + start, length);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
