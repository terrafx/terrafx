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
        public const uint VK_KHR_swapchain = 1;

        public const uint VK_KHR_SWAPCHAIN_SPEC_VERSION = 68;

        public const string VK_KHR_SWAPCHAIN_EXTENSION_NAME = "VK_KHR_swapchain";
        #endregion

        #region External Methods
        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateSwapchainKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateSwapchainKHR(
            [ComAliasName("VkDevice")] IntPtr device,
            VkSwapchainCreateInfoKHR* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkSwapchainKHR")] IntPtr* pSwapchain
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroySwapchainKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroySwapchainKHR(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkSwapchainKHR")] IntPtr swapchain,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetSwapchainImagesKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetSwapchainImagesKHR(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkSwapchainKHR")] IntPtr swapchain,
            uint* pSwapchainImageCount,
            [ComAliasName("VkImage")] IntPtr* pSwapchainImages
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAcquireNextImageKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkAcquireNextImageKHR(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkSwapchainKHR")] IntPtr swapchain,
            ulong timeout,
            [ComAliasName("VkSemaphore")] IntPtr semaphore,
            [ComAliasName("VkFence")] IntPtr fence,
            uint* pImageIndex
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueuePresentKHR", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkQueuePresentKHR(
            [ComAliasName("VkQueue")] IntPtr queue,
            VkPresentInfoKHR* pPresentInfo
        );
        #endregion
    }
}
