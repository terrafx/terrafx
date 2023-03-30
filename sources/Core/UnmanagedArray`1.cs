// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX;

/// <summary>Represents a non-resizable collection of items of a given type.</summary>
/// <typeparam name="T">The type of the items contained by the array.</typeparam>
[DebuggerDisplay("Alignment = {Alignment}; IsNull = {IsNull}; Length = {Length}")]
[DebuggerTypeProxy(typeof(UnmanagedArray<>.DebugView))]
public readonly unsafe partial struct UnmanagedArray<T> : IDisposable, IEnumerable<T>
    where T : unmanaged
{
    /// <summary>Gets an empty array.</summary>
    public static UnmanagedArray<T> Empty { get; } = new UnmanagedArray<T>(
        (Metadata*)RuntimeHelpers.AllocateTypeAssociatedMemory(typeof(UnmanagedArray<T>), sizeof(Metadata))
    );

    private readonly Metadata* _data;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedArray{T}" /> struct.</summary>
    public UnmanagedArray()
    {
        _data = Empty._data;
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedArray{T}" /> struct.</summary>
    /// <param name="length">The length, in items, of the array.</param>
    /// <param name="alignment">The alignment, in bytes, of the items in the array or <c>zero</c> to use the system default.</param>
    /// <param name="zero"><c>true</c> if the items in the array should be zeroed; otherwise, <c>false</c>.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    public UnmanagedArray(nuint length, nuint alignment = 0, bool zero = false)
    {
        if (alignment == 0)
        {
            alignment = DefaultAlignment;
        }
        ThrowIfNotPow2(alignment);

        Metadata* data;

        if (length != 0)
        {
            // This allocates one more item than necessary, but this can be used to help detect buffer
            // overruns and ensures that get a ref to an empty array isn't "undefined behavior".

            data = (Metadata*)Allocate(
                SizeOf<Metadata>() + (length * SizeOf<T>()),
                alignment,
                offset: SizeOf<nuint>(),
                zero
            );

            data->Alignment = alignment;
            data->Length = length;
        }
        else
        {
            data = Empty._data;
        }

        _data = data;
    }

    private UnmanagedArray(Metadata* data)
    {
        AssertNotNull(data);
        _data = data;
    }

    /// <summary>Gets the alignment, in bytes, of the items in the array or <c>zero</c> which indicates the system default.</summary>
    public nuint Alignment
    {
        get
        {
            AssertNotNull(this);
            return _data->Alignment;
        }
    }

    /// <summary><c>true</c> if the array is <c>null</c>; otherwise, <c>false</c>.</summary>
    public bool IsNull => _data is null;

    /// <summary>Gets the length, in items, of the array.</summary>
    public nuint Length
    {
        get
        {
            AssertNotNull(this);
            return _data->Length;
        }
    }

    /// <summary>Gets a reference to the item at the specified index of the array.</summary>
    /// <param name="index">The index of the item to get or set.</param>
    /// <returns>The item that exists at <paramref name="index" /> in the array.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Length" />.</exception>
    public ref T this[nuint index] => ref *GetPointer(index);

    /// <summary>Implicitly converts the array to a span.</summary>
    /// <param name="array">The array to convert.</param>
    /// <returns>A span that covers the same memory as <paramref name="array" />.</returns>
    public static implicit operator UnmanagedSpan<T>(UnmanagedArray<T> array) => new UnmanagedSpan<T>(array);

    /// <summary>Implicitly converts the array to a readonly span.</summary>
    /// <param name="array">The array to convert.</param>
    /// <returns>A readonly span that covers the same memory as <paramref name="array" />.</returns>
    public static implicit operator UnmanagedReadOnlySpan<T>(UnmanagedArray<T> array) => new UnmanagedReadOnlySpan<T>(array);

    /// <inheritdoc />
    public void Dispose()
    {
        var data = _data;

        if (data != Empty._data)
        {
            Free(data);
        }
    }

    /// <summary>Converts the array to a span.</summary>
    /// <returns>A span that covers the array.</returns>
    public Span<T> AsSpan() => AsUnmanagedSpan().AsSpan();

    /// <summary>Converts the array to a span starting at the specified index.</summary>
    /// <param name="start">The index of the array at which the span should start.</param>
    /// <returns>A span that covers the array beginning at <paramref name="start" />.</returns>
    public Span<T> AsSpan(nuint start) => AsUnmanagedSpan(start).AsSpan();

    /// <summary>Converts the array to a span starting at the specified index and continuing for the specified number of items.</summary>
    /// <param name="start">The index of the array at which the span should start.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <returns>A span that covers the array beginning at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    public Span<T> AsSpan(nuint start, nuint length) => AsUnmanagedSpan(start, length).AsSpan();

    /// <summary>Converts the array to an unmanaged span.</summary>
    /// <returns>An unmanaged span that covers the array.</returns>
    public UnmanagedSpan<T> AsUnmanagedSpan() => new UnmanagedSpan<T>(this);

    /// <summary>Converts the array to an unmanaged span starting at the specified index.</summary>
    /// <param name="start">The index of the array at which the span should start.</param>
    /// <returns>An unmanaged span that covers the array beginning at <paramref name="start" />.</returns>
    public UnmanagedSpan<T> AsUnmanagedSpan(nuint start) => new UnmanagedSpan<T>(this, start);

    /// <summary>Converts the array to an unmanaged span starting at the specified index and continuing for the specified number of items.</summary>
    /// <param name="start">The index of the array at which the span should start.</param>
    /// <param name="length">The length, in items, of the span.</param>
    /// <returns>An unmanaged span that covers the array beginning at <paramref name="start" /> and continuing for <paramref name="length" /> items.</returns>
    public UnmanagedSpan<T> AsUnmanagedSpan(nuint start, nuint length) => new UnmanagedSpan<T>(this, start, length);

    /// <summary>Clears all items in the array to <c>zero</c>.</summary>
    public void Clear()
    {
        AssertNotNull(this);

        var data = _data;
        ClearArrayUnsafe(&data->Item, data->Length);
    }

    /// <summary>Copies the items in the array to a given destination.</summary>
    /// <param name="destination">The destination array where the items should be copied.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Length" /> is greater than <paramref name="destination" />.</exception>
    public void CopyTo(UnmanagedArray<T> destination)
    {
        AssertNotNull(this);

        var data = _data;
        var length = data->Length;

        ThrowIfNull(destination);
        ThrowIfNotInInsertBounds(length, destination._data->Length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(0), &data->Item, length);
    }

    /// <summary>Copies the items in the array to a given destination.</summary>
    /// <param name="destination">The destination span where the items should be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Length" /> is greater than <paramref name="destination" />.</exception>
    public void CopyTo(UnmanagedSpan<T> destination)
    {
        AssertNotNull(this);

        var data = _data;
        var length = data->Length;

        ThrowIfNotInInsertBounds(length, destination.Length);

        CopyArrayUnsafe(destination.GetPointerUnsafe(0), &data->Item, length);
    }

    /// <summary>Gets an enumerator that can iterate through the items in the array.</summary>
    /// <returns>An enumerator that can iterate through the items in the array.</returns>
    public Enumerator GetEnumerator() => new Enumerator(this);

    /// <summary>Gets a pointer to the item at the specified index of the array.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the array.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Length" />.</exception>
    public T* GetPointer(nuint index)
    {
        ThrowIfNotInBounds(index, _data->Length);
        return GetPointerUnsafe(index);
    }

    /// <summary>Gets a pointer to the item at the specified index of the array.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A pointer to the item that exists at <paramref name="index" /> in the array.</returns>
    /// <remarks>This method is unsafe because it does not validate that <paramref name="index" /> is less than <see cref="Length" />.</remarks>
    public T* GetPointerUnsafe(nuint index)
    {
        AssertNotNull(this);

        var data = _data;
        Assert(index <= data->Length);

        return &_data->Item + index;
    }

    /// <summary>Gets a reference to the item at the specified index of the array.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the array.</returns>
    public ref T GetReference(nuint index) => ref *GetPointer(index);

    /// <summary>Gets a reference to the item at the specified index of the array.</summary>
    /// <param name="index">The index of the item to get a pointer to.</param>
    /// <returns>A reference to the item that exists at <paramref name="index" /> in the array.</returns>
    /// <remarks>This method is unsafe because it does not validate that <paramref name="index" /> is less than <see cref="Length" />.</remarks>
    public ref T GetReferenceUnsafe(nuint index) => ref *GetPointerUnsafe(index);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
