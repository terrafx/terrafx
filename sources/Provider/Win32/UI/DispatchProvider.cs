// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.Kernel32;
using static TerraFX.Provider.Win32.HelperUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Provides access to a Win32 based dispatch subsystem.</summary>
    public sealed unsafe class DispatchProvider : IDispatchProvider
    {
        private static ResettableLazy<DispatchProvider> s_instance = new ResettableLazy<DispatchProvider>(CreateDispatchProvider);

        private readonly double _tickFrequency;
        private readonly ConcurrentDictionary<Thread, IDispatcher> _dispatchers;

        private DispatchProvider()
        {
            _tickFrequency = GetTickFrequency();
            _dispatchers = new ConcurrentDictionary<Thread, IDispatcher>();
        }

        /// <summary>Gets the <see cref="DispatchProvider" /> instance for the current program.</summary>
        public static DispatchProvider Instance => s_instance.Value;

        /// <summary>Gets the current <see cref="Timestamp" /> for the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="QueryPerformanceCounter(LARGE_INTEGER*)" /> failed.</exception>
        public Timestamp CurrentTimestamp
        {
            get
            {
                LARGE_INTEGER performanceCount;
                ThrowExternalExceptionIfFalse(nameof(QueryPerformanceCounter), QueryPerformanceCounter(&performanceCount));

                var ticks = (long)(performanceCount.QuadPart * _tickFrequency);
                return new Timestamp(ticks);
            }
        }

        /// <summary>Gets the <see cref="IDispatcher" /> instance for <see cref="Thread.CurrentThread" />.</summary>
        /// <returns>The <see cref="IDispatcher" /> instance for <see cref="Thread.CurrentThread" />.</returns>
        /// <remarks>This will create a new <see cref="IDispatcher" /> instance if one does not already exist.</remarks>
        public IDispatcher DispatcherForCurrentThread => GetDispatcher(Thread.CurrentThread);

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

        private static DispatchProvider CreateDispatchProvider() => new DispatchProvider();

        private static double GetTickFrequency()
        {
            LARGE_INTEGER frequency;
            ThrowExternalExceptionIfFalse(nameof(QueryPerformanceFrequency), QueryPerformanceFrequency(&frequency));

            const double TicksPerSecond = Timestamp.TicksPerSecond;
            return TicksPerSecond / frequency.QuadPart;
        }
    }
}
