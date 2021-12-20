// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Numerics;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.DXGI;
using static TerraFX.Interop.DirectX.DXGI_SWAP_EFFECT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsSwapchain : GraphicsSwapchain
{
    private readonly ID3D12DescriptorHeap* _d3d12RtvDescriptorHeap;
    private readonly IDXGISwapChain3* _dxgiSwapchain;
    private readonly D3D12GraphicsRenderTarget[] _renderTargets;
    private readonly GraphicsFormat _renderTargetFormat;

    private uint _renderTargetIndex;

    private VolatileState _state;

    internal D3D12GraphicsSwapchain(D3D12GraphicsRenderPass renderPass, IGraphicsSurface surface, GraphicsFormat renderTargetFormat, uint minimumRenderTargetCount = 0)
        : base(renderPass, surface)
    {
        var renderTargetCount = minimumRenderTargetCount;

        var dxgiSwapchain = CreateDxgiSwapchain(renderPass, surface, ref renderTargetCount, renderTargetFormat);
        _dxgiSwapchain = dxgiSwapchain;

        _d3d12RtvDescriptorHeap = CreateD3D12RtvDescriptorHeap(renderPass, renderTargetCount);        
        _renderTargets = new D3D12GraphicsRenderTarget[renderTargetCount];

        _renderTargetFormat = renderTargetFormat;
        _renderTargetIndex = GetRenderTargetIndex(dxgiSwapchain, Fence);

        _ = _state.Transition(to: Initialized);

        InitializeRenderTargets(this, _renderTargets);
        Surface.SizeChanged += OnGraphicsSurfaceSizeChanged;

        static IDXGISwapChain3* CreateDxgiSwapchain(D3D12GraphicsRenderPass renderPass, IGraphicsSurface surface, ref uint renderTargetCount, GraphicsFormat renderTargetFormat)
        {
            IDXGISwapChain3* dxgiSwapchain;
            var surfaceHandle = (HWND)surface.Handle;

            if (renderTargetCount == 0)
            {
                renderTargetCount = 2;
            }

            var dxgiSwapchainDesc = new DXGI_SWAP_CHAIN_DESC1 {
                Width = (uint)surface.Width,
                Height = (uint)surface.Height,
                Format = renderTargetFormat.AsDxgiFormat(),
                SampleDesc = new DXGI_SAMPLE_DESC(count: 1, quality: 0),
                BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT,
                BufferCount = renderTargetCount,
                SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL,
            };

            var dxgiFactory = renderPass.Service.DxgiFactory;

            switch (surface.Kind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    ThrowExternalExceptionIfFailed(dxgiFactory->CreateSwapChainForHwnd(
                        (IUnknown*)renderPass.Device.D3D12DirectCommandQueue,
                        surfaceHandle,
                        &dxgiSwapchainDesc,
                        pFullscreenDesc: null,
                        pRestrictToOutput: null,
                        (IDXGISwapChain1**)&dxgiSwapchain)
                    );
                    break;
                }

                default:
                {
                    ThrowForInvalidKind(surface.Kind);
                    dxgiSwapchain = null;
                    break;
                }
            }

            // Fullscreen transitions are not currently supported
            ThrowExternalExceptionIfFailed(dxgiFactory->MakeWindowAssociation(surfaceHandle, DXGI_MWA_NO_ALT_ENTER));

            return dxgiSwapchain;
        }

        static ID3D12DescriptorHeap* CreateD3D12RtvDescriptorHeap(D3D12GraphicsRenderPass renderPass, uint renderTargetCount)
        {
            ID3D12DescriptorHeap* d3d12RtvDescriptorHeap;

            var d3d12RtvDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV,
                NumDescriptors = renderTargetCount,
            };
            ThrowExternalExceptionIfFailed(renderPass.Device.D3D12Device->CreateDescriptorHeap(&d3d12RtvDescriptorHeapDesc, __uuidof<ID3D12DescriptorHeap>(), (void**)&d3d12RtvDescriptorHeap));

            return d3d12RtvDescriptorHeap;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsSwapchain" /> class.</summary>
    ~D3D12GraphicsSwapchain() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> used by the swapchain for render target resources.</summary>
    public ID3D12DescriptorHeap* D3D12RtvDescriptorHeap
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12RtvDescriptorHeap;
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets the <see cref="IDXGISwapChain3" /> for the swapchain.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public IDXGISwapChain3* DxgiSwapchain
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _dxgiSwapchain;
        }
    }

    /// <inheritdoc cref="GraphicsSwapchain.Fence" />
    public new D3D12GraphicsFence Fence => base.Fence.As<D3D12GraphicsFence>();

    /// <inheritdoc cref="GraphicsRenderPassObject.RenderPass" />
    public new D3D12GraphicsRenderPass RenderPass => base.RenderPass.As<D3D12GraphicsRenderPass>();

    /// <inheritdoc />
    public override D3D12GraphicsRenderTarget RenderTarget => _renderTargets.GetReference(_renderTargetIndex);

    /// <inheritdoc />
    public override uint RenderTargetCount => (uint)_renderTargets.Length;

    /// <inheritdoc />
    public override GraphicsFormat RenderTargetFormat => _renderTargetFormat;

    /// <inheritdoc />
    public override uint RenderTargetIndex => _renderTargetIndex;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    private static void CleanupRenderTargets(D3D12GraphicsRenderTarget[] renderTargets)
    {
        for (var index = 0; index < renderTargets.Length; index++)
        {
            renderTargets[index].Dispose();
            renderTargets[index] = null!;
        }
    }

    private static void InitializeRenderTargets(D3D12GraphicsSwapchain swapchain, D3D12GraphicsRenderTarget[] renderTargets)
    {
        for (var index = 0; index < renderTargets.Length; index++)
        {
            renderTargets[index] = new D3D12GraphicsRenderTarget(swapchain, (uint)index);
        }
    }

    /// <inheritdoc />
    public override D3D12GraphicsRenderTarget GetRenderTarget(uint index)
    {
        ThrowIfNotInBounds(index, (uint)_renderTargets.Length);
        return _renderTargets[index];
    }

    /// <inheritdoc />
    public override void Present()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsSwapchain));

        var fence = Fence;
        fence.Wait();
        fence.Reset();

        var dxgiSwapchain = DxgiSwapchain;
        ThrowExternalExceptionIfFailed(dxgiSwapchain->Present(SyncInterval: 1, Flags: 0));

        _renderTargetIndex = GetRenderTargetIndex(dxgiSwapchain, fence);
    }

    /// <inheritdoc />
    public override void SetName(string value)
    {
        value = DxgiSwapchain->UpdateDXGIName(value);
        _ = D3D12RtvDescriptorHeap->UpdateD3D12Name(value);
        base.SetName(value);
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

            CleanupRenderTargets(_renderTargets);

            ReleaseIfNotNull(_d3d12RtvDescriptorHeap);
            ReleaseIfNotNull(_dxgiSwapchain);

            if (isDisposing)
            {
                Fence?.Dispose();
            }
        }

        _state.EndDispose();
    }

    private uint GetRenderTargetIndex(IDXGISwapChain3* dxgiSwapchain, D3D12GraphicsFence fence)
    {
        Device.Signal(fence);
        return dxgiSwapchain->GetCurrentBackBufferIndex();
    }

    private void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        var renderTargets = _renderTargets;
        CleanupRenderTargets(renderTargets);

        var dxgiSwapchain = DxgiSwapchain;
        ThrowExternalExceptionIfFailed(dxgiSwapchain->ResizeBuffers((uint)renderTargets.Length, (uint)eventArgs.CurrentValue.X, (uint)eventArgs.CurrentValue.Y, _renderTargetFormat.AsDxgiFormat(), SwapChainFlags: 0));
        _renderTargetIndex = GetRenderTargetIndex(dxgiSwapchain, fence);

        InitializeRenderTargets(this, renderTargets);
    }
}
