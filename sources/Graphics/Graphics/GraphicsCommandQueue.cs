// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_QUEUE_FLAGS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>Represents a queue of graphics commands.</summary>
public abstract unsafe class GraphicsCommandQueue : GraphicsDeviceObject
{
    private ComPtr<ID3D12CommandQueue> _d3d12CommandQueue;
    private readonly uint _d3d12CommandQueueVersion;

    private readonly GraphicsContextKind _kind;

    private GraphicsFence _waitForIdleFence;

    private protected GraphicsCommandQueue(GraphicsDevice device, GraphicsContextKind kind) : base(device)
    {
        var d3d12CommandListType = kind.AsD3D12CommandListType();

        var d3d12CommandQueue = CreateD3D12CommandQueue(d3d12CommandListType, out _d3d12CommandQueueVersion);
        _d3d12CommandQueue.Attach(d3d12CommandQueue);

        _kind = kind;

        _waitForIdleFence = device.CreateFence(isSignaled: false);

        SetNameUnsafe(Name);

        ID3D12CommandQueue* CreateD3D12CommandQueue(D3D12_COMMAND_LIST_TYPE d3d12CommandListType, out uint d3d12CommandQueueVersion)
        {
            ID3D12CommandQueue* d3d12CommandQueue;

            var d3d12CommandQueueDesc = new D3D12_COMMAND_QUEUE_DESC {
                Type = d3d12CommandListType,
                Priority = 0,
                Flags = D3D12_COMMAND_QUEUE_FLAG_NONE,
                NodeMask = 0,
            };
            ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateCommandQueue(&d3d12CommandQueueDesc, __uuidof<ID3D12CommandQueue>(), (void**)&d3d12CommandQueue));

            return GetLatestD3D12CommandQueue(d3d12CommandQueue, out d3d12CommandQueueVersion);
        }
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsCommandQueue" /> class.</summary>
    ~GraphicsCommandQueue() => Dispose(isDisposing: false);

    /// <summary>Gets the kind of contexts in the command queue.</summary>
    public GraphicsContextKind Kind => _kind;

    internal ID3D12CommandQueue* D3D12CommandQueue => _d3d12CommandQueue;

    internal uint D3D12CommandQueueVersion => _d3d12CommandQueueVersion;

    /// <summary>Executes a graphics context.</summary>
    /// <param name="context">The graphics context to execute.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The context kind for <paramref name="context" /> is not the same as <see cref="Kind" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="context" /> was not created for the same device as the command queue.</exception>
    /// <exception cref="ObjectDisposedException">The command queue has been disposed.</exception>
    public void ExecuteContext(GraphicsContext context)
    {
        ThrowIfDisposed();
        ThrowIfNull(context);

        if (context.Kind != Kind)
        {
            ThrowForInvalidKind(context.Kind);
        }

        if (context.Device != Device)
        {
            ThrowForInvalidParent(context.Device);
        }

        ExecuteContextUnsafe(context);
    }

    /// <summary>Rents a graphics context from the command queue, creating a new context if none are available.</summary>
    /// <returns>A rented graphics context.</returns>
    /// <exception cref="ObjectDisposedException">The command queue has been disposed.</exception>
    public GraphicsContext RentContext()
    {
        ThrowIfDisposed();
        return RentContextUnsafe();
    }

    /// <summary>Returns a graphics context to the command queue.</summary>
    /// <param name="context">The graphics context that should be returned to the command queue.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The context kind for <paramref name="context" /> is not the same as <see cref="Kind" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="context" /> was not created for the same device as the command queue.</exception>
    /// <exception cref="ObjectDisposedException">The command queue has been disposed.</exception>
    public void ReturnContext(GraphicsContext context)
    {
        ThrowIfDisposed();
        ThrowIfNull(context);

        if (context.Kind != Kind)
        {
            ThrowForInvalidKind(context.Kind);
        }

        if (context.Device != Device)
        {
            ThrowForInvalidParent(context.Device);
        }

        ReturnContextUnsafe(context);
    }

    /// <summary>Enqueues a GPU side signal for a graphics fence.</summary>
    /// <param name="fence">The graphics fence to signal.</param>
    /// <exception cref="ArgumentNullException"><paramref name="fence" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="fence" /> was not created for the same device as the command queue.</exception>
    /// <exception cref="ObjectDisposedException">The command queue has been disposed.</exception>
    public void SignalFence(GraphicsFence fence)
    {
        ThrowIfDisposed();
        ThrowIfNull(fence);

        if (fence.Device != Device)
        {
            ThrowForInvalidParent(fence.Device);
        }

        SignalFenceUnsafe(fence);
    }

    /// <summary>Enqueues a GPU side wait for a graphics fence.</summary>
    /// <param name="fence">The graphics fence for which to wait.</param>
    /// <exception cref="ArgumentNullException"><paramref name="fence" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="fence" /> was not created for the same device as the command queue.</exception>
    /// <exception cref="ObjectDisposedException">The command queue has been disposed.</exception>
    public void WaitForFence(GraphicsFence fence)
    {
        ThrowIfDisposed();
        ThrowIfNull(fence);

        if (fence.Device != Device)
        {
            ThrowForInvalidParent(fence.Device);
        }

        WaitForFenceUnsafe(fence);
    }

    /// <summary>Waits for the command queue to become idle.</summary>
    /// <exception cref="ObjectDisposedException">The command queue has been disposed.</exception>
    public void WaitForIdle()
    {
        ThrowIfDisposed();
        WaitForIdleUnsafe();
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _waitForIdleFence.Dispose();
            _waitForIdleFence = null!;
        }
        _ = _d3d12CommandQueue.Reset();
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12CommandQueue->SetD3D12Name(value);
    }

    internal void ExecuteContextUnsafe(GraphicsContext context)
    {
        var d3d12GraphicsCommandList = context.D3D12GraphicsCommandList;
        D3D12CommandQueue->ExecuteCommandLists(NumCommandLists: 1, (ID3D12CommandList**)&d3d12GraphicsCommandList);

        var fence = context.Fence;
        SignalFenceUnsafe(fence);
        fence.Wait();
    }

    internal abstract bool RemoveContext(GraphicsContext context);

    private protected abstract GraphicsContext RentContextUnsafe();

    private protected abstract void ReturnContextUnsafe(GraphicsContext context);

    private void SignalFenceUnsafe(GraphicsFence fence)
    {
        ThrowExternalExceptionIfFailed(D3D12CommandQueue->Signal(fence.D3D12Fence, Value: 1));
    }

    private void WaitForFenceUnsafe(GraphicsFence fence)
    {
        ThrowExternalExceptionIfFailed(D3D12CommandQueue->Wait(fence.D3D12Fence, Value: 1));
    }

    private void WaitForIdleUnsafe()
    {
        WaitForFenceUnsafe(_waitForIdleFence);
    }
}
