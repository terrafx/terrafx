// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Collections
{
    /// <summary>Defines a method that handles a <see cref="INotifyDictionaryChanged{TKey, TValue}.DictionaryChanged" /> event.</summary>
    /// <param name="sender">The <see cref="object" /> that raised the event.</param>
    /// <param name="eventArgs">The <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> for the event.</param>
    public delegate void NotifyDictionaryChangedEventHandler<TKey, TValue>(
        object sender,
        NotifyDictionaryChangedEventArgs<TKey, TValue> eventArgs
    );
}
