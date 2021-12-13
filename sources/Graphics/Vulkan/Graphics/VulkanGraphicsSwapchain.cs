// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

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
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsSwapchain : GraphicsSwapchain
{
    private readonly uint _framebufferCount;
    private readonly GraphicsFormat _framebufferFormat;

    private readonly VkSurfaceKHR _vkSurface;

    private UnmanagedArray<VkFramebuffer> _vkFramebuffers;
    private VkSwapchainKHR _vkSwapchain;
    private UnmanagedArray<VkImage> _vkSwapchainImages;
    private UnmanagedArray<VkImageView> _vkSwapchainImageViews;

    private uint _framebufferIndex;

    private VolatileState _state;

    internal VulkanGraphicsSwapchain(VulkanGraphicsDevice device, IGraphicsSurface surface)
        : base(device, surface)
    {
        var framebufferCount = 2u;
        _framebufferCount = framebufferCount;

        var framebufferFormat = GraphicsFormat.R8G8B8A8_UNORM;
        _framebufferFormat = framebufferFormat;

        var vkSurface = CreateVkSurface(device, surface);
        _vkSurface = vkSurface;

        var vkSwapchain = CreateVkSwapchain(device, surface, framebufferCount, framebufferFormat, vkSurface);
        _vkSwapchain = vkSwapchain;

        var vkSwapchainImages = GetVkSwapchainImages(device, vkSwapchain);
        _vkSwapchainImages = vkSwapchainImages;

        var vkSwapchainImageViews = CreateVkImageViews(device, framebufferFormat, vkSwapchainImages);
        _vkSwapchainImageViews = vkSwapchainImageViews;

        _vkFramebuffers = CreateVkFramebuffers(device, surface, vkSwapchainImageViews);
        _framebufferIndex = GetFramebufferIndex(device.VkDevice, vkSwapchain, Fence.VkFence);

        _ = _state.Transition(to: Initialized);

        Surface.SizeChanged += OnGraphicsSurfaceSizeChanged;

        static VkSurfaceKHR CreateVkSurface(VulkanGraphicsDevice device, IGraphicsSurface surface)
        {
            VkSurfaceKHR vkSurface;
            var vkInstance = device.Service.VkInstance;

            switch (surface.Kind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    var vkSurfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
                        hinstance = surface.ContextHandle,
                        hwnd = surface.Handle,
                    };

                    ThrowExternalExceptionIfNotSuccess(vkCreateWin32SurfaceKHR(vkInstance, &vkSurfaceCreateInfo, pAllocator: null, &vkSurface));
                    break;
                }

                case GraphicsSurfaceKind.Xlib:
                {
                    var vkSurfaceCreateInfo = new VkXlibSurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_XLIB_SURFACE_CREATE_INFO_KHR,
                        dpy = surface.ContextHandle,
                        window = (nuint)(nint)surface.Handle,
                    };

                    ThrowExternalExceptionIfNotSuccess(vkCreateXlibSurfaceKHR(vkInstance, &vkSurfaceCreateInfo, pAllocator: null, &vkSurface));
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
            ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceSupportKHR(device.Adapter.VkPhysicalDevice, device.VkCommandQueueFamilyIndex, vkSurface, &supported));

            if (!supported)
            {
                ThrowForMissingFeature();
            }
            return vkSurface;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsSwapchain" /> class.</summary>
    ~VulkanGraphicsSwapchain() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsSwapchain.Fence" />
    public new VulkanGraphicsFence Fence => base.Fence.As<VulkanGraphicsFence>();

    /// <inheritdoc />
    public override GraphicsFormat FramebufferFormat => _framebufferFormat;

    /// <inheritdoc />
    public override uint FramebufferIndex => _framebufferIndex;

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the <see cref="VkFramebuffer"/> used by the context.</summary>
    public UnmanagedReadOnlySpan<VkFramebuffer> VkFramebuffers
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkFramebuffers;
        }
    }

    /// <summary>Gets the <see cref="VkSurfaceKHR" /> used by the device.</summary>
    public VkSurfaceKHR VkSurface
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkSurface;
        }
    }

    /// <summary>Gets the <see cref="VkSwapchainKHR" /> used by the device.</summary>
    public VkSwapchainKHR VkSwapchain
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkSwapchain;
        }
    }

    /// <summary>Gets a readonly span of the <see cref="VkImage" /> used by <see cref="VkSwapchain" />.</summary>
    public UnmanagedReadOnlySpan<VkImage> VkSwapchainImages
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkSwapchainImages;
        }
    }

    /// <summary>Gets the <see cref="VkImageView" /> used by the context.</summary>
    public UnmanagedReadOnlySpan<VkImageView> VkSwapchainImageViews
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkSwapchainImageViews;
        }
    }

    /// <inheritdoc />
    public override void Present()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsSwapchain));

        var fence = Fence;
        fence.Wait();
        fence.Reset();

        var device = Device;

        var framebufferIndex = FramebufferIndex;
        var vkSwapchain = VkSwapchain;

        var vkPresentInfo = new VkPresentInfoKHR {
            sType = VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
            swapchainCount = 1,
            pSwapchains = &vkSwapchain,
            pImageIndices = &framebufferIndex,
        };
        ThrowExternalExceptionIfNotSuccess(vkQueuePresentKHR(device.VkCommandQueue, &vkPresentInfo));

        ThrowExternalExceptionIfNotSuccess(vkAcquireNextImageKHR(device.VkDevice, vkSwapchain, timeout: ulong.MaxValue, VkSemaphore.NULL, fence.VkFence, &framebufferIndex));
        _framebufferIndex = framebufferIndex;
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            var fence = Fence;
            fence.Wait();
            fence.Reset();

            var device = Device;
            var vkDevice = device.VkDevice;

            CleanupVkImageViews(vkDevice, _vkSwapchainImageViews);
            CleanupVkFramebuffers(vkDevice, _vkFramebuffers);
            CleanupVkSwapchain(vkDevice, _vkSwapchain);
            DisposeVkSurface(device.Service.VkInstance, _vkSurface);

            if (isDisposing)
            {
                Fence?.Dispose();
            }
        }

        _state.EndDispose();

        static void DisposeVkSurface(VkInstance vkInstance, VkSurfaceKHR vkSurface)
        {
            if (vkSurface != VkSurfaceKHR.NULL)
            {
                vkDestroySurfaceKHR(vkInstance, vkSurface, pAllocator: null);
            }
        }
    }

    private static void CleanupVkFramebuffers(VkDevice vkDevice, UnmanagedArray<VkFramebuffer> vkFramebuffers)
    {
        for (var i = 0u; i < vkFramebuffers.Length; i++)
        {
            vkDestroyFramebuffer(vkDevice, vkFramebuffers[i], pAllocator: null);
        }
    }

    private static void CleanupVkImageViews(VkDevice vkDevice, UnmanagedArray<VkImageView> vkImageViews)
    {
        for (var i = 0u; i < vkImageViews.Length; i++)
        {
            vkDestroyImageView(vkDevice, vkImageViews[i], pAllocator: null);
        }
    }

    private static void CleanupVkSwapchain(VkDevice vkDevice, VkSwapchainKHR vkSwapchain)
    {
        if (vkSwapchain != VkSwapchainKHR.NULL)
        {
            vkDestroySwapchainKHR(vkDevice, vkSwapchain, pAllocator: null);
        }
    }

    private static UnmanagedArray<VkFramebuffer> CreateVkFramebuffers(VulkanGraphicsDevice device, IGraphicsSurface surface, UnmanagedArray<VkImageView> vkImageViews)
    {
        var vkFramebuffers = new UnmanagedArray<VkFramebuffer>(vkImageViews.Length);

        var vkFramebufferCreateInfo = new VkFramebufferCreateInfo {
            sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
            renderPass = device.VkRenderPass,
            attachmentCount = 1,
            width = (uint)surface.Width,
            height = (uint)surface.Height,
            layers = 1,
        };

        for (var i = 0u; i < vkFramebuffers.Length; i++)
        {
            vkFramebufferCreateInfo.pAttachments = vkImageViews.GetPointerUnsafe(i);

            VkFramebuffer vkFramebuffer;
            ThrowExternalExceptionIfNotSuccess(vkCreateFramebuffer(device.VkDevice, &vkFramebufferCreateInfo, pAllocator: null, &vkFramebuffer));
            vkFramebuffers[i] = vkFramebuffer;
        }

        return vkFramebuffers;
    }

    private UnmanagedArray<VkImageView> CreateVkImageViews(VulkanGraphicsDevice device, GraphicsFormat framebufferFormat, UnmanagedArray<VkImage> vkImages)
    {
        var vkImageViews = new UnmanagedArray<VkImageView>(vkImages.Length);

        var vkImageViewCreateInfo = new VkImageViewCreateInfo {
            sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
            viewType = VK_IMAGE_VIEW_TYPE_2D,
            format = framebufferFormat.AsVkFormat(),
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

        for (var i = 0u; i < vkImageViews.Length; i++)
        {
            vkImageViewCreateInfo.image = vkImages[i];

            VkImageView vkImageView;
            ThrowExternalExceptionIfNotSuccess(vkCreateImageView(device.VkDevice, &vkImageViewCreateInfo, pAllocator: null, &vkImageView));
            vkImageViews[i] = vkImageView;
        }

        return vkImageViews;
    }

    private static uint GetFramebufferIndex(VkDevice vkDevice, VkSwapchainKHR vkSwapchain, VkFence vkFence)
    {
        uint framebufferIndex;
        ThrowExternalExceptionIfNotSuccess(vkAcquireNextImageKHR(vkDevice, vkSwapchain, timeout: ulong.MaxValue, VkSemaphore.NULL, vkFence, &framebufferIndex));
        return framebufferIndex;
    }

    private static VkSwapchainKHR CreateVkSwapchain(VulkanGraphicsDevice device, IGraphicsSurface surface, uint framebufferCount, GraphicsFormat framebufferFormat, VkSurfaceKHR vkSurface)
    {
        VkSwapchainKHR vkSwapchain;
        var vkPhysicalDevice = device.Adapter.VkPhysicalDevice;

        VkSurfaceCapabilitiesKHR vkSurfaceCapabilities;
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceCapabilitiesKHR(vkPhysicalDevice, vkSurface, &vkSurfaceCapabilities));

        uint vkPresentModeCount;
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfacePresentModesKHR(vkPhysicalDevice, vkSurface, &vkPresentModeCount, pPresentModes: null));

        var vkPresentModes = stackalloc VkPresentModeKHR[(int)vkPresentModeCount];
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfacePresentModesKHR(vkPhysicalDevice, vkSurface, &vkPresentModeCount, vkPresentModes));

        if ((framebufferCount < vkSurfaceCapabilities.minImageCount) || ((vkSurfaceCapabilities.maxImageCount != 0) && (framebufferCount > vkSurfaceCapabilities.maxImageCount)))
        {
            ThrowNotImplementedException();
        }

        var swapChainCreateInfo = new VkSwapchainCreateInfoKHR {
            sType = VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR,
            surface = vkSurface,
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

        if ((vkSurfaceCapabilities.supportedTransforms & VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR) == 0)
        {
            swapChainCreateInfo.preTransform = vkSurfaceCapabilities.currentTransform;
        }

        uint surfaceFormatCount;
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceFormatsKHR(vkPhysicalDevice, vkSurface, &surfaceFormatCount, pSurfaceFormats: null));

        var surfaceFormats = stackalloc VkSurfaceFormatKHR[(int)surfaceFormatCount];
        ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceFormatsKHR(vkPhysicalDevice, vkSurface, &surfaceFormatCount, surfaceFormats));

        var vkFramebufferFormat = framebufferFormat.AsVkFormat();
        for (uint i = 0; i < surfaceFormatCount; i++)
        {
            if (surfaceFormats[i].format == vkFramebufferFormat)
            {
                swapChainCreateInfo.imageFormat = surfaceFormats[i].format;
                swapChainCreateInfo.imageColorSpace = surfaceFormats[i].colorSpace;
                break;
            }
        }
        ThrowExternalExceptionIfNotSuccess(vkCreateSwapchainKHR(device.VkDevice, &swapChainCreateInfo, pAllocator: null, &vkSwapchain));

        return vkSwapchain;
    }

    private static UnmanagedArray<VkImage> GetVkSwapchainImages(VulkanGraphicsDevice device, VkSwapchainKHR vkSwapchain)
    {
        var vkDevice = device.VkDevice;

        uint vkSwapchainImageCount;
        ThrowExternalExceptionIfNotSuccess(vkGetSwapchainImagesKHR(vkDevice, vkSwapchain, &vkSwapchainImageCount, pSwapchainImages: null));

        var vkSwapchainImages = new UnmanagedArray<VkImage>(vkSwapchainImageCount);
        ThrowExternalExceptionIfNotSuccess(vkGetSwapchainImagesKHR(vkDevice, vkSwapchain, &vkSwapchainImageCount, vkSwapchainImages.GetPointerUnsafe(0)));

        return vkSwapchainImages;
    }

    private void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        var device = Device;
        var vkDevice = device.VkDevice;

        CleanupVkFramebuffers(vkDevice, _vkFramebuffers);
        CleanupVkImageViews(vkDevice, _vkSwapchainImageViews);
        CleanupVkSwapchain(vkDevice, _vkSwapchain);

        var surface = Surface;

        var vkSwapchain = CreateVkSwapchain(device, surface, _framebufferCount, FramebufferFormat, VkSurface);
        _vkSwapchain = vkSwapchain;

        var vkSwapchainImages = GetVkSwapchainImages(device, vkSwapchain);
        _vkSwapchainImages = vkSwapchainImages;

        var vkSwapchainImageViews = CreateVkImageViews(device, FramebufferFormat, vkSwapchainImages);
        _vkSwapchainImageViews = vkSwapchainImageViews;

        _vkFramebuffers = CreateVkFramebuffers(device, surface, vkSwapchainImageViews);
        _framebufferIndex = GetFramebufferIndex(device.VkDevice, vkSwapchain, Fence.VkFence);
    }
}
