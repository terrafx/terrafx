// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkFenceCreateFlags;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkResult;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsFence : GraphicsFence
{
    private VkFence _vkFence;

    internal VulkanGraphicsFence(VulkanGraphicsDevice device, in GraphicsFenceCreateOptions createOptions) : base(device)
    {
        device.AddFence(this);

        FenceInfo.IsSignalled = createOptions.IsSignalled;

        _vkFence = CreateVkFence(in createOptions);

        SetNameUnsafe(Name);

        VkFence CreateVkFence(in GraphicsFenceCreateOptions createOptions)
        {
            VkFence vkFence;

            var vkFenceCreateInfo = new VkFenceCreateInfo {
                sType = VK_STRUCTURE_TYPE_FENCE_CREATE_INFO,
                pNext = null,
                flags = createOptions.IsSignalled ? VK_FENCE_CREATE_SIGNALED_BIT : 0,
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
    public VkFence VkFence => _vkFence;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        DisposeVkFence(Device.VkDevice, _vkFence);
        _vkFence = VkFence.NULL;

        _ = Device.RemoveFence(this);

        static void DisposeVkFence(VkDevice vkDevice, VkFence vkFence)
        {
            if (vkFence != VkFence.NULL)
            {
                vkDestroyFence(vkDevice, vkFence, pAllocator: null);
            }
        }
    }

    /// <inheritdoc />
    protected override void ResetUnsafe()
    {
        var vkFence = VkFence;
        ThrowExternalExceptionIfNotSuccess(vkResetFences(Device.VkDevice, fenceCount: 1, &vkFence));
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_FENCE, VkFence, value);
    }

    /// <inheritdoc />
    protected override bool TryWaitUnsafe(uint millisecondsTimeout)
    {
        var isSignalled = false;

        var vulkanFence = VkFence;
        var result = vkWaitForFences(Device.VkDevice, fenceCount: 1, &vulkanFence, waitAll: VK_TRUE, millisecondsTimeout);

        if (result == VK_SUCCESS)
        {
            isSignalled = true;
        }
        else if (result != VK_TIMEOUT)
        {
            ThrowExternalException(nameof(vkWaitForFences), (int)result);
        }

        return isSignalled;
    }
}
