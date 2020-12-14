// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Linq;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Graphics.Geometry2D;
using TerraFX.UI;
using TerraFX.Utilities;

namespace TerraFX.Samples.Graphics
{
    public class HelloWindow : Sample
    {
        private GraphicsDevice _graphicsDevice = null!;
        private Window _window = null!;
        private TimeSpan _elapsedTime;

        public HelloWindow(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public GraphicsDevice GraphicsDevice => _graphicsDevice;

        public Window Window => _window;

        public override void Cleanup()
        {
            _graphicsDevice?.Dispose();
            _window?.Dispose();

            base.Cleanup();
        }

        /// <summary>Initializes the GUI for this sample.</summary>
        /// <param name="application">The hosting <see cref="Application" />.</param>
        /// <param name="timeout">The <see cref="TimeSpan" /> after which this sample should stop running.</param>
        public override void Initialize(Application application, TimeSpan timeout) => Initialize(application, timeout, windowBounds: null);

        /// <summary>Initializes the GUI for this sample.</summary>
        /// <param name="application">The hosting <see cref="Application" />.</param>
        /// <param name="timeout">The <see cref="TimeSpan" /> after which this sample should stop running.</param>
        /// <param name="windowBounds">The <see cref="Rectangle" /> that defines the initial window bounds.
        /// Note that it is a mix of outer window location and inner client rect size.</param>
        public virtual void Initialize(Application application, TimeSpan timeout, Rectangle? windowBounds = null)
        {
            ExceptionUtilities.ThrowIfNull(application, nameof(application));

            var windowProvider = application.GetService<WindowProvider>();

            _window = windowProvider.CreateWindow();
            _window.SetTitle(Name);
            if (windowBounds.HasValue)
            {
                if (windowBounds.Value.Location != default)
                {
                    _window.Relocate(windowBounds.Value.Location);
                }
                if (windowBounds.Value.Size != default)
                {
                    _window.ResizeClient(windowBounds.Value.Size);
                }
            }
            _window.Show();

            var graphicsProvider = application.GetService<GraphicsProvider>();
            var graphicsAdapter = graphicsProvider.Adapters.First();

            _graphicsDevice = graphicsAdapter.CreateDevice(_window, contextCount: 2);

            base.Initialize(application, timeout);
        }

        protected virtual void Draw(GraphicsContext graphicsContext) { }

        protected virtual void Update(TimeSpan delta) { }

        protected override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
        {
            ExceptionUtilities.ThrowIfNull(sender, nameof(sender));

            _elapsedTime += eventArgs.Delta;

            if (_elapsedTime >= Timeout)
            {
                var application = (Application)sender;
                application.RequestExit();
            }

            if (_window.IsVisible)
            {
                var currentGraphicsContext = _graphicsDevice.CurrentContext;
                currentGraphicsContext.BeginFrame();

                Update(eventArgs.Delta);
                Render();

                currentGraphicsContext.EndFrame();
                Present();
            }
        }

        protected void Present() => _graphicsDevice.PresentFrame();

        protected void Render()
        {
            var graphicsContext = GraphicsDevice.CurrentContext;
            var backgroundColor = new ColorRgba(red: 100.0f / 255.0f, green: 149.0f / 255.0f, blue: 237.0f / 255.0f, alpha: 1.0f);

            graphicsContext.BeginDrawing(backgroundColor);
            Draw(graphicsContext);
            graphicsContext.EndDrawing();
        }
    }
}
