// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Linq;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using TerraFX.UI;
using TerraFX.Utilities;

namespace TerraFX.Samples.Graphics;

public class HelloWindow : Sample
{
    private GraphicsDevice _graphicsDevice = null!;
    private GraphicsSwapchain _graphicsSwapchain = null!;
    private Window _window = null!;
    private TimeSpan _elapsedTime;
    private uint _secondsOfLastFpsUpdate;

    public HelloWindow(string name, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
    }

    public GraphicsDevice GraphicsDevice => _graphicsDevice;

    public GraphicsSwapchain GraphicsSwapchain => _graphicsSwapchain;

    public Window Window => _window;

    public override void Cleanup()
    {
        _graphicsSwapchain?.Dispose();
        _graphicsDevice?.Dispose();
        _window?.Dispose();

        base.Cleanup();
    }

    /// <summary>Initializes the GUI for this sample.</summary>
    /// <param name="application">The hosting <see cref="Application" />.</param>
    /// <param name="timeout">The <see cref="TimeSpan" /> after which this sample should stop running.</param>
    public override void Initialize(Application application, TimeSpan timeout) => Initialize(application, timeout, null, null);

    /// <summary>Initializes the GUI for this sample.</summary>
    /// <param name="application">The hosting <see cref="Application" />.</param>
    /// <param name="timeout">The <see cref="TimeSpan" /> after which this sample should stop running.</param>
    /// <param name="windowLocation">The <see cref="Vector2" /> that defines the initial window location.</param>
    /// <param name="windowSize">The <see cref="Vector2" /> that defines the initial window client rectangle size.</param>
    public virtual void Initialize(Application application, TimeSpan timeout, Vector2? windowLocation, Vector2? windowSize)
    {
        ExceptionUtilities.ThrowIfNull(application);

        var windowService = application.ServiceProvider.WindowService;

        _window = windowService.CreateWindow();
        _window.SetTitle(Name);
        if (windowLocation.HasValue)
        {
            _window.Relocate(windowLocation.GetValueOrDefault());
        }
        if (windowSize.HasValue)
        {
            _window.ResizeClient(windowSize.GetValueOrDefault());
        }
        _window.Show();

        var graphicsService = application.ServiceProvider.GraphicsService;
        var graphicsAdapter = graphicsService.Adapters.First();

        var graphicsDevice = graphicsAdapter.CreateDevice();
        _graphicsDevice = graphicsDevice;

        _graphicsSwapchain = graphicsDevice.CreateSwapchain(_window);
        base.Initialize(application, timeout);
    }

    protected virtual void Draw(GraphicsContext graphicsContext) { }

    protected virtual void Update(TimeSpan delta) { }

    protected override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
    {
        ExceptionUtilities.ThrowIfNull(sender);

        _elapsedTime += eventArgs.Delta;

        if (_elapsedTime >= Timeout)
        {
            var application = (Application)sender;
            application.RequestExit();
        }

        if (_window.IsVisible)
        {
            // add current fps to the end of the Window Title
            var seconds = (uint)_elapsedTime.TotalSeconds;
            if (_secondsOfLastFpsUpdate < seconds)
            {
                var newTitle = $"{Name} ({eventArgs.FramesPerSecond} fps)";
                Window.SetTitle(newTitle);
                _secondsOfLastFpsUpdate = seconds;
            }

            // update the rendering
            var graphicsSwapchain = GraphicsSwapchain;
            var currentGraphicsContext = GraphicsDevice.Contexts[(int)graphicsSwapchain.FramebufferIndex];
            currentGraphicsContext.BeginFrame(graphicsSwapchain);

            Update(eventArgs.Delta);
            Render();

            currentGraphicsContext.EndFrame();
            Present();
        }
    }

    protected void Present() => GraphicsSwapchain.Present();

    protected void Render()
    {
        var graphicsSwapchain = GraphicsSwapchain;
        var framebufferIndex = graphicsSwapchain.FramebufferIndex;
        var graphicsContext = GraphicsDevice.Contexts[(int)framebufferIndex];

        graphicsContext.BeginDrawing(framebufferIndex, Colors.CornflowerBlue);
        Draw(graphicsContext);
        graphicsContext.EndDrawing();
    }
}
