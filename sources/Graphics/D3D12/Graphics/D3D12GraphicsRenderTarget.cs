// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12_RTV_DIMENSION;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsRenderTarget : GraphicsRenderTarget
{
    private readonly D3D12_CPU_DESCRIPTOR_HANDLE _d3d12RtvDescriptorHandle;
    private readonly ID3D12Resource* _d3d12RtvResource;

    private VolatileState _state;

    internal D3D12GraphicsRenderTarget(D3D12GraphicsSwapchain swapchain, uint index)
        : base(swapchain, index)
    {
        var d3d12RtvDescriptorHandle = GetD3D12RtvDescriptorHandle(swapchain, index);
        _d3d12RtvDescriptorHandle = d3d12RtvDescriptorHandle;

        _d3d12RtvResource = CreateD3D12RtvResource(swapchain, index, d3d12RtvDescriptorHandle);

        _ = _state.Transition(to: Initialized);

        static ID3D12Resource* CreateD3D12RtvResource(D3D12GraphicsSwapchain swapchain, uint index, D3D12_CPU_DESCRIPTOR_HANDLE d3d12RtvDescriptorHandle)
        {
            var d3d12RtvDesc = new D3D12_RENDER_TARGET_VIEW_DESC {
                Format = swapchain.RenderTargetFormat.AsDxgiFormat(),
                ViewDimension = D3D12_RTV_DIMENSION_TEXTURE2D,
                Anonymous = new D3D12_RENDER_TARGET_VIEW_DESC._Anonymous_e__Union {
                    Texture2D = new D3D12_TEX2D_RTV(),
                },
            };

            ID3D12Resource* d3d12RtvResource;
            ThrowExternalExceptionIfFailed(swapchain.DxgiSwapchain->GetBuffer(index, __uuidof<ID3D12Resource>(), (void**)&d3d12RtvResource));

            swapchain.Device.D3D12Device->CreateRenderTargetView(d3d12RtvResource, &d3d12RtvDesc, d3d12RtvDescriptorHandle);
            return d3d12RtvResource;
        }

        static D3D12_CPU_DESCRIPTOR_HANDLE GetD3D12RtvDescriptorHandle(D3D12GraphicsSwapchain swapchain, uint index)
        {
            var d3d12RtvDescriptorHandleIncrementSize = swapchain.Device.D3D12RtvDescriptorHandleIncrementSize;
            var d3d12RtvDescriptorHandleForHeapStart = swapchain.D3D12RtvDescriptorHeap->GetCPUDescriptorHandleForHeapStart();

            D3D12_CPU_DESCRIPTOR_HANDLE.InitOffsetted(out var d3d12RtvDescriptorHandle, d3d12RtvDescriptorHandleForHeapStart, (int)index, d3d12RtvDescriptorHandleIncrementSize);
            return d3d12RtvDescriptorHandle;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsRenderTarget" /> class.</summary>
    ~D3D12GraphicsRenderTarget() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsSwapchainObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="D3D12_CPU_DESCRIPTOR_HANDLE" /> for the render target.</summary>
    public D3D12_CPU_DESCRIPTOR_HANDLE D3D12RtvDescriptorHandle => _d3d12RtvDescriptorHandle;

    /// <summary>Gets the <see cref="ID3D12Resource" /> for the render target.</summary>
    public ID3D12Resource* D3D12RtvResource => _d3d12RtvResource;

    /// <inheritdoc cref="GraphicsSwapchainObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsSwapchainObject.RenderPass" />
    public new D3D12GraphicsRenderPass RenderPass => base.RenderPass.As<D3D12GraphicsRenderPass>();

    /// <inheritdoc cref="GraphicsSwapchainObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc cref="GraphicsSwapchainObject.Swapchain" />
    public new D3D12GraphicsSwapchain Swapchain => base.Swapchain.As<D3D12GraphicsSwapchain>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            ReleaseIfNotNull(_d3d12RtvResource);
        }

        _state.EndDispose();
    }
}
