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
    private GraphicsRenderPass _renderPass = null!;
    private UIWindow _window = null!;
    private TimeSpan _elapsedTime;
    private uint _secondsOfLastFpsUpdate;

    public HelloWindow(string name) : base(name)
    {
    }

    public GraphicsDevice GraphicsDevice => _graphicsDevice;

    public GraphicsRenderPass RenderPass => _renderPass;

    public UIWindow Window => _window;

    public override void Cleanup()
    {
        _renderPass?.Dispose();
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

        var uiService = application.UIService;

        _window = uiService.DispatcherForCurrentThread.CreateWindow();
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

        var graphicsService = application.GraphicsService;
        var graphicsAdapter = graphicsService.Adapters.First();

        var graphicsDevice = graphicsAdapter.CreateDevice();
        _graphicsDevice = graphicsDevice;

        _renderPass = graphicsDevice.CreateRenderPass(_window, GraphicsFormat.B8G8R8A8_UNORM);
        base.Initialize(application, timeout);
    }

    protected virtual void Draw(GraphicsRenderContext renderContext) { }

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

    protected void Present() => RenderPass.Swapchain.Present();

    protected void Render()
    {
        var renderCommandQueue = GraphicsDevice.RenderCommandQueue;
        var renderContext = renderCommandQueue.RentContext();
        {
            renderContext.Reset();
            {
                renderContext.BeginRenderPass(RenderPass, Colors.CornflowerBlue);
                {
                    var surfaceSize = RenderPass.Surface.Size;

                    var viewport = BoundingBox.CreateFromSize(Vector3.Zero, Vector3.Create(surfaceSize, 1.0f));
                    renderContext.SetViewport(viewport);

                    var scissor = BoundingRectangle.CreateFromSize(Vector2.Zero, surfaceSize);
                    renderContext.SetScissor(scissor);

                    Draw(renderContext);
                }
                renderContext.EndRenderPass();
            }
            renderContext.Close();
            renderContext.Execute();
        }
        renderCommandQueue.ReturnContext(renderContext);
    }
}
