// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;

namespace TerraFX.UI
{
    /// <summary>Provides a means of dispatching events for a thread.</summary>
    public interface IDispatcher
    {
        /// <summary>Occurs when an exit event is dispatched from the queue.</summary>
        event EventHandler ExitRequested;

        /// <summary>Gets the <see cref="IDispatchProvider" /> for the instance.</summary>
        IDispatchProvider DispatchProvider { get; }

        /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
        Thread ParentThread { get; }

        /// <summary>Dispatches all events currently pending in the queue.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
        /// <remarks>
        ///   <para>This method does not wait for a new event to be raised if the queue is empty.</para>
        ///   <para>This method does not performing any translation or pre-processing on the dispatched events.</para>
        ///   <para>This method will continue dispatching pending events even after the <see cref="ExitRequested" /> event is raised.</para>
        /// </remarks>
        void DispatchPending();
    }
}
