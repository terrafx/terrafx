// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_LIST_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_LIST_TYPE;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsCopyContext : GraphicsCopyContext
{
    private ComPtr<ID3D12CommandAllocator> _d3d12CommandAllocator;
    private readonly uint _d3d12CommandAllocatorVersion;

    private ComPtr<ID3D12GraphicsCommandList> _d3d12GraphicsCommandList;
    private readonly uint _d3d12GraphicsCommandListVersion;

    internal D3D12GraphicsCopyContext(D3D12GraphicsCopyCommandQueue copyCommandQueue) : base(copyCommandQueue)
    {
        // No need for a ContextPool.AddComputeContext(this) as it will be done by the underlying pool

        ContextInfo.Fence = Device.CreateFence(isSignalled: true);

        _d3d12CommandAllocator = CreateD3D12CommandAllocator(out _d3d12CommandAllocatorVersion);
        _d3d12GraphicsCommandList = CreateD3D12GraphicsCommandList(out _d3d12GraphicsCommandListVersion);

        ID3D12CommandAllocator* CreateD3D12CommandAllocator(out uint d3d12CommandAllocatorVersion)
        {
            ID3D12CommandAllocator* d3d12CommandAllocator;
            ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_COPY, __uuidof<ID3D12CommandAllocator>(), (void**)&d3d12CommandAllocator));
            return GetLatestD3D12CommandAllocator(d3d12CommandAllocator, out d3d12CommandAllocatorVersion);
        }

        ID3D12GraphicsCommandList* CreateD3D12GraphicsCommandList(out uint d3d12GraphicsCommandListVersion)
        {
            ID3D12GraphicsCommandList* d3d12GraphicsCommandList;

            if (Device.D3D12DeviceVersion >= 4)
            {
                var d3d12Device4 = (ID3D12Device4*)Device.D3D12Device;
                ThrowExternalExceptionIfFailed(d3d12Device4->CreateCommandList1(nodeMask: 0, D3D12_COMMAND_LIST_TYPE_COPY, D3D12_COMMAND_LIST_FLAG_NONE, __uuidof<ID3D12GraphicsCommandList>(), (void**)&d3d12GraphicsCommandList));
            }
            else
            {
                var d3d12Device = Device.D3D12Device;
                ThrowExternalExceptionIfFailed(d3d12Device->CreateCommandList(nodeMask: 0, D3D12_COMMAND_LIST_TYPE_COPY, _d3d12CommandAllocator, pInitialState: null, __uuidof<ID3D12GraphicsCommandList>(), (void**)&d3d12GraphicsCommandList));

                // Command lists are created in the recording state, but there is nothing
                // to record yet. The main loop expects it to be closed, so close it now.
                ThrowExternalExceptionIfFailed(d3d12GraphicsCommandList->Close());
            }

            return GetLatestD3D12GraphicsCommandList(d3d12GraphicsCommandList, out d3d12GraphicsCommandListVersion);
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsCopyContext" /> class.</summary>
    ~D3D12GraphicsCopyContext() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <inheritdoc cref="GraphicsCommandQueueObject{TGraphicsContext}.CommandQueue" />
    public new D3D12GraphicsCopyCommandQueue CommandQueue => base.CommandQueue.As<D3D12GraphicsCopyCommandQueue>();

    /// <summary>Gets the <see cref="ID3D12CommandAllocator" /> used by the copy context.</summary>
    public ID3D12CommandAllocator* D3D12CommandAllocator => _d3d12CommandAllocator;

    /// <summary>Gets the interface version of <see cref="D3D12CommandAllocator" />.</summary>
    public uint D3D12CommandAllocatorVersion => _d3d12CommandAllocatorVersion;

    /// <summary>Gets the <see cref="ID3D12GraphicsCommandList" /> used by the copy context.</summary>
    public ID3D12GraphicsCommandList* D3D12GraphicsCommandList => _d3d12GraphicsCommandList;

    /// <summary>Gets the interface version of <see cref="D3D12GraphicsCommandList" />.</summary>
    public uint D3D12GraphicsCommandListVersion => _d3d12GraphicsCommandListVersion;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsContext{TGraphicsContext}.Fence" />
    public new D3D12GraphicsFence Fence => base.Fence.As<D3D12GraphicsFence>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void CloseUnsafe()
    {
        ThrowExternalExceptionIfFailed(D3D12GraphicsCommandList->Close());
    }

    /// <inheritdoc />
    protected override void CopyUnsafe(GraphicsBufferView destination, GraphicsBufferView source)
        => CopyUnsafe((D3D12GraphicsBufferView)destination, (D3D12GraphicsBufferView)source);

    /// <inheritdoc />
    protected override void CopyUnsafe(GraphicsTextureView destination, GraphicsBufferView source)
        => CopyUnsafe((D3D12GraphicsTextureView)destination, (D3D12GraphicsBufferView)source);

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            var fence = ContextInfo.Fence;
            fence.Wait();
            fence.Reset();

            fence.Dispose();
            ContextInfo.Fence = null!;
        }

        _ = _d3d12GraphicsCommandList.Reset();
        _ = _d3d12CommandAllocator.Reset();

        _ = CommandQueue.RemoveCopyContext(this);
    }

    /// <inheritdoc />
    protected override void ExecuteUnsafe()
    {
        CommandQueue.ExecuteContextUnsafe(this);
    }

    /// <inheritdoc />
    protected override void ResetUnsafe()
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        var d3d12CommandAllocator = D3D12CommandAllocator;
        ThrowExternalExceptionIfFailed(d3d12CommandAllocator->Reset());

        ThrowExternalExceptionIfFailed(D3D12GraphicsCommandList->Reset(d3d12CommandAllocator, pInitialState: null));
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12CommandAllocator->SetD3D12Name(value);
        D3D12GraphicsCommandList->SetD3D12Name(value);
    }

    private void CopyUnsafe(D3D12GraphicsBufferView destination, D3D12GraphicsBufferView source)
    {
        D3D12GraphicsCommandList->CopyBufferRegion(destination.Resource.D3D12Resource, destination.ByteOffset, source.Resource.D3D12Resource, source.ByteOffset, source.ByteLength);
    }

    private void CopyUnsafe(D3D12GraphicsTextureView destination, D3D12GraphicsBufferView source)
    {
        var d3d12DestinationTextureCopyLocation = new D3D12_TEXTURE_COPY_LOCATION(destination.Resource.D3D12Resource, Sub: destination.D3D12SubresourceIndex);

        var d3d12SourceTextureCopyLocation = new D3D12_TEXTURE_COPY_LOCATION(source.Resource.D3D12Resource, in destination.D3D12PlacedSubresourceFootprints[0]);
        d3d12SourceTextureCopyLocation.PlacedFootprint.Offset = source.ByteOffset;

        D3D12GraphicsCommandList->CopyTextureRegion(&d3d12DestinationTextureCopyLocation, DstX: 0, DstY: 0, DstZ: 0, &d3d12SourceTextureCopyLocation, pSrcBox: null);
    }
}
