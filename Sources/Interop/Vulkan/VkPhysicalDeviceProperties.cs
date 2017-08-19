// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct VkPhysicalDeviceProperties
    {
        #region Fields
        public uint apiVersion;

        public uint driverVersion;

        public uint vendorID;

        public uint deviceID;

        public VkPhysicalDeviceType deviceType;

        [ComAliasName("string")]
        public fixed sbyte deviceName[256];

        public fixed byte pipelineCacheUUID[16];

        public VkPhysicalDeviceLimits limits;

        public VkPhysicalDeviceSparseProperties sparseProperties;
        #endregion
    }
}
