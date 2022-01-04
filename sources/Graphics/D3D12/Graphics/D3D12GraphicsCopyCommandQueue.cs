// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_LIST_TYPE;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_QUEUE_FLAGS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsCopyCommandQueue : GraphicsCopyCommandQueue
{
    private ComPtr<ID3D12CommandQueue> _d3d12CommandQueue;
    private readonly uint _d3d12CommandQueueVersion;

    private ValuePool<D3D12GraphicsCopyContext> _copyContexts;
    private readonly ValueMutex _copyContextsMutex;

    private D3D12GraphicsFence _waitForIdleFence;

    /// <inheritdoc />
    public D3D12GraphicsCopyCommandQueue(D3D12GraphicsDevice device) : base(device)
    {
        _d3d12CommandQueue = CreateD3D12CommandQueue(out _d3d12CommandQueueVersion);

        _copyContexts = new ValuePool<D3D12GraphicsCopyContext>();
        _copyContextsMutex = new ValueMutex();

        _waitForIdleFence = device.CreateFence(isSignalled: false).As<D3D12GraphicsFence>();

        SetNameUnsafe(Name);

        ID3D12CommandQueue* CreateD3D12CommandQueue(out uint d3d12CommandQueueVersion)
        {
            ID3D12CommandQueue* d3d12CommandQueue;

            var commandQueueDesc = new D3D12_COMMAND_QUEUE_DESC {
                Type = D3D12_COMMAND_LIST_TYPE_COPY,
                Priority = 0,
                Flags = D3D12_COMMAND_QUEUE_FLAG_NONE,
                NodeMask = 0,
            };
            ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateCommandQueue(&commandQueueDesc, __uuidof<ID3D12CommandQueue>(), (void**)&d3d12CommandQueue));

            return GetLatestD3D12CommandQueue(d3d12CommandQueue, out d3d12CommandQueueVersion);
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsCopyCommandQueue" /> class.</summary>
    ~D3D12GraphicsCopyCommandQueue() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the underlying <see cref="ID3D12CommandQueue" /> for the context pool.</summary>
    public ID3D12CommandQueue* D3D12CommandQueue => _d3d12CommandQueue;

    /// <summary>Gets the interface version of <see cref="D3D12CommandQueue" />.</summary>
    public uint D3D12CommandQueueVersion => _d3d12CommandQueueVersion;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    private static D3D12GraphicsCopyContext CreateCopyContext(D3D12GraphicsCopyCommandQueue copyCommandQueue)
    {
        return new D3D12GraphicsCopyContext(copyCommandQueue);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _copyContexts.Dispose();

            _waitForIdleFence.Dispose();
            _waitForIdleFence = null!;
        }
        _copyContextsMutex.Dispose();

        _ = _d3d12CommandQueue.Reset();
    }

    /// <inheritdoc />
    protected override void ExecuteContextUnsafe(GraphicsCopyContext context)
    { 
        ExecuteContextUnsafe((D3D12GraphicsCopyContext)context);
    }

    /// <inheritdoc />
    protected override D3D12GraphicsCopyContext RentContextUnsafe()
    {
        return _copyContexts.Rent(&CreateCopyContext, this, _copyContextsMutex);
    }

    /// <inheritdoc />
    protected override void ReturnContextUnsafe(GraphicsCopyContext context)
    {
        ReturnContextUnsafe((D3D12GraphicsCopyContext)context);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12CommandQueue->SetD3D12Name(value);
    }

    /// <inheritdoc />
    protected override void SignalFenceUnsafe(GraphicsFence fence)
    { 
        SignalFenceUnsafe((D3D12GraphicsFence)fence);
    }

    /// <inheritdoc />
    protected override void WaitForFenceUnsafe(GraphicsFence fence)
    { 
        WaitForFenceUnsafe((D3D12GraphicsFence)fence);
    }

    /// <inheritdoc />
    protected override void WaitForIdleUnsafe()
    {
        WaitForFenceUnsafe(_waitForIdleFence);
    }

    internal void ExecuteContextUnsafe(D3D12GraphicsCopyContext context)
    {
        var d3d12GraphicsCommandList = context.D3D12GraphicsCommandList;
        D3D12CommandQueue->ExecuteCommandLists(NumCommandLists: 1, (ID3D12CommandList**)&d3d12GraphicsCommandList);

        var fence = context.Fence;
        SignalFenceUnsafe(fence);
        fence.Wait();
    }

    internal bool RemoveCopyContext(D3D12GraphicsCopyContext copyContext)
    {
        return IsDisposed || _copyContexts.Remove(copyContext);
    }

    internal void ReturnContextUnsafe(D3D12GraphicsCopyContext copyContext)
    {
        _copyContexts.Return(copyContext, _copyContextsMutex);
    }

    internal void SignalFenceUnsafe(D3D12GraphicsFence fence)
    {
        ThrowExternalExceptionIfFailed(D3D12CommandQueue->Signal(fence.D3D12Fence, Value: 1));
    }

    internal void WaitForFenceUnsafe(D3D12GraphicsFence fence)
    {
        ThrowExternalExceptionIfFailed(D3D12CommandQueue->Wait(fence.D3D12Fence, Value: 1));
    }
}
