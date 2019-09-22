// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;

namespace TerraFX.Collections
{
    /// <summary>Defines a means of listening for notifications that occur when a <see cref="IDictionary{TKey, TValue}" /> is changed.</summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public interface INotifyDictionaryChanged<TKey, TValue>
    {
        /// <summary>Occurs when the underlying <see cref="IDictionary{TKey, TValue}" /> changes.</summary>
        event EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>>? DictionaryChanged;
    }
}
