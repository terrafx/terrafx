// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public static unsafe partial class Vulkan
    {
        #region Constants
        public const uint VK_KHR_display = 1;

        public const uint VK_KHR_DISPLAY_SPEC_VERSION = 21;

        public const string VK_KHR_DISPLAY_EXTENSION_NAME = "VK_KHR_display";
        #endregion

        #region External Methods
        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceDisplayPropertiesKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetPhysicalDeviceDisplayPropertiesKHR(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, Out] uint* pPropertyCount,
            [Out, Optional, NativeTypeName("VkDisplayPropertiesKHR[]")] VkDisplayPropertiesKHR* pProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceDisplayPlanePropertiesKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetPhysicalDeviceDisplayPlanePropertiesKHR(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, Out] uint* pPropertyCount,
            [Out, Optional, NativeTypeName("VkDisplayPlanePropertiesKHR[]")] VkDisplayPlanePropertiesKHR* pProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDisplayPlaneSupportedDisplaysKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetDisplayPlaneSupportedDisplaysKHR(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In] uint planeIndex,
            [In, Out] uint* pDisplayCount,
            [Out, Optional, NativeTypeName("VkDisplayKHR[]")] IntPtr* pDisplays
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDisplayModePropertiesKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetDisplayModePropertiesKHR(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, NativeTypeName("VkDisplayKHR")] IntPtr display,
            [In, Out] uint* pPropertyCount,
            [Out, Optional, NativeTypeName("VkDisplayModePropertiesKHR[]")] VkDisplayModePropertiesKHR* pProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDisplayModeKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateDisplayModeKHR(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, NativeTypeName("VkDisplayKHR")] IntPtr display,
            [In] VkDisplayModeCreateInfoKHR* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkDisplayModeKHR")] IntPtr* pMode
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDisplayPlaneCapabilitiesKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetDisplayPlaneCapabilitiesKHR(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, NativeTypeName("VkDisplayModeKHR")] IntPtr mode,
            [In] uint planeIndex,
            [Out] VkDisplayPlaneCapabilitiesKHR* pCapabilities
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDisplayPlaneSurfaceKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateDisplayPlaneSurfaceKHR(
            [In, NativeTypeName("VkInstance")] IntPtr instance,
            [In] VkDisplaySurfaceCreateInfoKHR* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkSurfaceKHR")] IntPtr* pSurface
        );
        #endregion
    }
}
