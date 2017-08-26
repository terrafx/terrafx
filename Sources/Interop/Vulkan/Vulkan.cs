// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

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
        public const int VK_VERSION_1_0 = 1;

        public const int VK_HEADER_VERSION = 54;

        public const int VK_NULL_HANDLE = 0;

        public const float VK_LOD_CLAMP_NONE = 1000.0f;

        public const uint VK_REMAINING_MIP_LEVELS = ~0u;

        public const uint VK_REMAINING_ARRAY_LAYERS = ~0u;

        public const ulong VK_WHOLE_SIZE = ~0ul;

        public const uint VK_ATTACHMENT_UNUSED = ~0u;

        public const uint VK_TRUE = 1;

        public const uint VK_FALSE = 0;

        public const uint VK_QUEUE_FAMILY_IGNORED = ~0u;

        public const uint VK_SUBPASS_EXTERNAL = ~0u;

        public const int VK_MAX_PHYSICAL_DEVICE_NAME_SIZE = 256;

        public const int VK_UUID_SIZE = 16;

        public const int VK_MAX_MEMORY_TYPES = 32;

        public const int VK_MAX_MEMORY_HEAPS = 16;

        public const int VK_MAX_EXTENSION_NAME_SIZE = 256;

        public const int VK_MAX_DESCRIPTION_SIZE = 256;
        #endregion

        #region Static Fields
        public static readonly uint VK_API_VERSION_1_0 = VK_MAKE_VERSION(1, 0, 0);
        #endregion

        #region Static Methods
        public static uint VK_MAKE_VERSION(uint major, uint minor, uint patch)
        {
            return ((major & 0x03FF) << 22) | ((minor & 0x03FF) << 12) | (patch & 0x0FFF);
        }

        public static uint VK_VERSION_MAJOR(uint version)
        {
            return version >> 22;
        }

        public static uint VK_VERSION_MINOR(uint version)
        {
            return (version >> 12) & 0x03FF;
        }

        public static uint VK_VERSION_PATCH(uint version)
        {
            return version & 0x0FFF;
        }
        #endregion

        #region External Methods
        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateInstance", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateInstance(
            VkInstanceCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkInstance")] IntPtr* pInstance
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyInstance", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyInstance(
            [ComAliasName("VkInstance")] IntPtr instance,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumeratePhysicalDevices", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumeratePhysicalDevices(
            [ComAliasName("VkInstance")] IntPtr instance,
            uint* pPhysicalDeviceCount,
            [ComAliasName("VkPhysicalDevice")] IntPtr* pPhysicalDevices
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceFeatures", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceFeatures(
            [ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            VkPhysicalDeviceFeatures* pFeatures
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceFormatProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceFormatProperties(
            [ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            VkFormat format,
            VkFormatProperties* pFormatProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceImageFormatProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetPhysicalDeviceImageFormatProperties(
            [ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            VkFormat format,
            VkImageType type,
            VkImageTiling tiling,
            [ComAliasName("VkImageUsageFlags")] uint usage,
            [ComAliasName("VkImageCreateFlags")] uint flags,
            VkImageFormatProperties* pImageFormatProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceProperties(
            [ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            VkPhysicalDeviceProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceQueueFamilyProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceQueueFamilyProperties(
            [ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            uint* pQueueFamilyPropertyCount,
            VkQueueFamilyProperties* pQueueFamilyProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceMemoryProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceMemoryProperties(
            [ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            VkPhysicalDeviceMemoryProperties* pMemoryProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetInstanceProcAddr", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern PFN_vkVoidFunction vkGetInstanceProcAddr(
            [ComAliasName("VkInstance")] IntPtr instance,
            sbyte* pName
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDeviceProcAddr", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern PFN_vkVoidFunction vkGetDeviceProcAddr(
            [ComAliasName("VkDevice")] IntPtr device,
            sbyte* pName
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDevice", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateDevice(
            [ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            VkDeviceCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkDevice")] IntPtr* pDevice
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyDevice", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyDevice(
            [ComAliasName("VkDevice")] IntPtr device,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateInstanceExtensionProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumerateInstanceExtensionProperties(
            sbyte* pLayerName,
            uint* pPropertyCount,
            VkExtensionProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateDeviceExtensionProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumerateDeviceExtensionProperties(
            [ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            sbyte* pLayerName,
            uint* pPropertyCount,
            VkExtensionProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateInstanceLayerProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumerateInstanceLayerProperties(
            uint* pPropertyCount,
            VkLayerProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateDeviceLayerProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumerateDeviceLayerProperties(
            [ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            uint* pPropertyCount,
            VkLayerProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDeviceQueue", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetDeviceQueue(
            [ComAliasName("VkDevice")] IntPtr device,
            uint queueFamilyIndex,
            uint queueIndex,
            [ComAliasName("VkQueue")] IntPtr* pQueue
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueueSubmit", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkQueueSubmit(
            [ComAliasName("VkQueue")] IntPtr queue,
            uint submitCount,
            VkSubmitInfo* pSubmits,
            [ComAliasName("VkFence")] ulong fence
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueueWaitIdle", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkQueueWaitIdle(
            [ComAliasName("VkQueue")] IntPtr queue
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDeviceWaitIdle", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkDeviceWaitIdle(
            [ComAliasName("VkDevice")] IntPtr device
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAllocateMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkAllocateMemory(
            [ComAliasName("VkDevice")] IntPtr device,
            VkMemoryAllocateInfo* pAllocateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkDeviceMemory")] ulong* pMemory
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFreeMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkFreeMemory(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkDeviceMemory")] ulong memory,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkMapMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkMapMemory(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkDeviceMemory")] ulong memory,
            [ComAliasName("VkDeviceSize")] ulong offset,
            [ComAliasName("VkDeviceSize")] ulong size,
            [ComAliasName("VkMemoryMapFlags")] uint flags,
            void** ppData
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkUnmapMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkUnmapMemory(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkDeviceMemory")] ulong memory
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFlushMappedMemoryRanges", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkFlushMappedMemoryRanges(
            [ComAliasName("VkDevice")] IntPtr device,
            uint memoryRangeCount,
            VkMappedMemoryRange* pMemoryRanges
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkInvalidateMappedMemoryRanges", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkInvalidateMappedMemoryRanges(
            [ComAliasName("VkDevice")] IntPtr device,
            uint memoryRangeCount,
            VkMappedMemoryRange* pMemoryRanges
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDeviceMemoryCommitment", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetDeviceMemoryCommitment(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkDeviceMemory")] ulong memory,
            [ComAliasName("VkDeviceSize")] ulong* pCommittedMemoryInBytes
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkBindBufferMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkBindBufferMemory(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkBuffer")] ulong buffer,
            [ComAliasName("VkDeviceMemory")] ulong memory,
            [ComAliasName("VkDeviceSize")] ulong memoryOffset
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkBindImageMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkBindImageMemory(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkImage")] ulong image,
            [ComAliasName("VkDeviceMemory")] ulong memory,
            [ComAliasName("VkDeviceSize")] ulong memoryOffset
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetBufferMemoryRequirements", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetBufferMemoryRequirements(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkBuffer")] ulong buffer,
            VkMemoryRequirements* pMemoryRequirements
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetImageMemoryRequirements", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetImageMemoryRequirements(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkImage")] ulong image,
            VkMemoryRequirements* pMemoryRequirements
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetImageSparseMemoryRequirements", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetImageSparseMemoryRequirements(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkImage")] ulong image,
            uint* pSparseMemoryRequirementCount,
            VkSparseImageMemoryRequirements* pSparseMemoryRequirements
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceSparseImageFormatProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceSparseImageFormatProperties(
            [ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            VkFormat format,
            VkImageType type,
            VkSampleCountFlagBits samples,
            [ComAliasName("VkImageUsageFlags")] uint usage,
            VkImageTiling tiling,
            uint* pPropertyCount,
            VkSparseImageFormatProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueueBindSparse", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkQueueBindSparse(
            [ComAliasName("VkQueue")] IntPtr queue,
            uint bindInfoCount,
            VkBindSparseInfo* pBindInfo,
            [ComAliasName("VkFence")] ulong fence
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateFence", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateFence(
            [ComAliasName("VkDevice")] IntPtr device,
            VkFenceCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkFence")] ulong* pFence
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyFence", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyFence(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkFence")] ulong fence,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetFences", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetFences(
            [ComAliasName("VkDevice")] IntPtr device,
            uint fenceCount,
            [ComAliasName("VkFence")] ulong* pFences
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetFenceStatus", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetFenceStatus(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkFence")] ulong fence
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkWaitForFences", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkWaitForFences(
            [ComAliasName("VkDevice")] IntPtr device,
            uint fenceCount,
            [ComAliasName("VkFence")] ulong* pFences,
            [ComAliasName("VkBool32")] uint waitAll,
            ulong timeout
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateSemaphore", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateSemaphore(
            [ComAliasName("VkDevice")] IntPtr device,
            VkSemaphoreCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkSemaphore")] ulong* pSemaphore
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroySemaphore", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroySemaphore(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkSemaphore")] ulong semaphore,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateEvent(
            [ComAliasName("VkDevice")] IntPtr device,
            VkEventCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkEvent")] ulong* pEvent
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyEvent(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkEvent")] ulong @event,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetEventStatus", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetEventStatus(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkEvent")] ulong @event
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkSetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkSetEvent(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkEvent")] ulong @event
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetEvent(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkEvent")] ulong @event
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateQueryPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateQueryPool(
            [ComAliasName("VkDevice")] IntPtr device,
            VkQueryPoolCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkQueryPool")] ulong* pQueryPool
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyQueryPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyQueryPool(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkQueryPool")] ulong queryPool,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetQueryPoolResults", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetQueryPoolResults(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkQueryPool")] ulong queryPool,
            uint firstQuery,
            uint queryCount,
            nuint dataSize,
            void* pData,
            [ComAliasName("VkDeviceSize")] ulong stride,
            [ComAliasName("VkQueryResultFlags")] uint flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateBuffer(
            [ComAliasName("VkDevice")] IntPtr device,
            VkBufferCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkBuffer")] ulong* pBuffer
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyBuffer(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkBuffer")] ulong buffer,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateBufferView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateBufferView(
            [ComAliasName("VkDevice")] IntPtr device,
            VkBufferViewCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkBufferView")] ulong* pView
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyBufferView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyBufferView(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkBufferView")] ulong bufferView,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateImage(
            [ComAliasName("VkDevice")] IntPtr device,
            VkImageCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkImage")] ulong* pImage
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyImage(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkImage")] ulong image,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetImageSubresourceLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetImageSubresourceLayout(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkImage")] ulong image,
            VkImageSubresource* pSubresource,
            VkSubresourceLayout* pLayout
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateImageView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateImageView(
            [ComAliasName("VkDevice")] IntPtr device,
            VkImageViewCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkImageView")] ulong* pView
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyImageView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyImageView(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkImageView")] ulong imageView,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateShaderModule", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateShaderModule(
            [ComAliasName("VkDevice")] IntPtr device,
            VkShaderModuleCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkShaderModule")] ulong* pShaderModule
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyShaderModule", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyShaderModule(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkShaderModule")] ulong shaderModule,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreatePipelineCache", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreatePipelineCache(
            [ComAliasName("VkDevice")] IntPtr device,
            VkPipelineCacheCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkPipelineCache")] ulong* pPipelineCache
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyPipelineCache", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyPipelineCache(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkPipelineCache")] ulong pipelineCache,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPipelineCacheData", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetPipelineCacheData(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkPipelineCache")] ulong pipelineCache,
            nuint* pDataSize,
            void* pData
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkMergePipelineCaches", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkMergePipelineCaches(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkPipelineCache")] ulong dstCache,
            uint srcCacheCount,
            [ComAliasName("VkPipelineCache")] ulong* pSrcCaches
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateGraphicsPipelines", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateGraphicsPipelines(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkPipelineCache")] ulong pipelineCache,
            uint createInfoCount,
            VkGraphicsPipelineCreateInfo* pCreateInfos,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkPipeline")] ulong* pPipelines
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateComputePipelines", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateComputePipelines(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkPipelineCache")] ulong pipelineCache,
            uint createInfoCount,
            VkComputePipelineCreateInfo* pCreateInfos,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkPipeline")] ulong* pPipelines
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyPipeline", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyPipeline(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkPipeline")] ulong pipeline,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreatePipelineLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreatePipelineLayout(
            [ComAliasName("VkDevice")] IntPtr device,
            VkPipelineLayoutCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkPipelineLayout")] ulong* pPipelineLayout
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyPipelineLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyPipelineLayout(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkPipelineLayout")] ulong pipelineLayout,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateSampler", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateSampler(
            [ComAliasName("VkDevice")] IntPtr device,
            VkSamplerCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkSampler")] ulong* pSampler
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroySampler", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroySampler(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkSampler")] ulong sampler,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDescriptorSetLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateDescriptorSetLayout(
            [ComAliasName("VkDevice")] IntPtr device,
            VkDescriptorSetLayoutCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkDescriptorSetLayout")] ulong* pSetLayout
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyDescriptorSetLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyDescriptorSetLayout(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkDescriptorSetLayout")] ulong descriptorSetLayout,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDescriptorPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateDescriptorPool(
            [ComAliasName("VkDevice")] IntPtr device,
            VkDescriptorPoolCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkDescriptorPool")] ulong* pDescriptorPool
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyDescriptorPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyDescriptorPool(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkDescriptorPool")] ulong descriptorPool,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetDescriptorPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetDescriptorPool(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkDescriptorPool")] ulong descriptorPool,
            [ComAliasName("VkDescriptorPoolResetFlags")] uint flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAllocateDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkAllocateDescriptorSets(
            [ComAliasName("VkDevice")] IntPtr device,
            VkDescriptorSetAllocateInfo* pAllocateInfo,
            [ComAliasName("VkDescriptorSet")] ulong* pDescriptorSets
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFreeDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkFreeDescriptorSets(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkDescriptorPool")] ulong descriptorPool,
            uint descriptorSetCount,
            [ComAliasName("VkDescriptorSet")] ulong* pDescriptorSets
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkUpdateDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkUpdateDescriptorSets(
            [ComAliasName("VkDevice")] IntPtr device,
            uint descriptorWriteCount,
            VkWriteDescriptorSet* pDescriptorWrites,
            uint descriptorCopyCount,
            VkCopyDescriptorSet* pDescriptorCopies
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateFramebuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateFramebuffer(
            [ComAliasName("VkDevice")] IntPtr device,
            VkFramebufferCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkFramebuffer")] ulong* pFramebuffer
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyFramebuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyFramebuffer(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkFramebuffer")] ulong framebuffer,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateRenderPass(
            [ComAliasName("VkDevice")] IntPtr device,
            VkRenderPassCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkRenderPass")] ulong* pRenderPass
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyRenderPass(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkRenderPass")] ulong renderPass,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetRenderAreaGranularity", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetRenderAreaGranularity(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkRenderPass")] ulong renderPass,
            VkExtent2D* pGranularity
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateCommandPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateCommandPool(
            [ComAliasName("VkDevice")] IntPtr device,
            VkCommandPoolCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            [ComAliasName("VkCommandPool")] ulong* pCommandPool
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyCommandPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyCommandPool(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkCommandPool")] ulong commandPool,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetCommandPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetCommandPool(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkCommandPool")] ulong commandPool,
            [ComAliasName("VkCommandPoolResetFlags")] uint flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAllocateCommandBuffers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkAllocateCommandBuffers(
            [ComAliasName("VkDevice")] IntPtr device,
            VkCommandBufferAllocateInfo* pAllocateInfo,
            [ComAliasName("VkCommandBuffer")] IntPtr* pCommandBuffers
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFreeCommandBuffers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkFreeCommandBuffers(
            [ComAliasName("VkDevice")] IntPtr device,
            [ComAliasName("VkCommandPool")] ulong commandPool,
            uint commandBufferCount,
            [ComAliasName("VkCommandBuffer")] IntPtr* pCommandBuffers
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkBeginCommandBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkBeginCommandBuffer(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            VkCommandBufferBeginInfo* pBeginInfo
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEndCommandBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEndCommandBuffer(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetCommandBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetCommandBuffer(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkCommandBufferResetFlags")] uint flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindPipeline", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBindPipeline(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            VkPipelineBindPoint pipelineBindPoint,
            [ComAliasName("VkPipeline")] ulong pipeline
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetViewport", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetViewport(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            uint firstViewport,
            uint viewportCount,
            VkViewport* pViewports
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetScissor", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetScissor(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            uint firstScissor,
            uint scissorCount,
            VkRect2D* pScissors
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetLineWidth", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetLineWidth(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            float lineWidth
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetDepthBias", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetDepthBias(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            float depthBiasConstantFactor,
            float depthBiasClamp,
            float depthBiasSlopeFactor
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetBlendConstants", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetBlendConstants(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            float* blendConstants
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetDepthBounds", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetDepthBounds(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            float minDepthBounds,
            float maxDepthBounds
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetStencilCompareMask", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetStencilCompareMask(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkStencilFaceFlags")] uint faceMask,
            uint compareMask
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetStencilWriteMask", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetStencilWriteMask(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkStencilFaceFlags")] uint faceMask,
            uint writeMask
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetStencilReference", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetStencilReference(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkStencilFaceFlags")] uint faceMask,
            uint reference
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBindDescriptorSets(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            VkPipelineBindPoint pipelineBindPoint,
            [ComAliasName("VkPipelineLayout")] ulong layout,
            uint firstSet,
            uint descriptorSetCount,
            [ComAliasName("VkDescriptorSet")] ulong* pDescriptorSets,
            uint dynamicOffsetCount,
            uint* pDynamicOffsets
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindIndexBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBindIndexBuffer(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkBuffer")] ulong buffer,
            [ComAliasName("VkDeviceSize")] ulong offset,
            VkIndexType indexType
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindVertexBuffers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBindVertexBuffers(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            uint firstBinding,
            uint bindingCount,
            [ComAliasName("VkBuffer")] ulong* pBuffers,
            [ComAliasName("VkDeviceSize")] ulong* pOffsets
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDraw", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDraw(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            uint vertexCount,
            uint instanceCount,
            uint firstVertex,
            uint firstInstance
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDrawIndexed", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDrawIndexed(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            uint indexCount,
            uint instanceCount,
            uint firstIndex,
            int vertexOffset,
            uint firstInstance
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDrawIndirect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDrawIndirect(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkBuffer")] ulong buffer,
            [ComAliasName("VkDeviceSize")] ulong offset,
            uint drawCount,
            uint stride
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDrawIndexedIndirect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDrawIndexedIndirect(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkBuffer")] ulong buffer,
            [ComAliasName("VkDeviceSize")] ulong offset,
            uint drawCount,
            uint stride
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDispatch", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDispatch(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            uint groupCountX,
            uint groupCountY,
            uint groupCountZ
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDispatchIndirect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDispatchIndirect(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkBuffer")] ulong buffer,
            [ComAliasName("VkDeviceSize")] ulong offset
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyBuffer(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkBuffer")] ulong srcBuffer,
            [ComAliasName("VkBuffer")] ulong dstBuffer,
            uint regionCount,
            VkBufferCopy* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyImage(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkImage")] ulong srcImage,
            VkImageLayout srcImageLayout,
            [ComAliasName("VkImage")] ulong dstImage,
            VkImageLayout dstImageLayout,
            uint regionCount,
            VkImageCopy* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBlitImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBlitImage(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkImage")] ulong srcImage,
            VkImageLayout srcImageLayout,
            [ComAliasName("VkImage")] ulong dstImage,
            VkImageLayout dstImageLayout,
            uint regionCount,
            VkImageBlit* pRegions,
            VkFilter filter
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyBufferToImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyBufferToImage(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkBuffer")] ulong srcBuffer,
            [ComAliasName("VkImage")] ulong dstImage,
            VkImageLayout dstImageLayout,
            uint regionCount,
            VkBufferImageCopy* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyImageToBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyImageToBuffer(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkImage")] ulong srcImage,
            VkImageLayout srcImageLayout,
            [ComAliasName("VkBuffer")] ulong dstBuffer,
            uint regionCount,
            VkBufferImageCopy* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdUpdateBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdUpdateBuffer(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkBuffer")] ulong dstBuffer,
            [ComAliasName("VkDeviceSize")] ulong dstOffset,
            [ComAliasName("VkDeviceSize")] ulong dataSize,
            void* pData
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdFillBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdFillBuffer(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkBuffer")] ulong dstBuffer,
            [ComAliasName("VkDeviceSize")] ulong dstOffset,
            [ComAliasName("VkDeviceSize")] ulong size,
            uint data
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdClearColorImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdClearColorImage(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkImage")] ulong image,
            VkImageLayout imageLayout,
            VkClearColorValue* pColor,
            uint rangeCount,
            VkImageSubresourceRange* pRanges
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdClearDepthStencilImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdClearDepthStencilImage(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkImage")] ulong image,
            VkImageLayout imageLayout,
            VkClearDepthStencilValue* pDepthStencil,
            uint rangeCount,
            VkImageSubresourceRange* pRanges
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdClearAttachments", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdClearAttachments(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            uint attachmentCount,
            VkClearAttachment* pAttachments,
            uint rectCount,
            VkClearRect* pRects
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdResolveImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdResolveImage(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkImage")] ulong srcImage,
            VkImageLayout srcImageLayout,
            [ComAliasName("VkImage")] ulong dstImage,
            VkImageLayout dstImageLayout,
            uint regionCount,
            VkImageResolve* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetEvent(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkEvent")] ulong @event,
            [ComAliasName("VkPipelineStageFlags")] uint stageMask
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdResetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdResetEvent(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkEvent")] ulong @event,
            [ComAliasName("VkPipelineStageFlags")] uint stageMask
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdWaitEvents", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdWaitEvents(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            uint eventCount,
            [ComAliasName("VkEvent")] ulong* pEvents,
            [ComAliasName("VkPipelineStageFlags")] uint srcStageMask,
            [ComAliasName("VkPipelineStageFlags")] uint dstStageMask,
            uint memoryBarrierCount,
            VkMemoryBarrier* pMemoryBarriers,
            uint bufferMemoryBarrierCount,
            VkBufferMemoryBarrier* pBufferMemoryBarriers,
            uint imageMemoryBarrierCount,
            VkImageMemoryBarrier* pImageMemoryBarriers
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdPipelineBarrier", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdPipelineBarrier(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkPipelineStageFlags")] uint srcStageMask,
            [ComAliasName("VkPipelineStageFlags")] uint dstStageMask,
            [ComAliasName("VkDependencyFlags")] uint dependencyFlags,
            uint memoryBarrierCount,
            VkMemoryBarrier* pMemoryBarriers,
            uint bufferMemoryBarrierCount,
            VkBufferMemoryBarrier* pBufferMemoryBarriers,
            uint imageMemoryBarrierCount,
            VkImageMemoryBarrier* pImageMemoryBarriers
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBeginQuery", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBeginQuery(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkQueryPool")] ulong queryPool,
            uint query,
            [ComAliasName("VkQueryControlFlags")] uint flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdEndQuery", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdEndQuery(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkQueryPool")] ulong queryPool,
            uint query
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdResetQueryPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdResetQueryPool(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkQueryPool")] ulong queryPool,
            uint firstQuery,
            uint queryCount
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdWriteTimestamp", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdWriteTimestamp(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            VkPipelineStageFlagBits pipelineStage,
            [ComAliasName("VkQueryPool")] ulong queryPool,
            uint query
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyQueryPoolResults", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyQueryPoolResults(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkQueryPool")] ulong queryPool,
            uint firstQuery,
            uint queryCount,
            [ComAliasName("VkBuffer")] ulong dstBuffer,
            [ComAliasName("VkDeviceSize")] ulong dstOffset,
            [ComAliasName("VkDeviceSize")] ulong stride,
            [ComAliasName("VkQueryResultFlags")] uint flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdPushConstants", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdPushConstants(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [ComAliasName("VkPipelineLayout")] ulong layout,
            [ComAliasName("VkShaderStageFlags")] uint stageFlags,
            uint offset,
            uint size,
            void* pValues
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBeginRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBeginRenderPass(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            VkRenderPassBeginInfo* pRenderPassBegin,
            VkSubpassContents contents
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdNextSubpass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdNextSubpass(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            VkSubpassContents contents
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdEndRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdEndRenderPass(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdExecuteCommands", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdExecuteCommands(
            [ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            uint commandBufferCount,
            [ComAliasName("VkCommandBuffer")] IntPtr* pCommandBuffers
        );
        #endregion
    }
}
