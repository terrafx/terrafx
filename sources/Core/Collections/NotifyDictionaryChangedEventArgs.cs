// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#pragma warning disable CA1711 //  Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class.</summary>
public static class NotifyDictionaryChangedEventArgs
{
    /// <summary>Gets or creates an instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class for the <see cref="NotifyDictionaryChangedAction.Add" /> action.</summary>
    /// <param name="key">The key of the item that caused the event.</param>
    /// <returns>An instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class.</returns>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public static NotifyDictionaryChangedEventArgs<TKey, TValue> ForAddAction<TKey, TValue>(TKey key) => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Add, key);

    /// <summary>Gets or creates an instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class for the <see cref="NotifyDictionaryChangedAction.Remove" /> action.</summary>
    /// <param name="key">The key of the item that caused the event.</param>
    /// <returns>An instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class.</returns>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public static NotifyDictionaryChangedEventArgs<TKey, TValue> ForRemoveAction<TKey, TValue>(TKey key) => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Remove, key);

    /// <summary>Gets or creates an instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class for the <see cref="NotifyDictionaryChangedAction.Reset" /> action.</summary>
    /// <returns>An instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class.</returns>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public static NotifyDictionaryChangedEventArgs<TKey, TValue> ForResetAction<TKey, TValue>() => NotifyDictionaryChangedEventArgs<TKey, TValue>.s_reset;

    /// <summary>Gets or creates an instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class for the <see cref="NotifyDictionaryChangedAction.ValueChanged" /> action.</summary>
    /// <param name="key">The key of the item that caused the event.</param>
    /// <param name="oldValue">The old value of the item that caused the event.</param>
    /// <param name="newValue">The new value of the item that caused the event.</param>
    /// <returns>An instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class.</returns>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public static NotifyDictionaryChangedEventArgs<TKey, TValue> ForValueChangedAction<TKey, TValue>(TKey key, TValue oldValue, TValue newValue) => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.ValueChanged, key, oldValue, newValue);
}
