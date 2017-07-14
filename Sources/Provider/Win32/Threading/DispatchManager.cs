// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Composition;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Interop.Kernel32;

namespace TerraFX.Provider.Win32.Threading
{
    /// <summary>Provides a means of managing the message dispatch objects for an application.</summary>
    [Export(typeof(IDispatchManager))]
    [Export(typeof(DispatchManager))]
    [Shared]
    unsafe public sealed class DispatchManager : IDispatchManager
    {
        #region Fields
        internal readonly double _tickFrequency;

        internal readonly ConcurrentDictionary<Thread, Dispatcher> _dispatchers;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="DispatchManager" /> class.</summary>
        public DispatchManager()
        {
            LARGE_INTEGER frequency;

            var succeeded = QueryPerformanceFrequency(&frequency);

            if (succeeded == 0)
            {
                ExceptionUtilities.ThrowExternalExceptionForLastError(nameof(QueryPerformanceFrequency));
            }

            const double ticksPerSecond = Timestamp.TicksPerSecond;
            _tickFrequency = (ticksPerSecond / frequency.QuadPart);

            _dispatchers = new ConcurrentDictionary<Thread, Dispatcher>();
        }
        #endregion

        #region TerraFX.Threading.IDispatchManager Properties
        /// <summary>Gets the current <see cref="Timestamp" /> for the instance.</summary>
        public Timestamp CurrentTimestamp
        {
            get
            {
                LARGE_INTEGER performanceCount;
                var succeeded = QueryPerformanceCounter(&performanceCount);

                if (succeeded == 0)
                {
                    ExceptionUtilities.ThrowExternalExceptionForLastError(nameof(QueryPerformanceCounter));
                }

                var ticks = (long)(performanceCount.QuadPart * _tickFrequency);
                return new Timestamp(ticks);
            }
        }

        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</summary>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</returns>
        /// <remarks>This will create a new <see cref="IDispatcher" /> instance if one does not already exist.</remarks>
        public IDispatcher DispatcherForCurrentThread
        {
            get
            {
                return _dispatchers.GetOrAdd(Thread.CurrentThread, (thread) => new Dispatcher(thread));
            }
        }
        #endregion

        #region TerraFX.Threading.IDispatchManager Methods
        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with a <see cref="Thread" />.</summary>
        /// <param name="thread">The <see cref="Thread" /> for which the <see cref="IDispatcher" /> instance should be retrieved.</param>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <paramref name="thread" /> or <c>null</c> if an instance does not exist.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
        public IDispatcher GetDispatcherForThread(Thread thread)
        {
            if (thread is null)
            {
                ExceptionUtilities.ThrowArgumentNullException(nameof(thread));
            }

            _dispatchers.TryGetValue(thread, out var dispatcher);
            return dispatcher;
        }
        #endregion
    }
}
