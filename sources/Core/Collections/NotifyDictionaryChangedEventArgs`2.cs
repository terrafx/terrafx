// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Collections
{
    /// <summary>Provides data for the <see cref="INotifyDictionaryChanged{TKey, TValue}.DictionaryChanged" /> event.</summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public sealed class NotifyDictionaryChangedEventArgs<TKey, TValue> : EventArgs
    {
        private static readonly NotifyDictionaryChangedEventArgs<TKey, TValue> s_reset = new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Reset);

        private readonly NotifyDictionaryChangedAction _action;
        private readonly TKey? _key;
        private readonly TValue? _oldValue;
        private readonly TValue? _newValue;

        private NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction action, TKey? key = default, TValue? oldValue = default, TValue? newValue = default)
        {
            Assert(Enum.IsDefined(typeof(NotifyDictionaryChangedAction), action));

            _action = action;
            _key = key;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        /// <summary>Gets the action that caused the event.</summary>
        public NotifyDictionaryChangedAction Action => _action;

        /// <summary>Gets the key of the item that caused the event.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Action" /> is not <see cref="NotifyDictionaryChangedAction.Add" />, <see cref="NotifyDictionaryChangedAction.Remove" />, or <see cref="NotifyDictionaryChangedAction.ValueChanged" />.</exception>
        public TKey? Key
        {
            get
            {
                if (_action == NotifyDictionaryChangedAction.Reset)
                {
                    ThrowInvalidOperationException(Action, nameof(Action));
                }

                return _key;
            }
        }

        /// <summary>Gets the old value of the item that caused the event.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Action" /> is not <see cref="NotifyDictionaryChangedAction.ValueChanged" />.</exception>
        public TValue? OldValue
        {
            get
            {
                if (_action != NotifyDictionaryChangedAction.ValueChanged)
                {
                    ThrowInvalidOperationException(Action, nameof(Action));
                }

                return _oldValue;
            }
        }

        /// <summary>Gets the new value of the item that caused the event.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Action" /> is not <see cref="NotifyDictionaryChangedAction.ValueChanged" />.</exception>
        public TValue? NewValue
        {
            get
            {
                if (_action != NotifyDictionaryChangedAction.ValueChanged)
                {
                    ThrowInvalidOperationException(Action, nameof(Action));
                }

                return _newValue;
            }
        }

        /// <summary>Gets or creates an instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class for the <see cref="NotifyDictionaryChangedAction.Add" /> action.</summary>
        /// <param name="key">The key of the item that caused the event.</param>
        /// <returns>An instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class.</returns>
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> ForAddAction(TKey key) => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Add, key);

        /// <summary>Gets or creates an instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class for the <see cref="NotifyDictionaryChangedAction.Remove" /> action.</summary>
        /// <param name="key">The key of the item that caused the event.</param>
        /// <returns>An instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class.</returns>
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> ForRemoveAction(TKey key) => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Remove, key);

        /// <summary>Gets or creates an instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class for the <see cref="NotifyDictionaryChangedAction.Reset" /> action.</summary>
        /// <returns>An instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class.</returns>
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> ForResetAction() => s_reset;

        /// <summary>Gets or creates an instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class for the <see cref="NotifyDictionaryChangedAction.ValueChanged" /> action.</summary>
        /// <param name="key">The key of the item that caused the event.</param>
        /// <param name="oldValue">The old value of the item that caused the event.</param>
        /// <param name="newValue">The new value of the item that caused the event.</param>
        /// <returns>An instance of the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class.</returns>
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> ForValueChangedAction(TKey key, TValue oldValue, TValue newValue) => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.ValueChanged, key, oldValue, newValue);
    }
}
