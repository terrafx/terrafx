// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct VkDeviceCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [ComAliasName("VkDeviceCreateFlags")]
        public uint flags;

        public uint queueCreateInfoCount;

        [ComAliasName("VkDeviceQueueCreateInfo[]")]
        public VkDeviceQueueCreateInfo* pQueueCreateInfos;

        public uint enabledLayerCount;

        [ComAliasName("string[]")]
        public sbyte** ppEnabledLayerNames;

        public uint enabledExtensionCount;

        [ComAliasName("string[]")]
        public sbyte** ppEnabledExtensionNames;

        public VkPhysicalDeviceFeatures* pEnabledFeatures;
        #endregion
    }
}
