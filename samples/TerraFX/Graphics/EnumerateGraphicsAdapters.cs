// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Utilities;

namespace TerraFX.Samples.Graphics;

public sealed class EnumerateGraphicsAdapters(string name) : Sample(name)
{
    protected override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
    {
        ExceptionUtilities.ThrowIfNull(sender);

        var application = (Application)sender;
        {
            var graphicsService = application.GraphicsService;

            foreach (var graphicsAdapter in graphicsService.Adapters)
            {
                Console.WriteLine($"    Name: {graphicsAdapter.Name}");
                Console.WriteLine($"        Device ID: {graphicsAdapter.PciDeviceId}");
                Console.WriteLine($"        Vendor ID: {graphicsAdapter.PciVendorId}");
            }
        }
        application.RequestExit();
    }
}
