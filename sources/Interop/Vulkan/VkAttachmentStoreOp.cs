// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkAttachmentStoreOp
    {
        VK_ATTACHMENT_STORE_OP_STORE = 0,

        VK_ATTACHMENT_STORE_OP_DONT_CARE = 1,

        VK_ATTACHMENT_STORE_OP_BEGIN_RANGE = VK_ATTACHMENT_STORE_OP_STORE,

        VK_ATTACHMENT_STORE_OP_END_RANGE = VK_ATTACHMENT_STORE_OP_DONT_CARE,

        VK_ATTACHMENT_STORE_OP_RANGE_SIZE = VK_ATTACHMENT_STORE_OP_DONT_CARE - VK_ATTACHMENT_STORE_OP_STORE + 1,

        VK_ATTACHMENT_STORE_OP_MAX_ENUM = 0x7FFFFFFF
    }
}
