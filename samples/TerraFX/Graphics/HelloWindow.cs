// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Linq;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.UI;
using TerraFX.Utilities;

namespace TerraFX.Samples.Graphics
{
    public sealed class HelloWindow : Sample
    {
        private GraphicsContext _graphicsContext = null!;
        private Window _window = null!;
        private TimeSpan _elapsedTime;

        public HelloWindow(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void Cleanup()
        {
            if (_graphicsContext is IDisposable graphicsContext)
            {
                graphicsContext.Dispose();
            }

            if (_window is IDisposable window)
            {
                window.Dispose();
            }

            base.Cleanup();
        }

        public override void Initialize(Application application)
        {
            ExceptionUtilities.ThrowIfNull(application, nameof(application));

            var windowProvider = application.GetService<WindowProvider>();
            _window = windowProvider.CreateWindow();
            _window.Show();

            var graphicsProvider = application.GetService<GraphicsProvider>();
            var graphicsAdapter = graphicsProvider.GraphicsAdapters.First();

            var graphicsSurface = _window.CreateGraphicsSurface(bufferCount: 2);
            _graphicsContext = graphicsAdapter.CreateGraphicsContext(graphicsSurface);

            base.Initialize(application);
        }

        protected override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
        {
            ExceptionUtilities.ThrowIfNull(sender, nameof(sender));

            _elapsedTime += eventArgs.Delta;

            if (_elapsedTime.TotalSeconds >= 2.5)
            {
                var application = (Application)sender;
                application.RequestExit();
            }

            if (_window.IsVisible)
            {
                var backgroundColor = new ColorRgba(red: 100.0f / 255.0f, green: 149.0f / 255.0f, blue: 237.0f / 255.0f, alpha: 1.0f);
                _graphicsContext.BeginFrame(backgroundColor);

                _graphicsContext.EndFrame();
                _graphicsContext.PresentFrame();
            }
        }
    }
}
