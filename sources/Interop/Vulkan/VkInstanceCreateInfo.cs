// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct VkInstanceCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [NativeTypeName("VkInstanceCreateFlags")]
        public uint flags;

        public VkApplicationInfo* pApplicationInfo;

        public uint enabledLayerCount;

        [NativeTypeName("string[]")]
        public sbyte** ppEnabledLayerNames;

        public uint enabledExtensionCount;

        [NativeTypeName("string[]")]
        public sbyte** ppEnabledExtensionNames;
        #endregion
    }
}
