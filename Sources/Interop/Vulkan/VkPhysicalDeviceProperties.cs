// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    unsafe public struct VkPhysicalDeviceProperties
    {
        #region Fields
        public uint apiVersion;

        public uint driverVersion;

        public uint vendorID;

        public uint deviceID;

        public VkPhysicalDeviceType deviceType;

        public fixed byte deviceName[VK.MAX_PHYSICAL_DEVICE_NAME_SIZE];

        public fixed byte pipelineCacheUUID[VK.UUID_SIZE];

        public VkPhysicalDeviceLimits limits;

        public VkPhysicalDeviceSparseProperties sparseProperties;
        #endregion
    }
}
