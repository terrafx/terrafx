// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Composition;
using System.Diagnostics;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Threading;
using static TerraFX.Interop.libc;
using static TerraFX.Interop.libX11;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.libX11.Threading
{
    /// <summary>Provides a means of managing the message dispatch objects for an application.</summary>
    [Export(typeof(IDispatchManager))]
    [Export(typeof(DispatchManager))]
    [Shared]
    public sealed unsafe class DispatchManager : IDisposable, IDispatchManager
    {
        #region Fields
        internal IntPtr _display;

        internal readonly ConcurrentDictionary<Thread, Dispatcher> _dispatchers;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="DispatchManager" /> class.</summary>
        [ImportingConstructor]
        internal DispatchManager()
        {
            _display = CreateDisplay();
            _dispatchers = new ConcurrentDictionary<Thread, Dispatcher>();
        }
        #endregion

        #region Destructors
        /// <summary>Finalizes an instance of the <see cref="DispatchManager" /> class.</summary>
        ~DispatchManager()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region Properties
        /// <summary>Gets a pointer to the <see cref="Display" /> instance.</summary>
        public IntPtr Display
        {
            get
            {
                return _display;
            }
        }
        #endregion

        #region TerraFX.Threading.IDispatchManager Properties
        /// <summary>Gets the current <see cref="Timestamp" /> for the instance.</summary>
        public Timestamp CurrentTimestamp
        {
            get
            {
                timespec timespec;
                var result = clock_gettime(CLOCK_MONOTONIC, &timespec);

                if (result != 0)
                {
                    Debug.Assert(result == -1);
                    ThrowExternalExceptionForLastError(nameof(clock_gettime));
                }

                const long NanosecondsPerSecond = 1000000000;

                var ticks = (long)(timespec.tv_sec);
                {
                    ticks *= NanosecondsPerSecond;
                    ticks += (long)(timespec.tv_nsec);
                    ticks /= 100;
                }
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
                return _dispatchers.GetOrAdd(Thread.CurrentThread, (thread) => new Dispatcher(this, thread));
            }
        }
        #endregion

        #region Static Methods
        internal static IntPtr CreateDisplay()
        {
            var display = XOpenDisplay(display_name: null);

            if (display == IntPtr.Zero)
            {
                ThrowExternalExceptionForLastError(nameof(XOpenDisplay));
            }

            return display;
        }
        #endregion

        #region Methods
        internal void Dispose(bool isDisposing)
        {
            if (_display != IntPtr.Zero)
            {
                XCloseDisplay(_display);
                _display = IntPtr.Zero;
            }
        }
        #endregion

        #region System.IDisposable Methods
        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
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
                ThrowArgumentNullException(nameof(thread));
            }

            _dispatchers.TryGetValue(thread, out var dispatcher);
            return dispatcher;
        }
        #endregion
    }
}
