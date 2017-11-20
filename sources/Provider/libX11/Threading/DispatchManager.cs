// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Composition;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Interop.libc;
using static TerraFX.Interop.libX11;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.libX11.Threading
{
    /// <summary>Provides a means of managing the message dispatch objects for an application.</summary>
    [Export(typeof(IDispatchManager))]
    [Export(typeof(DispatchManager))]
    [Shared]
    public sealed unsafe class DispatchManager : IDisposable, IDispatchManager
    {
        #region Fields
        /// <summary>The <c>Display</c> that was created for the instance.</summary>
        internal readonly Lazy<IntPtr> _display;

        /// <summary>The <see cref="IDispatcher" /> instances that have been created by the instance.</summary>
        internal readonly ConcurrentDictionary<Thread, IDispatcher> _dispatchers;

        /// <summary>The <see cref="State" /> of the instance.</summary>
        internal readonly State _state;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="DispatchManager" /> class.</summary>
        [ImportingConstructor]
        public DispatchManager()
        {
            _display = new Lazy<IntPtr>(CreateDisplay, isThreadSafe: true);
            _dispatchers = new ConcurrentDictionary<Thread, IDispatcher>();
            _state.Transition(to: Initialized);
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
        /// <summary>Gets the <c>Display</c> that was created for the instance.</summary>
        public IntPtr Display
        {
            get
            {
                return _display.Value;
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
                return GetDispatcher(Thread.CurrentThread);
            }
        }
        #endregion

        #region Static Methods
        /// <summary>Creates a <see cref="Display" />.</summary>
        /// <returns>The created <see cref="Display" />.</returns>
        /// <exception cref="ExternalException">The call to <see cref="XOpenDisplay(byte*)" /> failed.</exception>
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
        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        internal void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                DisposeDisplay();
            }

            _state.EndDispose();
        }

        /// <summary>Disposes of the <c>Display</c> that was created for the instance.</summary>
        internal void DisposeDisplay()
        {
            if (_display.IsValueCreated)
            {
                XCloseDisplay(_display.Value);
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
        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with a <see cref="Thread" />, creating one if it does not exist.</summary>
        /// <param name="thread">The <see cref="Thread" /> for which the <see cref="IDispatcher" /> instance should be retrieved.</param>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <paramref name="thread" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">A <see cref="IDispatcher" /> instance for <paramref name="thread" /> could not be found.</exception>
        public IDispatcher GetDispatcher(Thread thread)
        {
            if (thread is null)
            {
                ThrowArgumentNullException(nameof(thread));
            }

            return _dispatchers.GetOrAdd(thread, (parentThread) => new Dispatcher(this, parentThread));
        }

        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with a <see cref="Thread" /> or <c>null</c> if one does not exist.</summary>
        /// <param name="thread">The <see cref="Thread" /> for which the <see cref="IDispatcher" /> instance should be retrieved.</param>
        /// <param name="dispatcher">The <see cref="IDispatcher" /> instance associated with <paramref name="thread" />.</param>
        /// <returns><c>true</c> if a <see cref="IDispatcher" /> instance was found for <paramref name="thread" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
        public bool TryGetDispatcher(Thread thread, out IDispatcher dispatcher)
        {
            if (thread is null)
            {
                ThrowArgumentNullException(nameof(thread));
            }

            return _dispatchers.TryGetValue(thread, out dispatcher);
        }
        #endregion
    }
}
