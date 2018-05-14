// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;

namespace TerraFX.Collections
{
    /// <summary>Defines a means of listening for notifications that occur when a <see cref="ICollection{T}" /> is changed.</summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public interface INotifyCollectionChanged<T>
    {
        #region Events
        /// <summary>Occurs when the underlying <see cref="ICollection{T}" /> changes.</summary>
        event EventHandler<NotifyCollectionChangedEventArgs<T>> CollectionChanged;
        #endregion
    }
}
