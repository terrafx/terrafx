// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkBufferUsageFlagBits;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsBuffer : GraphicsBuffer
    {
        private ValueLazy<VkBuffer> _vulkanBuffer;
        private State _state;

        internal VulkanGraphicsBuffer(GraphicsBufferKind kind, VulkanGraphicsHeap graphicsHeap, ulong offset, ulong size, ulong stride)
            : base(kind, graphicsHeap, offset, size, stride)
        {
            _vulkanBuffer = new ValueLazy<VkBuffer>(CreateVulkanBuffer);
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

        /// <inheritdoc cref="GraphicsResource.GraphicsHeap" />
        public VulkanGraphicsHeap VulkanGraphicsHeap => (VulkanGraphicsHeap)GraphicsHeap;

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(IntPtr, ulong, ulong, ulong, uint, void**)" /> failed.</exception>
        public override T* Map<T>(UIntPtr readRangeOffset, UIntPtr readRangeLength)
        {
            var vulkanGraphicsHeap = VulkanGraphicsHeap;
            var vulkanDeviceMemory = vulkanGraphicsHeap.VulkanDeviceMemory;

            var vulkanGraphicsDevice = vulkanGraphicsHeap.VulkanGraphicsDevice;
            var vulkanDevice = vulkanGraphicsDevice.VulkanDevice;

            void* pDestination;
            ThrowExternalExceptionIfNotSuccess(nameof(vkMapMemory), vkMapMemory(vulkanDevice, vulkanDeviceMemory, Offset, Size, flags: 0, &pDestination));

            if (readRangeLength != UIntPtr.Zero)
            {
                var vulkanGraphicsAdapter = vulkanGraphicsDevice.VulkanGraphicsAdapter;
                var nonCoherentAtomSize = vulkanGraphicsAdapter.VulkanPhysicalDeviceProperties.limits.nonCoherentAtomSize;

                var offset = Offset + (ulong)readRangeOffset;
                var size = ((ulong)readRangeLength + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

                var mappedMemoryRange = new VkMappedMemoryRange {
                    sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                    memory = vulkanDeviceMemory,
                    offset = offset,
                    size = size,
                };
                ThrowExternalExceptionIfNotSuccess(nameof(vkInvalidateMappedMemoryRanges), vkInvalidateMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange));
            }
            return (T*)pDestination;
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(IntPtr, uint, VkMappedMemoryRange*)" /> failed.</exception>
        public override void Unmap(UIntPtr writtenRangeOffset, UIntPtr writtenRangeLength)
        {
            var vulkanGraphicsHeap = VulkanGraphicsHeap;
            var vulkanDeviceMemory = vulkanGraphicsHeap.VulkanDeviceMemory;

            var vulkanGraphicsDevice = vulkanGraphicsHeap.VulkanGraphicsDevice;
            var vulkanDevice = vulkanGraphicsDevice.VulkanDevice;

            if (writtenRangeLength != UIntPtr.Zero)
            {
                var vulkanGraphicsAdapter = vulkanGraphicsDevice.VulkanGraphicsAdapter;
                var nonCoherentAtomSize = vulkanGraphicsAdapter.VulkanPhysicalDeviceProperties.limits.nonCoherentAtomSize;

                var offset = Offset + (ulong)writtenRangeOffset;
                var size = ((ulong)writtenRangeLength + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

                var mappedMemoryRange = new VkMappedMemoryRange {
                    sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                    memory = vulkanDeviceMemory,
                    offset = offset,
                    size = size,
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
            }

            _state.EndDispose();
        }

        private VkBuffer CreateVulkanBuffer()
        {
            _state.ThrowIfDisposedOrDisposing();

            VkBuffer vulkanBuffer;

            var vulkanGraphicsHeap = VulkanGraphicsHeap;
            var vulkanDeviceMemory = vulkanGraphicsHeap.VulkanDeviceMemory;

            var vulkanGraphicsDevice = vulkanGraphicsHeap.VulkanGraphicsDevice;
            var vulkanDevice = vulkanGraphicsDevice.VulkanDevice;

            var bufferCreateInfo = new VkBufferCreateInfo {
                sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
                size = Size,
                usage = GetVulkanBufferUsageKind(vulkanGraphicsHeap.CpuAccess, Kind),
            };

            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateBuffer), vkCreateBuffer(vulkanDevice, &bufferCreateInfo, pAllocator: null, (ulong*)&vulkanBuffer));
            ThrowExternalExceptionIfNotSuccess(nameof(vkBindBufferMemory), vkBindBufferMemory(vulkanDevice, vulkanBuffer, vulkanDeviceMemory, Offset));

            return vulkanBuffer;

            static uint GetVulkanBufferUsageKind(GraphicsHeapCpuAccess cpuAccess, GraphicsBufferKind kind)
            {
                var vulkanBufferUsageKind = cpuAccess switch {
                    GraphicsHeapCpuAccess.Read => VK_BUFFER_USAGE_TRANSFER_DST_BIT,
                    GraphicsHeapCpuAccess.Write => VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                    _ => VK_BUFFER_USAGE_TRANSFER_DST_BIT | VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                };

                if (cpuAccess != GraphicsHeapCpuAccess.Read)
                {
                    vulkanBufferUsageKind |= kind switch {
                        GraphicsBufferKind.Vertex => VK_BUFFER_USAGE_VERTEX_BUFFER_BIT,
                        GraphicsBufferKind.Index => VK_BUFFER_USAGE_INDEX_BUFFER_BIT,
                        GraphicsBufferKind.Constant => VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT,
                        _ => default,
                    };
                }

                return (uint)vulkanBufferUsageKind;
            }
        }

        private void DisposeVulkanBuffer(VkBuffer vulkanBuffer)
        {
            if (vulkanBuffer != VK_NULL_HANDLE)
            {
                vkDestroyBuffer(VulkanGraphicsHeap.VulkanGraphicsDevice.VulkanDevice, vulkanBuffer, pAllocator: null);
            }
        }
    }
}
