// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace TerraFX;

/// <summary>Represents a type and memory safe way to read a contiguous region of unmanaged memory.</summary>
/// <typeparam name="T">The type of items contained in the span.</typeparam>
[DebuggerDisplay("IsEmpty = {IsEmpty}; Length = {Length}")]
[DebuggerTypeProxy(typeof(UnmanagedReadOnlySpan<>.DebugView))]
public readonly unsafe partial struct UnmanagedReadOnlySpan<T>
    : IEnumerable<T>,
      IEquatable<UnmanagedReadOnlySpan<T>>
    where T : unmanaged
{
    private readonly UnmanagedSpan<T> _span;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedReadOnlySpan{T}" /> struct.</summary>
    /// <inheritdoc cref="UnmanagedSpan{T}.UnmanagedSpan(UnmanagedArray{T})" />
    public UnmanagedReadOnlySpan(UnmanagedArray<T> array)
        : this(array.AsUnmanagedSpan()) { }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedReadOnlySpan{T}" /> struct.</summary>
    /// <inheritdoc cref="UnmanagedSpan{T}.UnmanagedSpan(UnmanagedArray{T}, nuint)" />
    public UnmanagedReadOnlySpan(UnmanagedArray<T> array, nuint start)
        : this(array.AsUnmanagedSpan(start)) { }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedReadOnlySpan{T}" /> struct.</summary>
    /// <inheritdoc cref="UnmanagedSpan{T}.UnmanagedSpan(UnmanagedArray{T}, nuint, nuint)" />
    public UnmanagedReadOnlySpan(UnmanagedArray<T> array, nuint start, nuint length)
        : this(array.AsUnmanagedSpan(start, length)) { }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedReadOnlySpan{T}" /> struct.</summary>
    /// <inheritdoc cref="UnmanagedSpan{T}.UnmanagedSpan(T*, nuint)" />
    public UnmanagedReadOnlySpan(T* firstItem, nuint length)
        : this(new UnmanagedSpan<T>(firstItem, length)) { }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedReadOnlySpan{T}" /> struct.</summary>
    /// <param name="span">The underlying span for the readonly span.</param>
    public UnmanagedReadOnlySpan(UnmanagedSpan<T> span)
    {
        _span = span;
    }

    /// <inheritdoc cref="UnmanagedSpan{T}.IsEmpty" />
    public bool IsEmpty => _span.IsEmpty;

    /// <inheritdoc cref="UnmanagedSpan{T}.Length" />
    public nuint Length => _span.Length;

    /// <summary>Gets or sets the item at the specified index of the span.</summary>
    /// <param name="index">The index of the item to get or set.</param>
    /// <returns>The item that exists at <paramref name="index" /> in the span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Length" />.</exception>
    public ref readonly T this[nuint index] => ref _span[index];

    /// <summary>Compares two <see cref="UnmanagedReadOnlySpan{T}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="UnmanagedReadOnlySpan{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedReadOnlySpan{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(UnmanagedReadOnlySpan<T> left, UnmanagedReadOnlySpan<T> right) => left._span == right._span;

    /// <summary>Compares two <see cref="UnmanagedReadOnlySpan{T}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="UnmanagedReadOnlySpan{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedReadOnlySpan{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(UnmanagedReadOnlySpan<T> left, UnmanagedReadOnlySpan<T> right) => left._span != right._span;

    /// <inheritdoc cref="UnmanagedSpan{T}.AsSpan()" />
    public ReadOnlySpan<T> AsSpan() => _span.AsSpan();

    /// <inheritdoc cref="UnmanagedSpan{T}.AsSpan(nuint)" />
    public ReadOnlySpan<T> AsSpan(nuint start) => _span.AsSpan(start);

    /// <inheritdoc cref="UnmanagedSpan{T}.AsSpan(nuint, nuint)" />
    public ReadOnlySpan<T> AsSpan(nuint start, nuint length) => _span.AsSpan(start, length);

    /// <inheritdoc cref="UnmanagedSpan{T}.CopyTo(UnmanagedArray{T})" />
    public void CopyTo(UnmanagedArray<T> destination) => _span.CopyTo(destination);

    /// <inheritdoc cref="UnmanagedSpan{T}.CopyTo(UnmanagedSpan{T})" />
    public void CopyTo(UnmanagedSpan<T> destination) => _span.CopyTo(destination);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => (obj is UnmanagedReadOnlySpan<T> other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(UnmanagedReadOnlySpan<T> other) => this == other;

    /// <inheritdoc cref="UnmanagedSpan{T}.GetEnumerator()" />
    public Enumerator GetEnumerator() => new Enumerator(this);

    /// <inheritdoc />
    public override int GetHashCode() => _span.GetHashCode();

    /// <inheritdoc cref="UnmanagedSpan{T}.GetPointer(nuint)" />
    public T* GetPointer(nuint index) => _span.GetPointer(index);

    /// <inheritdoc cref="UnmanagedSpan{T}.GetPointerUnsafe(nuint)" />
    public T* GetPointerUnsafe(nuint index) => _span.GetPointerUnsafe(index);

    /// <inheritdoc cref="UnmanagedSpan{T}.GetReferenceUnsafe(nuint)" />
    public ref readonly T GetReferenceUnsafe(nuint index) => ref _span.GetReferenceUnsafe(index);

    /// <inheritdoc cref="UnmanagedSpan{T}.Slice(nuint)" />
    public UnmanagedReadOnlySpan<T> Slice(nuint start) => _span.Slice(start);

    /// <inheritdoc cref="UnmanagedSpan{T}.Slice(nuint, nuint)" />
    public UnmanagedReadOnlySpan<T> Slice(nuint start, nuint length) => _span.Slice(start, length);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
