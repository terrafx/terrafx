// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#pragma warning disable CA1711 //  Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="NotifyCollectionChangedEventArgs{T}" /> class.</summary>
public static class NotifyCollectionChangedEventArgs
{
    /// <summary>Gets or creates an instance of the <see cref="NotifyCollectionChangedEventArgs{T}" /> class for the <see cref="NotifyCollectionChangedAction.Add" /> action.</summary>
    /// <param name="value">The item that caused the event.</param>
    /// <returns>An instance of the <see cref="NotifyCollectionChangedEventArgs{T}" /> class.</returns>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static NotifyCollectionChangedEventArgs<T> ForAddAction<T>(T value) => new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, value);

    /// <summary>Gets or creates an instance of the <see cref="NotifyCollectionChangedEventArgs{T}" /> class for the <see cref="NotifyCollectionChangedAction.Remove" /> action.</summary>
    /// <param name="value">The item that caused the event.</param>
    /// <returns>An instance of the <see cref="NotifyCollectionChangedEventArgs{T}" /> class.</returns>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static NotifyCollectionChangedEventArgs<T> ForRemoveAction<T>(T value) => new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, value);

    /// <summary>Gets or creates an instance of the <see cref="NotifyCollectionChangedEventArgs{T}" /> class for the <see cref="NotifyCollectionChangedAction.Reset" /> action.</summary>
    /// <returns>An instance of the <see cref="NotifyCollectionChangedEventArgs{T}" /> class.</returns>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static NotifyCollectionChangedEventArgs<T> ForResetAction<T>() => NotifyCollectionChangedEventArgs<T>.s_reset;
}
