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

        internal VulkanGraphicsHeap(VulkanGraphicsDevice graphicsDevice, ulong size, GraphicsHeapCpuAccess cpuAccess)
            : base(graphicsDevice, size, cpuAccess)
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
            var nonCoherentAtomSize = 64 * 1024ul;

            var offset = _offset;
            size = (size + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);
            _offset = offset + size;

            return new VulkanGraphicsBuffer(kind, this, offset, size, stride);
        }

        /// <inheritdoc cref="CreateGraphicsTexture(GraphicsTextureKind, ulong, uint, ushort)" />
        public VulkanGraphicsTexture CreateVulkanGraphicsTexture(GraphicsTextureKind kind, ulong width, uint height, ushort depth)
        {
            _state.ThrowIfDisposedOrDisposing();

            var vulkanGraphicsAdapter = VulkanGraphicsDevice.VulkanGraphicsAdapter;
            var nonCoherentAtomSize = 64 * 1024ul;

            var size = width * height * depth * sizeof(uint);
            var offset = _offset;

            size = (size + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);
            _offset = offset + size;

            return new VulkanGraphicsTexture(kind, this, offset, size, width, height, depth);
        }

        /// <inheritdoc />
        public override GraphicsBuffer CreateGraphicsBuffer(GraphicsBufferKind kind, ulong size, ulong stride) => CreateVulkanGraphicsBuffer(kind, size, stride);

        /// <inheritdoc />
        public override GraphicsTexture CreateGraphicsTexture(GraphicsTextureKind kind, ulong width, uint height, ushort depth) => CreateVulkanGraphicsTexture(kind, width, height, depth);

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
                memoryTypeIndex = GetMemoryTypeIndex(VulkanGraphicsDevice.VulkanGraphicsAdapter.VulkanPhysicalDevice, CpuAccess),
            };
            ThrowExternalExceptionIfNotSuccess(nameof(vkAllocateMemory), vkAllocateMemory(VulkanGraphicsDevice.VulkanDevice, &memoryAllocateInfo, pAllocator: null, (ulong*)&vulkanDeviceMemory));

            return vulkanDeviceMemory;


            static uint GetMemoryTypeIndex(VkPhysicalDevice vulkanPhysicalDevice, GraphicsHeapCpuAccess cpuAccess)
            {
                var memoryTypeIndex = uint.MaxValue;
                var memoryTypeHasDesiredFlag = false;
                var memoryTypeHasUndesiredFlag = false;

                VkPhysicalDeviceMemoryProperties physicalDeviceMemoryProperties;
                vkGetPhysicalDeviceMemoryProperties(vulkanPhysicalDevice, &physicalDeviceMemoryProperties);

                var memoryTypesCount = physicalDeviceMemoryProperties.memoryTypeCount;
                var memoryTypes = physicalDeviceMemoryProperties.memoryTypes;

                var (requiredFlag, desiredFlag, undesiredFlag) = cpuAccess switch {
                    GraphicsHeapCpuAccess.Read => (VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT, VK_MEMORY_PROPERTY_HOST_CACHED_BIT, default(VkMemoryPropertyFlagBits)),
                    GraphicsHeapCpuAccess.Write => (VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT, default(VkMemoryPropertyFlagBits), VK_MEMORY_PROPERTY_HOST_COHERENT_BIT),
                    _ => (default(VkMemoryPropertyFlagBits), VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT, VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT),
                };

                for (uint index = 0; index < memoryTypesCount; index++)
                {
                    var propertyFlags = (VkMemoryPropertyFlagBits)memoryTypes[unchecked((int)index)].propertyFlags;

                    var hasRequiredFlag = (propertyFlags & requiredFlag) == requiredFlag;
                    var hasDesiredFlag = (propertyFlags & desiredFlag) == desiredFlag;
                    var hasUndesiredFlag = (propertyFlags & undesiredFlag) == undesiredFlag;

                    if (hasRequiredFlag)
                    {
                        if (hasDesiredFlag)
                        {
                            if (!hasUndesiredFlag)
                            {
                                // We have the required flag, the desired flag, and do not
                                // have the undesired flag; so we've found the best match.

                                memoryTypeIndex = index;
                                break;
                            }

                            if (!memoryTypeHasDesiredFlag)
                            {
                                // We have the required flag and the desired flag, but also the
                                // undesired flag. But, the last match did not have the desired
                                // flag, so we'll treat this as a better match.

                                memoryTypeIndex = index;
                                memoryTypeHasDesiredFlag = true;
                                memoryTypeHasUndesiredFlag = true;
                            }
                        }
                        else if (!memoryTypeHasDesiredFlag && ((!hasUndesiredFlag && memoryTypeHasUndesiredFlag) || (memoryTypeIndex == uint.MaxValue)))
                        {
                            // We have the required flag and do not have the desired flag, but
                            // the last match didn't have the desired flag either. So, if the last
                            // match had the undesired flag but we don't, then we'll treat this
                            // as a better match. We'll likewise treat this as a better match if
                            // we haven't found a match yet.

                            memoryTypeIndex = index;
                            memoryTypeHasUndesiredFlag = hasUndesiredFlag;
                        }
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
