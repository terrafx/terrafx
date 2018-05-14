// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;

namespace TerraFX.Collections
{
    /// <summary>Defines a generic collection that provides notifications when its contents are changed.</summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public interface IObservableCollection<T> : ICollection<T>, INotifyCollectionChanged<T>
    {
    }
}
