// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Collections;

/// <summary>Defines the action that caused a <see cref="INotifyCollectionChanged{T}.CollectionChanged" /> event.</summary>
public enum NotifyCollectionChangedAction
{
    /// <summary>An item was added to the collection.</summary>
    Add,

    /// <summary>An item was removed from the collection.</summary>
    Remove,

    /// <summary>The collection had its contents reset.</summary>
    Reset
}
