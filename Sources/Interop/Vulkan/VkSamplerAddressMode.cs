// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkSamplerAddressMode
    {
        REPEAT = 0,

        MIRRORED_REPEAT = 1,

        CLAMP_TO_EDGE = 2,

        CLAMP_TO_BORDER = 3,

        MIRROR_CLAMP_TO_EDGE = 4
    }
}
