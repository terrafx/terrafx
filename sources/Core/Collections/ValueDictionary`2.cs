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
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Represents a collection of key/value pairs that are organized based on the key.</summary>
/// <typeparam name="TKey">The type of the keys contained in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values contained in the dictionary.</typeparam>
/// <remarks>This type is meant to be used as an implementation detail of another type and should not be part of your public surface area.</remarks>
[DebuggerDisplay("Capacity = {Capacity}; Count = {Count}")]
[DebuggerTypeProxy(typeof(ValueDictionary<,>.DebugView))]
public partial struct ValueDictionary<TKey, TValue>
    : IEnumerable<(TKey Key, TValue Value)>,
      IEquatable<ValueDictionary<TKey, TValue>>
    where TKey : notnull
{
    private const int StartOfFreeList = -3;

    private readonly IEqualityComparer<TKey>? _comparer;

    private ulong _fastModMultiplier;

    private int[] _buckets;
    private Entry[] _entries;

    private int _count;
    private int _freeCount;
    private int _freeList;

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
            Initialize(capacity);
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
    public ValueDictionary(IEnumerable<(TKey Key, TValue Value)> source)
        : this(source, comparer: null) { }

    /// <summary>Initializes a new instance of the <see cref="ValueDictionary{TKey, TValue}" /> struct.</summary>
    /// <param name="source">The enumerable that is used to populate the dictionary.</param>
    /// <param name="comparer">The comparer to use when comparing keys or <c>null</c> to use the default <see cref="EqualityComparer{T}" /> for <typeparamref name="TKey" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    public ValueDictionary(IEnumerable<(TKey Key, TValue Value)> source, IEqualityComparer<TKey>? comparer) :
        this(capacity: (source is not null) && source.TryGetNonEnumeratedCount(out var count) ? count : 0, comparer)
    {
        ThrowIfNull(source);
        
        foreach ((var key, var value) in source)
        {
            Add(key, value);
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
            ref var entry = ref GetEntryReference(key);

            if (Unsafe.IsNullRef(in entry))
            {
                ThrowKeyNotFoundException(key, nameof(Dictionary<TKey, TValue>));
            }

            return entry.Value;
        }

        set
        {
            ref var entry = ref GetEntryReference(key, out bool _);
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

    /// <summary>Adds a value, associated with the specified key, to the dictionary.</summary>
    /// <param name="key">The key of the value to add to the dictionary.</param>
    /// <param name="value">The value to associate with <paramref name="key" />.</param>
    /// <exception cref="ArgumentException">A value associated with <paramref name="key" /> already exists in the dictionary.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public void Add(TKey key, TValue value)
    {
        ref var entry = ref GetEntryReference(key, out bool existing);

        if (existing)
        {
            ThrowForDictionaryExistingKey(key);
        }

        entry.Value = value;
    }

    /// <summary>Adds a value, associated with the specified key, to the dictionary.</summary>
    /// <param name="key">The key of the value to add to the dictionary.</param>
    /// <param name="value">The value to associate with <paramref name="key" />.</param>
    /// <param name="mutex">The mutex to use when adding <paramref name="key" /> to the list.</param>
    /// <exception cref="ArgumentException">A value associated with <paramref name="key" /> already exists in the dictionary.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public void Add(TKey key, TValue value, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        Add(key, value);
    }

    /// <summary>Removes all items from the dictionary.</summary>
    public void Clear()
    {
        var count = _count;

        if (count > 0)
        {
            Array.Clear(_buckets);
            Array.Clear(_entries, 0, count);

            _count = 0;
            _freeCount = 0;
            _freeList = -1;
        }
    }

    /// <summary>Checks whether the dictionary contains a specified key.</summary>
    /// <param name="key">The key to check for in the dictionary.</param>
    /// <returns><c>true</c> if <paramref name="key" /> was found in the dictionary; otherwise, <c>false</c>.</returns>
    public readonly bool ContainsKey(TKey key)
    {
        ref var entry = ref GetEntryReference(key);
        return !Unsafe.IsNullRef(in entry);
    }

    /// <summary>Checks whether the dictionary contains a specified value.</summary>
    /// <param name="value">The value to check for in the dictionary.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was found in the dictionary; otherwise, <c>false</c>.</returns>
    public readonly bool ContainsValue(TValue value)
    {
        var entries = _entries;
        var count = _count;

        if (value is null)
        {
            for (var i = 0; i < count; i++)
            {
                ref var entry = ref entries.GetReferenceUnsafe(i);

                if ((entry.Next >= -1) && (entry.Value is null))
                {
                    return true;
                }
            }
        }
        else if (typeof(TValue).IsValueType)
        {
            // ValueType: De-virtualize with EqualityComparer<TValue>.Default intrinsic
            for (var i = 0; i < count; i++)
            {
                ref var entry = ref entries.GetReferenceUnsafe(i);

                if ((entry.Next >= -1) && EqualityComparer<TValue>.Default.Equals(entry.Value, value))
                {
                    return true;
                }
            }
        }
        else
        {
            // Object type: Shared Generic, EqualityComparer<TValue>.Default won't de-virtualize
            // https://github.com/dotnet/runtime/issues/10050
            // So cache in a local rather than get EqualityComparer per loop iteration
            var comparer = EqualityComparer<TValue>.Default;

            for (var i = 0; i < count; i++)
            {
                ref var entry = ref entries.GetReferenceUnsafe(i);

                if ((entry.Next >= -1) && comparer.Equals(entry.Value, value))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>Copies the items of the dictionary to a span.</summary>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentException"><see cref="Count" /> is greater than the length of <paramref name="destination" />.</exception>
    private readonly void CopyTo(Span<(TKey Key, TValue Value)> destination)
    {
        var count = _count;
        ThrowIfNotInInsertBounds(count, destination.Length);

        var entries = _entries;

        for (var i = 0; i < count; i++)
        {
            ref var entry = ref entries.GetReferenceUnsafe(i);

            if (entry.Next >= -1)
            {
                destination.GetReferenceUnsafe(i) = (entry.Key, entry.Value);
            }
        }
    }

    /// <summary>Ensures the capacity of the dictionary is at least the specified value.</summary>
    /// <param name="capacity">The minimum capacity the dictionary should support.</param>
    /// <remarks>This method does not throw if <paramref name="capacity" /> is negative and instead does nothing.</remarks>
    public void EnsureCapacity(int capacity)
    {
        var currentCapacity = Capacity;

        if (capacity > currentCapacity)
        {
            if (_buckets is null)
            {
                Initialize(capacity);
            }
            Resize(HashUtilities.GetPrime(capacity));
        }
    }

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

    /// <summary>Gets a reference to the value, associated with a specified key, in the dictionary.</summary>
    /// <param name="key">The key of the value to get a reference to.</param>
    /// <returns>A reference to the value, associated with <paramref name="key" />; otherwise, <see cref="Unsafe.NullRef{T}" /> if the dictionary did not contain <paramref name="key" />.</returns>
    /// <remarks>This method is unsafe because other operations may invalidate the backing data.</remarks>
    public readonly ref TValue GetValueReferenceUnsafe(TKey key)
    {
        ref var entry = ref GetEntryReference(key);
        return ref !Unsafe.IsNullRef(in entry) ? ref entry.Value : ref Unsafe.NullRef<TValue>();
    }

    /// <summary>Gets a reference to the value, associated with a specified key, in the dictionary.</summary>
    /// <param name="key">The key of the value to get a reference to.</param>
    /// <param name="existing">On return, contains <c>true</c> if the value was found in the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="key" />.</param>
    /// <returns>A reference to the value, associated with <paramref name="key" />; otherwise, a reference to the newly added value if the dictionary did not contain <paramref name="key" />.</returns>
    /// <remarks>This method is unsafe because other operations may invalidate the backing data.</remarks>
    public readonly ref TValue GetValueReferenceUnsafe(TKey key, out bool existing)
    {
        ref var entry = ref GetEntryReference(key);
        existing = !Unsafe.IsNullRef(in entry);
        return ref existing ? ref entry.Value : ref Unsafe.NullRef<TValue>();
    }

    /// <summary>Removes the value, associated with a specified key, from the dictionary.</summary>
    /// <param name="key">The key of the value to remove from the dictionary.</param>
    /// <returns><c>true</c> if a value associated with <paramref name="key" /> was removed from the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="key" />.</returns>
    public bool Remove(TKey key) => Remove(key, out _);

    /// <summary>Removes the value, associated with a specified key, from the dictionary.</summary>
    /// <param name="key">The key of the value to remove from the dictionary.</param>
    /// <param name="mutex">The mutex to use when removing <paramref name="key" /> from the dictionary.</param>
    /// <returns><c>true</c> if a value associated with <paramref name="key" /> was removed from the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="key" />.</returns>
    public bool Remove(TKey key, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return Remove(key);
    }

    /// <summary>Removes the value, associated with a specified key, from the dictionary.</summary>
    /// <param name="key">The key of the value to remove from the dictionary.</param>
    /// <param name="value">On return, contains the value associated with <paramref name="key" /> if it was found in the dictionary; otherwise, <c>default</c>.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was removed from the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public bool Remove(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        ref var entry = ref GetEntryReference(key, out int lastIndex);
        var existing = !Unsafe.IsNullRef(in entry);

        if (existing)
        {
            int index;

            if (lastIndex < 0)
            {
                int hashCode;
                var comparer = _comparer;

                if (typeof(TKey).IsValueType && (comparer is null))
                {
                    hashCode = key.GetHashCode();
                }
                else
                {
                    AssertNotNull(comparer);
                    hashCode = comparer.GetHashCode(key);
                }

                ref var bucket = ref GetBucketReference(_buckets, hashCode);

                index = bucket - 1;
                bucket = entry.Next + 1; // Value in buckets is 1-based
            }
            else
            {
                ref var lastEntry = ref _entries.GetReferenceUnsafe(lastIndex);
                index = lastEntry.Next;
                lastEntry.Next = entry.Next;
            }

            value = entry.Value;

            // Shouldn't underflow because max hash table length is MaxPrimeArrayLength = 0x7FEFFFFD(2146435069), while _freeList underflow threshold is 2147483646
            var next = StartOfFreeList - _freeList;
            Assert(next < 0);
            entry.Next = next;

            if (RuntimeHelpers.IsReferenceOrContainsReferences<TKey>())
            {
                entry.Key = default!;
            }

            if (RuntimeHelpers.IsReferenceOrContainsReferences<TValue>())
            {
                entry.Value = default!;
            }

            _freeList = index;
            _freeCount++;
        }
        else
        {
            value = default;
        }

        return existing;
    }

    /// <summary>Removes the value, associated with a specified key, from the dictionary.</summary>
    /// <param name="key">The key of the value to remove from the dictionary.</param>
    /// <param name="mutex">The mutex to use when removing <paramref name="key" /> from the dictionary.</param>
    /// <param name="value">On return, contains the value associated with <paramref name="key" /> if it was found in the dictionary; otherwise, <c>default</c>.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was removed from the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public bool Remove(TKey key, ValueMutex mutex, [MaybeNullWhen(false)] out TValue value)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return Remove(key, out value);
    }

    /// <summary>Tries to get the value, associated with a specified key, from the dictionary.</summary>
    /// <param name="key">The key of the value to find in the dictionary.</param>
    /// <param name="value">On return, contains the value associated with <paramref name="key" /> if it was found in the dictionary; otherwise, <c>default</c>.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was found in the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public readonly bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        ref var entry = ref GetEntryReference(key);
        var existing = !Unsafe.IsNullRef(in entry);

        value = existing ? entry.Value : default;
        return existing;
    }

    /// <summary>Tries to add a value, associated with a specified key, to the dictionary.</summary>
    /// <param name="key">The key of the value to add to the dictionary.</param>
    /// <param name="value">The value to associate with <paramref name="key" />.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was succesfully added to the dictionary; otherwise, <c>false</c> if the dictionary already contained <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public bool TryAdd(TKey key, TValue value)
    {
        ref var entry = ref GetEntryReference(key, out bool existing);

        if (!existing)
        {
            entry.Value = value;
        }

        return !existing;
    }

    /// <summary>Tries to add a value, associated with a specified key, to the dictionary.</summary>
    /// <param name="key">The key of the value to add to the dictionary.</param>
    /// <param name="value">The value to associate with <paramref name="key" />.</param>
    /// <param name="mutex">The mutex to use when adding <paramref name="key" /> to the dictionary.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was succesfully added to the dictionary; otherwise, <c>false</c> if the dictionary already contained <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public bool TryAdd(TKey key, TValue value, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return TryAdd(key, value);
    }

    /// <summary>Trims any excess capacity, up to a given threshold, from the dictionary.</summary>
    /// <param name="threshold">A percentage, between <c>zero</c> and <c>one</c>, under which any excess will not be trimmed.</param>
    /// <remarks>This methods clamps <paramref name="threshold" /> to between <c>zero</c> and <c>one</c>, inclusive.</remarks>
    public void TrimExcess(float threshold = 1.0f)
    {
        var count = _count;
        var minCount = (int)(Capacity * Clamp(threshold, 0.0f, 1.0f));

        if (count < minCount)
        {
            Resize(HashUtilities.GetPrime(count));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private readonly ref int GetBucketReference(Span<int> buckets, int hashCode)
    {
        var i = Environment.Is64BitProcess
              ? HashUtilities.FastMod((uint)hashCode, (uint)buckets.Length, _fastModMultiplier)
              : (uint)hashCode % (uint)buckets.Length;

        return ref buckets.GetReferenceUnsafe(i);
    }

    private readonly ref Entry GetEntryReference(TKey key) => ref GetEntryReference(key, out int _);

    private readonly ref Entry GetEntryReference(TKey key, out int lastIndex)
    {
        if (key is null)
        {
            ThrowArgumentNullException(nameof(key));
        }

        ref var entry = ref Unsafe.NullRef<Entry>();
        var buckets = _buckets;

        lastIndex = -1;

        if (buckets is not null)
        {
            var entries = _entries;
            var comparer = _comparer;

            // Handle a null comparer for value type specially so that de-virtualization can occur

            if (typeof(TKey).IsValueType && (comparer is null))
            {
                var hashCode = key.GetHashCode();
                var collisionCount = 0;

                for (var i = GetBucketReference(buckets, hashCode) - 1; i < entries.Length; i = entry.Next)
                {
                    entry = ref entries.GetReferenceUnsafe(i);

                    if ((entry.HashCode == hashCode) && EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                    {
                        return ref entry;
                    }

                    collisionCount++;

                    if (collisionCount > entries.Length)
                    {
                        // The chain of entries forms a loop; which means a concurrent update has happened.
                        ThrowForDictionaryConcurrentReadOrWrite();
                    }

                    lastIndex = i;
                }
            }
            else
            {
                AssertNotNull(comparer);

                var hashCode = comparer.GetHashCode(key);
                var collisionCount = 0;

                for (var i = GetBucketReference(buckets, hashCode) - 1; i < entries.Length; i = entry.Next)
                {
                    entry = ref entries.GetReferenceUnsafe(i);

                    if ((entry.HashCode == hashCode) && comparer.Equals(entry.Key, key))
                    {
                        return ref entry;
                    }

                    collisionCount++;

                    if (collisionCount > entries.Length)
                    {
                        // The chain of entries forms a loop; which means a concurrent update has happened.
                        ThrowForDictionaryConcurrentReadOrWrite();
                    }

                    lastIndex = i;
                }
            }
        }

        return ref Unsafe.NullRef<Entry>();
    }

    private ref Entry GetEntryReference(TKey key, out bool existing)
    {
        ref var entry = ref GetEntryReference(key);

        if (!Unsafe.IsNullRef(in entry))
        {
            existing = true;
            return ref entry;
        }
        else if (_buckets is null)
        {
            Initialize(capacity: 0);
        }

        int hashCode;
        var comparer = _comparer;

        if (typeof(TKey).IsValueType && (comparer is null))
        {
            hashCode = key.GetHashCode();
        }
        else
        {
            AssertNotNull(comparer);
            hashCode = comparer.GetHashCode(key);
        }

        ref var bucket = ref GetBucketReference(_buckets, hashCode);

        var entries = _entries;
        int index;

        if (_freeCount > 0)
        {
            index = _freeList;
            entry = ref entries.GetReferenceUnsafe(_freeList);

            // Shouldn't overflow because `next` cannot underflow
            var freeList = StartOfFreeList - entry.Next;
            Assert(freeList >= -1);

            _freeCount--;
        }
        else
        {
            var count = _count;

            if (count == entries.Length)
            {
                Resize(HashUtilities.ExpandPrime(count));
                bucket = ref GetBucketReference(_buckets, hashCode);
            }
            index = count;

            _count = count + 1;

            entries = _entries;
        }

        entry = ref entries.GetReferenceUnsafe(index);

        entry.HashCode = hashCode;
        entry.Next = bucket - 1; // Value in _buckets is 1-based
        entry.Key = key;

        bucket = index + 1; // Value in _buckets is 1-based

        existing = false;
        return ref entry;
    }

    private void Initialize(int capacity)
    {
        var size = HashUtilities.GetPrime(capacity);

        var buckets = GC.AllocateUninitializedArray<int>(size);
        var entries = GC.AllocateUninitializedArray<Entry>(size);

        _freeList = -1;

        if (Environment.Is64BitProcess)
        {
            _fastModMultiplier = HashUtilities.GetFastModMultiplier((uint)size);
        }

        // Assign member variables after both arrays allocated to guard against corruption from OOM if second fails
        _buckets = buckets;
        _entries = entries;
    }

    private void Resize(int newCapacity)
    {
        var entries = _entries;
        Assert(newCapacity >= entries.Length);

        var newEntries = GC.AllocateUninitializedArray<Entry>(newCapacity);
        var newBuckets = GC.AllocateUninitializedArray<int>(newCapacity);

        if (Environment.Is64BitProcess)
        {
            _fastModMultiplier = HashUtilities.GetFastModMultiplier((uint)newCapacity);
        }

        var count = _count;
        entries.AsSpan(0, count).CopyTo(newEntries);

        for (var i = 0; i < count; i++)
        {
            ref var newEntry = ref newEntries.GetReferenceUnsafe(i);

            if (newEntry.Next >= -1)
            {
                ref var bucket = ref GetBucketReference(newBuckets, newEntry.HashCode);
                newEntry.Next = bucket - 1; // Value in _buckets is 1-based
                bucket = i + 1;
            }
        }

        _buckets = newBuckets;
        _entries = newEntries;
    }

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    readonly IEnumerator<(TKey Key, TValue Value)> IEnumerable<(TKey Key, TValue Value)>.GetEnumerator() => GetEnumerator();
}
