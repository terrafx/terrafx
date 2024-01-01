// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the UnmanagedValueDictionary<TKey, TValue> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="UnmanagedValueDictionary{TKey, TValue}" /> struct.</summary>
public static class UnmanagedValueDictionary
{
    internal const int StartOfFreeList = -3;

    /// <summary>Gets an empty dictionary.</summary>
    public static UnmanagedValueDictionary<TKey, TValue> Empty<TKey, TValue>()
        where TKey : unmanaged
        where TValue : unmanaged => new UnmanagedValueDictionary<TKey, TValue>();

    /// <summary>Adds a value, associated with the specified key, to the dictionary.</summary>
    /// <param name="dictionary">The dictionary to which the value should be added.</param>
    /// <param name="keyValuePair">The key/value pair to add to the dictionary.</param>
    /// <exception cref="ArgumentException">A value associated with the key of <paramref name="keyValuePair" /> already exists in the dictionary.</exception>
    /// <exception cref="ArgumentNullException">The key of <paramref name="keyValuePair" /> is <c>null</c>.</exception>
    public static void Add<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValuePair)
        where TKey : unmanaged
        where TValue : unmanaged => dictionary.Add(keyValuePair.Key, keyValuePair.Value);

    /// <summary>Adds a value, associated with the specified key, to the dictionary.</summary>
    /// <param name="dictionary">The dictionary to which the value should be added.</param>
    /// <param name="keyValuePair">The key/value pair to add to the dictionary.</param>
    /// <param name="mutex">The mutex to use when adding <paramref name="keyValuePair" /> to the dictionary.</param>
    /// <exception cref="ArgumentException">A value associated with the key of <paramref name="keyValuePair" /> already exists in the dictionary.</exception>
    /// <exception cref="ArgumentNullException">The key of <paramref name="keyValuePair" /> is <c>null</c>.</exception>
    public static void Add<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValuePair, ValueMutex mutex)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
    }

    /// <summary>Adds a value, associated with the specified key, to the dictionary.</summary>
    /// <param name="dictionary">The dictionary to which the value should be added.</param>
    /// <param name="key">The key of the value to add to the dictionary.</param>
    /// <param name="value">The value to associate with <paramref name="key" />.</param>
    /// <exception cref="ArgumentException">A value associated with <paramref name="key" /> already exists in the dictionary.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public static void Add<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        ref var entry = ref dictionary.GetEntryReference(key, out bool existing);

        if (existing)
        {
            ThrowForDictionaryExistingKey(key);
        }

        entry.Value = value;
    }

    /// <summary>Adds a value, associated with the specified key, to the dictionary.</summary>
    /// <param name="dictionary">The dictionary to which the value should be added.</param>
    /// <param name="key">The key of the value to add to the dictionary.</param>
    /// <param name="value">The value to associate with <paramref name="key" />.</param>
    /// <param name="mutex">The mutex to use when adding <paramref name="key" /> to the dictionary.</param>
    /// <exception cref="ArgumentException">A value associated with <paramref name="key" /> already exists in the dictionary.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public static void Add<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key, TValue value, ValueMutex mutex)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        dictionary.Add(key, value);
    }

    /// <summary>Removes all items from the dictionary.</summary>
    /// <param name="dictionary">The dictionary which should be cleaered.</param>
    public static void Clear<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        var count = dictionary._count;

        if (count > 0)
        {
            dictionary._buckets.Clear();
            dictionary._entries.AsSpan(0, (uint)count).Clear();

            dictionary._count = 0;
            dictionary._freeCount = 0;
            dictionary._freeList = -1;
        }
    }

    /// <summary>Checks whether the dictionary contains a specified key/value pair.</summary>
    /// <param name="dictionary">The dictionary for which to check for the key.</param>
    /// <param name="keyValuePair">The key/value pair to check for in the dictionary.</param>
    /// <returns><c>true</c> if <paramref name="keyValuePair" /> was found in the dictionary; otherwise, <c>false</c>.</returns>
    public static bool Contains<TKey, TValue>(this ref readonly UnmanagedValueDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValuePair)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        ref var entry = ref dictionary.GetEntryReference(keyValuePair.Key);
        return !Unsafe.IsNullRef(in entry) && EqualityComparer<TValue>.Default.Equals(entry.Value, keyValuePair.Value);
    }

    /// <summary>Checks whether the dictionary contains a specified key.</summary>
    /// <param name="dictionary">The dictionary for which to check for the key.</param>
    /// <param name="key">The key to check for in the dictionary.</param>
    /// <returns><c>true</c> if <paramref name="key" /> was found in the dictionary; otherwise, <c>false</c>.</returns>
    public static bool ContainsKey<TKey, TValue>(this ref readonly UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        ref var entry = ref dictionary.GetEntryReference(key);
        return !Unsafe.IsNullRef(in entry);
    }

    /// <summary>Checks whether the dictionary contains a specified value.</summary>
    /// <param name="dictionary">The dictionary for which to check for the value.</param>
    /// <param name="value">The value to check for in the dictionary.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was found in the dictionary; otherwise, <c>false</c>.</returns>
    public static bool ContainsValue<TKey, TValue>(this ref readonly UnmanagedValueDictionary<TKey, TValue> dictionary, TValue value)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        var entries = dictionary._entries;
        var count = dictionary._count;

        // ValueType: De-virtualize with EqualityComparer<TValue>.Default intrinsic
        for (var i = 0; i < count; i++)
        {
            ref var entry = ref entries.GetReferenceUnsafe((uint)i);

            if ((entry.Next >= -1) && EqualityComparer<TValue>.Default.Equals(entry.Value, value))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Copies the items of the dictionary to a span.</summary>
    /// <param name="dictionary">The dictionary which should be copied.</param>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentException"><see cref="UnmanagedValueDictionary{TKey, TValue}.Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public static void CopyTo<TKey, TValue>(this ref readonly UnmanagedValueDictionary<TKey, TValue> dictionary, UnmanagedSpan<KeyValuePair<TKey, TValue>> destination)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        var count = dictionary._count;
        ThrowIfNotInInsertBounds((uint)count, destination.Length);

        var entries = dictionary._entries;

        for (var i = 0; i < count; i++)
        {
            ref var entry = ref entries.GetReferenceUnsafe((uint)i);

            if (entry.Next >= -1)
            {
                destination.GetReferenceUnsafe((uint)i) = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
            }
        }
    }

    /// <summary>Ensures the capacity of the dictionary is at least the specified value.</summary>
    /// <param name="dictionary">The dictionary whose capacity should be ensured.</param>
    /// <param name="capacity">The minimum capacity the dictionary should support.</param>
    /// <exception cref="OverflowException"><paramref name="capacity" /> is greater than <see cref="int.MaxValue" />.</exception>
    public static void EnsureCapacity<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, nuint capacity)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        var currentCapacity = dictionary.Capacity;

        if (capacity > currentCapacity)
        {
            var newCapacity = checked((int)capacity);

            if (dictionary._buckets.IsNull)
            {
                dictionary.Initialize(newCapacity);
            }
            dictionary.Resize(HashUtilities.GetPrime(newCapacity));
        }
    }

    /// <summary>Gets a reference to the value, associated with a specified key, in the dictionary.</summary>
    /// <param name="dictionary">The dictionary for which to get a value reference.</param>
    /// <param name="key">The key of the value to get a reference to.</param>
    /// <returns>A reference to the value, associated with <paramref name="key" />; otherwise, <see cref="Unsafe.NullRef{T}" /> if the dictionary did not contain <paramref name="key" />.</returns>
    /// <remarks>This method is unsafe because other operations may invalidate the backing data.</remarks>
    public static ref TValue GetValueReferenceUnsafe<TKey, TValue>(this scoped ref readonly UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        ref var entry = ref dictionary.GetEntryReference(key);
        return ref !Unsafe.IsNullRef(in entry) ? ref entry.Value : ref Unsafe.NullRef<TValue>();
    }

    /// <summary>Gets a reference to the value, associated with a specified key, in the dictionary.</summary>
    /// <param name="dictionary">The dictionary for which to get a value reference.</param>
    /// <param name="key">The key of the value to get a reference to.</param>
    /// <param name="existing">On return, contains <c>true</c> if the value was found in the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="key" />.</param>
    /// <returns>A reference to the value, associated with <paramref name="key" />; otherwise, a reference to the newly added value if the dictionary did not contain <paramref name="key" />.</returns>
    /// <remarks>This method is unsafe because other operations may invalidate the backing data.</remarks>
    public static ref TValue GetValueReferenceUnsafe<TKey, TValue>(this scoped ref readonly UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key, out bool existing)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        ref var entry = ref dictionary.GetEntryReference(key);
        existing = !Unsafe.IsNullRef(in entry);
        return ref existing ? ref entry.Value : ref Unsafe.NullRef<TValue>();
    }

    /// <summary>Removes the key/value pair from the dictionary.</summary>
    /// <param name="dictionary">The dictionary for which the key/value pair should be removed.</param>
    /// <param name="keyValuePair">The key/value pair to remove from the dictionary.</param>
    /// <returns><c>true</c> if <paramref name="keyValuePair" /> was removed from the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="keyValuePair" />.</returns>
    public static bool Remove<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValuePair)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        ref var entry = ref dictionary.GetEntryReference(keyValuePair.Key, out int lastIndex);
        var existing = !Unsafe.IsNullRef(in entry) && EqualityComparer<TValue>.Default.Equals(entry.Value, keyValuePair.Value);

        if (existing)
        {
            int index;

            if (lastIndex < 0)
            {
                int hashCode;
                var comparer = dictionary._comparer;

                if (typeof(TKey).IsValueType && (comparer is null))
                {
                    hashCode = keyValuePair.Key.GetHashCode();
                }
                else
                {
                    AssertNotNull(comparer);
                    hashCode = comparer.GetHashCode(keyValuePair.Key);
                }

                ref var bucket = ref dictionary.GetBucketReference(dictionary._buckets, hashCode);

                index = bucket - 1;
                bucket = entry.Next + 1; // Value in buckets is 1-based
            }
            else
            {
                ref var lastEntry = ref dictionary._entries.GetReferenceUnsafe((uint)lastIndex);
                index = lastEntry.Next;
                lastEntry.Next = entry.Next;
            }

            // Shouldn't underflow because max hash table length is MaxPrimeArrayLength = 0x7FEFFFFD(2146435069), while _freeList underflow threshold is 2147483646
            var next = StartOfFreeList - dictionary._freeList;
            Assert(next < 0);
            entry.Next = next;

            dictionary._freeList = index;
            dictionary._freeCount++;
        }

        return existing;
    }

    /// <summary>Removes the value, associated with a specified key, from the dictionary.</summary>
    /// <param name="dictionary">The dictionary for which the key should be removed.</param>
    /// <param name="key">The key of the value to remove from the dictionary.</param>
    /// <returns><c>true</c> if a value associated with <paramref name="key" /> was removed from the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="key" />.</returns>
    public static bool Remove<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : unmanaged
        where TValue : unmanaged => dictionary.Remove(key, out _);

    /// <summary>Removes the key/value pair from the dictionary.</summary>
    /// <param name="dictionary">The dictionary for which the key/value pair should be removed.</param>
    /// <param name="keyValuePair">The key/value pair to remove from the dictionary.</param>
    /// <param name="mutex">The mutex to use when removing <paramref name="keyValuePair" /> from the dictionary.</param>
    /// <returns><c>true</c> if <paramref name="keyValuePair" /> was removed from the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="keyValuePair" />.</returns>
    public static bool Remove<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValuePair, ValueMutex mutex)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return dictionary.Remove(keyValuePair);
    }

    /// <summary>Removes the value, associated with a specified key, from the dictionary.</summary>
    /// <param name="dictionary">The dictionary for which the key should be removed.</param>
    /// <param name="key">The key of the value to remove from the dictionary.</param>
    /// <param name="mutex">The mutex to use when removing <paramref name="key" /> from the dictionary.</param>
    /// <returns><c>true</c> if a value associated with <paramref name="key" /> was removed from the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="key" />.</returns>
    public static bool Remove<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key, ValueMutex mutex)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return dictionary.Remove(key);
    }

    /// <summary>Removes the value, associated with a specified key, from the dictionary.</summary>
    /// <param name="dictionary">The dictionary for which the key should be removed.</param>
    /// <param name="key">The key of the value to remove from the dictionary.</param>
    /// <param name="value">On return, contains the value associated with <paramref name="key" /> if it was found in the dictionary; otherwise, <c>default</c>.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was removed from the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public static bool Remove<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key, [MaybeNullWhen(false)] out TValue value)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        ref var entry = ref dictionary.GetEntryReference(key, out int lastIndex);
        var existing = !Unsafe.IsNullRef(in entry);

        if (existing)
        {
            int index;

            if (lastIndex < 0)
            {
                int hashCode;
                var comparer = dictionary._comparer;

                if (typeof(TKey).IsValueType && (comparer is null))
                {
                    hashCode = key.GetHashCode();
                }
                else
                {
                    AssertNotNull(comparer);
                    hashCode = comparer.GetHashCode(key);
                }

                ref var bucket = ref dictionary.GetBucketReference(dictionary._buckets, hashCode);

                index = bucket - 1;
                bucket = entry.Next + 1; // Value in buckets is 1-based
            }
            else
            {
                ref var lastEntry = ref dictionary._entries.GetReferenceUnsafe((uint)lastIndex);
                index = lastEntry.Next;
                lastEntry.Next = entry.Next;
            }

            value = entry.Value;

            // Shouldn't underflow because max hash table length is MaxPrimeArrayLength = 0x7FEFFFFD(2146435069), while _freeList underflow threshold is 2147483646
            var next = StartOfFreeList - dictionary._freeList;
            Assert(next < 0);
            entry.Next = next;

            dictionary._freeList = index;
            dictionary._freeCount++;
        }
        else
        {
            value = default;
        }

        return existing;
    }

    /// <summary>Removes the value, associated with a specified key, from the dictionary.</summary>
    /// <param name="dictionary">The dictionary for which the key should be removed.</param>
    /// <param name="key">The key of the value to remove from the dictionary.</param>
    /// <param name="mutex">The mutex to use when removing <paramref name="key" /> from the dictionary.</param>
    /// <param name="value">On return, contains the value associated with <paramref name="key" /> if it was found in the dictionary; otherwise, <c>default</c>.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was removed from the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public static bool Remove<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key, ValueMutex mutex, [MaybeNullWhen(false)] out TValue value)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return dictionary.Remove(key, out value);
    }

    /// <summary>Tries to get the value, associated with a specified key, from the dictionary.</summary>
    /// <param name="dictionary">The dictionary for which the value should be retrieved.</param>
    /// <param name="key">The key of the value to find in the dictionary.</param>
    /// <param name="value">On return, contains the value associated with <paramref name="key" /> if it was found in the dictionary; otherwise, <c>default</c>.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was found in the dictionary; otherwise, <c>false</c> if the dictionary did not contain <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public static bool TryGetValue<TKey, TValue>(this ref readonly UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key, [MaybeNullWhen(false)] out TValue value)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        ref var entry = ref dictionary.GetEntryReference(key);
        var existing = !Unsafe.IsNullRef(in entry);

        value = existing ? entry.Value : default;
        return existing;
    }

    /// <summary>Tries to add a value, associated with a specified key, to the dictionary.</summary>
    /// <param name="dictionary">The dictionary for which the value should be added.</param>
    /// <param name="key">The key of the value to add to the dictionary.</param>
    /// <param name="value">The value to associate with <paramref name="key" />.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was successfully added to the dictionary; otherwise, <c>false</c> if the dictionary already contained <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public static bool TryAdd<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        ref var entry = ref dictionary.GetEntryReference(key, out bool existing);

        if (!existing)
        {
            entry.Value = value;
        }

        return !existing;
    }

    /// <summary>Tries to add a value, associated with a specified key, to the dictionary.</summary>
    /// <param name="dictionary">The dictionary for which the value should be added.</param>
    /// <param name="key">The key of the value to add to the dictionary.</param>
    /// <param name="value">The value to associate with <paramref name="key" />.</param>
    /// <param name="mutex">The mutex to use when adding <paramref name="key" /> to the dictionary.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was successfully added to the dictionary; otherwise, <c>false</c> if the dictionary already contained <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public static bool TryAdd<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key, TValue value, ValueMutex mutex)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return dictionary.TryAdd(key, value);
    }

    /// <summary>Trims any excess capacity, up to a given threshold, from the dictionary.</summary>
    /// <param name="dictionary">The dictionary which should be trimmed.</param>
    /// <param name="threshold">A percentage, between <c>zero</c> and <c>one</c>, under which any excess will not be trimmed.</param>
    /// <remarks>This methods clamps <paramref name="threshold" /> to between <c>zero</c> and <c>one</c>, inclusive.</remarks>
    public static void TrimExcess<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, float threshold = 1.0f)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        var count = dictionary._count;
        var minCount = (int)(dictionary.Capacity * Clamp(threshold, 0.0f, 1.0f));

        if (count < minCount)
        {
            dictionary.Resize(HashUtilities.GetPrime(count));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref int GetBucketReference<TKey, TValue>(this scoped ref readonly UnmanagedValueDictionary<TKey, TValue> dictionary, UnmanagedSpan<int> buckets, int hashCode)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        var i = Environment.Is64BitProcess
              ? HashUtilities.FastMod((uint)hashCode, (uint)buckets.Length, dictionary._fastModMultiplier)
              : (uint)hashCode % (uint)buckets.Length;

        return ref buckets.GetReferenceUnsafe(i);
    }

    internal static ref UnmanagedValueDictionary<TKey, TValue>.Entry GetEntryReference<TKey, TValue>(this scoped ref readonly UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : unmanaged
        where TValue : unmanaged => ref dictionary.GetEntryReference(key, out int _);

    internal static ref UnmanagedValueDictionary<TKey, TValue>.Entry GetEntryReference<TKey, TValue>(this scoped ref readonly UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key, out int lastIndex)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        ref var entry = ref Unsafe.NullRef<UnmanagedValueDictionary<TKey, TValue>.Entry>();
        var buckets = dictionary._buckets;

        lastIndex = -1;

        if (!buckets.IsNull)
        {
            var entries = dictionary._entries;
            var comparer = dictionary._comparer;

            // Handle a null comparer for value type specially so that de-virtualization can occur

            if (typeof(TKey).IsValueType && (comparer is null))
            {
                var hashCode = key.GetHashCode();
                var collisionCount = 0u;

                for (var i = dictionary.GetBucketReference(buckets, hashCode) - 1; (uint)i < entries.Length; i = entry.Next)
                {
                    entry = ref entries.GetReferenceUnsafe((uint)i);

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
                var collisionCount = 0u;

                for (var i = dictionary.GetBucketReference(buckets, hashCode) - 1; (uint)i < entries.Length; i = entry.Next)
                {
                    entry = ref entries.GetReferenceUnsafe((uint)i);

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

        return ref Unsafe.NullRef<UnmanagedValueDictionary<TKey, TValue>.Entry>();
    }

    internal static ref UnmanagedValueDictionary<TKey, TValue>.Entry GetEntryReference<TKey, TValue>(this scoped ref UnmanagedValueDictionary<TKey, TValue> dictionary, TKey key, out bool existing)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        ref var entry = ref dictionary.GetEntryReference(key);

        if (!Unsafe.IsNullRef(in entry))
        {
            existing = true;
            return ref entry;
        }
        else if (dictionary._buckets.IsNull)
        {
            dictionary.Initialize(capacity: 0);
        }

        int hashCode;
        var comparer = dictionary._comparer;

        if (typeof(TKey).IsValueType && (comparer is null))
        {
            hashCode = key.GetHashCode();
        }
        else
        {
            AssertNotNull(comparer);
            hashCode = comparer.GetHashCode(key);
        }

        ref var bucket = ref dictionary.GetBucketReference(dictionary._buckets, hashCode);

        var entries = dictionary._entries;
        int index;

        if (dictionary._freeCount > 0)
        {
            index = dictionary._freeList;
            entry = ref entries.GetReferenceUnsafe((uint)index);

            // Shouldn't overflow because `next` cannot underflow
            var freeList = StartOfFreeList - entry.Next;
            Assert(freeList >= -1);

            dictionary._freeCount--;
        }
        else
        {
            var count = dictionary._count;

            if ((uint)count == entries.Length)
            {
                dictionary.Resize(HashUtilities.ExpandPrime(count));
                bucket = ref dictionary.GetBucketReference(dictionary._buckets, hashCode);
            }
            index = count;

            dictionary._count = count + 1;

            entries = dictionary._entries;
        }

        entry = ref entries.GetReferenceUnsafe((uint)index);

        entry.HashCode = hashCode;
        entry.Next = bucket - 1; // Value in _buckets is 1-based
        entry.Key = key;

        bucket = index + 1; // Value in _buckets is 1-based

        existing = false;
        return ref entry;
    }

    internal static void Initialize<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, int capacity)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        var size = HashUtilities.GetPrime(capacity);

        var buckets = new UnmanagedArray<int>((uint)size);
        var entries = new UnmanagedArray<UnmanagedValueDictionary<TKey, TValue>.Entry>((uint)size);

        dictionary._freeList = -1;

        if (Environment.Is64BitProcess)
        {
            dictionary._fastModMultiplier = HashUtilities.GetFastModMultiplier((uint)size);
        }

        // Assign member variables after both arrays allocated to guard against corruption from OOM if second fails
        dictionary._buckets = buckets;
        dictionary._entries = entries;
    }

    internal static void Resize<TKey, TValue>(this ref UnmanagedValueDictionary<TKey, TValue> dictionary, int newCapacity)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        var entries = dictionary._entries;
        Assert((uint)newCapacity >= entries.Length);

        var newEntries = new UnmanagedArray<UnmanagedValueDictionary<TKey, TValue>.Entry>((uint)newCapacity);
        var newBuckets = new UnmanagedArray<int>((uint)newCapacity);

        if (Environment.Is64BitProcess)
        {
            dictionary._fastModMultiplier = HashUtilities.GetFastModMultiplier((uint)newCapacity);
        }

        var count = dictionary._count;
        entries.AsUnmanagedSpan(0, (uint)count).CopyTo(newEntries);

        for (var i = 0; i < count; i++)
        {
            ref var newEntry = ref newEntries.GetReferenceUnsafe((uint)i);

            if (newEntry.Next >= -1)
            {
                ref var bucket = ref dictionary.GetBucketReference(newBuckets, newEntry.HashCode);
                newEntry.Next = bucket - 1; // Value in _buckets is 1-based
                bucket = i + 1;
            }
        }

        dictionary._buckets = newBuckets;
        dictionary._entries = newEntries;
    }
}
