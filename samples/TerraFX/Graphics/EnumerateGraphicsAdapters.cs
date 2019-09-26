// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Utilities;

namespace TerraFX.Samples.Graphics
{
    public sealed class EnumerateGraphicsAdapters : Sample
    {
        public EnumerateGraphicsAdapters(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        protected override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
        {
            ExceptionUtilities.ThrowIfNull(sender, nameof(sender));

            var application = (Application)sender;
            {
                var graphicsProvider = application.GetService<IGraphicsProvider>();

                foreach (var graphicsAdapter in graphicsProvider.GraphicsAdapters)
                {
                    Console.WriteLine($"    Name: {graphicsAdapter.DeviceName}");
                    Console.WriteLine($"        Device ID: {graphicsAdapter.DeviceId}");
                    Console.WriteLine($"        Vendor ID: {graphicsAdapter.VendorId}");
                }
            }
            application.RequestExit();
        }
    }
}
