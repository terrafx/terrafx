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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IDispatcher DispatcherForCurrentThread => GetDispatcher(Thread.CurrentThread);

        /// <inheritdoc />
        public IDispatcher GetDispatcher(Thread thread)
        {
            ThrowIfNull(thread, nameof(thread));
            return _dispatchers.GetOrAdd(thread, (parentThread) => new Dispatcher(this, parentThread));
        }

        /// <inheritdoc />
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
