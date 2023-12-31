// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace TerraFX;

/// <summary>Represents a type and memory safe way to read a contiguous region of unmanaged memory.</summary>
/// <typeparam name="T">The type of items contained in the span.</typeparam>
/// <remarks>Initializes a new instance of the <see cref="UnmanagedReadOnlySpan{T}" /> struct.</remarks>
/// <param name="span">The underlying span for the readonly span.</param>
[DebuggerDisplay("IsEmpty = {IsEmpty}; Length = {Length}")]
[DebuggerTypeProxy(typeof(UnmanagedReadOnlySpan<>.DebugView))]
public readonly unsafe partial struct UnmanagedReadOnlySpan<T>(UnmanagedSpan<T> span)
    : IEnumerable<T>,
      IEquatable<UnmanagedReadOnlySpan<T>>
    where T : unmanaged
{
    internal readonly UnmanagedSpan<T> _value = span;

    /// <summary>Initializes a new instance of the <see cref="UnmanagedReadOnlySpan{T}" /> struct.</summary>
    /// <inheritdoc cref="UnmanagedSpan{T}.UnmanagedSpan(T*)" />
    public UnmanagedReadOnlySpan(T* firstItem)
        : this(new UnmanagedSpan<T>(firstItem)) { }

    /// <summary>Initializes a new instance of the <see cref="UnmanagedReadOnlySpan{T}" /> struct.</summary>
    /// <inheritdoc cref="UnmanagedSpan{T}.UnmanagedSpan(T*, nuint)" />
    public UnmanagedReadOnlySpan(T* firstItem, nuint length)
        : this(new UnmanagedSpan<T>(firstItem, length)) { }

    /// <inheritdoc cref="UnmanagedSpan{T}.IsEmpty" />
    public bool IsEmpty => _value.IsEmpty;

    /// <summary><c>true</c> if the span is <c>null</c>; otherwise, <c>false</c>.</summary>
    public bool IsNull => _value.IsNull;

    /// <inheritdoc cref="UnmanagedSpan{T}.Length" />
    public nuint Length => _value.Length;

    /// <summary>Gets or sets the item at the specified index of the span.</summary>
    /// <param name="index">The index of the item to get or set.</param>
    /// <returns>The item that exists at <paramref name="index" /> in the span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than or equal to <see cref="Length" />.</exception>
    public ref readonly T this[nuint index] => ref _value[index];

    /// <summary>Compares two <see cref="UnmanagedReadOnlySpan{T}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="UnmanagedReadOnlySpan{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedReadOnlySpan{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(UnmanagedReadOnlySpan<T> left, UnmanagedReadOnlySpan<T> right) => left._value == right._value;

    /// <summary>Compares two <see cref="UnmanagedReadOnlySpan{T}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="UnmanagedReadOnlySpan{T}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="UnmanagedReadOnlySpan{T}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(UnmanagedReadOnlySpan<T> left, UnmanagedReadOnlySpan<T> right) => left._value != right._value;

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => (obj is UnmanagedReadOnlySpan<T> other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(UnmanagedReadOnlySpan<T> other) => this == other;

    /// <inheritdoc cref="UnmanagedSpan{T}.GetEnumerator()" />
    public Enumerator GetEnumerator() => new Enumerator(this);

    /// <inheritdoc />
    public override int GetHashCode() => _value.GetHashCode();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
