// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Numerics;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.DXGI;
using static TerraFX.Interop.DirectX.DXGI_ALPHA_MODE;
using static TerraFX.Interop.DirectX.DXGI_SCALING;
using static TerraFX.Interop.DirectX.DXGI_SWAP_EFFECT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsSwapchain : GraphicsSwapchain
{
    private readonly uint _minimumRenderTargetCount;
    private DXGI_SWAP_CHAIN_DESC1 _dxgiSwapchainDesc;

    private ComPtr<ID3D12DescriptorHeap> _d3d12DsvDescriptorHeap;
    private readonly uint _d3d12DsvDescriptorHeapVersion;

    private ComPtr<ID3D12DescriptorHeap> _d3d12RtvDescriptorHeap;
    private readonly uint _d3d12RtvDescriptorHeapVersion;

    private ComPtr<IDXGISwapChain1> _dxgiSwapchain;
    private readonly uint _dxgiSwapchainVersion;

    internal D3D12GraphicsSwapchain(D3D12GraphicsRenderPass renderPass, in D3D12GraphicsSwapchainCreateOptions createOptions) : base(renderPass)
    {
        _minimumRenderTargetCount = createOptions.MinimumRenderTargetCount;

        SwapchainInfo.Fence = Device.CreateFence(isSignalled: false);
        SwapchainInfo.RenderTargetFormat = createOptions.RenderTargetFormat;
        SwapchainInfo.RenderTargets = Array.Empty<D3D12GraphicsRenderTarget>();
        SwapchainInfo.Surface = createOptions.Surface;

        _dxgiSwapchain = CreateDxgiSwapchain(out var renderTargetCount, out _dxgiSwapchainVersion);
        _d3d12DsvDescriptorHeap = CreateD3D12DsvDescriptorHeap(out _d3d12DsvDescriptorHeapVersion);
        _d3d12RtvDescriptorHeap = CreateD3D12RtvDescriptorHeap(out _d3d12RtvDescriptorHeapVersion);

        SwapchainInfo.RenderTargets = new GraphicsRenderTarget[renderTargetCount];
        InitializeRenderTargets();

        SetNameUnsafe(Name);
        SwapchainInfo.CurrentRenderTargetIndex = GetCurrentRenderTargetIndex();

        Surface.SizeChanged += OnGraphicsSurfaceSizeChanged;

        IDXGISwapChain1* CreateDxgiSwapchain(out uint renderTargetCount, out uint dxgiSwapchainVersion)
        {
            IDXGISwapChain1* dxgiSwapchain;

            var minimumRenderTargetCount = _minimumRenderTargetCount;

            if (minimumRenderTargetCount < 2)
            {
                // DirectX 12 requires at least 2 backbuffers for the flip model swapchains
                minimumRenderTargetCount = 2;
            }

            ThrowIfNotInInsertBounds(minimumRenderTargetCount, DXGI_MAX_SWAP_CHAIN_BUFFERS);
            renderTargetCount = minimumRenderTargetCount;

            var surface = Surface;
            var surfaceHandle = (HWND)surface.Handle;

            var dxgiSwapchainDesc = new DXGI_SWAP_CHAIN_DESC1 {
                Width = (uint)surface.PixelWidth,
                Height = (uint)surface.PixelHeight,
                Format = RenderTargetFormat.AsDxgiFormat(),
                Stereo = BOOL.FALSE,
                SampleDesc = new DXGI_SAMPLE_DESC(count: 1, quality: 0),
                BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT,
                BufferCount = renderTargetCount,
                Scaling = DXGI_SCALING_NONE,
                SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL,
                AlphaMode = DXGI_ALPHA_MODE_UNSPECIFIED,
                Flags = 0,
            };

            var dxgiFactory = renderPass.Service.DxgiFactory;

            switch (surface.Kind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    ThrowExternalExceptionIfFailed(dxgiFactory->CreateSwapChainForHwnd(
                        (IUnknown*)Device.RenderCommandQueue.D3D12CommandQueue,
                        surfaceHandle,
                        &dxgiSwapchainDesc,
                        pFullscreenDesc: null,
                        pRestrictToOutput: null,
                        &dxgiSwapchain
                    ));
                    break;
                }

                default:
                {
                    ThrowForInvalidKind(surface.Kind);
                    dxgiSwapchain = null;
                    break;
                }
            }

            ThrowExternalExceptionIfFailed(dxgiSwapchain->GetDesc1(&dxgiSwapchainDesc));
            _dxgiSwapchainDesc = dxgiSwapchainDesc;

            // Fullscreen transitions are not currently supported
            ThrowExternalExceptionIfFailed(dxgiFactory->MakeWindowAssociation(surfaceHandle, DXGI_MWA_NO_ALT_ENTER));

            return GetLatestDxgiSwapchain(dxgiSwapchain, out dxgiSwapchainVersion);
        }

        ID3D12DescriptorHeap* CreateD3D12DsvDescriptorHeap(out uint d3d12DsvDescriptorHeapVersion)
        {
            ID3D12DescriptorHeap* d3d12DsvDescriptorHeap;

            var d3d12DsvDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_DSV,
                NumDescriptors = _dxgiSwapchainDesc.BufferCount + 1,
                Flags = D3D12_DESCRIPTOR_HEAP_FLAG_NONE,
                NodeMask = 0,
            };
            ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateDescriptorHeap(&d3d12DsvDescriptorHeapDesc, __uuidof<ID3D12DescriptorHeap>(), (void**)&d3d12DsvDescriptorHeap));

            return GetLatestD3D12DescriptorHeap(d3d12DsvDescriptorHeap, out d3d12DsvDescriptorHeapVersion);
        }

        ID3D12DescriptorHeap* CreateD3D12RtvDescriptorHeap(out uint d3d12RtvDescriptorHeapVersion)
        {
            ID3D12DescriptorHeap* d3d12RtvDescriptorHeap;

            var d3d12RtvDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV,
                NumDescriptors = _dxgiSwapchainDesc.BufferCount,
                Flags = D3D12_DESCRIPTOR_HEAP_FLAG_NONE,
                NodeMask = 0,
            };
            ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateDescriptorHeap(&d3d12RtvDescriptorHeapDesc, __uuidof<ID3D12DescriptorHeap>(), (void**)&d3d12RtvDescriptorHeap));

            return GetLatestD3D12DescriptorHeap(d3d12RtvDescriptorHeap, out d3d12RtvDescriptorHeapVersion);
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsSwapchain" /> class.</summary>
    ~D3D12GraphicsSwapchain() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <inheritdoc cref="GraphicsSwapchain.CurrentRenderTarget" />
    public new D3D12GraphicsRenderTarget CurrentRenderTarget => base.CurrentRenderTarget.As<D3D12GraphicsRenderTarget>();

    /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> used by the swapchain for depth-stencil views.</summary>
    public ID3D12DescriptorHeap* D3D12DsvDescriptorHeap => _d3d12DsvDescriptorHeap;

    /// <summary>Gets the interface version of <see cref="D3D12DsvDescriptorHeap" />.</summary>
    public uint D3D12DsvDescriptorHeapVersion => _d3d12DsvDescriptorHeapVersion;

    /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> used by the swapchain for render target views.</summary>
    public ID3D12DescriptorHeap* D3D12RtvDescriptorHeap => _d3d12RtvDescriptorHeap;

    /// <summary>Gets the interface version of <see cref="D3D12RtvDescriptorHeap" />.</summary>
    public uint D3D12RtvDescriptorHeapVersion => _d3d12RtvDescriptorHeapVersion;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets the <see cref="IDXGISwapChain1" /> for the swapchain.</summary>
    public IDXGISwapChain1* DxgiSwapchain => _dxgiSwapchain;

    /// <summary>Gets the <see cref="DXGI_SWAP_CHAIN_DESC1" /> for <see cref="DxgiSwapchain" /></summary>
    public ref readonly DXGI_SWAP_CHAIN_DESC1 DxgiSwapchainDesc => ref _dxgiSwapchainDesc;

    /// <summary>Gets the interface version of <see cref="DxgiSwapchain" />.</summary>
    public uint DxgiSwapchainVersion => _dxgiSwapchainVersion;

    /// <inheritdoc cref="GraphicsSwapchain.Fence" />
    public new D3D12GraphicsFence Fence => base.Fence.As<D3D12GraphicsFence>();

    /// <inheritdoc cref="GraphicsRenderPassObject.RenderPass" />
    public new D3D12GraphicsRenderPass RenderPass => base.RenderPass.As<D3D12GraphicsRenderPass>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            var fence = SwapchainInfo.Fence;
            fence.Wait();
            fence.Reset();

            fence.Dispose();
            SwapchainInfo.Fence = null!;

            CleanupRenderTargets();
            SwapchainInfo.RenderTargets = null!;
        }

        _ = _d3d12RtvDescriptorHeap.Reset();
        _ = _dxgiSwapchain.Reset();
    }

    /// <inheritdoc />
    protected override void PresentUnsafe()
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        ThrowExternalExceptionIfFailed(DxgiSwapchain->Present(SyncInterval: 1, Flags: 0));
        SwapchainInfo.CurrentRenderTargetIndex = GetCurrentRenderTargetIndex();
    }

    private int GetCurrentRenderTargetIndex()
    {
        Device.RenderCommandQueue.SignalFence(Fence);

        if (_dxgiSwapchainVersion >= 3)
        {
            var dxgiSwapchain3 = (IDXGISwapChain3*)DxgiSwapchain;
            return (int)dxgiSwapchain3->GetCurrentBackBufferIndex();
        }
        else
        {
            var currentRenderTargetIndex = SwapchainInfo.CurrentRenderTargetIndex;
            currentRenderTargetIndex += 1;

            if (currentRenderTargetIndex > SwapchainInfo.RenderTargets.Length)
            {
                currentRenderTargetIndex = 0;
            }
            return currentRenderTargetIndex;
        }
    }

    private void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        CleanupRenderTargets();

        var dxgiSwapchain = DxgiSwapchain;

        ThrowExternalExceptionIfFailed(dxgiSwapchain->ResizeBuffers(
            (uint)SwapchainInfo.RenderTargets.Length,
            (uint)eventArgs.CurrentValue.X,
            (uint)eventArgs.CurrentValue.Y,
            SwapchainInfo.RenderTargetFormat.AsDxgiFormat(),
            SwapChainFlags: 0
        ));

        DXGI_SWAP_CHAIN_DESC1 dxgiSwapchainDesc;
        ThrowExternalExceptionIfFailed(dxgiSwapchain->GetDesc1(&dxgiSwapchainDesc));
        _dxgiSwapchainDesc = dxgiSwapchainDesc;

        SwapchainInfo.CurrentRenderTargetIndex = GetCurrentRenderTargetIndex();

        InitializeRenderTargets();
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        DxgiSwapchain->SetDxgiName(value);
        D3D12RtvDescriptorHeap->SetD3D12Name(value);
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

    private void InitializeRenderTargets()
    {
        var renderTargets = SwapchainInfo.RenderTargets;

        for (var index = 0; index < renderTargets.Length; index++)
        {
            renderTargets[index] = new D3D12GraphicsRenderTarget(this, index);
        }
    }
}
