// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    public static unsafe partial class Vulkan
    {
        #region Constants
        public const uint VK_KHR_win32_surface = 1;

        public const uint VK_KHR_WIN32_SURFACE_SPEC_VERSION = 6;

        public const string VK_KHR_WIN32_SURFACE_EXTENSION_NAME = "VK_KHR_win32_surface";
        #endregion

        #region External Methods
        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateWin32SurfaceKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateWin32SurfaceKHR(
            [ComAliasName("VkInstance")] IntPtr instance,
            VkWin32SurfaceCreateInfoKHR* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkSurfaceKHR")] IntPtr* pSurface
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceWin32PresentationSupportKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("VkBool32")]
        public static extern uint vkGetPhysicalDeviceWin32PresentationSupportKHR(
            [ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            uint queueFamilyIndex
        );
        #endregion
    }
}
