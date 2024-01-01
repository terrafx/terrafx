// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ValueDictionary<TKey, TValue> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using static TerraFX.Utilities.ExceptionUtilities;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Represents a collection of key/value pairs that are organized based on the key.</summary>
/// <typeparam name="TKey">The type of the keys contained in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values contained in the dictionary.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
[DebuggerTypeProxy(typeof(ValueDictionary<,>.DebugView))]
public partial struct ValueDictionary<TKey, TValue>
    : IEnumerable<KeyValuePair<TKey, TValue>>,
      IEquatable<ValueDictionary<TKey, TValue>>
    where TKey : notnull
{
    internal readonly IEqualityComparer<TKey>? _comparer;

    internal ulong _fastModMultiplier;

    internal int[] _buckets;
    internal Entry[] _entries;

    internal int _count;
    internal int _freeCount;
    internal int _freeList;

    /// <summary>Initializes a new instance of the <see cref="ValueDictionary{TKey, TValue}" /> struct.</summary>
    public ValueDictionary()
        : this(capacity: 0, comparer: null) { }

    /// <summary>Initializes a new instance of the <see cref="ValueDictionary{TKey, TValue}" /> struct.</summary>
    /// <param name="capacity">The initial capacity of the dictionary.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is <c>negative</c>.</exception>
    public ValueDictionary(int capacity)
        : this(capacity, comparer: null) { }

    /// <summary>Initializes a new instance of the <see cref="ValueDictionary{TKey, TValue}" /> struct.</summary>
    /// <param name="comparer">The comparer to use when comparing keys or <c>null</c> to use the default <see cref="EqualityComparer{T}" /> for <typeparamref name="TKey" />.</param>
    public ValueDictionary(IEqualityComparer<TKey>? comparer)
        : this(capacity: 0, comparer) { }

    /// <summary>Initializes a new instance of the <see cref="ValueDictionary{TKey, TValue}" /> struct.</summary>
    /// <param name="capacity">The initial capacity of the dictionary.</param>
    /// <param name="comparer">The comparer to use when comparing keys or <c>null</c> to use the default <see cref="EqualityComparer{T}" /> for <typeparamref name="TKey" />.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is <c>negative</c>.</exception>
    public ValueDictionary(int capacity, IEqualityComparer<TKey>? comparer)
    {
        ThrowIfNegative(capacity);

        _buckets = [];
        _entries = [];

        if (capacity > 0)
        {
            this.Initialize(capacity);
        }

        // For reference types, we always want to store a comparer instance, either
        // the one provided, or if one wasn't provided, the default (accessing
        // EqualityComparer<TKey>.Default with shared generics on every dictionary
        // access can add measurable overhead).  For value types, if no comparer is
        // provided, or if the default is provided, we'd prefer to use
        // EqualityComparer<TKey>.Default.Equals on every use, enabling the JIT to
        // de-virtualize and possibly inline the operation.

        if (!typeof(TKey).IsValueType)
        {
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
        }
        else if ((comparer is not null) && (comparer != EqualityComparer<TKey>.Default))
        {
            _comparer = comparer;
        }
    }

    /// <summary>Initializes a new instance of the <see cref="ValueDictionary{TKey, TValue}" /> struct.</summary>
    /// <param name="source">The enumerable that is used to populate the dictionary.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    public ValueDictionary(IEnumerable<KeyValuePair<TKey, TValue>> source)
        : this(source, comparer: null) { }

    /// <summary>Initializes a new instance of the <see cref="ValueDictionary{TKey, TValue}" /> struct.</summary>
    /// <param name="source">The enumerable that is used to populate the dictionary.</param>
    /// <param name="comparer">The comparer to use when comparing keys or <c>null</c> to use the default <see cref="EqualityComparer{T}" /> for <typeparamref name="TKey" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    public ValueDictionary(IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey>? comparer) :
        this(capacity: (source is not null) && source.TryGetNonEnumeratedCount(out var count) ? count : 0, comparer)
    {
        ThrowIfNull(source);
        
        foreach ((var key, var value) in source)
        {
            this.Add(key, value);
        }
    }

    /// <summary>Gets the comparer used when comparing keys.</summary>
    public readonly IEqualityComparer<TKey> Comparer => _comparer ?? EqualityComparer<TKey>.Default;

    /// <summary>Gets the number of items contained in the dictionary.</summary>
    public readonly int Count => _count - _freeCount;

    /// <summary>Gets the number of items that can be contained by the dictionary without being resized.</summary>
    public readonly int Capacity
    {
        get
        {
            var entries = _entries;
            return (entries is not null) ? entries.Length : 0;
        }
    }

    /// <summary>Gets or sets the value, associated with a specified key, in the dictionary.</summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <returns>The value associated with <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    /// <exception cref="KeyNotFoundException">On get, <paramref name="key" /> is not a valid key for the dictionary.</exception>
    public TValue this[TKey key]
    {
        readonly get
        {
            ref var entry = ref this.GetEntryReference(key);

            if (Unsafe.IsNullRef(in entry))
            {
                ThrowKeyNotFoundException(key, nameof(Dictionary<TKey, TValue>));
            }

            return entry.Value;
        }

        set
        {
            ref var entry = ref this.GetEntryReference(key, out bool _);
            entry.Value = value;
        }
    }

    /// <summary>Compares two <see cref="ValueDictionary{TKey, TValue}" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="ValueDictionary{TKey, TValue}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="ValueDictionary{TKey, TValue}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(ValueDictionary<TKey, TValue> left, ValueDictionary<TKey, TValue> right)
    {
        return (left._comparer == right._comparer)
            && (!Environment.Is64BitProcess || (left._fastModMultiplier == right._fastModMultiplier))
            && (left._buckets == right._buckets)
            && (left._entries == right._entries)
            && (left._count == right._count)
            && (left._freeCount == right._freeCount)
            && (left._freeList == right._freeList);
    }

    /// <summary>Compares two <see cref="ValueDictionary{TKey, TValue}" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="ValueDictionary{TKey, TValue}" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="ValueDictionary{TKey, TValue}" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(ValueDictionary<TKey, TValue> left, ValueDictionary<TKey, TValue> right) => !(left == right);

    /// <inheritdoc />
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is ValueDictionary<TKey, TValue> other) && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(ValueDictionary<TKey, TValue> other) => this == other;

    /// <summary>Gets an enumerator that can iterate through the items in the dictionary.</summary>
    /// <returns>An enumerator that can iterate through the items in the dictionary.</returns>
    public readonly ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        var hashCode = new HashCode();

        hashCode.Add(_comparer);

        if (Environment.Is64BitProcess)
        {
            hashCode.Add(_fastModMultiplier);
        }

        hashCode.Add(_buckets);
        hashCode.Add(_entries);

        hashCode.Add(_count);
        hashCode.Add(_freeList);
        hashCode.Add(_freeCount);

        return hashCode.ToHashCode();
    }

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    readonly IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();
}
