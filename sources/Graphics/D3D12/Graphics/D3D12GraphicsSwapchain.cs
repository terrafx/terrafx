// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Numerics;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_RTV_DIMENSION;
using static TerraFX.Interop.DirectX.DXGI;
using static TerraFX.Interop.DirectX.DXGI_FORMAT;
using static TerraFX.Interop.DirectX.DXGI_SWAP_EFFECT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsSwapchain : GraphicsSwapchain
{
    private readonly uint _framebufferCount;

    private ValueLazy<Pointer<ID3D12DescriptorHeap>> _d3d12RtvDescriptorHeap;
    private ValueLazy<UnmanagedArray<Pointer<ID3D12Resource>>> _d3d12RtvResources;
    private ValueLazy<Pointer<IDXGISwapChain3>> _dxgiSwapchain;

    private DXGI_FORMAT _framebufferFormat;
    private uint _framebufferIndex;

    private VolatileState _state;

    internal D3D12GraphicsSwapchain(D3D12GraphicsDevice device, IGraphicsSurface surface)
        : base(device, surface)
    {
        _framebufferCount = 2;

        _d3d12RtvDescriptorHeap = new ValueLazy<Pointer<ID3D12DescriptorHeap>>(CreateD3D12RtvDescriptorHeap);
        _d3d12RtvResources = new ValueLazy<UnmanagedArray<Pointer<ID3D12Resource>>>(CreateD3D12RtvResources);
        _dxgiSwapchain = new ValueLazy<Pointer<IDXGISwapChain3>>(CreateDxgiSwapchain);

        _ = _state.Transition(to: Initialized);

        Surface.SizeChanged += OnGraphicsSurfaceSizeChanged;
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsSwapchain" /> class.</summary>
    ~D3D12GraphicsSwapchain() => Dispose(isDisposing: false);

    /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> used by the device for render target resources.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public ID3D12DescriptorHeap* D3D12RtvDescriptorHeap => _d3d12RtvDescriptorHeap.Value;

    /// <summary>Gets the <see cref="ID3D12Resource" /> for the render target used by the context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public UnmanagedSpan<Pointer<ID3D12Resource>> D3D12RenderTargetResources => _d3d12RtvResources.Value.AsUnmanagedSpan();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => (D3D12GraphicsDevice)base.Device;

    /// <summary>Gets the <see cref="IDXGISwapChain3" /> for the device.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public IDXGISwapChain3* DxgiSwapchain => _dxgiSwapchain.Value;

    /// <inheritdoc cref="GraphicsSwapchain.Fence" />
    public new D3D12GraphicsFence Fence => (D3D12GraphicsFence)base.Fence;

    /// <summary>Gets the <see cref="DXGI_FORMAT" /> used by <see cref="DxgiSwapchain" />.</summary>
    public DXGI_FORMAT FramebufferFormat => _framebufferFormat;

    /// <inheritdoc />
    public override uint FramebufferIndex => _framebufferIndex;

    /// <inheritdoc />
    public override void Present()
    {
        ThrowExternalExceptionIfFailed(DxgiSwapchain->Present(SyncInterval: 1, Flags: 0));
        Device.Signal(Fence);
        _framebufferIndex = DxgiSwapchain->GetCurrentBackBufferIndex();
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            _d3d12RtvDescriptorHeap.Dispose(ReleaseIfNotNull);
            _dxgiSwapchain.Dispose(ReleaseIfNotNull);
        }

        _state.EndDispose();
    }

    private Pointer<ID3D12DescriptorHeap> CreateD3D12RtvDescriptorHeap()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));

        ID3D12DescriptorHeap* rtvDescriptorHeap;

        var rtvDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
            Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV,
            NumDescriptors = _framebufferCount,
        };
        ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateDescriptorHeap(&rtvDescriptorHeapDesc, __uuidof<ID3D12DescriptorHeap>(), (void**)&rtvDescriptorHeap));

        return rtvDescriptorHeap;
    }

    private UnmanagedArray<Pointer<ID3D12Resource>> CreateD3D12RtvResources()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsContext));

        var rtvResources = new UnmanagedArray<Pointer<ID3D12Resource>>(_framebufferCount);

        var device = Device;
        var framebufferFormat = FramebufferFormat;

        var rtvDescriptorIncrementSize = device.D3D12RtvDescriptorHandleIncrementSize;
        var d3d12Device = device.D3D12Device;

        var firstRtvDescriptor = D3D12RtvDescriptorHeap->GetCPUDescriptorHandleForHeapStart();

        var rtvDesc = new D3D12_RENDER_TARGET_VIEW_DESC {
            Format = framebufferFormat,
            ViewDimension = D3D12_RTV_DIMENSION_TEXTURE2D,
            Anonymous = new D3D12_RENDER_TARGET_VIEW_DESC._Anonymous_e__Union {
                Texture2D = new D3D12_TEX2D_RTV(),
            },
        };

        for (var i = 0u; i < _framebufferCount; i++)
        {
            ID3D12Resource* rtvResource;
            ThrowExternalExceptionIfFailed(DxgiSwapchain->GetBuffer(i, __uuidof<ID3D12Resource>(), (void**)&rtvResource));

            var rtvDescriptor = new D3D12_CPU_DESCRIPTOR_HANDLE(in firstRtvDescriptor, (int)i, rtvDescriptorIncrementSize);
            _ = firstRtvDescriptor.Offset((int)i, rtvDescriptorIncrementSize);

            d3d12Device->CreateRenderTargetView(rtvResource, &rtvDesc, firstRtvDescriptor);
            rtvResources[i] = rtvResource;
        }

        return rtvResources;
    }

    private Pointer<IDXGISwapChain3> CreateDxgiSwapchain()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));

        IDXGISwapChain3* dxgiSwapChain;

        var surface = Surface;
        var surfaceHandle = (HWND)surface.Handle;

        var swapchainDesc = new DXGI_SWAP_CHAIN_DESC1 {
            Width = (uint)surface.Width,
            Height = (uint)surface.Height,
            Format = DXGI_FORMAT_R8G8B8A8_UNORM,
            SampleDesc = new DXGI_SAMPLE_DESC(count: 1, quality: 0),
            BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT,
            BufferCount = _framebufferCount,
            SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL,
        };

        var service = Device.Adapter.Service;

        switch (surface.Kind)
        {
            case GraphicsSurfaceKind.Win32:
            {
                ThrowExternalExceptionIfFailed(service.DxgiFactory->CreateSwapChainForHwnd((IUnknown*)Device.D3D12CommandQueue, surfaceHandle, &swapchainDesc, pFullscreenDesc: null, pRestrictToOutput: null, (IDXGISwapChain1**)&dxgiSwapChain));
                break;
            }

            default:
            {
                ThrowForInvalidKind(surface.Kind);
                dxgiSwapChain = null;
                break;
            }
        }

        // Fullscreen transitions are not currently supported
        ThrowExternalExceptionIfFailed(service.DxgiFactory->MakeWindowAssociation(surfaceHandle, DXGI_MWA_NO_ALT_ENTER));

        _framebufferFormat = swapchainDesc.Format;
        _framebufferIndex = dxgiSwapChain->GetCurrentBackBufferIndex();

        return dxgiSwapChain;
    }

    private void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
    {
        Device.WaitForIdle();

        if (_d3d12RtvResources.IsValueCreated)
        {
            var d3d12RtvResources = _d3d12RtvResources.Value;

            for (var i = 0u; i < d3d12RtvResources.Length; i++)
            {
                var d3d12RenderTargetResource = d3d12RtvResources[i];
                ReleaseIfNotNull(d3d12RenderTargetResource);
            }

            _d3d12RtvResources.Reset(CreateD3D12RtvResources);
        }

        if (_dxgiSwapchain.IsValueCreated)
        {
            var dxgiSwapchain = DxgiSwapchain;
            ThrowExternalExceptionIfFailed(dxgiSwapchain->ResizeBuffers(_framebufferCount, (uint)eventArgs.CurrentValue.X, (uint)eventArgs.CurrentValue.Y, _framebufferFormat, SwapChainFlags: 0));
            _framebufferIndex = dxgiSwapchain->GetCurrentBackBufferIndex();
        }
    }
}
