// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Composition.Hosting;
using System.Reflection;
using System.Threading;
using TerraFX.Threading;
using TerraFX.UI;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.ApplicationModel
{
    /// <summary>A multimedia-based application.</summary>
    public sealed class Application : IDisposable, IServiceProvider
    {
        private const int Stopped = 1;
        private const int Running = 2;
        private const int Exiting = 3;

        private readonly Assembly[] _compositionAssemblies;
        private readonly Thread _parentThread;
        private ValueLazy<CompositionHost> _compositionHost;

        private VolatileState _state;

        /// <summary>Initializes a new instance of the <see cref="Application" /> class.</summary>
        /// <param name="compositionAssemblies">The <see cref="Assembly" /> instances to search for type exports.</param>
        /// <exception cref="ArgumentNullException"><paramref name="compositionAssemblies" /> is <c>null</c>.</exception>
        public Application(params Assembly[] compositionAssemblies)
        {
            ThrowIfNull(compositionAssemblies, nameof(compositionAssemblies));

            _compositionAssemblies = compositionAssemblies;
            _parentThread = Thread.CurrentThread;
            _compositionHost = new ValueLazy<CompositionHost>(CreateCompositionHost);

            _ = _state.Transition(to: Stopped);
        }

        /// <summary>Occurs when the event loop for the current instance becomes idle.</summary>
        public event EventHandler<ApplicationIdleEventArgs>? Idle;

        /// <summary>Gets a value that indicates whether the event loop for the instance is running.</summary>
        public bool IsRunning => _state == Running;

        /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
        public Thread ParentThread => _parentThread;

        /// <inheritdoc />
        public TService GetService<TService>()
        {
            _ = _compositionHost.Value.TryGetExport<TService>(out var service);
            return service;
        }

        /// <summary>Requests that the instance exits the event loop.</summary>
        /// <remarks>
        ///   <para>This method does nothing if <see cref="IsRunning" /> is <c>false</c>.</para>
        ///   <para>This method can be called from any thread.</para>
        /// </remarks>
        public void RequestExit() => _ = _state.TryTransition(from: Running, to: Exiting);

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
                var windowProvider = _compositionHost.Value.GetExport<WindowProvider>();

                var dispatchProvider = windowProvider.DispatchProvider;
                var dispatcher = dispatchProvider.DispatcherForCurrentThread;
                var previousTimestamp = dispatchProvider.CurrentTimestamp;

                var previousFrameCount = 0u;
                var framesPerSecond = 0u;
                var framesThisSecond = 0u;

                var secondCounter = TimeSpan.Zero;

                // We need to do an initial dispatch to cover the case where a quit
                // message was posted before the message pump was started, otherwise
                // we can end up with a NullReferenceException when we try to execute
                // OnIdle.

                dispatcher.ExitRequested += OnDispatcherExitRequested;
                dispatcher.DispatchPending();

                while (_state == Running)
                {
                    var currentTimestamp = dispatchProvider.CurrentTimestamp;
                    var frameCount = previousFrameCount++;
                    {
                        var delta = currentTimestamp - previousTimestamp;
                        secondCounter += delta;

                        OnIdle(delta, framesPerSecond);
                        framesThisSecond++;

                        if (secondCounter.TotalSeconds >= 1.0)
                        {
                            framesPerSecond = framesThisSecond;
                            framesThisSecond = 0;

                            var ticks = secondCounter.Ticks - TimeSpan.TicksPerSecond;
                            secondCounter = TimeSpan.FromTicks(ticks);
                        }
                    }
                    previousFrameCount = frameCount;
                    previousTimestamp = currentTimestamp;

                    dispatcher.DispatchPending();
                }

                dispatcher.ExitRequested -= OnDispatcherExitRequested;
            }
            _ = _state.TryTransition(from: Exiting, to: Stopped);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeCompositionHost(isDisposing: true);
            }

            _state.EndDispose();
        }

        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="serviceType">The type of the service object to get.</param>
        /// <returns>A service object of <paramref name="serviceType" /> if one exists; otherwise, <c>null</c>.</returns>
        public object GetService(Type serviceType)
        {
            _ = _compositionHost.Value.TryGetExport(serviceType, out var service);
            return service;
        }

        private CompositionHost CreateCompositionHost()
        {
            ThrowIfDisposedOrDisposing(_state, nameof(Application));

            var containerConfiguration = new ContainerConfiguration();
            {
                containerConfiguration = containerConfiguration.WithAssemblies(_compositionAssemblies);
            }
            return containerConfiguration.CreateContainer();
        }

        private void DisposeCompositionHost(bool isDisposing)
        {
            AssertDisposing(_state);

            if (isDisposing && _compositionHost.IsValueCreated)
            {
                _compositionHost.Value.Dispose();
            }
        }

        private void OnDispatcherExitRequested(object? sender, EventArgs e) => RequestExit();

        private void OnIdle(TimeSpan delta, uint framesPerSecond)
        {
            var idle = Idle;

            if (idle != null)
            {
                var eventArgs = new ApplicationIdleEventArgs(delta, framesPerSecond);
                idle(this, eventArgs);
            }
        }
    }
}
