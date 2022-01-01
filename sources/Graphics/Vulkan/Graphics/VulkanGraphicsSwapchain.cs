// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using TerraFX.Numerics;
using static TerraFX.Interop.Vulkan.VkColorSpaceKHR;
using static TerraFX.Interop.Vulkan.VkCompositeAlphaFlagsKHR;
using static TerraFX.Interop.Vulkan.VkFormat;
using static TerraFX.Interop.Vulkan.VkImageUsageFlags;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkPresentModeKHR;
using static TerraFX.Interop.Vulkan.VkSharingMode;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.VkSurfaceTransformFlagsKHR;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsSwapchain : GraphicsSwapchain
{
    private readonly uint _minimumRenderTargetCount;
    private VkSurfaceCapabilitiesKHR _vkSurfaceCapabilities;

    private VkSurfaceKHR _vkSurface;
    private VkSwapchainKHR _vkSwapchain;
    private UnmanagedArray<VkImage> _vkSwapchainImages;

    internal VulkanGraphicsSwapchain(VulkanGraphicsRenderPass renderPass, in VulkanGraphicsSwapchainCreateOptions createOptions) : base(renderPass)
    {
        _minimumRenderTargetCount = createOptions.MinimumRenderTargetCount;

        SwapchainInfo.Fence = Device.CreateFence(isSignalled: false);
        SwapchainInfo.RenderTargetFormat = createOptions.RenderTargetFormat;
        SwapchainInfo.RenderTargets = Array.Empty<VulkanGraphicsRenderTarget>();
        SwapchainInfo.Surface = createOptions.Surface;

        _vkSurface = CreateVkSurface();

        _vkSwapchain = CreateVkSwapchain();
        _vkSwapchainImages = GetVkSwapchainImages();

        SwapchainInfo.RenderTargets = new VulkanGraphicsRenderTarget[_vkSwapchainImages.Length];
        InitializeRenderTargets();

        SetNameUnsafe(Name);
        SwapchainInfo.CurrentRenderTargetIndex = GetCurrentRenderTargetIndex();

        Surface.SizeChanged += OnGraphicsSurfaceSizeChanged;

        VkSurfaceKHR CreateVkSurface()
        {
            VkSurfaceKHR vkSurface;

            var device = Device;
            var service = Service;
            var surface = Surface;

            var vkInstance = service.VkInstance;
            ref readonly var vkInstanceManualImports = ref service.VkInstanceManualImports;

            switch (surface.Kind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    var vkSurfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
                        pNext = null,
                        flags = 0,
                        hinstance = surface.ContextHandle,
                        hwnd = surface.Handle,
                    };
                    ThrowIfNull(vkInstanceManualImports.vkCreateWin32SurfaceKHR);

                    ThrowExternalExceptionIfNotSuccess(vkInstanceManualImports.vkCreateWin32SurfaceKHR(vkInstance, &vkSurfaceCreateInfo, null, &vkSurface));
                    break;
                }

                case GraphicsSurfaceKind.Xlib:
                {
                    var vkSurfaceCreateInfo = new VkXlibSurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_XLIB_SURFACE_CREATE_INFO_KHR,
                        pNext = null,
                        flags = 0,
                        dpy = surface.ContextHandle,
                        window = (nuint)(nint)surface.Handle,
                    };
                    ThrowIfNull(vkInstanceManualImports.vkCreateXlibSurfaceKHR);

                    ThrowExternalExceptionIfNotSuccess(vkInstanceManualImports.vkCreateXlibSurfaceKHR(vkInstance, &vkSurfaceCreateInfo, null, &vkSurface));
                    break;
                }

                default:
                {
                    ThrowForInvalidKind(surface.Kind);
                    vkSurface = VkSurfaceKHR.NULL;
                    break;
                }
            }

            VkBool32 supported;
            ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceSupportKHR(device.Adapter.VkPhysicalDevice, device.RenderCommandQueue.VkQueueFamilyIndex, vkSurface, &supported));

            if (!supported)
            {
                ThrowForMissingFeature();
            }
            return vkSurface;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsSwapchain" /> class.</summary>
    ~VulkanGraphicsSwapchain() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsSwapchain.CurrentRenderTarget" />
    public new VulkanGraphicsRenderTarget CurrentRenderTarget => base.CurrentRenderTarget.As<VulkanGraphicsRenderTarget>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsSwapchain.Fence" />
    public new VulkanGraphicsFence Fence => base.Fence.As<VulkanGraphicsFence>();

    /// <inheritdoc cref="GraphicsRenderPassObject.RenderPass" />
    public new VulkanGraphicsRenderPass RenderPass => base.RenderPass.As<VulkanGraphicsRenderPass>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the <see cref="VkSurfaceKHR" /> used by the device.</summary>
    public VkSurfaceKHR VkSurface => _vkSurface;

    /// <summary>Gets the <see cref="VkSurfaceCapabilitiesKHR" /> for <see cref="VkSurface" /></summary>
    public ref readonly VkSurfaceCapabilitiesKHR VkSurfaceCapabilities => ref _vkSurfaceCapabilities;

    /// <summary>Gets the <see cref="VkSwapchainKHR" /> used by the device.</summary>
    public VkSwapchainKHR VkSwapchain => _vkSwapchain;

    /// <summary>Gets a readonly span of the <see cref="VkImage" />s used by <see cref="VkSwapchain" />.</summary>
    public UnmanagedReadOnlySpan<VkImage> VkSwapchainImages => _vkSwapchainImages;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            var fence = SwapchainInfo.Fence;
            fence.Wait();
            fence.Reset();

            Fence?.Dispose();
            SwapchainInfo.Fence = null!;

            CleanupRenderTargets();
            SwapchainInfo.RenderTargets = null!;
        }
        _vkSwapchainImages.Dispose();

        DisposeVkSwapchain(Device.VkDevice, _vkSwapchain);
        _vkSwapchain = VkSwapchainKHR.NULL;

        DisposeVkSurface(Service.VkInstance, _vkSurface);
        _vkSurface = VkSurfaceKHR.NULL;

        static void DisposeVkSurface(VkInstance vkInstance, VkSurfaceKHR vkSurface)
        {
            if (vkSurface != VkSurfaceKHR.NULL)
            {
                vkDestroySurfaceKHR(vkInstance, vkSurface, pAllocator: null);
            }
        }

        static void DisposeVkSwapchain(VkDevice vkDevice, VkSwapchainKHR vkSwapchain)
        {
            if (vkSwapchain != VkSwapchainKHR.NULL)
            {
                vkDestroySwapchainKHR(vkDevice, vkSwapchain, pAllocator: null);
            }
        }
    }

    /// <inheritdoc />
    protected override void PresentUnsafe()
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        var device = Device;

        var renderTargetIndex = (uint)CurrentRenderTargetIndex;
        var vkSwapchain = VkSwapchain;

        var vkPresentInfo = new VkPresentInfoKHR {
            sType = VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
            pNext = null,
            waitSemaphoreCount = 0,
            pWaitSemaphores = null,
            swapchainCount = 1,
            pSwapchains = &vkSwapchain,
            pImageIndices = &renderTargetIndex,
            pResults = null,
        };

        ThrowExternalExceptionIfNotSuccess(vkQueuePresentKHR(device.RenderCommandQueue.VkQueue, &vkPresentInfo));
        SwapchainInfo.CurrentRenderTargetIndex = GetCurrentRenderTargetIndex();
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_SURFACE_KHR, VkSurface, value);
        Device.SetVkObjectName(VK_OBJECT_TYPE_SWAPCHAIN_KHR, VkSwapchain, value);
    }

    private void CleanupRenderTargets()
    {
        var renderTargets = SwapchainInfo.RenderTargets;

        for (var index = 0; index < renderTargets.Length; index++)
        {
            renderTargets[index].Dispose();
            renderTargets[index] = null!;
        }
    }

    private VkSwapchainKHR CreateVkSwapchain()
    {
        VkSwapchainKHR vkSwapchain;

        var vkPhysicalDevice = Adapter.VkPhysicalDevice;
        var vkSurface = _vkSurface;

        VkSurfaceCapabilitiesKHR vkSurfaceCapabilities;
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceCapabilitiesKHR(vkPhysicalDevice, vkSurface, &vkSurfaceCapabilities));
        _vkSurfaceCapabilities = vkSurfaceCapabilities;

        var minimumRenderTargetCount = _minimumRenderTargetCount;

        if (minimumRenderTargetCount < vkSurfaceCapabilities.minImageCount)
        {
            minimumRenderTargetCount = vkSurfaceCapabilities.minImageCount;
        }

        if (vkSurfaceCapabilities.maxImageCount != 0)
        {
            ThrowIfNotInInsertBounds(minimumRenderTargetCount, vkSurfaceCapabilities.maxImageCount);
        }

        uint vkPresentModeCount;
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfacePresentModesKHR(vkPhysicalDevice, vkSurface, &vkPresentModeCount, pPresentModes: null));

        var vkPresentModes = stackalloc VkPresentModeKHR[(int)vkPresentModeCount];
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfacePresentModesKHR(vkPhysicalDevice, vkSurface, &vkPresentModeCount, vkPresentModes));

        var vkOldSwapchain = _vkSwapchain;

        var vkSwapchainCreateInfo = new VkSwapchainCreateInfoKHR {
            sType = VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR,
            pNext = null,
            flags = 0,
            surface = vkSurface,
            minImageCount = minimumRenderTargetCount,
            imageFormat = VK_FORMAT_UNDEFINED,
            imageColorSpace = VK_COLOR_SPACE_SRGB_NONLINEAR_KHR,
            imageExtent = vkSurfaceCapabilities.currentExtent,
            imageArrayLayers = 1,
            imageUsage = VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
            imageSharingMode = VK_SHARING_MODE_EXCLUSIVE,
            queueFamilyIndexCount = 0,
            pQueueFamilyIndices = null,
            preTransform = VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR,
            compositeAlpha = VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR,
            presentMode = VK_PRESENT_MODE_FIFO_KHR,
            clipped = VK_TRUE,
            oldSwapchain = vkOldSwapchain,
        };

        if ((vkSurfaceCapabilities.supportedTransforms & VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR) == 0)
        {
            vkSwapchainCreateInfo.preTransform = vkSurfaceCapabilities.currentTransform;
        }

        uint vkSurfaceFormatCount;
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceFormatsKHR(vkPhysicalDevice, vkSurface, &vkSurfaceFormatCount, pSurfaceFormats: null));

        var vkSurfaceFormats = stackalloc VkSurfaceFormatKHR[(int)vkSurfaceFormatCount];
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceFormatsKHR(vkPhysicalDevice, vkSurface, &vkSurfaceFormatCount, vkSurfaceFormats));

        var vkFormat = SwapchainInfo.RenderTargetFormat.AsVkFormat();

        for (var index = 0u; index < vkSurfaceFormatCount; index++)
        {
            if (vkSurfaceFormats[index].format == vkFormat)
            {
                vkSwapchainCreateInfo.imageFormat = vkSurfaceFormats[index].format;
                vkSwapchainCreateInfo.imageColorSpace = vkSurfaceFormats[index].colorSpace;
                break;
            }
        }

        var vkDevice = Device.VkDevice;
        ThrowExternalExceptionIfNotSuccess(vkCreateSwapchainKHR(vkDevice, &vkSwapchainCreateInfo, pAllocator: null, &vkSwapchain));

        if (vkOldSwapchain != VkSwapchainKHR.NULL)
        {
            vkDestroySwapchainKHR(vkDevice, vkOldSwapchain, pAllocator: null);
        }
        return vkSwapchain;
    }

    private int GetCurrentRenderTargetIndex()
    {
        uint renderTargetIndex;
        ThrowExternalExceptionIfNotSuccess(vkAcquireNextImageKHR(Device.VkDevice, VkSwapchain, timeout: ulong.MaxValue, VkSemaphore.NULL, Fence.VkFence, &renderTargetIndex));
        return (int)renderTargetIndex;
    }

    private UnmanagedArray<VkImage> GetVkSwapchainImages()
    {
        var vkDevice = Device.VkDevice;
        var vkSwapchain = _vkSwapchain;

        uint vkSwapchainImageCount;
        ThrowExternalExceptionIfNotSuccess(vkGetSwapchainImagesKHR(vkDevice, vkSwapchain, &vkSwapchainImageCount, pSwapchainImages: null));

        var vkSwapchainImages = new UnmanagedArray<VkImage>(vkSwapchainImageCount);
        ThrowExternalExceptionIfNotSuccess(vkGetSwapchainImagesKHR(vkDevice, vkSwapchain, &vkSwapchainImageCount, vkSwapchainImages.GetPointerUnsafe(0)));

        return vkSwapchainImages;
    }

    private void InitializeRenderTargets()
    {
        var renderTargets = SwapchainInfo.RenderTargets;

        for (var index = 0; index < renderTargets.Length; index++)
        {
            renderTargets[index] = new VulkanGraphicsRenderTarget(this, index);
        }
    }

    private void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        CleanupRenderTargets();

        _vkSwapchain = CreateVkSwapchain();
        _vkSwapchainImages = GetVkSwapchainImages();

        if ((uint)SwapchainInfo.RenderTargets.Length != _vkSwapchainImages.Length)
        {
            SwapchainInfo.RenderTargets = new VulkanGraphicsRenderTarget[_vkSwapchainImages.Length];
        }
        InitializeRenderTargets();

        SetNameUnsafe(Name);
        SwapchainInfo.CurrentRenderTargetIndex = GetCurrentRenderTargetIndex();
    }
}
