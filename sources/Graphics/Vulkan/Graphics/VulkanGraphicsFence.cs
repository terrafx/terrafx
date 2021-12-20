// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkFenceCreateFlags;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkResult;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsFence : GraphicsFence
{
    private readonly VkFence _vkFence;

    private VolatileState _state;

    internal VulkanGraphicsFence(VulkanGraphicsDevice device, bool isSignalled)
        : base(device)
    {
        _vkFence = CreateVkFence(device, isSignalled ? VK_FENCE_CREATE_SIGNALED_BIT : 0);

        _ = _state.Transition(to: Initialized);

        static VkFence CreateVkFence(VulkanGraphicsDevice device, VkFenceCreateFlags vkFenceCreateFlags)
        {
            VkFence vkFence;

            var vkFenceCreateInfo = new VkFenceCreateInfo {
                sType = VK_STRUCTURE_TYPE_FENCE_CREATE_INFO,
                pNext = null,
                flags = vkFenceCreateFlags,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateFence(device.VkDevice, &vkFenceCreateInfo, pAllocator: null, &vkFence));

            return vkFence;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsFence" /> class.</summary>
    ~VulkanGraphicsFence() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkFence" /> for the fence.</summary>
    public VkFence VkFence
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkFence;
        }
    }

    /// <inheritdoc />
    public override bool IsSignalled => vkGetFenceStatus(Device.VkDevice, VkFence) == VK_SUCCESS;

    /// <inheritdoc />
    public override void Reset()
    {
        var vulkanFence = VkFence;
        ThrowExternalExceptionIfNotSuccess(vkResetFences(Device.VkDevice, fenceCount: 1, &vulkanFence));
    }

    /// <inheritdoc />
    public override void SetName(string value)
    {
        value = Device.UpdateName(VK_OBJECT_TYPE_FENCE, VkFence, value);
        base.SetName(value);
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
            DisposeVkFence(Device.VkDevice, _vkFence);
        }

        _state.EndDispose();

        static void DisposeVkFence(VkDevice vkDevice, VkFence vkFence)
        {
            if (vkFence != VkFence.NULL)
            {
                vkDestroyFence(vkDevice, vkFence, pAllocator: null);
            }
        }
    }    

    private bool TryWait(ulong millisecondsTimeout)
    {
        var fenceSignalled = IsSignalled;

        if (!fenceSignalled)
        {
            var vulkanFence = VkFence;
            var result = vkWaitForFences(Device.VkDevice, fenceCount: 1, &vulkanFence, waitAll: VK_TRUE, millisecondsTimeout);

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
