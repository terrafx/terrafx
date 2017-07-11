// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public /* blittable */ struct VkDrawIndirectCommand
    {
        #region Fields
        public uint vertexCount;

        public uint instanceCount;

        public uint firstVertex;

        public uint firstInstance;
        #endregion
    }
}
