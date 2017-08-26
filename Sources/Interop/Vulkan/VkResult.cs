// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkResult
    {
        VK_SUCCESS = 0,

        VK_NOT_READY = 1,

        VK_TIMEOUT = 2,

        VK_EVENT_SET = 3,

        VK_EVENT_RESET = 4,

        VK_INCOMPLETE = 5,

        VK_ERROR_OUT_OF_HOST_MEMORY = -1,

        VK_ERROR_OUT_OF_DEVICE_MEMORY = -2,

        VK_ERROR_INITIALIZATION_FAILED = -3,

        VK_ERROR_DEVICE_LOST = -4,

        VK_ERROR_MEMORY_MAP_FAILED = -5,

        VK_ERROR_LAYER_NOT_PRESENT = -6,

        VK_ERROR_EXTENSION_NOT_PRESENT = -7,

        VK_ERROR_FEATURE_NOT_PRESENT = -8,

        VK_ERROR_INCOMPATIBLE_DRIVER = -9,

        VK_ERROR_TOO_MANY_OBJECTS = -10,

        VK_ERROR_FORMAT_NOT_SUPPORTED = -11,

        VK_ERROR_FRAGMENTED_POOL = -12,

        VK_ERROR_SURFACE_LOST_KHR = -1000000000,

        VK_ERROR_NATIVE_WINDOW_IN_USE_KHR = -1000000001,

        VK_SUBOPTIMAL_KHR = 1000001003,

        VK_ERROR_OUT_OF_DATE_KHR = -1000001004,

        VK_ERROR_INCOMPATIBLE_DISPLAY_KHR = -1000003001,

        VK_ERROR_VALIDATION_FAILED_EXT = -1000011001,

        VK_ERROR_INVALID_SHADER_NV = -1000012000,

        VK_ERROR_OUT_OF_POOL_MEMORY_KHR = -1000069000,

        VK_ERROR_INVALID_EXTERNAL_HANDLE_KHX = -1000072003,

        VK_RESULT_BEGIN_RANGE = VK_ERROR_FRAGMENTED_POOL,

        VK_RESULT_END_RANGE = VK_INCOMPLETE,

        VK_RESULT_RANGE_SIZE = (VK_INCOMPLETE - VK_ERROR_FRAGMENTED_POOL + 1),

        VK_RESULT_MAX_ENUM = 0x7FFFFFFF
    }
}
