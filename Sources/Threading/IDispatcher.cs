// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Threading;

namespace TerraFX.Threading
{
    /// <summary>Provides a means of dispatching messages for a thread.</summary>
    public interface IDispatcher
    {
        #region Properties
        /// <summary>Gets the <see cref="Thread" /> associated with the instance.</summary>
        Thread ParentThread { get; }
        #endregion

        #region Methods
        /// <summary>Dispatches all messages currently pending in the queue.</summary>
        /// <remarks>This method does not wait for a new event to be raised if the queue is empty.</remarks>
        void DispatchPending();
        #endregion
    }
}
