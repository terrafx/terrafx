// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace TerraFX.UI
{
    /// <summary>Provides access to a dispatch subsystem.</summary>
    public abstract class DispatchProvider : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="DispatchProvider" /> class.</summary>
        protected DispatchProvider() { }

        /// <summary>Gets the current <see cref="Timestamp" /> for the instance.</summary>
        public abstract Timestamp CurrentTimestamp { get; }

        /// <summary>Gets the <see cref="Dispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</summary>
        /// <returns>The <see cref="Dispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</returns>
        /// <remarks>This will create a new <see cref="Dispatcher" /> instance if one does not already exist.</remarks>
        public abstract Dispatcher DispatcherForCurrentThread { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Gets the <see cref="Dispatcher" /> instance associated with a <see cref="Thread" />, creating one if it does not exist.</summary>
        /// <param name="thread">The <see cref="Thread" /> for which the <see cref="Dispatcher" /> instance should be retrieved.</param>
        /// <returns>The <see cref="Dispatcher" /> instance associated with <paramref name="thread" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
        public abstract Dispatcher GetDispatcher(Thread thread);

        /// <summary>Gets the <see cref="Dispatcher" /> instance associated with a <see cref="Thread" /> or <c>null</c> if one does not exist.</summary>
        /// <param name="thread">The <see cref="Thread" /> for which the <see cref="Dispatcher" /> instance should be retrieved.</param>
        /// <param name="dispatcher">The <see cref="Dispatcher" /> instance associated with <paramref name="thread" />.</param>
        /// <returns><c>true</c> if a <see cref="Dispatcher" /> instance was found for <paramref name="thread" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
        public abstract bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out Dispatcher dispatcher);

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
