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
#if Windows_NT
        private const string DllName = "Vulkan-1";
#else
        private const string DllName = "libvulkan";
#endif

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
        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateInstance", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateInstance(
            [In] in VkInstanceCreateInfo pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkInstance")] out IntPtr pInstance
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyInstance", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyInstance(
            [In, NativeTypeName("VkInstance")] IntPtr instance,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumeratePhysicalDevices", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumeratePhysicalDevices(
            [In, NativeTypeName("VkInstance")] IntPtr instance,
            [In, Out] uint* pPhysicalDeviceCount,
            [Out, Optional, NativeTypeName("VkPhysicalDevice[]")] IntPtr* pPhysicalDevices
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceFeatures", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceFeatures(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [Out] VkPhysicalDeviceFeatures* pFeatures
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceFormatProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceFormatProperties(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In] VkFormat format,
            [Out] VkFormatProperties* pFormatProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceImageFormatProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetPhysicalDeviceImageFormatProperties(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In] VkFormat format,
            [In] VkImageType type,
            [In] VkImageTiling tiling,
            [In, NativeTypeName("VkImageUsageFlags")] uint usage,
            [In, NativeTypeName("VkImageCreateFlags")] uint flags,
            [Out] VkImageFormatProperties* pImageFormatProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceProperties(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [Out] VkPhysicalDeviceProperties* pProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceQueueFamilyProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceQueueFamilyProperties(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, Out] uint* pQueueFamilyPropertyCount,
            [Out, Optional, NativeTypeName("VkQueueFamilyProperties[]")] VkQueueFamilyProperties* pQueueFamilyProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceMemoryProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceMemoryProperties(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [Out] VkPhysicalDeviceMemoryProperties* pMemoryProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetInstanceProcAddr", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("PFN_vkVoidFunction")]
        public static extern IntPtr vkGetInstanceProcAddr(
            [In, NativeTypeName("VkInstance")] IntPtr instance,
            [In, NativeTypeName("string")] sbyte* pName
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDeviceProcAddr", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("PFN_vkVoidFunction")]
        public static extern IntPtr vkGetDeviceProcAddr(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("string")] sbyte* pName
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDevice", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateDevice(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In] VkDeviceCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkDevice")] IntPtr* pDevice
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyDevice", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyDevice(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateInstanceExtensionProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumerateInstanceExtensionProperties(
            [In, Optional, NativeTypeName("string")] sbyte* pLayerName,
            [In, Out] uint* pPropertyCount,
            [Out, Optional, NativeTypeName("VkExtensionProperties[]")] VkExtensionProperties* pProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateDeviceExtensionProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumerateDeviceExtensionProperties(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, Optional, NativeTypeName("string")] sbyte* pLayerName,
            [In, Out] uint* pPropertyCount,
            [Out, Optional, NativeTypeName("VkExtensionProperties[]")] VkExtensionProperties* pProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateInstanceLayerProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumerateInstanceLayerProperties(
            [In, Out] uint* pPropertyCount,
            [Out, Optional, NativeTypeName("VkLayerProperties[]")] VkLayerProperties* pProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateDeviceLayerProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEnumerateDeviceLayerProperties(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In, Out] uint* pPropertyCount,
            [Out, Optional, NativeTypeName("VkLayerProperties[]")] VkLayerProperties* pProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDeviceQueue", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetDeviceQueue(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] uint queueFamilyIndex,
            [In] uint queueIndex,
            [Out, NativeTypeName("VkQueue")] IntPtr* pQueue
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueueSubmit", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkQueueSubmit(
            [In, NativeTypeName("VkQueue")] IntPtr queue,
            [In] uint submitCount,
            [In, NativeTypeName("VkSubmitInfo[]")] VkSubmitInfo* pSubmits,
            [In, NativeTypeName("VkFence")] ulong fence
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueueWaitIdle", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkQueueWaitIdle(
            [In, NativeTypeName("VkQueue")] IntPtr queue
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDeviceWaitIdle", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkDeviceWaitIdle(
            [In, NativeTypeName("VkDevice")] IntPtr device
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAllocateMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkAllocateMemory(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkMemoryAllocateInfo* pAllocateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkDeviceMemory")] ulong* pMemory
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFreeMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkFreeMemory(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkDeviceMemory")] ulong memory,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkMapMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkMapMemory(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkDeviceMemory")] ulong memory,
            [In, NativeTypeName("VkDeviceSize")] ulong offset,
            [In, NativeTypeName("VkDeviceSize")] ulong size,
            [In, NativeTypeName("VkMemoryMapFlags")] uint flags,
            [Out] void** ppData
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkUnmapMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkUnmapMemory(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkDeviceMemory")] ulong memory
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFlushMappedMemoryRanges", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkFlushMappedMemoryRanges(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] uint memoryRangeCount,
            [In, NativeTypeName("VkMappedMemoryRange[]")] VkMappedMemoryRange* pMemoryRanges
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkInvalidateMappedMemoryRanges", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkInvalidateMappedMemoryRanges(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] uint memoryRangeCount,
            [In, NativeTypeName("VkMappedMemoryRange[]")] VkMappedMemoryRange* pMemoryRanges
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDeviceMemoryCommitment", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetDeviceMemoryCommitment(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkDeviceMemory")] ulong memory,
            [Out, NativeTypeName("VkDeviceSize")] ulong* pCommittedMemoryInBytes
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkBindBufferMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkBindBufferMemory(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkBuffer")] ulong buffer,
            [In, NativeTypeName("VkDeviceMemory")] ulong memory,
            [In, NativeTypeName("VkDeviceSize")] ulong memoryOffset
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkBindImageMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkBindImageMemory(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkImage")] ulong image,
            [In, NativeTypeName("VkDeviceMemory")] ulong memory,
            [In, NativeTypeName("VkDeviceSize")] ulong memoryOffset
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetBufferMemoryRequirements", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetBufferMemoryRequirements(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkBuffer")] ulong buffer,
            [Out] VkMemoryRequirements* pMemoryRequirements
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetImageMemoryRequirements", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetImageMemoryRequirements(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkImage")] ulong image,
            [Out] VkMemoryRequirements* pMemoryRequirements
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetImageSparseMemoryRequirements", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetImageSparseMemoryRequirements(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkImage")] ulong image,
            [In, Out] uint* pSparseMemoryRequirementCount,
            [Out, Optional, NativeTypeName("VkSparseImageMemoryRequirements[]")] VkSparseImageMemoryRequirements* pSparseMemoryRequirements
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceSparseImageFormatProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetPhysicalDeviceSparseImageFormatProperties(
            [In, NativeTypeName("VkPhysicalDevice")] IntPtr physicalDevice,
            [In] VkFormat format,
            [In] VkImageType type,
            [In] VkSampleCountFlagBits samples,
            [In, NativeTypeName("VkImageUsageFlags")] uint usage,
            [In] VkImageTiling tiling,
            [In, Out] uint* pPropertyCount,
            [Out, Optional, NativeTypeName("VkSparseImageFormatProperties[]")] VkSparseImageFormatProperties* pProperties
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueueBindSparse", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkQueueBindSparse(
            [In, NativeTypeName("VkQueue")] IntPtr queue,
            [In] uint bindInfoCount,
            [In, NativeTypeName("VkBindSparseInfo[]")] VkBindSparseInfo* pBindInfo,
            [In, NativeTypeName("VkFence")] ulong fence
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateFence", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateFence(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkFenceCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkFence")] ulong* pFence
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyFence", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyFence(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkFence")] ulong fence,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetFences", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetFences(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] uint fenceCount,
            [In, NativeTypeName("VkFence[]")] ulong* pFences
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetFenceStatus", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetFenceStatus(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkFence")] ulong fence
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkWaitForFences", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkWaitForFences(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] uint fenceCount,
            [In, NativeTypeName("VkFence[]")] ulong* pFences,
            [In, NativeTypeName("VkBool32")] uint waitAll,
            [In] ulong timeout
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateSemaphore", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateSemaphore(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkSemaphoreCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkSemaphore")] ulong* pSemaphore
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroySemaphore", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroySemaphore(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkSemaphore")] ulong semaphore,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateEvent(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkEventCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkEvent")] ulong* pEvent
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyEvent(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkEvent")] ulong @event,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetEventStatus", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetEventStatus(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkEvent")] ulong @event
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkSetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkSetEvent(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkEvent")] ulong @event
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetEvent(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkEvent")] ulong @event
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateQueryPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateQueryPool(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkQueryPoolCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkQueryPool")] ulong* pQueryPool
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyQueryPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyQueryPool(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkQueryPool")] ulong queryPool,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetQueryPoolResults", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetQueryPoolResults(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkQueryPool")] ulong queryPool,
            [In] uint firstQuery,
            [In] uint queryCount,
            [In] UIntPtr dataSize,
            [In] void* pData,
            [In, NativeTypeName("VkDeviceSize")] ulong stride,
            [In, NativeTypeName("VkQueryResultFlags")] uint flags
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateBuffer(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkBufferCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkBuffer")] ulong* pBuffer
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyBuffer(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkBuffer")] ulong buffer,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateBufferView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateBufferView(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkBufferViewCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkBufferView")] ulong* pView
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyBufferView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyBufferView(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkBufferView")] ulong bufferView,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateImage(
            [NativeTypeName("VkDevice")] IntPtr device,
            [In] VkImageCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkImage")] ulong* pImage
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyImage(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkImage")] ulong image,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetImageSubresourceLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetImageSubresourceLayout(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkImage")] ulong image,
            [In] VkImageSubresource* pSubresource,
            [Out] VkSubresourceLayout* pLayout
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateImageView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateImageView(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkImageViewCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkImageView")] ulong pView
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyImageView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyImageView(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkImageView")] ulong imageView,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateShaderModule", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateShaderModule(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkShaderModuleCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkShaderModule")] ulong* pShaderModule
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyShaderModule", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyShaderModule(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkShaderModule")] ulong shaderModule,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreatePipelineCache", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreatePipelineCache(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkPipelineCacheCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkPipelineCache")] ulong* pPipelineCache
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyPipelineCache", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyPipelineCache(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkPipelineCache")] ulong pipelineCache,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPipelineCacheData", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkGetPipelineCacheData(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkPipelineCache")] ulong pipelineCache,
            [In, Out] UIntPtr* pDataSize,
            [Out] void* pData = null
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkMergePipelineCaches", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkMergePipelineCaches(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkPipelineCache")] ulong dstCache,
            [In] uint srcCacheCount,
            [In, NativeTypeName("VkPipelineCache[]")] ulong* pSrcCaches
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateGraphicsPipelines", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateGraphicsPipelines(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkPipelineCache")] ulong pipelineCache,
            [In] uint createInfoCount,
            [In, NativeTypeName("VkGraphicsPipelineCreateInfo")] VkGraphicsPipelineCreateInfo* pCreateInfos,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkPipeline[]")] ulong* pPipelines
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateComputePipelines", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateComputePipelines(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkPipelineCache")] ulong pipelineCache,
            [In] uint createInfoCount,
            [In, NativeTypeName("VkComputePipelineCreateInfo[]")] VkComputePipelineCreateInfo* pCreateInfos,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkPipeline[]")] ulong* pPipelines
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyPipeline", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyPipeline(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkPipeline")] ulong pipeline,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreatePipelineLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreatePipelineLayout(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkPipelineLayoutCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkPipelineLayout")] ulong* pPipelineLayout
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyPipelineLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyPipelineLayout(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkPipelineLayout")] ulong pipelineLayout,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateSampler", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateSampler(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkSamplerCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkSampler")] ulong* pSampler
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroySampler", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroySampler(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkSampler")] ulong sampler,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDescriptorSetLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateDescriptorSetLayout(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkDescriptorSetLayoutCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkDescriptorSetLayout")] ulong* pSetLayout
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyDescriptorSetLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyDescriptorSetLayout(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkDescriptorSetLayout")] ulong descriptorSetLayout,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDescriptorPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateDescriptorPool(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkDescriptorPoolCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkDescriptorPool")] ulong* pDescriptorPool
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyDescriptorPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyDescriptorPool(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkDescriptorPool")] ulong descriptorPool,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetDescriptorPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetDescriptorPool(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkDescriptorPool")] ulong descriptorPool,
            [In, NativeTypeName("VkDescriptorPoolResetFlags")] uint flags
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAllocateDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkAllocateDescriptorSets(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkDescriptorSetAllocateInfo* pAllocateInfo,
            [Out, NativeTypeName("VkDescriptorSet")] ulong* pDescriptorSets
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFreeDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkFreeDescriptorSets(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkDescriptorPool")] ulong descriptorPool,
            [In] uint descriptorSetCount,
            [In, NativeTypeName("VkDescriptorSet[]")] ulong* pDescriptorSets
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkUpdateDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkUpdateDescriptorSets(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] uint descriptorWriteCount,
            [In, NativeTypeName("VkWriteDescriptorSet[]")] VkWriteDescriptorSet* pDescriptorWrites,
            [In] uint descriptorCopyCount,
            [In, NativeTypeName("VkWriteDescriptorSet[]")] VkCopyDescriptorSet* pDescriptorCopies
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateFramebuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateFramebuffer(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkFramebufferCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkFramebuffer")] ulong* pFramebuffer
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyFramebuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyFramebuffer(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkFramebuffer")] ulong framebuffer,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateRenderPass(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkRenderPassCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkRenderPass")] ulong* pRenderPass
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyRenderPass(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkRenderPass")] ulong renderPass,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetRenderAreaGranularity", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkGetRenderAreaGranularity(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkRenderPass")] ulong renderPass,
            [Out] VkExtent2D* pGranularity
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateCommandPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkCreateCommandPool(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkCommandPoolCreateInfo* pCreateInfo,
            [In, Optional] VkAllocationCallbacks* pAllocator,
            [Out, NativeTypeName("VkCommandPool")] ulong* pCommandPool
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyCommandPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkDestroyCommandPool(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkCommandPool")] ulong commandPool,
            [In, Optional] VkAllocationCallbacks* pAllocator
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetCommandPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetCommandPool(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkCommandPool")] ulong commandPool,
            [In, NativeTypeName("VkCommandPoolResetFlags")] uint flags
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAllocateCommandBuffers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkAllocateCommandBuffers(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In] VkCommandBufferAllocateInfo* pAllocateInfo,
            [Out, NativeTypeName("VkCommandBuffer[]")] IntPtr* pCommandBuffers
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFreeCommandBuffers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkFreeCommandBuffers(
            [In, NativeTypeName("VkDevice")] IntPtr device,
            [In, NativeTypeName("VkCommandPool")] ulong commandPool,
            [In] uint commandBufferCount,
            [In, NativeTypeName("VkCommandBuffer[]")] IntPtr* pCommandBuffers
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkBeginCommandBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkBeginCommandBuffer(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] VkCommandBufferBeginInfo* pBeginInfo
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEndCommandBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkEndCommandBuffer(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetCommandBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VkResult vkResetCommandBuffer(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkCommandBufferResetFlags")] uint flags
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindPipeline", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBindPipeline(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] VkPipelineBindPoint pipelineBindPoint,
            [In, NativeTypeName("VkPipeline")] ulong pipeline
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetViewport", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetViewport(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint firstViewport,
            [In] uint viewportCount,
            [In, NativeTypeName("VkViewport[]")] VkViewport* pViewports
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetScissor", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetScissor(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint firstScissor,
            [In] uint scissorCount,
            [In, NativeTypeName("VkRect2D[]")] VkRect2D* pScissors
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetLineWidth", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetLineWidth(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] float lineWidth
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetDepthBias", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetDepthBias(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] float depthBiasConstantFactor,
            [In] float depthBiasClamp,
            [In] float depthBiasSlopeFactor
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetBlendConstants", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetBlendConstants(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("float[4]")] float* blendConstants
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetDepthBounds", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetDepthBounds(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] float minDepthBounds,
            [In] float maxDepthBounds
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetStencilCompareMask", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetStencilCompareMask(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkStencilFaceFlags")] uint faceMask,
            [In] uint compareMask
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetStencilWriteMask", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetStencilWriteMask(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkStencilFaceFlags")] uint faceMask,
            [In] uint writeMask
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetStencilReference", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetStencilReference(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkStencilFaceFlags")] uint faceMask,
            [In] uint reference
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBindDescriptorSets(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] VkPipelineBindPoint pipelineBindPoint,
            [In, NativeTypeName("VkPipelineLayout")] ulong layout,
            [In] uint firstSet,
            [In] uint descriptorSetCount,
            [In, NativeTypeName("VkDescriptorSet[]")] ulong* pDescriptorSets,
            [In] uint dynamicOffsetCount,
            [In, NativeTypeName("uint[]")] uint* pDynamicOffsets
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindIndexBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBindIndexBuffer(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkBuffer")] ulong buffer,
            [In, NativeTypeName("VkDeviceSize")] ulong offset,
            [In] VkIndexType indexType
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindVertexBuffers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBindVertexBuffers(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint firstBinding,
            [In] uint bindingCount,
            [In, NativeTypeName("VkBuffer[]")] ulong* pBuffers,
            [In, NativeTypeName("VkDeviceSize[]")] ulong* pOffsets
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDraw", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDraw(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint vertexCount,
            [In] uint instanceCount,
            [In] uint firstVertex,
            [In] uint firstInstance
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDrawIndexed", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDrawIndexed(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint indexCount,
            [In] uint instanceCount,
            [In] uint firstIndex,
            [In] int vertexOffset,
            [In] uint firstInstance
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDrawIndirect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDrawIndirect(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkBuffer")] ulong buffer,
            [In, NativeTypeName("VkDeviceSize")] ulong offset,
            [In] uint drawCount,
            [In] uint stride
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDrawIndexedIndirect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDrawIndexedIndirect(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkBuffer")] ulong buffer,
            [In, NativeTypeName("VkDeviceSize")] ulong offset,
            [In] uint drawCount,
            [In] uint stride
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDispatch", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDispatch(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint groupCountX,
            [In] uint groupCountY,
            [In] uint groupCountZ
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDispatchIndirect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdDispatchIndirect(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkBuffer")] ulong buffer,
            [In, NativeTypeName("VkDeviceSize")] ulong offset
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyBuffer(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkBuffer")] ulong srcBuffer,
            [In, NativeTypeName("VkBuffer")] ulong dstBuffer,
            [In] uint regionCount,
            [In, NativeTypeName("VkBufferCopy[]")] VkBufferCopy* pRegions
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyImage(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkImage")] ulong srcImage,
            [In] VkImageLayout srcImageLayout,
            [In, NativeTypeName("VkImage")] ulong dstImage,
            [In] VkImageLayout dstImageLayout,
            [In] uint regionCount,
            [In, NativeTypeName("VkImageCopy[]")] VkImageCopy* pRegions
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBlitImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBlitImage(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkImage")] ulong srcImage,
            [In] VkImageLayout srcImageLayout,
            [In, NativeTypeName("VkImage")] ulong dstImage,
            [In] VkImageLayout dstImageLayout,
            [In] uint regionCount,
            [In, NativeTypeName("VkImageBlit[]")] VkImageBlit* pRegions,
            [In] VkFilter filter
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyBufferToImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyBufferToImage(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkBuffer")] ulong srcBuffer,
            [In, NativeTypeName("VkImage")] ulong dstImage,
            [In] VkImageLayout dstImageLayout,
            [In] uint regionCount,
            [In, NativeTypeName("VkBufferImageCopy")] VkBufferImageCopy* pRegions
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyImageToBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyImageToBuffer(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkImage")] ulong srcImage,
            [In] VkImageLayout srcImageLayout,
            [In, NativeTypeName("VkBuffer")] ulong dstBuffer,
            [In] uint regionCount,
            [In, NativeTypeName("VkBufferImageCopy[]")] VkBufferImageCopy* pRegions
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdUpdateBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdUpdateBuffer(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkBuffer")] ulong dstBuffer,
            [In, NativeTypeName("VkDeviceSize")] ulong dstOffset,
            [In, NativeTypeName("VkDeviceSize")] ulong dataSize,
            [In] void* pData
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdFillBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdFillBuffer(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkBuffer")] ulong dstBuffer,
            [In, NativeTypeName("VkDeviceSize")] ulong dstOffset,
            [In, NativeTypeName("VkDeviceSize")] ulong size,
            [In] uint data
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdClearColorImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdClearColorImage(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkImage")] ulong image,
            [In] VkImageLayout imageLayout,
            [In] VkClearColorValue* pColor,
            [In] uint rangeCount,
            [In, NativeTypeName("VkImageSubresourceRange[]")] VkImageSubresourceRange* pRanges
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdClearDepthStencilImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdClearDepthStencilImage(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkImage")] ulong image,
            [In] VkImageLayout imageLayout,
            [In] VkClearDepthStencilValue* pDepthStencil,
            [In] uint rangeCount,
            [In, NativeTypeName("VkImageSubresourceRange[]")] VkImageSubresourceRange* pRanges
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdClearAttachments", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdClearAttachments(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint attachmentCount,
            [In, NativeTypeName("VkClearAttachment[]")] VkClearAttachment* pAttachments,
            [In] uint rectCount,
            [In, NativeTypeName("VkClearRect[]")] VkClearRect* pRects
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdResolveImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdResolveImage(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkImage")] ulong srcImage,
            [In] VkImageLayout srcImageLayout,
            [In, NativeTypeName("VkImage")] ulong dstImage,
            [In] VkImageLayout dstImageLayout,
            [In] uint regionCount,
            [In, NativeTypeName("VkImageResolve[]")] VkImageResolve* pRegions
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdSetEvent(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkEvent")] ulong @event,
            [In, NativeTypeName("VkPipelineStageFlags")] uint stageMask
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdResetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdResetEvent(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkEvent")] ulong @event,
            [In, NativeTypeName("VkPipelineStageFlags")] uint stageMask
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdWaitEvents", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdWaitEvents(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint eventCount,
            [In, NativeTypeName("VkEvent[]")] ulong* pEvents,
            [In, NativeTypeName("VkPipelineStageFlags")] uint srcStageMask,
            [In, NativeTypeName("VkPipelineStageFlags")] uint dstStageMask,
            [In] uint memoryBarrierCount,
            [In, Optional, NativeTypeName("VkMemoryBarrier[]")] VkMemoryBarrier* pMemoryBarriers,
            [In] uint bufferMemoryBarrierCount,
            [In, Optional, NativeTypeName("VkBufferMemoryBarrier[]")] VkBufferMemoryBarrier* pBufferMemoryBarriers,
            [In] uint imageMemoryBarrierCount,
            [In, Optional, NativeTypeName("VkImageMemoryBarrier[]")] VkImageMemoryBarrier* pImageMemoryBarriers
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdPipelineBarrier", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdPipelineBarrier(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkPipelineStageFlags")] uint srcStageMask,
            [In, NativeTypeName("VkPipelineStageFlags")] uint dstStageMask,
            [In, NativeTypeName("VkDependencyFlags")] uint dependencyFlags,
            [In] uint memoryBarrierCount,
            [In, Optional, NativeTypeName("VkMemoryBarrier[]")] VkMemoryBarrier* pMemoryBarriers,
            [In] uint bufferMemoryBarrierCount,
            [In, Optional, NativeTypeName("VkBufferMemoryBarrier[]")] VkBufferMemoryBarrier* pBufferMemoryBarriers,
            [In] uint imageMemoryBarrierCount,
            [In, Optional, NativeTypeName("VkImageMemoryBarrier[]")] VkImageMemoryBarrier* pImageMemoryBarriers
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBeginQuery", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBeginQuery(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkQueryPool")] ulong queryPool,
            [In] uint query,
            [In, NativeTypeName("VkQueryControlFlags")] uint flags
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdEndQuery", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdEndQuery(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkQueryPool")] ulong queryPool,
            [In] uint query
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdResetQueryPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdResetQueryPool(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkQueryPool")] ulong queryPool,
            [In] uint firstQuery,
            [In] uint queryCount
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdWriteTimestamp", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdWriteTimestamp(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] VkPipelineStageFlagBits pipelineStage,
            [In, NativeTypeName("VkQueryPool")] ulong queryPool,
            [In] uint query
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyQueryPoolResults", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdCopyQueryPoolResults(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkQueryPool")] ulong queryPool,
            [In] uint firstQuery,
            [In] uint queryCount,
            [In, NativeTypeName("VkBuffer")] ulong dstBuffer,
            [In, NativeTypeName("VkDeviceSize")] ulong dstOffset,
            [In, NativeTypeName("VkDeviceSize")] ulong stride,
            [In, NativeTypeName("VkQueryResultFlags")] uint flags
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdPushConstants", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdPushConstants(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In, NativeTypeName("VkPipelineLayout")] ulong layout,
            [In, NativeTypeName("VkShaderStageFlags")] uint stageFlags,
            [In] uint offset,
            [In] uint size,
            [In] void* pValues
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBeginRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdBeginRenderPass(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] VkRenderPassBeginInfo* pRenderPassBegin,
            [In] VkSubpassContents contents
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdNextSubpass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdNextSubpass(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] VkSubpassContents contents
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdEndRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdEndRenderPass(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdExecuteCommands", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void vkCmdExecuteCommands(
            [In, NativeTypeName("VkCommandBuffer")] IntPtr commandBuffer,
            [In] uint commandBufferCount,
            [In, NativeTypeName("VkCommandBuffer[]")] IntPtr* pCommandBuffers
        );
        #endregion
    }
}
