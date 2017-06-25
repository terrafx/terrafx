// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Composition;
using System.Threading;
using TerraFX.Threading;
using TerraFX.Utilities;

namespace TerraFX.Provider.Win32.Threading
{
    /// <summary>Provides a means of managing the message dispatch objects for an application.</summary>
    [Export(typeof(IDispatchManager))]
    [Shared]
    public sealed class DispatchManager : IDispatchManager
    {
        #region Fields
        private static readonly ConcurrentDictionary<Thread, Dispatcher> _dispatchers = new ConcurrentDictionary<Thread, Dispatcher>();
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="DispatchManager" /> class.</summary>
        public DispatchManager()
        {
        }
        #endregion

        #region Properties
        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</summary>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</returns>
        /// <remarks>This will create a new <see cref="IDispatcher" /> instance if one does not already exist.</remarks>
        public static IDispatcher DispatcherForCurrentThread
        {
            get
            {
                return _dispatchers.GetOrAdd(Thread.CurrentThread, (thread) => new Dispatcher(thread));
            }
        }
        #endregion

        #region Methods
        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with a <see cref="Thread" />.</summary>
        /// <param name="thread">The <see cref="Thread" /> for which the <see cref="IDispatcher" /> instance should be retrieved.</param>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <paramref name="thread" /> or <c>null</c> if an instance does not exist.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
        public static IDispatcher GetDispatcherForThread(Thread thread)
        {
            if (thread is null)
            {
                ExceptionUtilities.ThrowArgumentNullException(nameof(thread));
            }

            _dispatchers.TryGetValue(thread, out var dispatcher);
            return dispatcher;
        }
        #endregion

        #region TerraFX.Threading.IDispatchManager
        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</summary>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</returns>
        /// <remarks>This will create a new <see cref="IDispatcher" /> instance if one does not already exist.</remarks>
        IDispatcher IDispatchManager.DispatcherForCurrentThread
        {
            get
            {
                return DispatcherForCurrentThread;
            }
        }

        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with a <see cref="Thread" />.</summary>
        /// <param name="thread">The <see cref="Thread" /> for which the <see cref="IDispatcher" /> instance should be retrieved.</param>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <paramref name="thread" /> or <c>null</c> if an instance does not exist.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
        IDispatcher IDispatchManager.GetDispatcherForThread(Thread thread)
        {
            return GetDispatcherForThread(thread);
        }
        #endregion
    }
}
