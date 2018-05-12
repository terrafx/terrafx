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
        /// <summary>The <see cref="GraphicsManager" /> for the instance.</summary>
        private readonly GraphicsManager _graphicsManager;

        /// <summary>The Vulkan device for the instance.</summary>
        private IntPtr _physicalDevice;

        /// <summary>The name of the device.</summary>
        private string _deviceName;

        /// <summary>The PCI ID of the vendor.</summary>
        private uint _vendorId;

        /// <summary>The PCI ID of the device.</summary>
        private uint _deviceId;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="GraphicsAdapter" /> class.</summary>
        /// <param name="graphicsManager">The <see cref="GraphicsManager" /> for the instance.</param>
        /// <param name="physicalDevice">The Vulkan device for the instance.</param>
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

        /// <summary>Gets the <see cref="IGraphicsManager" /> for the instance.</summary>
        public IGraphicsManager GraphicsManager
        {
            get
            {
                return _graphicsManager;
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
