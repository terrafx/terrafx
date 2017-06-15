// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkDescriptorType
    {
        SAMPLER = 0,

        COMBINED_IMAGE_SAMPLER = 1,

        SAMPLED_IMAGE = 2,

        STORAGE_IMAGE = 3,

        UNIFORM_TEXEL_BUFFER = 4,

        STORAGE_TEXEL_BUFFER = 5,

        UNIFORM_BUFFER = 6,

        STORAGE_BUFFER = 7,

        UNIFORM_BUFFER_DYNAMIC = 8,

        STORAGE_BUFFER_DYNAMIC = 9,

        INPUT_ATTACHMENT = 10
    }
}
