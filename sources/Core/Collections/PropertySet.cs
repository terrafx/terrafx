// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Collections
{
    /// <summary>Represents a <see cref="string" />-<see cref="object" /> dictionary that provides notifications when its contents are changed.</summary>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(DebugView))]
    [Serializable]
    public sealed partial class PropertySet : IPropertySet
    {
        private readonly IDictionary<string, object> _items;

        /// <summary>Initializes a new instance of the <see cref="PropertySet" /> class.</summary>
        /// <remarks>This constructor is equivalent to calling <see cref="PropertySet(IDictionary{string, object})" /> with <see cref="Dictionary{TKey, TValue}()" />.</remarks>
        public PropertySet()
        {
            _items = new Dictionary<string, object>();
        }

        /// <summary>Initializes a new instance of the <see cref="PropertySet" /> class.</summary>
        /// <param name="capacity">The initial size of the set.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is less than zero.</exception>
        public PropertySet(int capacity)
        {
            _items = new Dictionary<string, object>(capacity);
        }

        /// <summary>Initializes a new instance of the <see cref="PropertySet" /> class.</summary>
        /// <param name="items">The <see cref="IEnumerable{T}" /> whose elements are copied to the set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="items" /> is <c>null</c>.</exception>
        public PropertySet(IEnumerable<KeyValuePair<string, object>> items)
        {
            ThrowIfNull(items, nameof(items));
            _items = new Dictionary<string, object>(items);
        }

        /// <summary>Initializes a new instance of the <see cref="PropertySet" /> class.</summary>
        /// <param name="items">The <see cref="IDictionary{TKey, TValue}" /> whose elements are copied to the set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="items" /> is <c>null</c>.</exception>
        public PropertySet(IDictionary<string, object> items)
        {
            ThrowIfNull(items, nameof(items));
            _items = new Dictionary<string, object>(items);
        }

        /// <summary>Occurs when the underlying dictionary changes.</summary>
        public event EventHandler<NotifyDictionaryChangedEventArgs<string, object>>? DictionaryChanged;

        /// <summary>Gets the number of items contained by the instance.</summary>
        public int Count => _items.Count;

        /// <summary>Gets a value that indicates whether the instance is <c>read-only</c>.</summary>
        /// <remarks>An instance that is <c>read-only</c> does not allow the modification of the items contained by an instance.</remarks>
        public bool IsReadOnly => _items.IsReadOnly;

        /// <summary>Gets a <see cref="ICollection{TKey}" /> that contains the keys for the instance.</summary>
        public ICollection<string> Keys => _items.Keys;

        /// <summary>Gets a <see cref="ICollection{TKey}" /> that contains the values for the instance.</summary>
        public ICollection<object> Values => _items.Values;

        /// <summary>Gets or sets the item with the specified <paramref name="key" />.</summary>
        /// <param name="key">The key of the item to get or set.</param>
        /// <returns>The item with the specified <paramref name="key" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">On <c>get</c>: An item with the specified <paramref name="key" /> could not be found.</exception>
        /// <exception cref="NotSupportedException">On <c>set</c>: The instance is <c>read-only</c>.</exception>
        public object this[string key]
        {
            get
            {
                return _items[key];
            }

            set
            {
                if (_items.ContainsKey(key))
                {
                    var oldValue = _items[key];
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

        /// <summary>Removes all items from the instance.</summary>
        /// <exception cref="NotSupportedException">The instance is <c>read-only</c>.</exception>
        public void Clear()
        {
            _items.Clear();
            OnDictionaryReset();
        }

        /// <summary>Adds an item to the instance.</summary>
        /// <param name="key">The key of the item to add to the instance.</param>
        /// <param name="value">The value of the item to add to the instance.</param>
        /// <exception cref="ArgumentException">An item with the same <paramref name="key" /> already exists in the instance.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">The instance is <c>read-only</c>.</exception>
        public void Add(string key, object value)
        {
            _items.Add(key, value);
            OnDictionaryItemAdded(key);
        }

        /// <summary>Determines whether the instance contains a specific key.</summary>
        /// <param name="key">The key for which to check.</param>
        /// <returns><c>true</c> if the instance contains <paramref name="key" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
        public bool ContainsKey(string key) => _items.ContainsKey(key);

        /// <summary>Gets an <see cref="IEnumerator{T}" /> that can iterate through the items contained by the instance.</summary>
        /// <returns>An <see cref="IEnumerator{T}" /> that can iterate through the items contained by the instance.</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _items.GetEnumerator();

        /// <summary>Removes an item from the instance.</summary>
        /// <param name="key">The key of the item to remove from the instance.</param>
        /// <returns><c>true</c> if the item was succesfully removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">The instance is <c>read-only</c>.</exception>
        public bool Remove(string key)
        {
            var removed = _items.Remove(key);

            if (removed)
            {
                OnDictionaryItemRemoved(key);
            }

            return removed;
        }

        /// <summary>Attempts to get the value of an item from the instance.</summary>
        /// <param name="key">The key of the item to get from the instance.</param>
        /// <param name="value">On <c>return</c>: Contains the value of the item if it was found; otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if an item with the specified <paramref name="key" /> was found; otherwise, <c>false</c>.</returns>
        public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value) => _items.TryGetValue(key, out value!);

        private void OnDictionaryReset()
        {
            if (DictionaryChanged != null)
            {
                var eventArgs = NotifyDictionaryChangedEventArgs<string, object>.ForResetAction();
                DictionaryChanged(this, eventArgs);
            }
        }

        private void OnDictionaryItemAdded(string key)
        {
            if (DictionaryChanged != null)
            {
                var eventArgs = NotifyDictionaryChangedEventArgs<string, object>.ForAddAction(key);
                DictionaryChanged(this, eventArgs);
            }
        }

        private void OnDictionaryItemChanged(string key, object oldValue, object newValue)
        {
            if (DictionaryChanged != null)
            {
                var eventArgs = NotifyDictionaryChangedEventArgs<string, object>.ForValueChangedAction(key, oldValue, newValue);
                DictionaryChanged(this, eventArgs);
            }
        }

        private void OnDictionaryItemRemoved(string key)
        {
            if (DictionaryChanged != null)
            {
                var eventArgs = NotifyDictionaryChangedEventArgs<string, object>.ForRemoveAction(key);
                DictionaryChanged(this, eventArgs);
            }
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            _items.Add(item);
            OnDictionaryItemAdded(item.Key);
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item) => _items.Contains(item);

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            var removed = _items.Remove(item);

            if (removed)
            {
                OnDictionaryItemRemoved(item.Key);
            }

            return removed;
        }
    }
}
