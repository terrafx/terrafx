// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkObjectType
    {
        UNKNOWN = 0,

        INSTANCE = 1,

        PHYSICAL_DEVICE = 2,

        DEVICE = 3,

        QUEUE = 4,

        SEMAPHORE = 5,

        COMMAND_BUFFER = 6,

        FENCE = 7,

        DEVICE_MEMORY = 8,

        BUFFER = 9,

        IMAGE = 10,

        EVENT = 11,

        QUERY_POOL = 12,

        BUFFER_VIEW = 13,

        IMAGE_VIEW = 14,

        SHADER_MODULE = 15,

        PIPELINE_CACHE = 16,

        PIPELINE_LAYOUT = 17,

        RENDER_PASS = 18,

        PIPELINE = 19,

        DESCRIPTOR_SET_LAYOUT = 20,

        SAMPLER = 21,

        DESCRIPTOR_POOL = 22,

        DESCRIPTOR_SET = 23,

        FRAMEBUFFER = 24,

        COMMAND_POOL = 25,

        SURFACE_KHR = 1000000000,

        SWAPCHAIN_KHR = 1000001000,

        DISPLAY_KHR = 1000002000,

        DISPLAY_MODE_KHR = 1000002001,

        DEBUG_REPORT_CALLBACK_EXT = 1000011000,

        DESCRIPTOR_UPDATE_TEMPLATE_KHR = 1000085000,

        OBJECT_TABLE_NVX = 1000086000,

        INDIRECT_COMMANDS_LAYOUT_NVX = 1000086001
    }
}
