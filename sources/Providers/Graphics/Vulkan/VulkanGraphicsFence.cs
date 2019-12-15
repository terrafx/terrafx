// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkFenceCreateFlagBits;
using static TerraFX.Interop.VkResult;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsFence : GraphicsFence
    {
        private ValueLazy<VkFence> _vulkanFence;

        private State _state;

        internal VulkanGraphicsFence(GraphicsContext graphicsContext)
            : base(graphicsContext)
        {
            _vulkanFence = new ValueLazy<VkFence>(CreateVulkanFence);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsFence" /> class.</summary>
        ~VulkanGraphicsFence()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the underlying <see cref="VkFence" /> for the fence.</summary>
        /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
        public VkFence VulkanFence => _vulkanFence.Value;

        /// <inheritdoc cref="GraphicsFence.GraphicsContext" />
        public VulkanGraphicsContext VulkanGraphicsContext => (VulkanGraphicsContext)GraphicsContext;

        /// <inheritdoc />
        public override bool IsSignalled => vkGetFenceStatus(VulkanGraphicsContext.Device, VulkanFence) == VK_SUCCESS;

        /// <inheritdoc />
        public override void Reset()
        {
            var vulkanFence = VulkanFence;
            ThrowExternalExceptionIfNotSuccess(nameof(vkResetFences), vkResetFences(VulkanGraphicsContext.Device, fenceCount: 1, (ulong*)&vulkanFence));
        }

        /// <inheritdoc />
        public override bool TryWait(int millisecondsTimeout = -1)
        {
            _state.ThrowIfDisposedOrDisposing();

            if (millisecondsTimeout < Timeout.Infinite)
            {
                ThrowArgumentOutOfRangeException(nameof(millisecondsTimeout), millisecondsTimeout);
            }
            return TryWait(unchecked((ulong)millisecondsTimeout));
        }

        /// <inheritdoc />
        public override bool TryWait(TimeSpan timeout)
        {
            _state.ThrowIfDisposedOrDisposing();

            var millisecondsTimeout = (long)timeout.TotalMilliseconds;

            if (millisecondsTimeout < Timeout.Infinite)
            {
                ThrowArgumentOutOfRangeException(nameof(timeout), timeout);
            }

            return TryWait(unchecked((ulong)millisecondsTimeout));
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _vulkanFence.Dispose(DisposeVulkanFence);
            }

            _state.EndDispose();
        }

        private VkFence CreateVulkanFence()
        {
            _state.ThrowIfDisposedOrDisposing();

            var fenceCreateInfo = new VkFenceCreateInfo {
                sType = VK_STRUCTURE_TYPE_FENCE_CREATE_INFO,
                pNext = null,
                flags = (uint)VK_FENCE_CREATE_SIGNALED_BIT,
            };

            VkFence vulkanFence;
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateFence), vkCreateFence(VulkanGraphicsContext.Device, &fenceCreateInfo, pAllocator: null, (ulong*)&vulkanFence));
            return vulkanFence;
        }

        private void DisposeVulkanFence(VkFence vulkanFence)
        {
            _state.AssertDisposing();

            if (vulkanFence != VK_NULL_HANDLE)
            {
                vkDestroyFence(VulkanGraphicsContext.Device, vulkanFence, pAllocator: null);
            }
        }

        private bool TryWait(ulong millisecondsTimeout)
        {
            _state.AssertNotDisposedOrDisposing();

            var fenceSignalled = IsSignalled;

            if (!fenceSignalled)
            {
                var vulkanFence = VulkanFence;
                var result = vkWaitForFences(VulkanGraphicsContext.Device, fenceCount: 1, (ulong*)&vulkanFence, waitAll: VK_TRUE, millisecondsTimeout);

                if (result == VK_SUCCESS)
                {
                    fenceSignalled = true;
                }
                else if (result != VK_TIMEOUT)
                {
                    ThrowExternalException(nameof(vkWaitForFences), (int)result);
                }
            }

            return fenceSignalled;
        }
    }
}
