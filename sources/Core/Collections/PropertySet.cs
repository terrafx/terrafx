// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Collections;

/// <summary>Represents a string to object dictionary that provides notifications when its contents are changed.</summary>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(DebugView))]
public sealed partial class PropertySet : IPropertySet
{
    /// <summary>Gets an empty property set.</summary>
    public static PropertySet Empty { get; } = [];

    internal ValueDictionary<string, object> _items;

    /// <summary>Initializes a new instance of the <see cref="PropertySet" /> class.</summary>
    /// <remarks>This constructor is equivalent to calling <see cref="PropertySet(IDictionary{string, object})" /> with <see cref="Dictionary{TKey, TValue}()" />.</remarks>
    public PropertySet()
    {
        _items = [];
    }

    /// <summary>Initializes a new instance of the <see cref="PropertySet" /> class.</summary>
    /// <param name="capacity">The initial size of the set.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is less than zero.</exception>
    public PropertySet(int capacity)
    {
        _items = new ValueDictionary<string, object>(capacity);
    }

    /// <summary>Initializes a new instance of the <see cref="PropertySet" /> class.</summary>
    /// <param name="items">The <see cref="IEnumerable{T}" /> whose elements are copied to the set.</param>
    /// <exception cref="ArgumentNullException"><paramref name="items" /> is <c>null</c>.</exception>
    public PropertySet(IEnumerable<KeyValuePair<string, object>> items)
    {
        ThrowIfNull(items);
        _items = [.. items];
    }

    /// <summary>Initializes a new instance of the <see cref="PropertySet" /> class.</summary>
    /// <param name="items">The <see cref="IDictionary{TKey, TValue}" /> whose elements are copied to the set.</param>
    /// <exception cref="ArgumentNullException"><paramref name="items" /> is <c>null</c>.</exception>
    public PropertySet(IDictionary<string, object> items)
    {
        ThrowIfNull(items);
        _items = [.. items];
    }

    /// <inheritdoc />
    public event EventHandler<NotifyDictionaryChangedEventArgs<string, object>>? DictionaryChanged;

    /// <inheritdoc />
    public int Count => _items.Count;

    /// <summary><c>true</c> if the property set is <c>empty</c>; otherwise, <c>false</c>.</summary>
    public bool IsEmpty => _items.Count == 0;

    /// <summary>Gets a collection containing the keys for the property set.</summary>
    public KeyCollection Keys => new KeyCollection(this);

    /// <summary>Gets a collection containing the values for the property set.</summary>
    public ValueCollection Values => new ValueCollection(this);

    /// <inheritdoc />
    public object this[string key]
    {
        get
        {
            return _items[key];
        }

        set
        {
            if (_items.TryGetValue(key, out var oldValue))
            {
                _items[key] = value;
                OnDictionaryItemChanged(key, oldValue, value);
            }
            else
            {
                _items[key] = value;
                OnDictionaryItemAdded(key);
            }
        }
    }

    /// <inheritdoc />
    public void Add(KeyValuePair<string, object> item) => Add(item.Key, item.Value);

    /// <inheritdoc />
    public void Add(string key, object value)
    {
        _items.Add(key, value);
        OnDictionaryItemAdded(key);
    }

    /// <summary>Adds a value, associated with the specified key, to the property set.</summary>
    /// <param name="keyValuePair">The key/value pair to add to the property set.</param>
    /// <param name="mutex">The mutex to use when adding <paramref name="keyValuePair" /> to the list.</param>
    /// <exception cref="ArgumentException">A value associated with the key of <paramref name="keyValuePair" /> already exists in the property set.</exception>
    /// <exception cref="ArgumentNullException">The key of <paramref name="keyValuePair" /> is <c>null</c>.</exception>
    public void Add(KeyValuePair<string, object> keyValuePair, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        Add(keyValuePair);
    }

    /// <summary>Adds a value, associated with the specified key, to the property set.</summary>
    /// <param name="key">The key of the value to add to the property set.</param>
    /// <param name="value">The value to associate with <paramref name="key" />.</param>
    /// <param name="mutex">The mutex to use when adding <paramref name="key" /> to the property set.</param>
    /// <exception cref="ArgumentException">A value associated with <paramref name="key" /> already exists in the dictionary.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public void Add(string key, object value, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        Add(key, value);
    }

    /// <inheritdoc />
    public void Clear()
    {
        _items.Clear();
        OnDictionaryReset();
    }

    /// <inheritdoc />
    public bool Contains(KeyValuePair<string, object> item) => _items.Contains(item);

    /// <inheritdoc />
    public bool ContainsKey(string key) => _items.ContainsKey(key);

    /// <summary>Checks whether the property set contains a specified value.</summary>
    /// <param name="value">The value to check for in the property set.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was found in the property set; otherwise, <c>false</c>.</returns>
    public bool ContainsValue(object value) => _items.ContainsValue(value);

    /// <summary>Copies the items of the dictionary to a span.</summary>
    /// <param name="destination">The span to which the items will be copied.</param>
    /// <exception cref="ArgumentException"><see cref="Count" /> is greater than the length of <paramref name="destination" />.</exception>
    public void CopyTo(Span<KeyValuePair<string, object>> destination) => _items.CopyTo(destination);

    /// <summary>Ensures the capacity of the property set is at least the specified value.</summary>
    /// <param name="capacity">The minimum capacity the property set should support.</param>
    public void EnsureCapacity(int capacity) => _items.EnsureCapacity(capacity);

    /// <inheritdoc />
    public ItemsEnumerator GetEnumerator() => new ItemsEnumerator(this);

    /// <inheritdoc />
    public bool Remove(string key)
    {
        var removed = _items.Remove(key);

        if (removed)
        {
            OnDictionaryItemRemoved(key);
        }

        return removed;
    }

    /// <inheritdoc />
    public bool Remove(KeyValuePair<string, object> item)
    {
        var removed = _items.Remove(item);

        if (removed)
        {
            OnDictionaryItemRemoved(item.Key);
        }

        return removed;
    }

    /// <summary>Removes the value, associated with a specified key, from the property set.</summary>
    /// <param name="key">The key of the value to remove from the property set.</param>
    /// <param name="value">On return, contains the value associated with <paramref name="key" /> if it was found in the property set; otherwise, <c>default</c>.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was removed from the property set; otherwise, <c>false</c> if the property set did not contain <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public bool Remove(string key, [MaybeNullWhen(false)] out object value)
    {
        var removed = _items.Remove(key, out value);

        if (removed)
        {
            OnDictionaryItemRemoved(key);
        }

        return removed;
    }

    /// <summary>Removes the value, associated with a specified key, from the property set.</summary>
    /// <param name="key">The key of the value to remove from the property set.</param>
    /// <param name="mutex">The mutex to use when removing <paramref name="key" /> from the property set.</param>
    /// <returns><c>true</c> if a value associated with <paramref name="key" /> was removed from the property set; otherwise, <c>false</c> if the property set did not contain <paramref name="key" />.</returns>
    public bool Remove(string key, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return Remove(key);
    }

    /// <summary>Removes the key/value pair from the property set.</summary>
    /// <param name="keyValuePair">The key/value pair to remove from the property set.</param>
    /// <param name="mutex">The mutex to use when removing <paramref name="keyValuePair" /> from the property set.</param>
    /// <returns><c>true</c> if <paramref name="keyValuePair" /> was removed from the property set; otherwise, <c>false</c> if the property set did not contain <paramref name="keyValuePair" />.</returns>
    public bool Remove(KeyValuePair<string, object> keyValuePair, ValueMutex mutex)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return Remove(keyValuePair);
    }

    /// <summary>Removes the value, associated with a specified key, from the property set.</summary>
    /// <param name="key">The key of the value to remove from the property set.</param>
    /// <param name="mutex">The mutex to use when removing <paramref name="key" /> from the property set.</param>
    /// <param name="value">On return, contains the value associated with <paramref name="key" /> if it was found in the property set; otherwise, <c>default</c>.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was removed from the property set; otherwise, <c>false</c> if the property set did not contain <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public bool Remove(string key, ValueMutex mutex, [MaybeNullWhen(false)] out object value)
    {
        using var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false);
        return Remove(key, out value);
    }

    /// <inheritdoc />
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value) => _items.TryGetValue(key, out value);

    /// <summary>Tries to add a value, associated with a specified key, to the property set.</summary>
    /// <param name="key">The key of the value to add to the property set.</param>
    /// <param name="value">The value to associate with <paramref name="key" />.</param>
    /// <returns><c>true</c> if <paramref name="value" /> was successfully added to the property set; otherwise, <c>false</c> if the property set already contained <paramref name="key" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
    public bool TryAdd(string key, object value)
    {
        var added = _items.TryAdd(key, value);

        if (added)
        {
            OnDictionaryItemAdded(key);
        }

        return added;
    }

    /// <summary>Trims any excess capacity, up to a given threshold, from the property set.</summary>
    /// <param name="threshold">A percentage, between <c>zero</c> and <c>one</c>, under which any excess will not be trimmed.</param>
    /// <remarks>This methods clamps <paramref name="threshold" /> to between <c>zero</c> and <c>one</c>, inclusive.</remarks>
    public void TrimExcess(float threshold = 1.0f) => _items.TrimExcess(threshold);

    private void OnDictionaryReset()
    {
        if (DictionaryChanged != null)
        {
            var eventArgs = NotifyDictionaryChangedEventArgs.ForResetAction<string, object>();
            DictionaryChanged(this, eventArgs);
        }
    }

    private void OnDictionaryItemAdded(string key)
    {
        if (DictionaryChanged != null)
        {
            var eventArgs = NotifyDictionaryChangedEventArgs.ForAddAction<string, object>(key);
            DictionaryChanged(this, eventArgs);
        }
    }

    private void OnDictionaryItemChanged(string key, object oldValue, object newValue)
    {
        if (DictionaryChanged != null)
        {
            var eventArgs = NotifyDictionaryChangedEventArgs.ForValueChangedAction(key, oldValue, newValue);
            DictionaryChanged(this, eventArgs);
        }
    }

    private void OnDictionaryItemRemoved(string key)
    {
        if (DictionaryChanged != null)
        {
            var eventArgs = NotifyDictionaryChangedEventArgs.ForRemoveAction<string, object>(key);
            DictionaryChanged(this, eventArgs);
        }
    }

    bool ICollection<KeyValuePair<string, object>>.IsReadOnly => false;

    ICollection<string> IDictionary<string, object>.Keys => new KeyCollection(this);

    ICollection<object> IDictionary<string, object>.Values => new ValueCollection(this);

    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => _items.CopyTo(array.AsSpan(arrayIndex));

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator() => GetEnumerator();
}
