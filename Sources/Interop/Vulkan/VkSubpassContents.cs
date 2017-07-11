// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkSubpassContents
    {
        VK_SUBPASS_CONTENTS_INLINE = 0,

        VK_SUBPASS_CONTENTS_SECONDARY_COMMAND_BUFFERS = 1,

        VK_SUBPASS_CONTENTS_BEGIN_RANGE = VK_SUBPASS_CONTENTS_INLINE,

        VK_SUBPASS_CONTENTS_END_RANGE = VK_SUBPASS_CONTENTS_SECONDARY_COMMAND_BUFFERS,

        VK_SUBPASS_CONTENTS_RANGE_SIZE = (VK_SUBPASS_CONTENTS_SECONDARY_COMMAND_BUFFERS - VK_SUBPASS_CONTENTS_INLINE + 1),

        VK_SUBPASS_CONTENTS_MAX_ENUM = 0x7FFFFFFF
    }
}
