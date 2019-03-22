// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public struct VkSparseImageMemoryRequirements
    {
        #region Fields
        public VkSparseImageFormatProperties formatProperties;

        public uint imageMipTailFirstLod;

        [ComAliasName("VkDeviceSize")]
        public ulong imageMipTailSize;

        [ComAliasName("VkDeviceSize")]
        public ulong imageMipTailOffset;

        [ComAliasName("VkDeviceSize")]
        public ulong imageMipTailStride;
        #endregion
    }
}
