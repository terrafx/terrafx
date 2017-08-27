// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Graphics;
using TerraFX.Interop;
using static TerraFX.Interop.Vulkan;

namespace TerraFX.Provider.Vulkan.Graphics
{
    /// <summary>Represents a graphics adapter.</summary>
    unsafe public sealed class GraphicsAdapter : IGraphicsAdapter
    {
        #region Fields
        internal readonly GraphicsManager _graphicsManager;

        internal IntPtr _physicalDevice;

        internal string _deviceName;

        internal uint _vendorId;

        internal uint _deviceId;
        #endregion

        #region Constructors
        internal GraphicsAdapter(GraphicsManager graphicsManager, IntPtr physicalDevice)
        {
            _graphicsManager = graphicsManager;
            _physicalDevice = physicalDevice;

            VkPhysicalDeviceProperties properties;
            vkGetPhysicalDeviceProperties(physicalDevice, &properties);

            _deviceName = Marshal.PtrToStringAnsi((IntPtr)(properties.deviceName));
            _vendorId = properties.vendorID;
            _deviceId = properties.deviceID;
        }
        #endregion

        #region TerraFX.Graphics.IGraphicsAdapter Properties
        /// <summary>Gets the name of the device.</summary>
        public string DeviceName { get; }

        /// <summary>Gets the PCI ID of the vendor.</summary>
        public uint VendorId { get; }

        /// <summary>Gets the PCI ID of the device.</summary>
        public uint DeviceId { get; }
        #endregion
    }
}
