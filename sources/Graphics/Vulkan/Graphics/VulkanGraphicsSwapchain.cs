// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.Vulkan;
using TerraFX.Numerics;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkComponentSwizzle;
using static TerraFX.Interop.Vulkan.VkCompositeAlphaFlagsKHR;
using static TerraFX.Interop.Vulkan.VkFormat;
using static TerraFX.Interop.Vulkan.VkImageAspectFlags;
using static TerraFX.Interop.Vulkan.VkImageUsageFlags;
using static TerraFX.Interop.Vulkan.VkImageViewType;
using static TerraFX.Interop.Vulkan.VkPresentModeKHR;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.VkSurfaceTransformFlagsKHR;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsSwapchain : GraphicsSwapchain
{
    private readonly uint _framebufferCount;

    private ValueLazy<UnmanagedArray<VkFramebuffer>> _vulkanFramebuffers;
    private ValueLazy<VkSurfaceKHR> _vulkanSurface;
    private ValueLazy<VkSwapchainKHR> _vulkanSwapchain;
    private ValueLazy<UnmanagedArray<VkImage>> _vulkanSwapchainImages;
    private ValueLazy<UnmanagedArray<VkImageView>> _vulkanSwapchainImageViews;

    private VkFormat _framebufferFormat;
    private uint _framebufferIndex;

    private VolatileState _state;

    internal VulkanGraphicsSwapchain(VulkanGraphicsDevice device, IGraphicsSurface surface)
        : base(device, surface)
    {
        _framebufferCount = 2;


        _vulkanFramebuffers = new ValueLazy<UnmanagedArray<VkFramebuffer>>(CreateVulkanFramebuffers);
        _vulkanSurface = new ValueLazy<VkSurfaceKHR>(CreateVulkanSurface);
        _vulkanSwapchain = new ValueLazy<VkSwapchainKHR>(CreateVulkanSwapchain);
        _vulkanSwapchainImages = new ValueLazy<UnmanagedArray<VkImage>>(GetVulkanSwapchainImages);
        _vulkanSwapchainImageViews = new ValueLazy<UnmanagedArray<VkImageView>>(CreateVulkanSwapchainImageViews);

        _ = _state.Transition(to: Initialized);

        Surface.SizeChanged += OnGraphicsSurfaceSizeChanged;
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsSwapchain" /> class.</summary>
    ~VulkanGraphicsSwapchain() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => (VulkanGraphicsDevice)base.Device;

    /// <inheritdoc cref="GraphicsSwapchain.Fence" />
    public new VulkanGraphicsFence Fence => (VulkanGraphicsFence)base.Fence;

    /// <summary>Gets the <see cref="VkFormat" /> used by <see cref="VulkanSwapchain" />.</summary>
    public VkFormat FramebufferFormat => _framebufferFormat;

    /// <inheritdoc />
    public override uint FramebufferIndex => _framebufferIndex;

    /// <summary>Gets the <see cref="VkFramebuffer"/> used by the context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public UnmanagedReadOnlySpan<VkFramebuffer> VulkanFramebuffers => _vulkanFramebuffers.Value;

    /// <summary>Gets the <see cref="VkSurfaceKHR" /> used by the device.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public VkSurfaceKHR VulkanSurface => _vulkanSurface.Value;

    /// <summary>Gets the <see cref="VkSwapchainKHR" /> used by the device.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public VkSwapchainKHR VulkanSwapchain => _vulkanSwapchain.Value;

    /// <summary>Gets a readonly span of the <see cref="VkImage" /> used by <see cref="VulkanSwapchain" />.</summary>
    public UnmanagedReadOnlySpan<VkImage> VulkanSwapchainImages => _vulkanSwapchainImages.Value;

    /// <summary>Gets the <see cref="VkImageView" /> used by the context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public UnmanagedReadOnlySpan<VkImageView> VulkanSwapchainImageViews => _vulkanSwapchainImageViews.Value;

    /// <inheritdoc />
    public override void Present()
    {
        var device = Device;

        var framebufferIndex = FramebufferIndex;
        var vulkanSwapchain = VulkanSwapchain;

        var presentInfo = new VkPresentInfoKHR {
            sType = VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
            swapchainCount = 1,
            pSwapchains = &vulkanSwapchain,
            pImageIndices = &framebufferIndex,
        };
        ThrowExternalExceptionIfNotSuccess(vkQueuePresentKHR(device.VulkanCommandQueue, &presentInfo));

        device.Signal(Fence);

        ThrowExternalExceptionIfNotSuccess(vkAcquireNextImageKHR(device.VulkanDevice, vulkanSwapchain, timeout: ulong.MaxValue, VkSemaphore.NULL, VkFence.NULL, &framebufferIndex));
        _framebufferIndex = framebufferIndex;
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            _vulkanSwapchainImageViews.Dispose(DisposeVulkanSwapchainImageViews);
            _vulkanFramebuffers.Dispose(DisposeVulkanFramebuffers);
            _vulkanSwapchain.Dispose(DisposeVulkanSwapchain);
            _vulkanSurface.Dispose(DisposeVulkanSurface);
        }

        _state.EndDispose();
    }

    private UnmanagedArray<VkFramebuffer> CreateVulkanFramebuffers()
    {
        var swapchainImageViews = VulkanSwapchainImageViews;
        var vulkanFramebuffers = new UnmanagedArray<VkFramebuffer>(swapchainImageViews.Length);

        var device = Device;
        var surface = Surface;

        var framebufferCreateInfo = new VkFramebufferCreateInfo {
            sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
            renderPass = device.VulkanRenderPass,
            attachmentCount = 1,
            width = (uint)surface.Width,
            height = (uint)surface.Height,
            layers = 1,
        };

        for (var i = 0u; i < vulkanFramebuffers.Length; i++)
        {
            framebufferCreateInfo.pAttachments = swapchainImageViews.GetPointerUnsafe(i);

            VkFramebuffer framebuffer;
            ThrowExternalExceptionIfNotSuccess(vkCreateFramebuffer(device.VulkanDevice, &framebufferCreateInfo, pAllocator: null, &framebuffer));
            vulkanFramebuffers[i] = framebuffer;
        }

        return vulkanFramebuffers;
    }

    private VkSurfaceKHR CreateVulkanSurface()
    {
        VkSurfaceKHR vulkanSurface;

        var adapter = Device.Adapter;
        var vulkanInstance = adapter.Service.VulkanInstance;

        switch (Surface.Kind)
        {
            case GraphicsSurfaceKind.Win32:
            {
                var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR {
                    sType = VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
                    hinstance = Surface.ContextHandle,
                    hwnd = Surface.Handle,
                };

                ThrowExternalExceptionIfNotSuccess(vkCreateWin32SurfaceKHR(vulkanInstance, &surfaceCreateInfo, pAllocator: null, &vulkanSurface));
                break;
            }

            case GraphicsSurfaceKind.Xlib:
            {
                var surfaceCreateInfo = new VkXlibSurfaceCreateInfoKHR {
                    sType = VK_STRUCTURE_TYPE_XLIB_SURFACE_CREATE_INFO_KHR,
                    dpy = Surface.ContextHandle,
                    window = (nuint)(nint)Surface.Handle,
                };

                ThrowExternalExceptionIfNotSuccess(vkCreateXlibSurfaceKHR(vulkanInstance, &surfaceCreateInfo, pAllocator: null, &vulkanSurface));
                break;
            }

            default:
            {
                ThrowForInvalidKind(Surface.Kind);
                vulkanSurface = VkSurfaceKHR.NULL;
                break;
            }
        }

        VkBool32 supported;
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceSupportKHR(adapter.VulkanPhysicalDevice, Device.VulkanCommandQueueFamilyIndex, vulkanSurface, &supported));

        if (!supported)
        {
            ThrowForMissingFeature();
        }
        return vulkanSurface;
    }

    private VkSwapchainKHR CreateVulkanSwapchain()
    {
        VkSwapchainKHR vulkanSwapchain;

        var device = Device;
        var vulkanPhysicalDevice = device.Adapter.VulkanPhysicalDevice;
        var vulkanSurface = VulkanSurface;

        VkSurfaceCapabilitiesKHR surfaceCapabilities;
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceCapabilitiesKHR(vulkanPhysicalDevice, vulkanSurface, &surfaceCapabilities));

        uint presentModeCount;
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfacePresentModesKHR(vulkanPhysicalDevice, vulkanSurface, &presentModeCount, pPresentModes: null));

        var presentModes = stackalloc VkPresentModeKHR[(int)presentModeCount];
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfacePresentModesKHR(vulkanPhysicalDevice, vulkanSurface, &presentModeCount, presentModes));

        var surface = Surface;
        var framebufferCount = _framebufferCount;

        if ((framebufferCount < surfaceCapabilities.minImageCount) || ((surfaceCapabilities.maxImageCount != 0) && (framebufferCount > surfaceCapabilities.maxImageCount)))
        {
            ThrowNotImplementedException();
        }

        var swapChainCreateInfo = new VkSwapchainCreateInfoKHR {
            sType = VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR,
            surface = vulkanSurface,
            minImageCount = framebufferCount,
            imageExtent = new VkExtent2D {
                width = (uint)surface.Width,
                height = (uint)surface.Height,
            },
            imageArrayLayers = 1,
            imageUsage = VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
            preTransform = VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR,
            compositeAlpha = VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR,
            presentMode = VK_PRESENT_MODE_FIFO_KHR,
            clipped = VK_TRUE,
        };

        if ((surfaceCapabilities.supportedTransforms & VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR) == 0)
        {
            swapChainCreateInfo.preTransform = surfaceCapabilities.currentTransform;
        }

        uint surfaceFormatCount;
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceFormatsKHR(vulkanPhysicalDevice, VulkanSurface, &surfaceFormatCount, pSurfaceFormats: null));

        var surfaceFormats = stackalloc VkSurfaceFormatKHR[(int)surfaceFormatCount];
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceFormatsKHR(vulkanPhysicalDevice, VulkanSurface, &surfaceFormatCount, surfaceFormats));

        for (uint i = 0; i < surfaceFormatCount; i++)
        {
            if (surfaceFormats[i].format == VK_FORMAT_B8G8R8A8_UNORM)
            {
                swapChainCreateInfo.imageFormat = surfaceFormats[i].format;
                swapChainCreateInfo.imageColorSpace = surfaceFormats[i].colorSpace;
                break;
            }
        }
        ThrowExternalExceptionIfNotSuccess(vkCreateSwapchainKHR(device.VulkanDevice, &swapChainCreateInfo, pAllocator: null, &vulkanSwapchain));

        _framebufferFormat = swapChainCreateInfo.imageFormat;
        return vulkanSwapchain;
    }

    private UnmanagedArray<VkImageView> CreateVulkanSwapchainImageViews()
    {
        var swapchainImages = VulkanSwapchainImages;
        var swapchainImageViews = new UnmanagedArray<VkImageView>(swapchainImages.Length);

        var device = Device;
        var framebufferFormat = _framebufferFormat;

        var imageViewCreateInfo = new VkImageViewCreateInfo {
            sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
            viewType = VK_IMAGE_VIEW_TYPE_2D,
            format = framebufferFormat,
            components = new VkComponentMapping {
                r = VK_COMPONENT_SWIZZLE_R,
                g = VK_COMPONENT_SWIZZLE_G,
                b = VK_COMPONENT_SWIZZLE_B,
                a = VK_COMPONENT_SWIZZLE_A,
            },
            subresourceRange = new VkImageSubresourceRange {
                aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                levelCount = 1,
                layerCount = 1,
            },
        };

        for (var i = 0u; i < swapchainImageViews.Length; i++)
        {
            imageViewCreateInfo.image = swapchainImages[i];

            VkImageView imageView;
            ThrowExternalExceptionIfNotSuccess(vkCreateImageView(device.VulkanDevice, &imageViewCreateInfo, pAllocator: null, &imageView));
            swapchainImageViews[i] = imageView;
        }

        return swapchainImageViews;
    }

    private void DisposeVulkanFramebuffers(UnmanagedArray<VkFramebuffer> vulkanFramebuffers)
    {
        AssertDisposing(_state);
        var vulkanDevice = Device.VulkanDevice;

        for (var i = 0u; i < vulkanFramebuffers.Length; i++)
        {
            vkDestroyFramebuffer(vulkanDevice, vulkanFramebuffers[i], pAllocator: null);
        }
    }

    private void DisposeVulkanSurface(VkSurfaceKHR vulkanSurface)
    {
        AssertDisposing(_state);

        if (vulkanSurface != VkSurfaceKHR.NULL)
        {
            vkDestroySurfaceKHR(Device.Adapter.Service.VulkanInstance, vulkanSurface, pAllocator: null);
        }
    }

    private void DisposeVulkanSwapchain(VkSwapchainKHR vulkanSwapchain)
    {
        AssertDisposing(_state);

        if (vulkanSwapchain != VkSwapchainKHR.NULL)
        {
            vkDestroySwapchainKHR(Device.VulkanDevice, vulkanSwapchain, pAllocator: null);
        }
    }

    private void DisposeVulkanSwapchainImageViews(UnmanagedArray<VkImageView> vulkanSwapchainImageViews)
    {
        AssertDisposing(_state);
        var vulkanDevice = Device.VulkanDevice;

        for (var i = 0u; i < VulkanSwapchainImageViews.Length; i++)
        {
            vkDestroyImageView(vulkanDevice, vulkanSwapchainImageViews[i], pAllocator: null);
        }
    }

    private UnmanagedArray<VkImage> GetVulkanSwapchainImages()
    {
        var vulkanDevice = Device.VulkanDevice;
        var vulkanSwapchain = VulkanSwapchain;

        uint swapchainImageCount;
        ThrowExternalExceptionIfNotSuccess(vkGetSwapchainImagesKHR(vulkanDevice, vulkanSwapchain, &swapchainImageCount, pSwapchainImages: null));

        var vulkanSwapchainImages = new UnmanagedArray<VkImage>(swapchainImageCount);
        ThrowExternalExceptionIfNotSuccess(vkGetSwapchainImagesKHR(vulkanDevice, vulkanSwapchain, &swapchainImageCount, vulkanSwapchainImages.GetPointerUnsafe(0)));

        uint framebufferIndex;
        ThrowExternalExceptionIfNotSuccess(vkAcquireNextImageKHR(vulkanDevice, vulkanSwapchain, timeout: ulong.MaxValue, VkSemaphore.NULL, VkFence.NULL, &framebufferIndex));
        _framebufferIndex = framebufferIndex;

        return vulkanSwapchainImages;
    }

    private void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
    {
        Device.WaitForIdle();

        if (_vulkanSwapchainImages.IsValueCreated)
        {
            _vulkanSwapchainImages.Reset(GetVulkanSwapchainImages);
        }

        if (_vulkanSwapchain.IsValueCreated)
        {
            vkDestroySwapchainKHR(Device.VulkanDevice, _vulkanSwapchain.Value, pAllocator: null);
            _vulkanSwapchain.Reset(CreateVulkanSwapchain);
            _framebufferIndex = 0;
        }

        if (_vulkanFramebuffers.IsValueCreated)
        {
            var vulkanDevice = Device.VulkanDevice;
            var vulkanFramebuffers = _vulkanFramebuffers.Value;

            for (var i = 0u; i < vulkanFramebuffers.Length; i++)
            {
                vkDestroyFramebuffer(vulkanDevice, vulkanFramebuffers[i], pAllocator: null);
            }

            _vulkanFramebuffers.Reset(CreateVulkanFramebuffers);
        }

        if (_vulkanSwapchainImageViews.IsValueCreated)
        {
            var vulkanDevice = Device.VulkanDevice;
            var vulkanSwapChainImageViews = _vulkanSwapchainImageViews.Value;

            for (var i = 0u; i < vulkanSwapChainImageViews.Length; i++)
            {
                vkDestroyImageView(vulkanDevice, vulkanSwapChainImageViews[i], pAllocator: null);
            }

            _vulkanSwapchainImageViews.Reset(CreateVulkanSwapchainImageViews);
        }
    }
}
