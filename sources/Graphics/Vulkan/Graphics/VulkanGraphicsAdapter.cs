// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsAdapter : GraphicsAdapter
{
    private readonly VkPhysicalDevice _vkPhysicalDevice;
    private readonly VkPhysicalDeviceProperties _vkPhysicalDeviceProperties;
    private readonly VkPhysicalDeviceMemoryProperties _vkPhysicalDeviceMemoryProperties;
    private readonly string _name;

    private VolatileState _state;

    internal VulkanGraphicsAdapter(VulkanGraphicsService service, VkPhysicalDevice vkPhysicalDevice)
        : base(service)
    {
        AssertNotNull(vkPhysicalDevice);
 
        _vkPhysicalDevice = vkPhysicalDevice;

        _vkPhysicalDeviceMemoryProperties = GetVkPhysicalDeviceMemoryProperties(vkPhysicalDevice);
        _vkPhysicalDeviceProperties = GetVkPhysicalDeviceProperties(vkPhysicalDevice);
        _name = GetName(in _vkPhysicalDeviceProperties);

        _ = _state.Transition(to: Initialized);

        static string GetName(in VkPhysicalDeviceProperties vulkanPhysicalDeviceProperties)
        {
            var name = GetUtf8Span(in vulkanPhysicalDeviceProperties.deviceName[0], 256).GetString();
            return name ?? string.Empty;
        }

        static VkPhysicalDeviceMemoryProperties GetVkPhysicalDeviceMemoryProperties(VkPhysicalDevice vkPhysicalDevice)
        {
            VkPhysicalDeviceMemoryProperties physicalDeviceMemoryProperties;
            vkGetPhysicalDeviceMemoryProperties(vkPhysicalDevice, &physicalDeviceMemoryProperties);
            return physicalDeviceMemoryProperties;
        }

        static VkPhysicalDeviceProperties GetVkPhysicalDeviceProperties(VkPhysicalDevice vkPhysicalDevice)
        {
            VkPhysicalDeviceProperties physicalDeviceProperties;
            vkGetPhysicalDeviceProperties(vkPhysicalDevice, &physicalDeviceProperties);
            return physicalDeviceProperties;
        }
    }

    /// <inheritdoc />
    public override uint DeviceId => VkPhysicalDeviceProperties.deviceID;

    /// <inheritdoc />
    public override string Name => _name;

    /// <inheritdoc cref="GraphicsAdapter.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc />
    public override uint VendorId => VkPhysicalDeviceProperties.vendorID;

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkPhysicalDevice" /> for the adapter.</summary>
    public VkPhysicalDevice VkPhysicalDevice
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkPhysicalDevice;
        }
    }

    /// <summary>Gets the <see cref="Interop.Vulkan.VkPhysicalDeviceProperties" /> for <see cref="VkPhysicalDevice" />.</summary>
    public ref readonly VkPhysicalDeviceMemoryProperties VkPhysicalDeviceMemoryProperties => ref _vkPhysicalDeviceMemoryProperties;

    /// <summary>Gets the <see cref="Interop.Vulkan.VkPhysicalDeviceProperties" /> for <see cref="VkPhysicalDevice" />.</summary>
    public ref readonly VkPhysicalDeviceProperties VkPhysicalDeviceProperties => ref _vkPhysicalDeviceProperties;

    /// <inheritdoc />
    public override VulkanGraphicsDevice CreateDevice()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsAdapter));
        return new VulkanGraphicsDevice(this);
    }

    /// <inheritdoc />
    /// <remarks>While there are no unmanaged resources to cleanup, we still want to mark the instance as disposed if, for example, <see cref="GraphicsAdapter.Service" /> was disposed.</remarks>
    protected override void Dispose(bool isDisposing)
    {
        _ = _state.BeginDispose();
        _state.EndDispose();
    }
}
