// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ struct VkSubpassDependency
    {
        #region Fields
        public uint srcSubpass;

        public uint dstSubpass;

        [ComAliasName("VkPipelineStageFlags")]
        public uint srcStageMask;

        [ComAliasName("VkPipelineStageFlags")]
        public uint dstStageMask;

        [ComAliasName("VkAccessFlags")]
        public uint srcAccessMask;

        [ComAliasName("VkAccessFlags")]
        public uint dstAccessMask;

        [ComAliasName("VkDependencyFlags")]
        public uint dependencyFlags;
        #endregion
    }
}
