// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using static TerraFX.Interop.DirectX.D3D12_RTV_DIMENSION;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsRenderTarget : GraphicsRenderTarget
{
    private readonly D3D12_CPU_DESCRIPTOR_HANDLE _d3d12RtvDescriptorHandle;

    private ID3D12Resource* _d3d12RtvResource;
    private readonly uint _d3d12RtvResourceVersion;

    internal D3D12GraphicsRenderTarget(D3D12GraphicsSwapchain swapchain, int index) : base(swapchain)
    {
        RenderTargetInfo.Index = index;

        _d3d12RtvDescriptorHandle = GetD3D12RtvDescriptorHandle();
        _d3d12RtvResource = CreateD3D12RtvResource(out _d3d12RtvResourceVersion);

        ID3D12Resource* CreateD3D12RtvResource(out uint d3d12RtvResourceVersion)
        {
            var d3d12RtvDesc = new D3D12_RENDER_TARGET_VIEW_DESC {
                Format = swapchain.RenderTargetFormat.AsDxgiFormat(),
                ViewDimension = D3D12_RTV_DIMENSION_TEXTURE2D,
            };

            d3d12RtvDesc.Texture2D = new D3D12_TEX2D_RTV();

            ID3D12Resource* d3d12RtvResource;
            ThrowExternalExceptionIfFailed(swapchain.DxgiSwapchain->GetBuffer((uint)RenderTargetInfo.Index, __uuidof<ID3D12Resource>(), (void**)&d3d12RtvResource));

            swapchain.Device.D3D12Device->CreateRenderTargetView(d3d12RtvResource, &d3d12RtvDesc, _d3d12RtvDescriptorHandle);
            return GetLatestD3D12Resource(d3d12RtvResource, out d3d12RtvResourceVersion);
        }

        D3D12_CPU_DESCRIPTOR_HANDLE GetD3D12RtvDescriptorHandle()
        {
            var d3d12RtvDescriptorHandleIncrementSize = Device.RtvDescriptorSize;
            var d3d12RtvDescriptorHandleForHeapStart = Swapchain.D3D12RtvDescriptorHeap->GetCPUDescriptorHandleForHeapStart();

            D3D12_CPU_DESCRIPTOR_HANDLE.InitOffsetted(out var d3d12RtvDescriptorHandle, d3d12RtvDescriptorHandleForHeapStart, RenderTargetInfo.Index, d3d12RtvDescriptorHandleIncrementSize);
            return d3d12RtvDescriptorHandle;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsRenderTarget" /> class.</summary>
    ~D3D12GraphicsRenderTarget() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="D3D12_CPU_DESCRIPTOR_HANDLE" /> for the render target.</summary>
    public D3D12_CPU_DESCRIPTOR_HANDLE D3D12RtvDescriptorHandle => _d3d12RtvDescriptorHandle;

    /// <summary>Gets the <see cref="ID3D12Resource" /> for the render target.</summary>
    public ID3D12Resource* D3D12RtvResource => _d3d12RtvResource;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsRenderPassObject.RenderPass" />
    public new D3D12GraphicsRenderPass RenderPass => base.RenderPass.As<D3D12GraphicsRenderPass>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc cref="GraphicsSwapchainObject.Swapchain" />
    public new D3D12GraphicsSwapchain Swapchain => base.Swapchain.As<D3D12GraphicsSwapchain>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        ReleaseIfNotNull(_d3d12RtvResource);
        _d3d12RtvResource = null;
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12RtvResource->SetD3D12Name(value);
    }
}
