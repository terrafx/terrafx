// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX;

/// <summary>Represents a non-resizable collection of items of a given type.</summary>
/// <typeparam name="T">The type of the items contained by the array.</typeparam>
/// <typeparam name="TData">The type of the additional data carried by the array.</typeparam>
[DebuggerDisplay("Alignment = {Alignment}; IsNull = {IsNull}; Length = {Length}")]
[DebuggerTypeProxy(typeof(UnmanagedArray<,>.DebugView))]
public readonly unsafe partial struct UnmanagedArray<T, TData>
    : IDisposable,
      IEnumerable<T>,
      IEquatable<UnmanagedArray<T, TData>>
    where T : unmanaged
    where TData : unmanaged
{
    internal static readonly UnmanagedArray<T, TData> s_empty = new UnmanagedArray<T, TData>(
        (Metadata*)RuntimeHelpers.AllocateTypeAssociatedMemory(typeof(UnmanagedArray<T, TData>), sizeof(Metadata))
    );

    internal readonly Metadata* _metadata;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedArray{T, TData}" /> struct.</summary>
    public UnmanagedArray()
    {
        this = s_empty;
    }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedArray{T, TData}" /> struct.</summary>
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

        Metadata* metadata;

        if (length != 0)
        {
            // This allocates one more item than necessary, but this can be used to help detect buffer
            // overruns and ensures that get a ref to an empty array isn't "undefined behavior"

            metadata = (Metadata*)Allocate(
                SizeOf<Metadata>() + (length * SizeOf<T>()),
                alignment,
                offset: Metadata.OffsetOf_Item,
                zero
            );

            metadata->Alignment = alignment;
            metadata->Length = length;

            _metadata = metadata;
        }
        else
        {
            this = s_empty;
        }
    }

    private UnmanagedArray(Metadata* metadata)
    {
        AssertNotNull(metadata);
        _metadata = metadata;
    }

    /// <summary>Gets the alignment, in bytes, of the items in the array or <c>zero</c> which indicates the system default.</summary>
    public nuint Alignment
    {
        get
        {
            AssertNotNull(this);
            return _metadata->Alignment;
        }
    }

    /// <summary>Gets the additional data carried by the array.</summary>
    public ref TData Data
    {
        get
        {
            AssertNotNull(this);
            return ref _metadata->Data;
        }
    }

    /// <summary><c>true</c> if the array is empty; otherwise, <c>false</c>.</summary>
    public bool IsEmpty => this == s_empty;

    /// <summary><c>true</c> if the array is <c>null</c>; otherwise, <c>false</c>.</summary>
    public bool IsNull => _metadata is null;

    /// <summary>Gets the length, in items, of the array.</summary>
    public nuint Length
    {
        get
        {
            AssertNotNull(this);
            return _metadata->Length;
        }
    }

    /// <summary>Gets a reference to the item at the specified index in the array.</summary>
    /// <param name="index">The index of the item to get or set.</param>
    /// <returns>The item that exists at <paramref name="index" /> in the array.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Length" />.</exception>
    public ref T this[nuint index] => ref *this.GetPointer(index);

    /// <summary>Compares two <see cref="UnmanagedArray{T, TData}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="UnmanagedArray{T, TData}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedArray{T, TData}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(UnmanagedArray<T, TData> left, UnmanagedArray<T, TData> right) => left._metadata == right._metadata;

    /// <summary>Compares two <see cref="UnmanagedArray{T, TData}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="UnmanagedArray{T, TData}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedArray{T, TData}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(UnmanagedArray<T, TData> left, UnmanagedArray<T, TData> right) => left._metadata != right._metadata;

    /// <summary>Implicitly converts the array to a span.</summary>
    /// <param name="array">The array to convert.</param>
    /// <returns>A span that covers the same memory as <paramref name="array" />.</returns>
    public static implicit operator UnmanagedSpan<T>(UnmanagedArray<T, TData> array)
        => !array.IsNull ? new UnmanagedSpan<T>(array.GetPointerUnsafe(), array.Length) : UnmanagedSpan.Empty<T>();

    /// <summary>Implicitly converts the array to a readonly span.</summary>
    /// <param name="array">The array to convert.</param>
    /// <returns>A readonly span that covers the same memory as <paramref name="array" />.</returns>
    public static implicit operator UnmanagedReadOnlySpan<T>(UnmanagedArray<T, TData> array) => (UnmanagedSpan<T>)array;

    /// <inheritdoc />
    public void Dispose()
    {
        if (this != s_empty)
        {
            Free(_metadata);
        }
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => (obj is UnmanagedArray<T, TData> other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(UnmanagedArray<T, TData> other) => this == other;

    /// <summary>Gets an enumerator that can iterate through the items in the array.</summary>
    /// <returns>An enumerator that can iterate through the items in the array.</returns>
    public Enumerator GetEnumerator() => new Enumerator(this);

    /// <inheritdoc />
    public override int GetHashCode() => ((nuint)_metadata).GetHashCode();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
