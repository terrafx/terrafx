// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsAdapter : GraphicsAdapter
{
    private VkPhysicalDevice _vkPhysicalDevice;

    private readonly VkPhysicalDeviceFeatures _vkPhysicalDeviceFeatures;
    private readonly VkPhysicalDeviceMemoryProperties _vkPhysicalDeviceMemoryProperties;
    private readonly VkPhysicalDeviceProperties _vkPhysicalDeviceProperties;
    private readonly UnmanagedArray<VkQueueFamilyProperties> _vkQueueFamilyProperties;

    private ValueList<VulkanGraphicsDevice> _devices;
    private readonly ValueMutex _devicesMutex;

    internal VulkanGraphicsAdapter(VulkanGraphicsService service, VkPhysicalDevice vkPhysicalDevice)
        : base(service)
    {
        AssertNotNull(vkPhysicalDevice);
 
        _vkPhysicalDevice = vkPhysicalDevice;

        _vkPhysicalDeviceFeatures = GetVkPhysicalDeviceFeatures();
        _vkPhysicalDeviceProperties = GetVkPhysicalDeviceProperties();
        _vkPhysicalDeviceMemoryProperties = GetVkPhysicalDeviceMemoryProperties();
        _vkQueueFamilyProperties = GetVkQueueFamilyProperties();

        AdapterInfo.Description = GetUtf8Span(in _vkPhysicalDeviceProperties.deviceName[0], 256).GetString() ?? string.Empty;
        AdapterInfo.PciDeviceId = _vkPhysicalDeviceProperties.deviceID;
        AdapterInfo.PciVendorId = _vkPhysicalDeviceProperties.vendorID;

        _devices = new ValueList<VulkanGraphicsDevice>();
        _devicesMutex = new ValueMutex();

        SetName(AdapterInfo.Description);

        VkPhysicalDeviceFeatures GetVkPhysicalDeviceFeatures()
        {
            VkPhysicalDeviceFeatures vkPhysicalDeviceFeatures;
            vkGetPhysicalDeviceFeatures(_vkPhysicalDevice, &vkPhysicalDeviceFeatures);
            return vkPhysicalDeviceFeatures;
        }

        VkPhysicalDeviceMemoryProperties GetVkPhysicalDeviceMemoryProperties()
        {
            VkPhysicalDeviceMemoryProperties vkPhysicalDeviceMemoryProperties;
            vkGetPhysicalDeviceMemoryProperties(_vkPhysicalDevice, &vkPhysicalDeviceMemoryProperties);
            return vkPhysicalDeviceMemoryProperties;
        }

        VkPhysicalDeviceProperties GetVkPhysicalDeviceProperties()
        {
            VkPhysicalDeviceProperties vkPhysicalDeviceProperties;
            vkGetPhysicalDeviceProperties(_vkPhysicalDevice, &vkPhysicalDeviceProperties);
            return vkPhysicalDeviceProperties;
        }

        UnmanagedArray<VkQueueFamilyProperties> GetVkQueueFamilyProperties()
        {
            uint vkQueueFamilyPropertyCount;
            vkGetPhysicalDeviceQueueFamilyProperties(vkPhysicalDevice, &vkQueueFamilyPropertyCount, pQueueFamilyProperties: null);

            var vkQueueFamilyProperties = new UnmanagedArray<VkQueueFamilyProperties>(vkQueueFamilyPropertyCount);
            vkGetPhysicalDeviceQueueFamilyProperties(vkPhysicalDevice, &vkQueueFamilyPropertyCount, vkQueueFamilyProperties.GetPointerUnsafe(0));

            return vkQueueFamilyProperties;
        }
    }

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkPhysicalDevice" /> for the adapter.</summary>
    public VkPhysicalDevice VkPhysicalDevice => _vkPhysicalDevice;

    /// <summary>Gets the <see cref="Interop.Vulkan.VkPhysicalDeviceFeatures" /> for <see cref="VkPhysicalDevice" />.</summary>
    public ref readonly VkPhysicalDeviceFeatures VkPhysicalDeviceFeatures => ref _vkPhysicalDeviceFeatures;

    /// <summary>Gets the <see cref="Interop.Vulkan.VkPhysicalDeviceProperties" /> for <see cref="VkPhysicalDevice" />.</summary>
    public ref readonly VkPhysicalDeviceMemoryProperties VkPhysicalDeviceMemoryProperties => ref _vkPhysicalDeviceMemoryProperties;

    /// <summary>Gets the <see cref="Interop.Vulkan.VkPhysicalDeviceProperties" /> for <see cref="VkPhysicalDevice" />.</summary>
    public ref readonly VkPhysicalDeviceProperties VkPhysicalDeviceProperties => ref _vkPhysicalDeviceProperties;

    /// <summary>Gets the <see cref="Interop.Vulkan.VkQueueFamilyProperties" /> for <see cref="VkPhysicalDevice" />.</summary>
    public UnmanagedReadOnlySpan<VkQueueFamilyProperties> VkQueueFamilyProperties => _vkQueueFamilyProperties;

    /// <summary>Tries to query the <see cref="VkPhysicalDeviceMemoryBudgetPropertiesEXT" /> for <see cref="VkPhysicalDevice" />.</summary>
    /// <param name="vkPhysicalDeviceMemoryBudgetProperties">The memory budget properties that will be filled.</param>
    /// <returns><c>true</c> if the query succeeded; otherwise, <c>false</c>.</returns>
    public bool TryGetVkPhysicalDeviceMemoryBudgetProperties(VkPhysicalDeviceMemoryBudgetPropertiesEXT* vkPhysicalDeviceMemoryBudgetProperties)
    {
        var vkPhysicalDeviceMemoryProperties = new VkPhysicalDeviceMemoryProperties2 {
            sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_MEMORY_PROPERTIES_2_KHR,
            pNext = vkPhysicalDeviceMemoryBudgetProperties,
            memoryProperties = default,
        };

        vkGetPhysicalDeviceMemoryProperties2(VkPhysicalDevice, &vkPhysicalDeviceMemoryProperties);
        return true;
    }

    /// <inheritdoc />
    protected override VulkanGraphicsDevice CreateDeviceUnsafe(in GraphicsDeviceCreateOptions createOptions)
    {
        return new VulkanGraphicsDevice(this, in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _devices.Dispose();
        }
        _devicesMutex.Dispose();

        _vkQueueFamilyProperties.Dispose();

        _vkPhysicalDevice = VkPhysicalDevice.NULL;
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }

    internal void AddDevice(VulkanGraphicsDevice device)
    {
        _devices.Add(device, _devicesMutex);
    }

    internal bool RemoveDevice(VulkanGraphicsDevice device)
    {
        return IsDisposed || _devices.Remove(device, _devicesMutex);
    }
}
