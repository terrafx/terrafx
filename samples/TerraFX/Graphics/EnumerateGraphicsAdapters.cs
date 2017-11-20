// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;
using TerraFX.ApplicationModel;

namespace TerraFX.Samples.Graphics
{
    public sealed class EnumerateGraphicsAdapters : Sample
    {
        #region Constructors
        public EnumerateGraphicsAdapters(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }
        #endregion

        #region Methods
        public override void OnIdle(object sender, IdleEventArgs eventArgs)
        {
            var application = (Application)(sender);
            {
                var graphicsManager = application.GraphicsManager;

                foreach (var graphicsAdapter in graphicsManager.GraphicsAdapters)
                {
                    Console.WriteLine($"    Name: {graphicsAdapter.DeviceName}");
                    Console.WriteLine($"        Device ID: {graphicsAdapter.DeviceId}");
                    Console.WriteLine($"        Vendor ID: {graphicsAdapter.VendorId}");
                }
            }
            application.RequestExit();
        }
        #endregion
    }
}
