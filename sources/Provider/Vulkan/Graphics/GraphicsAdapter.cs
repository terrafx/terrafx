// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Graphics;
using TerraFX.Interop;
using static TerraFX.Interop.Vulkan;

namespace TerraFX.Provider.Vulkan.Graphics
{
    /// <summary>Represents a graphics adapter.</summary>
    public sealed unsafe class GraphicsAdapter : IGraphicsAdapter
    {
        private readonly GraphicsProvider _graphicsProvider;
        private readonly IntPtr _physicalDevice;
        private readonly string _deviceName;
        private readonly uint _vendorId;
        private readonly uint _deviceId;

        internal GraphicsAdapter(GraphicsProvider graphicsProvider, IntPtr physicalDevice)
        {
            _graphicsProvider = graphicsProvider;
            _physicalDevice = physicalDevice;

            VkPhysicalDeviceProperties properties;
            vkGetPhysicalDeviceProperties(physicalDevice, &properties);

            _deviceName = Marshal.PtrToStringAnsi((IntPtr)properties.deviceName)!;
            _vendorId = properties.vendorID;
            _deviceId = properties.deviceID;
        }

        /// <summary>Gets the PCI ID of the device.</summary>
        public uint DeviceId => _deviceId;

        /// <summary>Gets the name of the device.</summary>
        public string DeviceName => _deviceName;

        /// <summary>Gets the <see cref="IGraphicsProvider" /> for the instance.</summary>
        public IGraphicsProvider GraphicsProvider => _graphicsProvider;

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle => _physicalDevice;

        /// <summary>Gets the PCI ID of the vendor.</summary>
        public uint VendorId => _vendorId;
    }
}
