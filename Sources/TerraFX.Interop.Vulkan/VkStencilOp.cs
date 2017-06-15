// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkStencilOp
    {
        KEEP = 0,

        ZERO = 1,

        REPLACE = 2,

        INCREMENT_AND_CLAMP = 3,

        DECREMENT_AND_CLAMP = 4,

        INVERT = 5,

        INCREMENT_AND_WRAP = 6,

        DECREMENT_AND_WRAP = 7
    }
}
