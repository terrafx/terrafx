// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Collections
{
    /// <summary>Provides data for the <see cref="INotifyCollectionChanged{T}.CollectionChanged" /> event.</summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public sealed class NotifyCollectionChangedEventArgs<T> : EventArgs
    {
        #region Static Fields
        private static readonly NotifyCollectionChangedEventArgs<T> Reset = new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Reset);
        #endregion

        #region Fields
        private readonly NotifyCollectionChangedAction _action;
        private readonly T _value;
        #endregion

        #region Constructors
        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, T value = default)
        {
            Debug.Assert(Enum.IsDefined(typeof(NotifyCollectionChangedAction), action));

            _action = action;
            _value = value;
        }
        #endregion

        #region Properties
        /// <summary>Gets the action that caused the event.</summary>
        public NotifyCollectionChangedAction Action
        {
            get
            {
                return _action;
            }
        }

        /// <summary>Gets the value of the item that caused the event.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Action" /> is not <see cref="NotifyDictionaryChangedAction.Add" /> or <see cref="NotifyDictionaryChangedAction.Remove" />.</exception>
        public T Value
        {
            get
            {
                if (_action == NotifyCollectionChangedAction.Reset)
                {
                    ThrowInvalidOperationException(nameof(Action), Action);
                }

                return _value;
            }
        }
        #endregion

        #region Static Methods
        /// <summary>Gets or creates an instance of the <see cref="NotifyCollectionChangedEventArgs{T}" /> class for the <see cref="NotifyCollectionChangedAction.Add" /> action.</summary>
        /// <param name="value">The item that caused the event.</param>
        /// <returns>An instance of the <see cref="NotifyCollectionChangedEventArgs{T}" /> class.</returns>
        public static NotifyCollectionChangedEventArgs<T> ForAddAction(T value)
        {
            return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, value);
        }

        /// <summary>Gets or creates an instance of the <see cref="NotifyCollectionChangedEventArgs{T}" /> class for the <see cref="NotifyCollectionChangedAction.Remove" /> action.</summary>
        /// <param name="value">The item that caused the event.</param>
        /// <returns>An instance of the <see cref="NotifyCollectionChangedEventArgs{T}" /> class.</returns>
        public static NotifyCollectionChangedEventArgs<T> ForRemoveAction(T value)
        {
            return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, value);
        }

        /// <summary>Gets or creates an instance of the <see cref="NotifyCollectionChangedEventArgs{T}" /> class for the <see cref="NotifyCollectionChangedAction.Reset" /> action.</summary>
        /// <returns>An instance of the <see cref="NotifyCollectionChangedEventArgs{T}" /> class.</returns>
        public static NotifyCollectionChangedEventArgs<T> ForResetAction()
        {
            return Reset;
        }
        #endregion
    }
}
