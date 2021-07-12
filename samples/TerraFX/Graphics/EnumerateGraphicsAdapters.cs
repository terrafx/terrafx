// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Utilities;

namespace TerraFX.Samples.Graphics
{
    public sealed class EnumerateGraphicsAdapters : Sample
    {
        public EnumerateGraphicsAdapters(string name, ApplicationServiceProvider serviceProvider)
            : base(name, serviceProvider)
        {
        }

        protected override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
        {
            ExceptionUtilities.ThrowIfNull(sender, nameof(sender));

            var application = (Application)sender;
            {
                var graphicsService = application.ServiceProvider.GraphicsService;

                foreach (var graphicsAdapter in graphicsService.Adapters)
                {
                    Console.WriteLine($"    Name: {graphicsAdapter.Name}");
                    Console.WriteLine($"        Device ID: {graphicsAdapter.DeviceId}");
                    Console.WriteLine($"        Vendor ID: {graphicsAdapter.VendorId}");
                }
            }
            application.RequestExit();
        }
    }
}
