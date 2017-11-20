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
        public const uint VK_KHR_swapchain = 1;

        public const uint VK_KHR_SWAPCHAIN_SPEC_VERSION = 68;

        public const string VK_KHR_SWAPCHAIN_EXTENSION_NAME = "VK_KHR_swapchain";
        #endregion

        #region External Methods
        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateSwapchainKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateSwapchainKHR(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkSwapchainCreateInfoKHR* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkSwapchainKHR")] IntPtr* pSwapchain
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroySwapchainKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroySwapchainKHR(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkSwapchainKHR")] IntPtr swapchain,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetSwapchainImagesKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetSwapchainImagesKHR(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkSwapchainKHR")] IntPtr swapchain,
            [In, Out] uint* pSwapchainImageCount,
            [Out, ComAliasName("VkImage[]")] IntPtr* pSwapchainImages
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAcquireNextImageKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkAcquireNextImageKHR(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkSwapchainKHR")] IntPtr swapchain,
            [In] ulong timeout,
            [In, ComAliasName("VkSemaphore")] IntPtr semaphore,
            [In, ComAliasName("VkFence")] IntPtr fence,
            [Out] uint* pImageIndex
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueuePresentKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkQueuePresentKHR(
            [In, ComAliasName("VkQueue")] IntPtr queue,
            [In] VkPresentInfoKHR* pPresentInfo
        );
        #endregion
    }
}
