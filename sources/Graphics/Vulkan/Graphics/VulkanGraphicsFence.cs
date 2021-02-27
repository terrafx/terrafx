// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Threading;
using static TerraFX.Utilities.VulkanUtilities;
using static TerraFX.Interop.VkFenceCreateFlagBits;
using static TerraFX.Interop.VkResult;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsFence : GraphicsFence
    {
        private ValueLazy<VkFence> _vulkanFence;

        private VolatileState _state;

        internal VulkanGraphicsFence(VulkanGraphicsDevice device)
            : base(device)
        {
            _vulkanFence = new ValueLazy<VkFence>(CreateVulkanFence);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsFence" /> class.</summary>
        ~VulkanGraphicsFence() => Dispose(isDisposing: false);

        /// <inheritdoc cref="GraphicsDeviceObject.Device" />
        public new VulkanGraphicsDevice Device => (VulkanGraphicsDevice)base.Device;

        /// <summary>Gets the underlying <see cref="VkFence" /> for the fence.</summary>
        /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
        public VkFence VulkanFence => _vulkanFence.Value;

        /// <inheritdoc />
        public override bool IsSignalled => vkGetFenceStatus(Device.VulkanDevice, VulkanFence) == VK_SUCCESS;

        /// <inheritdoc />
        public override void Reset()
        {
            var vulkanFence = VulkanFence;
            ThrowExternalExceptionIfNotSuccess(vkResetFences(Device.VulkanDevice, fenceCount: 1, (ulong*)&vulkanFence), nameof(vkResetFences));
        }

        /// <inheritdoc />
        public override bool TryWait(int millisecondsTimeout = -1)
        {
            Assert(AssertionsEnabled && (millisecondsTimeout >= Timeout.Infinite));
            return TryWait(unchecked((ulong)millisecondsTimeout));
        }

        /// <inheritdoc />
        public override bool TryWait(TimeSpan timeout)
        {
            var millisecondsTimeout = (long)timeout.TotalMilliseconds;
            Assert(AssertionsEnabled && (millisecondsTimeout >= Timeout.Infinite));
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
            ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsFence));

            VkFence vulkanFence;

            var fenceCreateInfo = new VkFenceCreateInfo {
                sType = VK_STRUCTURE_TYPE_FENCE_CREATE_INFO,
                pNext = null,
                flags = (uint)VK_FENCE_CREATE_SIGNALED_BIT,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateFence(Device.VulkanDevice, &fenceCreateInfo, pAllocator: null, (ulong*)&vulkanFence), nameof(vkCreateFence));

            return vulkanFence;
        }

        private void DisposeVulkanFence(VkFence vulkanFence)
        {
            AssertDisposing(_state);

            if (vulkanFence != VK_NULL_HANDLE)
            {
                vkDestroyFence(Device.VulkanDevice, vulkanFence, pAllocator: null);
            }
        }

        private bool TryWait(ulong millisecondsTimeout)
        {
            var fenceSignalled = IsSignalled;

            if (!fenceSignalled)
            {
                var vulkanFence = VulkanFence;
                var result = vkWaitForFences(Device.VulkanDevice, fenceCount: 1, (ulong*)&vulkanFence, waitAll: VK_TRUE, millisecondsTimeout);

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
