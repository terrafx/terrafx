// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkMemoryPropertyFlagBits;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsHeap : GraphicsHeap
    {
        private ulong _offset;
        private ValueLazy<VkDeviceMemory> _vulkanDeviceMemory;
        private State _state;

        internal VulkanGraphicsHeap(VulkanGraphicsDevice graphicsDevice, ulong size)
            : base(graphicsDevice, size)
        {
            _vulkanDeviceMemory = new ValueLazy<VkDeviceMemory>(CreateVulkanDeviceMemory);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsHeap" /> class.</summary>
        ~VulkanGraphicsHeap()
        {
            Dispose(isDisposing: true);
        }

        /// <summary>Gets the underlying <see cref="VkDeviceMemory" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkAllocateMemory(IntPtr, VkMemoryAllocateInfo*, VkAllocationCallbacks*, ulong*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
        public VkDeviceMemory VulkanDeviceMemory => _vulkanDeviceMemory.Value;

        /// <inheritdoc cref="GraphicsHeap.GraphicsDevice" />
        public VulkanGraphicsDevice VulkanGraphicsDevice => (VulkanGraphicsDevice)GraphicsDevice;

        /// <inheritdoc cref="CreateGraphicsBuffer(GraphicsBufferKind, ulong, ulong)" />
        public VulkanGraphicsBuffer CreateVulkanGraphicsBuffer(GraphicsBufferKind kind, ulong size, ulong stride)
        {
            _state.ThrowIfDisposedOrDisposing();

            var vulkanGraphicsAdapter = VulkanGraphicsDevice.VulkanGraphicsAdapter;
            var nonCoherentAtomSize = vulkanGraphicsAdapter.VulkanPhysicalDeviceProperties.limits.nonCoherentAtomSize;

            var offset = _offset;
            size = (size + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);
            _offset = offset + size;

            return new VulkanGraphicsBuffer(kind, this, offset, size, stride);
        }

        /// <inheritdoc />
        public override GraphicsBuffer CreateGraphicsBuffer(GraphicsBufferKind kind, ulong size, ulong stride) => CreateVulkanGraphicsBuffer(kind, size, stride);

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _vulkanDeviceMemory.Dispose(DisposeVulkanDeviceMemory);
            }

            _state.EndDispose();
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

        private void DisposeVulkanDeviceMemory(VkDeviceMemory vulkanDeviceMemory)
        {
            if (vulkanDeviceMemory != VK_NULL_HANDLE)
            {
                vkFreeMemory(VulkanGraphicsDevice.VulkanDevice, vulkanDeviceMemory, pAllocator: null);
            }
        }
    }
}
