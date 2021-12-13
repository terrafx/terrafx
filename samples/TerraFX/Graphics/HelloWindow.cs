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
    private GraphicsRenderPass _graphicsRenderPass = null!;
    private Window _window = null!;
    private TimeSpan _elapsedTime;
    private uint _secondsOfLastFpsUpdate;

    public HelloWindow(string name, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
    }

    public GraphicsDevice GraphicsDevice => _graphicsDevice;

    public GraphicsRenderPass GraphicsRenderPass => _graphicsRenderPass;

    public Window Window => _window;

    public override void Cleanup()
    {
        _graphicsRenderPass?.Dispose();
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

        _graphicsRenderPass = graphicsDevice.CreateRenderPass(_window, GraphicsFormat.R8G8B8A8_UNORM);
        base.Initialize(application, timeout);
    }

    protected virtual void Draw(GraphicsRenderContext graphicsRenderContext) { }

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

            Update(eventArgs.Delta);
            Render();
            Present();
        }
    }

    protected void Present() => GraphicsRenderPass.Swapchain.Present();

    protected void Render()
    {
        var graphicsRenderContext = GraphicsDevice.RentRenderContext();
        {
            graphicsRenderContext.Reset();
            graphicsRenderContext.BeginRenderPass(GraphicsRenderPass, Colors.CornflowerBlue);
            {
                var surfaceSize = GraphicsRenderPass.Surface.Size;

                var viewport = BoundingBox.CreateFromSize(Vector3.Zero, Vector3.Create(surfaceSize, 1.0f));
                graphicsRenderContext.SetViewport(viewport);

                var scissor = BoundingRectangle.CreateFromSize(Vector2.Zero, surfaceSize);
                graphicsRenderContext.SetScissor(scissor);

                Draw(graphicsRenderContext);
            }
            graphicsRenderContext.EndRenderPass();
            graphicsRenderContext.Flush();
        }
        GraphicsDevice.ReturnRenderContext(graphicsRenderContext);
    }
}
