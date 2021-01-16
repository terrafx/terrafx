// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Collections
{
    /// <summary>Represents a string to object dictionary that provides notifications when its contents are changed.</summary>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(DictionaryDebugView<string, object>))]
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

        /// <inheritdoc />
        public event EventHandler<NotifyDictionaryChangedEventArgs<string, object>>? DictionaryChanged;

        /// <inheritdoc />
        public int Count => _items.Count;

        /// <inheritdoc />
        public bool IsReadOnly => _items.IsReadOnly;

        /// <inheritdoc />
        public ICollection<string> Keys => _items.Keys;

        /// <inheritdoc />
        public ICollection<object> Values => _items.Values;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Clear()
        {
            _items.Clear();
            OnDictionaryReset();
        }

        /// <inheritdoc />
        public void Add(string key, object value)
        {
            _items.Add(key, value);
            OnDictionaryItemAdded(key);
        }

        /// <inheritdoc />
        public bool ContainsKey(string key) => _items.ContainsKey(key);

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _items.GetEnumerator();

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
