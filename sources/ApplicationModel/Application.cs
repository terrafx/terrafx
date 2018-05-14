// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Composition.Hosting;
using System.Reflection;
using System.Threading;
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
        private const int Stopped = 1;
        private const int Running = 2;
        private const int Exiting = 3;
        #endregion

        #region Fields
        private readonly Assembly[] _compositionAssemblies;
        private readonly Thread _parentThread;
        private readonly Lazy<CompositionHost> _compositionHost;
        private State _state;
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

            _state.Transition(to: Stopped);
        }
        #endregion

        #region Events
        /// <summary>Occurs when the event loop for the current instance becomes idle.</summary>
        public event EventHandler<ApplicationIdleEventArgs> Idle;
        #endregion

        #region Properties
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
        #endregion

        #region Methods
        private CompositionHost CreateCompositionHost()
        {
            _state.ThrowIfDisposedOrDisposing();

            var containerConfiguration = new ContainerConfiguration();
            {
                containerConfiguration = containerConfiguration.WithAssemblies(_compositionAssemblies);
            }
            return containerConfiguration.CreateContainer();
        }

        /// <summary>Gets the service object of the specified type.</summary>
        /// <typeparam name="TService">The type of the service object to get.</typeparam>
        /// <returns>A service object of <typeparamref name="TService" /> if one exists; otherwise, <c>default</c>.</returns>
        public TService GetService<TService>()
        {
            _compositionHost.Value.TryGetExport<TService>(out var service);
            return service;
        }

        private void OnIdle(TimeSpan delta)
        {
            var idle = Idle;

            if (idle != null)
            {
                var eventArgs = new ApplicationIdleEventArgs(delta);
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
                var dispatchManager = _compositionHost.Value.GetExport<IDispatchManager>();
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
            var priorState = _state.BeginDispose();

            if (priorState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                _compositionHost?.Dispose();
            }

            _state.EndDispose();
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
