// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Graphics;
using TerraFX.Interop;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;

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

        /// <summary>Gets the physical device for the instance.</summary>
        public IntPtr PhysicalDevice => _physicalDevice;

        /// <summary>Gets the PCI ID of the vendor.</summary>
        public uint VendorId => _vendorId;

        /// <summary>Creates a new <see cref="IGraphicsContext" />.</summary>
        /// <param name="graphicsSurface">The <see cref="IGraphicsSurface" /> on which the graphics context can draw.</param>
        /// <returns>A new <see cref="IGraphicsContext" /> which utilizes the current instance and which can draw on <paramref name="graphicsSurface" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsSurface" /> is <c>null</c>.</exception>
        public IGraphicsContext CreateGraphicsContext(IGraphicsSurface graphicsSurface)
        {
            ThrowIfNull(graphicsSurface, nameof(graphicsSurface));
            return new GraphicsContext(this, graphicsSurface);
        }
    }
}
