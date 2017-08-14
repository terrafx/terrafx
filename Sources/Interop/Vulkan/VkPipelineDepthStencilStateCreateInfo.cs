// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct VkPipelineDepthStencilStateCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [ComAliasName("VkPipelineDepthStencilStateCreateFlags")]
        public uint flags;

        [ComAliasName("VkBool32")]
        public uint depthTestEnable;

        [ComAliasName("VkBool32")]
        public uint depthWriteEnable;

        public VkCompareOp depthCompareOp;

        [ComAliasName("VkBool32")]
        public uint depthBoundsTestEnable;

        [ComAliasName("VkBool32")]
        public uint stencilTestEnable;

        public VkStencilOpState front;

        public VkStencilOpState back;

        public float minDepthBounds;

        public float maxDepthBounds;
        #endregion
    }
}
