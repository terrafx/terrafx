// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    unsafe public static partial class Vulkan
    {
        #region Constants
        public const uint VK_KHR_android_surface = 1;

        public const uint VK_KHR_ANDROID_SURFACE_SPEC_VERSION = 6;

        public const string VK_KHR_ANDROID_SURFACE_EXTENSION_NAME = "VK_KHR_android_surface";
        #endregion

        #region External Methods
        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateAndroidSurfaceKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateAndroidSurfaceKHR(
            [ComAliasName("VkInstance")] IntPtr instance,
            VkAndroidSurfaceCreateInfoKHR* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkSurfaceKHR")] IntPtr* pSurface
        );
        #endregion
    }
}
