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
        #region Fields
        /// <summary>The <see cref="GraphicsProvider" /> for the instance.</summary>
        private readonly GraphicsProvider _graphicsProvider;

        /// <summary>The Vulkan device for the instance.</summary>
        private readonly IntPtr _physicalDevice;

        /// <summary>The name of the device.</summary>
        private readonly string _deviceName;

        /// <summary>The PCI ID of the vendor.</summary>
        private readonly uint _vendorId;

        /// <summary>The PCI ID of the device.</summary>
        private readonly uint _deviceId;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="GraphicsAdapter" /> class.</summary>
        /// <param name="graphicsProvider">The <see cref="GraphicsProvider" /> for the instance.</param>
        /// <param name="physicalDevice">The Vulkan device for the instance.</param>
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
        #endregion

        #region TerraFX.Graphics.IGraphicsAdapter Properties
        /// <summary>Gets the PCI ID of the device.</summary>
        public uint DeviceId
        {
            get
            {
                return _deviceId;
            }
        }

        /// <summary>Gets the name of the device.</summary>
        public string DeviceName
        {
            get
            {
                return _deviceName;
            }
        }

        /// <summary>Gets the <see cref="IGraphicsProvider" /> for the instance.</summary>
        public IGraphicsProvider GraphicsProvider
        {
            get
            {
                return _graphicsProvider;
            }
        }

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle
        {
            get
            {
                return _physicalDevice;
            }
        }

        /// <summary>Gets the PCI ID of the vendor.</summary>
        public uint VendorId
        {
            get
            {
                return _vendorId;
            }
        }
        #endregion
    }
}
