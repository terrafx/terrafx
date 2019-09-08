// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Graphics;
using TerraFX.Interop;
using static TerraFX.Interop.VkQueueFlagBits;
using static TerraFX.Interop.VkResult;
using static TerraFX.Interop.VkStructureType;
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

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle => _physicalDevice;

        /// <summary>Gets the PCI ID of the vendor.</summary>
        public uint VendorId => _vendorId;

        /// <summary>Creates a new <see cref="IGraphicsDevice" /> for the instance.</summary>
        /// <returns>A new <see cref="IGraphicsDevice" /> for the instance.</returns>
        public IGraphicsDevice CreateDevice()
        {
            IntPtr device;
            IntPtr queue;

            uint queueFamilyPropertyCount;
            vkGetPhysicalDeviceQueueFamilyProperties(_physicalDevice, &queueFamilyPropertyCount, pQueueFamilyProperties: null);

            var queueFamilyProperties = stackalloc VkQueueFamilyProperties[(int)queueFamilyPropertyCount];
            vkGetPhysicalDeviceQueueFamilyProperties(_physicalDevice, &queueFamilyPropertyCount, queueFamilyProperties);

            uint? queueFamilyIndex = default;

            for (uint i = 0; i < queueFamilyPropertyCount; i++)
            {
                if ((queueFamilyProperties[i].queueFlags & (uint)VK_QUEUE_GRAPHICS_BIT) != 0)
                {
                    queueFamilyIndex = i;
                    break;
                }
            }

            if (!queueFamilyIndex.HasValue)
            {
                ThrowInvalidOperationException(nameof(queueFamilyIndex), queueFamilyIndex!);
            }

            var queueCreateInfo = new VkDeviceQueueCreateInfo {
                sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                queueFamilyIndex = queueFamilyIndex.GetValueOrDefault(),
                queueCount = 1,
            };

            var createInfo = new VkDeviceCreateInfo {
                sType = VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO,
                queueCreateInfoCount = 1,
                pQueueCreateInfos = &queueCreateInfo,
            };

            var result = vkCreateDevice(_physicalDevice, &createInfo, pAllocator: null, &device);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateDevice), (int)result);
            }

            vkGetDeviceQueue(device, queueFamilyIndex.GetValueOrDefault(), 0, &queue);

            return new GraphicsDevice(this, device, queue);
        }
    }
}
