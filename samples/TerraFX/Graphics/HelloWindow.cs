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
        private IGraphicsDevice? _graphicsDevice;
        private ISwapChain? _swapChain;
        private IWindow? _window;
        private TimeSpan _elapsedTime;

        public HelloWindow(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
        {
            ExceptionUtilities.ThrowIfNull(sender, nameof(sender));

            var application = (Application)sender;

            if (_window is null)
            {
                var windowProvider = application.GetService<IWindowProvider>();
                _window = windowProvider.CreateWindow();

                _window.Show();
            }
            else if (_window.IsVisible)
            {
                if (_graphicsDevice is null)
                {
                    var graphicsProvider = application.GetService<IGraphicsProvider>();

                    var graphicsAdapter = graphicsProvider.GraphicsAdapters.First();
                    _graphicsDevice = graphicsAdapter.CreateDevice();

                    var graphicsSurface = _window.CreateGraphicsSurface(bufferCount: 2);
                    _swapChain = _graphicsDevice.CreateSwapChain(graphicsSurface);
                }
                else
                {
                    _elapsedTime += eventArgs.Delta;

                    if (_elapsedTime.TotalSeconds >= 2.5)
                    {
                        application.RequestExit();
                    }
                }
            }
        }
    }
}
