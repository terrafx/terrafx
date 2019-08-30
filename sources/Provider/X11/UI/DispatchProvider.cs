// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.c;
using static TerraFX.Interop.libc;
using static TerraFX.Interop.X11;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.X11.UI
{
    /// <summary>Provides access to an X11 based dispatch subsystem.</summary>
    [Export(typeof(IDispatchProvider))]
    [Export(typeof(DispatchProvider))]
    [Shared]
    public sealed unsafe class DispatchProvider : IDisposable, IDispatchProvider
    {
        /// <summary>The <c>Display</c> that was created for the instance.</summary>
        private readonly Lazy<IntPtr> _display;

        /// <summary>The <see cref="IDispatcher" /> instances that have been created by the instance.</summary>
        private readonly ConcurrentDictionary<Thread, IDispatcher> _dispatchers;

        /// <summary>The <see cref="State" /> of the instance.</summary>
        private State _state;

        /// <summary>Initializes a new instance of the <see cref="DispatchProvider" /> class.</summary>
        [ImportingConstructor]
        public DispatchProvider()
        {
            _display = new Lazy<IntPtr>((Func<IntPtr>)CreateDisplay, isThreadSafe: true);
            _dispatchers = new ConcurrentDictionary<Thread, IDispatcher>();
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="DispatchProvider" /> class.</summary>
        ~DispatchProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <c>Display</c> that was created for the instance.</summary>
        public IntPtr Display => _state.IsNotDisposedOrDisposing ? _display.Value : IntPtr.Zero;

        /// <summary>Gets the current <see cref="Timestamp" /> for the instance.</summary>
        public Timestamp CurrentTimestamp
        {
            get
            {
                timespec timespec;
                var result = clock_gettime(CLOCK_MONOTONIC, &timespec);

                if (result != 0)
                {
                    ThrowExternalExceptionForLastError(nameof(clock_gettime));
                }

                const long NanosecondsPerSecond = TimeSpan.TicksPerSecond * 100;
                Assert(NanosecondsPerSecond == 1000000000, Resources.ArgumentOutOfRangeExceptionMessage, nameof(NanosecondsPerSecond), NanosecondsPerSecond);

                var ticks = (long)timespec.tv_sec;
                {
                    ticks *= NanosecondsPerSecond;
                    ticks += (long)timespec.tv_nsec;
                    ticks /= 100;
                }
                return new Timestamp(ticks);
            }
        }

        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</summary>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</returns>
        /// <remarks>This will create a new <see cref="IDispatcher" /> instance if one does not already exist.</remarks>
        public IDispatcher DispatcherForCurrentThread => GetDispatcher(Thread.CurrentThread);

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
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

        /// <summary>Creates a <see cref="Display" />.</summary>
        /// <returns>The created <see cref="Display" />.</returns>
        /// <exception cref="ExternalException">The call to <see cref="XOpenDisplay(sbyte*)" /> failed.</exception>
        private static IntPtr CreateDisplay()
        {
            var display = XOpenDisplay(param0: null);

            if (display == null)
            {
                ThrowExternalExceptionForLastError(nameof(XOpenDisplay));
            }

            return (IntPtr)display;
        }

        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                DisposeDisplay();
            }

            _state.EndDispose();
        }

        /// <summary>Disposes of the <c>Display</c> that was created for the instance.</summary>
        private void DisposeDisplay()
        {
            _state.AssertDisposing();

            if (_display.IsValueCreated)
            {
                _ = XCloseDisplay((XDisplay*)_display.Value);
            }
        }
    }
}
