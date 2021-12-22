// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Advanced;
using TerraFX.Interop.DirectX;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_LIST_TYPE;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsComputeContext : GraphicsComputeContext
{
    private readonly ID3D12CommandAllocator* _d3d12CommandAllocator;
    private readonly ID3D12GraphicsCommandList* _d3d12GraphicsCommandList;
    private readonly D3D12GraphicsFence _fence;

    internal D3D12GraphicsComputeContext(D3D12GraphicsDevice device)
        : base(device)
    {
        var d3d12CommandAllocator = CreateD3D12CommandAllocator(device);
        _d3d12CommandAllocator = d3d12CommandAllocator;

        _d3d12GraphicsCommandList = CreateD3D12GraphicsCommandList(device, d3d12CommandAllocator);
        _fence = device.CreateFence(isSignalled: true);

        static ID3D12CommandAllocator* CreateD3D12CommandAllocator(D3D12GraphicsDevice device)
        {
            ID3D12CommandAllocator* d3d12CommandAllocator;
            ThrowExternalExceptionIfFailed(device.D3D12Device->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_COMPUTE, __uuidof<ID3D12CommandAllocator>(), (void**)&d3d12CommandAllocator));
            return d3d12CommandAllocator;
        }

        static ID3D12GraphicsCommandList* CreateD3D12GraphicsCommandList(D3D12GraphicsDevice device, ID3D12CommandAllocator* d3d12CommandAllocator)
        {
            ID3D12GraphicsCommandList* d3d12GraphicsCommandList;
            ThrowExternalExceptionIfFailed(device.D3D12Device->CreateCommandList(nodeMask: 0, D3D12_COMMAND_LIST_TYPE_COMPUTE, d3d12CommandAllocator, pInitialState: null, __uuidof<ID3D12GraphicsCommandList>(), (void**)&d3d12GraphicsCommandList));

            // Command lists are created in the recording state, but there is nothing
            // to record yet. The main loop expects it to be closed, so close it now.
            ThrowExternalExceptionIfFailed(d3d12GraphicsCommandList->Close());

            return d3d12GraphicsCommandList;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsComputeContext" /> class.</summary>
    ~D3D12GraphicsComputeContext() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="ID3D12CommandAllocator" /> used by the context.</summary>
    public ID3D12CommandAllocator* D3D12CommandAllocator
    {
        get
        {
            AssertNotDisposed();
            return _d3d12CommandAllocator;
        }
    }

    /// <summary>Gets the <see cref="ID3D12GraphicsCommandList" /> used by the context.</summary>
    public ID3D12GraphicsCommandList* D3D12GraphicsCommandList
    {
        get
        {
            AssertNotDisposed();
            return _d3d12GraphicsCommandList;
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc />
    public override D3D12GraphicsFence Fence => _fence;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    public override void Flush()
    {
        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;

        var d3d12CommandQueue = Device.D3D12ComputeCommandQueue;
        ThrowExternalExceptionIfFailed(d3d12GraphicsCommandList->Close());
        d3d12CommandQueue->ExecuteCommandLists(1, (ID3D12CommandList**)&d3d12GraphicsCommandList);

        var fence = Fence;
        ThrowExternalExceptionIfFailed(d3d12CommandQueue->Signal(fence.D3D12Fence, fence.D3D12FenceSignalValue));
        fence.Wait();
    }

    /// <inheritdoc />
    public override void Reset()
    {
        Fence.Reset();

        var d3d12CommandAllocator = D3D12CommandAllocator;

        ThrowExternalExceptionIfFailed(d3d12CommandAllocator->Reset());
        ThrowExternalExceptionIfFailed(D3D12GraphicsCommandList->Reset(d3d12CommandAllocator, pInitialState: null));
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        ReleaseIfNotNull(_d3d12GraphicsCommandList);
        ReleaseIfNotNull(_d3d12CommandAllocator);

        if (isDisposing)
        {
            _fence?.Dispose();
        }
    }

    /// <inheritdoc />
    protected override void SetNameInternal(string value)
    {
        D3D12CommandAllocator->SetD3D12Name(value);
        D3D12GraphicsCommandList->SetD3D12Name(value);
    }
}
