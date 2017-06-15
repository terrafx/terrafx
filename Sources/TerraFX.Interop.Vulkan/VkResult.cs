// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkResult
    {
        SUCCESS = 0,

        NOT_READY = 1,

        TIMEOUT = 2,

        EVENT_SET = 3,

        EVENT_RESET = 4,

        INCOMPLETE = 5,

        ERROR_OUT_OF_HOST_MEMORY = -1,

        ERROR_OUT_OF_DEVICE_MEMORY = -2,

        ERROR_INITIALIZATION_FAILED = -3,

        ERROR_DEVICE_LOST = -4,

        ERROR_MEMORY_MAP_FAILED = -5,

        ERROR_LAYER_NOT_PRESENT = -6,

        ERROR_EXTENSION_NOT_PRESENT = -7,

        ERROR_FEATURE_NOT_PRESENT = -8,

        ERROR_INCOMPATIBLE_DRIVER = -9,

        ERROR_TOO_MANY_OBJECTS = -10,

        ERROR_FORMAT_NOT_SUPPORTED = -11,

        ERROR_FRAGMENTED_POOL = -12,

        ERROR_SURFACE_LOST_KHR = -1000000000,

        ERROR_NATIVE_WINDOW_IN_USE_KHR = -1000000001,

        SUBOPTIMAL_KHR = 1000001003,

        ERROR_OUT_OF_DATE_KHR = -1000001004,

        ERROR_INCOMPATIBLE_DISPLAY_KHR = -1000003001,

        ERROR_VALIDATION_FAILED_EXT = -1000011001,

        ERROR_INVALID_SHADER_NV = -1000012000,

        ERROR_OUT_OF_POOL_MEMORY_KHR = -1000069000,

        ERROR_INVALID_EXTERNAL_HANDLE_KHX = -1000072003
    }
}
