// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkDescriptorType
    {
        VK_DESCRIPTOR_TYPE_SAMPLER = 0,

        VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER = 1,

        VK_DESCRIPTOR_TYPE_SAMPLED_IMAGE = 2,

        VK_DESCRIPTOR_TYPE_STORAGE_IMAGE = 3,

        VK_DESCRIPTOR_TYPE_UNIFORM_TEXEL_BUFFER = 4,

        VK_DESCRIPTOR_TYPE_STORAGE_TEXEL_BUFFER = 5,

        VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER = 6,

        VK_DESCRIPTOR_TYPE_STORAGE_BUFFER = 7,

        VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER_DYNAMIC = 8,

        VK_DESCRIPTOR_TYPE_STORAGE_BUFFER_DYNAMIC = 9,

        VK_DESCRIPTOR_TYPE_INPUT_ATTACHMENT = 10,

        VK_DESCRIPTOR_TYPE_BEGIN_RANGE = VK_DESCRIPTOR_TYPE_SAMPLER,

        VK_DESCRIPTOR_TYPE_END_RANGE = VK_DESCRIPTOR_TYPE_INPUT_ATTACHMENT,

        VK_DESCRIPTOR_TYPE_RANGE_SIZE = (VK_DESCRIPTOR_TYPE_INPUT_ATTACHMENT - VK_DESCRIPTOR_TYPE_SAMPLER + 1),

        VK_DESCRIPTOR_TYPE_MAX_ENUM = 0x7FFFFFFF
    }
}
