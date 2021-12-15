// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12_FENCE_FLAGS;
using static TerraFX.Interop.Windows.WAIT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsFence : GraphicsFence
{
    private readonly ID3D12Fence* _d3d12Fence;
    private readonly HANDLE _d3d12FenceSignalEvent;

    private ulong _d3d12FenceSignalValue;
    private string _name = null!;

    private VolatileState _state;

    internal D3D12GraphicsFence(D3D12GraphicsDevice device, bool isSignalled)
        : base(device)
    {
        _d3d12Fence = CreateD3D12Fence(device, isSignalled);
        _d3d12FenceSignalEvent = CreateEventHandle();

        _ = _state.Transition(to: Initialized);
        Name = nameof(D3D12GraphicsDevice);

        static ID3D12Fence* CreateD3D12Fence(D3D12GraphicsDevice device, bool isSignalled)
        {
            ID3D12Fence* d3d12Fence;

            var initialValue = isSignalled ? 0UL : 1UL;
            ThrowExternalExceptionIfFailed(device.D3D12Device->CreateFence(initialValue, D3D12_FENCE_FLAG_NONE, __uuidof<ID3D12Fence>(), (void**)&d3d12Fence));

            return d3d12Fence;
        }

        static HANDLE CreateEventHandle()
        {
            HANDLE eventHandle;
            ThrowForLastErrorIfZero(eventHandle = CreateEventW(lpEventAttributes: null, bManualReset: FALSE, bInitialState: FALSE, lpName: null));
            return eventHandle;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsFence" /> class.</summary>
    ~D3D12GraphicsFence() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the underlying <see cref="ID3D12Fence" /> for the fence.</summary>
    public ID3D12Fence* D3D12Fence
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12Fence;
        }
    }

    /// <summary>Gets a <see cref="HANDLE" /> to an event which is raised when the fence enters the signalled state.</summary>
    public HANDLE D3D12FenceSignalEvent
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12FenceSignalEvent;
        }
    }

    /// <summary>Gets the value at which the fence will enter the signalled state.</summary>
    public ulong D3D12FenceSignalValue => _d3d12FenceSignalValue;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets or sets the name for the fence.</summary>
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = D3D12Fence->UpdateD3D12Name(value);
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    public override bool IsSignalled => D3D12Fence->GetCompletedValue() >= D3D12FenceSignalValue;

    /// <inheritdoc />
    public override void Reset()
    {
        if (IsSignalled)
        {
            _d3d12FenceSignalValue = D3D12Fence->GetCompletedValue() + 1;
        }
    }

    /// <inheritdoc />
    public override bool TryWait(int millisecondsTimeout = -1)
    {
        Assert(AssertionsEnabled && (millisecondsTimeout >= Timeout.Infinite));
        return TryWait(unchecked((uint)millisecondsTimeout));
    }

    /// <inheritdoc />
    public override bool TryWait(TimeSpan timeout)
    {
        var remainingMilliseconds = (long)timeout.TotalMilliseconds;
        Assert(AssertionsEnabled && (remainingMilliseconds >= Timeout.Infinite));

        var isSignalled = false;

        while (remainingMilliseconds > INFINITE)
        {
            const uint MillisecondsTimeout = INFINITE - 1;

            if (TryWait(MillisecondsTimeout))
            {
                isSignalled = true;
                break;
            }

            remainingMilliseconds -= MillisecondsTimeout;
        }

        return isSignalled || TryWait(unchecked((uint)remainingMilliseconds));
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            ReleaseIfNotNull(_d3d12Fence);
            DisposeEventHandle(_d3d12FenceSignalEvent);
        }

        _state.EndDispose();

        static void DisposeEventHandle(HANDLE eventHandle)
        {
            if (eventHandle != HANDLE.NULL)
            {
                _ = CloseHandle(eventHandle);
            }
        }
    }

    private bool TryWait(uint millisecondsTimeout)
    {
        var isSignalled = IsSignalled;

        var d3d12Fence = D3D12Fence;
        var d3d12FenceSignalEvent = D3D12FenceSignalEvent;

        if (!isSignalled)
        {
            ThrowExternalExceptionIfFailed(D3D12Fence->SetEventOnCompletion(D3D12FenceSignalValue, d3d12FenceSignalEvent));

            var result = WaitForSingleObject(d3d12FenceSignalEvent, millisecondsTimeout);

            if (result == WAIT_OBJECT_0)
            {
                isSignalled = true;
            }
            else if (result != WAIT_TIMEOUT)
            {
                ThrowForLastError(nameof(WaitForSingleObject));
            }
        }

        return isSignalled;
    }
}
