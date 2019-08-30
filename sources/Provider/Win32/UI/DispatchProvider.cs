// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop;
using TerraFX.UI;
using static TerraFX.Interop.Kernel32;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Provides access to a Win32 based dispatch subsystem.</summary>
    [Export(typeof(IDispatchProvider))]
    [Export(typeof(DispatchProvider))]
    [Shared]
    public sealed unsafe class DispatchProvider : IDispatchProvider
    {
        /// <summary>The tick frequency for the system's monotonic timer.</summary>
        private readonly double _tickFrequency;

        /// <summary>The <see cref="IDispatcher" /> instances that have been created by the instance.</summary>
        private readonly ConcurrentDictionary<Thread, IDispatcher> _dispatchers;

        /// <summary>Initializes a new instance of the <see cref="DispatchProvider" /> class.</summary>
        /// <exception cref="ExternalException">The call to <see cref="QueryPerformanceFrequency(LARGE_INTEGER*)" /> failed.</exception>
        public DispatchProvider()
        {
            _tickFrequency = GetTickFrequency();
            _dispatchers = new ConcurrentDictionary<Thread, IDispatcher>();
        }

        /// <summary>Gets the tick frequency for the system's monotonic timer.</summary>
        /// <returns>The tick frequency for the system's monotonic timer.</returns>
        /// <exception cref="ExternalException">The call to <see cref="QueryPerformanceFrequency(LARGE_INTEGER*)" /> failed.</exception>
        private static double GetTickFrequency()
        {
            LARGE_INTEGER frequency;
            var succeeded = QueryPerformanceFrequency(&frequency);

            if (succeeded == FALSE)
            {
                ThrowExternalExceptionForLastError(nameof(QueryPerformanceFrequency));
            }

            const double ticksPerSecond = Timestamp.TicksPerSecond;
            return ticksPerSecond / frequency.QuadPart;
        }

        /// <summary>Gets the current <see cref="Timestamp" /> for the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="QueryPerformanceCounter(LARGE_INTEGER*)" /> failed.</exception>
        public Timestamp CurrentTimestamp
        {
            get
            {
                LARGE_INTEGER performanceCount;
                var succeeded = QueryPerformanceCounter(&performanceCount);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(QueryPerformanceCounter));
                }

                var ticks = (long)(performanceCount.QuadPart * _tickFrequency);
                return new Timestamp(ticks);
            }
        }

        /// <summary>Gets the <see cref="IDispatcher" /> instance for <see cref="Thread.CurrentThread" />.</summary>
        /// <returns>The <see cref="IDispatcher" /> instance for <see cref="Thread.CurrentThread" />.</returns>
        /// <remarks>This will create a new <see cref="IDispatcher" /> instance if one does not already exist.</remarks>
        public IDispatcher DispatcherForCurrentThread
        {
            get
            {
                return GetDispatcher(Thread.CurrentThread);
            }
        }

        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with a <see cref="Thread" />, creating one if it does not exist.</summary>
        /// <param name="thread">The <see cref="Thread" /> for which the <see cref="IDispatcher" /> instance should be retrieved.</param>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <paramref name="thread" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">A <see cref="IDispatcher" /> instance for <paramref name="thread" /> could not be found.</exception>
        public IDispatcher GetDispatcher(Thread thread)
        {
            ThrowIfNull(thread, nameof(thread));
            return _dispatchers.GetOrAdd(thread, (parentThread) => new Dispatcher(this, parentThread));
        }

        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with a <see cref="Thread" /> or <c>null</c> if one does not exist.</summary>
        /// <param name="thread">The <see cref="Thread" /> for which the <see cref="IDispatcher" /> instance should be retrieved.</param>
        /// <param name="dispatcher">The <see cref="IDispatcher" /> instance associated with <paramref name="thread" />.</param>
        /// <returns><c>true</c> if a <see cref="IDispatcher" /> instance was found for <paramref name="thread" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
        public bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out IDispatcher dispatcher)
        {
            ThrowIfNull(thread, nameof(thread));
            return _dispatchers.TryGetValue(thread, out dispatcher!);
        }
    }
}
