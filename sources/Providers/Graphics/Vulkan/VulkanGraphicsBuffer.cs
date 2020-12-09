// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Utilities.State;
using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public abstract unsafe class VulkanGraphicsBuffer : GraphicsBuffer
    {
        private VkBuffer _vulkanBuffer;
        private protected State _state;

        private protected VulkanGraphicsBuffer(GraphicsBufferKind kind, in GraphicsMemoryRegion<GraphicsMemoryBlock> blockRegion, GraphicsResourceCpuAccess cpuAccess, VkBuffer vulkanBuffer)
            : base(kind, in blockRegion, cpuAccess)
        {
            _vulkanBuffer = vulkanBuffer;
            ThrowExternalExceptionIfNotSuccess(vkBindBufferMemory(Allocator.Device.VulkanDevice, vulkanBuffer, Block.VulkanDeviceMemory, blockRegion.Offset), nameof(vkBindBufferMemory));
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsBuffer{TMetadata}" /> class.</summary>
        ~VulkanGraphicsBuffer() => Dispose(isDisposing: true);

        /// <inheritdoc cref="GraphicsResource.Allocator" />
        public new VulkanGraphicsMemoryAllocator Allocator => (VulkanGraphicsMemoryAllocator)base.Allocator;

        /// <inheritdoc />
        public new VulkanGraphicsMemoryBlock Block => (VulkanGraphicsMemoryBlock)base.Block;

        /// <summary>Gets the underlying <see cref="VkBuffer" /> for the buffer.</summary>
        /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
        public VkBuffer VulkanBuffer
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _vulkanBuffer;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(IntPtr, ulong, ulong, ulong, uint, void**)" /> failed.</exception>
        public override T* Map<T>()
        {
            var device = Allocator.Device;

            var vulkanDevice = device.VulkanDevice;
            var vulkanDeviceMemory = Block.VulkanDeviceMemory;

            byte* pDestination;
            ThrowExternalExceptionIfNotSuccess(vkMapMemory(vulkanDevice, vulkanDeviceMemory, Offset, Size, flags: 0, (void**)&pDestination), nameof(vkMapMemory));

            return (T*)pDestination;
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(IntPtr, ulong, ulong, ulong, uint, void**)" /> failed.</exception>
        public override T* Map<T>(nuint rangeOffset, nuint rangeLength)
        {
            var device = Allocator.Device;

            var vulkanDevice = device.VulkanDevice;
            var vulkanDeviceMemory = Block.VulkanDeviceMemory;

            byte* pDestination;
            ThrowExternalExceptionIfNotSuccess(vkMapMemory(vulkanDevice, vulkanDeviceMemory, Offset, Size, flags: 0, (void**)&pDestination), nameof(vkMapMemory));

            return (T*)(pDestination + rangeOffset);
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(IntPtr, ulong, ulong, ulong, uint, void**)" /> failed.</exception>
        public override T* MapForRead<T>()
        {
            var device = Allocator.Device;

            var vulkanDevice = device.VulkanDevice;
            var vulkanDeviceMemory = Block.VulkanDeviceMemory;

            void* pDestination;
            ThrowExternalExceptionIfNotSuccess(vkMapMemory(vulkanDevice, vulkanDeviceMemory, Offset, Size, flags: 0, &pDestination), nameof(vkMapMemory));

            var nonCoherentAtomSize = device.Adapter.VulkanPhysicalDeviceProperties.limits.nonCoherentAtomSize;

            var offset = Offset;
            var size = (Size + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

            var mappedMemoryRange = new VkMappedMemoryRange {
                sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                memory = vulkanDeviceMemory,
                offset = offset,
                size = size,
            };
            ThrowExternalExceptionIfNotSuccess(vkInvalidateMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange), nameof(vkInvalidateMappedMemoryRanges));

            return (T*)pDestination;
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(IntPtr, ulong, ulong, ulong, uint, void**)" /> failed.</exception>
        public override T* MapForRead<T>(nuint readRangeOffset, nuint readRangeLength)
        {
            var device = Allocator.Device;

            var vulkanDevice = device.VulkanDevice;
            var vulkanDeviceMemory = Block.VulkanDeviceMemory;

            byte* pDestination;
            ThrowExternalExceptionIfNotSuccess(vkMapMemory(vulkanDevice, vulkanDeviceMemory, Offset, Size, flags: 0, (void**)&pDestination), nameof(vkMapMemory));

            if (readRangeLength != 0)
            {
                var nonCoherentAtomSize = device.Adapter.VulkanPhysicalDeviceProperties.limits.nonCoherentAtomSize;

                var offset = Offset + readRangeOffset;
                var size = (readRangeLength + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

                var mappedMemoryRange = new VkMappedMemoryRange {
                    sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                    memory = vulkanDeviceMemory,
                    offset = offset,
                    size = size,
                };
                ThrowExternalExceptionIfNotSuccess(vkInvalidateMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange), nameof(vkInvalidateMappedMemoryRanges));
            }
            return (T*)(pDestination + readRangeOffset);
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(IntPtr, uint, VkMappedMemoryRange*)" /> failed.</exception>
        public override void Unmap()
        {
            var device = Allocator.Device;

            var vulkanDevice = device.VulkanDevice;
            var vulkanDeviceMemory = Block.VulkanDeviceMemory;

            vkUnmapMemory(vulkanDevice, vulkanDeviceMemory);
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(IntPtr, uint, VkMappedMemoryRange*)" /> failed.</exception>
        public override void UnmapAndWrite()
        {
            var device = Allocator.Device;

            var vulkanDevice = device.VulkanDevice;
            var vulkanDeviceMemory = Block.VulkanDeviceMemory;

            var nonCoherentAtomSize = device.Adapter.VulkanPhysicalDeviceProperties.limits.nonCoherentAtomSize;

            var offset = Offset;
            var size = (Size + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

            var mappedMemoryRange = new VkMappedMemoryRange {
                sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                memory = vulkanDeviceMemory,
                offset = offset,
                size = size,
            };
            ThrowExternalExceptionIfNotSuccess(vkFlushMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange), nameof(vkFlushMappedMemoryRanges));

            vkUnmapMemory(vulkanDevice, vulkanDeviceMemory);
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(IntPtr, uint, VkMappedMemoryRange*)" /> failed.</exception>
        public override void UnmapAndWrite(nuint writtenRangeOffset, nuint writtenRangeLength)
        {
            var device = Allocator.Device;

            var vulkanDevice = device.VulkanDevice;
            var vulkanDeviceMemory = Block.VulkanDeviceMemory;

            if (writtenRangeLength != 0)
            {
                var nonCoherentAtomSize = device.Adapter.VulkanPhysicalDeviceProperties.limits.nonCoherentAtomSize;

                var offset = Offset + writtenRangeOffset;
                var size = (writtenRangeLength + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

                var mappedMemoryRange = new VkMappedMemoryRange {
                    sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                    memory = vulkanDeviceMemory,
                    offset = offset,
                    size = size,
                };
                ThrowExternalExceptionIfNotSuccess(vkFlushMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange), nameof(vkFlushMappedMemoryRanges));
            }
            vkUnmapMemory(vulkanDevice, vulkanDeviceMemory);
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeVulkanBuffer(_vulkanBuffer);
                BlockRegion.Collection.Free(in BlockRegion);
            }

            _state.EndDispose();
        }

        private void DisposeVulkanBuffer(VkBuffer vulkanBuffer)
        {
            if (vulkanBuffer != VK_NULL_HANDLE)
            {
                vkDestroyBuffer(Allocator.Device.VulkanDevice, vulkanBuffer, pAllocator: null);
            }
        }
    }
}
