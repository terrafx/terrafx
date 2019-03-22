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
        public const uint VK_KHR_swapchain = 1;

        public const uint VK_KHR_SWAPCHAIN_SPEC_VERSION = 68;

        public const string VK_KHR_SWAPCHAIN_EXTENSION_NAME = "VK_KHR_swapchain";
        #endregion

        #region External Methods
        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateSwapchainKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateSwapchainKHR(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkSwapchainCreateInfoKHR* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkSwapchainKHR")] IntPtr* pSwapchain
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroySwapchainKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroySwapchainKHR(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkSwapchainKHR")] IntPtr swapchain,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetSwapchainImagesKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetSwapchainImagesKHR(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkSwapchainKHR")] IntPtr swapchain,
            [In, Out] uint* pSwapchainImageCount,
            [Out, NativeTypeName("VkImage[]")] IntPtr* pSwapchainImages
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAcquireNextImageKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkAcquireNextImageKHR(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkSwapchainKHR")] IntPtr swapchain,
            [In] ulong timeout,
            [In, NativeTypeName("VkSemaphore")] IntPtr semaphore,
            [In, NativeTypeName("VkFence")] IntPtr fence,
            [Out] uint* pImageIndex
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueuePresentKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkQueuePresentKHR(
            [In, NativeTypeName("VkQueue")] IntPtr queue,
            [In] VkPresentInfoKHR* pPresentInfo
        );
        #endregion
    }
}
