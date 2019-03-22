// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct VkSubpassDescription
    {
        #region Fields
        [NativeTypeName("VkSubpassDescriptionFlags")]
        public uint flags;

        public VkPipelineBindPoint pipelineBindPoint;

        public uint inputAttachmentCount;

        [NativeTypeName("VkAttachmentReference[]")]
        public VkAttachmentReference* pInputAttachments;

        public uint colorAttachmentCount;

        [NativeTypeName("VkAttachmentReference[]")]
        public VkAttachmentReference* pColorAttachments;

        [NativeTypeName("VkAttachmentReference[]")]
        public VkAttachmentReference* pResolveAttachments;

        public VkAttachmentReference* pDepthStencilAttachment;

        public uint preserveAttachmentCount;

        [NativeTypeName("uint[]")]
        public uint* pPreserveAttachments;
        #endregion
    }
}
