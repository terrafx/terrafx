// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Advanced;
using TerraFX.Interop.Vulkan;
using TerraFX.Utilities;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.MarshalUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsAdapter : GraphicsAdapter
{
    private readonly VkPhysicalDevice _vkPhysicalDevice;
    private readonly VkPhysicalDeviceFeatures _vkPhysicalDeviceFeatures;
    private readonly VkPhysicalDeviceMemoryProperties _vkPhysicalDeviceMemoryProperties;
    private readonly VkPhysicalDeviceProperties _vkPhysicalDeviceProperties;

    internal VulkanGraphicsAdapter(VulkanGraphicsService service, VkPhysicalDevice vkPhysicalDevice)
        : base(service)
    {
        AssertNotNull(vkPhysicalDevice);
 
        _vkPhysicalDevice = vkPhysicalDevice;

        _vkPhysicalDeviceFeatures = GetVkPhysicalDeviceFeatures(vkPhysicalDevice);
        _vkPhysicalDeviceProperties = GetVkPhysicalDeviceProperties(vkPhysicalDevice);
        _vkPhysicalDeviceMemoryProperties = GetVkPhysicalDeviceMemoryProperties(vkPhysicalDevice);

        var name = GetName(in _vkPhysicalDeviceProperties);
        SetName(name);

        static string GetName(in VkPhysicalDeviceProperties vulkanPhysicalDeviceProperties)
        {
            var name = GetUtf8Span(in vulkanPhysicalDeviceProperties.deviceName[0], 256).GetString();
            return name ?? string.Empty;
        }

        static VkPhysicalDeviceFeatures GetVkPhysicalDeviceFeatures(VkPhysicalDevice vkPhysicalDevice)
        {
            VkPhysicalDeviceFeatures vkPhysicalDeviceFeatures;
            vkGetPhysicalDeviceFeatures(vkPhysicalDevice, &vkPhysicalDeviceFeatures);
            return vkPhysicalDeviceFeatures;
        }

        static VkPhysicalDeviceMemoryProperties GetVkPhysicalDeviceMemoryProperties(VkPhysicalDevice vkPhysicalDevice)
        {
            VkPhysicalDeviceMemoryProperties vkPhysicalDeviceMemoryProperties;
            vkGetPhysicalDeviceMemoryProperties(vkPhysicalDevice, &vkPhysicalDeviceMemoryProperties);
            return vkPhysicalDeviceMemoryProperties;
        }

        static VkPhysicalDeviceProperties GetVkPhysicalDeviceProperties(VkPhysicalDevice vkPhysicalDevice)
        {
            VkPhysicalDeviceProperties vkPhysicalDeviceProperties;
            vkGetPhysicalDeviceProperties(vkPhysicalDevice, &vkPhysicalDeviceProperties);
            return vkPhysicalDeviceProperties;
        }
    }

    /// <inheritdoc />
    public override uint DeviceId => VkPhysicalDeviceProperties.deviceID;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc />
    public override uint VendorId => VkPhysicalDeviceProperties.vendorID;

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkPhysicalDevice" /> for the adapter.</summary>
    public VkPhysicalDevice VkPhysicalDevice
    {
        get
        {
            AssertNotDisposed();
            return _vkPhysicalDevice;
        }
    }

    /// <summary>Gets the <see cref="Interop.Vulkan.VkPhysicalDeviceFeatures" /> for <see cref="VkPhysicalDevice" />.</summary>
    public ref readonly VkPhysicalDeviceFeatures VkPhysicalDeviceFeatures => ref _vkPhysicalDeviceFeatures;

    /// <summary>Gets the <see cref="Interop.Vulkan.VkPhysicalDeviceProperties" /> for <see cref="VkPhysicalDevice" />.</summary>
    public ref readonly VkPhysicalDeviceMemoryProperties VkPhysicalDeviceMemoryProperties => ref _vkPhysicalDeviceMemoryProperties;

    /// <summary>Gets the <see cref="Interop.Vulkan.VkPhysicalDeviceProperties" /> for <see cref="VkPhysicalDevice" />.</summary>
    public ref readonly VkPhysicalDeviceProperties VkPhysicalDeviceProperties => ref _vkPhysicalDeviceProperties;

    /// <inheritdoc />
    public override VulkanGraphicsDevice CreateDevice(delegate*<GraphicsDeviceObject, delegate*<in GraphicsMemoryRegion, void>, nuint, bool, GraphicsMemoryAllocator> createMemoryAllocator)
    {
        ThrowIfDisposed();
        return new VulkanGraphicsDevice(this, createMemoryAllocator);
    }

    /// <summary>Tries to query the <see cref="VkPhysicalDeviceMemoryBudgetPropertiesEXT" /> for <see cref="VkPhysicalDevice" />.</summary>
    /// <param name="vkPhysicalDeviceMemoryBudgetProperties">The memory budget properties that will be filled.</param>
    /// <returns><c>true</c> if the query succeeded; otherwise, <c>false</c>.</returns>
    public bool TryGetVkPhysicalDeviceMemoryBudgetProperties(VkPhysicalDeviceMemoryBudgetPropertiesEXT* vkPhysicalDeviceMemoryBudgetProperties)
    {
        var vkPhysicalDeviceMemoryProperties = new VkPhysicalDeviceMemoryProperties2 {
            sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_MEMORY_PROPERTIES_2_KHR,
            pNext = vkPhysicalDeviceMemoryBudgetProperties,
        };

        vkGetPhysicalDeviceMemoryProperties2(VkPhysicalDevice, &vkPhysicalDeviceMemoryProperties);
        return true;
    }
}
