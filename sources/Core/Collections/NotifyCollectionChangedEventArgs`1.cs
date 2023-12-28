// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Collections;

/// <summary>Provides data for the <see cref="INotifyCollectionChanged{T}.CollectionChanged" /> event.</summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public sealed class NotifyCollectionChangedEventArgs<T> : EventArgs
{
    internal static readonly NotifyCollectionChangedEventArgs<T> s_reset = new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Reset);

    private readonly NotifyCollectionChangedAction _action;
    private readonly T? _value;

    internal NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, T? value = default)
    {
        AssertIsDefined(action);

        _action = action;
        _value = value;
    }

    /// <summary>Gets the action that caused the event.</summary>
    public NotifyCollectionChangedAction Action => _action;

    /// <summary>Gets the value of the item that caused the event.</summary>
    /// <remarks>The value may not be valid if <see cref="Action" /> is <see cref="NotifyCollectionChangedAction.Reset" />.</remarks>
    public T? Value
    {
        get
        {
            Assert(_action != NotifyCollectionChangedAction.Reset);
            return _value;
        }
    }
}
