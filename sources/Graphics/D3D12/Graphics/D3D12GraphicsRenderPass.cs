// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsRenderPass : GraphicsRenderPass
{
    internal D3D12GraphicsRenderPass(D3D12GraphicsDevice device, in GraphicsRenderPassCreateOptions createOptions) : base(device)
    {
        device.AddRenderPass(this);

        RenderPassInfo.RenderTargetFormat = createOptions.RenderTargetFormat;
        RenderPassInfo.Surface = createOptions.Surface;

        var swapchainCreateOptions = new D3D12GraphicsSwapchainCreateOptions {
            MinimumRenderTargetCount = createOptions.MinimumRenderTargetCount,
            RenderTargetFormat = createOptions.RenderTargetFormat,
            Surface = createOptions.Surface,
        };
        RenderPassInfo.Swapchain = new D3D12GraphicsSwapchain(this, in swapchainCreateOptions);
    }

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc cref="GraphicsSwapchainObject.Swapchain" />
    public new D3D12GraphicsSwapchain Swapchain => base.Swapchain.As<D3D12GraphicsSwapchain>();

    /// <inheritdoc />
    protected override D3D12GraphicsPipeline CreatePipelineUnsafe(in GraphicsPipelineCreateOptions createOptions)
    {
        return new D3D12GraphicsPipeline(this, in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            RenderPassInfo.Swapchain.Dispose();
            RenderPassInfo.Swapchain = null!;
        }

        _ = Device.RemoveRenderPass(this);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }
}
