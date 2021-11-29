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
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsSwapchain : GraphicsSwapchain
{
    private readonly ID3D12DescriptorHeap* _d3d12RtvDescriptorHeap;
    private readonly UnmanagedArray<Pointer<ID3D12Resource>> _d3d12RtvResources;
    private readonly IDXGISwapChain3* _dxgiSwapchain;
    private readonly uint _framebufferCount;
    private readonly DXGI_FORMAT _framebufferFormat;

    private uint _framebufferIndex;

    private VolatileState _state;

    internal D3D12GraphicsSwapchain(D3D12GraphicsDevice device, IGraphicsSurface surface)
        : base(device, surface)
    {
        var framebufferCount = 2u;
        _framebufferCount = framebufferCount;

        var framebufferFormat = DXGI_FORMAT_R8G8B8A8_UNORM;
        _framebufferFormat = framebufferFormat;

        var dxgiSwapchain = CreateDxgiSwapchain(device, surface, framebufferCount, framebufferFormat);
        _dxgiSwapchain = dxgiSwapchain;

        _d3d12RtvDescriptorHeap = CreateD3D12RtvDescriptorHeap(device, framebufferCount);
        _d3d12RtvResources = new UnmanagedArray<Pointer<ID3D12Resource>>(framebufferCount);

        _framebufferIndex = GetFramebufferIndex(dxgiSwapchain, Fence);

        _ = _state.Transition(to: Initialized);

        InitializeD3D12RtvResources();
        Surface.SizeChanged += OnGraphicsSurfaceSizeChanged;

        static IDXGISwapChain3* CreateDxgiSwapchain(D3D12GraphicsDevice device, IGraphicsSurface surface, uint framebufferCount, DXGI_FORMAT framebufferFormat)
        {
            IDXGISwapChain3* dxgiSwapchain;
            var surfaceHandle = (HWND)surface.Handle;

            var dxgiSwapchainDesc = new DXGI_SWAP_CHAIN_DESC1 {
                Width = (uint)surface.Width,
                Height = (uint)surface.Height,
                Format = framebufferFormat,
                SampleDesc = new DXGI_SAMPLE_DESC(count: 1, quality: 0),
                BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT,
                BufferCount = framebufferCount,
                SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL,
            };

            var service = device.Service;

            switch (surface.Kind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    ThrowExternalExceptionIfFailed(service.DxgiFactory->CreateSwapChainForHwnd(
                        (IUnknown*)device.D3D12CommandQueue,
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
            ThrowExternalExceptionIfFailed(service.DxgiFactory->MakeWindowAssociation(surfaceHandle, DXGI_MWA_NO_ALT_ENTER));

            return dxgiSwapchain;
        }

        static ID3D12DescriptorHeap* CreateD3D12RtvDescriptorHeap(D3D12GraphicsDevice device, uint framebufferCount)
        {
            ID3D12DescriptorHeap* d3d12RtvDescriptorHeap;

            var d3d12RtvDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV,
                NumDescriptors = framebufferCount,
            };
            ThrowExternalExceptionIfFailed(device.D3D12Device->CreateDescriptorHeap(&d3d12RtvDescriptorHeapDesc, __uuidof<ID3D12DescriptorHeap>(), (void**)&d3d12RtvDescriptorHeap));

            return d3d12RtvDescriptorHeap;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsSwapchain" /> class.</summary>
    ~D3D12GraphicsSwapchain() => Dispose(isDisposing: false);

    /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> used by the device for render target resources.</summary>
    public ID3D12DescriptorHeap* D3D12RtvDescriptorHeap
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12RtvDescriptorHeap;
        }
    }

    /// <summary>Gets the <see cref="ID3D12Resource" /> for the render target used by the context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public UnmanagedReadOnlySpan<Pointer<ID3D12Resource>> D3D12RenderTargetResources
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12RtvResources.AsUnmanagedSpan();
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets the <see cref="IDXGISwapChain3" /> for the device.</summary>
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

    /// <summary>Gets the <see cref="DXGI_FORMAT" /> used by <see cref="DxgiSwapchain" />.</summary>
    public DXGI_FORMAT FramebufferFormat => _framebufferFormat;

    /// <inheritdoc />
    public override uint FramebufferIndex => _framebufferIndex;

    /// <inheritdoc />
    public override void Present()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsSwapchain));

        var fence = Fence;
        fence.Wait();
        fence.Reset();

        var dxgiSwapchain = DxgiSwapchain;
        ThrowExternalExceptionIfFailed(dxgiSwapchain->Present(SyncInterval: 1, Flags: 0));

        _framebufferIndex = GetFramebufferIndex(dxgiSwapchain, fence);
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

            CleanupD3D12RtvResources();

            ReleaseIfNotNull(_d3d12RtvDescriptorHeap);
            ReleaseIfNotNull(_dxgiSwapchain);

            if (isDisposing)
            {
                Fence?.Dispose();
            }
        }

        _state.EndDispose();
    }

    private void CleanupD3D12RtvResources()
    {
        var d3d12RtvResources = _d3d12RtvResources;

        for (var i = 0u; i < d3d12RtvResources.Length; i++)
        {
            ReleaseIfNotNull(d3d12RtvResources[i]);
        }
    }

    private uint GetFramebufferIndex(IDXGISwapChain3* dxgiSwapchain, D3D12GraphicsFence fence)
    {
        Device.Signal(fence);
        return dxgiSwapchain->GetCurrentBackBufferIndex();
    }

    private void InitializeD3D12RtvResources()
    {
        var d3d12RtvResources = _d3d12RtvResources;

        var device = Device;
        var d3d12RtvDescriptorIncrementSize = device.D3D12RtvDescriptorHandleIncrementSize;
        var d3d12Device = device.D3D12Device;

        var d3d12RtvDescriptorHeapStart = D3D12RtvDescriptorHeap->GetCPUDescriptorHandleForHeapStart();

        var d3d12RtvDesc = new D3D12_RENDER_TARGET_VIEW_DESC {
            Format = FramebufferFormat,
            ViewDimension = D3D12_RTV_DIMENSION_TEXTURE2D,
            Anonymous = new D3D12_RENDER_TARGET_VIEW_DESC._Anonymous_e__Union {
                Texture2D = new D3D12_TEX2D_RTV(),
            },
        };

        var dxgiSwapchain = DxgiSwapchain;

        for (var i = 0u; i < d3d12RtvResources.Length; i++)
        {
            ID3D12Resource* rtvResource;
            ThrowExternalExceptionIfFailed(dxgiSwapchain->GetBuffer(i, __uuidof<ID3D12Resource>(), (void**)&rtvResource));

            var rtvDescriptor = new D3D12_CPU_DESCRIPTOR_HANDLE(in d3d12RtvDescriptorHeapStart, (int)i, d3d12RtvDescriptorIncrementSize);
            _ = d3d12RtvDescriptorHeapStart.Offset((int)i, d3d12RtvDescriptorIncrementSize);

            d3d12Device->CreateRenderTargetView(rtvResource, &d3d12RtvDesc, d3d12RtvDescriptorHeapStart);
            d3d12RtvResources[i] = rtvResource;
        }
    }

    private void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        CleanupD3D12RtvResources();

        var dxgiSwapchain = DxgiSwapchain;
        ThrowExternalExceptionIfFailed(dxgiSwapchain->ResizeBuffers(_framebufferCount, (uint)eventArgs.CurrentValue.X, (uint)eventArgs.CurrentValue.Y, _framebufferFormat, SwapChainFlags: 0));
        _framebufferIndex = dxgiSwapchain->GetCurrentBackBufferIndex();

        InitializeD3D12RtvResources();
    }
}
