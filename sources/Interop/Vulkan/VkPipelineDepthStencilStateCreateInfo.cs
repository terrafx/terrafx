// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct VkPipelineDepthStencilStateCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [NativeTypeName("VkPipelineDepthStencilStateCreateFlags")]
        public uint flags;

        [NativeTypeName("VkBool32")]
        public uint depthTestEnable;

        [NativeTypeName("VkBool32")]
        public uint depthWriteEnable;

        public VkCompareOp depthCompareOp;

        [NativeTypeName("VkBool32")]
        public uint depthBoundsTestEnable;

        [NativeTypeName("VkBool32")]
        public uint stencilTestEnable;

        public VkStencilOpState front;

        public VkStencilOpState back;

        public float minDepthBounds;

        public float maxDepthBounds;
        #endregion
    }
}
