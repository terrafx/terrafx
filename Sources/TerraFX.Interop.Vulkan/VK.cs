// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public static class VK
    {
        #region Constants
        public const uint VERSION_1_0 = 1;

        public const uint HEADER_VERSION = 51;

        public const float LOD_CLAMP_NONE = 1000.0f;

        public const uint REMAINING_MIP_LEVELS = ~0u;

        public const uint REMAINING_ARRAY_LAYERS = ~0u;

        public const ulong WHOLE_SIZE = ~0ul;

        public const uint ATTACHMENT_UNUSED = ~0u;

        public const uint TRUE = 1;

        public const uint FALSE = 0;

        public const uint QUEUE_FAMILY_IGNORED = ~0u;

        public const uint SUBPASS_EXTERNAL = ~0u;

        public const int MAX_PHYSICAL_DEVICE_NAME_SIZE = 256;

        public const int UUID_SIZE = 16;

        public const int MAX_MEMORY_TYPES = 32;

        public const int MAX_MEMORY_HEAPS = 16;

        public const int MAX_EXTENSION_NAME_SIZE = 256;

        public const int MAX_DESCRIPTION_SIZE = 256;

        public static readonly uint API_VERSION_1_0 = MAKE_VERSION(1, 0, 0);
        #endregion

        #region Methods
        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateInstance", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateInstance(
            VkInstanceCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkInstance* pInstance
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyInstance", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyInstance(
            VkInstance instance,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumeratePhysicalDevices", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult EnumeratePhysicalDevices(
            VkInstance instance,
            uint* pPhysicalDeviceCount,
            VkPhysicalDevice* pPhysicalDevices
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceFeatures", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetPhysicalDeviceFeatures(
            VkPhysicalDevice physicalDevice,
            VkPhysicalDeviceFeatures* pFeatures
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceFormatProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetPhysicalDeviceFormatProperties(
            VkPhysicalDevice physicalDevice,
            VkFormat format,
            VkFormatProperties* pFormatProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceImageFormatProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult GetPhysicalDeviceImageFormatProperties(
            VkPhysicalDevice physicalDevice,
            VkFormat format,
            VkImageType type,
            VkImageTiling tiling,
            VkImageUsageFlags usage,
            VkImageCreateFlags flags,
            VkImageFormatProperties* pImageFormatProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetPhysicalDeviceProperties(
            VkPhysicalDevice physicalDevice,
            VkPhysicalDeviceProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceQueueFamilyProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetPhysicalDeviceQueueFamilyProperties(
            VkPhysicalDevice physicalDevice,
            uint* pQueueFamilyPropertyCount,
            VkQueueFamilyProperties* pQueueFamilyProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceMemoryProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetPhysicalDeviceMemoryProperties(
            VkPhysicalDevice physicalDevice,
            VkPhysicalDeviceMemoryProperties* pMemoryProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetInstanceProcAddr", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern PFN_vkVoidFunction GetInstanceProcAddr(
            VkInstance instance,
            byte* pName
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDeviceProcAddr", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern PFN_vkVoidFunction GetDeviceProcAddr(
            VkDevice device,
            byte* pName
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDevice", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateDevice(
            VkPhysicalDevice physicalDevice,
            VkDeviceCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkDevice* pDevice
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyDevice", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyDevice(
            VkDevice device,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateInstanceExtensionProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult EnumerateInstanceExtensionProperties(
            byte* pLayerName,
            uint* pPropertyCount,
            VkExtensionProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateDeviceExtensionProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult EnumerateDeviceExtensionProperties(
            VkPhysicalDevice physicalDevice,
            byte* pLayerName,
            uint* pPropertyCount,
            VkExtensionProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateInstanceLayerProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult EnumerateInstanceLayerProperties(
            uint* pPropertyCount,
            VkLayerProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEnumerateDeviceLayerProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult EnumerateDeviceLayerProperties(
            VkPhysicalDevice physicalDevice,
            uint* pPropertyCount,
            VkLayerProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDeviceQueue", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetDeviceQueue(
            VkDevice device,
            uint queueFamilyIndex,
            uint queueIndex,
            VkQueue* pQueue
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueueSubmit", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult QueueSubmit(
            VkQueue queue,
            uint submitCount,
            VkSubmitInfo* pSubmits,
            VkFence fence
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueueWaitIdle", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult QueueWaitIdle(
            VkQueue queue
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDeviceWaitIdle", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult DeviceWaitIdle(
            VkDevice device
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAllocateMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult AllocateMemory(
            VkDevice device,
            VkMemoryAllocateInfo* pAllocateInfo,
            VkAllocationCallbacks* pAllocator,
            VkDeviceMemory* pMemory
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFreeMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void FreeMemory(
            VkDevice device,
            VkDeviceMemory memory,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkMapMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult MapMemory(
            VkDevice device,
            VkDeviceMemory memory,
            VkDeviceSize offset,
            VkDeviceSize size,
            VkMemoryMapFlags flags,
            void** ppData
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkUnmapMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void UnmapMemory(
            VkDevice device,
            VkDeviceMemory memory
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFlushMappedMemoryRanges", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult FlushMappedMemoryRanges(
            VkDevice device,
            uint memoryRangeCount,
            VkMappedMemoryRange* pMemoryRanges
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkInvalidateMappedMemoryRanges", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult InvalidateMappedMemoryRanges(
            VkDevice device,
            uint memoryRangeCount,
            VkMappedMemoryRange* pMemoryRanges
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetDeviceMemoryCommitment", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetDeviceMemoryCommitment(
            VkDevice device,
            VkDeviceMemory memory,
            VkDeviceSize* pCommittedMemoryInBytes
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkBindBufferMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult BindBufferMemory(
            VkDevice device,
            VkBuffer buffer,
            VkDeviceMemory memory,
            VkDeviceSize memoryOffset
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkBindImageMemory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult BindImageMemory(
            VkDevice device,
            VkImage image,
            VkDeviceMemory memory,
            VkDeviceSize memoryOffset
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetBufferMemoryRequirements", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetBufferMemoryRequirements(
            VkDevice device,
            VkBuffer buffer,
            VkMemoryRequirements* pMemoryRequirements
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetImageMemoryRequirements", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetImageMemoryRequirements(
            VkDevice device,
            VkImage image,
            VkMemoryRequirements* pMemoryRequirements
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetImageSparseMemoryRequirements", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetImageSparseMemoryRequirements(
            VkDevice device,
            VkImage image,
            uint* pSparseMemoryRequirementCount,
            VkSparseImageMemoryRequirements* pSparseMemoryRequirements
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPhysicalDeviceSparseImageFormatProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetPhysicalDeviceSparseImageFormatProperties(
            VkPhysicalDevice physicalDevice,
            VkFormat format,
            VkImageType type,
            VkSampleCountFlags samples,
            VkImageUsageFlags usage,
            VkImageTiling tiling,
            uint* pPropertyCount,
            VkSparseImageFormatProperties* pProperties
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkQueueBindSparse", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult QueueBindSparse(
            VkQueue queue,
            uint bindInfoCount,
            VkBindSparseInfo* pBindInfo,
            VkFence fence
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateFence", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateFence(
            VkDevice device,
            VkFenceCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkFence* pFence
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyFence", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyFence(
            VkDevice device,
            VkFence fence,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetFences", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult ResetFences(
            VkDevice device,
            uint fenceCount,
            VkFence* pFences
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetFenceStatus", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult GetFenceStatus(
            VkDevice device,
            VkFence fence
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkWaitForFences", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult WaitForFences(
            VkDevice device,
            uint fenceCount,
            VkFence* pFences,
            VkBool32 waitAll,
            ulong timeout
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateSemaphore", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateSemaphore(
            VkDevice device,
            VkSemaphoreCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkSemaphore* pSemaphore
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroySemaphore", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroySemaphore(
            VkDevice device,
            VkSemaphore semaphore,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateEvent(
            VkDevice device,
            VkEventCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkEvent* pEvent
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyEvent(
            VkDevice device,
            VkEvent @event,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetEventStatus", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult GetEventStatus(
            VkDevice device,
            VkEvent @event
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkSetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult SetEvent(
            VkDevice device,
            VkEvent @event
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult ResetEvent(
            VkDevice device,
            VkEvent @event
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateQueryPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateQueryPool(
            VkDevice device,
            VkQueryPoolCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkQueryPool* pQueryPool
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyQueryPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyQueryPool(
            VkDevice device,
            VkQueryPool queryPool,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetQueryPoolResults", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult GetQueryPoolResults(
            VkDevice device,
            VkQueryPool queryPool,
            uint firstQuery,
            uint queryCount,
            UIntPtr dataSize,
            void* pData,
            VkDeviceSize stride,
            VkQueryResultFlags flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateBuffer(
            VkDevice device,
            VkBufferCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkBuffer* pBuffer
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyBuffer(
            VkDevice device,
            VkBuffer buffer,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateBufferView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateBufferView(
            VkDevice device,
            VkBufferViewCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkBufferView* pView
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyBufferView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyBufferView(
            VkDevice device,
            VkBufferView bufferView,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateImage(
            VkDevice device,
            VkImageCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkImage* pImage
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyImage(
            VkDevice device,
            VkImage image,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetImageSubresourceLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetImageSubresourceLayout(
            VkDevice device,
            VkImage image,
            VkImageSubresource* pSubresource,
            VkSubresourceLayout* pLayout
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateImageView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateImageView(
            VkDevice device,
             VkImageViewCreateInfo* pCreateInfo,
             VkAllocationCallbacks* pAllocator,
            VkImageView* pView
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyImageView", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyImageView(
            VkDevice device,
            VkImageView imageView,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateShaderModule", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateShaderModule(
            VkDevice device,
            VkShaderModuleCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkShaderModule* pShaderModule
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyShaderModule", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyShaderModule(
            VkDevice device,
            VkShaderModule shaderModule,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreatePipelineCache", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreatePipelineCache(
            VkDevice device,
            VkPipelineCacheCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkPipelineCache* pPipelineCache
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyPipelineCache", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyPipelineCache(
            VkDevice device,
            VkPipelineCache pipelineCache,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetPipelineCacheData", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult GetPipelineCacheData(
            VkDevice device,
            VkPipelineCache pipelineCache,
            UIntPtr* pDataSize,
            void* pData
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkMergePipelineCaches", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult MergePipelineCaches(
            VkDevice device,
            VkPipelineCache dstCache,
            uint srcCacheCount,
            VkPipelineCache* pSrcCaches
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateGraphicsPipelines", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateGraphicsPipelines(
            VkDevice device,
            VkPipelineCache pipelineCache,
            uint createInfoCount,
            VkGraphicsPipelineCreateInfo* pCreateInfos,
            VkAllocationCallbacks* pAllocator,
            VkPipeline* pPipelines
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateComputePipelines", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateComputePipelines(
            VkDevice device,
            VkPipelineCache pipelineCache,
            uint createInfoCount,
            VkComputePipelineCreateInfo* pCreateInfos,
            VkAllocationCallbacks* pAllocator,
            VkPipeline* pPipelines
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyPipeline", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyPipeline(
            VkDevice device,
            VkPipeline pipeline,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreatePipelineLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreatePipelineLayout(
            VkDevice device,
            VkPipelineLayoutCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkPipelineLayout* pPipelineLayout
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyPipelineLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyPipelineLayout(
            VkDevice device,
            VkPipelineLayout pipelineLayout,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateSampler", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateSampler(
            VkDevice device,
            VkSamplerCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkSampler* pSampler
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroySampler", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroySampler(
            VkDevice device,
            VkSampler sampler,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDescriptorSetLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateDescriptorSetLayout(
            VkDevice device,
            VkDescriptorSetLayoutCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkDescriptorSetLayout* pSetLayout
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyDescriptorSetLayout", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyDescriptorSetLayout(
            VkDevice device,
            VkDescriptorSetLayout descriptorSetLayout,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateDescriptorPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateDescriptorPool(
            VkDevice device,
            VkDescriptorPoolCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkDescriptorPool* pDescriptorPool
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyDescriptorPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyDescriptorPool(
            VkDevice device,
            VkDescriptorPool descriptorPool,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetDescriptorPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult ResetDescriptorPool(
            VkDevice device,
            VkDescriptorPool descriptorPool,
            VkDescriptorPoolResetFlags flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAllocateDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult AllocateDescriptorSets(
            VkDevice device,
            VkDescriptorSetAllocateInfo* pAllocateInfo,
            VkDescriptorSet* pDescriptorSets
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFreeDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult FreeDescriptorSets(
            VkDevice device,
            VkDescriptorPool descriptorPool,
            uint descriptorSetCount,
            VkDescriptorSet* pDescriptorSets
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkUpdateDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void UpdateDescriptorSets(
            VkDevice device,
            uint descriptorWriteCount,
            VkWriteDescriptorSet* pDescriptorWrites,
            uint descriptorCopyCount,
            VkCopyDescriptorSet* pDescriptorCopies
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateFramebuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateFramebuffer(
            VkDevice device,
            VkFramebufferCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkFramebuffer* pFramebuffer
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyFramebuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyFramebuffer(
            VkDevice device,
            VkFramebuffer framebuffer,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateRenderPass(
            VkDevice device,
            VkRenderPassCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkRenderPass* pRenderPass
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyRenderPass(
            VkDevice device,
            VkRenderPass renderPass,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkGetRenderAreaGranularity", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void GetRenderAreaGranularity(
            VkDevice device,
            VkRenderPass renderPass,
            VkExtent2D* pGranularity
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCreateCommandPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult CreateCommandPool(
            VkDevice device,
            VkCommandPoolCreateInfo* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkCommandPool* pCommandPool
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkDestroyCommandPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DestroyCommandPool(
            VkDevice device,
            VkCommandPool commandPool,
            VkAllocationCallbacks* pAllocator
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetCommandPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult ResetCommandPool(
            VkDevice device,
            VkCommandPool commandPool,
            VkCommandPoolResetFlags flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkAllocateCommandBuffers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult AllocateCommandBuffers(
            VkDevice device,
            VkCommandBufferAllocateInfo* pAllocateInfo,
            VkCommandBuffer* pCommandBuffers
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkFreeCommandBuffers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void FreeCommandBuffers(
            VkDevice device,
            VkCommandPool commandPool,
            uint commandBufferCount,
            VkCommandBuffer* pCommandBuffers
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkBeginCommandBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult BeginCommandBuffer(
            VkCommandBuffer commandBuffer,
            VkCommandBufferBeginInfo* pBeginInfo
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkEndCommandBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult EndCommandBuffer(
            VkCommandBuffer commandBuffer
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkResetCommandBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VkResult ResetCommandBuffer(
            VkCommandBuffer commandBuffer,
            VkCommandBufferResetFlags flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindPipeline", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdBindPipeline(
            VkCommandBuffer commandBuffer,
            VkPipelineBindPoint pipelineBindPoint,
            VkPipeline pipeline
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetViewport", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdSetViewport(
            VkCommandBuffer commandBuffer,
            uint firstViewport,
            uint viewportCount,
            VkViewport* pViewports
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetScissor", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdSetScissor(
            VkCommandBuffer commandBuffer,
            uint firstScissor,
            uint scissorCount,
            VkRect2D* pScissors
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetLineWidth", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdSetLineWidth(
            VkCommandBuffer commandBuffer,
            float lineWidth
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetDepthBias", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdSetDepthBias(
            VkCommandBuffer commandBuffer,
            float depthBiasConstantFactor,
            float depthBiasClamp,
            float depthBiasSlopeFactor
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetBlendConstants", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdSetBlendConstants(
            VkCommandBuffer commandBuffer,
            float* blendConstants
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetDepthBounds", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdSetDepthBounds(
            VkCommandBuffer commandBuffer,
            float minDepthBounds,
            float maxDepthBounds
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetStencilCompareMask", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdSetStencilCompareMask(
            VkCommandBuffer commandBuffer,
            VkStencilFaceFlags faceMask,
            uint compareMask
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetStencilWriteMask", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdSetStencilWriteMask(
            VkCommandBuffer commandBuffer,
            VkStencilFaceFlags faceMask,
            uint writeMask
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetStencilReference", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdSetStencilReference(
            VkCommandBuffer commandBuffer,
            VkStencilFaceFlags faceMask,
            uint reference
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindDescriptorSets", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdBindDescriptorSets(
            VkCommandBuffer commandBuffer,
            VkPipelineBindPoint pipelineBindPoint,
            VkPipelineLayout layout,
            uint firstSet,
            uint descriptorSetCount,
            VkDescriptorSet* pDescriptorSets,
            uint dynamicOffsetCount,
            uint* pDynamicOffsets
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindIndexBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdBindIndexBuffer(
            VkCommandBuffer commandBuffer,
            VkBuffer buffer,
            VkDeviceSize offset,
            VkIndexType indexType
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBindVertexBuffers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdBindVertexBuffers(
            VkCommandBuffer commandBuffer,
            uint firstBinding,
            uint bindingCount,
            VkBuffer* pBuffers,
            VkDeviceSize* pOffsets
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDraw", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdDraw(
            VkCommandBuffer commandBuffer,
            uint vertexCount,
            uint instanceCount,
            uint firstVertex,
            uint firstInstance
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDrawIndexed", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdDrawIndexed(
            VkCommandBuffer commandBuffer,
            uint indexCount,
            uint instanceCount,
            uint firstIndex,
            int vertexOffset,
            uint firstInstance
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDrawIndirect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdDrawIndirect(
            VkCommandBuffer commandBuffer,
            VkBuffer buffer,
            VkDeviceSize offset,
            uint drawCount,
            uint stride
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDrawIndexedIndirect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdDrawIndexedIndirect(
            VkCommandBuffer commandBuffer,
            VkBuffer buffer,
            VkDeviceSize offset,
            uint drawCount,
            uint stride
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDispatch", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdDispatch(
            VkCommandBuffer commandBuffer,
            uint groupCountX,
            uint groupCountY,
            uint groupCountZ
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdDispatchIndirect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdDispatchIndirect(
            VkCommandBuffer commandBuffer,
            VkBuffer buffer,
            VkDeviceSize offset
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdCopyBuffer(
            VkCommandBuffer commandBuffer,
            VkBuffer srcBuffer,
            VkBuffer dstBuffer,
            uint regionCount,
            VkBufferCopy* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdCopyImage(
            VkCommandBuffer commandBuffer,
            VkImage srcImage,
            VkImageLayout srcImageLayout,
            VkImage dstImage,
            VkImageLayout dstImageLayout,
            uint regionCount,
            VkImageCopy* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBlitImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdBlitImage(
            VkCommandBuffer commandBuffer,
            VkImage srcImage,
            VkImageLayout srcImageLayout,
            VkImage dstImage,
            VkImageLayout dstImageLayout,
            uint regionCount,
            VkImageBlit* pRegions,
            VkFilter filter
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyBufferToImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdCopyBufferToImage(
            VkCommandBuffer commandBuffer,
            VkBuffer srcBuffer,
            VkImage dstImage,
            VkImageLayout dstImageLayout,
            uint regionCount,
            VkBufferImageCopy* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyImageToBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdCopyImageToBuffer(
            VkCommandBuffer commandBuffer,
            VkImage srcImage,
            VkImageLayout srcImageLayout,
            VkBuffer dstBuffer,
            uint regionCount,
            VkBufferImageCopy* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdUpdateBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdUpdateBuffer(
            VkCommandBuffer commandBuffer,
            VkBuffer dstBuffer,
            VkDeviceSize dstOffset,
            VkDeviceSize dataSize,
            void* pData
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdFillBuffer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdFillBuffer(
            VkCommandBuffer commandBuffer,
            VkBuffer dstBuffer,
            VkDeviceSize dstOffset,
            VkDeviceSize size,
            uint data
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdClearColorImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdClearColorImage(
            VkCommandBuffer commandBuffer,
            VkImage image,
            VkImageLayout imageLayout,
            VkClearColorValue* pColor,
            uint rangeCount,
            VkImageSubresourceRange* pRanges
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdClearDepthStencilImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdClearDepthStencilImage(
            VkCommandBuffer commandBuffer,
            VkImage image,
            VkImageLayout imageLayout,
            VkClearDepthStencilValue* pDepthStencil,
            uint rangeCount,
            VkImageSubresourceRange* pRanges
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdClearAttachments", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdClearAttachments(
            VkCommandBuffer commandBuffer,
            uint attachmentCount,
            VkClearAttachment* pAttachments,
            uint rectCount,
            VkClearRect* pRects
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdResolveImage", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdResolveImage(
            VkCommandBuffer commandBuffer,
            VkImage srcImage,
            VkImageLayout srcImageLayout,
            VkImage dstImage,
            VkImageLayout dstImageLayout,
            uint regionCount,
            VkImageResolve* pRegions
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdSetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdSetEvent(
            VkCommandBuffer commandBuffer,
            VkEvent @event,
            VkPipelineStageFlags stageMask
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdResetEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdResetEvent(
            VkCommandBuffer commandBuffer,
            VkEvent @event,
            VkPipelineStageFlags stageMask
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdWaitEvents", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdWaitEvents(
            VkCommandBuffer commandBuffer,
            uint eventCount,
            VkEvent* pEvents,
            VkPipelineStageFlags srcStageMask,
            VkPipelineStageFlags dstStageMask,
            uint memoryBarrierCount,
            VkMemoryBarrier* pMemoryBarriers,
            uint bufferMemoryBarrierCount,
            VkBufferMemoryBarrier* pBufferMemoryBarriers,
            uint imageMemoryBarrierCount,
            VkImageMemoryBarrier* pImageMemoryBarriers
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdPipelineBarrier", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdPipelineBarrier(
            VkCommandBuffer commandBuffer,
            VkPipelineStageFlags srcStageMask,
            VkPipelineStageFlags dstStageMask,
            VkDependencyFlags dependencyFlags,
            uint memoryBarrierCount,
            VkMemoryBarrier* pMemoryBarriers,
            uint bufferMemoryBarrierCount,
            VkBufferMemoryBarrier* pBufferMemoryBarriers,
            uint imageMemoryBarrierCount,
            VkImageMemoryBarrier* pImageMemoryBarriers
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBeginQuery", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdBeginQuery(
            VkCommandBuffer commandBuffer,
            VkQueryPool queryPool,
            uint query,
            VkQueryControlFlags flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdEndQuery", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdEndQuery(
            VkCommandBuffer commandBuffer,
            VkQueryPool queryPool,
            uint query
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdResetQueryPool", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdResetQueryPool(
            VkCommandBuffer commandBuffer,
            VkQueryPool queryPool,
            uint firstQuery,
            uint queryCount
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdWriteTimestamp", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdWriteTimestamp(
            VkCommandBuffer commandBuffer,
            VkPipelineStageFlags pipelineStage,
            VkQueryPool queryPool,
            uint query
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdCopyQueryPoolResults", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdCopyQueryPoolResults(
            VkCommandBuffer commandBuffer,
            VkQueryPool queryPool,
            uint firstQuery,
            uint queryCount,
            VkBuffer dstBuffer,
            VkDeviceSize dstOffset,
            VkDeviceSize stride,
            VkQueryResultFlags flags
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdPushConstants", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdPushConstants(
            VkCommandBuffer commandBuffer,
            VkPipelineLayout layout,
            VkShaderStageFlags stageFlags,
            uint offset,
            uint size,
            void* pValues
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdBeginRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdBeginRenderPass(
            VkCommandBuffer commandBuffer,
            VkRenderPassBeginInfo* pRenderPassBegin,
            VkSubpassContents contents
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdNextSubpass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdNextSubpass(
            VkCommandBuffer commandBuffer,
            VkSubpassContents contents
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdEndRenderPass", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdEndRenderPass(
            VkCommandBuffer commandBuffer
        );

        [DllImport("Vulkan-1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, EntryPoint = "vkCmdExecuteCommands", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CmdExecuteCommands(
            VkCommandBuffer commandBuffer,
            uint commandBufferCount,
            VkCommandBuffer* pCommandBuffers
        );

        public static uint MAKE_VERSION(uint major, uint minor, uint patch)
        {
            return ((major & 0x03FF) << 22) | ((minor & 0x03FF) << 12) | (patch & 0x0FFF);
        }

        public static uint VERSION_MAJOR(uint version)
        {
            return version >> 22;
        }

        public static uint VERSION_MINOR(uint version)
        {
            return (version >> 12) & 0x03FF;
        }

        public static uint VERSION_PATCH(uint version)
        {
            return version & 0x0FFF;
        }
        #endregion
    }
}
