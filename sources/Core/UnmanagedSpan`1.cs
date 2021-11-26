// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
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
    where T : unmanaged
{
    /// <summary>An empty span.</summary>
    public static readonly UnmanagedSpan<T> Empty = new UnmanagedSpan<T>();

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
        else
        {
            this = Empty;
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
            ThrowIfNotInBounds(start, array.Length, nameof(start), nameof(array.Length));

            _length = array.Length - start;
            _items = array.GetPointerUnsafe(start);
        }
        else
        {
            ThrowIfNotZero(start);
            this = Empty;
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
            ThrowIfNotInBounds(start, array.Length, nameof(start), nameof(array.Length));
            ThrowIfNotInBounds(start + length, array.Length, nameof(length), nameof(array.Length));

            _length = length;
            _items = array.GetPointerUnsafe(start);
        }
        else
        {
            ThrowIfNotZero(start);
            ThrowIfNotZero(length);
            this = Empty;
        }
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedSpan{T}" /> struct.</summary>
    /// <param name="pointer">A pointer to the first item the span will contain.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pointer" /> is null and <paramref name="length" /> is not <c>zero</c>.</exception>
    public UnmanagedSpan(T* pointer, nuint length)
    {
        if (pointer == null)
        {
            ThrowForLastErrorIfNotZero(length, nameof(length));
        }

        _length = length;
        _items = pointer;
    }

    /// <summary><c>true</c> if the span is empty; otherwise, <c>false</c>.</summary>
    public bool IsEmpty => _length == 0;

    /// <summary>Gets the length, in items, of the span.</summary>
    public nuint Length => _length;

    /// <summary>Gets or sets the item at the specified index of the span.</summary>
    /// <param name="index">The index of the item to get or set.</param>
    /// <returns>The item that exists at <paramref name="index" /> in the span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Length" />.</exception>
    public ref T this[nuint index]
        => ref AsRef<T>(GetPointer(index));

    /// <summary>Implicitly converts the span to a readonly span.</summary>
    /// <param name="span">The span to convert.</param>
    /// <returns>A readonly span that covers the same memory as <paramref name="span" />.</returns>
    public static implicit operator UnmanagedReadOnlySpan<T>(UnmanagedSpan<T> span)
        => new UnmanagedReadOnlySpan<T>(span);

    /// <summary>Clears all items in the span to <c>zero</c>.</summary>
    public void Clear() => ClearArrayUnsafe<T>(_items, _length);

    /// <summary>Copies the items in the span to a given destination.</summary>
    /// <param name="destination">The destination array where the items should be copied.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Length" /> is greater than <paramref name="destination" />.</exception>
    public void CopyTo(UnmanagedArray<T> destination)
    {
        var items = _items;
        var length = _length;

        ThrowIfNull(destination);
        ThrowIfNotInInsertBounds(length, destination.Length, nameof(Length), nameof(destination));

        CopyArrayUnsafe<T>(destination.GetPointerUnsafe(0), items, length);
    }

    /// <summary>Copies the items in the array to a given destination.</summary>
    /// <param name="destination">The destination span where the items should be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Length" /> is greater than <paramref name="destination" />.</exception>
    public void CopyTo(UnmanagedSpan<T> destination)
    {
        var items = _items;
        var length = _length;

        ThrowIfNotInInsertBounds(length, destination.Length, nameof(Length), nameof(destination));

        CopyArrayUnsafe<T>(destination.GetPointerUnsafe(0), items, length);
    }

    /// <summary>Gets a pointer to the item at the specified index of the span.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Length" />.</exception>
    public T* GetPointer(nuint index)
    {
        ThrowIfNotInBounds(index, Length, nameof(index), nameof(Length));
        return GetPointerUnsafe(index);
    }

    /// <summary>Gets a pointer to the item at the specified index of the span.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the span.</returns>
    /// <remarks>This method is unsafe because it does not validated that <paramref name="index" /> is less than <see cref="Length" />.</remarks>
    public T* GetPointerUnsafe(nuint index)
    {
        Assert(AssertionsEnabled && (index < Length));
        return _items + index;
    }

    /// <summary>Gets a reference to the item at the specified index of the span.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the span.</returns>
    /// <remarks>This method is unsafe because it does not validated that <paramref name="index" /> is less than <see cref="Length" />.</remarks>
    public ref T GetReferenceUnsafe(nuint index)
        => ref AsRef<T>(GetPointerUnsafe(index));

    /// <summary>Slices the span so that it begins at the specified index.</summary>
    /// <param name="start">The index at which the span should start.</param>
    /// <returns>A span that covers the same memory as the current span beginning at <paramref name="start" />.</returns>
    public UnmanagedSpan<T> Slice(nuint start)
    {
        ThrowIfNotInBounds(start, Length, nameof(start), nameof(Length));
        return new UnmanagedSpan<T>(_items + start, Length - start);
    }

    /// <summary>Slices the span so that it begins at the specified index and continuing for the specified number of items.</summary>
    /// <param name="start">The index at which the span should start.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <returns>A span that covers the same memory as the current span beginning at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    public UnmanagedSpan<T> Slice(nuint start, nuint length)
    {
        ThrowIfNotInBounds(start, Length, nameof(start), nameof(Length));
        ThrowIfNotInBounds(start + length, Length, nameof(length), nameof(Length));
        return new UnmanagedSpan<T>(_items + start, length);
    }
}
