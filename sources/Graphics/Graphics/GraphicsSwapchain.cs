// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Numerics;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.DXGI;
using static TerraFX.Interop.DirectX.DXGI_ALPHA_MODE;
using static TerraFX.Interop.DirectX.DXGI_SCALING;
using static TerraFX.Interop.DirectX.DXGI_SWAP_EFFECT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A swapchain which manages framebuffers for a graphics device.</summary>
public sealed unsafe class GraphicsSwapchain : IDisposable, INameable
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsDevice _device;
    private readonly GraphicsRenderPass _renderPass;
    private readonly GraphicsService _service;

    private int _currentRenderTargetIndex;

    private ComPtr<ID3D12DescriptorHeap> _d3d12DsvDescriptorHeap;
    private readonly uint _d3d12DsvDescriptorHeapVersion;

    private ComPtr<ID3D12DescriptorHeap> _d3d12RtvDescriptorHeap;
    private readonly uint _d3d12RtvDescriptorHeapVersion;

    private ComPtr<IDXGISwapChain1> _dxgiSwapchain;
    private readonly uint _dxgiSwapchainVersion;

    private DXGI_SWAP_CHAIN_DESC1 _dxgiSwapchainDesc;

    private GraphicsFence _fence;

    private readonly uint _minimumRenderTargetCount;

    private GraphicsRenderTarget[] _renderTargets;

    private readonly GraphicsFormat _renderTargetFormat;

    private readonly IGraphicsSurface _surface;

    private string _name;
    private VolatileState _state;

    internal GraphicsSwapchain(GraphicsRenderPass renderPass, in GraphicsSwapchainCreateOptions createOptions)
    {
        AssertNotNull(renderPass);
        _renderPass = renderPass;

        var device = renderPass.Device;
        _device = device;

        var adapter = device.Adapter;
        _adapter = adapter;

        var service = adapter.Service;
        _service = service;

        _minimumRenderTargetCount = createOptions.MinimumRenderTargetCount;

        _fence = Device.CreateFence(isSignaled: false);
        _renderTargetFormat = createOptions.RenderTargetFormat;
        _renderTargets = [];
        _surface = createOptions.Surface;

        var dxgiSwapchain = CreateDxgiSwapchain(out var renderTargetCount, out _dxgiSwapchainVersion);
        _dxgiSwapchain.Attach(dxgiSwapchain);

        var d3d12DsvDescriptorHeap = CreateD3D12DsvDescriptorHeap(out _d3d12DsvDescriptorHeapVersion);
        _d3d12DsvDescriptorHeap.Attach(d3d12DsvDescriptorHeap);

        var d3d12RtvDescriptorHeap = CreateD3D12RtvDescriptorHeap(out _d3d12RtvDescriptorHeapVersion);
        _d3d12RtvDescriptorHeap.Attach(d3d12RtvDescriptorHeap);

        _renderTargets = new GraphicsRenderTarget[renderTargetCount];
        InitializeRenderTargets();

        _name = GetType().Name;
        SetNameUnsafe(Name);

        _currentRenderTargetIndex = GetCurrentRenderTargetIndex();

        Surface.SizeChanged += OnGraphicsSurfaceSizeChanged;
        _ = _state.Transition(VolatileState.Initialized);

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
                case GraphicsSurfaceKind.Unknown:
                case GraphicsSurfaceKind.Android:
                case GraphicsSurfaceKind.Wayland:
                case GraphicsSurfaceKind.Xcb:
                case GraphicsSurfaceKind.Xlib:
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

    /// <summary>Finalizes an instance of the <see cref="GraphicsSwapchain" /> class.</summary>
    ~GraphicsSwapchain() => Dispose(isDisposing: false);

    /// <summary>Gets the adapter for which the object was created.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the current render target of the swapchain.</summary>
    public GraphicsRenderTarget CurrentRenderTarget => _renderTargets[_currentRenderTargetIndex];

    /// <summary>Gets the index of the current render target.</summary>
    public int CurrentRenderTargetIndex => _currentRenderTargetIndex;

    /// <summary>Gets the device for which the object was created.</summary>
    public GraphicsDevice Device => _device;

    /// <summary>Gets the fence used to synchronize the swapchain.</summary>
    public GraphicsFence Fence => _fence;

    /// <summary>Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.</summary>
    public bool IsDisposed => _state.IsDisposedOrDisposing;

    /// <inheritdoc />
    [AllowNull]
    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value ?? GetType().Name;
            SetNameUnsafe(_name);
        }
    }

    /// <summary>Gets the render pass for which the object was created.</summary>
    public GraphicsRenderPass RenderPass => _renderPass;

    /// <summary>Gets the render targets for the swapchain.</summary>
    public ReadOnlySpan<GraphicsRenderTarget> RenderTargets => _renderTargets;

    /// <summary>Gets the format for the render targets of the swapchain.</summary>
    public GraphicsFormat RenderTargetFormat => _renderTargetFormat;

    /// <summary>Gets the service for which the object was created.</summary>
    public GraphicsService Service => _service;

    /// <summary>Gets the surface on which the swapchain can render.</summary>
    public IGraphicsSurface Surface => _surface;

    internal ID3D12DescriptorHeap* D3D12DsvDescriptorHeap => _d3d12DsvDescriptorHeap;

    internal uint D3D12DsvDescriptorHeapVersion => _d3d12DsvDescriptorHeapVersion;

    internal ID3D12DescriptorHeap* D3D12RtvDescriptorHeap => _d3d12RtvDescriptorHeap;

    internal uint D3D12RtvDescriptorHeapVersion => _d3d12RtvDescriptorHeapVersion;

    internal IDXGISwapChain1* DxgiSwapchain => _dxgiSwapchain;

    internal ref readonly DXGI_SWAP_CHAIN_DESC1 DxgiSwapchainDesc => ref _dxgiSwapchainDesc;

    internal uint DxgiSwapchainVersion => _dxgiSwapchainVersion;

    /// <inheritdoc />
    public void Dispose()
    {
        _ = _state.BeginDispose();
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        _state.EndDispose();
    }

    /// <summary>Presents the current framebuffer.</summary>
    /// <exception cref="ObjectDisposedException">The swapchain has been disposed.</exception>
    public void Present()
    {
        ThrowIfDisposedOrDisposing(_state, _name);
        PresentUnsafe();
    }

    /// <inheritdoc />
    public override string ToString() => _name;

    private void CleanupRenderTargets()
    {
        var renderTargets = _renderTargets;

        for (var index = 0; index < renderTargets.Length; index++)
        {
            renderTargets[index].Dispose();
            renderTargets[index] = null!;
        }
    }

    private void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            var fence = _fence;
            fence.Wait();
            fence.Reset();

            fence.Dispose();
            _fence = null!;

            CleanupRenderTargets();
            _renderTargets = null!;
        }

        _ = _d3d12DsvDescriptorHeap.Reset();
        _ = _d3d12RtvDescriptorHeap.Reset();
        _ = _dxgiSwapchain.Reset();
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
            var currentRenderTargetIndex = _currentRenderTargetIndex;
            currentRenderTargetIndex += 1;

            if (currentRenderTargetIndex > _renderTargets.Length)
            {
                currentRenderTargetIndex = 0;
            }
            return currentRenderTargetIndex;
        }
    }

    private void InitializeRenderTargets()
    {
        var renderTargets = _renderTargets;

        for (var index = 0; index < renderTargets.Length; index++)
        {
            renderTargets[index] = new GraphicsRenderTarget(this, index);
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
            (uint)_renderTargets.Length,
            (uint)eventArgs.CurrentValue.X,
            (uint)eventArgs.CurrentValue.Y,
            _renderTargetFormat.AsDxgiFormat(),
            SwapChainFlags: 0
        ));

        DXGI_SWAP_CHAIN_DESC1 dxgiSwapchainDesc;
        ThrowExternalExceptionIfFailed(dxgiSwapchain->GetDesc1(&dxgiSwapchainDesc));
        _dxgiSwapchainDesc = dxgiSwapchainDesc;

        _currentRenderTargetIndex = GetCurrentRenderTargetIndex();

        InitializeRenderTargets();
    }

    private void PresentUnsafe()
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        ThrowExternalExceptionIfFailed(DxgiSwapchain->Present(SyncInterval: 1, Flags: 0));
        _currentRenderTargetIndex = GetCurrentRenderTargetIndex();
    }

    private void SetNameUnsafe(string value)
    {
        DxgiSwapchain->SetDxgiName(value);
        D3D12RtvDescriptorHeap->SetD3D12Name(value);
    }
}
