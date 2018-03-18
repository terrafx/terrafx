// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Composition.Hosting;
using System.Reflection;
using System.Threading;
using TerraFX.Graphics;
using TerraFX.Threading;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Utilities.DisposeUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.ApplicationModel
{
    /// <summary>A multimedia-based application.</summary>
    public sealed class Application : IDisposable, IServiceProvider
    {
        #region State Constants
        /// <summary>The event loop is stopped.</summary>
        internal const int Stopped = 1;

        /// <summary>The event loop is running.</summary>
        internal const int Running = 2;

        /// <summary>The event loop is exiting.</summary>
        internal const int Exiting = 3;
        #endregion

        #region Fields
        /// <summary>The <see cref="Assembly" /> instances to search for type exports.</summary>
        internal readonly Assembly[] _compositionAssemblies;

        /// <summary>The <see cref="Thread" /> that was used to create the instance.</summary>
        internal readonly Thread _parentThread;

        /// <summary>The <see cref="CompositionHost" /> for the instance.</summary>
        internal readonly Lazy<CompositionHost> _compositionHost;

        /// <summary>The <see cref="IDispatchManager" /> for the instance.</summary>
        internal readonly Lazy<IDispatchManager> _dispatchManager;

        /// <summary>The <see cref="IGraphicsManager" /> for the instance.</summary>
        internal readonly Lazy<IGraphicsManager> _graphicsManager;

        /// <summary>The <see cref="IWindowManager" /> for the instance.</summary>
        internal readonly Lazy<IWindowManager> _windowManager;

        /// <summary>The <see cref="State" /> of the instance.</summary>
        internal State _state;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Application" /> class.</summary>
        /// <param name="compositionAssemblies">The <see cref="Assembly" /> instances to search for type exports.</param>
        /// <exception cref="ArgumentNullException"><paramref name="compositionAssemblies" /> is <c>null</c>.</exception>
        public Application(params Assembly[] compositionAssemblies)
        {
            ThrowIfNull(nameof(compositionAssemblies), compositionAssemblies);

            _compositionAssemblies = compositionAssemblies;
            _parentThread = Thread.CurrentThread;

            _compositionHost = new Lazy<CompositionHost>(CreateCompositionHost, isThreadSafe: true);
            _dispatchManager = new Lazy<IDispatchManager>(_compositionHost.Value.GetExport<IDispatchManager>, isThreadSafe: true);
            _graphicsManager = new Lazy<IGraphicsManager>(_compositionHost.Value.GetExport<IGraphicsManager>, isThreadSafe: true);
            _windowManager = new Lazy<IWindowManager>(_compositionHost.Value.GetExport<IWindowManager>, isThreadSafe: true);

            _state.Transition(to: Stopped);
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
                return _parentThread;
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

        #region Methods
        /// <summary>Creates a new instance of the <see cref="CompositionHost" /> class.</summary>
        /// <returns>A new instance of the <see cref="CompositionHost" /> configured to use <see cref="_compositionAssemblies" />.</returns>
        internal CompositionHost CreateCompositionHost()
        {
            _state.ThrowIfDisposedOrDisposing();

            var containerConfiguration = new ContainerConfiguration();
            {
                containerConfiguration = containerConfiguration.WithAssemblies(_compositionAssemblies);
            }
            return containerConfiguration.CreateContainer();
        }

        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        /// <remarks>This method exits the event loop for the instance if it is running.</remarks>
        internal void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                DisposeIfValueCreated(isDisposing, _compositionHost);
            }

            _state.EndDispose();
        }

        /// <summary>Gets the service object of the specified type.</summary>
        /// <typeparam name="TService">The type of the service object to get.</typeparam>
        /// <returns>A service object of <typeparamref name="TService" /> if one exists; otherwise, <c>default</c>.</returns>
        public TService GetService<TService>()
        {
            _compositionHost.Value.TryGetExport<TService>(out var service);
            return service;
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

        /// <summary>Requests that the instance exits the event loop.</summary>
        /// <remarks>
        ///     <para>This method does nothing if <see cref="IsRunning" /> is <c>false</c>.</para>
        ///     <para>This method can be called from any thread.</para>
        /// </remarks>
        public void RequestExit()
        {
            _state.TryTransition(from: Running, to: Exiting);
        }

        /// <summary>Runs the event loop for the instance.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
        /// <exception cref="InvalidOperationException">The state of the object is not <see cref="Stopped" />.</exception>
        /// <remarks>This method does nothing if an exit is requested before the first iteration of the event loop has started.</remarks>
        public void Run()
        {
            ThrowIfNotThread(_parentThread);

            // We enforce the starting transition to be Stopped, which also covers attempting to run a disposed object.
            // However, we do not enforce the ending transition to also be Stopped, as we may have stopped due to disposal.

            _state.Transition(from: Stopped, to: Running);
            {
                var dispatchManager = _dispatchManager.Value;
                var dispatcher = dispatchManager.DispatcherForCurrentThread;
                var previousTimestamp = dispatchManager.CurrentTimestamp;

                // We need to do an initial dispatch to cover the case where a quit
                // message was posted before the message pump was started, otherwise
                // we can end up with a NullReferenceException when we try to execute
                // OnIdle

                dispatcher.DispatchPending();

                while (_state == Running)
                {
                    var currentTimestamp = dispatchManager.CurrentTimestamp;
                    {
                        var delta = currentTimestamp - previousTimestamp;
                        OnIdle(delta);
                    }
                    previousTimestamp = currentTimestamp;

                    dispatcher.DispatchPending();
                }
            }
            _state.TryTransition(from: Exiting, to: Stopped);
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
            _compositionHost.Value.TryGetExport(serviceType, out var service);
            return service;
        }
        #endregion
    }
}
