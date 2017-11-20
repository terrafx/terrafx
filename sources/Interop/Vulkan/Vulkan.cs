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
            [In] VkInstanceCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkInstance")] IntPtr* pInstance
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyInstance", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyInstance(
            [In, ComAliasName("VkInstance")] IntPtr instance,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumeratePhysicalDevices", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumeratePhysicalDevices(
            [In, ComAliasName("VkInstance")] IntPtr instance,
            [In, Out] uint* pPhysicalDeviceCount,
            [Out, Optional, ComAliasName("VkPhysicalDevice[]")] IntPtr* pPhysicalDevices
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceFeatures", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceFeatures(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [Out] VkPhysicalDeviceFeatures* pFeatures
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceFormatProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceFormatProperties(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In] VkFormat format,
            [Out] VkFormatProperties* pFormatProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceImageFormatProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetPhysicalDeviceImageFormatProperties(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In] VkFormat format,
            [In] VkImageType type,
            [In] VkImageTiling tiling,
            [In, ComAliasName("VkImageUsageFlags")] uint usage,
            [In, ComAliasName("VkImageCreateFlags")] uint flags,
            [Out] VkImageFormatProperties* pImageFormatProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceProperties(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [Out] VkPhysicalDeviceProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceQueueFamilyProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceQueueFamilyProperties(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, Out] uint* pQueueFamilyPropertyCount,
            [Out, Optional, ComAliasName("VkQueueFamilyProperties[]")] VkQueueFamilyProperties* pQueueFamilyProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceMemoryProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceMemoryProperties(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [Out] VkPhysicalDeviceMemoryProperties* pMemoryProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetInstanceProcAddr", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("PFN_vkVoidFunction")]
        public static extern IntPtr vkGetInstanceProcAddr(
            [In, ComAliasName("VkInstance")] IntPtr instance,
            [In, ComAliasName("string")] sbyte* pName
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDeviceProcAddr", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("PFN_vkVoidFunction")]
        public static extern IntPtr vkGetDeviceProcAddr(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("string")] sbyte* pName
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDevice", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateDevice(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In] VkDeviceCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkDevice")] IntPtr* pDevice
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyDevice", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyDevice(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateInstanceExtensionProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumerateInstanceExtensionProperties(
            [In, Optional, ComAliasName("string")] sbyte* pLayerName,
            [In, Out] uint* pPropertyCount,
            [Out, Optional, ComAliasName("VkExtensionProperties[]")] VkExtensionProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateDeviceExtensionProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumerateDeviceExtensionProperties(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, Optional, ComAliasName("string")] sbyte* pLayerName,
            [In, Out] uint* pPropertyCount,
            [Out, Optional, ComAliasName("VkExtensionProperties[]")] VkExtensionProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateInstanceLayerProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumerateInstanceLayerProperties(
            [In, Out] uint* pPropertyCount,
            [Out, Optional, ComAliasName("VkLayerProperties[]")] VkLayerProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateDeviceLayerProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumerateDeviceLayerProperties(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, Out] uint* pPropertyCount,
            [Out, Optional, ComAliasName("VkLayerProperties[]")] VkLayerProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDeviceQueue", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetDeviceQueue(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] uint queueFamilyIndex,
            [In] uint queueIndex,
            [Out, ComAliasName("VkQueue")] IntPtr* pQueue
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueueSubmit", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkQueueSubmit(
            [In, ComAliasName("VkQueue")] IntPtr queue,
            [In] uint submitCount,
            [In, ComAliasName("VkSubmitInfo[]")] VkSubmitInfo* pSubmits,
            [In, ComAliasName("VkFence")] ulong fence
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueueWaitIdle", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkQueueWaitIdle(
            [In, ComAliasName("VkQueue")] IntPtr queue
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDeviceWaitIdle", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkDeviceWaitIdle(
            [In, ComAliasName("VkDevice")] IntPtr device
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAllocateMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkAllocateMemory(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkMemoryAllocateInfo* pAllocateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkDeviceMemory")] ulong* pMemory
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFreeMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkFreeMemory(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkDeviceMemory")] ulong memory,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkMapMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkMapMemory(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkDeviceMemory")] ulong memory,
            [In, ComAliasName("VkDeviceSize")] ulong offset,
            [In, ComAliasName("VkDeviceSize")] ulong size,
            [In, ComAliasName("VkMemoryMapFlags")] uint flags,
            [Out] void** ppData
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkUnmapMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkUnmapMemory(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkDeviceMemory")] ulong memory
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFlushMappedMemoryRanges", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkFlushMappedMemoryRanges(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] uint memoryRangeCount,
            [In, ComAliasName("VkMappedMemoryRange[]")] VkMappedMemoryRange* pMemoryRanges
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkInvalidateMappedMemoryRanges", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkInvalidateMappedMemoryRanges(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] uint memoryRangeCount,
            [In, ComAliasName("VkMappedMemoryRange[]")] VkMappedMemoryRange* pMemoryRanges
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDeviceMemoryCommitment", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetDeviceMemoryCommitment(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkDeviceMemory")] ulong memory,
            [Out, ComAliasName("VkDeviceSize")] ulong* pCommittedMemoryInBytes
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkBindBufferMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkBindBufferMemory(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkBuffer")] ulong buffer,
            [In, ComAliasName("VkDeviceMemory")] ulong memory,
            [In, ComAliasName("VkDeviceSize")] ulong memoryOffset
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkBindImageMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkBindImageMemory(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkImage")] ulong image,
            [In, ComAliasName("VkDeviceMemory")] ulong memory,
            [In, ComAliasName("VkDeviceSize")] ulong memoryOffset
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetBufferMemoryRequirements", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetBufferMemoryRequirements(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkBuffer")] ulong buffer,
            [Out] VkMemoryRequirements* pMemoryRequirements
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetImageMemoryRequirements", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetImageMemoryRequirements(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkImage")] ulong image,
            [Out] VkMemoryRequirements* pMemoryRequirements
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetImageSparseMemoryRequirements", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetImageSparseMemoryRequirements(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkImage")] ulong image,
            [In, Out] uint* pSparseMemoryRequirementCount,
            [Out, Optional, ComAliasName("VkSparseImageMemoryRequirements[]")] VkSparseImageMemoryRequirements* pSparseMemoryRequirements
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceSparseImageFormatProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceSparseImageFormatProperties(
            [In, ComAliasName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In] VkFormat format,
            [In] VkImageType type,
            [In] VkSampleCountFlagBits samples,
            [In, ComAliasName("VkImageUsageFlags")] uint usage,
            [In] VkImageTiling tiling,
            [In, Out] uint* pPropertyCount,
            [Out, Optional, ComAliasName("VkSparseImageFormatProperties[]")] VkSparseImageFormatProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueueBindSparse", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkQueueBindSparse(
            [In, ComAliasName("VkQueue")] IntPtr queue,
            [In] uint bindInfoCount,
            [In, ComAliasName("VkBindSparseInfo[]")] VkBindSparseInfo* pBindInfo,
            [In, ComAliasName("VkFence")] ulong fence
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateFence", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateFence(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkFenceCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkFence")] ulong* pFence
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyFence", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyFence(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkFence")] ulong fence,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetFences", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetFences(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] uint fenceCount,
            [In, ComAliasName("VkFence[]")] ulong* pFences
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetFenceStatus", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetFenceStatus(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkFence")] ulong fence
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkWaitForFences", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkWaitForFences(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] uint fenceCount,
            [In, ComAliasName("VkFence[]")] ulong* pFences,
            [In, ComAliasName("VkBool32")] uint waitAll,
            [In] ulong timeout
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateSemaphore", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateSemaphore(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkSemaphoreCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkSemaphore")] ulong* pSemaphore
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroySemaphore", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroySemaphore(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkSemaphore")] ulong semaphore,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateEvent(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkEventCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkEvent")] ulong* pEvent
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyEvent(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkEvent")] ulong @event,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetEventStatus", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetEventStatus(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkEvent")] ulong @event
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkSetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkSetEvent(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkEvent")] ulong @event
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetEvent(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkEvent")] ulong @event
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateQueryPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateQueryPool(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkQueryPoolCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkQueryPool")] ulong* pQueryPool
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyQueryPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyQueryPool(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkQueryPool")] ulong queryPool,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetQueryPoolResults", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetQueryPoolResults(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkQueryPool")] ulong queryPool,
            [In] uint firstQuery,
            [In] uint queryCount,
            [In] nuint dataSize,
            [In] void* pData,
            [In, ComAliasName("VkDeviceSize")] ulong stride,
            [In, ComAliasName("VkQueryResultFlags")] uint flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateBuffer(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkBufferCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkBuffer")] ulong* pBuffer
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyBuffer(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkBuffer")] ulong buffer,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateBufferView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateBufferView(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkBufferViewCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkBufferView")] ulong* pView
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyBufferView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyBufferView(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkBufferView")] ulong bufferView,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateImage(
            [ComAliasName("VkDevice")] IntPtr device,
            [In] VkImageCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkImage")] ulong* pImage
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyImage(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkImage")] ulong image,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetImageSubresourceLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetImageSubresourceLayout(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkImage")] ulong image,
            [In] VkImageSubresource* pSubresource,
            [Out] VkSubresourceLayout* pLayout
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateImageView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateImageView(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkImageViewCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkImageView")] ulong pView
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyImageView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyImageView(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkImageView")] ulong imageView,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateShaderModule", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateShaderModule(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkShaderModuleCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkShaderModule")] ulong* pShaderModule
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyShaderModule", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyShaderModule(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkShaderModule")] ulong shaderModule,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreatePipelineCache", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreatePipelineCache(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkPipelineCacheCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkPipelineCache")] ulong* pPipelineCache
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyPipelineCache", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyPipelineCache(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkPipelineCache")] ulong pipelineCache,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPipelineCacheData", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetPipelineCacheData(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkPipelineCache")] ulong pipelineCache,
            [In, Out] nuint* pDataSize,
            [Out] void* pData = null
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkMergePipelineCaches", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkMergePipelineCaches(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkPipelineCache")] ulong dstCache,
            [In] uint srcCacheCount,
            [In, ComAliasName("VkPipelineCache[]")] ulong* pSrcCaches
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateGraphicsPipelines", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateGraphicsPipelines(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkPipelineCache")] ulong pipelineCache,
            [In] uint createInfoCount,
            [In, ComAliasName("VkGraphicsPipelineCreateInfo")] VkGraphicsPipelineCreateInfo* pCreateInfos,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkPipeline[]")] ulong* pPipelines
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateComputePipelines", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateComputePipelines(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkPipelineCache")] ulong pipelineCache,
            [In] uint createInfoCount,
            [In, ComAliasName("VkComputePipelineCreateInfo[]")] VkComputePipelineCreateInfo* pCreateInfos,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkPipeline[]")] ulong* pPipelines
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyPipeline", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyPipeline(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkPipeline")] ulong pipeline,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreatePipelineLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreatePipelineLayout(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkPipelineLayoutCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkPipelineLayout")] ulong* pPipelineLayout
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyPipelineLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyPipelineLayout(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkPipelineLayout")] ulong pipelineLayout,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateSampler", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateSampler(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkSamplerCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkSampler")] ulong* pSampler
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroySampler", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroySampler(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkSampler")] ulong sampler,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDescriptorSetLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateDescriptorSetLayout(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkDescriptorSetLayoutCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkDescriptorSetLayout")] ulong* pSetLayout
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyDescriptorSetLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyDescriptorSetLayout(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkDescriptorSetLayout")] ulong descriptorSetLayout,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDescriptorPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateDescriptorPool(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkDescriptorPoolCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkDescriptorPool")] ulong* pDescriptorPool
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyDescriptorPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyDescriptorPool(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkDescriptorPool")] ulong descriptorPool,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetDescriptorPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetDescriptorPool(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkDescriptorPool")] ulong descriptorPool,
            [In, ComAliasName("VkDescriptorPoolResetFlags")] uint flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAllocateDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkAllocateDescriptorSets(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkDescriptorSetAllocateInfo* pAllocateInfo,
            [Out, ComAliasName("VkDescriptorSet")] ulong* pDescriptorSets
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFreeDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkFreeDescriptorSets(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkDescriptorPool")] ulong descriptorPool,
            [In] uint descriptorSetCount,
            [In, ComAliasName("VkDescriptorSet[]")] ulong* pDescriptorSets
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkUpdateDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkUpdateDescriptorSets(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] uint descriptorWriteCount,
            [In, ComAliasName("VkWriteDescriptorSet[]")] VkWriteDescriptorSet* pDescriptorWrites,
            [In] uint descriptorCopyCount,
            [In, ComAliasName("VkWriteDescriptorSet[]")] VkCopyDescriptorSet* pDescriptorCopies
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateFramebuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateFramebuffer(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkFramebufferCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkFramebuffer")] ulong* pFramebuffer
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyFramebuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyFramebuffer(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkFramebuffer")] ulong framebuffer,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateRenderPass(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkRenderPassCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkRenderPass")] ulong* pRenderPass
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyRenderPass(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkRenderPass")] ulong renderPass,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetRenderAreaGranularity", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetRenderAreaGranularity(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkRenderPass")] ulong renderPass,
            [Out] VkExtent2D* pGranularity
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateCommandPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateCommandPool(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkCommandPoolCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, ComAliasName("VkCommandPool")] ulong* pCommandPool
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyCommandPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyCommandPool(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkCommandPool")] ulong commandPool,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetCommandPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetCommandPool(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkCommandPool")] ulong commandPool,
            [In, ComAliasName("VkCommandPoolResetFlags")] uint flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAllocateCommandBuffers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkAllocateCommandBuffers(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In] VkCommandBufferAllocateInfo* pAllocateInfo,
            [Out, ComAliasName("VkCommandBuffer[]")] IntPtr* pCommandBuffers
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFreeCommandBuffers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkFreeCommandBuffers(
            [In, ComAliasName("VkDevice")] IntPtr device,
            [In, ComAliasName("VkCommandPool")] ulong commandPool,
            [In] uint commandBufferCount,
            [In, ComAliasName("VkCommandBuffer[]")] IntPtr* pCommandBuffers
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkBeginCommandBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkBeginCommandBuffer(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] VkCommandBufferBeginInfo* pBeginInfo
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEndCommandBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEndCommandBuffer(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetCommandBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetCommandBuffer(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkCommandBufferResetFlags")] uint flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindPipeline", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBindPipeline(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] VkPipelineBindPoint pipelineBindPoint,
            [In, ComAliasName("VkPipeline")] ulong pipeline
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetViewport", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetViewport(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint firstViewport,
            [In] uint viewportCount,
            [In, ComAliasName("VkViewport[]")] VkViewport* pViewports
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetScissor", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetScissor(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint firstScissor,
            [In] uint scissorCount,
            [In, ComAliasName("VkRect2D[]")] VkRect2D* pScissors
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetLineWidth", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetLineWidth(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] float lineWidth
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetDepthBias", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetDepthBias(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] float depthBiasConstantFactor,
            [In] float depthBiasClamp,
            [In] float depthBiasSlopeFactor
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetBlendConstants", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetBlendConstants(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("float[4]")] float* blendConstants
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetDepthBounds", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetDepthBounds(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] float minDepthBounds,
            [In] float maxDepthBounds
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetStencilCompareMask", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetStencilCompareMask(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkStencilFaceFlags")] uint faceMask,
            [In] uint compareMask
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetStencilWriteMask", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetStencilWriteMask(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkStencilFaceFlags")] uint faceMask,
            [In] uint writeMask
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetStencilReference", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetStencilReference(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkStencilFaceFlags")] uint faceMask,
            [In] uint reference
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBindDescriptorSets(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] VkPipelineBindPoint pipelineBindPoint,
            [In, ComAliasName("VkPipelineLayout")] ulong layout,
            [In] uint firstSet,
            [In] uint descriptorSetCount,
            [In, ComAliasName("VkDescriptorSet[]")] ulong* pDescriptorSets,
            [In] uint dynamicOffsetCount,
            [In, ComAliasName("uint[]")] uint* pDynamicOffsets
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindIndexBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBindIndexBuffer(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkBuffer")] ulong buffer,
            [In, ComAliasName("VkDeviceSize")] ulong offset,
            [In] VkIndexType indexType
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindVertexBuffers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBindVertexBuffers(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint firstBinding,
            [In] uint bindingCount,
            [In, ComAliasName("VkBuffer[]")] ulong* pBuffers,
            [In, ComAliasName("VkDeviceSize[]")] ulong* pOffsets
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDraw", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDraw(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint vertexCount,
            [In] uint instanceCount,
            [In] uint firstVertex,
            [In] uint firstInstance
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDrawIndexed", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDrawIndexed(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint indexCount,
            [In] uint instanceCount,
            [In] uint firstIndex,
            [In] int vertexOffset,
            [In] uint firstInstance
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDrawIndirect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDrawIndirect(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkBuffer")] ulong buffer,
            [In, ComAliasName("VkDeviceSize")] ulong offset,
            [In] uint drawCount,
            [In] uint stride
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDrawIndexedIndirect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDrawIndexedIndirect(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkBuffer")] ulong buffer,
            [In, ComAliasName("VkDeviceSize")] ulong offset,
            [In] uint drawCount,
            [In] uint stride
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDispatch", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDispatch(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint groupCountX,
            [In] uint groupCountY,
            [In] uint groupCountZ
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDispatchIndirect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDispatchIndirect(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkBuffer")] ulong buffer,
            [In, ComAliasName("VkDeviceSize")] ulong offset
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyBuffer(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkBuffer")] ulong srcBuffer,
            [In, ComAliasName("VkBuffer")] ulong dstBuffer,
            [In] uint regionCount,
            [In, ComAliasName("VkBufferCopy[]")] VkBufferCopy* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyImage(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkImage")] ulong srcImage,
            [In] VkImageLayout srcImageLayout,
            [In, ComAliasName("VkImage")] ulong dstImage,
            [In] VkImageLayout dstImageLayout,
            [In] uint regionCount,
            [In, ComAliasName("VkImageCopy[]")] VkImageCopy* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBlitImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBlitImage(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkImage")] ulong srcImage,
            [In] VkImageLayout srcImageLayout,
            [In, ComAliasName("VkImage")] ulong dstImage,
            [In] VkImageLayout dstImageLayout,
            [In] uint regionCount,
            [In, ComAliasName("VkImageBlit[]")] VkImageBlit* pRegions,
            [In] VkFilter filter
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyBufferToImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyBufferToImage(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkBuffer")] ulong srcBuffer,
            [In, ComAliasName("VkImage")] ulong dstImage,
            [In] VkImageLayout dstImageLayout,
            [In] uint regionCount,
            [In, ComAliasName("VkBufferImageCopy")] VkBufferImageCopy* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyImageToBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyImageToBuffer(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkImage")] ulong srcImage,
            [In] VkImageLayout srcImageLayout,
            [In, ComAliasName("VkBuffer")] ulong dstBuffer,
            [In] uint regionCount,
            [In, ComAliasName("VkBufferImageCopy[]")] VkBufferImageCopy* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdUpdateBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdUpdateBuffer(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkBuffer")] ulong dstBuffer,
            [In, ComAliasName("VkDeviceSize")] ulong dstOffset,
            [In, ComAliasName("VkDeviceSize")] ulong dataSize,
            [In] void* pData
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdFillBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdFillBuffer(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkBuffer")] ulong dstBuffer,
            [In, ComAliasName("VkDeviceSize")] ulong dstOffset,
            [In, ComAliasName("VkDeviceSize")] ulong size,
            [In] uint data
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdClearColorImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdClearColorImage(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkImage")] ulong image,
            [In] VkImageLayout imageLayout,
            [In] VkClearColorValue* pColor,
            [In] uint rangeCount,
            [In, ComAliasName("VkImageSubresourceRange[]")] VkImageSubresourceRange* pRanges
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdClearDepthStencilImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdClearDepthStencilImage(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkImage")] ulong image,
            [In] VkImageLayout imageLayout,
            [In] VkClearDepthStencilValue* pDepthStencil,
            [In] uint rangeCount,
            [In, ComAliasName("VkImageSubresourceRange[]")] VkImageSubresourceRange* pRanges
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdClearAttachments", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdClearAttachments(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint attachmentCount,
            [In, ComAliasName("VkClearAttachment[]")] VkClearAttachment* pAttachments,
            [In] uint rectCount,
            [In, ComAliasName("VkClearRect[]")] VkClearRect* pRects
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdResolveImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdResolveImage(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkImage")] ulong srcImage,
            [In] VkImageLayout srcImageLayout,
            [In, ComAliasName("VkImage")] ulong dstImage,
            [In] VkImageLayout dstImageLayout,
            [In] uint regionCount,
            [In, ComAliasName("VkImageResolve[]")] VkImageResolve* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetEvent(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkEvent")] ulong @event,
            [In, ComAliasName("VkPipelineStageFlags")] uint stageMask
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdResetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdResetEvent(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkEvent")] ulong @event,
            [In, ComAliasName("VkPipelineStageFlags")] uint stageMask
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdWaitEvents", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdWaitEvents(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint eventCount,
            [In, ComAliasName("VkEvent[]")] ulong* pEvents,
            [In, ComAliasName("VkPipelineStageFlags")] uint srcStageMask,
            [In, ComAliasName("VkPipelineStageFlags")] uint dstStageMask,
            [In] uint memoryBarrierCount,
            [In, Optional, ComAliasName("VkMemoryBarrier[]")] VkMemoryBarrier* pMemoryBarriers,
            [In] uint bufferMemoryBarrierCount,
            [In, Optional, ComAliasName("VkBufferMemoryBarrier[]")] VkBufferMemoryBarrier* pBufferMemoryBarriers,
            [In] uint imageMemoryBarrierCount,
            [In, Optional, ComAliasName("VkImageMemoryBarrier[]")] VkImageMemoryBarrier* pImageMemoryBarriers
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdPipelineBarrier", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdPipelineBarrier(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkPipelineStageFlags")] uint srcStageMask,
            [In, ComAliasName("VkPipelineStageFlags")] uint dstStageMask,
            [In, ComAliasName("VkDependencyFlags")] uint dependencyFlags,
            [In] uint memoryBarrierCount,
            [In, Optional, ComAliasName("VkMemoryBarrier[]")] VkMemoryBarrier* pMemoryBarriers,
            [In] uint bufferMemoryBarrierCount,
            [In, Optional, ComAliasName("VkBufferMemoryBarrier[]")] VkBufferMemoryBarrier* pBufferMemoryBarriers,
            [In] uint imageMemoryBarrierCount,
            [In, Optional, ComAliasName("VkImageMemoryBarrier[]")] VkImageMemoryBarrier* pImageMemoryBarriers
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBeginQuery", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBeginQuery(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkQueryPool")] ulong queryPool,
            [In] uint query,
            [In, ComAliasName("VkQueryControlFlags")] uint flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdEndQuery", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdEndQuery(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkQueryPool")] ulong queryPool,
            [In] uint query
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdResetQueryPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdResetQueryPool(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkQueryPool")] ulong queryPool,
            [In] uint firstQuery,
            [In] uint queryCount
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdWriteTimestamp", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdWriteTimestamp(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] VkPipelineStageFlagBits pipelineStage,
            [In, ComAliasName("VkQueryPool")] ulong queryPool,
            [In] uint query
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyQueryPoolResults", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyQueryPoolResults(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkQueryPool")] ulong queryPool,
            [In] uint firstQuery,
            [In] uint queryCount,
            [In, ComAliasName("VkBuffer")] ulong dstBuffer,
            [In, ComAliasName("VkDeviceSize")] ulong dstOffset,
            [In, ComAliasName("VkDeviceSize")] ulong stride,
            [In, ComAliasName("VkQueryResultFlags")] uint flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdPushConstants", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdPushConstants(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, ComAliasName("VkPipelineLayout")] ulong layout,
            [In, ComAliasName("VkShaderStageFlags")] uint stageFlags,
            [In] uint offset,
            [In] uint size,
            [In] void* pValues
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBeginRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBeginRenderPass(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] VkRenderPassBeginInfo* pRenderPassBegin,
            [In] VkSubpassContents contents
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdNextSubpass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdNextSubpass(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] VkSubpassContents contents
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdEndRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdEndRenderPass(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdExecuteCommands", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdExecuteCommands(
            [In, ComAliasName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint commandBufferCount,
            [In, ComAliasName("VkCommandBuffer[]")] IntPtr* pCommandBuffers
        );
        #endregion
    }
}
