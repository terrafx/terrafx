// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Threading;

namespace TerraFX.Threading
{
    /// <summary>Provides a means of managing the message dispatch objects for an application.</summary>
    public interface IDispatchManager
    {
        #region Properties
        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</summary>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</returns>
        /// <remarks>This will create a new <see cref="IDispatcher" /> instance if one does not already exist.</remarks>
        IDispatcher DispatcherForCurrentThread { get; }
        #endregion

        #region Methods
        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with a <see cref="Thread" />.</summary>
        /// <param name="thread">The <see cref="Thread" /> for which the <see cref="IDispatcher" /> instance should be retrieved.</param>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <paramref name="thread" /> or <c>null</c> if an instance does not exist.</returns>
        IDispatcher GetDispatcherForThread(Thread thread);
        #endregion
    }
}
