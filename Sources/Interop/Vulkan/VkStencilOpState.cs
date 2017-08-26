// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public /* blittable */ struct VkStencilOpState
    {
        #region Fields
        public VkStencilOp failOp;

        public VkStencilOp passOp;

        public VkStencilOp depthFailOp;

        public VkCompareOp compareOp;

        public uint compareMask;

        public uint writeMask;

        public uint reference;
        #endregion
    }
}
