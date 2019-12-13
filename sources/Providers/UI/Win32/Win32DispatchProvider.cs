// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.Kernel32;
using static TerraFX.UI.Providers.Win32.HelperUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI.Providers.Win32
{
    /// <summary>Provides access to a Win32 based dispatch subsystem.</summary>
    public sealed unsafe class Win32DispatchProvider : IDispatchProvider
    {
        private static ValueLazy<Win32DispatchProvider> s_instance = new ValueLazy<Win32DispatchProvider>(CreateDispatchProvider);

        private readonly double _tickFrequency;
        private readonly ConcurrentDictionary<Thread, IDispatcher> _dispatchers;

        private Win32DispatchProvider()
        {
            _tickFrequency = GetTickFrequency();
            _dispatchers = new ConcurrentDictionary<Thread, IDispatcher>();
        }

        /// <summary>Gets the <see cref="Win32DispatchProvider" /> instance for the current program.</summary>
        public static Win32DispatchProvider Instance => s_instance.Value;

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
            return _dispatchers.GetOrAdd(thread, (parentThread) => new Win32Dispatcher(this, parentThread));
        }

        /// <inheritdoc />
        public bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out IDispatcher dispatcher)
        {
            ThrowIfNull(thread, nameof(thread));
            return _dispatchers.TryGetValue(thread, out dispatcher!);
        }

        private static Win32DispatchProvider CreateDispatchProvider() => new Win32DispatchProvider();

        private static double GetTickFrequency()
        {
            LARGE_INTEGER frequency;
            ThrowExternalExceptionIfFalse(nameof(QueryPerformanceFrequency), QueryPerformanceFrequency(&frequency));

            const double TicksPerSecond = Timestamp.TicksPerSecond;
            return TicksPerSecond / frequency.QuadPart;
        }
    }
}
