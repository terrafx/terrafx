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
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.ApplicationModel
{
    /// <summary>Represents a multimedia-based application.</summary>
    public sealed class Application : IDisposable
    {
        #region Fields
        /// <summary>The <see cref="CompositionHost" /> for the instance.</summary>
        internal readonly CompositionHost _compositionHost;

        /// <summary>The <see cref="Thread" /> that was used to create the instance.</summary>
        internal readonly Thread _parentThread;

        /// <summary>The <see cref="IDispatchManager" /> for the instance.</summary>
        internal readonly Lazy<IDispatchManager> _dispatchManager;

        /// <summary>The <see cref="IGraphicsManager" /> for the instance.</summary>
        internal readonly Lazy<IGraphicsManager> _graphicsManager;

        /// <summary>The <see cref="IWindowManager" /> for the instance.</summary>
        internal readonly Lazy<IWindowManager> _windowManager;

        /// <summary>A value that indicates whether the instance is running.</summary>
        internal bool _isRunning;

        /// <summary>A value that indicates whether the instance is disposed.</summary>
        internal bool _isDisposed;

        /// <summary>A value that indicates whether an exit has been requested.</summary>
        internal bool _exitRequested;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Application" /> class.</summary>
        /// <param name="compositionAssemblies">The set of <see cref="Assembly" /> instances to search for type exports.</param>
        /// <exception cref="InvalidOperationException">The <see cref="ApartmentState" /> for <see cref="Thread.CurrentThread" /> is not <see cref="ApartmentState.STA"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="compositionAssemblies" /> is <c>null</c>.</exception>
        public Application(IEnumerable<Assembly> compositionAssemblies)
        {
            _parentThread = GetParentThread();
            _compositionHost = CreateCompositionHost(compositionAssemblies);
            _dispatchManager = new Lazy<IDispatchManager>(_compositionHost.GetExport<IDispatchManager>, isThreadSafe: true);
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
        /// <summary>Gets the <see cref="CompositionHost" /> for the instance.</summary>
        public CompositionHost CompositionHost
        {
            get
            {
                return _compositionHost;
            }
        }

        /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
        public Thread ParentThread
        {
            get
            {
                return _parentThread;
            }
        }

        /// <summary>Gets the <see cref="IDispatchManager" /> for the instance.</summary>
        public IDispatchManager DispatchManager
        {
            get
            {
                return _dispatchManager.Value;
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

        /// <summary>Gets a value that indicates whether the instance is running.</summary>
        public bool IsRunning
        {
            get
            {
                return _isRunning;
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

        /// <summary>Gets <see cref="Thread.CurrentThread" /> and validates that <see cref="Thread.GetApartmentState" /> is <see cref="ApartmentState.STA" />.</summary>
        /// <returns><see cref="Thread.CurrentThread"/></returns>
        /// <exception cref="InvalidOperationException">The <see cref="ApartmentState" /> for <see cref="Thread.CurrentThread" /> is not <see cref="ApartmentState.STA"/>.</exception>
        internal static Thread GetParentThread()
        {
            var currentThread = Thread.CurrentThread;

            if (currentThread.GetApartmentState() != ApartmentState.STA)
            {
                ThrowInvalidOperationException(nameof(Thread.GetApartmentState), currentThread.GetApartmentState());
            }

            return currentThread;
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
            ThrowIfDisposed();

            if (_isRunning == false)
            {
                _exitRequested = true;
            }
        }

        /// <summary>Runs the event loop for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
        /// <remarks>This method does nothing if an exit is requested before the first iteration of the event loop has started.</remarks>
        public void Run()
        {
            ThrowIfDisposed();
            ThrowIfNotDispatcherThread();

            Debug.Assert(_isRunning == false);

            _isRunning = true;
            {
                var dispatchManager = _dispatchManager.Value;
                RunLoop(dispatchManager, dispatchManager.DispatcherForCurrentThread);
            }
            _isRunning = false;
        }

        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        /// <remarks>This method will call <see cref="RequestExit" /> to ensure that the event loop is no longer running.</remarks>
        internal void Dispose(bool isDisposing)
        {
            if (_isDisposed)
            {
                Debug.Assert(_isRunning == false);
                return;
            }

            // Make sure we request an exit before actually disposing anything
            RequestExit();

            if (isDisposing)
            {
                _compositionHost?.Dispose();
            }

            _isDisposed = true;
        }

        /// <summary>Raises the <see cref="Idle" /> event.</summary>
        /// <param name="delta">The delta between the current and previous <see cref="Idle" /> events.</param>
        /// <remarks>This method takes a <see cref="TimeSpan" /> rather than a <see cref="IdleEventArgs" /> to help reduce allocations when <see cref="Idle" /> is <c>null</c>.</remarks>
        internal void OnIdle(TimeSpan delta)
        {
            if (Idle != null)
            {
                var eventArgs = new IdleEventArgs(delta);
                Idle(this, eventArgs);
            }
        }

        /// <summary>Runs the event loop for the instance.</summary>
        /// <param name="dispatchManager">The <see cref="IDispatchManager" /> for the instance.</param>
        /// <param name="parentDispatcher">The <see cref="IDispatchManager" /> associated with <see cref="ParentThread" />.</param>
        internal void RunLoop(IDispatchManager dispatchManager, IDispatcher parentDispatcher)
        {
            Debug.Assert(_isRunning);
            Debug.Assert(Thread.CurrentThread == _parentThread);
            Debug.Assert(dispatchManager != null);
            Debug.Assert(parentDispatcher != null);
            Debug.Assert(parentDispatcher.ParentThread == _parentThread);

            var previousTimestamp = dispatchManager.CurrentTimestamp;

            while (_exitRequested == false)
            {
                parentDispatcher.DispatchPending();

                var currentTimestamp = dispatchManager.CurrentTimestamp;
                {
                    var delta = currentTimestamp - previousTimestamp;
                    OnIdle(delta);
                }
                previousTimestamp = currentTimestamp;
            }
            _exitRequested = false;
        }

        /// <summary>Throws a <see cref="ObjectDisposedException" /> if the instance has already been disposed.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        internal void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                ThrowObjectDisposedException(nameof(Application));
            }
        }

        /// <summary>Throws a <see cref="InvalidOperationException" /> if <see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
        internal void ThrowIfNotDispatcherThread()
        {
            if (Thread.CurrentThread != _parentThread)
            {
                ThrowInvalidOperationException(nameof(Thread.CurrentThread), Thread.CurrentThread);
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
    }
}
