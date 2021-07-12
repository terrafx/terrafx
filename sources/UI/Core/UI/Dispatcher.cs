// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI
{
    /// <summary>Provides a means of dispatching events for a thread.</summary>
    public abstract class Dispatcher : IDisposable
    {
        private readonly DispatchService _dispatchService;
        private readonly Thread _parentThread;

        /// <summary>Initializes a new instance of the <see cref="Dispatcher" /> class.</summary>
        /// <param name="dispatchService">The dispatch service which created the dispatcher.</param>
        /// <param name="parentThread">The thread on which the dispatcher operates.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dispatchService" /> is <c>null</c>.</exception>
        protected Dispatcher(DispatchService dispatchService, Thread parentThread)
        {
            ThrowIfNull(dispatchService, nameof(dispatchService));
            ThrowIfNull(parentThread, nameof(parentThread));

            _dispatchService = dispatchService;
            _parentThread = parentThread;
        }

        /// <summary>Occurs when an exit event is dispatched from the queue.</summary>
        public abstract event EventHandler ExitRequested;

        /// <summary>Gets the <see cref="UI.DispatchService" /> for the instance.</summary>
        public DispatchService DispatchService => _dispatchService;

        /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
        public Thread ParentThread => _parentThread;

        /// <summary>Dispatches all events currently pending in the queue.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
        /// <remarks>
        ///   <para>This method does not wait for a new event to be raised if the queue is empty.</para>
        ///   <para>This method does not performing any translation or pre-processing on the dispatched events.</para>
        ///   <para>This method will continue dispatching pending events even after the <see cref="ExitRequested" /> event is raised.</para>
        /// </remarks>
        public abstract void DispatchPending();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
