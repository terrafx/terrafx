// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Interop.VkColorSpaceKHR;

namespace TerraFX.Interop
{
    public static unsafe partial class Vulkan
    {
        #region Constants
        public const uint VK_KHR_surface = 1;

        public const uint VK_KHR_SURFACE_SPEC_VERSION = 25;

        public const string VK_KHR_SURFACE_EXTENSION_NAME = "VK_KHR_surface";

        public const VkColorSpaceKHR VK_COLORSPACE_SRGB_NONLINEAR_KHR = VK_COLOR_SPACE_SRGB_NONLINEAR_KHR;
        #endregion

        #region External Methods
        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroySurfaceKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroySurfaceKHR(
            [In, ComAliasName("VkInstance")] IntPtr instance,
            [In, ComAliasName("VkSurfaceKHR")] IntPtr surface,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceSurfaceSupportKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetPhysicalDeviceSurfaceSupportKHR(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In] uint queueFamilyIndex,
            [In, ComAliasName("VkSurfaceKHR")] IntPtr surface,
            [Out, ComAliasName("VkBool32")] uint* pSupported
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceSurfaceCapabilitiesKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetPhysicalDeviceSurfaceCapabilitiesKHR(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, ComAliasName("VkSurfaceKHR")] IntPtr surface,
            [Out] VkSurfaceCapabilitiesKHR* pSurfaceCapabilities
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceSurfaceFormatsKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetPhysicalDeviceSurfaceFormatsKHR(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, ComAliasName("VkSurfaceKHR")] IntPtr surface,
            [In, Out] uint* pSurfaceFormatCount,
            [Out, Optional, ComAliasName("VkSurfaceFormatKHR[]")] VkSurfaceFormatKHR* pSurfaceFormats
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceSurfacePresentModesKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetPhysicalDeviceSurfacePresentModesKHR(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, ComAliasName("VkSurfaceKHR")] IntPtr surface,
            [In, Out] uint* pPresentModeCount,
            [Out, Optional, ComAliasName("VkPresentModeKHR[]")] VkPresentModeKHR* pPresentModes
        );
        #endregion
    }
}
