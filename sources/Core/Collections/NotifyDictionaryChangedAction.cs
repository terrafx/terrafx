// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Collections;

/// <summary>Defines the action that caused a <see cref="INotifyDictionaryChanged{TKey, TValue}.DictionaryChanged" /> event.</summary>
public enum NotifyDictionaryChangedAction
{
    /// <summary>An item was added to the dictionary.</summary>
    Add,

    /// <summary>An item was removed from the dictionary.</summary>
    Remove,

    /// <summary>An item in the dictionary had its value changed.</summary>
    ValueChanged,

    /// <summary>The dictionary had its contents reset.</summary>
    Reset
}
