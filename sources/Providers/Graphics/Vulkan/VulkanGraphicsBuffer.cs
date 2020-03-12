// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkBufferUsageFlagBits;
using static TerraFX.Interop.VkMemoryPropertyFlagBits;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsBuffer : GraphicsBuffer
    {
        private ValueLazy<VkBuffer> _vulkanBuffer;
        private ValueLazy<VkDeviceMemory> _vulkanDeviceMemory;

        private State _state;

        internal VulkanGraphicsBuffer(GraphicsBufferKind kind, VulkanGraphicsDevice graphicsDevice, ulong size, ulong stride)
            : base(kind, graphicsDevice, size, stride)
        {
            _vulkanBuffer = new ValueLazy<VkBuffer>(CreateVulkanBuffer);
            _vulkanDeviceMemory = new ValueLazy<VkDeviceMemory>(CreateVulkanDeviceMemory);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsBuffer" /> class.</summary>
        ~VulkanGraphicsBuffer()
        {
            Dispose(isDisposing: true);
        }

        /// <summary>Gets the underlying <see cref="VkBuffer" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateBuffer(IntPtr, VkBufferCreateInfo*, VkAllocationCallbacks*, ulong*)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="vkBindBufferMemory(IntPtr, ulong, ulong, ulong)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
        public VkBuffer VulkanBuffer => _vulkanBuffer.Value;

        /// <summary>Gets the underlying <see cref="VkDeviceMemory" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkAllocateMemory(IntPtr, VkMemoryAllocateInfo*, VkAllocationCallbacks*, ulong*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
        public VkDeviceMemory VulkanDeviceMemory => _vulkanDeviceMemory.Value;

        /// <inheritdoc cref="GraphicsResource.GraphicsDevice" />
        public VulkanGraphicsDevice VulkanGraphicsDevice => (VulkanGraphicsDevice)GraphicsDevice;

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(IntPtr, ulong, ulong, ulong, uint, void**)" /> failed.</exception>
        public override T* Map<T>(UIntPtr readRangeOffset, UIntPtr readRangeLength)
        {
            var vulkanDevice = VulkanGraphicsDevice.VulkanDevice;
            var vulkanDeviceMemory = VulkanDeviceMemory;

            void* pDestination;
            ThrowExternalExceptionIfNotSuccess(nameof(vkMapMemory), vkMapMemory(vulkanDevice, vulkanDeviceMemory, offset: 0, size: VK_WHOLE_SIZE, flags: 0, &pDestination));

            if (readRangeLength != UIntPtr.Zero)
            {
                var mappedMemoryRange = new VkMappedMemoryRange {
                    sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                    memory = vulkanDeviceMemory,
                    offset = (ulong)readRangeOffset,
                    size = (ulong)readRangeLength,
                };
                ThrowExternalExceptionIfNotSuccess(nameof(vkInvalidateMappedMemoryRanges), vkInvalidateMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange));
            }
            return (T*)pDestination;
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(IntPtr, uint, VkMappedMemoryRange*)" /> failed.</exception>
        public override void Unmap(UIntPtr writtenRangeOffset, UIntPtr writtenRangeLength)
        {
            var vulkanDevice = VulkanGraphicsDevice.VulkanDevice;
            var vulkanDeviceMemory = VulkanDeviceMemory;

            if (writtenRangeLength != UIntPtr.Zero)
            {
                var mappedMemoryRange = new VkMappedMemoryRange {
                    sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                    memory = vulkanDeviceMemory,
                    offset = (ulong)writtenRangeOffset,
                    size = (ulong)writtenRangeLength,
                };
                ThrowExternalExceptionIfNotSuccess(nameof(vkFlushMappedMemoryRanges), vkFlushMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange));
            }
            vkUnmapMemory(vulkanDevice, vulkanDeviceMemory);
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _vulkanBuffer.Dispose(DisposeVulkanBuffer);
                _vulkanDeviceMemory.Dispose(DisposeVulkanDeviceMemory);
            }

            _state.EndDispose();
        }

        private VkBuffer CreateVulkanBuffer()
        {
            _state.ThrowIfDisposedOrDisposing();

            VkBuffer vulkanBuffer;

            var bufferCreateInfo = new VkBufferCreateInfo {
                sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
                size = Size,
                usage = GetVulkanBufferUsageKind(Kind),
            };
            var vulkanDevice = VulkanGraphicsDevice.VulkanDevice;

            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateBuffer), vkCreateBuffer(vulkanDevice, &bufferCreateInfo, pAllocator: null, (ulong*)&vulkanBuffer));
            ThrowExternalExceptionIfNotSuccess(nameof(vkBindBufferMemory), vkBindBufferMemory(vulkanDevice, vulkanBuffer, VulkanDeviceMemory, memoryOffset: 0));

            return vulkanBuffer;
        }

        private VkDeviceMemory CreateVulkanDeviceMemory()
        {
            _state.ThrowIfDisposedOrDisposing();

            VkDeviceMemory vulkanDeviceMemory;

            var memoryAllocateInfo = new VkMemoryAllocateInfo {
                sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
                allocationSize = Size,
                memoryTypeIndex = GetMemoryTypeIndex(VulkanGraphicsDevice.VulkanGraphicsAdapter.VulkanPhysicalDevice),
            };
            ThrowExternalExceptionIfNotSuccess(nameof(vkAllocateMemory), vkAllocateMemory(VulkanGraphicsDevice.VulkanDevice, &memoryAllocateInfo, pAllocator: null, (ulong*)&vulkanDeviceMemory));

            return vulkanDeviceMemory;


            static uint GetMemoryTypeIndex(VkPhysicalDevice vulkanPhysicalDevice)
            {
                var memoryTypeIndex = uint.MaxValue;

                VkPhysicalDeviceMemoryProperties physicalDeviceMemoryProperties;
                vkGetPhysicalDeviceMemoryProperties(vulkanPhysicalDevice, &physicalDeviceMemoryProperties);

                var memoryTypesCount = physicalDeviceMemoryProperties.memoryTypeCount;
                var memoryTypes = physicalDeviceMemoryProperties.memoryTypes;

                for (uint index = 0; index < memoryTypesCount; index++)
                {
                    if ((memoryTypes[unchecked((int)index)].propertyFlags & (uint)VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT) != 0)
                    {
                        memoryTypeIndex = index;
                        break;
                    }
                }

                if (memoryTypeIndex == uint.MaxValue)
                {
                    ThrowInvalidOperationException(nameof(memoryTypeIndex), memoryTypeIndex);
                }
                return memoryTypeIndex;
            }
        }

        private uint GetVulkanBufferUsageKind(GraphicsBufferKind kind)
        {
            VkBufferUsageFlagBits vulkanBufferUsageKind;

            switch (kind)
            {
                case GraphicsBufferKind.Vertex:
                {
                    vulkanBufferUsageKind = VK_BUFFER_USAGE_VERTEX_BUFFER_BIT;
                    break;
                }

                case GraphicsBufferKind.Index:
                {
                    vulkanBufferUsageKind = VK_BUFFER_USAGE_INDEX_BUFFER_BIT;
                    break;
                }

                case GraphicsBufferKind.Uniform:
                {
                    vulkanBufferUsageKind = VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT;
                    break;
                }

                default:
                {
                    vulkanBufferUsageKind = 0;
                    break;
                }
            }

            return (uint)vulkanBufferUsageKind;
        }

        private void DisposeVulkanBuffer(VkBuffer vulkanBuffer)
        {
            if (vulkanBuffer != VK_NULL_HANDLE)
            {
                vkDestroyBuffer(VulkanGraphicsDevice.VulkanDevice, vulkanBuffer, pAllocator: null);
            }
        }

        private void DisposeVulkanDeviceMemory(VkDeviceMemory vulkanDeviceMemory)
        {
            if (vulkanDeviceMemory != VK_NULL_HANDLE)
            {
                vkFreeMemory(VulkanGraphicsDevice.VulkanDevice, vulkanDeviceMemory, pAllocator: null);
            }
        }
    }
}
