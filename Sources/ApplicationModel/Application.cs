// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using TerraFX.Graphics;
using TerraFX.Threading;
using TerraFX.UI;
using static System.Threading.Interlocked;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.ApplicationModel
{
    /// <summary>A multimedia-based application.</summary>
    public sealed class Application : IDisposable, IServiceProvider
    {
        #region State Constants
        /// <summary>The event loop for the instance is stopped.</summary>
        internal const int Stopped = 0;

        /// <summary>The event loop for the instance is running.</summary>
        internal const int Running = 1;

        /// <summary>The event loop for the instance is exiting.</summary>
        internal const int Exiting = 2;

        /// <summary>The instance is being disposed.</summary>
        internal const int Disposing = 3;

        /// <summary>The instance has been disposed.</summary>
        internal const int Disposed = 4;
        #endregion

        #region Fields
        /// <summary>The <see cref="CompositionHost" /> for the instance.</summary>
        internal readonly CompositionHost _compositionHost;

        /// <summary>The <see cref="IDispatcher" /> for the <see cref="Thread" /> that was used to create the instance.</summary>
        internal readonly IDispatcher _dispatcher;

        /// <summary>The <see cref="IGraphicsManager" /> for the instance.</summary>
        internal readonly Lazy<IGraphicsManager> _graphicsManager;

        /// <summary>The <see cref="IWindowManager" /> for the instance.</summary>
        internal readonly Lazy<IWindowManager> _windowManager;

        /// <summary>The state for the instance.</summary>
        /// <remarks>
        ///     <para>This field is <c>volatile</c> to ensure state changes update all threads simultaneously.</para>
        ///     <para><c>volatile</c> does add a read/write barrier at every access, but the state transitions are believed to be infrequent enough for this to not be a problem.</para>
        /// </remarks>
        internal volatile int _state;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Application" /> class.</summary>
        /// <param name="compositionAssemblies">The set of <see cref="Assembly" /> instances to search for type exports.</param>
        /// <exception cref="InvalidOperationException">The <see cref="ApartmentState" /> for <see cref="Thread.CurrentThread" /> is not <see cref="ApartmentState.STA"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="compositionAssemblies" /> is <c>null</c>.</exception>
        public Application(IEnumerable<Assembly> compositionAssemblies)
        {
            ThrowIfCurrentThreadIsNotSTA();

            var compositionHost = CreateCompositionHost(compositionAssemblies);
            var dispatchManager = _compositionHost.GetExport<IDispatchManager>();

            _dispatcher = dispatchManager.DispatcherForCurrentThread;

            _graphicsManager = new Lazy<IGraphicsManager>(_compositionHost.GetExport<IGraphicsManager>, isThreadSafe: true);
            _windowManager = new Lazy<IWindowManager>(_compositionHost.GetExport<IWindowManager>, isThreadSafe: true);
        }
        #endregion

        #region Destructors
        /// <summary>Finalizes an instance of the <see cref="Application" /> class.</summary>
        ~Application()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region Events
        /// <summary>Occurs when the event loop for the current instance becomes idle.</summary>
        public event EventHandler<IdleEventArgs> Idle;
        #endregion

        #region Properties
        /// <summary>Gets the <see cref="IDispatcher" /> for the <see cref="Thread" /> that was used to create the instance.</summary>
        public IDispatcher Dispatcher
        {
            get
            {
                return _dispatcher;
            }
        }

        /// <summary>Gets the <see cref="IDispatchManager" /> for the instance.</summary>
        public IDispatchManager DispatchManager
        {
            get
            {
                return _dispatcher.DispatchManager;
            }
        }

        /// <summary>Gets the <see cref="IGraphicsManager" /> for the instance.</summary>
        public IGraphicsManager GraphicsManager
        {
            get
            {
                return _graphicsManager.Value;
            }
        }

        /// <summary>Gets a value that indicates whether the event loop for the instance is running.</summary>
        public bool IsRunning
        {
            get
            {
                return (_state == Running);
            }
        }

        /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
        public Thread ParentThread
        {
            get
            {
                return _dispatcher.ParentThread;
            }
        }

        /// <summary>Gets the <see cref="IWindowManager" /> for the current instance.</summary>
        public IWindowManager WindowManager
        {
            get
            {
                return _windowManager.Value;
            }
        }
        #endregion

        #region Static Methods
        /// <summary>Creates a new instance of the <see cref="CompositionHost" /> class.</summary>
        /// <param name="compositionAssemblies">The set of <see cref="Assembly" /> instances to search for type exports.</param>
        /// <returns>A new instance of the <see cref="CompositionHost" /> configured to use <paramref name="compositionAssemblies" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="compositionAssemblies" /> is <c>null</c>.</exception>
        internal static CompositionHost CreateCompositionHost(IEnumerable<Assembly> compositionAssemblies)
        {
            if (compositionAssemblies is null)
            {
                ThrowArgumentNullException(nameof(compositionAssemblies));
            }

            var containerConfiguration = new ContainerConfiguration();
            {
                containerConfiguration = containerConfiguration.WithAssemblies(compositionAssemblies);
            }
            return containerConfiguration.CreateContainer();
        }

        /// <summary>Thows a <see cref="InvalidOperationException"/> if the <see cref="ApartmentState" /> for <see cref="Thread.CurrentThread" /> is not <see cref="ApartmentState.STA"/>.</summary>
        /// <exception cref="InvalidOperationException">The <see cref="ApartmentState" /> for <see cref="Thread.CurrentThread" /> is not <see cref="ApartmentState.STA"/>.</exception>
        internal static void ThrowIfCurrentThreadIsNotSTA()
        {
            var apartmentState = Thread.CurrentThread.GetApartmentState();

            if (apartmentState != ApartmentState.STA)
            {
                ThrowInvalidOperationException(nameof(Thread.GetApartmentState), apartmentState);
            }
        }

        /// <summary>Throws a <see cref="ObjectDisposedException" /> if the instance has already been disposed.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        internal static void ThrowIfDisposed(int state)
        {
            if (state >= Disposing) // (_state == Disposing) || (_state == Disposed)
            {
                ThrowObjectDisposedException(nameof(Application));
            }
        }
        #endregion

        #region Methods
        /// <summary>Requests that the instance exits the event loop.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <remarks>
        ///     <para>This method does nothing if <see cref="IsRunning" /> is <c>false</c>.</para>
        ///     <para>This method can be called from any thread.</para>
        /// </remarks>
        public void RequestExit()
        {
            var previousState = CompareExchange(ref _state, Exiting, Running); // if (_state == Running) { _state = ExitRequested; }
            ThrowIfDisposed(previousState);
        }

        /// <summary>Runs the event loop for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
        /// <remarks>This method does nothing if an exit is requested before the first iteration of the event loop has started.</remarks>
        public void Run()
        {
            var previousState = CompareExchange(ref _state, Running, Stopped); // if (_state == Stopped) { _state = Running; }

            ThrowIfDisposed(previousState);
            ThrowIfNotParentThread();

            RunEventLoop();

            CompareExchange(ref _state, Stopped, Exiting); // if (_state == ExitRequested) { _state = Stopped; }
        }

        /// <summary>Gets the service object of the specified type.</summary>
        /// <typeparam name="TService">The type of the service object to get.</typeparam>
        /// <returns>A service object of <typeparamref name="TService" /> if one exists; otherwise, <c>default</c>.</returns>
        public TService GetService<TService>()
        {
            _compositionHost.TryGetExport<TService>(out var service);
            return service;
        }

        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        /// <remarks>This method exits the event loop for the instance if it is running.</remarks>
        internal void Dispose(bool isDisposing)
        {
            var previousState = Exchange(ref _state, Disposing);

            if (previousState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                if (isDisposing)
                {
                    _compositionHost?.Dispose();
                }
            }

            _state = Disposed;
        }

        /// <summary>Raises the <see cref="Idle" /> event.</summary>
        /// <param name="delta">The delta between the current and previous <see cref="Idle" /> events.</param>
        /// <remarks>This method takes a <see cref="TimeSpan" /> rather than a <see cref="IdleEventArgs" /> to help reduce allocations when <see cref="Idle" /> is <c>null</c>.</remarks>
        internal void OnIdle(TimeSpan delta)
        {
            var idle = Idle;

            if (idle != null)
            {
                var eventArgs = new IdleEventArgs(delta);
                idle(this, eventArgs);
            }
        }

        /// <summary>Runs the event loop for the instance.</summary>
        internal void RunEventLoop()
        {
            var dispatcher = _dispatcher;
            Debug.Assert(Thread.CurrentThread == dispatcher.ParentThread);

            var dispatchManager = dispatcher.DispatchManager;
            var previousTimestamp = dispatchManager.CurrentTimestamp;

            while (_state == Running)
            {
                dispatcher.DispatchPending();

                var currentTimestamp = dispatchManager.CurrentTimestamp;
                {
                    var delta = currentTimestamp - previousTimestamp;
                    OnIdle(delta);
                }
                previousTimestamp = currentTimestamp;
            }
        }

        /// <summary>Throws a <see cref="InvalidOperationException" /> if <see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
        internal void ThrowIfNotParentThread()
        {
            var currentThread = Thread.CurrentThread;

            if (currentThread != _dispatcher.ParentThread)
            {
                ThrowInvalidOperationException(nameof(Thread.CurrentThread), currentThread);
            }
        }
        #endregion

        #region System.IDisposable Methods
        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region System.IServiceProvider Methods
        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="serviceType">The type of the service object to get.</param>
        /// <returns>A service object of <paramref name="serviceType" /> if one exists; otherwise, <c>null</c>.</returns>
        public object GetService(Type serviceType)
        {
            _compositionHost.TryGetExport(serviceType, out var service);
            return service;
        }
        #endregion
    }
}
