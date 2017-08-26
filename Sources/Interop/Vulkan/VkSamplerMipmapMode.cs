// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkSamplerMipmapMode
    {
        VK_SAMPLER_MIPMAP_MODE_NEAREST = 0,

        VK_SAMPLER_MIPMAP_MODE_LINEAR = 1,

        VK_SAMPLER_MIPMAP_MODE_BEGIN_RANGE = VK_SAMPLER_MIPMAP_MODE_NEAREST,

        VK_SAMPLER_MIPMAP_MODE_END_RANGE = VK_SAMPLER_MIPMAP_MODE_LINEAR,

        VK_SAMPLER_MIPMAP_MODE_RANGE_SIZE = (VK_SAMPLER_MIPMAP_MODE_LINEAR - VK_SAMPLER_MIPMAP_MODE_NEAREST + 1),

        VK_SAMPLER_MIPMAP_MODE_MAX_ENUM = 0x7FFFFFFF
    }
}
