// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using TerraFX.Graphics;
using TerraFX.Threading;
using TerraFX.UI;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.ApplicationModel;

/// <summary>A multimedia-based application.</summary>
public sealed class Application : IDisposable, INameable
{
    private GraphicsService _graphicsService;
    private readonly UIService _uiService;

    private bool _isRunning;

    private readonly Thread _parentThread;

    private string _name;
    private VolatileState _state;

    /// <summary>Initializes a new instance of the <see cref="Application" /> class.</summary>
    public Application()
    {
        _graphicsService = new GraphicsService();
        _parentThread = Thread.CurrentThread;
        _uiService = UIService.Instance;

        _name = GetType().Name;
        _ = _state.Transition(VolatileState.Initialized);
    }

    /// <summary>Occurs when the event loop for the current instance becomes idle.</summary>
    public event EventHandler<ApplicationIdleEventArgs>? Idle;

    /// <summary>Gets the graphics service for the instance.</summary>
    public GraphicsService GraphicsService => _graphicsService;

    /// <summary>Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.</summary>
    public bool IsDisposed => _state.IsDisposedOrDisposing;

    /// <summary>Gets a value that indicates whether the event loop for the instance is running.</summary>
    public bool IsRunning => _isRunning;

    /// <inheritdoc />
    [AllowNull]
    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value ?? GetType().Name;
        }
    }

    /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
    public Thread ParentThread => _parentThread;

    /// <summary>Gets the UI service for the instance.</summary>
    public UIService UIService => _uiService;

    /// <inheritdoc />
    public void Dispose()
    {
        _ = _state.BeginDispose();
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        _state.EndDispose();
    }

    /// <summary>Requests that the instance exits the event loop.</summary>
    /// <remarks>
    ///   <para>This method does nothing if <see cref="IsRunning" /> is <c>false</c>.</para>
    ///   <para>This method can be called from any thread.</para>
    /// </remarks>
    public void RequestExit() => _isRunning = false;

    /// <summary>Runs the event loop for the instance.</summary>
    /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
    /// <exception cref="ObjectDisposedException">The application has been disposed.</exception>
    /// <remarks>This method does nothing if an exit is requested before the first iteration of the event loop has started.</remarks>
    public void Run()
    {
        ThrowIfDisposedOrDisposing(_state, _name);
        ThrowIfNotThread(_parentThread);

        RunUnsafe();
    }

    /// <inheritdoc />
    public override string ToString() => _name;

    private void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _graphicsService.Dispose();
            _graphicsService = null!;
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

    private void RunUnsafe()
    {
        var uiService = _uiService;

        var dispatcher = uiService.DispatcherForCurrentThread;
        var previousTimestamp = uiService.CurrentTimestamp;

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

        _isRunning = true;

        do
        {
            var currentTimestamp = uiService.CurrentTimestamp;
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
        while (IsRunning);

        dispatcher.ExitRequested -= OnDispatcherExitRequested;
    }
}
