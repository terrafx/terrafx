// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public struct VkSubpassDependency
    {
        #region Fields
        public uint srcSubpass;

        public uint dstSubpass;

        [NativeTypeName("VkPipelineStageFlags")]
        public uint srcStageMask;

        [NativeTypeName("VkPipelineStageFlags")]
        public uint dstStageMask;

        [NativeTypeName("VkAccessFlags")]
        public uint srcAccessMask;

        [NativeTypeName("VkAccessFlags")]
        public uint dstAccessMask;

        [NativeTypeName("VkDependencyFlags")]
        public uint dependencyFlags;
        #endregion
    }
}
