// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Collections;

/// <summary>Provides data for the <see cref="INotifyDictionaryChanged{TKey, TValue}.DictionaryChanged" /> event.</summary>
/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
public sealed class NotifyDictionaryChangedEventArgs<TKey, TValue> : EventArgs
{
    internal static readonly NotifyDictionaryChangedEventArgs<TKey, TValue> s_reset = new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Reset);

    private readonly NotifyDictionaryChangedAction _action;
    private readonly TKey? _key;
    private readonly TValue? _oldValue;
    private readonly TValue? _newValue;

    internal NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction action, TKey? key = default, TValue? oldValue = default, TValue? newValue = default)
    {
        AssertIsDefined(action);

        _action = action;
        _key = key;
        _oldValue = oldValue;
        _newValue = newValue;
    }

    /// <summary>Gets the action that caused the event.</summary>
    public NotifyDictionaryChangedAction Action => _action;

    /// <summary>Gets the key of the item that caused the event.</summary>
    /// <remarks>The value may not be valid if <see cref="Action" /> is <see cref="NotifyDictionaryChangedAction.Reset" />.</remarks>
    public TKey? Key
    {
        get
        {
            Assert(_action != NotifyDictionaryChangedAction.Reset);
            return _key;
        }
    }

    /// <summary>Gets the old value of the item that caused the event.</summary>
    /// <remarks>The value may not be valid if <see cref="Action" /> is not <see cref="NotifyDictionaryChangedAction.ValueChanged" />.</remarks>
    public TValue? OldValue
    {
        get
        {
            Assert(_action == NotifyDictionaryChangedAction.ValueChanged);
            return _oldValue;
        }
    }

    /// <summary>Gets the new value of the item that caused the event.</summary>
    /// <remarks>The value may not be valid if <see cref="Action" /> is not <see cref="NotifyDictionaryChangedAction.ValueChanged" />.</remarks>
    public TValue? NewValue
    {
        get
        {
            Assert(_action == NotifyDictionaryChangedAction.ValueChanged);
            return _newValue;
        }
    }
}
