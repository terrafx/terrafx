// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_LIST_FLAGS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;

namespace TerraFX.Graphics;

/// <summary>Represents a graphics context, which can be used for executing commands.</summary>
public abstract unsafe class GraphicsContext : GraphicsCommandQueueObject
{
    private ComPtr<ID3D12CommandAllocator> _d3d12CommandAllocator;
    private readonly uint _d3d12CommandAllocatorVersion;

    private ComPtr<ID3D12GraphicsCommandList> _d3d12GraphicsCommandList;
    private readonly uint _d3d12GraphicsCommandListVersion;

    private GraphicsFence _fence;

    private readonly GraphicsContextKind _kind;

    private protected GraphicsContext(GraphicsCommandQueue commandQueue, GraphicsContextKind kind) : base(commandQueue)
    {
        // No need for a CommandQueue.AddContext(this) as it will be done by the underlying pool

        var d3d12CommandListType = kind.AsD3D12CommandListType();

        var d3d12CommandAllocator = CreateD3D12CommandAllocator(d3d12CommandListType, out _d3d12CommandAllocatorVersion);
        _d3d12CommandAllocator.Attach(d3d12CommandAllocator);

        var d3d12GraphicsCommandList = CreateD3D12GraphicsCommandList(d3d12CommandListType, out _d3d12GraphicsCommandListVersion);
        _d3d12GraphicsCommandList.Attach(d3d12GraphicsCommandList);

        _kind = kind;
        _fence = Device.CreateFence(isSignalled: true);

        SetNameUnsafe(Name);

        ID3D12CommandAllocator* CreateD3D12CommandAllocator(D3D12_COMMAND_LIST_TYPE d3d12CommandListType, out uint d3d12CommandAllocatorVersion)
        {
            ID3D12CommandAllocator* d3d12CommandAllocator;
            ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateCommandAllocator(d3d12CommandListType, __uuidof<ID3D12CommandAllocator>(), (void**)&d3d12CommandAllocator));
            return GetLatestD3D12CommandAllocator(d3d12CommandAllocator, out d3d12CommandAllocatorVersion);
        }

        ID3D12GraphicsCommandList* CreateD3D12GraphicsCommandList(D3D12_COMMAND_LIST_TYPE d3d12CommandListType, out uint d3d12GraphicsCommandListVersion)
        {
            ID3D12GraphicsCommandList* d3d12GraphicsCommandList;

            if (Device.D3D12DeviceVersion >= 4)
            {
                var d3d12Device4 = (ID3D12Device4*)Device.D3D12Device;
                ThrowExternalExceptionIfFailed(d3d12Device4->CreateCommandList1(nodeMask: 0, d3d12CommandListType, D3D12_COMMAND_LIST_FLAG_NONE, __uuidof<ID3D12GraphicsCommandList>(), (void**)&d3d12GraphicsCommandList));
            }
            else
            {
                var d3d12Device = Device.D3D12Device;
                ThrowExternalExceptionIfFailed(d3d12Device->CreateCommandList(nodeMask: 0, d3d12CommandListType, _d3d12CommandAllocator, pInitialState: null, __uuidof<ID3D12GraphicsCommandList>(), (void**)&d3d12GraphicsCommandList));

                // Command lists are created in the recording state, but there is nothing
                // to record yet. The main loop expects it to be closed, so close it now.
                ThrowExternalExceptionIfFailed(d3d12GraphicsCommandList->Close());
            }

            return GetLatestD3D12GraphicsCommandList(d3d12GraphicsCommandList, out d3d12GraphicsCommandListVersion);
        }
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsContext" /> class.</summary>
    ~GraphicsContext() => Dispose(isDisposing: false);

    /// <summary>Gets the fence used by the context for synchronization.</summary>
    public GraphicsFence Fence => _fence;

    /// <summary>Gets the context kind.</summary>
    public GraphicsContextKind Kind => _kind;

    internal ID3D12CommandAllocator* D3D12CommandAllocator => _d3d12CommandAllocator;

    internal uint D3D12CommandAllocatorVersion => _d3d12CommandAllocatorVersion;

    internal ID3D12GraphicsCommandList* D3D12GraphicsCommandList => _d3d12GraphicsCommandList;

    internal uint D3D12GraphicsCommandListVersion => _d3d12GraphicsCommandListVersion;

    /// <summary>Closes the graphics context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public void Close()
    {
        ThrowIfDisposed();
        CloseUnsafe();
    }

    /// <summary>Executes the graphics context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public void Execute()
    {
        ThrowIfDisposed();
        ExecuteUnsafe();
    }

    /// <summary>Resets the graphics context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public void Reset()
    {
        ThrowIfDisposed();
        ResetUnsafe();
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            var fence = Fence;
            fence.Wait();
            fence.Reset();

            fence.Dispose();
            _fence = null!;
        }

        _ = _d3d12GraphicsCommandList.Reset();
        _ = _d3d12CommandAllocator.Reset();

        _ = CommandQueue.RemoveContext(this);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12CommandAllocator->SetD3D12Name(value);
        D3D12GraphicsCommandList->SetD3D12Name(value);
    }

    private void CloseUnsafe()
    {
        ThrowExternalExceptionIfFailed(D3D12GraphicsCommandList->Close());
    }

    private void ExecuteUnsafe()
    {
        CommandQueue.ExecuteContextUnsafe(this);
    }

    private void ResetUnsafe()
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        var d3d12CommandAllocator = D3D12CommandAllocator;
        ThrowExternalExceptionIfFailed(d3d12CommandAllocator->Reset());

        ThrowExternalExceptionIfFailed(D3D12GraphicsCommandList->Reset(d3d12CommandAllocator, pInitialState: null));
    }
}
