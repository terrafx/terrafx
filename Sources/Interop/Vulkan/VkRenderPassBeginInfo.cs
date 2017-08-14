// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct VkRenderPassBeginInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [ComAliasName("VkRenderPass")]
        public ulong renderPass;

        [ComAliasName("VkFramebuffer")]
        public ulong framebuffer;

        public VkRect2D renderArea;

        public uint clearValueCount;

        public VkClearValue* pClearValues;
        #endregion
    }
}
